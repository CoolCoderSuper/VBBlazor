Imports Microsoft.AspNetCore.Components
Imports Microsoft.AspNetCore.Components.Forms
Imports Microsoft.JSInterop
Imports VBBlazor.Core.Controls

Public Class MainView
    Inherits Control
    Public Sub New()
        DataContext = Me
    End Sub

    <Parameter>
    Public Property Title As String = "Joe"
    Public Property Checked As Boolean = True
    Public Property Number As Decimal = 5
    Public Property Description As String = "Hello world"
    Public Property Time As Date = Date.Now
    Public Property InputDateControl As InputDate(Of Date)
    <Inject>
    Public Property Runtime As IJSRuntime

    Public Overrides Function GetContent() As XElement
        Dim names = {"Joe", "Bob", "Mary"}
        Return <div class="hello" xmlns:web="Microsoft.AspNetCore.Components.Web" xmlns:forms="Microsoft.AspNetCore.Components.Forms" xmlns:local="VBBlazor.Shared" xmlns:vblocal="VBBlazor.Core">
                   <web:PageTitle>Index from VB</web:PageTitle>
                   <web:HeadContent>
                       <meta name="description" content="@Description"/>
                   </web:HeadContent>
                   <b>
                       <i>Hello</i>
                       <br/>
                       <u>World</u>
                       <button class="btn btn-primary" event-onclick="@Hello">Hello</button>
                   </b>
                   <forms:InputText bind-Value="@Title"/>
                   <forms:InputCheckbox bind-Value="@Checked"/>
                   <forms:InputNumber TValue="System.Decimal" bind-Value="@Number"/>
                   <forms:InputDate ref="@InputDateControl" TValue="System.DateTime" bind-Value="@Time"/>
                   <h1>Bye</h1>
                   <ul>
                       <%= From name In names
                           Select <li><%= name %></li> %>
                   </ul>
                   <vblocal:Thing Name="@Title">
                       <b>Thing child</b>
                       <vblocal:Thing Name="Thing that is child of thing"/>
                   </vblocal:Thing>
                   <vblocal:Counter/>
               </div>
    End Function

    Public Sub Hello(e As EventArgs)
        Console.WriteLine(Time)
        Title = "Hello"
        Runtime.InvokeVoidAsync("alert", "Hello")
    End Sub
End Class