Imports System.Reflection
Imports VBBlazor.Runtime.Controls

Public Class Routes
    Inherits Control

    Public Sub New()
        DataContext = Me
    End Sub

    Public Property AppAssembly As Assembly = GetType(Program).Assembly
    Public Property LayoutType As Type = GetType(MainLayout)

    Public Overrides Function GetContent() As XElement
        Return <div xmlns:route="Microsoft.AspNetCore.Components.Routing" xmlns:web="Microsoft.AspNetCore.Components">
                   <route:Router AppAssembly="@AppAssembly">
                       <route:Found>
                           <web:RouteView RouteData="@Context" DefaultLayout="@LayoutType"/>
                           <route:FocusOnNavigate RouteData="@Context" Selector="h1"/>
                       </route:Found>
                   </route:Router>
               </div>
    End Function
End Class
