'
' Simple Gallery for DotNetNuke -  http://www.dotnetnuke.com
' Copyright (c) 2002-2007
' by Ventrian ( sales@ventrian.com ) ( http://www.ventrian.com )
'

Imports DotNetNuke.Common
Imports DotNetNuke.Common.Utilities
Imports DotNetNuke.Services.Exceptions
Imports DotNetNuke.Services.Localization

Imports Ventrian.SimpleGallery.Entities

Namespace Ventrian.SimpleGallery

    Partial Public Class BulkEdit
        Inherits SimpleGalleryBase

#Region " Private Members "

        Private _albumID As Integer = Null.NullInteger
        Private _tagID As Integer = Null.NullInteger
        Private _returnUrl As String = Null.NullString

#End Region

#Region " Private Methods "

        Private Sub BindBreadCrumbs()

            ucGalleryMenu.AddCrumb(Localization.GetString("AllAlbums", LocalResourceFile), NavigateURL())

            Dim objAlbumController As New AlbumController
            Dim objAlbum As AlbumInfo = objAlbumController.Get(_albumID)

            While Not objAlbum Is Nothing
                If (GallerySettings.AlbumFilter <> objAlbum.AlbumID.ToString()) Then
                    ucGalleryMenu.InsertCrumb(1, objAlbum.Caption, NavigateURL(Me.TabId, "", "AlbumID=" & Me.ModuleId.ToString() & "-" & objAlbum.AlbumID.ToString()))
                End If

                If (objAlbum.ParentAlbumID = Null.NullInteger) Then
                    objAlbum = Nothing
                Else
                    objAlbum = objAlbumController.Get(objAlbum.ParentAlbumID)
                End If
            End While


            ucGalleryMenu.AddCrumb(Localization.GetString("EditPhotos", Me.LocalResourceFile), EditUrl("AlbumID", _albumID.ToString(), "BulkEdit"))

        End Sub

        Private Sub BindPhotos()

            ucEditPhotos.AlbumID = _albumID
            ucEditPhotos.TagID = _tagID
            ucEditPhotos.BindPhotos()

        End Sub

        Private Sub ReadQueryString()

            If Not (Request("AlbumID") Is Nothing) Then
                _albumID = Convert.ToInt32(Request("AlbumID"))
            End If

            If Not (Request("ReturnUrl") Is Nothing) Then
                _returnUrl = Request("ReturnUrl")
            End If

            If Not (Request("TagID") Is Nothing) Then
                _tagID = Convert.ToInt32(Request("TagID"))
            End If

        End Sub

        Private Sub SavePhotos()

            ucEditPhotos.Save()

            If (_returnUrl <> "") Then
                Response.Redirect(_returnUrl, True)
            Else
                Response.Redirect(NavigateURL(), True)
            End If

        End Sub

        Private Sub SecurityCheck()

            If (Me.HasEditPhotoPermissions) = False Then
                Response.Redirect(NavigateURL, True)
            End If

        End Sub

#End Region

#Region " Protected Methods "

#End Region

#Region " Event Handlers "

        Private Sub Page_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load

            Try

                SecurityCheck()
                ReadQueryString()
                BindBreadCrumbs()

                If (Page.IsPostBack = False) Then
                    BindPhotos()
                End If

            Catch exc As Exception    'Module failed to load
                ProcessModuleLoadException(Me, exc)
            End Try

        End Sub

        Private Sub imgSave_Click(ByVal sender As System.Object, ByVal e As System.Web.UI.ImageClickEventArgs) Handles imgSave.Click

            Try

                If (Page.IsValid) Then
                    SavePhotos()
                End If

            Catch exc As Exception    'Module failed to load
                ProcessModuleLoadException(Me, exc)
            End Try

        End Sub

        Private Sub cmdSave_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cmdSave.Click

            Try

                If (Page.IsValid) Then
                    SavePhotos()
                End If

            Catch exc As Exception    'Module failed to load
                ProcessModuleLoadException(Me, exc)
            End Try

        End Sub

#End Region

    End Class

End Namespace