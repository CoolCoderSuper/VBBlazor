Namespace Controls
    Public Interface IRenderable
        ReadOnly Property DataContext As Object
        Function GetContent() As XElement
    End Interface
End Namespace