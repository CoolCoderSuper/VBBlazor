Imports System.Linq.Expressions
Imports System.Reflection
Imports System.Xml
Imports Microsoft.AspNetCore.Components
Imports Microsoft.AspNetCore.Components.CompilerServices
Imports Microsoft.AspNetCore.Components.Rendering
Imports VBBlazor.Runtime.Controls
'TODO: Routing
'TODO: Cascading parameters
Public Class XmlRenderer
    Private ReadOnly _page As IRenderable
    Private ReadOnly _receiver As Object

    Public Sub New(page As IRenderable, receiver As Object)
        _page = page
        _receiver = receiver
    End Sub

    Public Function Render() As RenderFragment
        Return Render(_page.GetContent(), Nothing)
    End Function

    Private Function Render(element As XElement, context As Object) As RenderFragment
        Return Sub(builder)
                   Dim index As Integer = 0
                   Dim componentType As Type
                   Dim processedAttributes As New HashSet(Of String)
                   If element.Name.NamespaceName = "" Then
                       builder.OpenElement(index, element.Name.ToString())
                   Else
                       componentType = GetTypeByName($"{element.Name.NamespaceName}.{element.Name.LocalName}")
                       If componentType.ContainsGenericParameters Then
                           Dim parentType As Type = _page.GetType()
                           Dim actualArgs As Type() = parentType.GetGenericArguments()
                           Dim argumentNames As List(Of String) = If(parentType.IsGenericType, parentType.GetGenericTypeDefinition().GetGenericArguments().Select(Function(x) x.Name).ToList(), Nothing)
                           Dim types As New List(Of Type)
                           For Each typeParam As Type In componentType.GetGenericArguments()
                               Dim value = element.Attribute(typeParam.Name).Value
                               processedAttributes.Add(value)
                               Dim typeParamValue As Type
                               If parentType.IsGenericType AndAlso argumentNames.Contains(value) Then
                                   typeParamValue = actualArgs(argumentNames.IndexOf(value))
                               Else
                                   typeParamValue = GetTypeByName(value)
                               End If
                               types.Add(typeParamValue)
                           Next
                           componentType = componentType.MakeGenericType(types.ToArray())
                       End If
                       builder.OpenComponent(index, componentType)
                   End If
                   For Each attr As XAttribute In element.Attributes().Where(Function(x) x.Name.NamespaceName = "" AndAlso x.Name <> "ref" AndAlso Not processedAttributes.Contains(x.Value))
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
                               Dim propertyValue As Object = GetPropertyValue(valueName, context)
                               If componentType IsNot Nothing AndAlso componentType.GetProperties().Any(Function(x) x.Name = attr.Name.LocalName) Then
                                   propertyValue = CTypeDynamic(propertyValue, componentType.GetProperty(attr.Name.LocalName).PropertyType)
                               End If
                               builder.AddAttribute(index, attr.Name.LocalName, propertyValue)
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
                       If componentType Is Nothing Then
                           For Each el As XNode In element.Nodes()
                               index += 1
                               If el.NodeType = XmlNodeType.Text Then
                                   builder.AddContent(index, el.ToString())
                               Elseif el.NodeType = XmlNodeType.Element Then
                                   builder.AddContent(index, Render(el, context))
                               End If
                           Next
                       Else
                           Dim props As PropertyInfo() = componentType.GetProperties()
                           Dim elementNames As String() = element.Elements().Select(Function(x) x.Name.LocalName).ToArray()
                           If props.Any(Function(x) elementNames.Contains(x.Name)) Then
                               For Each el As XElement In element.Elements()
                                   Dim prop As PropertyInfo = props.FirstOrDefault(Function(x) x.Name = el.Name.LocalName)
                                   If prop IsNot Nothing Then
                                       index += 1
                                       If prop.PropertyType = GetType(RenderFragment) Then
                                           builder.AddAttribute(index, prop.Name, GetFragment(el, context))
                                       ElseIf prop.PropertyType.IsGenericType AndAlso prop.PropertyType.GetGenericTypeDefinition() = GetType(RenderFragment(Of )) Then
                                           Dim itemType As Type = prop.PropertyType.GetGenericArguments()(0)
                                           Dim fragmentMethod As MethodInfo = GetType(XmlRenderer).GetMethod("GetFragment", BindingFlags.NonPublic Or BindingFlags.Instance)
                                           Dim contextParam = Expression.Parameter(itemType)
                                           Dim bodyExpr = Expression.Call(Expression.Constant(Me), fragmentMethod, Expression.Constant(el), contextParam)
                                           Dim params = {contextParam}
                                           Dim lamdaMethod = GetType(Expression).GetMethods().First(Function(x) x.Name = "Lambda" AndAlso x.GetParameters().Length = 2).MakeGenericMethod(prop.PropertyType)
                                           Dim lambdaExpr = lamdaMethod.Invoke(Nothing, {bodyExpr, params})
                                           Dim compileMethod = GetType(LambdaExpression).GetMethods().First(Function(x) x.Name = "Compile" AndAlso x.GetParameters().Length = 0)
                                           Dim lamda = compileMethod.Invoke(lambdaExpr, Nothing)
                                           builder.AddAttribute(index, prop.Name, lamda)
                                       End If
                                   End If
                               Next
                           Else
                               index += 1
                               builder.AddAttribute(index, "ChildContent", GetFragment(element, context))
                           End If
                       End If
                   End If
                   If element.Name.NamespaceName = "" Then
                       builder.CloseElement()
                   Else
                       builder.CloseComponent()
                   End If
               End Sub
    End Function

    Private Function GetFragment(element As XElement, context As Object) As RenderFragment
        Return Sub(builder As RenderTreeBuilder)
                   Dim index As Integer = 0
                   For Each el As XNode In element.Nodes()
                       index += 1
                       If el.NodeType = XmlNodeType.Text Then
                           builder.AddContent(index, el.ToString())
                       ElseIf el.NodeType = XmlNodeType.Element Then
                           builder.AddContent(index, Render(el, context))
                       End If
                   Next
               End Sub
    End Function

    Private Function GetPropertyValue(name As String, context As Object) As Object
        If name = "Context" Then Return context
        Dim useContext As Boolean = name.StartsWith("Context.")
        If useContext Then
            name = name.Remove(0, 8)
        End If
        Dim obj As Object = If(useContext, context, _page.DataContext)
        Dim prop As PropertyInfo = obj.GetType().GetProperty(name)
        If prop IsNot Nothing Then
            Return prop.GetValue(obj)
        End If
        Return Nothing
    End Function

    Private Shared Function GetTypeByName(name As String) As Type
        For Each assembly In AppDomain.CurrentDomain.GetAssemblies().Reverse()
            Dim tt As Type = assembly.GetTypes().FirstOrDefault(Function(x) RemoveGenericText(x.FullName) = name)
            If tt IsNot Nothing Then
                Return tt
            End If
        Next
        Return Nothing
    End Function
    
    Private Shared Function RemoveGenericText(name As String) As String
        Return name.Split("`")(0)
    End Function
End Class