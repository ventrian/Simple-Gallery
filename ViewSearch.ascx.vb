'
' Simple Gallery for DotNetNuke -  http://www.dotnetnuke.com
' Copyright (c) 2002-2007
' by Ventrian ( sales@ventrian.com ) ( http://www.ventrian.com )
'

Imports DotNetNuke.Common
Imports DotNetNuke.Common.Utilities
Imports DotNetNuke.Services.Exceptions
Imports DotNetNuke.Services.Localization

Imports Ventrian.SimpleGallery.Common
Imports Ventrian.SimpleGallery.Entities

Namespace Ventrian.SimpleGallery

    Partial Public Class ViewSearch
        Inherits SimpleGalleryBase

#Region " Private Members "

        Private _searchText As String = ""

#End Region

#Region " Private Methods "

        Private Sub BindBreadCrumbs()

            ucGalleryMenu.AddCrumb(Localization.GetString("AllAlbums", LocalResourceFile), NavigateURL())
            ucGalleryMenu.AddCrumb(Localization.GetString("Search", LocalResourceFile), NavigateURL(Me.TabId, "", "SearchID=" & TabModuleId))

            If (_searchText <> "") Then
                ucGalleryMenu.AddCrumb(_searchText, Request.RawUrl)
            End If

        End Sub

        Private Sub BindPhotos()

            If (_searchText.Trim() <> "") Then
                ucViewPhotos.SearchText = _searchText
            End If

            If (Request("Page") <> "") Then
                Try
                    ucViewPhotos.PageNumber = Convert.ToInt32(Request("Page"))
                Catch
                End Try
            End If

        End Sub

        Private Sub ReadQueryString()

            If (Request("SearchText") <> "") Then
                _searchText = Server.HtmlEncode(Server.UrlDecode(Request("SearchText")))
            End If

        End Sub

#End Region

#Region " Event Handlers "

        Private Sub Page_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load

            Try

                If (GallerySettings.EnableSearch = False) Then
                    Response.Redirect(NavigateURL(), True)
                End If

                ReadQueryString()
                BindBreadCrumbs()
                BindPhotos()

            Catch exc As Exception    'Module failed to load
                ProcessModuleLoadException(Me, exc)
            End Try

        End Sub

#End Region

    End Class

End Namespace