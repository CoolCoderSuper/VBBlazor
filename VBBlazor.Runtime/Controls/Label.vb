Imports Microsoft.AspNetCore.Components
Imports Microsoft.AspNetCore.Components.Rendering

Namespace Controls
    Public Class Label
        Inherits Control
        
        Public Sub New()
            DataContext = Me
        End Sub

        <Parameter>
        Public Property Text As String

        Protected Overrides Sub BuildRenderTree(builder As RenderTreeBuilder)
            builder.AddContent(0, Text)
        End Sub
    End Class
End Namespace