'
' Simple Gallery for DotNetNuke -  http://www.dotnetnuke.com
' Copyright (c) 2002-2007
' by Ventrian ( sales@ventrian.com ) ( http://www.ventrian.com )
'

Imports System.IO

Imports DotNetNuke.Common
Imports DotNetNuke.Common.Utilities
Imports DotNetNuke.Entities.Modules
Imports DotNetNuke.Entities.Modules.Definitions
Imports DotNetNuke.Entities.Tabs
Imports DotNetNuke.Security
Imports DotNetNuke.Security.Roles
Imports DotNetNuke.Services.Exceptions
Imports DotNetNuke.Services.Localization

Imports Ventrian.SimpleGallery.Common
Imports Ventrian.SimpleGallery.Entities

Namespace Ventrian.SimpleGallery

    Partial Public Class RandomPhotoOptions
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

        Private Sub BindModes()

            For Each value As Integer In System.Enum.GetValues(GetType(ModeType))
                Dim li As New ListItem
                li.Value = System.Enum.GetName(GetType(ModeType), value)
                li.Text = Localization.GetString(System.Enum.GetName(GetType(ModeType), value), Me.LocalResourceFile)
                rdoMode.Items.Add(li)
            Next

        End Sub

        Private Sub BindDisplay()

            For Each value As Integer In System.Enum.GetValues(GetType(DisplayType))
                Dim li As New ListItem
                li.Value = System.Enum.GetName(GetType(DisplayType), value)
                li.Text = Localization.GetString("Display-" & System.Enum.GetName(GetType(DisplayType), value), Me.LocalResourceFile)
                rdoDisplay.Items.Add(li)
            Next

        End Sub

        Private Sub BindRepeatDirection()

            For Each value As Integer In System.Enum.GetValues(GetType(RepeatDirection))
                Dim li As New ListItem
                li.Value = System.Enum.GetName(GetType(RepeatDirection), value)
                li.Text = Localization.GetString(System.Enum.GetName(GetType(RepeatDirection), value), Me.LocalResourceFile)
                drpRepeatDirection.Items.Add(li)
            Next

        End Sub

        Private Sub BindTemplateType()

            For Each value As Integer In System.Enum.GetValues(GetType(TemplateModeType))
                Dim li As New ListItem
                li.Value = System.Enum.GetName(GetType(TemplateModeType), value)
                li.Text = Localization.GetString("Template-" & System.Enum.GetName(GetType(TemplateModeType), value), Me.LocalResourceFile)
                rdoTemplateType.Items.Add(li)
            Next

        End Sub

        Private Sub BindModules()

            Dim objDesktopModuleController As New DesktopModuleController
            Dim objDesktopModuleInfo As DesktopModuleInfo = objDesktopModuleController.GetDesktopModuleByModuleName("SimpleGallery")

            If Not (objDesktopModuleInfo Is Nothing) Then

                Dim objTabController As New TabController()
                Dim objTabs As ArrayList = objTabController.GetTabs(PortalId)
                For Each objTab As DotNetNuke.Entities.Tabs.TabInfo In objTabs
                    If Not (objTab Is Nothing) Then
                        If (objTab.IsDeleted = False) Then
                            Dim objModules As New ModuleController
                            For Each pair As KeyValuePair(Of Integer, ModuleInfo) In objModules.GetTabModules(objTab.TabID)
                                Dim objModule As ModuleInfo = pair.Value
                                If (objModule.IsDeleted = False) Then
                                    If (objModule.DesktopModuleID = objDesktopModuleInfo.DesktopModuleID) Then
                                        If PortalSecurity.IsInRoles(objModule.AuthorizedEditRoles) = True And objModule.IsDeleted = False Then
                                            Dim strPath As String = objTab.TabName
                                            Dim objTabSelected As TabInfo = objTab
                                            While objTabSelected.ParentId <> Null.NullInteger
                                                objTabSelected = objTabController.GetTab(objTabSelected.ParentId, objTab.PortalID, False)
                                                If (objTabSelected Is Nothing) Then
                                                    Exit While
                                                End If
                                                strPath = objTabSelected.TabName & " -> " & strPath
                                            End While

                                            Dim objListItem As New ListItem

                                            objListItem.Value = objModule.TabID.ToString() & "-" & objModule.ModuleID.ToString()
                                            objListItem.Text = strPath & " -> " & objModule.ModuleTitle

                                            drpModuleID.Items.Add(objListItem)
                                        End If
                                    End If
                                End If
                            Next
                        End If
                    End If
                Next

            End If

            If (drpModuleID.Items.Count > 0) Then
                BindAlbums()
            End If

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

        Private Sub BindAlbums()

            If (drpModuleID.Items.Count > 0) Then
                Dim objAlbumController As New AlbumController

                Dim values As String() = drpModuleID.SelectedValue.Split(Convert.ToChar("-"))

                drpAlbums.DataSource = objAlbumController.List(Convert.ToInt32(values(1)), Null.NullInteger, True, True, AlbumSortType.Caption, SortDirection.ASC)
                drpAlbums.DataBind()

                drpAlbums.Items.Insert(0, New ListItem(Localization.GetString("AllAlbums", Me.LocalResourceFile), "-1"))
            End If

        End Sub

        Private Sub BindCompressionType()

            For Each value As Integer In System.Enum.GetValues(GetType(CompressionType))
                Dim li As New ListItem
                li.Value = System.Enum.GetName(GetType(CompressionType), value)
                li.Text = Localization.GetString(System.Enum.GetName(GetType(CompressionType), value), Me.LocalResourceFile)
                drpCompressionType.Items.Add(li)
            Next

        End Sub

        Private Sub BindThumbnailType()

            For Each value As Integer In System.Enum.GetValues(GetType(ThumbnailType))
                Dim li As New ListItem
                li.Value = System.Enum.GetName(GetType(ThumbnailType), value)
                li.Text = Localization.GetString(System.Enum.GetName(GetType(ThumbnailType), value), Me.LocalResourceFile)
                rdoThumbnailType.Items.Add(li)
            Next

        End Sub

        Private Sub BindTokens()

            If (trPhotoTemplate.Visible) Then
                For Each templateTokenType As TemplateTokenPhotoInfo In System.Enum.GetValues(GetType(TemplateTokenPhotoInfo))
                    Dim objColumn1 As New HtmlTableCell
                    Dim objColumn2 As New HtmlTableCell

                    objColumn1.Width = "200px"
                    objColumn1.Attributes.Add("Class", "SubHead")
                    objColumn1.InnerHtml = "[" & templateTokenType.ToString() & "]"
                    objColumn2.Attributes.Add("Class", "Normal")
                    objColumn2.InnerHtml = Localization.GetString(templateTokenType.ToString(), Me.LocalResourceFile)

                    Dim objRow As New HtmlTableRow
                    objRow.Cells.Add(objColumn1)
                    objRow.Cells.Add(objColumn2)

                    tblTemplateSettings.Rows.Add(objRow)
                Next
            Else
                For Each templateTokenType As TemplateTokenAlbumInfo In System.Enum.GetValues(GetType(TemplateTokenAlbumInfo))
                    Dim objColumn1 As New HtmlTableCell
                    Dim objColumn2 As New HtmlTableCell

                    objColumn1.Width = "200px"
                    objColumn1.Attributes.Add("Class", "SubHead")
                    objColumn1.InnerHtml = "[" & templateTokenType.ToString() & "]"
                    objColumn2.Attributes.Add("Class", "Normal")
                    If (templateTokenType.ToString() = "EDIT" Or templateTokenType.ToString() = "TITLE" Or templateTokenType.ToString() = "DESCRIPTION" Or templateTokenType.ToString() = "DATECREATED") Then
                        objColumn2.InnerHtml = Localization.GetString("Album-" & templateTokenType.ToString(), Me.LocalResourceFile)
                    Else
                        objColumn2.InnerHtml = Localization.GetString(templateTokenType.ToString(), Me.LocalResourceFile)
                    End If

                    Dim objRow As New HtmlTableRow
                    objRow.Cells.Add(objColumn1)
                    objRow.Cells.Add(objColumn2)

                    tblTemplateSettings.Rows.Add(objRow)
                Next
            End If

        End Sub

        Private Sub BindSettings()

            BindThumbnailType()

            If (Settings.Contains(Constants.SETTING_RANDOM_PHOTO_MODULE_ID) And Settings.Contains(Constants.SETTING_RANDOM_PHOTO_TAB_ID)) Then
                If Not (drpModuleID.Items.FindByValue(Settings(Constants.SETTING_RANDOM_PHOTO_TAB_ID).ToString() & "-" & Settings(Constants.SETTING_RANDOM_PHOTO_MODULE_ID).ToString()) Is Nothing) Then
                    drpModuleID.SelectedValue = Settings(Constants.SETTING_RANDOM_PHOTO_TAB_ID).ToString() & "-" & Settings(Constants.SETTING_RANDOM_PHOTO_MODULE_ID).ToString()
                End If
                BindAlbums()
            End If

            If (Settings.Contains(Constants.SETTING_RANDOM_PHOTO_ALBUM_ID)) Then
                If Not (drpAlbums.Items.FindByValue(Settings(Constants.SETTING_RANDOM_PHOTO_ALBUM_ID).ToString()) Is Nothing) Then
                    drpAlbums.SelectedValue = Settings(Constants.SETTING_RANDOM_PHOTO_ALBUM_ID).ToString()
                End If
            End If

            If (Settings.Contains(Constants.SETTING_BORDER_STYLE)) Then
                If Not (drpBorderStyle.Items.FindByValue(Settings(Constants.SETTING_BORDER_STYLE).ToString()) Is Nothing) Then
                    drpBorderStyle.SelectedValue = Settings(Constants.SETTING_BORDER_STYLE).ToString()
                End If
            Else
                If Not (drpBorderStyle.Items.FindByValue(Constants.DEFAULT_BORDER_STYLE.ToString()) Is Nothing) Then
                    drpBorderStyle.SelectedValue = Constants.DEFAULT_BORDER_STYLE
                End If
            End If

            If Not (rdoMode.Items.FindByValue(Me.GallerySettings.RandomMode.ToString()) Is Nothing) Then
                rdoMode.SelectedValue = Me.GallerySettings.RandomMode.ToString()
            End If

            If Not (rdoDisplay.Items.FindByValue(Me.GallerySettings.RandomDisplay.ToString()) Is Nothing) Then
                rdoDisplay.SelectedValue = Me.GallerySettings.RandomDisplay.ToString()
            End If
            SetTemplateVisibility()

            txtMaxCount.Text = Me.GallerySettings.RandomMaxCount.ToString()

            Dim objTagController As New TagController()
            If (Me.GallerySettings.RandomTagFilter <> Null.NullInteger) Then
                Dim objTag As TagInfo = objTagController.Get(Me.GallerySettings.RandomTagFilter)
                If (objTag IsNot Nothing) Then
                    txtTagFilter.Text = objTag.Name
                End If
            End If

            If Not (drpRepeatDirection.Items.FindByValue(Me.GallerySettings.RandomRepeatDirection.ToString()) Is Nothing) Then
                drpRepeatDirection.SelectedValue = Me.GallerySettings.RandomRepeatDirection.ToString()
            End If

            txtRepeatColumns.Text = Me.GallerySettings.RandomRepeatColumns.ToString()

            txtWidth.Text = Me.GallerySettings.RandomWidth.ToString()
            txtHeight.Text = Me.GallerySettings.RandomHeight.ToString()
            txtSquare.Text = Me.GallerySettings.RandomSquare.ToString()

            txtPhotoTemplate.Text = Me.GallerySettings.RandomTemplate
            txtAlbumTemplate.Text = Me.GallerySettings.RandomTemplateAlbum

            chkLaunchSlideshow.Checked = Me.GallerySettings.RandomLaunchSlideshow
            chkAlbumSlideshow.Checked = Me.GallerySettings.RandomAlbumSlideshow
            chkIncludeJQuery.Checked = Me.GallerySettings.RandomIncludeJQuery

            If (Settings.Contains(Constants.SETTING_RANDOM_COMPRESSION)) Then
                If Not (drpCompressionType.Items.FindByValue(Settings(Constants.SETTING_RANDOM_COMPRESSION).ToString()) Is Nothing) Then
                    drpCompressionType.SelectedValue = Settings(Constants.SETTING_RANDOM_COMPRESSION).ToString()
                End If
            Else
                If Not (drpCompressionType.Items.FindByValue(Constants.DEFAULT_RANDOM_COMPRESSION.ToString()) Is Nothing) Then
                    drpCompressionType.SelectedValue = Constants.DEFAULT_RANDOM_COMPRESSION
                End If
            End If

            If Not (rdoThumbnailType.Items.FindByValue(Me.GallerySettings.RandomThumbnail.ToString()) Is Nothing) Then
                rdoThumbnailType.SelectedValue = Me.GallerySettings.RandomThumbnail.ToString()
            End If

            If Not (rdoTemplateType.Items.FindByValue(Me.GallerySettings.RandomTemplateMode.ToString()) Is Nothing) Then
                rdoTemplateType.SelectedValue = Me.GallerySettings.RandomTemplateMode.ToString()
            End If

        End Sub

        Private Sub SaveSettings()

            Dim objModuleController As New ModuleController
            Dim moduleID As Integer = Null.NullInteger

            If (drpModuleID.Items.Count > 0) Then

                Dim values As String() = drpModuleID.SelectedValue.Split(Convert.ToChar("-"))

                If (values.Length = 2) Then
                    objModuleController.UpdateTabModuleSetting(Me.TabModuleId, Constants.SETTING_RANDOM_PHOTO_TAB_ID, values(0))
                    objModuleController.UpdateTabModuleSetting(Me.TabModuleId, Constants.SETTING_RANDOM_PHOTO_MODULE_ID, values(1))

                    moduleID = values(1)
                End If

            End If

            objModuleController.UpdateTabModuleSetting(Me.TabModuleId, Constants.SETTING_RANDOM_PHOTO_ALBUM_ID, drpAlbums.SelectedValue)
            objModuleController.UpdateTabModuleSetting(Me.TabModuleId, Constants.SETTING_BORDER_STYLE, drpBorderStyle.SelectedValue)
            objModuleController.UpdateTabModuleSetting(Me.TabModuleId, Constants.SETTING_RANDOM_MODE, rdoMode.SelectedValue)
            objModuleController.UpdateTabModuleSetting(Me.TabModuleId, Constants.SETTING_RANDOM_DISPLAY, rdoDisplay.SelectedValue)
            objModuleController.UpdateTabModuleSetting(Me.TabModuleId, Constants.SETTING_RANDOM_MAX_COUNT, txtMaxCount.Text)

            If (txtTagFilter.Text.Trim() <> "") Then
                Dim objTagController As New TagController
                Dim objTag As TagInfo = objTagController.Get(moduleID, txtTagFilter.Text.ToLower())
                If (objTag IsNot Nothing) Then
                    objModuleController.UpdateTabModuleSetting(Me.TabModuleId, Constants.SETTING_RANDOM_TAG_FILTER, objTag.TagID)
                Else
                    objModuleController.UpdateTabModuleSetting(Me.TabModuleId, Constants.SETTING_RANDOM_TAG_FILTER, Null.NullInteger.ToString())
                End If
            Else
                objModuleController.UpdateTabModuleSetting(Me.TabModuleId, Constants.SETTING_RANDOM_TAG_FILTER, Null.NullInteger.ToString())
            End If

            objModuleController.UpdateTabModuleSetting(Me.TabModuleId, Constants.SETTING_RANDOM_REPEAT_DIRECTION, drpRepeatDirection.SelectedValue)
            objModuleController.UpdateTabModuleSetting(Me.TabModuleId, Constants.SETTING_RANDOM_REPEAT_COLUMNS, txtRepeatColumns.Text)
            objModuleController.UpdateTabModuleSetting(Me.TabModuleId, Constants.SETTING_RANDOM_WIDTH, txtWidth.Text)
            objModuleController.UpdateTabModuleSetting(Me.TabModuleId, Constants.SETTING_RANDOM_HEIGHT, txtHeight.Text)
            If (txtSquare.Text <> "" AndAlso CInt(txtSquare.Text) > 0) Then
                objModuleController.UpdateTabModuleSetting(Me.TabModuleId, Constants.SETTING_RANDOM_SQUARE, txtSquare.Text)
            End If
            objModuleController.UpdateTabModuleSetting(Me.TabModuleId, Constants.SETTING_RANDOM_TEMPLATE, txtPhotoTemplate.Text)
            objModuleController.UpdateTabModuleSetting(Me.TabModuleId, Constants.SETTING_RANDOM_TEMPLATE_ALBUM, txtAlbumTemplate.Text)
            objModuleController.UpdateTabModuleSetting(Me.TabModuleId, Constants.SETTING_RANDOM_COMPRESSION, drpCompressionType.SelectedValue)
            objModuleController.UpdateTabModuleSetting(Me.TabModuleId, Constants.SETTING_RANDOM_THUMBNAIL, rdoThumbnailType.SelectedValue)
            objModuleController.UpdateTabModuleSetting(Me.TabModuleId, Constants.SETTING_RANDOM_LAUNCH_SLIDESHOW, chkLaunchSlideshow.Checked.ToString())
            objModuleController.UpdateTabModuleSetting(Me.TabModuleId, Constants.SETTING_RANDOM_ALBUM_SLIDESHOW, chkAlbumSlideshow.Checked.ToString())
            objModuleController.UpdateTabModuleSetting(Me.TabModuleId, Constants.SETTING_RANDOM_INCLUDE_JQUERY, chkIncludeJQuery.Checked.ToString())
            objModuleController.UpdateTabModuleSetting(Me.TabModuleId, Constants.SETTING_RANDOM_TEMPLATE_MODE, rdoTemplateType.SelectedValue)

        End Sub

        Private Sub SetTemplateVisibility()

            trPhotoTemplate.Visible = (rdoDisplay.SelectedValue = DisplayType.Photo.ToString())
            trAlbumTemplate.Visible = (rdoDisplay.SelectedValue = DisplayType.Album.ToString())

        End Sub

        Private Sub SetVisibility()

            Dim objThumbnail As ThumbnailType = CType(System.Enum.Parse(GetType(ThumbnailType), rdoThumbnailType.SelectedValue), ThumbnailType)

            trWidth.Visible = (objThumbnail = ThumbnailType.Proportion)
            trHeight.Visible = (objThumbnail = ThumbnailType.Proportion)
            trSquare.Visible = (objThumbnail = ThumbnailType.Square)

        End Sub

#End Region

#Region " Event Handlers "

        Private Sub Page_PreRender(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.PreRender

            Try

                SetVisibility()

            Catch exc As Exception 'Module failed to load
                ProcessModuleLoadException(Me, exc)
            End Try

        End Sub

        Private Sub drpModuleID_SelectedIndexChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles drpModuleID.SelectedIndexChanged

            Try
                BindAlbums()
            Catch exc As Exception 'Module failed to load
                ProcessModuleLoadException(Me, exc)
            End Try

        End Sub

        Protected Sub rdoDisplay_SelectedIndexChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles rdoDisplay.SelectedIndexChanged

            Try
                SetTemplateVisibility()
                BindTokens()
            Catch exc As Exception 'Module failed to load
                ProcessModuleLoadException(Me, exc)
            End Try

        End Sub

#End Region

#Region " Base Method Implementations "

        Public Overrides Sub LoadSettings()

            Try

                If (IsPostBack = False) Then

                    BindModes()
                    BindDisplay()
                    BindRepeatDirection()
                    BindModules()
                    BindBorderStyles()
                    BindCompressionType()
                    BindTemplateType()
                    BindSettings()
                    BindTokens()

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