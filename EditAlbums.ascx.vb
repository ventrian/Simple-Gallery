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

    Partial Public Class EditAlbums
        Inherits SimpleGalleryBase

#Region " Private Methods "

        Private Sub BindAlbums()

            Dim objAlbumController As New AlbumController

            Localization.LocalizeDataGrid(grdAlbums, Me.LocalResourceFile)

            grdAlbums.DataSource = objAlbumController.List(Me.ModuleId, Null.NullInteger, False, True, Common.AlbumSortType.Caption, Common.SortDirection.ASC)
            grdAlbums.DataBind()

            If (grdAlbums.Items.Count > 0) Then
                grdAlbums.Visible = True
                lblNoAlbums.Visible = False
            Else
                grdAlbums.Visible = False
                lblNoAlbums.Visible = True
                lblNoAlbums.Text = Localization.GetString("NoAlbumsMessage.Text", LocalResourceFile)
            End If

        End Sub

        Private Sub BindCrumbs()

            ucGalleryMenu.AddCrumb(Localization.GetString("AllAlbums", LocalResourceFile), NavigateURL())
            ucGalleryMenu.AddCrumb(Localization.GetString("EditAlbums", LocalResourceFile), EditUrl("EditAlbums"))

        End Sub

#End Region

#Region " Event Handlers "

        Private Sub Page_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load

            If (HasAlbumPermissions() = False) Then
                Response.Redirect(NavigateURL(), True)
            End If

            Try

                BindCrumbs()
                BindAlbums()

            Catch exc As Exception    'Module failed to load
                ProcessModuleLoadException(Me, exc)
            End Try

        End Sub

        Private Sub cmdAddAlbum_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cmdAddAlbum.Click

            Response.Redirect(EditUrl("EditAlbum"), True)

        End Sub

        Private Sub cmdReturnToGallery_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cmdReturnToGallery.Click

            Response.Redirect(NavigateURL(), True)

        End Sub

        Private Sub cmdEditSortOrder_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cmdEditSortOrder.Click

            Response.Redirect(EditUrl("SortOrder"), True)

        End Sub

#End Region

    End Class

End Namespace