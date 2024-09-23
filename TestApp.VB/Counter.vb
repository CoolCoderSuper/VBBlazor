Imports VBBlazor.Runtime.Controls

Public Class Counter
    Inherits Control
    
    Public Sub New()
        DataContext = Me
    End Sub

    Public Property CurrentCount As Integer = 0

    Public Overrides Function GetContent() As XElement
        Return <div xmlns:local="VBBlazor.Runtime.Controls">
                   <h1>Counter</h1>
                   <p role="status">Current count: <local:Label Text="@CurrentCount"/></p>
                   <button class="btn btn-primary" event-onclick="@IncrementCount">Click me</button>
               </div>
    End Function

    Public Sub IncrementCount()
        CurrentCount += 1
    End Sub
End Class