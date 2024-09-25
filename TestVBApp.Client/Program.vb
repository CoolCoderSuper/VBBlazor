Imports Microsoft.AspNetCore.Components.WebAssembly.Hosting

Public Module Program
    Public Sub Main(args As String())
        Dim builder = WebAssemblyHostBuilder.CreateDefault(args)
        builder.Build().RunAsync()
    End Sub
End Module
