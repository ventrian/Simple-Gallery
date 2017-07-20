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

    Partial Public Class EditSortOrder
        Inherits SimpleGalleryBase

#Region " Private Members "

        Private _albumID As Integer = Null.NullInteger

#End Region

#Region " Private Methods "

        Private Sub ReadQueryString()

            If Not (Request("AlbumID") Is Nothing) Then
                _albumID = Convert.ToInt32(Request("AlbumID"))
            End If

        End Sub

        Private Sub BindAlbums()

            Dim objAlbumController As New AlbumController

            drpParentAlbum.DataSource = objAlbumController.List(Me.ModuleId, Null.NullInteger, False, True, Common.AlbumSortType.Custom, Common.SortDirection.ASC)
            drpParentAlbum.DataBind()

            drpParentAlbum.Items.Insert(0, New System.Web.UI.WebControls.ListItem(Localization.GetString("NoParentAlbum", Me.LocalResourceFile), "-1"))

        End Sub

        Private Sub BindCrumbs()

            ucGalleryMenu.AddCrumb(Localization.GetString("AllAlbums", LocalResourceFile), NavigateURL())
            ucGalleryMenu.AddCrumb(Localization.GetString("EditAlbums", LocalResourceFile), EditUrl("EditAlbums"))
            ucGalleryMenu.AddCrumb(Localization.GetString("EditSortOrder", LocalResourceFile), EditUrl("SortOrder"))

        End Sub

        Private Sub BindSortOrder()

            Dim objAlbumController As New AlbumController

            lstAlbums.DataSource = objAlbumController.List(Me.ModuleId, Convert.ToInt32(drpParentAlbum.SelectedValue), False, False, Common.AlbumSortType.Custom, Common.SortDirection.ASC)
            lstAlbums.DataBind()

            If (lstAlbums.Items.Count = 0) Then
                pnlSortOrder.Visible = False
                cmdUpdate.Visible = False
                lblNoAlbums.Text = Localization.GetString("NoAlbumsMessage", Me.LocalResourceFile)
                lblNoAlbums.Visible = True
            Else
                pnlSortOrder.Visible = True
                cmdUpdate.Visible = True
                lblNoAlbums.Visible = False
            End If

        End Sub

#End Region

#Region " Event Handlers "

        Private Sub Page_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load

            Try

                BindCrumbs()

                If (IsPostBack = False) Then

                    BindAlbums()
                    BindSortOrder()

                End If

            Catch exc As Exception    'Module failed to load
                ProcessModuleLoadException(Me, exc)
            End Try

        End Sub

        Private Sub drpParentAlbum_SelectedIndexChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles drpParentAlbum.SelectedIndexChanged

            BindSortOrder()

        End Sub

        Private Sub cmdReturnToGallery_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cmdReturnToGallery.Click

            Response.Redirect(EditUrl("EditAlbums"), True)

        End Sub

        Private Sub cmdUp_Click(ByVal sender As System.Object, ByVal e As System.Web.UI.ImageClickEventArgs) Handles cmdUp.Click

            If (lstAlbums.Items.Count > 1) Then
                If (lstAlbums.SelectedIndex > 0) Then
                    Dim objListItem As New ListItem

                    objListItem.Value = lstAlbums.SelectedItem.Value
                    objListItem.Text = lstAlbums.SelectedItem.Text

                    Dim index As Integer = lstAlbums.SelectedIndex

                    lstAlbums.Items.RemoveAt(index)
                    lstAlbums.Items.Insert(index - 1, objListItem)
                    lstAlbums.SelectedIndex = index - 1
                End If
            End If

        End Sub

        Private Sub cmdDown_Click(ByVal sender As System.Object, ByVal e As System.Web.UI.ImageClickEventArgs) Handles cmdDown.Click

            If (lstAlbums.Items.Count > 1) Then
                If (lstAlbums.SelectedIndex < (lstAlbums.Items.Count - 1)) Then
                    Dim objListItem As New ListItem

                    objListItem.Value = lstAlbums.SelectedItem.Value
                    objListItem.Text = lstAlbums.SelectedItem.Text

                    Dim index As Integer = lstAlbums.SelectedIndex

                    lstAlbums.Items.RemoveAt(index)
                    lstAlbums.Items.Insert(index + 1, objListItem)
                    lstAlbums.SelectedIndex = index + 1
                End If
            End If

        End Sub

        Private Sub cmdEdit_Click(ByVal sender As System.Object, ByVal e As System.Web.UI.ImageClickEventArgs) Handles cmdEdit.Click

            If (lstAlbums.Items.Count > 0) Then
                If Not (lstAlbums.SelectedItem Is Nothing) Then
                    Response.Redirect(EditUrl("AlbumID", lstAlbums.SelectedValue, "EditAlbum"), True)
                End If
            End If

        End Sub

        Private Sub cmdView_Click(ByVal sender As System.Object, ByVal e As System.Web.UI.ImageClickEventArgs) Handles cmdView.Click

            If (lstAlbums.Items.Count > 0) Then
                If Not (lstAlbums.SelectedItem Is Nothing) Then
                    Response.Redirect(NavigateURL(Me.TabId, "", "AlbumID=" & Me.ModuleId.ToString() & "-" & lstAlbums.SelectedValue), True)
                End If
            End If

        End Sub

        Private Sub cmdUpdate_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cmdUpdate.Click

            Dim objAlbumController As New AlbumController

            Dim index As Integer = 0
            For Each item As ListItem In lstAlbums.Items
                Dim objAlbumInfo As AlbumInfo = objAlbumController.Get(Convert.ToInt32(item.Value))

                If Not (objAlbumInfo Is Nothing) Then
                    objAlbumInfo.AlbumOrder = index
                    objAlbumController.Update(objAlbumInfo)
                    index = index + 1
                End If
            Next

            lblAlbumUpdated.Visible = True

        End Sub

#End Region

    End Class

End Namespace