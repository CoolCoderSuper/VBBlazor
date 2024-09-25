Imports Microsoft.AspNetCore.Components
Imports Microsoft.AspNetCore.Components.Web

Public Class ServerRenderMode
    Inherits RenderModeAttribute
    Public Overrides ReadOnly Property Mode As IComponentRenderMode = RenderMode.InteractiveServer
End Class