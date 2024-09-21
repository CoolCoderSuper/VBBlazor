Imports Microsoft.AspNetCore.Components
Imports Microsoft.AspNetCore.Components.Rendering

Namespace Controls
    Public Class [Loop]
        Inherits Control

        Public Sub New()
            DataContext = Me
        End Sub

        <Parameter>
        Public Property Items As IEnumerable(Of Object)

        <Parameter>
        Public Property Template As RenderFragment(Of Object)

        Protected Overrides Sub BuildRenderTree(builder As RenderTreeBuilder)
            Dim index As Integer = 0
            For Each item In Items
                builder.AddContent(0, Template(item))
                index += 1
            Next
        End Sub
    End Class
End NameSpace