Imports Microsoft.AspNetCore.Components
Imports Microsoft.AspNetCore.Components.Rendering
Imports VBBlazor.Runtime.Controls

Public Class TableTemplate(Of TItem)
    Inherits Control

    Public Sub New()
        DataContext = Me
    End Sub

    <Parameter>
    Public Property TableHeader As RenderFragment
    <Parameter>
    Public Property RowTemplate As RenderFragment(Of TItem)
    <Parameter>
    Public Property Items As IEnumerable(Of TItem)

    Public ReadOnly Property ActualRowTemplate As RenderFragment(Of TItem)
        Get
            Return Function(item As TItem)
                       Return Sub(builder As RenderTreeBuilder)
                                  builder.OpenElement(0, "tr")
                                  If RowTemplate IsNot Nothing Then
                                      builder.AddContent(1, RowTemplate(item))
                                  End If
                                  builder.CloseElement()
                              End Sub
                   End Function
        End Get
    End Property

    Public Overrides Function GetContent() As XElement
        Return <table class="table" xmlns:local="VBBlazor.Runtime.Controls">
                   <thead>
                       <tr>
                           <local:Fragment ChildContent="@TableHeader"/>
                       </tr>
                   </thead>
                   <tbody>
                        <local:Loop TItem="TItem" Items="@Items" Template="@ActualRowTemplate"/>
                   </tbody>
               </table>
    End Function
End Class