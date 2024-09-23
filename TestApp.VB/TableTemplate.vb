Imports Microsoft.AspNetCore.Components
Imports VBBlazor.Runtime.Controls

Public Class TableTemplate(Of TItem)
    Inherits Control

    Public Sub New()
        DataContext = Me
    End Sub

    <Parameter>
    Public Property TableHeader As RenderFragment
    <Parameter>
    Public Property RowTemplate As RenderFragment(Of TItem)
    <Parameter>
    Public Property Items As IEnumerable(Of TItem)

    Public Overrides Function GetContent() As XElement
        Return <table class="table" xmlns:local="VBBlazor.Runtime.Controls">
                   <thead>
                       <tr>
                           <local:Fragment ChildContent="@TableHeader"/>
                       </tr>
                   </thead>
                   <tbody>
                       <local:Loop TItem="TItem" Items="@Items">
                           <Template>
                               <tr>
                                   <local:ValueFragment T="TItem" Value="@Context" ChildContent="@RowTemplate"/>
                               </tr>
                           </Template>
                       </local:Loop>
                   </tbody>
               </table>
    End Function
End Class