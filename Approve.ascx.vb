'
' Simple Gallery for DotNetNuke -  http://www.dotnetnuke.com
' Copyright (c) 2002-2007
' by Ventrian ( sales@ventrian.com ) ( http://www.ventrian.com )
'

Imports DotNetNuke.Common
Imports DotNetNuke.Common.Utilities
Imports DotNetNuke.Entities.Users
Imports DotNetNuke.Services.Exceptions
Imports DotNetNuke.Services.Localization

Imports Ventrian.SimpleGallery.Common
Imports Ventrian.SimpleGallery.Entities

Namespace Ventrian.SimpleGallery

    Partial Public Class Approve
        Inherits SimpleGalleryBase

#Region " Private Members "

        Private _albumID As Integer = Null.NullInteger

#End Region

#Region " Private Methods "

        Private Sub CheckSecurity()

            If (Me.HasApprovePhotoPermissions = False) Then
                Response.Redirect(NavigateURL, True)
            End If

        End Sub
        Private Sub BindPhotos()

            Dim crumbs As New ArrayList
            Dim viewAlbum As Boolean = False

            Dim crumbAllAlbums As New CrumbInfo
            crumbAllAlbums.Caption = Localization.GetString("AllAlbums", LocalResourceFile)
            crumbAllAlbums.Url = NavigateURL()
            crumbs.Add(crumbAllAlbums)

            Dim objPhotoController As New PhotoController
            Dim objPhotos As ArrayList
            If (GallerySettings.AlbumFilter <> "-1") Then
                objPhotos = objPhotoController.List(Me.ModuleId, Convert.ToInt32(GallerySettings.AlbumFilter), False, Null.NullInteger, Null.NullBoolean, Null.NullInteger, Null.NullString, Null.NullString, SortType.DateCreated, SortDirection.ASC)
            Else
                objPhotos = objPhotoController.List(Me.ModuleId, Null.NullInteger, False, Null.NullInteger, Null.NullBoolean, Null.NullInteger, Null.NullString, Null.NullString, SortType.DateCreated, SortDirection.ASC)
            End If

            dlGallery.DataSource = objPhotos
            dlGallery.DataBind()

            If (dlGallery.Items.Count = 0) Then
                lblNoPhotos.Visible = True
            Else
                lblNoPhotos.Visible = False
            End If

            Dim approveCrumb As New CrumbInfo
            approveCrumb.Caption = Localization.GetString("ApprovePhotos", LocalResourceFile)
            approveCrumb.Url = Request.Url.ToString()
            crumbs.Add(approveCrumb)

            If (crumbs.Count > 1) Then
                rptBreadCrumbs.DataSource = crumbs
                rptBreadCrumbs.DataBind()
            End If

        End Sub

        Private Sub NotifyAuthor(ByVal objPhoto As PhotoInfo, ByVal isApproval As Boolean)

            Dim subject As String = ""
            Dim body As String = ""

            If (isApproval) Then
                subject = Localization.GetString("EMAIL_PHOTO_APPROVAL_SUBJECT", Me.LocalResourceFile)
                body = Localization.GetString("EMAIL_PHOTO_APPROVAL_BODY", Me.LocalResourceFile)
            Else
                subject = Localization.GetString("EMAIL_PHOTO_REJECT_SUBJECT", Me.LocalResourceFile)
                body = Localization.GetString("EMAIL_PHOTO_REJECT_BODY", Me.LocalResourceFile)
            End If

            subject = subject.Replace("[PORTALNAME]", Me.PortalSettings.PortalName)

            body = body.Replace("[PORTALNAME]", Me.PortalSettings.PortalName)

            body = body.Replace("[FULLNAME]", objPhoto.AuthorFirstName & " " & objPhoto.AuthorLastName)
            body = body.Replace("[FIRSTNAME]", objPhoto.AuthorFirstName)
            body = body.Replace("[LASTNAME]", objPhoto.AuthorLastName)
            body = body.Replace("[USERNAME]", objPhoto.AuthorUserName)
            body = body.Replace("[DISPLAYNAME]", objPhoto.AuthorDisplayName)

            body = body.Replace("[LINK]", NavigateURL(Me.TabId, "", "AlbumID=" & Me.ModuleId.ToString() & "-" & objPhoto.AlbumID.ToString()))
            body = body.Replace("[PHOTONAME]", objPhoto.Name)
            If (txtRejectMessage.Text.Trim().Length > 0) Then
                body = body.Replace("[MESSAGE]", txtRejectMessage.Text)
            Else
                body = body.Replace("[MESSAGE]", Localization.GetString("NoReason", Me.LocalResourceFile))
            End If

            Dim objUserController As New UserController
            Dim objUser As UserInfo = objUserController.GetUser(Me.PortalId, objPhoto.AuthorID)

            If Not (objUser Is Nothing) Then
                DotNetNuke.Services.Mail.Mail.SendMail(PortalSettings.Email, objUser.Membership.Email, "", subject, body, "", "", "", "", "", "")
            End If

        End Sub

