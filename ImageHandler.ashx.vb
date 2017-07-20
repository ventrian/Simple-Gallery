'
' Simple Gallery for DotNetNuke -  http://www.dotnetnuke.com
' Copyright (c) 2002-2007
' by Ventrian ( sales@ventrian.com ) ( http://www.ventrian.com )
'

Imports System.Drawing
Imports System.Drawing.Drawing2d
Imports System.Drawing.Imaging
Imports System.Web
Imports System.Web.Services

Imports DotNetNuke.Common
Imports DotNetNuke.Common.Utilities
Imports System.IO

Imports Ventrian.ImageResizer

Namespace Ventrian.SimpleGallery

    Public Class ImageHandler
        Implements System.Web.IHttpHandler

#Region " Private Members "

        Private _width As Integer = 100
        Private _height As Integer = 100
        Private _homeDirectory As String = Null.NullString
        Private _fileName As String = Null.NullString
        Private _quality As Boolean = False
        Private _cropped As Boolean = False

#End Region

#Region " Private Methods "

        Private Sub ReadQueryString(ByVal context As HttpContext)

            If Not (context.Request("Width") Is Nothing) Then
                If (IsNumeric(context.Request("Width"))) Then
                    _width = Convert.ToInt32(context.Request("Width"))
                End If
            End If

            If Not (context.Request("Height") Is Nothing) Then
                If (IsNumeric(context.Request("Height"))) Then
                    _height = Convert.ToInt32(context.Request("Height"))
                End If
            End If

            If Not (context.Request("HomeDirectory") Is Nothing) Then
                _homeDirectory = context.Server.UrlDecode(context.Request("HomeDirectory"))
            End If

            If Not (context.Request("FileName") Is Nothing) Then
                _fileName = context.Request("FileName").Replace("+", "111222333444555")
                _fileName = context.Server.UrlDecode(_fileName)
                _fileName = _fileName.Replace("111222333444555", "+")
            End If

            If Not (context.Request("Q") Is Nothing) Then
                If (context.Request("Q") = "1") Then
                    _quality = True
                End If
            End If

            If Not (context.Request("S") Is Nothing) Then
                If (context.Request("S") = "1") Then
                    _cropped = True
                End If
            End If

        End Sub

#End Region

#Region " Properties "

        ReadOnly Property IsReusable() As Boolean Implements IHttpHandler.IsReusable
            Get
                Return False
            End Get
        End Property

#End Region

#Region " Event Handlers "

        Public Sub ProcessRequest(ByVal context As HttpContext) Implements IHttpHandler.ProcessRequest

            ' Set up the response settings
            context.Response.ContentType = "image/jpeg"

            ' Caching 
            context.Response.Cache.SetCacheability(HttpCacheability.Public)
            context.Response.Cache.SetExpires(DateTime.Now.AddDays(30))
            context.Response.Cache.VaryByParams("FileName") = True
            context.Response.Cache.VaryByParams("HomeDirectory") = True
            context.Response.Cache.VaryByParams("Width") = True
            context.Response.Cache.VaryByParams("Height") = True
            context.Response.Cache.AppendCacheExtension("max-age=86400")

            context.Items.Add("httpcompress.attemptedinstall", "true")

            ReadQueryString(context)

            If (_fileName <> "") Then

                Dim path As String = ""
                If _fileName = "placeholder-600.jpg" Then
                    path = "Images/placeholder-600.jpg"
                Else
                    path = _homeDirectory & "/" & _fileName
                End If

                Dim objQueryString As New NameValueCollection()

                For Each key As String In context.Request.QueryString.Keys
                    Dim values() As String = context.Request.QueryString.GetValues(key)
                    For Each value As String In values
                        objQueryString.Add(key.Replace("width", "maxwidth").Replace("height", "maxheight"), value)
                        If (key = "width" Or key = "height") Then
                            objQueryString.Add(key, value)
                        End If
                    Next
                Next

                If (_cropped) Then
                    objQueryString.Add("crop", "auto")
                End If

                Dim objImage As Bitmap = ImageManager.getBestInstance().BuildImage(context.Server.MapPath(path), objQueryString, New WatermarkSettings(objQueryString))
                If (path.ToLower().EndsWith("jpg")) Then
                    objImage.Save(context.Response.OutputStream, ImageFormat.Jpeg)
                Else
                    If (path.ToLower().EndsWith("gif")) Then
                        context.Response.ContentType = "image/gif"
                        Dim ios As ImageOutputSettings = New ImageOutputSettings(ImageOutputSettings.GetImageFormatFromPhysicalPath(context.Server.MapPath(path)), objQueryString)
                        ios.SaveImage(context.Response.OutputStream, objImage)
                    Else
                        If (path.ToLower().EndsWith("png")) Then
                            Dim objMemoryStream As New MemoryStream()
                            context.Response.ContentType = "image/png"
                            objImage.Save(objMemoryStream, ImageFormat.Png)
                            objMemoryStream.WriteTo(context.Response.OutputStream)
                        Else
                            objImage.Save(context.Response.OutputStream, ImageFormat.Jpeg)
                        End If
                    End If
                End If

            End If

        End Sub

        Public Shared Function GetEncoderInfo(ByVal mimeType As String) As ImageCodecInfo
            Dim codecs() As ImageCodecInfo = ImageCodecInfo.GetImageEncoders()

            Dim i As Integer
            For i = 0 To codecs.Length - 1 Step i + 1
                If codecs(i).MimeType = mimeType Then
                    Return codecs(i)
                End If
            Next

            Return Nothing
        End Function

#End Region

    End Class

End Namespace