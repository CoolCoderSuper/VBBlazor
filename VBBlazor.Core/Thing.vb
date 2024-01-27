Imports Microsoft.AspNetCore.Components
Imports VBBlazor.Core.Controls

Public Class Thing
    Inherits Control
    Public Sub New()
        DataContext = Me
    End Sub

    <Parameter>
    Public Property Name As String
    <Parameter>
    Public Property ChildContent As RenderFragment

    Public Overrides Function GetContent() As XElement
        Return <div xmlns:local="VBBlazor.Core.Controls">
                   <local:Label Text="@Name"/>
                   <br/>
                   <local:Label ChildContent="@ChildContent"/>
               </div>
    End Function
End Class