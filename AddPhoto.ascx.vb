'
' Simple Gallery for DotNetNuke -  http://www.dotnetnuke.com
' Copyright (c) 2002-2007
' by Ventrian ( sales@ventrian.com ) ( http://www.ventrian.com )
'

Imports System.IO
Imports System.Linq
Imports DotNetNuke.Common
Imports DotNetNuke.Common.Utilities
Imports DotNetNuke.Entities.Portals
Imports DotNetNuke.Entities.Users
Imports DotNetNuke.Security.Roles
Imports DotNetNuke.Services.Exceptions
Imports DotNetNuke.Services.Localization

Imports Ventrian.SimpleGallery.Common
Imports Ventrian.SimpleGallery.Entities
Imports DotNetNuke.Security
Imports DotNetNuke.Security.Permissions
Imports DotNetNuke.Services.FileSystem
Imports DotNetNuke.Services.Log.EventLog
Imports Ventrian.ImageResizer

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
                            Dim objUsers As IList(Of UserInfo) = RoleController.Instance.GetUsersByRole(Me.PortalId, objRole.RoleName)
                            For Each objUser As UserInfo In objUsers
                                If (userList.Contains(objUser.UserID) = False) Then
                                    If (objUser.Email.Length > 0) Then
                                        userList.Add(objUser.UserID, objUser.Email)
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

                            litBatchID.Value = System.Guid.NewGuid().ToString()
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

                    Response.Redirect(EditUrl("AlbumID", _albumID.ToString(), "Add", "BatchID=" & litBatchID.Value), True)

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

