Imports Microsoft.AspNetCore.Components
Imports VBBlazor.Runtime.Controls

Public Class MultiContentTest
    Inherits Control
    
    Public Sub New()
        DataContext = Me
    End Sub

    <Parameter>
    Public Property Header As RenderFragment
    <Parameter>
    Public Property Footer As RenderFragment

    Public Overrides Function GetContent() As XElement
        Return <div xmlns:local="VBBlazor.Runtime.Controls">
                   <local:Fragment ChildContent="@Header"/>
                   <br/>
                   Nah bro
                   <br/>
                   <local:Fragment ChildContent="@Footer"/>
               </div>
    End Function
End Class