#End Region

#Region " Protected Methods "

        Protected Function GetPhotoPath(ByVal dataItem As Object) As String

            Dim objPhoto As PhotoInfo = CType(dataItem, PhotoInfo)

            If Not (objPhoto Is Nothing) Then
                Return Me.ResolveUrl("ImageHandler.ashx?width=" & GetPhotoWidth(CType(objPhoto, Object)) & "&height=" & GetPhotoHeight(CType(objPhoto, Object)) & "&HomeDirectory=" & System.Uri.EscapeDataString(Me.PortalSettings.HomeDirectory & objPhoto.HomeDirectory) & "&fileName=" & System.Uri.EscapeDataString(objPhoto.FileName) & "&portalid=" & Me.PortalId.ToString())
            End If

            Return ""

        End Function

        Protected Function GetPhotoWidth(ByVal dataItem As Object) As String

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

        Protected Function GetPhotoHeight(ByVal dataItem As Object) As String

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

        Protected Function GetAlbumPath(ByVal albumID As String, ByVal homeDirectory As String) As String

            Dim objPhotoController As New PhotoController
            Dim objPhoto As PhotoInfo
            objPhoto = objPhotoController.GetFirstFromAlbum(Convert.ToInt32(albumID), Me.ModuleId)

            If Not (objPhoto Is Nothing) Then
                Dim width As Integer
                If (objPhoto.Width > GallerySettings.AlbumThumbnailWidth) Then
                    width = GallerySettings.AlbumThumbnailWidth
                Else
                    width = objPhoto.Width
                End If

                Dim height As Integer = Convert.ToInt32(objPhoto.Height / (objPhoto.Width / width))
                If (height > GallerySettings.AlbumThumbnailHeight) Then
                    height = GallerySettings.AlbumThumbnailHeight
                    width = Convert.ToInt32(objPhoto.Width / (objPhoto.Height / height))
                End If

                If (objPhoto.HomeDirectory = "") Then
                    Return Me.ResolveUrl("ImageHandler.ashx?width=" & width.ToString() & "&height=" & height.ToString() & "&HomeDirectory=" & System.Uri.EscapeDataString(Me.PortalSettings.HomeDirectory & homeDirectory) & "&fileName=" & System.Uri.EscapeDataString(objPhoto.FileName) & "&portalid=" & Me.PortalId.ToString())
                Else
                    Return Me.ResolveUrl("ImageHandler.ashx?width=" & width.ToString() & "&height=" & height.ToString() & "&HomeDirectory=" & System.Uri.EscapeDataString(Me.PortalSettings.HomeDirectory & objPhoto.HomeDirectory) & "&fileName=" & System.Uri.EscapeDataString(objPhoto.FileName) & "&portalid=" & Me.PortalId.ToString())
                End If
            Else
                Dim width As Integer
                If (600 > GallerySettings.AlbumThumbnailWidth) Then
                    width = GallerySettings.AlbumThumbnailWidth
                Else
                    width = 600
                End If

                Dim height As Integer = Convert.ToInt32(450 / (600 / width))
                If (height > GallerySettings.AlbumThumbnailHeight) Then
                    height = GallerySettings.AlbumThumbnailHeight
                    width = Convert.ToInt32(600 / (450 / height))
                End If
                Return Me.ResolveUrl("ImageHandler.ashx?width=" & width.ToString() & "&height=" & height.ToString() & "&fileName=" & System.Uri.EscapeDataString("placeholder-600.jpg") & "&portalid=" & Me.PortalId.ToString())
            End If

        End Function

        Protected Function GetPhotoCount(ByVal dataItem As Object) As String

            Dim objAlbum As AlbumInfo = CType(dataItem, AlbumInfo)

            If Not (objAlbum Is Nothing) Then
                If (objAlbum.NumberOfPhotos > 0) Then
                    If (objAlbum.NumberOfPhotos = 1) Then
                        Return "&nbsp;(" & objAlbum.NumberOfPhotos.ToString() & " " & Localization.GetString("Photo", Me.LocalResourceFile) & ")"
                    Else
                        Return "&nbsp;(" & objAlbum.NumberOfPhotos.ToString() & " " & Localization.GetString("Photos", Me.LocalResourceFile) & ")"
                    End If
                Else
                    Return ""
                End If
            Else
                Return ""
            End If

        End Function

        Protected Function GetAlbumCount(ByVal dataItem As Object) As String

            Dim objAlbum As AlbumInfo = CType(dataItem, AlbumInfo)

            If Not (objAlbum Is Nothing) Then
                If (objAlbum.NumberOfAlbums > 0) Then
                    If (objAlbum.NumberOfAlbumPhotos > 0) Then
                        If (objAlbum.NumberOfAlbums = 1) Then
                            If (objAlbum.NumberOfAlbumPhotos = 1) Then
                                Return "<br>" & objAlbum.NumberOfAlbums.ToString() & " " & Localization.GetString("SubAlbum", Me.LocalResourceFile) & " (" & objAlbum.NumberOfAlbumPhotos.ToString() & " " & Localization.GetString("Photo", Me.LocalResourceFile) & ")"
                            Else
                                Return "<br>" & objAlbum.NumberOfAlbums.ToString() & " " & Localization.GetString("SubAlbum", Me.LocalResourceFile) & " (" & objAlbum.NumberOfAlbumPhotos.ToString() & " " & Localization.GetString("Photos", Me.LocalResourceFile) & ")"
                            End If
                        Else
                            If (objAlbum.NumberOfAlbumPhotos = 1) Then
                                Return "<br>" & objAlbum.NumberOfAlbums.ToString() & " " & Localization.GetString("SubAlbums", Me.LocalResourceFile) & " (" & objAlbum.NumberOfAlbumPhotos.ToString() & " " & Localization.GetString("Photo", Me.LocalResourceFile) & ")"
                            Else
                                Return "<br>" & objAlbum.NumberOfAlbums.ToString() & " " & Localization.GetString("SubAlbums", Me.LocalResourceFile) & " (" & objAlbum.NumberOfAlbumPhotos.ToString() & " " & Localization.GetString("Photos", Me.LocalResourceFile) & ")"
                            End If
                        End If
                    Else
                        If (objAlbum.NumberOfAlbums = 1) Then
                            Return "<br>" & objAlbum.NumberOfAlbums.ToString() & " " & Localization.GetString("SubAlbum", Me.LocalResourceFile)
                        Else
                            Return "<br>" & objAlbum.NumberOfAlbums.ToString() & " " & Localization.GetString("SubAlbums", Me.LocalResourceFile)
                        End If
                    End If
                Else
                    Return ""
                End If
            Else
                Return ""
            End If

        End Function

        Protected Function GetAlbumUrl(ByVal albumID As String) As String

            Return NavigateURL(Me.TabId, "", "AlbumID=" & Me.ModuleId.ToString() & "-" & albumID)

        End Function

        Protected Function GetPhotoUrl(ByVal photoID As String) As String

            If (GallerySettings.Slideshow = SlideshowType.Popup) Then
                Return "javascript:openScreenWin('smallscreen', '" & Me.ResolveUrl("SlideShowPopup.aspx?PortalID=" & Me.PortalId.ToString() & "&ItemID=" & photoID & "&Border=" & GallerySettings.BorderStyle & "&sb=" & GallerySettings.SortBy & "&sd=" & GallerySettings.SortDirection) & "', " & GallerySettings.PopupWidth.ToString() & ", " & GallerySettings.PopupHeight.ToString() & ");"
            Else
                Return EditUrl("ItemID", photoID, "SlideShow")
            End If

        End Function

        Protected Function GetAlternateText(ByVal dataItem As Object) As String

            Dim objPhoto As PhotoInfo = CType(dataItem, PhotoInfo)

            If (objPhoto.Description.Trim().Length > 0) Then
                Return Server.HtmlEncode(objPhoto.Description).Replace(Chr(34), "")
            Else
                Return Server.HtmlEncode(objPhoto.Name).Replace(Chr(34), "")
            End If

        End Function

        Protected Function GetAlternateTextForAlbum(ByVal dataItem As Object) As String

            Dim objAlbum As AlbumInfo = CType(dataItem, AlbumInfo)

            If Not (objAlbum Is Nothing) Then
                Return String.Format(Localization.GetString("AlternateText", Me.LocalResourceFile), objAlbum.Caption)
            Else
                Return ""
            End If

        End Function

        Protected Function IsPhotoEditable() As Boolean

            Return (Me.HasEditPhotoPermissions Or Me.HasDeletePhotoPermissions)

        End Function

        Protected Function IsAlbumEditable() As Boolean

            Return Me.HasEditPermissions

        End Function

