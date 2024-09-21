Imports Microsoft.AspNetCore.Components
Imports Microsoft.AspNetCore.Components.Rendering

Namespace Controls
    Public Class Fragment
        Inherits Control
        Public Sub New()
            DataContext = Me
        End Sub

        <Parameter>
        Public Property ChildContent As RenderFragment

        Protected Overrides Sub BuildRenderTree(builder As RenderTreeBuilder)
            builder.AddContent(0, ChildContent)
        End Sub
    End Class
    
    Public Class Fragment(Of T)
        Inherits Control
        Public Sub New()
            DataContext = Me
        End Sub

        <Parameter>
        Public Property ChildContent As RenderFragment(Of T)
        <Parameter>
        Public Property Value As T
        
        Protected Overrides Sub BuildRenderTree(builder As RenderTreeBuilder)
            builder.AddContent(0, ChildContent(Value))
        End Sub
    End Class
End NameSpace