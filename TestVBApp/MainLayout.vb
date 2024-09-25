Imports VBBlazor.Runtime.Controls

Public Class MainLayout
    Inherits Layout

    Public Sub New()
        DataContext = Me
    End Sub

    Public Overrides Function GetContent() As XElement
        Return _
            <div xmlns:vb="VBBlazor.Runtime.Controls">
                <h1>Test VB App</h1>
                <vb:Fragment ChildContent="@Body"/>
            </div>
    End Function
End Class
