'
' Simple Gallery for DotNetNuke -  http://www.dotnetnuke.com
' Copyright (c) 2002-2007
' by Ventrian ( sales@ventrian.com ) ( http://www.ventrian.com )
'

Imports System.Drawing
Imports System.Drawing.Drawing2D
Imports System.Drawing.Imaging
Imports System.IO
Imports System.Threading

Imports DotNetNuke.Common
Imports DotNetNuke.Common.Utilities
Imports DotNetNuke.Services.Exceptions
Imports DotNetNuke.Services.Localization

Imports Ventrian.SimpleGallery.Common
Imports Ventrian.SimpleGallery.Entities
Imports DotNetNuke.Security.Roles
Imports DotNetNuke.Entities.Modules

Namespace Ventrian.SimpleGallery

    Partial Public Class EditAlbum
        Inherits SimpleGalleryBase

#Region " Private Members "

        Private _albumID As Integer = Null.NullInteger
        Private _returnUrl As String = Null.NullString
        Private _selAlbumID As Integer = Null.NullInteger

        ' Because the Me.UserInfo and Me.PortalSettings properties are all inherited from PortalModuleBase as ReadOnly, and 
        ' they are unable to serialize for the thread's queue execution, we define these as local variables and use their values during the 
        ' threaded sync.
        Private _threadUserInfo As DotNetNuke.Entities.Users.UserInfo
        Private _threadPortalSettings As DotNetNuke.Entities.Portals.PortalSettings

#End Region

