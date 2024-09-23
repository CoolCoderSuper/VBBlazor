Imports Microsoft.AspNetCore.Components
Imports VBBlazor.Runtime.Controls

Public Class CascadeTest
    Inherits Control
    
    Public Sub New()
        DataContext = Me
    End Sub
    
    <CascadingParameter>
    Public Property Name As String

    Public Overrides Function GetContent() As XElement
        Return <div xmlns:local="VBBlazor.Runtime.Controls">
                   <local:Label Text="@Name"/>
               </div>
    End Function
End Class