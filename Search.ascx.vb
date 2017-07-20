Imports DotNetNuke.Common
Imports DotNetNuke.Common.Utilities
Imports DotNetNuke.Services.Exceptions
Imports DotNetNuke.Services.Localization

Imports Ventrian.SimpleGallery.Common

Namespace Ventrian.SimpleGallery

    Partial Public Class Search
        Inherits SimpleGalleryBase

#Region " Private Members "

        Private _tabID As Integer = Null.NullInteger
        Private _moduleID As Integer = Null.NullInteger
        Private _tabModuleID As Integer = Null.NullInteger

#End Region

#Region " Private Methods "

        Private Sub BindSearch()

            If (Settings.Contains(Constants.SETTING_SEARCH_TAB_ID)) Then
                _tabID = Convert.ToInt32(Settings(Constants.SETTING_SEARCH_TAB_ID).ToString())
            End If

            If (Settings.Contains(Constants.SETTING_SEARCH_MODULE_ID)) Then
                _moduleID = Convert.ToInt32(Settings(Constants.SETTING_SEARCH_MODULE_ID).ToString())
            End If

            If (Settings.Contains(Constants.SETTING_SEARCH_TAB_MODULE_ID)) Then
                _tabModuleID = Convert.ToInt32(Settings(Constants.SETTING_SEARCH_TAB_MODULE_ID).ToString())
            End If

            If (_moduleID <> Null.NullInteger) Then

                Dim delimStr As String = "[]"
                Dim delimiter As Char() = delimStr.ToCharArray()

                Dim template As String = Me.GallerySettings.SearchTemplate
                Dim templateTokens As String() = Me.GallerySettings.SearchTemplate.Split(delimiter)

                For iPtr As Integer = 0 To templateTokens.Length - 1 Step 2

                    pnlSearch.Controls.Add(New LiteralControl(templateTokens(iPtr).ToString()))

                    If iPtr < templateTokens.Length - 1 Then

                        Select Case templateTokens(iPtr + 1)

                            Case "BUTTON"
                                Dim objButton As New Button
                                objButton.ID = Globals.CreateValidID("SearchButton-" & ModuleId.ToString() & "-" & iPtr.ToString())
                                objButton.CssClass = "Normal"
                                objButton.ValidationGroup = "Search-" & ModuleId.ToString()
                                objButton.Text = Localization.GetString("Search", Me.LocalResourceFile)
                                AddHandler objButton.Click, AddressOf Search_OnClick
                                pnlSearch.DefaultButton = objButton.ID
                                pnlSearch.Controls.Add(objButton)

                            Case "LINKBUTTON"
                                Dim objLinkButton As New LinkButton
                                objLinkButton.ID = Globals.CreateValidID("SearchButton-" & ModuleId.ToString() & "-" & iPtr.ToString())
                                objLinkButton.CssClass = "CommandButton"
                                objLinkButton.ValidationGroup = "Search-" & ModuleId.ToString()
                                objLinkButton.Text = Localization.GetString("Search", Me.LocalResourceFile)
                                AddHandler objLinkButton.Click, AddressOf Search_OnClick
                                pnlSearch.DefaultButton = objLinkButton.ID
                                pnlSearch.Controls.Add(objLinkButton)

                            Case "TEXTBOX"
                                Dim objTextBox As New TextBox
                                objTextBox.ID = Globals.CreateValidID("SearchText-" & ModuleId.ToString() & "-" & iPtr.ToString())
                                objTextBox.CssClass = "Normal"
                                objTextBox.ValidationGroup = "Search-" & ModuleId.ToString()
                                If (Request("SearchText") <> "" And Request("SearchID") = _tabModuleID.ToString()) Then
                                    objTextBox.Text = Server.HtmlEncode(Request("SearchText"))
                                End If
                                pnlSearch.Controls.Add(objTextBox)

                        End Select

                    End If
                Next

            Else

                Dim objLabel As New Label
                objLabel.ID = Globals.CreateValidID("Search-" & Me.ModuleId.ToString())
                objLabel.CssClass = "Normal"
                objLabel.Text = Localization.GetString("Configure", Me.LocalResourceFile)
                pnlSearch.Controls.Add(objLabel)

            End If

        End Sub

#End Region

#Region " Event Handlers "

        Private Sub Page_Init(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Init

            Try

                BindSearch()

            Catch exc As Exception
                ProcessModuleLoadException(Me, exc)
            End Try

        End Sub

        Private Sub Search_OnClick(ByVal sender As System.Object, ByVal e As System.EventArgs)

            Try

                Dim searchText As String = ""
                For Each objControl As Control In pnlSearch.Controls
                    If (objControl IsNot Nothing) Then
                        If (objControl.ID <> "" AndAlso objControl.ID.StartsWith("SearchText")) Then
                            Dim objTextBox As TextBox = CType(objControl, TextBox)
                            searchText = objTextBox.Text
                        End If
                    End If
                Next

                If (searchText.Trim() <> "") Then
                    Response.Redirect(NavigateURL(_tabID, "", "SearchID=" & _tabModuleID.ToString(), "SearchText=" & Server.UrlEncode(searchText)), True)
                End If

            Catch exc As Exception
                ProcessModuleLoadException(Me, exc)
            End Try

        End Sub

#End Region

    End Class

End Namespace
