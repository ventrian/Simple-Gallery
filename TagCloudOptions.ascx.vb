'
' Simple Gallery for DotNetNuke -  http://www.dotnetnuke.com
' Copyright (c) 2002-2007
' by Ventrian ( sales@ventrian.com ) ( http://www.ventrian.com )
'

Imports System.Linq
Imports DotNetNuke.Common
Imports DotNetNuke.Common.Utilities
Imports DotNetNuke.Entities.Modules
Imports DotNetNuke.Entities.Tabs
Imports DotNetNuke.Security
Imports DotNetNuke.Security.Permissions
Imports DotNetNuke.Services.Localization
Imports DotNetNuke.Services.Exceptions

Imports Ventrian.SimpleGallery.Common
Imports Ventrian.SimpleGallery.Entities

Namespace Ventrian.SimpleGallery

    Partial Public Class TagCloudOptions
        Inherits ModuleSettingsBase

#Region " Private Methods "

        Private Sub BindAlbums()

            If (drpModuleID.Items.Count > 0) Then
                Dim objAlbumController As New AlbumController

                Dim values As String() = drpModuleID.SelectedValue.Split(Convert.ToChar("-"))

                drpAlbums.DataSource = objAlbumController.List(Convert.ToInt32(values(1)), Null.NullInteger, True, True, AlbumSortType.Caption, SortDirection.ASC)
                drpAlbums.DataBind()

                drpAlbums.Items.Insert(0, New ListItem(Localization.GetString("AllAlbums", Me.LocalResourceFile), "-1"))
            End If

        End Sub

        Private Sub BindModules()

            Dim objDesktopModuleInfo As DesktopModuleInfo = DesktopModuleController.GetDesktopModuleByModuleName("SimpleGallery", PortalId)

            If Not (objDesktopModuleInfo Is Nothing) Then

                Dim objTabController As New TabController()
                Dim objTabs As TabCollection = objTabController.GetTabsByPortal(PortalId)
                For Each objTab As TabInfo In objTabs.Values.Where(function(info) info.IsDeleted = False)
                    Dim objModules As New ModuleController
                    For Each pair As KeyValuePair(Of Integer, ModuleInfo) In objModules.GetTabModules(objTab.TabID).Where(function(info) info.Value.IsDeleted = False)
                        Dim objModule As ModuleInfo = pair.Value
                        If (objModule.DesktopModuleID = objDesktopModuleInfo.DesktopModuleID) Then
                            If ModulePermissionController.CanEditModuleContent(objModule) = True And objModule.IsDeleted = False Then
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
                    Next
                Next

            End If

            If (drpModuleID.Items.Count > 0) Then
                BindAlbums()
            End If

        End Sub

        Private Sub BindSettings()

            If (Settings.Contains(Constants.SETTING_TAG_CLOUD_PHOTO_MODULE_ID) And Settings.Contains(Constants.SETTING_TAG_CLOUD_PHOTO_TAB_ID)) Then
                If Not (drpModuleID.Items.FindByValue(Settings(Constants.SETTING_TAG_CLOUD_PHOTO_TAB_ID).ToString() & "-" & Settings(Constants.SETTING_TAG_CLOUD_PHOTO_MODULE_ID).ToString()) Is Nothing) Then
                    drpModuleID.SelectedValue = Settings(Constants.SETTING_TAG_CLOUD_PHOTO_TAB_ID).ToString() & "-" & Settings(Constants.SETTING_TAG_CLOUD_PHOTO_MODULE_ID).ToString()
                End If
                BindAlbums()
            End If

            If (Settings.Contains(Constants.SETTING_TAG_CLOUD_PHOTO_ALBUM_ID)) Then
                If Not (drpAlbums.Items.FindByValue(Settings(Constants.SETTING_TAG_CLOUD_PHOTO_ALBUM_ID).ToString()) Is Nothing) Then
                    drpAlbums.SelectedValue = Settings(Constants.SETTING_TAG_CLOUD_PHOTO_ALBUM_ID).ToString()
                End If
            End If

            If (Settings.Contains(Constants.SETTING_TAG_CLOUD_MAX_COUNT)) Then
                txtMaxCount.Text = CType(Settings(Constants.SETTING_TAG_CLOUD_MAX_COUNT), String)
            Else
                txtMaxCount.Text = Constants.DEFAULT_TAG_CLOUD_MAX_COUNT.ToString()
            End If

        End Sub

        Private Sub SaveSettings()

            Dim objModuleController As New ModuleController

            If (drpModuleID.Items.Count > 0) Then

                Dim values As String() = drpModuleID.SelectedValue.Split(Convert.ToChar("-"))

                If (values.Length = 2) Then
                    objModuleController.UpdateTabModuleSetting(Me.TabModuleId, Constants.SETTING_TAG_CLOUD_PHOTO_TAB_ID, values(0))
                    objModuleController.UpdateTabModuleSetting(Me.TabModuleId, Constants.SETTING_TAG_CLOUD_PHOTO_MODULE_ID, values(1))

                    Dim objModule As ModuleInfo = objModuleController.GetModule(Convert.ToInt32(values(1)), Convert.ToInt32(values(0)))
                    If Not (objModule Is Nothing) Then
                        objModuleController.UpdateTabModuleSetting(Me.TabModuleId, Constants.SETTING_TAG_CLOUD_PHOTO_TAB_MODULE_ID, objModule.TabModuleID.ToString())
                    End If

                End If

            End If

            objModuleController.UpdateTabModuleSetting(Me.TabModuleId, Constants.SETTING_TAG_CLOUD_PHOTO_ALBUM_ID, drpAlbums.SelectedValue)
            objModuleController.UpdateTabModuleSetting(Me.TabModuleId, Constants.SETTING_TAG_CLOUD_MAX_COUNT, txtMaxCount.Text)

        End Sub

#End Region

#Region " Event Handlers "

        Private Sub Page_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load

            Try
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

#End Region

#Region " Base Method Implementations "

        Public Overrides Sub LoadSettings()

            Try

                If (Page.IsPostBack = False) Then

                    BindModules()
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