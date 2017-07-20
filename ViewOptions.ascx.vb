'
' Simple Gallery for DotNetNuke -  http://www.dotnetnuke.com
' Copyright (c) 2002-2007
' by Ventrian ( sales@ventrian.com ) ( http://www.ventrian.com )
'

Imports System.IO

Imports DotNetNuke.Common
Imports DotNetNuke.Common.Utilities
Imports DotNetNuke.Entities.Modules
Imports DotNetNuke.Entities.Portals
Imports DotNetNuke.Security.Roles
Imports DotNetNuke.Services.Exceptions
Imports DotNetNuke.Services.Localization

Imports Ventrian.SimpleGallery.Common
Imports Ventrian.SimpleGallery.Entities

Namespace Ventrian.SimpleGallery

    Partial Public Class ViewOptions
        Inherits ModuleSettingsBase

#Region " Private Members "

        Private _gallerySettings As GallerySettings

#End Region

#Region " Private Properties "

        Public ReadOnly Property GallerySettings() As Entities.GallerySettings
            Get
                If (_gallerySettings Is Nothing) Then
                    _gallerySettings = New Entities.GallerySettings(Settings)
                End If
                Return _gallerySettings
            End Get
        End Property

#End Region

#Region " Private Methods "

        Private Sub BindAlbums()

            Dim objAlbumController As New AlbumController

            drpAlbumFilter.DataSource = objAlbumController.List(Me.ModuleId, Null.NullInteger, True, True, AlbumSortType.Caption, SortDirection.ASC)
            drpAlbumFilter.DataBind()

            drpAlbumFilter.Items.Insert(0, New ListItem(Localization.GetString("AllAlbums", LocalResourceFile), "-1"))

        End Sub

        Private Sub BindSortBy()

            For Each value As Integer In System.Enum.GetValues(GetType(SortType))
                Dim li As New ListItem
                li.Value = System.Enum.GetName(GetType(SortType), value)
                li.Text = Localization.GetString(System.Enum.GetName(GetType(SortType), value), Me.LocalResourceFile)
                drpSortBy.Items.Add(li)
            Next

            For Each value As Integer In System.Enum.GetValues(GetType(AlbumSortType))
                Dim li As New ListItem
                li.Value = System.Enum.GetName(GetType(AlbumSortType), value)
                li.Text = Localization.GetString(System.Enum.GetName(GetType(AlbumSortType), value), Me.LocalResourceFile)
                drpAlbumSortBy.Items.Add(li)
            Next

        End Sub

        Private Sub BindSortDirection()

            For Each value As Integer In System.Enum.GetValues(GetType(SortDirection))
                Dim li As New ListItem
                li.Value = System.Enum.GetName(GetType(SortDirection), value)
                li.Text = Localization.GetString(System.Enum.GetName(GetType(SortDirection), value), Me.LocalResourceFile)
                drpSortDirection.Items.Add(li)
            Next

            For Each value As Integer In System.Enum.GetValues(GetType(SortDirection))
                Dim li As New ListItem
                li.Value = System.Enum.GetName(GetType(SortDirection), value)
                li.Text = Localization.GetString(System.Enum.GetName(GetType(SortDirection), value), Me.LocalResourceFile)
                drpAlbumSortDirection.Items.Add(li)
            Next

        End Sub

        Private Sub BindThumbnailType()

            For Each value As Integer In System.Enum.GetValues(GetType(ThumbnailType))
                Dim li As New ListItem
                li.Value = System.Enum.GetName(GetType(ThumbnailType), value)
                li.Text = Localization.GetString(System.Enum.GetName(GetType(ThumbnailType), value), Me.LocalResourceFile)
                rdoThumbnailTypeAlbum.Items.Add(li)
            Next

            For Each value As Integer In System.Enum.GetValues(GetType(ThumbnailType))
                Dim li As New ListItem
                li.Value = System.Enum.GetName(GetType(ThumbnailType), value)
                li.Text = Localization.GetString(System.Enum.GetName(GetType(ThumbnailType), value), Me.LocalResourceFile)
                rdoThumbnailTypePhoto.Items.Add(li)
            Next

        End Sub

        Private Sub BindCompressionType()

            For Each value As Integer In System.Enum.GetValues(GetType(CompressionType))
                Dim li As New ListItem
                li.Value = System.Enum.GetName(GetType(CompressionType), value)
                li.Text = Localization.GetString(System.Enum.GetName(GetType(CompressionType), value), Me.LocalResourceFile)
                drpCompressionType.Items.Add(li)
                drpCompressionTypeAlbum.Items.Add(New ListItem(li.Text, li.Value))
                drpCompressionTypePhoto.Items.Add(New ListItem(li.Text, li.Value))
            Next

        End Sub

        Private Sub BindWatermarkPosition()

            For Each value As Integer In System.Enum.GetValues(GetType(WatermarkPosition))
                Dim li As New ListItem
                li.Value = System.Enum.GetName(GetType(WatermarkPosition), value)
                li.Text = Localization.GetString(System.Enum.GetName(GetType(WatermarkPosition), value), Me.LocalResourceFile)
                drpWatermarkPosition.Items.Add(li)
            Next

        End Sub

        Private Sub BindSlideshowType()

            For Each value As Integer In System.Enum.GetValues(GetType(SlideshowType))
                Dim li As New ListItem
                li.Value = System.Enum.GetName(GetType(SlideshowType), value)
                li.Text = Localization.GetString(System.Enum.GetName(GetType(SlideshowType), value), Me.LocalResourceFile)
                drpSlideshowType.Items.Add(li)
            Next

        End Sub

        Private Sub BindBorderStyles()

            Dim templateRoot As String = Me.MapPath("Images/Borders")
            If Directory.Exists(templateRoot) Then
                Dim arrFolders() As String = Directory.GetDirectories(templateRoot)
                For Each folder As String In arrFolders
                    Dim folderName As String = folder.Substring(folder.LastIndexOf("\") + 1)
                    Dim objListItem As ListItem = New ListItem
                    objListItem.Text = folderName
                    objListItem.Value = folderName
                    drpBorderStyle.Items.Add(objListItem)
                Next
            End If

        End Sub

        Private Sub BindSettings()

            Dim objSettingController As New SettingController

            ' Basic Settings
            Dim lb As ListItem = drpAlbumFilter.Items.FindByValue(GallerySettings.AlbumFilter)
            If Not lb Is Nothing Then lb.Selected = True
            txtAlbumsPerRow.Text = GallerySettings.AlbumsPerRow.ToString()
            If Not (drpAlbumSortBy.Items.FindByValue(GallerySettings.AlbumSortBy.ToString()) Is Nothing) Then
                drpAlbumSortBy.SelectedValue = GallerySettings.AlbumSortBy.ToString()
            End If
            If Not (drpAlbumSortDirection.Items.FindByValue(GallerySettings.AlbumSortDirection.ToString()) Is Nothing) Then
                drpAlbumSortDirection.SelectedValue = GallerySettings.AlbumSortDirection.ToString()
            End If

            If Not (drpBorderStyle.Items.FindByValue(GallerySettings.BorderStyle.ToString()) Is Nothing) Then
                drpBorderStyle.SelectedValue = Me.GallerySettings.BorderStyle
            End If
            txtPhotosPerPage.Text = GallerySettings.PhotosPerPage.ToString()
            txtThumbnailsPerRow.Text = GallerySettings.PhotosPerRow.ToString()
            If Not (drpSortBy.Items.FindByValue(GallerySettings.SortBy.ToString()) Is Nothing) Then
                drpSortBy.SelectedValue = GallerySettings.SortBy.ToString()
            End If
            If Not (drpSortDirection.Items.FindByValue(GallerySettings.SortDirection.ToString()) Is Nothing) Then
                drpSortDirection.SelectedValue = GallerySettings.SortDirection.ToString()
            End If

            ' Advanced Settings
            txtAlbumDefaultPath.Text = GallerySettings.AlbumDefaultPath
            txtLightboxDefaultPath.Text = GallerySettings.LightboxDefaultPath
            chkEnableSearch.Checked = GallerySettings.EnableSearch
            chkEnableSyndication.Checked = GallerySettings.EnableSyndication
            chkEnableTooltip.Checked = GallerySettings.EnableTooltip
            chkHideBreadCrumbs.Checked = GallerySettings.HideBreadCrumbs
            chkHidePager.Checked = GallerySettings.HidePager
            chkIncludeJQuery.Checked = GallerySettings.IncludeJQuery
            chkIncludeViewCart.Checked = GallerySettings.IncludeViewCart
            chkUseAlbumAnchors.Checked = GallerySettings.UseAlbumAnchors

            ' Compression Settings - Resize
            chkResizePhoto.Checked = GallerySettings.ResizePhoto
            If Not (drpCompressionType.Items.FindByValue(GallerySettings.Compression.ToString()) Is Nothing) Then
                drpCompressionType.SelectedValue = GallerySettings.Compression.ToString()
            End If
            txtImageWidth.Text = GallerySettings.ImageWidth.ToString()
            txtImageHeight.Text = GallerySettings.ImageHeight.ToString()
            chkUseWatermark.Checked = Me.GallerySettings.UseWatermark
            txtWatermarkText.Text = Me.GallerySettings.WatermarkText
            ctlWatermarkImage.Url = Me.GallerySettings.WatermarkImage
            If Not (drpWatermarkPosition.Items.FindByValue(GallerySettings.WatermarkImagePosition.ToString()) Is Nothing) Then
                drpWatermarkPosition.SelectedValue = GallerySettings.WatermarkImagePosition.ToString()
            End If

            ' Compression Settings - Album
            If Not (drpCompressionTypeAlbum.Items.FindByValue(GallerySettings.CompressionAlbum.ToString()) Is Nothing) Then
                drpCompressionTypeAlbum.SelectedValue = GallerySettings.CompressionAlbum.ToString()
            End If
            If Not (rdoThumbnailTypeAlbum.Items.FindByValue(GallerySettings.ThumbnailAlbum.ToString()) Is Nothing) Then
                rdoThumbnailTypeAlbum.SelectedValue = GallerySettings.ThumbnailAlbum.ToString()
            End If
            txtAlbumThumbnailWidth.Text = GallerySettings.AlbumThumbnailWidth.ToString()
            txtAlbumThumbnailHeight.Text = GallerySettings.AlbumThumbnailHeight.ToString()
            txtAlbumThumbnailSquare.Text = GallerySettings.AlbumThumbnailSquare.ToString()

            ' Compression Settings - Photo
            If Not (drpCompressionTypePhoto.Items.FindByValue(GallerySettings.CompressionPhoto.ToString()) Is Nothing) Then
                drpCompressionTypePhoto.SelectedValue = GallerySettings.CompressionPhoto.ToString()
            End If
            If Not (rdoThumbnailTypePhoto.Items.FindByValue(GallerySettings.ThumbnailPhoto.ToString()) Is Nothing) Then
                rdoThumbnailTypePhoto.SelectedValue = GallerySettings.ThumbnailPhoto.ToString()
            End If
            txtThumbnailWidth.Text = GallerySettings.ThumbnailWidth.ToString()
            txtThumbnailHeight.Text = GallerySettings.ThumbnailHeight.ToString()
            txtThumbnailSquare.Text = GallerySettings.ThumbnailSquare.ToString()

            ' Security Settings
            chkPhotoModeration.Checked = Me.GallerySettings.PhotoModeration
            Dim objRoleController As New RoleController
            dgPhotoPermissions.DataSource = objRoleController.GetPortalRoles(PortalController.GetCurrentPortalSettings.PortalId)
            dgPhotoPermissions.DataBind()
            dgPhotoPermissions.Columns(4).Visible = Me.GallerySettings.PhotoModeration

            ' SlideShow Settings
            drpSlideshowType.SelectedValue = Me.GallerySettings.Slideshow.ToString()
            txtStandardWidth.Text = GallerySettings.StandardWidth.ToString()
            txtPopupWidth.Text = GallerySettings.PopupWidth.ToString()
            txtPopupHeight.Text = GallerySettings.PopupHeight.ToString()
            chkEnableScrollbar.Checked = GallerySettings.EnableScrollbar
            txtNextKey.Text = GallerySettings.LightboxNextKey
            txtPreviousKey.Text = GallerySettings.LightboxPreviousKey
            txtCloseKey.Text = GallerySettings.LightboxCloseKey
            txtDownloadKey.Text = GallerySettings.LightboxDownloadKey
            txtSlideInterval.Text = GallerySettings.LightboxSlideInterval.ToString()
            chkSlideHideTitle.Checked = GallerySettings.LightboxHideTitle
            chkSlideHideDescription.Checked = GallerySettings.LightboxHideDescription
            chkSlideHidePaging.Checked = GallerySettings.LightboxHidePaging
            chkSlideHideTags.Checked = GallerySettings.LightboxHideTags
            chkSlideHideDownload.Checked = GallerySettings.LightboxHideDownload

            SetSlideshowVisibility()

            ' Tag Settings
            chkEnableTags.Checked = Me.GallerySettings.EnableTags
            chkRequireTags.Checked = Me.GallerySettings.RequireTags
            txtTagCount.Text = Me.GallerySettings.TagCount.ToString()

            ' Uploader Settings
            txtUploaderFileSize.Text = Me.GallerySettings.UploaderFileSize.ToString()
            chkUseXmpExif.Checked = Me.GallerySettings.UseXmpExif

            ' Zip Settings
            chkEnableZip.Checked = Me.GallerySettings.ZipEnabled
            chkIncludeSubFolders.Checked = Me.GallerySettings.ZipIncludeSubFolders

        End Sub

        Private Sub SaveSettings()

            Dim objModuleController As New ModuleController

            ' Basic Settings
            objModuleController.UpdateTabModuleSetting(Me.TabModuleId, Constants.SETTING_ALBUM_FILTER, drpAlbumFilter.SelectedValue)
            objModuleController.UpdateTabModuleSetting(Me.TabModuleId, Constants.SETTING_ALBUM_SORT_BY, drpAlbumSortBy.SelectedValue)
            objModuleController.UpdateTabModuleSetting(Me.TabModuleId, Constants.SETTING_ALBUM_SORT_DIRECTION, drpAlbumSortDirection.SelectedValue)
            objModuleController.UpdateTabModuleSetting(Me.TabModuleId, Constants.SETTING_BORDER_STYLE, drpBorderStyle.SelectedValue)
            objModuleController.UpdateTabModuleSetting(Me.TabModuleId, Constants.SETTING_ALBUMS_PER_ROW, txtAlbumsPerRow.Text)
            objModuleController.UpdateTabModuleSetting(Me.TabModuleId, Constants.SETTING_PHOTOS_PER_PAGE, txtPhotosPerPage.Text)
            objModuleController.UpdateTabModuleSetting(Me.TabModuleId, Constants.SETTING_PHOTOS_PER_ROW, txtThumbnailsPerRow.Text)
            objModuleController.UpdateTabModuleSetting(Me.TabModuleId, Constants.SETTING_SORT_BY, drpSortBy.SelectedValue)
            objModuleController.UpdateTabModuleSetting(Me.TabModuleId, Constants.SETTING_SORT_DIRECTION, drpSortDirection.SelectedValue)

            ' Advanced Settings
            objModuleController.UpdateModuleSetting(Me.ModuleId, Constants.SETTING_ALBUM_DEFAULT_PATH, txtAlbumDefaultPath.Text)
            objModuleController.UpdateModuleSetting(Me.ModuleId, Constants.SETTING_LIGHTBOX_DEFAULT_PATH, txtLightboxDefaultPath.Text)
            objModuleController.UpdateModuleSetting(Me.ModuleId, Constants.SETTING_ENABLE_SEARCH, chkEnableSearch.Checked.ToString())
            objModuleController.UpdateTabModuleSetting(Me.TabModuleId, Constants.SETTING_ENABLE_SYNDICATION, chkEnableSyndication.Checked.ToString())
            objModuleController.UpdateTabModuleSetting(Me.TabModuleId, Constants.SETTING_ENABLE_TOOLTIP, chkEnableTooltip.Checked.ToString())
            objModuleController.UpdateTabModuleSetting(Me.TabModuleId, Constants.SETTING_HIDE_BREADCRUMBS, chkHideBreadCrumbs.Checked.ToString())
            objModuleController.UpdateTabModuleSetting(Me.TabModuleId, Constants.SETTING_HIDE_PAGER, chkHidePager.Checked.ToString())
            objModuleController.UpdateTabModuleSetting(Me.TabModuleId, Constants.SETTING_INCLUDE_JQUERY, chkIncludeJQuery.Checked.ToString())
            objModuleController.UpdateTabModuleSetting(Me.TabModuleId, Constants.SETTING_INCLUDE_VIEWCART, chkIncludeViewCart.Checked.ToString())
            objModuleController.UpdateTabModuleSetting(Me.TabModuleId, Constants.SETTING_USE_ALBUM_ANCHORS, chkUseAlbumAnchors.Checked.ToString())

            ' Compression Settings - Resize
            objModuleController.UpdateModuleSetting(Me.ModuleId, Constants.SETTING_RESIZE_PHOTO, chkResizePhoto.Checked.ToString())
            objModuleController.UpdateModuleSetting(Me.ModuleId, Constants.SETTING_COMPRESSION, drpCompressionType.SelectedValue)
            objModuleController.UpdateModuleSetting(Me.ModuleId, Constants.SETTING_WIDTH, txtImageWidth.Text)
            objModuleController.UpdateModuleSetting(Me.ModuleId, Constants.SETTING_HEIGHT, txtImageHeight.Text)
            objModuleController.UpdateModuleSetting(Me.ModuleId, Constants.SETTING_USE_WATERMARK, chkUseWatermark.Checked.ToString())
            objModuleController.UpdateModuleSetting(Me.ModuleId, Constants.SETTING_WATERMARK_TEXT, txtWatermarkText.Text)
            objModuleController.UpdateModuleSetting(Me.ModuleId, Constants.SETTING_WATERMARK_IMAGE, ctlWatermarkImage.Url)
            objModuleController.UpdateModuleSetting(Me.ModuleId, Constants.SETTING_WATERMARK_IMAGE_POSITION, drpWatermarkPosition.SelectedValue)

            ' Compression Settings - Album
            objModuleController.UpdateModuleSetting(Me.ModuleId, Constants.SETTING_COMPRESSION_ALBUM, drpCompressionTypeAlbum.SelectedValue)
            objModuleController.UpdateModuleSetting(Me.ModuleId, Constants.SETTING_THUMBNAIL_ALBUM, rdoThumbnailTypeAlbum.SelectedValue)
            objModuleController.UpdateModuleSetting(Me.ModuleId, Constants.SETTING_ALBUM_THUMBNAIL_WIDTH, txtAlbumThumbnailWidth.Text)
            objModuleController.UpdateModuleSetting(Me.ModuleId, Constants.SETTING_ALBUM_THUMBNAIL_HEIGHT, txtAlbumThumbnailHeight.Text)
            objModuleController.UpdateModuleSetting(Me.ModuleId, Constants.SETTING_ALBUM_THUMBNAIL_SQUARE, txtAlbumThumbnailSquare.Text)

            ' Compression Settings - Photo
            objModuleController.UpdateModuleSetting(Me.ModuleId, Constants.SETTING_COMPRESSION_PHOTO, drpCompressionTypePhoto.SelectedValue)
            objModuleController.UpdateModuleSetting(Me.ModuleId, Constants.SETTING_THUMBNAIL_PHOTO, rdoThumbnailTypePhoto.SelectedValue)
            objModuleController.UpdateModuleSetting(Me.ModuleId, Constants.SETTING_THUMBNAIL_WIDTH, txtThumbnailWidth.Text)
            objModuleController.UpdateModuleSetting(Me.ModuleId, Constants.SETTING_THUMBNAIL_HEIGHT, txtThumbnailHeight.Text)
            objModuleController.UpdateModuleSetting(Me.ModuleId, Constants.SETTING_THUMBNAIL_SQUARE, txtThumbnailSquare.Text)

            ' Security Settings
            Dim uploadRoles As String = ""
            Dim editRoles As String = ""
            Dim deleteRoles As String = ""
            Dim approveRoles As String = ""
            Dim albumRoles As String = ""

            For Each item As DataGridItem In dgPhotoPermissions.Items
                If (item.ItemType = ListItemType.Item Or item.ItemType = ListItemType.AlternatingItem) Then

                    Dim chkUpload As CheckBox = CType(item.FindControl("chkUpload"), CheckBox)
                    Dim chkEdit As CheckBox = CType(item.FindControl("chkEdit"), CheckBox)
                    Dim chkDelete As CheckBox = CType(item.FindControl("chkDelete"), CheckBox)
                    Dim chkApprove As CheckBox = CType(item.FindControl("chkApprove"), CheckBox)
                    Dim chkAlbum As CheckBox = CType(item.FindControl("chkAlbum"), CheckBox)

                    Dim objRoleController As New RoleController
                    Dim objRole As RoleInfo = objRoleController.GetRole(Convert.ToInt32(dgPhotoPermissions.DataKeys(item.ItemIndex).ToString()), Me.PortalId)

                    If (chkUpload.Checked) Then
                        uploadRoles = uploadRoles & objRole.RoleName & ";"
                    End If

                    If (chkEdit.Checked) Then
                        editRoles = editRoles & objRole.RoleName & ";"
                    End If

                    If (chkDelete.Checked) Then
                        deleteRoles = deleteRoles & objRole.RoleName & ";"
                    End If

                    If (chkApprove.Checked) Then
                        approveRoles = approveRoles & objRole.RoleName & ";"
                    End If

                    If (chkAlbum.Checked) Then
                        albumRoles = albumRoles & objRole.RoleName & ";"
                    End If

                End If
            Next

            objModuleController.UpdateModuleSetting(ModuleId, Constants.SETTING_UPLOAD_ROLES, uploadRoles)
            objModuleController.UpdateModuleSetting(ModuleId, Constants.SETTING_EDIT_ROLES, editRoles)
            objModuleController.UpdateModuleSetting(ModuleId, Constants.SETTING_DELETE_ROLES, deleteRoles)
            objModuleController.UpdateModuleSetting(ModuleId, Constants.SETTING_APPROVE_ROLES, approveRoles)
            objModuleController.UpdateModuleSetting(ModuleId, Constants.SETTING_ALBUM_ROLES, albumRoles)

            ' SlideShow Settings
            objModuleController.UpdateModuleSetting(Me.ModuleId, Constants.SETTING_SLIDESHOW_TYPE, drpSlideshowType.SelectedValue)
            objModuleController.UpdateModuleSetting(Me.ModuleId, Constants.SETTING_STANDARD_WIDTH, txtStandardWidth.Text)
            objModuleController.UpdateModuleSetting(Me.ModuleId, Constants.SETTING_POPUP_WIDTH, txtPopupWidth.Text)
            objModuleController.UpdateModuleSetting(Me.ModuleId, Constants.SETTING_POPUP_HEIGHT, txtPopupHeight.Text)
            objModuleController.UpdateModuleSetting(Me.ModuleId, Constants.SETTING_ENABLE_SCROLLBAR, chkEnableScrollbar.Checked.ToString())
            objModuleController.UpdateModuleSetting(Me.ModuleId, Constants.SETTING_LIGHTBOX_NEXT_KEY, txtNextKey.Text)
            objModuleController.UpdateModuleSetting(Me.ModuleId, Constants.SETTING_LIGHTBOX_PREVIOUS_KEY, txtPreviousKey.Text)
            objModuleController.UpdateModuleSetting(Me.ModuleId, Constants.SETTING_LIGHTBOX_CLOSE_KEY, txtCloseKey.Text)
            objModuleController.UpdateModuleSetting(Me.ModuleId, Constants.SETTING_LIGHTBOX_DOWNLOAD_KEY, txtDownloadKey.Text)
            If (Convert.ToInt32(txtSlideInterval.Text) > 0) Then
                objModuleController.UpdateModuleSetting(Me.ModuleId, Constants.SETTING_LIGHTBOX_SLIDE_INTERVAL, txtSlideInterval.Text)
            End If
            objModuleController.UpdateModuleSetting(Me.ModuleId, Constants.SETTING_LIGHTBOX_HIDE_TITLE, chkSlideHideTitle.Checked.ToString())
            objModuleController.UpdateModuleSetting(Me.ModuleId, Constants.SETTING_LIGHTBOX_HIDE_DESCRIPTION, chkSlideHideDescription.Checked.ToString())
            objModuleController.UpdateModuleSetting(Me.ModuleId, Constants.SETTING_LIGHTBOX_HIDE_PAGING, chkSlideHidePaging.Checked.ToString())
            objModuleController.UpdateModuleSetting(Me.ModuleId, Constants.SETTING_LIGHTBOX_HIDE_TAGS, chkSlideHideTags.Checked.ToString())
            objModuleController.UpdateModuleSetting(Me.ModuleId, Constants.SETTING_LIGHTBOX_HIDE_DOWNLOAD, chkSlideHideDownload.Checked.ToString())

            ' Tag Settings
            Dim tagCount As Integer = Convert.ToInt32(txtTagCount.Text)
            If (tagCount < 0) Then
                tagCount = Constants.DEFAULT_TAG_COUNT
            End If
            objModuleController.UpdateModuleSetting(Me.ModuleId, Constants.SETTING_ENABLE_TAGS, chkEnableTags.Checked.ToString())
            objModuleController.UpdateModuleSetting(Me.ModuleId, Constants.SETTING_REQUIRE_TAGS, chkRequireTags.Checked.ToString())
            objModuleController.UpdateModuleSetting(Me.ModuleId, Constants.SETTING_TAG_COUNT, tagCount.ToString())

            objModuleController.UpdateModuleSetting(Me.ModuleId, Constants.SETTING_PHOTO_MODERATION, chkPhotoModeration.Checked.ToString())

            Dim maximumFileSize As Integer = Convert.ToInt32(txtUploaderFileSize.Text)
            If (maximumFileSize > 0) Then
                objModuleController.UpdateModuleSetting(Me.ModuleId, Constants.SETTING_UPLOADER_FILE_SIZE, txtUploaderFileSize.Text)
            End If
            objModuleController.UpdateModuleSetting(Me.ModuleId, Constants.SETTING_USE_XMP_EXIF, chkUseXmpExif.Checked.ToString())


            objModuleController.UpdateModuleSetting(Me.ModuleId, Constants.SETTING_ZIP_ENABLED, chkEnableZip.Checked.ToString())
            objModuleController.UpdateModuleSetting(Me.ModuleId, Constants.SETTING_ZIP_INCLUDE_SUBFOLDERS, chkIncludeSubFolders.Checked.ToString())

        End Sub

        Private Sub SetSlideshowVisibility()

            trStandardWidth.Visible = (drpSlideshowType.SelectedValue = "Standard")

            trPopupWidth.Visible = (drpSlideshowType.SelectedValue = "Popup")
            trPopupHeight.Visible = (drpSlideshowType.SelectedValue = "Popup")
            trEnableScrollbar.Visible = (drpSlideshowType.SelectedValue = "Popup")

            trLightboxNextKey.Visible = (drpSlideshowType.SelectedValue = "Lightbox")
            trLightboxPreviousKey.Visible = (drpSlideshowType.SelectedValue = "Lightbox")
            trLightboxCloseKey.Visible = (drpSlideshowType.SelectedValue = "Lightbox")
            trLightboxDownloadKey.Visible = (drpSlideshowType.SelectedValue = "Lightbox")
            trLightboxSlideInterval.Visible = (drpSlideshowType.SelectedValue = "Lightbox")
            trLightboxHideTitle.Visible = (drpSlideshowType.SelectedValue = "Lightbox")
            trLightboxHideDescription.Visible = (drpSlideshowType.SelectedValue = "Lightbox")
            trLightboxHideDownload.Visible = (drpSlideshowType.SelectedValue = "Lightbox")

        End Sub

        Private Sub SetVisibility()

            Dim objThumbnailAlbum As ThumbnailType = CType(System.Enum.Parse(GetType(ThumbnailType), rdoThumbnailTypeAlbum.SelectedValue), ThumbnailType)
            Dim objThumbnailPhoto As ThumbnailType = CType(System.Enum.Parse(GetType(ThumbnailType), rdoThumbnailTypePhoto.SelectedValue), ThumbnailType)

            trAlbumThumbnailWidth.Visible = (objThumbnailAlbum = ThumbnailType.Proportion)
            trAlbumThumbnailHeight.Visible = (objThumbnailAlbum = ThumbnailType.Proportion)
            trAlbumThumbnailSquare.Visible = (objThumbnailAlbum = ThumbnailType.Square)

            trThumbnailWidth.Visible = (objThumbnailPhoto = ThumbnailType.Proportion)
            trThumbnailHeight.Visible = (objThumbnailPhoto = ThumbnailType.Proportion)
            trThumbnailSquare.Visible = (objThumbnailPhoto = ThumbnailType.Square)

        End Sub

#End Region

#Region " Event Handlers "

        Private Sub Page_PreRender(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.PreRender

            SetVisibility()

        End Sub

        Private Sub dgPhotoPermissions_ItemDataBound(ByVal sender As System.Object, ByVal e As System.Web.UI.WebControls.DataGridItemEventArgs) Handles dgPhotoPermissions.ItemDataBound

            If (e.Item.ItemType = ListItemType.Item Or e.Item.ItemType = ListItemType.AlternatingItem) Then

                Dim chkUpload As CheckBox = CType(e.Item.FindControl("chkUpload"), CheckBox)
                Dim chkEdit As CheckBox = CType(e.Item.FindControl("chkEdit"), CheckBox)
                Dim chkDelete As CheckBox = CType(e.Item.FindControl("chkDelete"), CheckBox)
                Dim chkApprove As CheckBox = CType(e.Item.FindControl("chkApprove"), CheckBox)
                Dim chkAlbum As CheckBox = CType(e.Item.FindControl("chkAlbum"), CheckBox)

                Dim objRole As RoleInfo = CType(e.Item.DataItem, RoleInfo)

                If (Settings.Contains(Constants.SETTING_UPLOAD_ROLES)) Then
                    Dim roles As String = Settings(Constants.SETTING_UPLOAD_ROLES).ToString()
                    For Each role As String In roles.Split(Convert.ToChar(";"))
                        If (role = objRole.RoleName) Then
                            chkUpload.Checked = True
                            Exit For
                        End If
                    Next
                End If

                If (Settings.Contains(Constants.SETTING_EDIT_ROLES)) Then
                    Dim roles As String = Settings(Constants.SETTING_EDIT_ROLES).ToString()
                    For Each role As String In roles.Split(Convert.ToChar(";"))
                        If (role = objRole.RoleName) Then
                            chkEdit.Checked = True
                            Exit For
                        End If
                    Next
                End If

                If (Settings.Contains(Constants.SETTING_DELETE_ROLES)) Then
                    Dim roles As String = Settings(Constants.SETTING_DELETE_ROLES).ToString()
                    For Each role As String In roles.Split(Convert.ToChar(";"))
                        If (role = objRole.RoleName) Then
                            chkDelete.Checked = True
                            Exit For
                        End If
                    Next
                End If

                If (Settings.Contains(Constants.SETTING_APPROVE_ROLES)) Then
                    Dim roles As String = Settings(Constants.SETTING_APPROVE_ROLES).ToString()
                    For Each role As String In roles.Split(Convert.ToChar(";"))
                        If (role = objRole.RoleName) Then
                            chkApprove.Checked = True
                            Exit For
                        End If
                    Next
                End If

                If (Settings.Contains(Constants.SETTING_ALBUM_ROLES)) Then
                    Dim roles As String = Settings(Constants.SETTING_ALBUM_ROLES).ToString()
                    For Each role As String In roles.Split(Convert.ToChar(";"))
                        If (role = objRole.RoleName) Then
                            chkAlbum.Checked = True
                            Exit For
                        End If
                    Next
                End If

            End If

        End Sub

        Private Sub chkPhotoModeration_CheckedChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles chkPhotoModeration.CheckedChanged

            dgPhotoPermissions.Columns(4).Visible = chkPhotoModeration.Checked

        End Sub

        Private Sub drpSlideshowType_SelectedIndexChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles drpSlideshowType.SelectedIndexChanged

            SetSlideshowVisibility()

        End Sub

#End Region

#Region " Base Method Implementations "

        Public Overrides Sub LoadSettings()

            Try

                If (Page.IsPostBack = False) Then

                    ctlWatermarkImage.FileFilter = DotNetNuke.Common.glbImageFileTypes
                    BindAlbums()
                    BindSortBy()
                    BindSortDirection()
                    BindThumbnailType()
                    BindCompressionType()
                    BindWatermarkPosition()
                    BindSlideshowType()
                    BindBorderStyles()
                    BindSettings()

                End If

            Catch exc As Exception    'Module failed to load
                ProcessModuleLoadException(Me, exc)
            End Try

        End Sub

        Public Overrides Sub UpdateSettings()

            Try

                If (Page.IsValid) Then
                    SaveSettings()
                End If

            Catch exc As Exception    'Module failed to load
                ProcessModuleLoadException(Me, exc)
            End Try

        End Sub

#End Region

    End Class

End Namespace