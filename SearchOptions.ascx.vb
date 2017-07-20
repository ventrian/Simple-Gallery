Imports DotNetNuke.Common.Utilities
Imports DotNetNuke.Entities.Modules
Imports DotNetNuke.Entities.Tabs
Imports DotNetNuke.Security
Imports DotNetNuke.Services.Exceptions
Imports DotNetNuke.Services.Localization

Imports Ventrian.SimpleGallery.Common
Imports Ventrian.SimpleGallery.Entities

Namespace Ventrian.SimpleGallery

    Partial Public Class SearchOptions
        Inherits ModuleSettingsBase

#Region " Private Methods "

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

        End Sub

        Private Sub BindSettings()

            If (Settings.Contains(Constants.SETTING_SEARCH_MODULE_ID) And Settings.Contains(Constants.SETTING_SEARCH_TAB_ID)) Then
                If Not (drpModuleID.Items.FindByValue(Settings(Constants.SETTING_SEARCH_TAB_ID).ToString() & "-" & Settings(Constants.SETTING_SEARCH_MODULE_ID).ToString()) Is Nothing) Then
                    drpModuleID.SelectedValue = Settings(Constants.SETTING_SEARCH_TAB_ID).ToString() & "-" & Settings(Constants.SETTING_SEARCH_MODULE_ID).ToString()
                End If
            End If

            If (Settings.Contains(Constants.SETTING_SEARCH_TEMPLATE)) Then
                txtSearchTemplate.Text = Settings(Constants.SETTING_SEARCH_TEMPLATE).ToString()
            Else
                txtSearchTemplate.Text = Constants.DEFAULT_SEARCH_TEMPLATE
            End If

        End Sub

        Private Sub SaveSettings()

            Dim objModuleController As New ModuleController

            If (drpModuleID.Items.Count > 0) Then

                Dim values As String() = drpModuleID.SelectedValue.Split(Convert.ToChar("-"))

                If (values.Length = 2) Then
                    objModuleController.UpdateTabModuleSetting(Me.TabModuleId, Constants.SETTING_SEARCH_TAB_ID, values(0))
                    objModuleController.UpdateTabModuleSetting(Me.TabModuleId, Constants.SETTING_SEARCH_MODULE_ID, values(1))

                    Dim objModule As ModuleInfo = objModuleController.GetModule(Convert.ToInt32(values(1)), Convert.ToInt32(values(0)))
                    If Not (objModule Is Nothing) Then
                        objModuleController.UpdateTabModuleSetting(Me.TabModuleId, Constants.SETTING_SEARCH_TAB_MODULE_ID, objModule.TabModuleID.ToString())
                    End If

                End If

            End If

            objModuleController.UpdateTabModuleSetting(Me.TabModuleId, Constants.SETTING_SEARCH_TEMPLATE, txtSearchTemplate.Text)

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
