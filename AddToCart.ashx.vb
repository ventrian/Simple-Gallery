Imports System.Web
Imports System.Web.Services

Imports DotNetNuke.Common.Utilities

Imports Ventrian.SimpleGallery.Entities

Namespace Ventrian.SimpleGallery

    Public Class AddToCart
        Implements System.Web.IHttpHandler

#Region " Private Members "

        Private _moduleID As Integer = Null.NullInteger
        Private _itemName As String = Null.NullString
        Private _itemID As String = Null.NullString

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

            If (context.Request("ItemName") <> "") Then
                _itemName = context.Request("ItemName")
            End If

            If (context.Request("ItemID") <> "") Then
                _itemID = context.Request("ItemID")
            End If

        End Sub

#End Region

#Region " Event Handlers "

        Sub ProcessRequest(ByVal context As HttpContext) Implements IHttpHandler.ProcessRequest

            context.Response.ContentType = "text/html"

            context.Response.Cache.SetCacheability(HttpCacheability.Public)
            context.Response.Cache.SetExpires(DateTime.Now.AddDays(30))
            context.Response.Cache.VaryByParams("mid") = True
            context.Response.Cache.VaryByParams("ItemName") = True
            context.Response.Cache.VaryByParams("ItemID") = True
            context.Response.Cache.AppendCacheExtension("max-age=86400")

            ReadQueryString(context)

            If (_moduleID <> Null.NullInteger) Then

                Dim objTemplateController As New TemplateController()
                Dim objTemplate As TemplateInfo = objTemplateController.Get(_moduleID, TemplateType.AddToCart.ToString())

                If (objTemplate IsNot Nothing) Then

                    Dim html As String = "<html><head></head><body>" & objTemplate.Template.Replace("[ITEMID]", _itemID.ToString()).Replace("[ITEMNAME]", _itemName) & "</body></html>"
                    context.Response.Write(html)

                End If

            End If

            ' context.Response.Write("<html><head></head><body><form target=""paypal"" action=""https://www.paypal.com/cgi-bin/webscr"" method=""post""><input type=""hidden"" name=""cmd"" value=""_s-xclick""><input type=""hidden"" name=""business"" value=""smcculloch@iinet.net.au""><input type=""hidden"" name=""display"" value=""1""><table><tr><td><input type=""hidden"" name=""on0"" value=""Sizes"">Sizes</td></tr><tr><td><select name=""os0""><option value=""5  4&quot;x6&quot; Print on Glossy Paper"">5  4&quot;x6&quot; Print on Glossy Paper $25.00</option><option value=""4  5&quot;x7&quot; Print on Glossy Paper"">4  5&quot;x7&quot; Print on Glossy Paper $25.00</option><option value=""2  8&quot;x10&quot; Print on Glossy"">2  8&quot;x10&quot; Print on Glossy $25.00</option><option value=""1  11&quot;x14&quot; Print on Glossy Paper"">1  11&quot;x14&quot; Print on Glossy Paper $25.00</option><option value=""1  11&quot;x14&quot; Glossy Paper on Black Foam Board"">1  11&quot;x14&quot; Glossy Paper on Black Foam Board $39.00</option></select> </td></tr></table><input type=""hidden"" name=""currency_code"" value=""USD""><input type=""image"" src=""https://www.paypalobjects.com/en_AU/i/btn/btn_cart_LG.gif"" border=""0"" name=""submit"" alt=""PayPal - The safer, easier way to pay online.""><img alt="""" border=""0"" src=""https://www.paypalobjects.com/en_AU/i/scr/pixel.gif"" width=""1"" height=""1""></form></body></html>")

        End Sub

#End Region


    End Class

End Namespace