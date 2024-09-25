Imports Microsoft.AspNetCore.Builder
Imports Microsoft.Extensions.DependencyInjection
Imports Microsoft.Extensions.Hosting
Imports TestApp.VB

Public Module Program
    Public Sub Main(args As String())
        Dim builder = WebApplication.CreateBuilder(args)
        builder.Services.AddRazorComponents().AddInteractiveServerComponents().AddInteractiveWebAssemblyComponents()
        Dim app = builder.Build()
        If app.Environment.IsDevelopment() Then
        Else
            app.UseExceptionHandler("/Error", createScopeForErrors:=True)
            app.UseHsts()
        End If
        'app.UseHttpsRedirection()
        app.UseStaticFiles()
        app.UseAntiforgery()
        app.MapRazorComponents(Of App).AddInteractiveServerRenderMode().AddInteractiveWebAssemblyRenderMode().AddAdditionalAssemblies(GetType(TestVBApp.Client.Program).Assembly)
        Dim t As Type = GetType(MainView)
        app.Run("http://localhost:4564")
    End Sub
End Module