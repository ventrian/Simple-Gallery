'
' Simple Gallery for DotNetNuke -  http://www.dotnetnuke.com
' Copyright (c) 2002-2007
' by Ventrian ( sales@ventrian.com ) ( http://www.ventrian.com )
'

Imports DotNetNuke.Common
Imports DotNetNuke.Common.Utilities
Imports DotNetNuke.Entities.Users
Imports DotNetNuke.Security.Roles
Imports DotNetNuke.Services.Exceptions
Imports DotNetNuke.Services.Localization

Imports Ventrian.SimpleGallery.Common
Imports Ventrian.SimpleGallery.Entities
Imports DotNetNuke.Security

Namespace Ventrian.SimpleGallery

    Partial Public Class AddPhoto
        Inherits SimpleGalleryBase

#Region " Private Members "

        Private _albumID As Integer = Null.NullInteger
        Private _selAlbumID As Integer = Null.NullInteger
        Private _returnUrl As String = Null.NullString
        Private _batchID As String = Null.NullString

#End Region

#Region " Private Methods "

        Private Sub BindAlbums()

            Dim objAlbumController As New AlbumController

            Dim objAlbums As ArrayList = objAlbumController.List(Me.ModuleId, GallerySettings.AlbumFilter, Not HasEditPermissions(), True, AlbumSortType.Caption, SortDirection.ASC)
            Dim objAlbumsSelected As New ArrayList()

            For Each objAlbum As AlbumInfo In objAlbums
                If (objAlbum.InheritSecurity) Then
                    objAlbumsSelected.Add(objAlbum)
                Else
                    If (Settings.Contains(objAlbum.AlbumID & "-" & Constants.SETTING_PERMISSION_ADD_ALBUM)) Then
                        If (PortalSecurity.IsInRoles(Settings(objAlbum.AlbumID & "-" & Constants.SETTING_PERMISSION_ADD_ALBUM).ToString())) Then
                            objAlbumsSelected.Add(objAlbum)
                        End If
                    End If
                End If
            Next

            If (GallerySettings.AlbumFilter <> Null.NullInteger) Then
                For Each objAlbum As AlbumInfo In objAlbumsSelected
                    objAlbum.CaptionIndented = ".." & objAlbum.CaptionIndented
                Next
                Dim objAlbumFilter As AlbumInfo = objAlbumController.Get(GallerySettings.AlbumFilter)
                If Not (objAlbumFilter Is Nothing) Then
                    objAlbumFilter.CaptionIndented = objAlbumFilter.Caption
                    If (objAlbumFilter.InheritSecurity) Then
                        objAlbumsSelected.Insert(0, objAlbumFilter)
                    Else
                        If (Settings.Contains(objAlbumFilter.AlbumID & "-" & Constants.SETTING_PERMISSION_ADD_ALBUM)) Then
                            If (PortalSecurity.IsInRoles(Settings(objAlbumFilter.AlbumID & "-" & Constants.SETTING_PERMISSION_ADD_ALBUM).ToString())) Then
                                objAlbumsSelected.Insert(0, objAlbumFilter)
                            End If
                        End If
                    End If
                End If
            End If

            drpAlbums.DataSource = objAlbumsSelected
            drpAlbums.DataBind()

            If (Me.HasUploadPhotoPermissions = True And Me.HasAlbumPermissions = False) Then
                ' Only Upload Permissions
                If (drpAlbums.Items.Count = 0) Then
                    If (_returnUrl <> "") Then
                        Response.Redirect(_returnUrl, True)
                    Else
                        Response.Redirect(NavigateURL(), True)
                    End If
                End If
            Else
                drpAlbums.Items.Insert(0, New ListItem(Localization.GetString("SelectNewAlbum", Me.LocalResourceFile), "-1"))
            End If

            If (_selAlbumID <> Null.NullInteger) Then
                If Not (drpAlbums.Items.FindByValue(_selAlbumID.ToString()) Is Nothing) Then
                    drpAlbums.SelectedValue = _selAlbumID.ToString()
                End If
            End If

            drpParentAlbum.DataSource = objAlbums
            drpParentAlbum.DataBind()

            drpParentAlbum.Items.Insert(0, New ListItem(Localization.GetString("NoParentAlbum", Me.LocalResourceFile), "-1"))

            If (drpParentAlbum.Items.Count = 1) Then
                trParentAlbum.Visible = False
            End If

        End Sub

        Private Sub BindBreadCrumbs()

            ucGalleryMenu.AddCrumb(Localization.GetString("AllAlbums", LocalResourceFile), NavigateURL())
            ucGalleryMenu.AddCrumb(Localization.GetString("AddNewPhoto", LocalResourceFile), EditUrl("Add"))

        End Sub

        Private Function GetApproverDistributionList() As Hashtable

            Dim userList As Hashtable = New Hashtable

            If (Me.Settings.Contains(Constants.SETTING_APPROVE_ROLES)) Then

                Dim roles As String = Settings(Constants.SETTING_APPROVE_ROLES).ToString()
                Dim rolesArray() As String = roles.Split(Convert.ToChar(";"))

                For Each role As String In rolesArray
                    If (role.Length > 0) Then
                        Dim objRoleController As RoleController = New RoleController
                        Dim objRole As RoleInfo = objRoleController.GetRoleByName(Me.PortalId, role)

                        If Not (objRole Is Nothing) Then
                            Dim objUsers As ArrayList = objRoleController.GetUserRolesByRoleName(Me.PortalId, objRole.RoleName)
                            For Each objUser As UserRoleInfo In objUsers
                                If (userList.Contains(objUser.UserID) = False) Then
                                    Dim objUserController As UserController = New UserController
                                    Dim objSelectedUser As UserInfo = objUserController.GetUser(Me.PortalId, objUser.UserID)
                                    If Not (objSelectedUser Is Nothing) Then
                                        If (objSelectedUser.Membership.Email.Length > 0) Then
                                            userList.Add(objUser.UserID, objSelectedUser.Membership.Email)
                                        End If
                                    End If
                                End If
                            Next
                        End If
                    End If
                Next

            End If

            Return userList

        End Function

        Private Sub ReadQueryString()

            If (Request("AlbumID") <> "") Then
                _albumID = Convert.ToInt32(Request("AlbumID"))
            End If

            If (Request("SelAlbumID") <> "") Then
                _selAlbumID = Convert.ToInt32(Request("SelAlbumID"))
            End If

            If (Request("ReturnUrl") <> "") Then
                _returnUrl = Server.UrlDecode(Request("ReturnUrl"))
            End If

            If (Request("BatchID") <> "") Then
                _batchID = Request("BatchID")
            End If

        End Sub

        Private Sub SecurityCheck()

            If (Me.HasUploadPhotoPermissions) = False Then
                Response.Redirect(NavigateURL, True)
            End If

        End Sub


