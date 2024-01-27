Imports Microsoft.AspNetCore.Components
Imports Microsoft.AspNetCore.Components.Rendering

Namespace Controls
    Public Class Layout
        Inherits LayoutComponentBase
        Implements IRenderable

        Public Property DataContext As Object Implements IRenderable.DataContext

        Public Overridable Function GetContent() As XElement Implements IRenderable.GetContent
            Return <div/>
        End Function

        Protected Overrides Sub BuildRenderTree(builder As RenderTreeBuilder)
            Dim renderer As New XmlRenderer(Me, Me)
            builder.AddContent(0, renderer.Render)
        End Sub
    End Class
End Namespace