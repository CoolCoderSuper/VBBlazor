Imports Microsoft.AspNetCore.Components
Imports Microsoft.AspNetCore.Components.Forms
Imports Microsoft.JSInterop
Imports VBBlazor.Runtime.Controls

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
    Public Property Pets As New List(Of Pet) From {
        New Pet With {.PetId = 2, .Name = "Mr. Bigglesworth"},
        New Pet With {.PetId = 4, .Name = "Salem Saberhagen"},
        New Pet With {.PetId = 7, .Name = "K-9"}
    }
    Public Property OtherValue As String = "Bruh"
    Public Property InputDateControl As InputDate(Of Date)
    <Inject>
    Public Property Runtime As IJSRuntime

    Public Overrides Function GetContent() As XElement
        Return <div class="hello" xmlns:components="Microsoft.AspNetCore.Components" xmlns:web="Microsoft.AspNetCore.Components.Web" xmlns:forms="Microsoft.AspNetCore.Components.Forms" xmlns:local="TestApp.VB" xmlns:vb="VBBlazor.Runtime.Controls">
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
                   <local:ChildContentTest Name="@Title">
                       <b>Thing child</b>
                       <local:ChildContentTest Name="Thing that is child of thing"/>
                   </local:ChildContentTest>
                   <local:MultiContentTest>
                       <Header>
                           <b>Header</b>
                       </Header>
                       <Footer>
                           <b>Footer</b>
                       </Footer>
                   </local:MultiContentTest>
                   <local:Counter/>
                   <components:CascadingValue TValue="System.String" Value="@OtherValue">
                       <local:CascadeTest/>
                   </components:CascadingValue>
                   <local:NameList/>
                   <local:TableTemplate TItem="TestApp.VB.Pet" Items="@Pets">
                       <TableHeader>
                           <th>ID</th>
                           <th>Name</th>
                       </TableHeader>
                       <RowTemplate>
                           <td><vb:Label Text="@Context.PetId"/></td>
                           <td><vb:Label Text="@Context.Name"/></td>
                       </RowTemplate>
                   </local:TableTemplate>
               </div>
    End Function

    Public Sub Hello(e As EventArgs)
        Console.WriteLine(Time)
        Title = "Hello"
        Runtime.InvokeVoidAsync("alert", "Hello")
    End Sub
End Class

Public Class Pet
    Public Property PetId As Integer
    Public Property Name As String
End Class