#End Region

#Region " Event Handlers "

        Private Sub Page_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load

            Try
                CheckSecurity()
                If (Page.IsPostBack = False) Then
                    BindPhotos()
                End If
            Catch exc As Exception    'Module failed to load
                ProcessModuleLoadException(Me, exc)
            End Try

        End Sub

        Private Sub cmdApproveSelected_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cmdApproveSelected.Click

            For Each item As DataListItem In dlGallery.Items

                Dim chkApprove As CheckBox = CType(item.FindControl("chkApprove"), CheckBox)

                If Not (chkApprove Is Nothing) Then
                    If (chkApprove.Checked) Then

                        Dim photoID As Integer = Convert.ToInt32(dlGallery.DataKeys(item.ItemIndex))
                        Dim objPhotoController As New PhotoController
                        Dim objPhoto As PhotoInfo = objPhotoController.Get(photoID)

                        If Not (objPhoto Is Nothing) Then
                            objPhoto.IsApproved = True
                            objPhoto.DateApproved = DateTime.Now
                            objPhoto.ApproverID = Me.UserId
                            objPhotoController.Update(objPhoto)

                            If (objPhoto.AuthorID <> Me.UserId) Then
                                NotifyAuthor(objPhoto, True)
                            End If
                        End If

                    End If
                End If

            Next

            BindPhotos()

        End Sub

        Private Sub cmdApproveAll_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cmdApproveAll.Click

            For Each item As DataListItem In dlGallery.Items

                Dim photoID As Integer = Convert.ToInt32(dlGallery.DataKeys(item.ItemIndex))
                Dim objPhotoController As New PhotoController
                Dim objPhoto As PhotoInfo = objPhotoController.Get(photoID)

                If Not (objPhoto Is Nothing) Then
                    objPhoto.IsApproved = True
                    objPhoto.DateApproved = DateTime.Now
                    objPhoto.ApproverID = Me.UserId
                    objPhotoController.Update(objPhoto)
                    If (objPhoto.AuthorID <> Me.UserId) Then
                        NotifyAuthor(objPhoto, True)
                    End If
                End If

            Next

            BindPhotos()

        End Sub

        Private Sub cmdRejectSelected_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cmdRejectSelected.Click

            For Each item As DataListItem In dlGallery.Items

                Dim chkReject As CheckBox = CType(item.FindControl("chkApprove"), CheckBox)

                If Not (chkReject Is Nothing) Then
                    If (chkReject.Checked) Then

                        Dim photoID As Integer = Convert.ToInt32(dlGallery.DataKeys(item.ItemIndex))
                        Dim objPhotoController As New PhotoController
                        Dim objPhoto As PhotoInfo = objPhotoController.Get(photoID)

                        If Not (objPhoto Is Nothing) Then
                            Dim objAlbumController As New AlbumController
                            Dim objAlbum As AlbumInfo = objAlbumController.Get(objPhoto.AlbumID)
                            Dim filePath As String = ""

                            If Not (objAlbum Is Nothing) Then
                                filePath = PortalSettings.HomeDirectoryMapPath & objAlbum.HomeDirectory & "\"
                            End If
                            System.IO.File.Delete(filePath & objPhoto.FileName)
                            objPhotoController.Delete(photoID)
                            If (objPhoto.AuthorID <> Me.UserId) Then
                                NotifyAuthor(objPhoto, False)
                            End If
                        End If

                    End If
                End If

            Next

            BindPhotos()

        End Sub

        Private Sub cmdRejectAll_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cmdRejectAll.Click

            For Each item As DataListItem In dlGallery.Items

                Dim photoID As Integer = Convert.ToInt32(dlGallery.DataKeys(item.ItemIndex))
                Dim objPhotoController As New PhotoController
                Dim objPhoto As PhotoInfo = objPhotoController.Get(photoID)

                If Not (objPhoto Is Nothing) Then
                    Dim objAlbumController As New AlbumController
                    Dim objAlbum As AlbumInfo = objAlbumController.Get(objPhoto.AlbumID)
                    Dim filePath As String = ""

                    If Not (objAlbum Is Nothing) Then
                        filePath = PortalSettings.HomeDirectoryMapPath & objAlbum.HomeDirectory & "\"
                    End If
                    System.IO.File.Delete(filePath & objPhoto.FileName)
                    objPhotoController.Delete(photoID)
                    If (objPhoto.AuthorID <> Me.UserId) Then
                        NotifyAuthor(objPhoto, False)
                    End If
                End If

            Next

            BindPhotos()

        End Sub

#End Region

    End Class

End Namespace