#Region "Upload foto's"

        Private Function ExtractFileName(ByVal path As String) As String

            Dim extractPos As Integer = path.LastIndexOf("\") + 1
            Return path.Substring(extractPos, path.Length - extractPos)

        End Function

        Private Function ExtractFileExtension(ByVal fileName As String) As String

            Dim extension As String = ""

            If (fileName.Length > 0) Then
                If (fileName.IndexOf("."c) <> -1) Then
                    If (fileName.LastIndexOf("."c) < fileName.Length) Then
                        extension = fileName.Substring(fileName.LastIndexOf("."c) + 1, fileName.Length - (fileName.LastIndexOf("."c) + 1))
                    End If
                End If
            End If

            Return extension

        End Function

        Private Function RemoveExtension(ByVal fileName As String) As String

            Dim name As String = ""

            If (fileName.Length > 0) Then
                If (fileName.IndexOf("."c) <> -1) Then
                    name = fileName.Substring(0, fileName.LastIndexOf("."c))
                End If
            End If

            Return name

        End Function

        Private Function GetFilePath(ByVal albumID As Integer) As String

            Dim filePath As String = ""

            Dim objAlbumController As New AlbumController
            Dim objAlbum As AlbumInfo = objAlbumController.Get(albumID)

            If Not (objAlbum Is Nothing) Then
                filePath = PortalSettings.HomeDirectoryMapPath & objAlbum.HomeDirectory & "\"
            End If

            If Not (Directory.Exists(filePath)) Then
                Directory.CreateDirectory(filePath)
            End If

            Return filePath

        End Function

        Private Sub DnnSyncAlbumFiles(ByVal albumID As Integer)

            Dim objAlbumController As New AlbumController
            Dim objAlbum As AlbumInfo = objAlbumController.Get(albumID)

            If Not (objAlbum Is Nothing) Then
                FolderManager.Instance.Synchronize(PortalId, objAlbum.HomeDirectory, False, True)
            End If

        End Sub

        Private Function IsInRole(ByVal role As String) As Boolean

            Dim _context As HttpContext = HttpContext.Current

            If (role <> "" AndAlso Not role Is Nothing AndAlso ((_context.Request.IsAuthenticated = False And role = glbRoleUnauthUserName))) Then
                Return True
            Else
                Dim roles As String = CType(_context.Items("UserRoles"), String)
                If Not roles Is Nothing Then
                    Dim rolesArr As String() = roles.Split(";"c)
                    For Each strRole As String In rolesArr
                        If strRole = role Then
                            Return True
                        End If
                    Next
                End If
                Return False
            End If

        End Function

        Private Function IsInRoles(ByVal roles As String) As Boolean

            If Not roles Is Nothing Then
                Dim objUserInfo As UserInfo = UserController.Instance.GetCurrentUserInfo()
                Dim role As String

                For Each role In roles.Split(New Char() {";"c})
                    If objUserInfo.IsSuperUser Or (role <> "" AndAlso Not role Is Nothing AndAlso
                                                   (role = glbRoleAllUsersName Or
                                                    IsInRole(role) = True
                                                       )) Then
                        Return True
                    End If
                Next role

            End If

            Return False

        End Function

        Private Function HasApproval() As Boolean

            If (Me.HasEditPermissions Or Me.HasEditPhotoPermissions Or Me.HasApprovePhotoPermissions) Then
                Return True
            End If

            If ModulePermissionController.CanEditModuleContent(ModuleConfiguration) Then
                Return True
            End If

            If (Settings.Contains(Constants.SETTING_EDIT_ROLES)) Then
                If (IsInRoles(Settings(Constants.SETTING_EDIT_ROLES).ToString())) Then
                    Return True
                End If
            End If

            If (Settings.Contains(Constants.SETTING_APPROVE_ROLES)) Then
                If (IsInRoles(Settings(Constants.SETTING_APPROVE_ROLES).ToString())) Then
                    Return True
                End If
            End If

        End Function

        Private Function GetPhotoHeight(ByVal dataItem As Object) As String

            Dim objPhoto As PhotoInfo = CType(dataItem, PhotoInfo)

            If Not (objPhoto Is Nothing) Then
                Dim width As Integer
                If (objPhoto.Width > GallerySettings.ThumbnailWidth) Then
                    width = GallerySettings.ThumbnailWidth
                Else
                    width = objPhoto.Width
                End If

                Dim height As Integer = Convert.ToInt32(objPhoto.Height / (objPhoto.Width / width))
                If (height > GallerySettings.ThumbnailHeight) Then
                    height = GallerySettings.ThumbnailHeight
                    width = Convert.ToInt32(objPhoto.Width / (objPhoto.Height / height))
                End If

                Return height.ToString()
            Else
                Return GallerySettings.ThumbnailWidth.ToString()
            End If

        End Function

        Private Function GetPhotoWidth(ByVal dataItem As Object) As String

            Dim objPhoto As PhotoInfo = CType(dataItem, PhotoInfo)

            If Not (objPhoto Is Nothing) Then
                Dim width As Integer
                If (objPhoto.Width > GallerySettings.ThumbnailWidth) Then
                    width = GallerySettings.ThumbnailWidth
                Else
                    width = objPhoto.Width
                End If

                Dim height As Integer = Convert.ToInt32(objPhoto.Height / (objPhoto.Width / width))
                If (height > GallerySettings.ThumbnailHeight) Then
                    height = GallerySettings.ThumbnailHeight
                    width = Convert.ToInt32(objPhoto.Width / (objPhoto.Height / height))
                End If

                Return width.ToString()
            Else
                Return GallerySettings.ThumbnailWidth.ToString()
            End If

        End Function

        Protected Sub btnUploadFiles_OnClick(sender As Object, e As System.EventArgs)
            If Not fupFile.PostedFiles.Any() Then
                Return
            End If

            Dim objPortalController As New PortalController()

            Try
                Dim filePath As String = GetFilePath(_albumID)

                Dim objQueryString As New NameValueCollection()
                objQueryString.Add("maxwidth", GallerySettings.ImageWidth.ToString())
                objQueryString.Add("maxheight", GallerySettings.ImageHeight.ToString())

                Dim objWatermarkSettings As New WatermarkSettings(Request.QueryString)
                If (GallerySettings.UseWatermark And GallerySettings.WatermarkText <> "") Then
                    objWatermarkSettings.WatermarkText = GallerySettings.WatermarkText
                End If
                If (GallerySettings.UseWatermark And GallerySettings.WatermarkImage <> "") Then
                    objWatermarkSettings.WatermarkImagePath = (PortalSettings.HomeDirectoryMapPath & GallerySettings.WatermarkImage)
                    objWatermarkSettings.WatermarkImagePosition = GallerySettings.WatermarkImagePosition
                End If


                Dim allAddedPhotos As List(Of PhotoInfo)
                If Not IsNothing(addedPhotosRepeater.DataSource) then   
                    allAddedPhotos = CType(addedPhotosRepeater.DataSource, List(Of PhotoInfo))
                Else 
                    allAddedPhotos = New List(Of PhotoInfo)()
                End If

                For Each objFile As HttpPostedFile In fupFile.PostedFiles

                    If (objPortalController.HasSpaceAvailable(PortalId, objFile.ContentLength) = False) Then
                        'TODO Feedback for no space available
                        Exit For
                    End If

                    Dim fileName As String = ExtractFileName(objFile.FileName)
                    Dim fileExtension As String = ExtractFileExtension(fileName)
                    Dim fileNameWithoutExtension As String = RemoveExtension(fileName).Replace("/", "_").Replace(".", "_").Replace("%", "_").Replace("+", "")

                    fileName = fileNameWithoutExtension & "." & fileExtension

                    objFile.SaveAs(filePath & fileName)

                    Dim resize As Boolean = False
                    Dim photo As Drawing.Image = Drawing.Image.FromStream(objFile.InputStream)

                    If (GallerySettings.ImageWidth < photo.Width Or GallerySettings.ImageHeight < photo.Height) Then
                        resize = True
                    End If

                    Dim target As String = filePath & fileName
                    If (resize And GallerySettings.ResizePhoto) Then
                        ImageManager.getBestInstance().BuildImage(filePath & fileName, target, objQueryString, objWatermarkSettings)
                    End If

                    Dim width As Integer = photo.Width
                    Dim height As Integer = photo.Height

                    If (GallerySettings.ResizePhoto) Then

                        If (width > GallerySettings.ImageWidth) Then
                            width = GallerySettings.ImageWidth
                            height = Convert.ToInt32(height / (photo.Width / GallerySettings.ImageWidth))
                        End If

                        If (height > GallerySettings.ImageHeight) Then
                            height = GallerySettings.ImageHeight
                            width = Convert.ToInt32(photo.Width / (photo.Height / GallerySettings.ImageHeight))
                        End If

                    End If

                    photo.Dispose()


                    Dim objPhoto As New PhotoInfo
                    Dim objPhotoController As New PhotoController

                    objPhoto.Name = RemoveExtension(ExtractFileName(objFile.FileName))
                    objPhoto.ModuleID = ModuleId
                    objPhoto.AlbumID = _albumID
                    objPhoto.AuthorID = UserId
                    objPhoto.DateCreated = DateTime.Now
                    objPhoto.DateUpdated = objPhoto.DateCreated

                    objPhoto.FileName = fileNameWithoutExtension & "." & fileExtension
                    objPhoto.Width = width
                    objPhoto.Height = height

                    Try
                        If GallerySettings.UseXmpExif Then
                            Dim objXmpReader As New Entities.MetaData.XmpReader()
                            objXmpReader.ApplyAttributes(objPhoto, objFile.InputStream)
                        End If
                    Catch
                        ' Many things can go wrong here, so just ignore if we can't extract XMP data.
                    End Try

                    DataCache.RemoveCache("SG-Album-Zip-" & _albumID)

                    ' Clear Zip Cache
                    Dim objAlbumController As New AlbumController()
                    Dim objAlbum As AlbumInfo = objAlbumController.Get(_albumID)

                    While (objAlbum IsNot Nothing)
                        DataCache.RemoveCache("SG-Album-Zip-" & objAlbum.AlbumID.ToString())
                        objAlbum = objAlbumController.Get(objAlbum.ParentAlbumID)
                    End While

                    If (GallerySettings.PhotoModeration) Then
                        If (HasApproval()) Then
                            objPhoto.IsApproved = True
                            objPhoto.DateApproved = objPhoto.DateCreated
                            objPhoto.ApproverID = UserId
                        Else
                            objPhoto.IsApproved = False
                            objPhoto.DateApproved = Null.NullDate
                            objPhoto.ApproverID = Null.NullInteger
                        End If
                    Else
                        objPhoto.IsApproved = True
                        objPhoto.DateApproved = objPhoto.DateCreated
                        objPhoto.ApproverID = UserId
                    End If

                    objPhoto.BatchID = _batchID
                    objPhoto.PhotoID = objPhotoController.Add(objPhoto)

                    If (objPhoto.Tags <> "") Then
                        Dim tags As String() = objPhoto.Tags.Split(","c)
                        For Each tag As String In tags
                            If (tag <> "") Then
                                Dim objTagController As New TagController
                                Dim objTag As TagInfo = objTagController.Get(ModuleId, tag.ToLower())

                                If (objTag Is Nothing) Then
                                    objTag = New TagInfo
                                    objTag.ModuleID = ModuleId
                                    objTag.Name = tag
                                    objTag.NameLowered = tag.ToLower()
                                    objTag.Usages = 0
                                    objTag.TagID = objTagController.Add(objTag)
                                End If

                                objTagController.Add(objPhoto.PhotoID, objTag.TagID)
                            End If
                        Next
                    End If

                    ' Re-get the photo to get AlbumPath.
                    objPhoto = objPhotoController.Get(objPhoto.PhotoID)

                    allAddedPhotos.Add(objPhoto)

                    'If (GallerySettings.Compression = CompressionType.MinSize) Then
                    '    Response.Write(Me.ResolveUrl("ImageHandler.ashx?width=" & GetPhotoWidth(CType(objPhoto, Object)) & "&height=" & GetPhotoHeight(CType(objPhoto, Object)) & "&HomeDirectory=" & System.Uri.EscapeDataString(DotNetNuke.Common.Globals.ApplicationPath + "/" + PortalSettings.HomeDirectory + "/" & objPhoto.HomeDirectory) & "&fileName=" & System.Uri.EscapeDataString(objPhoto.FileName) & "&portalid=" & PortalId.ToString() & "&i=" & objPhoto.PhotoID))
                    'Else
                    '    Response.Write(Me.ResolveUrl("ImageHandler.ashx?width=" & GetPhotoWidth(CType(objPhoto, Object)) & "&height=" & GetPhotoHeight(CType(objPhoto, Object)) & "&HomeDirectory=" & System.Uri.EscapeDataString(DotNetNuke.Common.Globals.ApplicationPath + "/" + PortalSettings.HomeDirectory + "/" & objPhoto.HomeDirectory) & "&fileName=" & System.Uri.EscapeDataString(objPhoto.FileName) & "&portalid=" & PortalId.ToString() & "&i=" & objPhoto.PhotoID & "&q=1"))
                    'End If

                    Try

                        ' Update DNN File Meta Info
                        Dim strFileName As String = Path.GetFileName(filePath & fileName)
                        Dim strFolderpath As String = GetSubFolderPath(filePath & fileName, PortalId)
                        Dim finfo As New System.IO.FileInfo(filePath & fileName)

                        Dim strContentType As String = ""
                        Dim strExtension As String = Path.GetExtension(fileName).Replace(".", "")

                        Select Case strExtension
                            Case "jpg", "jpeg" : strContentType = "image/jpeg"
                            Case "gif" : strContentType = "image/gif"
                            Case "png" : strContentType = "image/png"
                            Case Else : strContentType = "application/octet-stream"
                        End Select

                        Dim folderID As Integer = Null.NullInteger
                        Dim folder As FolderInfo = FolderManager.Instance.GetFolder(PortalId, strFolderpath)

                        If (folder Is Nothing) Then
                            folder = FolderManager.Instance.AddFolder(PortalId, strFolderpath)
                        End If
                        folderID = folder.FolderID

                        'Dim parentFolderPath As String = strFolderpath.Substring(0, strFolderpath.Substring(0, strFolderpath.Length - 1).LastIndexOf("/") + 1)

                        ''Get Parents permissions
                        'Dim objFolderPermissionController As New FolderPermissionController
                        'Dim objFolderPermissions As FolderPermissionCollection
                        'objFolderPermissions = objFolderPermissionController.GetFolderPermissionsCollectionByFolderPath(_portalID, parentFolderPath)

                        ''Iterate parent permissions to see if permisison has already been added
                        'For Each objPermission As FolderPermissionInfo In objFolderPermissions
                        '    FileSystemUtils.SetFolderPermission(_portalID, folderID, objPermission.PermissionID, objPermission.RoleID, objPermission.UserID, parentFolderPath)
                        'Next
                    Catch
                    End Try

                Next

                'DnnSyncAlbumFiles(_albumID)

                addedPhotosRepeater.DataSource = allAddedPhotos
                addedPhotosRepeater.DataBind()

            Catch exc As Exception    'Module failed to load
                Dim objEventLog As New EventLogController
                If (exc.InnerException IsNot Nothing) Then
                    objEventLog.AddLog("GalleryUploaderException", exc.InnerException.ToString(), PortalSettings, -1, EventLogController.EventLogType.ADMIN_ALERT)
                End If
                objEventLog.AddLog("GalleryUploaderException", exc.ToString(), PortalSettings, -1, EventLogController.EventLogType.ADMIN_ALERT)
            End Try


        End Sub
#End Region

        Protected Sub addedPhotosRepeater_OnItemDataBound(sender As Object, e As RepeaterItemEventArgs)
            If e.Item.ItemType = ListItemType.Item OrElse e.Item.ItemType = ListItemType.AlternatingItem Then
                Dim objPhoto As PhotoInfo = CType(e.Item.DataItem, PhotoInfo)
                Dim img As Image = CType(e.Item.FindControl("addedPhoto"), Image)

                img.ImageUrl = Me.ResolveUrl("ImageHandler.ashx?width=" & GetPhotoWidth(e.Item.DataItem) & "&height=" & GetPhotoHeight(e.Item.DataItem) & "&HomeDirectory=" & System.Uri.EscapeDataString(DotNetNuke.Common.Globals.ApplicationPath + "/" + PortalSettings.HomeDirectory + "/" & objPhoto.HomeDirectory) & "&fileName=" & System.Uri.EscapeDataString(objPhoto.FileName) & "&portalid=" & PortalId.ToString() & "&i=" & objPhoto.PhotoID)
            End If
        End Sub
    End Class

End Namespace