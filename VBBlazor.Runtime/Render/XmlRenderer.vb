Imports System.Linq.Expressions
Imports System.Reflection
Imports Microsoft.AspNetCore.Components
Imports Microsoft.AspNetCore.Components.CompilerServices
Imports Microsoft.AspNetCore.Components.Rendering
Imports VBBlazor.Runtime.Controls
'TODO: Improve type resolution
'TODO: Routing
'TODO: Cascading parameters
'TODO: Child content
'TODO: Prevent default
'TODO: @bind:event, @bind:after, @bind:format, @bind
'TODO: Templating
Public Class XmlRenderer
    Private ReadOnly _page As IRenderable
    Private ReadOnly _receiver As Object

    Public Sub New(page As IRenderable, receiver As Object)
        _page = page
        _receiver = receiver
    End Sub

    Public Function Render() As RenderFragment
        Return Render(_page.GetContent())
    End Function

    Private Function Render(element As XElement) As RenderFragment
        Return Sub(builder)
                   Dim index As Integer = 0
                   If element.Name.NamespaceName = "" Then
                       builder.OpenElement(index, element.Name.ToString())
                   Else
                       Dim componentType As Type = GetTypeByName($"{element.Name.NamespaceName}.{element.Name.LocalName}")
                       If componentType.ContainsGenericParameters Then
                           Dim types As New List(Of Type)
                           For Each typeParam As Type In componentType.GetGenericArguments()
                               Dim value = element.Attribute(typeParam.Name).Value
                               Dim typeParamValue As Type = GetTypeByName(value)
                               types.Add(typeParamValue)
                           Next
                           componentType = componentType.MakeGenericType(types.ToArray())
                       End If
                       builder.OpenComponent(index, componentType)
                   End If
                   For Each attr As XAttribute In element.Attributes().Where(Function(x) x.Name.NamespaceName = "" AndAlso x.Name <> "ref")
                       index += 1
                       If attr.Value.StartsWith("@") Then
                           Dim valueName As String = attr.Value.Remove(0, 1)
                           If attr.Name.LocalName.StartsWith("bind-") Then
                               Dim actualName As String = attr.Name.LocalName.Remove(0, 5)
                               Dim valueProp As PropertyInfo = _page.DataContext.GetType().GetProperty(valueName)
                               If valueProp IsNot Nothing Then
                                   Dim propValue As Object = valueProp.GetValue(_page.DataContext)
                                   builder.AddAttribute(index, actualName, propValue)
                                   index += 1
                                   Dim runtimeHelpersType As Type = GetType(RuntimeHelpers)
                                   Dim createInferredEventCallbackMethod As MethodInfo = runtimeHelpersType.GetMethods().First(Function(x) x.Name = "CreateInferredEventCallback")
                                   Dim createInferredEventCallbackGenericMethod As MethodInfo = createInferredEventCallbackMethod.MakeGenericMethod(valueProp.PropertyType)
                                   Dim param As ParameterExpression = Expression.Parameter(valueProp.PropertyType)
                                   Dim propSet As Expression = Expression.Call(Expression.Constant(_page.DataContext), valueProp.GetSetMethod(), param)
                                   Dim callback As [Delegate] = Expression.Lambda(propSet, param).Compile()
                                   Dim eventCallback As Object = createInferredEventCallbackGenericMethod.Invoke(Nothing, New Object() {_receiver, callback, valueProp.GetValue(_page.DataContext)})
                                   builder.AddAttribute(index, $"{actualName}Changed", eventCallback)
                                   index += 1
                                   Dim valueExpr As Expression = Expression.Property(Expression.Constant(_page.DataContext), valueProp)
                                   Dim expr As Expression = Expression.Lambda(valueExpr)
                                   builder.AddAttribute(index, $"{actualName}Expression", expr)
                               End If
                           ElseIf attr.Name.LocalName.StartsWith("event-") Then
                               Dim actualName As String = attr.Name.LocalName.Remove(0, 6)
                               Dim eventMember As MethodInfo = _page.DataContext.GetType().GetMethod(valueName)
                               If eventMember IsNot Nothing Then
                                   index += 1
                                   Dim parameters As ParameterExpression() = eventMember.GetParameters().Select(Function(x) Expression.Parameter(x.ParameterType)).ToArray()
                                   Dim eventExpression As Expression = Expression.Call(Expression.Constant(_page.DataContext), eventMember, parameters)
                                   builder.AddAttribute(index, actualName, New EventCallback(_receiver, Expression.Lambda(eventExpression, parameters).Compile))
                               End If
                           Else
                               builder.AddAttribute(index, attr.Name.LocalName, GetPropertyValue(valueName))
                           End If
                       Else
                           builder.AddAttribute(index, attr.Name.LocalName, attr.Value)
                       End If
                   Next
                   Dim refAttr As XAttribute = element.Attributes().FirstOrDefault(Function(x) x.Name.NamespaceName = "" AndAlso x.Name = "ref")
                   If refAttr IsNot Nothing Then
                       Dim refName As String = refAttr.Value.Remove(0, 1)
                       Dim refProp As PropertyInfo = _page.DataContext.GetType().GetProperty(refName)
                       If refProp IsNot Nothing Then
                           builder.AddComponentReferenceCapture(index, Sub(x) refProp.SetValue(_page.DataContext, x))
                       End If
                   End If
                   If Not element.IsEmpty Then
                       index += 1
                       If element.Name.NamespaceName = "" Then
                           builder.AddContent(index, If(element.Elements().Any(), "", element.Value))
                           For Each el As XElement In element.Elements()
                               index += 1
                               builder.AddContent(index, Render(el))
                           Next
                       Else
                           builder.AddAttribute(index, "ChildContent", CType(Sub(b As RenderTreeBuilder)
                                                                                 Dim index2 As Integer = 0
                                                                                 b.AddContent(index2, If(element.Elements().Any(), "", element.Value))
                                                                                 For Each el As XElement In element.Elements()
                                                                                     index2 += 1
                                                                                     b.AddContent(index2, Render(el))
                                                                                 Next
                                                                             End Sub, RenderFragment))
                       End If
                   End If
                   If element.Name.NamespaceName = "" Then
                       builder.CloseElement()
                   Else
                       builder.CloseComponent()
                   End If
               End Sub
    End Function

    Private Function GetPropertyValue(name As String) As Object
        Dim prop As PropertyInfo = _page.DataContext.GetType().GetProperty(name)
        If prop IsNot Nothing Then
            Return prop.GetValue(_page.DataContext)
        End If
        Return Nothing
    End Function

    Private Shared Function GetTypeByName(name As String) As Type
        For Each assembly In AppDomain.CurrentDomain.GetAssemblies().Reverse()
            Dim tt As Type = assembly.GetTypes().FirstOrDefault(Function(x) x.FullName.StartsWith(name))
            If tt IsNot Nothing Then
                Return tt
            End If
        Next
        Return Nothing
    End Function
End Class