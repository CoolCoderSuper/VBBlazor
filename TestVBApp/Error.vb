Imports Microsoft.AspNetCore.Components
Imports VBBlazor.Runtime.Controls

<Route("/Error")>
Public Class [Error]
    Inherits Control

    Public Overrides Function GetContent() As XElement
        Return <div xmlns:web="Microsoft.AspNetCore.Components.Web">
                   <web:PageTitle>Error</web:PageTitle>
                   <h1 class="text-danger">Error.</h1>
                   <h2 class="text-danger">An error occurred while processing your request.</h2>
               </div>
    End Function
End Class