#Region " Private Methods "

        Private Sub ReadQueryString()

            If Not (Request("AlbumID") Is Nothing) Then
                _albumID = Convert.ToInt32(Request("AlbumID"))
            End If

            If Not (Request("ReturnUrl") Is Nothing) Then
                _returnUrl = Request("ReturnUrl")
            End If

            If (Request("SelAlbumID") <> "") Then
                _selAlbumID = Convert.ToInt32(Request("SelAlbumID"))
            End If

        End Sub

        Private Sub BindDefaultPhotos()

            If (_albumID <> Null.NullInteger) Then
                Dim objPhotoController As New PhotoController
                drpDefaultPhoto.DataSource = objPhotoController.List(Me.ModuleId, _albumID, True, Null.NullInteger, Null.NullBoolean, Null.NullInteger, Null.NullString, Null.NullString, Me.GallerySettings.SortBy, Me.GallerySettings.SortDirection)
                drpDefaultPhoto.DataBind()
            End If

            drpDefaultPhoto.Items.Insert(0, New ListItem(Localization.GetString("NoneSpecified", Me.LocalResourceFile), "-1"))

        End Sub

        Private Sub BindAlbums()

            Dim objAlbumController As New AlbumController

            drpParentAlbum.DataSource = objAlbumController.List(Me.ModuleId, Null.NullInteger, False, True, AlbumSortType.Caption, SortDirection.ASC)
            drpParentAlbum.DataBind()

            drpParentAlbum.Items.Insert(0, New System.Web.UI.WebControls.ListItem(Localization.GetString("NoParentAlbum", Me.LocalResourceFile), "-1"))

        End Sub

        Private Sub BindAlbum()

            If (_albumID = Null.NullInteger) Then

                pnlSynch.Visible = False
                trDefaultPhoto.Visible = False
                txtHomeDirectory.Text = GallerySettings.AlbumDefaultPath
                txtCreateDate.Text = DateTime.Now.ToShortDateString()
                drpCreateTimeHour.SelectedValue = DateTime.Now.Hour.ToString()
                drpCreateTimeMinute.SelectedValue = DateTime.Now.Minute.ToString()
                pnlDelete.Visible = False
                cmdSynchronize.Visible = False

                If (_selAlbumID <> Null.NullInteger) Then
                    If Not (drpParentAlbum.Items.FindByValue(_selAlbumID.ToString()) Is Nothing) Then
                        drpParentAlbum.SelectedValue = _selAlbumID.ToString()
                    End If
                End If

            Else

                pnlSynch.Visible = True
                trDefaultPhoto.Visible = True
                cmdCustomize.Visible = False

                pnlDelete.Visible = True
                cmdDelete.Attributes.Add("onClick", "javascript:return confirm('" & Localization.GetString("Confirmation", LocalResourceFile) & "');")

                cmdSynchronize.Visible = True
                cmdSynchronize.Attributes.Add("onClick", "javascript:return confirm('" & Localization.GetString("SynchronizeWarning", LocalResourceFile) & "');")

                Dim objAlbumController As New AlbumController
                Dim objAlbum As AlbumInfo = objAlbumController.Get(_albumID)

                If Not (objAlbum Is Nothing) Then
                    Dim li As ListItem = drpParentAlbum.Items.FindByValue(objAlbum.ParentAlbumID.ToString())
                    If Not (li Is Nothing) Then
                        li.Selected = True
                    End If
                    txtCaption.Text = objAlbum.Caption
                    txtDescription.Text = objAlbum.Description
                    chkIsPublic.Checked = objAlbum.IsPublic
                    txtHomeDirectory.Text = objAlbum.HomeDirectory
                    If (objAlbum.CreateDate <> Null.NullDate) Then
                        txtCreateDate.Text = objAlbum.CreateDate.ToShortDateString()
                        drpCreateTimeHour.SelectedValue = objAlbum.CreateDate.Hour.ToString()
                        drpCreateTimeMinute.SelectedValue = objAlbum.CreateDate.Minute.ToString()
                    Else
                        txtCreateDate.Text = DateTime.Now.ToShortDateString()
                        drpCreateTimeHour.SelectedValue = DateTime.Now.Hour.ToString()
                        drpCreateTimeMinute.SelectedValue = DateTime.Now.Minute.ToString()
                    End If
                    chkInheritSecurity.Checked = objAlbum.InheritSecurity

                    If (chkInheritSecurity.Checked = False) Then
                        trPermissions.Visible = True
                    End If

                    Dim objPhotoController As New PhotoController
                    Dim objPhoto As PhotoInfo = objPhotoController.GetFirstFromAlbum(objAlbum.AlbumID, Me.ModuleId)

                    If Not (objPhoto Is Nothing) Then
                        If (objPhoto.IsDefault) Then
                            If Not (drpDefaultPhoto.Items.FindByValue(objPhoto.PhotoID.ToString()) Is Nothing) Then
                                drpDefaultPhoto.SelectedValue = objPhoto.PhotoID.ToString()
                            End If
                        End If
                    End If
                End If

            End If

        End Sub

        Private Sub BindCrumbs()

            ucGalleryMenu.AddCrumb(Localization.GetString("AllAlbums", LocalResourceFile), NavigateURL())
            ucGalleryMenu.AddCrumb(Localization.GetString("EditAlbums", LocalResourceFile), EditUrl("EditAlbums"))
            ucGalleryMenu.AddCrumb(Localization.GetString("EditAlbum", LocalResourceFile), EditUrl("AlbumID", _albumID.ToString(), "EditAlbum"))

        End Sub

        Private Sub BindRoles()

            Dim objRole As New RoleController
            Dim availableRoles As New ArrayList
            Dim roles As ArrayList = objRole.GetPortalRoles(PortalId)

            If Not roles Is Nothing Then
                For Each Role As RoleInfo In roles
                    availableRoles.Add(New ListItem(Role.RoleName, Role.RoleName))
                Next
            End If

            grdAlbumPermissions.DataSource = availableRoles
            grdAlbumPermissions.DataBind()

        End Sub

        Public Sub DeleteAlbumAndSubalbums(ByVal albumID As Integer, ByVal homeDirectoryMapPath As String, ByVal deleteFiles As Boolean)
            ' -- Recursion pattern

            ' If subalbums, delete those first
            Dim objAlbumController As New AlbumController
            Dim albumArrayList As System.Collections.ArrayList = objAlbumController.List(Me.ModuleId, albumID, False, False, AlbumSortType.Caption, SortDirection.ASC)
            For Each albumInArrayList As AlbumInfo In albumArrayList
                DeleteAlbumAndSubalbums(albumInArrayList.AlbumID, homeDirectoryMapPath, deleteFiles)   ' Recursive call
            Next

            ' Delete photos in current album
            Dim objPhotoController As New PhotoController
            Dim photos As System.Collections.ArrayList = objPhotoController.List(Me.ModuleId, albumID, True, Null.NullInteger, Null.NullBoolean, Null.NullInteger, Null.NullString, Null.NullString, Me.GallerySettings.SortBy, Me.GallerySettings.SortDirection)
            For Each photo As PhotoInfo In photos
                If (deleteFiles) Then
                    If (File.Exists(homeDirectoryMapPath & photo.HomeDirectory & "\" & photo.FileName)) Then
                        File.Delete(homeDirectoryMapPath & photo.HomeDirectory & "\" & photo.FileName)
                    End If
                End If
                objPhotoController.Delete(photo.PhotoID)
            Next

            If (deleteFiles) Then
                ' Delete this album
                Dim directoryPath As String
                directoryPath = homeDirectoryMapPath.Replace("/", "\") + objAlbumController.Get(albumID).HomeDirectory.Replace("/", "\")
                If (Directory.Exists(directoryPath)) Then
                    Dim files As String() = Directory.GetFiles(directoryPath)
                    Dim directories As String() = Directory.GetDirectories(directoryPath)
                    If ((files.Length < 1) And (directories.Length < 1)) Then
                        Directory.Delete(directoryPath)
                    End If
                End If
            End If
            objAlbumController.Delete(albumID)
        End Sub

        Private Function IsValidImage(ByVal fileName As String) As Boolean

            Select Case fileName.Substring(fileName.LastIndexOf(".")).ToLower
                Case ".gif", ".jpg", ".jpeg", ".png"
                    Return True
                Case Else
                    Return False
            End Select

        End Function

        Private Function GetImageType(ByVal fileName As String) As Imaging.ImageFormat

            Select Case fileName.Substring(fileName.LastIndexOf(".")).ToLower
                Case ".gif"
                    Return Imaging.ImageFormat.Gif
                Case ".jpg", ".jpeg"
                    Return Imaging.ImageFormat.Jpeg
                Case ".png"
                    Return Imaging.ImageFormat.Png
                Case Else
                    Return Imaging.ImageFormat.Jpeg
            End Select

        End Function

        Private Sub SynchAlbum(ByVal albumID As Integer, ByRef _albumCount As Integer, ByRef _photoCount As Integer)

            Dim objAlbumController As New AlbumController
            Dim objAlbum As AlbumInfo = objAlbumController.Get(albumID)

            ' Add album if it doesn't exist
            If objAlbum Is Nothing Then
                Return
            End If

            Dim path As String = _threadPortalSettings.HomeDirectory & objAlbum.HomeDirectory
            _albumCount = _albumCount + 1

            ' Remove Photos
            Dim objPhotoController As New PhotoController
            Dim objPhotos As ArrayList = objPhotoController.List(Me.ModuleId, objAlbum.AlbumID, True, Null.NullInteger, Null.NullBoolean, Null.NullInteger, Null.NullString, Null.NullString, Me.GallerySettings.SortBy, Me.GallerySettings.SortDirection)

            ' Remove Photos from DB if no longer on drive or if doing absolute sync
            For Each objPhotoToDelete As PhotoInfo In objPhotos
                If ((chkAbsoluteSync.Checked) Or (Not File.Exists(_threadPortalSettings.HomeDirectoryMapPath & objPhotoToDelete.HomeDirectory & "\" & objPhotoToDelete.FileName))) Then
                    objPhotoController.Delete(objPhotoToDelete.PhotoID)
                End If
            Next
            ' Refresh my working list
            objPhotos = objPhotoController.List(Me.ModuleId, objAlbum.AlbumID, True, Null.NullInteger, Null.NullBoolean, Null.NullInteger, Null.NullString, Null.NullString, Me.GallerySettings.SortBy, Me.GallerySettings.SortDirection)
            Dim mapPath As String = Server.MapPath(path)

            If (Directory.Exists(mapPath) = False) Then
                Directory.CreateDirectory(mapPath)
            End If

            Dim files As String() = Directory.GetFiles(mapPath)

            For Each file As String In files

                If (IsValidImage(file)) Then

                    ' Per MSDN, Drawing.Image must be disposed when done using it. 
                    Using photo As Drawing.Image = Drawing.Image.FromFile(file)

                        Dim Width As Integer = photo.Width
                        Dim Height As Integer = photo.Height

                        If (chkResizeImages.Checked) Then

                            If (Width > GallerySettings.ImageWidth) Then
                                Width = GallerySettings.ImageWidth
                                Height = Convert.ToInt32(Height / (photo.Width / GallerySettings.ImageWidth))
                            End If

                            If (Height > GallerySettings.ImageHeight) Then
                                Height = GallerySettings.ImageHeight
                                Width = Convert.ToInt32(photo.Width / (photo.Height / GallerySettings.ImageHeight))
                            End If

                            Dim bmp As New Bitmap(Width, Height)
                            Dim g As Graphics = Graphics.FromImage(DirectCast(bmp, System.Drawing.Image))

                            If (GallerySettings.Compression = CompressionType.Quality) Then
                                g.InterpolationMode = InterpolationMode.HighQualityBicubic
                                g.SmoothingMode = SmoothingMode.HighQuality
                                g.PixelOffsetMode = PixelOffsetMode.HighQuality
                                g.CompositingQuality = CompositingQuality.HighQuality
                            End If

                            Try
                                Dim tempFileName As String = file & "_" & DateTime.Now.ToString("yyyyMMddHHmmss")
                                If (System.IO.File.Exists(tempFileName) = False) Then
                                    g.DrawImage(photo, 0, 0, Width, Height)
                                    photo.Dispose()

                                    If (GallerySettings.Compression = CompressionType.Quality) Then
                                        Dim info As ImageCodecInfo() = ImageCodecInfo.GetImageEncoders()
                                        Dim params As New EncoderParameters
                                        params.Param(0) = New EncoderParameter(Encoder.Quality, 100L)
                                        bmp.Save(tempFileName, info(1), params)
                                    Else
                                        bmp.Save(tempFileName, GetImageType(file))
                                    End If
                                    bmp.Dispose()

                                    System.IO.File.Delete(file)
                                    System.IO.File.Move(tempFileName, file)
                                Else
                                    photo.Dispose()
                                End If
                            Catch

                            End Try

                        End If

                        Dim objPhoto As PhotoInfo = PhotoInfoByFileName(objPhotos, _threadPortalSettings.HomeDirectoryMapPath, file)
                        If (objPhoto Is Nothing) Then
                            objPhoto = New PhotoInfo()
                            objPhoto.ModuleID = Me.ModuleId
                            objPhoto.Name = file.Substring(file.LastIndexOf("\")).TrimStart(Convert.ToChar("\"))
                            objPhoto.FileName = file.Substring(file.LastIndexOf("\")).TrimStart(Convert.ToChar("\"))
                            objPhoto.DateCreated = DateTime.Now
                            objPhoto.DateUpdated = DateTime.Now
                            objPhoto.Width = Width
                            objPhoto.Height = Height
                            objPhoto.AlbumID = objAlbum.AlbumID
                            objPhoto.IsApproved = True
                            objPhoto.AuthorID = _threadUserInfo.UserID
                            objPhoto.ApproverID = _threadUserInfo.UserID
                            objPhoto.DateApproved = objPhoto.DateCreated

                            objPhotoController.Add(objPhoto)
                        Else
                            objPhoto.Width = Width
                            objPhoto.Height = Height
                            objPhotoController.Update(objPhoto)
                        End If
                        _photoCount = _photoCount + 1
                    End Using

                End If

            Next

            If (chkIncludeSubFolders.Checked) Then
                ' Process sub-folders

                ' For each subfolder in the current folder, add to database if not there already
                Dim subFolders As String() = Directory.GetDirectories(Server.MapPath(path))
                If (subFolders.Length > 0) Then
                    For Each folder As String In subFolders
                        Dim objAlbumToAdd As New AlbumInfo
                        Dim directory As New DirectoryInfo(folder)
                        ' Check to see of the path of the current folder is in the DB... if not, then add it.
                        Dim subAlbumId As Integer
                        Dim foundAlbumInfo As AlbumInfo
                        foundAlbumInfo = (objAlbumController.GetByPath(Me.ModuleId, folder.Replace(_threadPortalSettings.HomeDirectoryMapPath, "").Replace("\", "/")))
                        If (foundAlbumInfo Is Nothing) Then
                            objAlbumToAdd.Caption = directory.Name
                            objAlbumToAdd.ModuleID = Me.ModuleId
                            objAlbumToAdd.IsPublic = True
                            objAlbumToAdd.Description = ""
                            objAlbumToAdd.ParentAlbumID = objAlbum.AlbumID
                            objAlbumToAdd.HomeDirectory = folder.Replace(_threadPortalSettings.HomeDirectoryMapPath, "").Replace("\", "/")
                            objAlbumToAdd.InheritSecurity = True
                            subAlbumId = objAlbumController.Add(objAlbumToAdd)
                        Else
                            subAlbumId = foundAlbumInfo.AlbumID
                        End If
                    Next
                End If
                ' Delete subalbums from DB if not found on physical drive; otherwise,  sync it
                Dim albumArrayList As ArrayList
                albumArrayList = objAlbumController.List(Me.ModuleId, objAlbum.AlbumID, False, False, AlbumSortType.Caption, SortDirection.ASC)
                For Each albumInArrayList As AlbumInfo In albumArrayList
                    ' check to see if folder exists... if not, then remove from DB
                    If (Not Directory.Exists(_threadPortalSettings.HomeDirectoryMapPath.Replace("/", "\") + albumInArrayList.HomeDirectory.Replace("/", "\"))) Then
                        DeleteAlbumAndSubalbums(albumInArrayList.AlbumID, _threadPortalSettings.HomeDirectoryMapPath, False)
                    Else
                        SynchAlbum(albumInArrayList.AlbumID, _albumCount, _photoCount)
                    End If
                Next
            End If

        End Sub

        Private Function IsInRole(ByVal roleName As String, ByVal roles As String()) As Boolean

            For Each role As String In roles
                If (roleName = role) Then
                    Return True
                End If
            Next

            Return False

        End Function

        Private Sub QueueSynchAlbum(ByVal state As Object)
            ' 
            Dim _albumCount As Integer = 0
            Dim _photoCount As Integer = 0
            Dim editAlbumState As EditAlbum
            editAlbumState = CType(state, EditAlbum)
            Dim mutexKey As String
            mutexKey = Thread.CurrentPrincipal.Identity.Name
            mutexKey.Replace("\"c, "_"c) ' Required, otherwise mutex will consider it a file system path
            Dim mMutex As Mutex
            mMutex = New Mutex(False, mutexKey + "_SynchAlbum")
            mMutex.WaitOne()    ' Make sure only one synch is done at a time... otherwise, wait.
            Try
                editAlbumState.SynchAlbum(_albumID, _albumCount, _photoCount)
                Dim subject As String = "Synchronization completed successfully"
                Dim body As String = "The synchronization completed successfully: " + vbCrLf + String.Format("{0} album(s) and {1} photo(s) synchronized.", _albumCount.ToString(), _photoCount.ToString())
                Dim eventLogControl As DotNetNuke.Services.Log.EventLog.EventLogController
                eventLogControl = New DotNetNuke.Services.Log.EventLog.EventLogController()
                eventLogControl.AddLog(subject, body, Me._threadPortalSettings, Me._threadUserInfo.UserID, DotNetNuke.Services.Log.EventLog.EventLogController.EventLogType.ADMIN_ALERT)
                DotNetNuke.Services.Mail.Mail.SendMail(Me._threadPortalSettings.Email, Me._threadUserInfo.Email, "", subject, body, "", "", "", "", "", "")
            Catch exception As Exception
                Dim subject As String = "Synchronization completed unsuccessfully"
                Dim body As String = "The synchronization completed unsuccessfully: " & vbCrLf & exception.Message
                Dim eventLogControl As DotNetNuke.Services.Log.EventLog.EventLogController
                eventLogControl = New DotNetNuke.Services.Log.EventLog.EventLogController()
                eventLogControl.AddLog(subject, body, Me._threadPortalSettings, Me._threadUserInfo.UserID, DotNetNuke.Services.Log.EventLog.EventLogController.EventLogType.ADMIN_ALERT)
                DotNetNuke.Services.Mail.Mail.SendMail(Me._threadPortalSettings.Email, Me._threadUserInfo.Email, "", subject, body, "", "", "", "", "", "")
            Finally
                mMutex.ReleaseMutex()
            End Try
        End Sub

        Private Function PhotoInfoByFileName(ByRef objPhotos As ArrayList, ByVal homeDirectoryMapPath As String, ByVal fileName As String) As PhotoInfo
            Dim returnValue As PhotoInfo = Nothing
            For Each objPhoto As PhotoInfo In objPhotos
                If (objPhoto.FileName = System.IO.Path.GetFileName(fileName)) Then
                    returnValue = objPhoto
                    Exit For
                End If
            Next
            Return returnValue
        End Function

#End Region

#Region " Event Handlers "

        Private Sub Page_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load

            If (HasAlbumPermissions() = False) Then
                Response.Redirect(NavigateURL(), True)
            End If

            Try

                ReadQueryString()
                BindCrumbs()

                cmdCreateCalendar.NavigateUrl = DotNetNuke.Common.Utilities.Calendar.InvokePopupCal(txtCreateDate)

                If (IsPostBack = False) Then

                    BindDefaultPhotos()
                    BindAlbums()
                    BindAlbum()
                    BindRoles()

                    txtCaption.Focus()

                End If

            Catch exc As Exception    'Module failed to load
                ProcessModuleLoadException(Me, exc)
            End Try

        End Sub

        Private Sub cmdUpdate_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cmdUpdate.Click

            Try

                If (Page.IsValid) Then

                    Dim objAlbumController As New AlbumController

                    Dim objAlbum As New AlbumInfo

                    If (_albumID <> Null.NullInteger) Then

                        objAlbum = objAlbumController.Get(_albumID)
                        objAlbum.HomeDirectory = txtHomeDirectory.Text

                    Else

                        objAlbum = CType(CBO.InitializeObject(objAlbum, GetType(AlbumInfo)), AlbumInfo)
                        objAlbum.HomeDirectory = txtHomeDirectory.Text

                    End If

                    objAlbum.ParentAlbumID = Convert.ToInt32(drpParentAlbum.SelectedValue)
                    objAlbum.ModuleID = Me.ModuleId
                    objAlbum.Caption = txtCaption.Text
                    objAlbum.IsPublic = chkIsPublic.Checked
                    If (txtDescription.Text.Length > 2000) Then
                        objAlbum.Description = txtDescription.Text.Substring(0, 2000)
                    Else
                        objAlbum.Description = txtDescription.Text
                    End If
                    objAlbum.CreateDate = Convert.ToDateTime(txtCreateDate.Text).AddHours(Convert.ToInt32(drpCreateTimeHour.SelectedValue)).AddMinutes(Convert.ToInt32(drpCreateTimeMinute.SelectedValue))
                    objAlbum.InheritSecurity = chkInheritSecurity.Checked

                    If (_albumID = Null.NullInteger) Then

                        Dim objAlbums As ArrayList = objAlbumController.List(Me.ModuleId, objAlbum.ParentAlbumID, False, False, AlbumSortType.Caption, SortDirection.ASC)
                        objAlbum.AlbumOrder = objAlbums.Count

                        Dim newId As Integer = objAlbumController.Add(objAlbum)

                        objAlbum = objAlbumController.Get(newId)
                        objAlbum.HomeDirectory = objAlbum.HomeDirectory.Replace("[AlbumID]", newId.ToString())
                        objAlbumController.Update(objAlbum)

                        objAlbum.AlbumID = newId

                    Else

                        objAlbumController.Update(objAlbum)

                        Dim objPhotoController As New PhotoController
                        objPhotoController.SetDefaultPhoto(Convert.ToInt32(drpDefaultPhoto.SelectedValue), objAlbum.AlbumID)

                    End If

                    Dim addRoles As String = ""
                    Dim editRoles As String = ""
                    Dim deleteRoles As String = ""

                    If (chkInheritSecurity.Checked = False) Then

                        For Each item As DataGridItem In grdAlbumPermissions.Items
                            Dim role As String = grdAlbumPermissions.DataKeys(item.ItemIndex).ToString()

                            Dim chkAdd As CheckBox = CType(item.FindControl("chkAdd"), CheckBox)
                            If (chkAdd.Checked) Then
                                If (addRoles = "") Then
                                    addRoles = role
                                Else
                                    addRoles = addRoles & ";" & role
                                End If
                            End If

                            Dim chkEdit As CheckBox = CType(item.FindControl("chkEdit"), CheckBox)
                            If (chkEdit.Checked) Then
                                If (editRoles = "") Then
                                    editRoles = role
                                Else
                                    editRoles = editRoles & ";" & role
                                End If
                            End If

                            Dim chkDelete As CheckBox = CType(item.FindControl("chkDelete"), CheckBox)
                            If (chkDelete.Checked) Then
                                If (deleteRoles = "") Then
                                    deleteRoles = role
                                Else
                                    deleteRoles = deleteRoles & ";" & role
                                End If
                            End If
                        Next

                        Dim objModuleController As New ModuleController()
                        objModuleController.UpdateModuleSetting(Me.ModuleId, objAlbum.AlbumID.ToString() & "-" & Constants.SETTING_PERMISSION_ADD_ALBUM, addRoles)
                        objModuleController.UpdateModuleSetting(Me.ModuleId, objAlbum.AlbumID.ToString() & "-" & Constants.SETTING_PERMISSION_EDIT_ALBUM, editRoles)
                        objModuleController.UpdateModuleSetting(Me.ModuleId, objAlbum.AlbumID.ToString() & "-" & Constants.SETTING_PERMISSION_DELETE_ALBUM, deleteRoles)

                    Else

                        Dim objModuleController As New ModuleController()
                        objModuleController.UpdateModuleSetting(Me.ModuleId, objAlbum.AlbumID.ToString() & "-" & Constants.SETTING_PERMISSION_ADD_ALBUM, addRoles)
                        objModuleController.UpdateModuleSetting(Me.ModuleId, objAlbum.AlbumID.ToString() & "-" & Constants.SETTING_PERMISSION_EDIT_ALBUM, editRoles)
                        objModuleController.UpdateModuleSetting(Me.ModuleId, objAlbum.AlbumID.ToString() & "-" & Constants.SETTING_PERMISSION_DELETE_ALBUM, deleteRoles)

                    End If



                    ' Create album folder if not already existing
                    If Not (Directory.Exists(PortalSettings.HomeDirectoryMapPath & objAlbum.HomeDirectory)) Then
                        Directory.CreateDirectory(PortalSettings.HomeDirectoryMapPath & objAlbum.HomeDirectory)
                    End If

                    If (_returnUrl <> "") Then
                        Response.Redirect(_returnUrl, True)
                    Else
                        Response.Redirect(EditUrl("EditAlbums"), True)
                    End If

                End If

            Catch exc As Exception    'Module failed to load
                ProcessModuleLoadException(Me, exc)
            End Try

        End Sub

        Private Sub cmdDelete_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cmdDelete.Click

            Try

                Dim objPhotoController As New PhotoController
                Dim photos As ArrayList = objPhotoController.List(Me.ModuleId, _albumID, True, Null.NullInteger, Null.NullBoolean, Null.NullInteger, Null.NullString, Null.NullString, Me.GallerySettings.SortBy, Me.GallerySettings.SortDirection)

                ' Delete album, and also delete all child albums than have it as a parent.
                DeleteAlbumAndSubalbums(_albumID, PortalSettings.HomeDirectoryMapPath, chkDeletePhysicalFiles.Checked)

                If (_returnUrl <> "") Then
                    Response.Redirect(_returnUrl, True)
                Else
                    Response.Redirect(EditUrl("EditAlbums"), True)
                End If

            Catch exc As Exception    'Module failed to load
                ProcessModuleLoadException(Me, exc)
            End Try

        End Sub

        Private Sub cmdCancel_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cmdCancel.Click

            Try

                If (_returnUrl <> "") Then
                    Response.Redirect(_returnUrl, True)
                Else
                    Response.Redirect(EditUrl("EditAlbums"), True)
                End If

            Catch exc As Exception    'Module failed to load
                ProcessModuleLoadException(Me, exc)
            End Try

        End Sub

        Private Sub cmdSynchronize_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cmdSynchronize.Click

            Try

                _threadPortalSettings = Me.PortalSettings
                _threadUserInfo = Me.UserInfo

                If (chkAsynchronous.Checked) Then
                    ' Submit the synchronization to a thread for processing so the user won't have to wait... an email will be
                    ' sent to the user upon failed or successful completion.
                    Try
                        If (Not ThreadPool.QueueUserWorkItem(New WaitCallback(AddressOf QueueSynchAlbum), Me)) Then
                            Throw New Exception("ThreadPool.QueueUserWorkItem returned code indicating failure to queue.")
                        End If
                        'ThreadPool.QueueUserWorkItem(New WaitCallback(AddressOf QueueSynchAlbum), Me)
                        lblSynchResults.Text = Localization.GetString("BackgroundSynch", Me.LocalResourceFile)
                        lblSynchResults.Visible = True
                        lblSynchResults.ForeColor = Color.Red
                    Catch ex As Exception
                        lblSynchResults.Text = String.Format(Localization.GetString("SynchException", Me.LocalResourceFile), ex.Message)
                        lblSynchResults.Visible = True
                        lblSynchResults.ForeColor = Color.Red
                    End Try
                Else
                    Dim albumCount As Integer = 0
                    Dim photoCount As Integer = 0
                    SynchAlbum(_albumID, albumCount, photoCount)
                    Try
                        lblSynchResults.Text = String.Format(Localization.GetString("SynchComplete", Me.LocalResourceFile), albumCount.ToString(), photoCount.ToString())
                        lblSynchResults.Visible = True
                        lblSynchResults.ForeColor = Color.Green
                    Catch ex As Exception
                        lblSynchResults.Text = String.Format(Localization.GetString("SynchException", Me.LocalResourceFile), ex.Message)
                        lblSynchResults.Visible = True
                        lblSynchResults.ForeColor = Color.Red
                    End Try
                End If

            Catch exc As Exception    'Module failed to load
                ProcessModuleLoadException(Me, exc)
            End Try

        End Sub

        Private Sub cmdCustomize_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cmdCustomize.Click

            Try

                txtHomeDirectory.Enabled = Not txtHomeDirectory.Enabled

                If (txtHomeDirectory.Enabled) Then
                    txtHomeDirectory.Text = ""
                Else
                    txtHomeDirectory.Text = "Gallery/Album/[AlbumID]"
                End If

            Catch exc As Exception    'Module failed to load
                ProcessModuleLoadException(Me, exc)
            End Try

        End Sub

        Private Sub valInvalidParentAlbum_ServerValidate(ByVal source As System.Object, ByVal args As System.Web.UI.WebControls.ServerValidateEventArgs) Handles valInvalidParentAlbum.ServerValidate

            Try

                If (_albumID = Null.NullInteger Or drpParentAlbum.SelectedValue = "-1") Then
                    args.IsValid = True
                    Return
                End If

                Dim objAlbumController As New AlbumController
                Dim objAlbum As AlbumInfo = objAlbumController.Get(Convert.ToInt32(drpParentAlbum.SelectedValue))

                While Not objAlbum Is Nothing
                    If (_albumID = objAlbum.AlbumID) Then
                        args.IsValid = False
                        Return
                    End If
                    objAlbum = objAlbumController.Get(objAlbum.ParentAlbumID)
                End While

                args.IsValid = True

            Catch exc As Exception    'Module failed to load
                ProcessModuleLoadException(Me, exc)
            End Try

        End Sub

        Private Sub grdAlbumPermissions_ItemDataBound(ByVal sender As System.Object, ByVal e As System.Web.UI.WebControls.DataGridItemEventArgs) Handles grdAlbumPermissions.ItemDataBound

            Try

                If (e.Item.ItemType = ListItemType.Item Or e.Item.ItemType = ListItemType.AlternatingItem) Then
                    Dim objListItem As ListItem = CType(e.Item.DataItem, ListItem)

                    If Not (objListItem Is Nothing) Then

                        Dim role As String = CType(e.Item.DataItem, ListItem).Value

                        Dim chkAdd As CheckBox = CType(e.Item.FindControl("chkAdd"), CheckBox)
                        Dim chkEdit As CheckBox = CType(e.Item.FindControl("chkEdit"), CheckBox)
                        Dim chkDelete As CheckBox = CType(e.Item.FindControl("chkDelete"), CheckBox)

                        If (objListItem.Value = PortalSettings.AdministratorRoleName.ToString()) Then
                            chkAdd.Enabled = False
                            chkAdd.Checked = True
                            chkEdit.Enabled = False
                            chkEdit.Checked = True
                            chkDelete.Enabled = False
                            chkDelete.Checked = True
                        Else
                            If (_albumID <> Null.NullInteger) Then
                                If (Settings.Contains(_albumID.ToString() & "-" & Constants.SETTING_PERMISSION_ADD_ALBUM)) Then
                                    chkAdd.Checked = IsInRole(role, Settings(_albumID.ToString() & "-" & Constants.SETTING_PERMISSION_ADD_ALBUM).ToString().Split(";"c))
                                End If
                                If (Settings.Contains(_albumID.ToString() & "-" & Constants.SETTING_PERMISSION_EDIT_ALBUM)) Then
                                    chkEdit.Checked = IsInRole(role, Settings(_albumID.ToString() & "-" & Constants.SETTING_PERMISSION_EDIT_ALBUM).ToString().Split(";"c))
                                End If
                                If (Settings.Contains(_albumID.ToString() & "-" & Constants.SETTING_PERMISSION_DELETE_ALBUM)) Then
                                    chkDelete.Checked = IsInRole(role, Settings(_albumID.ToString() & "-" & Constants.SETTING_PERMISSION_DELETE_ALBUM).ToString().Split(";"c))
                                End If
                            End If
                        End If

                    End If

                End If

            Catch exc As Exception    'Module failed to load
                ProcessModuleLoadException(Me, exc)
            End Try

        End Sub

        Protected Sub chkInheritSecurity_CheckedChanged(ByVal sender As Object, ByVal e As EventArgs) Handles chkInheritSecurity.CheckedChanged

            Try

                trPermissions.Visible = Not chkInheritSecurity.Checked

            Catch exc As Exception    'Module failed to load
                ProcessModuleLoadException(Me, exc)
            End Try

        End Sub

#End Region

    End Class

End Namespace