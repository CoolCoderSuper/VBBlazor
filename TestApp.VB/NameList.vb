Imports VBBlazor.Runtime.Controls

Public Class NameList
    Inherits Control
    
    Public Sub New()
        DataContext = Me
    End Sub
    
    Public ReadOnly Property Names As List(Of String) = New List(Of String) From {"Bob", "Alice", "Charlie"}
    
    Public Property Name As String

    Public Overrides Function GetContent() As XElement
        Return <div xmlns:forms="Microsoft.AspNetCore.Components.Forms" xmlns:local="VBBlazor.Runtime.Controls">
                   <h1>Names</h1>
                   <ul>
                       <%= From name In names
                           Select <li><%= name %></li> %>
                   </ul>
                   <forms:InputText bind-Value="@Name"/>
                   <button class="btn btn-primary" event-onclick="@Add">Add</button>
               </div>
    End Function
    
    Public Sub Add()
        Names.Add(Name)
        Name = ""
    End Sub
End Class