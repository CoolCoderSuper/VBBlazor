﻿Imports VBBlazor.Runtime.Controls

Public Class Counter
    Inherits Control
    Public Sub New()
        DataContext = Me
    End Sub

    Private _currentCount As Integer = 0

    Public ReadOnly Property Label As String
        Get
            Return _currentCount
        End Get
    End Property

    Public Overrides Function GetContent() As XElement
        Return <div xmlns:local="VBBlazor.Runtime.Controls">
                   <h1>Counter</h1>
                   <p role="status">Current count: <local:Label Text="@Label"/></p>
                   <button class="btn btn-primary" event-onclick="@IncrementCount">Click me</button>
               </div>
    End Function

    Public Sub IncrementCount()
        _currentCount += 1
    End Sub
End Class