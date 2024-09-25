Imports VBBlazor.Runtime.Controls

Public Class App
    Inherits Control

    Public Overrides Function GetContent() As XElement
        Return <html xmlns:web="Microsoft.AspNetCore.Components.Web" xmlns:local="TestVBApp">
                   <head>
                       <meta charset="utf-8"/>
                       <meta name="viewport" content="width=device-width, initial-scale=1.0"/>
                       <base href="/"/>
                       <link rel="stylesheet" href="bootstrap/bootstrap.min.css"/>
                       <link rel="stylesheet" href="app.css"/>
                       <link rel="stylesheet" href="TestApp.styles.css"/>
                       <link rel="icon" type="image/png" href="favicon.png"/>
                       <web:HeadOutlet/>
                   </head>
                   <body>
                       <local:Routes/>
                       <script src="_framework/blazor.web.js"></script>
                   </body>
               </html>
    End Function
End Class