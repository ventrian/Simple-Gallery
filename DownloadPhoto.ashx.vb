Imports System.Web
Imports System.Web.Services

Imports DotNetNuke.Common
Imports DotNetNuke.Common.Utilities
Imports DotNetNuke.Entities.Portals

Namespace Ventrian.SimpleGallery

    Public Class DownloadPhoto
        Implements System.Web.IHttpHandler

#Region " Private Members "

        Private _photoID As Integer = Null.NullInteger

#End Region

#Region " Private Methods "

        Private Sub ReadQueryString(ByVal context As HttpContext)

            If Not (context.Request("PhotoID") Is Nothing) Then
                If (IsNumeric(context.Request("PhotoID"))) Then
                    _photoID = Convert.ToInt32(context.Request("PhotoID"))
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

        Sub ProcessRequest(ByVal context As HttpContext) Implements IHttpHandler.ProcessRequest

            Try

                context.Items.Add("httpcompress.attemptedinstall", "true")

                ReadQueryString(context)

                If (_photoID <> Null.NullInteger) Then

                    Dim objPhotoController As New Ventrian.SimpleGallery.Entities.PhotoController
                    Dim objPhoto As Ventrian.SimpleGallery.Entities.PhotoInfo = objPhotoController.Get(_photoID)

                    If (objPhoto IsNot Nothing) Then


                        Dim filePath As String = context.Server.MapPath(PortalSettings.Current.HomeDirectory & objPhoto.HomeDirectory & "/" & objPhoto.FileName)
                        Dim fileInfo As System.IO.FileInfo = New System.IO.FileInfo(filePath)
                        context.Response.Clear()
                        context.Response.AddHeader("Content-Disposition", "attachment;filename=" + context.Server.UrlEncode(fileInfo.Name))
                        context.Response.AddHeader("Content-Length", fileInfo.Length.ToString())
                        context.Response.ContentType = "image/jpeg"
                        context.Response.WriteFile(fileInfo.FullName)
                        context.Response.Flush()
                        context.Response.End()

                    End If

                End If

            Catch ex As Exception
                Throw
            End Try

        End Sub

#End Region

    End Class

End Namespace