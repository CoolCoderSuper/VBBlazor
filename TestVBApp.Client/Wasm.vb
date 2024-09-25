Imports Microsoft.AspNetCore.Components
Imports VBBlazor.Runtime
Imports VBBlazor.Runtime.Controls

<Route("Wasm")>
<AutoRenderMode>
Public Class Wasm
    Inherits Control

    Public Overrides Function GetContent() As XElement
        Return _
            <div xmlns:local="TestApp.VB">
                <local:MainView Title="Bob"/>
            </div>
    End Function
End Class
