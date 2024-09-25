Imports Microsoft.AspNetCore.Components
Imports Microsoft.AspNetCore.Components.Web

Public Class AutoRenderMode
    Inherits RenderModeAttribute
    Public Overrides ReadOnly Property Mode As IComponentRenderMode = RenderMode.InteractiveAuto
End Class