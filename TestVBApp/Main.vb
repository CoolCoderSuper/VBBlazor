Imports Microsoft.AspNetCore.Components
Imports VBBlazor.Runtime.Controls

<Route("/")>
Public Class Main
    Inherits Control

    Public Overrides Function GetContent() As XElement
        Return _
            <div xmlns:vb="VBBlazor.Runtime.Controls">
                <a href="/server">Server</a>
                <br/>
                <a href="/wasm">WASM</a>
            </div>
    End Function
End Class
