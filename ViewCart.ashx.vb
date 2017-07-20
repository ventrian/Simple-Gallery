Imports System.Web
Imports System.Web.Services

Imports DotNetNuke.Common.Utilities
Imports Ventrian.SimpleGallery.Entities

Namespace Ventrian.SimpleGallery

    Public Class ViewCart
        Implements System.Web.IHttpHandler

#Region " Private Members "

        Private _moduleID As Integer = Null.NullInteger

#End Region

#Region " Properties "

        ReadOnly Property IsReusable() As Boolean Implements IHttpHandler.IsReusable
            Get
                Return False
            End Get
        End Property

#End Region

#Region " Private Methods "

        Private Sub ReadQueryString(ByVal context As HttpContext)

            If (context.Request("mid") <> "") Then
                If (IsNumeric(context.Request("mid"))) Then
                    _moduleID = Convert.ToInt32(context.Request("mid"))
                End If
            End If

        End Sub

#End Region

#Region " Event Handlers "

        Sub ProcessRequest(ByVal context As HttpContext) Implements IHttpHandler.ProcessRequest

            context.Response.ContentType = "text/html"

            ReadQueryString(context)


            If (_moduleID <> Null.NullInteger) Then

                Dim objTemplateController As New TemplateController()
                Dim objTemplate As TemplateInfo = objTemplateController.Get(_moduleID, TemplateType.ViewCart.ToString())

                If (objTemplate IsNot Nothing) Then

                    Dim html As String = "<html><head></head><body>" & objTemplate.Template & "</body></html>"
                    context.Response.Write(html)

                End If

            End If

        End Sub

#End Region


    End Class

End Namespace