#End Region

#Region " Protected Methods "

        Protected Function GetMaximumFileSize() As String

            Return GallerySettings.UploaderFileSize.ToString()

        End Function

        Protected Function GetUploadUrl() As String

            Return Page.ResolveUrl("~/DesktopModules/SimpleGallery/Uploader.aspx?PortalID=" & Me.PortalId.ToString())

        End Function

#End Region

#Region " Event Handlers "

        Private Sub Page_PreRender(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.PreRender

            Try

                If (HttpContext.Current.Items("SimpleGallery-ScriptsRegistered") Is Nothing) Then

                    If (HttpContext.Current.Items("jquery_registered") Is Nothing And HttpContext.Current.Items("jQueryRequested") Is Nothing And GallerySettings.IncludeJQuery) Then

                        Dim version As Integer = Convert.ToInt32(PortalSettings.Version.Split("."c)(0))
                        If (version < 6) Then

                            Dim litLink As New Literal
                            litLink.Text = "" & vbCrLf _
                                & "<script type=""text/javascript"" src='" & Me.ResolveUrl("js/lightbox/jquery.js?v=" & GallerySettings.JavascriptVersion) & "'></script>"
                            Page.Header.Controls.Add(litLink)
                        End If

                    End If

                    HttpContext.Current.Items.Add("SimpleGallery-ScriptsRegistered", "true")

                End If

            Catch exc As Exception    'Module failed to load
                ProcessModuleLoadException(Me, exc)
            End Try

        End Sub

        Private Sub Page_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load

            Try

                SecurityCheck()
                ReadQueryString()
                BindBreadCrumbs()

                litModuleID.Text = Me.ModuleId.ToString()

                If Me.GallerySettings.PhotoModeration AndAlso (Me.HasEditPermissions() Or Me.HasEditPhotoPermissions Or Me.HasApprovePhotoPermissions) = False Then
                    lblRequiresApproval.Visible = True
                End If

                If (IsPostBack = False) Then

                    If (_albumID = Null.NullInteger) Then

                        BindAlbums()

                        If (drpAlbums.Items.Count = 0) Then
                            phStep1a.Visible = False
                            rdoCreateNew.Checked = True
                        Else
                            rdoSelectExisting.Checked = True
                            If (drpAlbums.Items.Count = 2) Then
                                drpAlbums.SelectedIndex = 1
                            End If
                        End If

                        If (Me.HasAlbumPermissions() = False) Then
                            phStep1b.Visible = False
                        End If
                        txtCaption.Focus()

                        pnlStep1.Visible = True
                        pnlStep2.Visible = False
                        ucEditPhotos.Visible = False

                        pnlWizard.Visible = True
                        pnlSave.Visible = False

                        imgPrevious.Visible = False
                        cmdPrevious.Visible = False

                        imgStep.ImageUrl = "~/DesktopModules/SimpleGallery/Images/iconStep1.gif"
                        lblStep.Text = Localization.GetString("Step1", Me.LocalResourceFile)
                        lblStepDescription.Text = Localization.GetString("Step1Description", Me.LocalResourceFile)

                    Else

                        If (_batchID = Null.NullString) Then

                            pnlStep1.Visible = False
                            pnlStep2.Visible = True
                            ucEditPhotos.Visible = False

                            pnlWizard.Visible = True
                            pnlSave.Visible = False

                            imgStep.ImageUrl = "~/DesktopModules/SimpleGallery/Images/iconStep2.gif"
                            lblStep.Text = Localization.GetString("Step2", Me.LocalResourceFile)
                            lblStepDescription.Text = Localization.GetString("Step2Description", Me.LocalResourceFile)

                            litBatchID.Text = System.Guid.NewGuid().ToString()
                            litTicketID.Text = Request.Cookies(System.Web.Security.FormsAuthentication.FormsCookieName()).Value

                        Else

                            pnlStep1.Visible = False
                            pnlStep2.Visible = False
                            ucEditPhotos.Visible = True

                            pnlWizard.Visible = False
                            pnlSave.Visible = True

                            imgStep.ImageUrl = "~/DesktopModules/SimpleGallery/Images/iconStep3.gif"
                            lblStep.Text = Localization.GetString("Step3", Me.LocalResourceFile)
                            lblStepDescription.Text = Localization.GetString("Step3Description", Me.LocalResourceFile)

                            ucEditPhotos.BatchID = _batchID
                            ucEditPhotos.BindPhotos()

                        End If

                    End If

                End If

            Catch exc As Exception    'Module failed to load
                ProcessModuleLoadException(Me, exc)
            End Try

        End Sub

        Private Sub MoveNext()

            If (Page.IsValid) Then

                If (pnlStep1.Visible) Then

                    If (drpAlbums.SelectedValue <> "-1" And rdoCreateNew.Checked = False) Then

                        If (_returnUrl <> "") Then
                            Response.Redirect(EditUrl("AlbumID", drpAlbums.SelectedValue, "Add", "ReturnUrl=" & System.Uri.EscapeDataString(_returnUrl)), True)
                        Else
                            Response.Redirect(EditUrl("AlbumID", drpAlbums.SelectedValue, "Add"), True)
                        End If

                    Else

                        Dim objAlbumController As New AlbumController
                        Dim objAlbum As New AlbumInfo

                        objAlbum.ModuleID = Me.ModuleId
                        objAlbum.ParentAlbumID = Convert.ToInt32(drpParentAlbum.SelectedValue)
                        objAlbum.Caption = txtCaption.Text
                        objAlbum.Description = txtDescription.Text
                        objAlbum.IsPublic = True
                        objAlbum.HomeDirectory = GallerySettings.AlbumDefaultPath

                        objAlbum.AlbumID = objAlbumController.Add(objAlbum)
                        objAlbum.HomeDirectory = GallerySettings.AlbumDefaultPath.ToLower.Replace("[albumid]", objAlbum.AlbumID.ToString())
                        objAlbumController.Update(objAlbum)

                        If (_returnUrl <> "") Then
                            Response.Redirect(EditUrl("AlbumID", objAlbum.AlbumID.ToString(), "Add", "ReturnUrl=" & System.Uri.EscapeDataString(_returnUrl)), True)
                        Else
                            Response.Redirect(EditUrl("AlbumID", objAlbum.AlbumID.ToString(), "Add"), True)
                        End If

                    End If

                ElseIf (pnlStep2.Visible) Then

                    Response.Redirect(EditUrl("AlbumID", _albumID.ToString(), "Add", "BatchID=" & litBatchID.Text), True)

                End If

            End If

        End Sub

        Private Sub MoveCancel()

            If (_returnUrl <> "") Then
                Response.Redirect(_returnUrl, True)
            Else
                Response.Redirect(NavigateURL(), True)
            End If

        End Sub

        Private Sub MovePrevious()

            If (pnlStep2.Visible) Then

                If (_returnUrl <> "") Then
                    Response.Redirect(EditUrl("ReturnUrl", System.Uri.EscapeDataString(_returnUrl), "Add"), True)
                Else
                    Response.Redirect(EditUrl("Add"), True)
                End If

            End If

        End Sub

        Private Sub Save()

            If (Page.IsValid) Then

                ucEditPhotos.Save()

                If (Me.HasEditPermissions() Or Me.HasEditPhotoPermissions Or Me.HasApprovePhotoPermissions) = False Then
                    ' Send notification to approvers

                    Dim objPhotoController As New PhotoController

                    Dim objPhotos As ArrayList = objPhotoController.List(Me.ModuleId, Null.NullInteger, True, Null.NullInteger, True, Null.NullInteger, _batchID, Null.NullString, SortType.Name, SortDirection.ASC)

                    If (objPhotos.Count > 0) Then
                        Dim approvers As Hashtable = GetApproverDistributionList()

                        For Each email As String In approvers.Values
                            Dim subject As String = Localization.GetString("EMAIL_PHOTO_SUBMIT_BULK_SUBJECT", Me.LocalResourceFile)
                            Dim body As String = Localization.GetString("EMAIL_PHOTO_SUBMIT_BULK_BODY", Me.LocalResourceFile)

                            subject = subject.Replace("[PORTALNAME]", Me.PortalSettings.PortalName)

                            body = body.Replace("[PORTALNAME]", Me.PortalSettings.PortalName)
                            body = body.Replace("[LINK]", NavigateURL(Me.TabId, "", "AlbumID=" & Me.ModuleId.ToString() & "-" & _albumID.ToString()))

                            DotNetNuke.Services.Mail.Mail.SendMail(PortalSettings.Email, email, "", subject, body, "", "", "", "", "", "")
                        Next
                    End If
                End If

                Response.Redirect(NavigateURL(Me.TabId, "", "AlbumID=" & Me.ModuleId.ToString() & "-" & _albumID.ToString()), True)

            End If

        End Sub

        Private Sub cmdNext_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cmdNext.Click, imgNext.Click

            MoveNext()

        End Sub

        Private Sub cmdCancel_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cmdCancel.Click, imgCancel.Click

            MoveCancel()

        End Sub

        Private Sub imgPrevious_Click(ByVal sender As System.Object, ByVal e As System.Web.UI.ImageClickEventArgs) Handles imgPrevious.Click

            MovePrevious()

        End Sub

        Private Sub cmdPrevious_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cmdPrevious.Click

            MovePrevious()

        End Sub

        Private Sub imgSave_Click(ByVal sender As System.Object, ByVal e As System.Web.UI.ImageClickEventArgs) Handles imgSave.Click

            Save()

        End Sub

        Private Sub cmdSave_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cmdSave.Click

            Save()

        End Sub

        Protected Sub valSelectNew_ServerValidate(ByVal source As Object, ByVal args As System.Web.UI.WebControls.ServerValidateEventArgs) Handles valSelectNew.ServerValidate

            args.IsValid = False

            If (rdoSelectExisting.Checked) Then
                args.IsValid = True
            Else
                If (txtCaption.Text.Trim() <> "") Then
                    args.IsValid = True
                End If
            End If

        End Sub

        Protected Sub valSelectExisting_ServerValidate(ByVal source As Object, ByVal args As System.Web.UI.WebControls.ServerValidateEventArgs) Handles valSelectExisting.ServerValidate

            args.IsValid = False

            If (rdoCreateNew.Checked) Then
                args.IsValid = True
            Else
                If (drpAlbums.Items.Count > 0 AndAlso drpAlbums.SelectedValue <> "-1") Then
                    args.IsValid = True
                End If
            End If

        End Sub

#End Region

    End Class

End Namespace