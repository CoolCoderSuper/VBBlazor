Imports Microsoft.AspNetCore.Components
Imports Microsoft.AspNetCore.Components.Rendering

Namespace Controls
    Public Class [Loop](Of TItem)
        Inherits Control

        Public Sub New()
            DataContext = Me
        End Sub

        <Parameter>
        Public Property Items As IEnumerable(Of TItem)

        <Parameter>
        Public Property Template As RenderFragment(Of TItem)

        Protected Overrides Sub BuildRenderTree(builder As RenderTreeBuilder)
            Dim index As Integer = 0
            For Each item In Items
                builder.AddContent(index, Template(item))
                index += 1
            Next
        End Sub
    End Class
End NameSpace