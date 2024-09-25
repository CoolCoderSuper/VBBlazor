Imports Microsoft.AspNetCore.Components
Imports Microsoft.AspNetCore.Components.Web

Public Class WasmRenderMode
    Inherits RenderModeAttribute
    Public Overrides ReadOnly Property Mode As IComponentRenderMode = RenderMode.InteractiveWebAssembly
End Class