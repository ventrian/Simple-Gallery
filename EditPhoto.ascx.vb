'
' Simple Gallery for DotNetNuke -  http://www.dotnetnuke.com
' Copyright (c) 2002-2007
' by Ventrian ( sales@ventrian.com ) ( http://www.ventrian.com )
'

Imports System.IO

Imports DotNetNuke.Common
Imports DotNetNuke.Common.Utilities
Imports DotNetNuke.Services.Exceptions
Imports DotNetNuke.Services.Localization

Imports Ventrian.SimpleGallery.Common
Imports Ventrian.SimpleGallery.Entities
Imports System.Drawing
Imports System.Drawing.Drawing2D
Imports System.Drawing.Imaging
Imports DotNetNuke.Security

Namespace Ventrian.SimpleGallery

    Partial Public Class EditPhoto
        Inherits SimpleGalleryBase

#Region " Private Members "

        Private _itemID As Integer = Null.NullInteger
        Private _albumID As Integer = Null.NullInteger
        Private _width As Integer
        Private _height As Integer
        Private _returnUrl As String = Null.NullString

#End Region

#Region " Private Properties "

        Private ReadOnly Property DefaultWidth() As Integer
            Get
                Dim objSettingController As New SettingController
                Return Convert.ToInt32(objSettingController.GetSetting(Constants.SETTING_WIDTH, Me.Settings()))

            End Get
        End Property

        Private Property Width() As Integer
            Get
                Return _width
            End Get
            Set(ByVal Value As Integer)
                _width = Value
            End Set
        End Property

        Private Property Height() As Integer
            Get
                Return _height
            End Get
            Set(ByVal Value As Integer)
                _height = Value
            End Set
        End Property

#End Region

#Region " Private Methods "

        Private Sub BindAlbums()

            Dim objAlbumController As New AlbumController
            Dim objAlbums As ArrayList

            If (Me.HasEditPermissions) Then
                objAlbums = objAlbumController.List(ModuleId, GallerySettings.AlbumFilter, False, True, AlbumSortType.Caption, SortDirection.ASC)
            Else
                objAlbums = objAlbumController.List(ModuleId, GallerySettings.AlbumFilter, True, True, AlbumSortType.Caption, SortDirection.ASC)
            End If

            If (GallerySettings.AlbumFilter <> Null.NullInteger) Then
                For Each objAlbum As AlbumInfo In objAlbums
                    objAlbum.CaptionIndented = ".." & objAlbum.CaptionIndented
                Next
                Dim objAlbumFilter As AlbumInfo = objAlbumController.Get(GallerySettings.AlbumFilter)
                If Not (objAlbumFilter Is Nothing) Then
                    objAlbumFilter.CaptionIndented = objAlbumFilter.Caption
                    objAlbums.Insert(0, objAlbumFilter)
                End If
            End If

            drpAlbums.DataSource = objAlbums
            drpAlbums.DataBind()

            If (drpAlbums.Items.Count > 1) Then
                drpAlbums.Items.Insert(0, New System.Web.UI.WebControls.ListItem(Localization.GetString("SelectAlbum", Me.LocalResourceFile), "-1"))
            End If

            If (_albumID <> Null.NullInteger) Then
                If Not (drpAlbums.Items.FindByValue(_albumID.ToString()) Is Nothing) Then
                    drpAlbums.SelectedValue = _albumID.ToString()
                End If
            Else
                If (drpAlbums.Items.Count = 2) Then
                    drpAlbums.SelectedIndex = 1
                End If
            End If

        End Sub

        Private Sub BindCrumbs()

            ucGalleryMenu.AddCrumb(Localization.GetString("AllAlbums", LocalResourceFile), NavigateURL())
            ucGalleryMenu.AddCrumb(Localization.GetString("EditPhoto", LocalResourceFile), Request.Url.ToString())

        End Sub

        Private Function ExtractFileName(ByVal path As String) As String

            Dim extractPos As Integer = path.LastIndexOf("\") + 1
            Return path.Substring(extractPos, path.Length - extractPos)

        End Function

        Protected Function GetPhotoHeight(ByVal dataItem As Object) As String

            If (GallerySettings.ThumbnailPhoto = ThumbnailType.Square) Then
                Return GallerySettings.ThumbnailSquare
            End If

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

        Protected Function GetPhotoPath(ByVal dataItem As Object) As String

            Dim objPhoto As PhotoInfo = CType(dataItem, PhotoInfo)

            If Not (objPhoto Is Nothing) Then
                If (GallerySettings.CompressionPhoto = CompressionType.MinSize) Then
                    If (GallerySettings.ThumbnailPhoto = ThumbnailType.Proportion) Then
                        Return Me.ResolveUrl("ImageHandler.ashx?width=" & GetPhotoWidth(CType(objPhoto, Object)) & "&height=" & GetPhotoHeight(CType(objPhoto, Object)) & "&HomeDirectory=" & System.Uri.EscapeDataString(PortalSettings.HomeDirectory & objPhoto.HomeDirectory) & "&fileName=" & System.Uri.EscapeDataString(objPhoto.FileName) & "&portalid=" & PortalId.ToString() & "&i=" & objPhoto.PhotoID)
                    Else
                        Return Me.ResolveUrl("ImageHandler.ashx?width=" & GetPhotoWidth(CType(objPhoto, Object)) & "&height=" & GetPhotoHeight(CType(objPhoto, Object)) & "&HomeDirectory=" & System.Uri.EscapeDataString(PortalSettings.HomeDirectory & objPhoto.HomeDirectory) & "&fileName=" & System.Uri.EscapeDataString(objPhoto.FileName) & "&portalid=" & PortalId.ToString() & "&i=" & objPhoto.PhotoID & "&s=1")
                    End If
                Else
                    If (GallerySettings.ThumbnailPhoto = ThumbnailType.Proportion) Then
                        Return Me.ResolveUrl("ImageHandler.ashx?width=" & GetPhotoWidth(CType(objPhoto, Object)) & "&height=" & GetPhotoHeight(CType(objPhoto, Object)) & "&HomeDirectory=" & System.Uri.EscapeDataString(PortalSettings.HomeDirectory & objPhoto.HomeDirectory) & "&fileName=" & System.Uri.EscapeDataString(objPhoto.FileName) & "&portalid=" & PortalId.ToString() & "&i=" & objPhoto.PhotoID & "&q=1")
                    Else
                        Return Me.ResolveUrl("ImageHandler.ashx?width=" & GetPhotoWidth(CType(objPhoto, Object)) & "&height=" & GetPhotoHeight(CType(objPhoto, Object)) & "&HomeDirectory=" & System.Uri.EscapeDataString(PortalSettings.HomeDirectory & objPhoto.HomeDirectory) & "&fileName=" & System.Uri.EscapeDataString(objPhoto.FileName) & "&portalid=" & PortalId.ToString() & "&i=" & objPhoto.PhotoID & "&q=1" & "&s=1")
                    End If
                End If
            End If

            Return ""

        End Function

        Protected Function GetPhotoWidth(ByVal dataItem As Object) As String

            If (GallerySettings.ThumbnailPhoto = ThumbnailType.Square) Then
                Return GallerySettings.ThumbnailSquare
            End If

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

        Private Sub SecurityCheck()

            If (Me.HasEditPhotoPermissions Or Me.HasDeletePhotoPermissions) = False Then
                If (Request.IsAuthenticated) Then
                    Dim objPhotoController As New PhotoController
                    Dim objPhoto As PhotoInfo = objPhotoController.Get(_itemID)

                    If (objPhoto IsNot Nothing) Then
                        Dim objAlbumController As New AlbumController()
                        Dim objAlbum As AlbumInfo = objAlbumController.Get(objPhoto.AlbumID)

                        If (objAlbum IsNot Nothing) Then
                            If (objAlbum.InheritSecurity = False) Then
                                If (Settings.Contains(objAlbum.AlbumID & "-" & Constants.SETTING_PERMISSION_EDIT_ALBUM)) Then
                                    If (PortalSecurity.IsInRoles(Settings(objAlbum.AlbumID & "-" & Constants.SETTING_PERMISSION_EDIT_ALBUM).ToString())) Then
                                        ' OK
                                    Else
                                        Response.Redirect(NavigateURL, True)
                                    End If
                                Else
                                    Response.Redirect(NavigateURL, True)
                                End If
                            Else
                                Response.Redirect(NavigateURL, True)
                            End If
                        Else
                            Response.Redirect(NavigateURL, True)
                        End If
                    Else
                        Response.Redirect(NavigateURL, True)
                    End If
                Else
                    Response.Redirect(NavigateURL, True)
                End If
            End If

        End Sub

        Private Sub SetButtonVisibility()

            cmdEditAlbums.Visible = Me.HasEditPermissions
            cmdUpdate.Visible = (Me.HasUploadPhotoPermissions Or Me.HasEditPhotoPermissions)
            cmdDelete.Visible = Me.HasDeletePhotoPermissions
            If (cmdDelete.Visible = False) Then
                Dim objAlbumController As New AlbumController()
                Dim objAlbum As AlbumInfo = objAlbumController.Get(Convert.ToInt32(drpAlbums.SelectedValue))

                If (objAlbum IsNot Nothing) Then
                    If (objAlbum.InheritSecurity = False) Then
                        If (Settings.Contains(objAlbum.AlbumID & "-" & Constants.SETTING_PERMISSION_DELETE_ALBUM)) Then
                            If (PortalSecurity.IsInRoles(Settings(objAlbum.AlbumID & "-" & Constants.SETTING_PERMISSION_DELETE_ALBUM).ToString())) Then
                                cmdDelete.Visible = True
                            End If
                        End If
                    End If
                End If

            End If
            cmdSetDefault.Visible = Me.HasEditPermissions

        End Sub


        Private Sub ReadQueryString()

            If Not (Request.QueryString("ItemID") Is Nothing) Then
                _itemID = Int32.Parse(Request.QueryString("ItemID"))
            End If

            If Not (Request.QueryString("AlbumID") Is Nothing) Then
                _albumID = Int32.Parse(Request.QueryString("AlbumID"))
            End If

            If Not (Request.QueryString("ReturnUrl") Is Nothing) Then
                _returnUrl = Request.QueryString("ReturnUrl")
            End If

        End Sub

#End Region

#Region " Event Handlers "

        Private Sub Page_Initialization(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Init

            Try

                ReadQueryString()
                SecurityCheck()
                BindCrumbs()

            Catch exc As Exception    'Module failed to load
                ProcessModuleLoadException(Me, exc)
            End Try

        End Sub

        Private Sub Page_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load

            Try

                If Page.IsPostBack = False Then

                    BindAlbums()

                    trTags.Visible = GallerySettings.EnableTags

                    cmdDelete.Attributes.Add("onClick", "javascript:return confirm('" & Localization.GetString("DeletePhoto", LocalResourceFile) & "');")
                    cmdSetDefault.Attributes.Add("onClick", "javascript:return confirm('" & Localization.GetString("SetDefault", LocalResourceFile) & "');")

                    If Not Null.IsNull(_itemID) Then

                        valName.Enabled = True

                        Dim objPhotoController As New PhotoController
                        Dim objPhoto As PhotoInfo = objPhotoController.Get(_itemID)

                        If Not objPhoto Is Nothing Then

                            txtName.Text = objPhoto.Name
                            drpAlbums.SelectedValue = objPhoto.AlbumID.ToString()
                            txtDescription.Text = objPhoto.Description
                            txtTags.Text = objPhoto.Tags

                            litPhoto.Text = "<table class=""photo-frame"">" _
                                        & "<tr><td class=""topx--""></td><td class=""top-x-""></td><td class=""top--x""></td></tr>" _
                                        & "<tr><td class=""midx--""></td><td valign=""top""><img src=""" & GetPhotoPath(objPhoto) & """ class=""photo_198"" width=""" & GetPhotoWidth(objPhoto) & """ height=""" & GetPhotoHeight(objPhoto) & """></td><td class=""mid--x""></td></tr>" _
                                        & "<tr><td class=""botx--""></td><td class=""bot-x-""></td><td class=""bot--x""></td></tr>" _
                                        & "</table>"


                        Else       ' security violation attempt to access item not related to this Module
                            Response.Redirect(NavigateURL(), True)
                        End If

                    Else
                        Response.Redirect(EditUrl("Add"), True)
                    End If

                    SetButtonVisibility()

                End If

            Catch exc As Exception    'Module failed to load
                ProcessModuleLoadException(Me, exc)
            End Try

        End Sub

        Private Sub cmdCancel_Click(ByVal sender As Object, ByVal e As EventArgs) Handles cmdCancel.Click
            Try

                If (_returnUrl <> "") Then
                    Response.Redirect(_returnUrl, True)
                End If

                If (Me._itemID <> Null.NullInteger) Then
                    Response.Redirect(NavigateURL(Me.TabId, "", "AlbumID=" & Me.ModuleId.ToString() & "-" & drpAlbums.SelectedValue), True)
                End If

                If (Me._albumID <> Null.NullInteger) Then
                    Response.Redirect(NavigateURL(Me.TabId, "", "AlbumID=" & Me.ModuleId.ToString() & "-" & _albumID.ToString()), True)
                End If

                Response.Redirect(NavigateURL(), True)
            Catch exc As Exception    'Module failed to load
                ProcessModuleLoadException(Me, exc)
            End Try
        End Sub

        Private Sub cmdSetDefault_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cmdSetDefault.Click
            Try

                Dim objPhotoController As New PhotoController
                Dim objPhoto As PhotoInfo = objPhotoController.Get(_itemID)

                If Not (objPhoto Is Nothing) Then
                    objPhotoController.SetDefaultPhoto(objPhoto.PhotoID, objPhoto.AlbumID)
                End If

                If (_returnUrl <> "") Then
                    Response.Redirect(_returnUrl, True)
                Else
                    Response.Redirect(NavigateURL(Me.TabId, "", "AlbumID=" & Me.ModuleId.ToString() & "-" & drpAlbums.SelectedValue), True)
                End If

            Catch exc As Exception    'Module failed to load
                ProcessModuleLoadException(Me, exc)
            End Try
        End Sub

        Private Sub cmdDelete_Click(ByVal sender As Object, ByVal e As EventArgs) Handles cmdDelete.Click
            Try
                If Not Null.IsNull(_itemID) Then
                    Dim objPhotoController As New PhotoController
                    Dim objPhoto As PhotoInfo = objPhotoController.Get(_itemID)
                    If Not (objPhoto Is Nothing) Then
                        Dim filePath As String = GetFilePath(objPhoto.AlbumID)
                        If (File.Exists(filePath & objPhoto.FileName)) Then
                            File.Delete(filePath & objPhoto.FileName)
                        End If
                        objPhotoController.Delete(_itemID)
                    End If
                    If (_returnUrl <> "") Then
                        Response.Redirect(_returnUrl, True)
                    Else
                        Response.Redirect(NavigateURL(Me.TabId, "", "AlbumID=" & Me.ModuleId.ToString() & "-" & objPhoto.AlbumID), True)
                    End If
                End If

                If (_returnUrl <> "") Then
                    Response.Redirect(_returnUrl, True)
                Else
                    Response.Redirect(NavigateURL(), True)
                End If

            Catch exc As Exception    'Module failed to load
                ProcessModuleLoadException(Me, exc)
            End Try
        End Sub

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

        Private Function RemoveExtension(ByVal fileName As String) As String

            Dim name As String = ""

            If (fileName.Length > 0) Then
                If (fileName.IndexOf("."c) <> -1) Then
                    name = fileName.Substring(0, fileName.LastIndexOf("."c))
                End If
            End If

            Return name

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

        Private Sub cmdUpdate_Click(ByVal sender As Object, ByVal e As EventArgs) Handles cmdUpdate.Click

            Try

                If Page.IsValid = True Then

                    Dim objPhoto As New PhotoInfo
                    Dim objPhotoController As New PhotoController

                    If Not (Null.IsNull(_itemID)) Then
                        objPhoto = objPhotoController.Get(_itemID)

                        If Not (objPhoto Is Nothing) Then
                            If (objPhoto.AlbumID <> Convert.ToInt32(drpAlbums.SelectedValue)) Then
                                ' Move Photo
                                Dim originalFileName As String = objPhoto.FileName
                                Dim originalFilePath As String = GetFilePath(objPhoto.AlbumID)

                                Dim destinationFileName As String = objPhoto.FileName
                                Dim destinationFilePath As String = GetFilePath(Convert.ToInt32(drpAlbums.SelectedValue))

                                If (File.Exists(destinationFilePath & destinationFileName)) Then
                                    Dim fileExtension As String = ExtractFileExtension(destinationFileName)
                                    Dim fileNameWithoutExtension As String = RemoveExtension(destinationFileName)
                                    For i As Integer = 1 To 1000
                                        If Not (File.Exists(destinationFilePath & fileNameWithoutExtension & "_" & i.ToString() & "." & fileExtension)) Then
                                            destinationFileName = fileNameWithoutExtension & "_" & i.ToString() & "." & fileExtension
                                            Exit For
                                        End If
                                    Next
                                End If

                                File.Move(originalFilePath & originalFileName, destinationFilePath & destinationFileName)
                                objPhoto.FileName = destinationFileName

                            End If
                        End If
                    End If

                    objPhoto.ModuleID = Me.ModuleId
                    objPhoto.AlbumID = Convert.ToInt32(drpAlbums.SelectedValue)
                    objPhoto.Name = txtName.Text
                    objPhoto.Description = txtDescription.Text
                    objPhoto.DateUpdated = DateTime.Now
                    objPhotoController.Update(objPhoto)

                    Dim objTagController As New TagController
                    objTagController.DeletePhotoTag(objPhoto.PhotoID)

                    If (txtTags.Text <> "") Then
                        Dim tags As String() = txtTags.Text.Split(","c)
                        For Each tag As String In tags
                            If (tag <> "") Then
                                Dim objTag As TagInfo = objTagController.Get(ModuleId, tag)

                                If (objTag Is Nothing) Then
                                    objTag = New TagInfo
                                    objTag.Name = tag
                                    objTag.NameLowered = tag.ToLower()
                                    objTag.ModuleID = ModuleId
                                    objTag.TagID = objTagController.Add(objTag)
                                End If

                                objTagController.Add(objPhoto.PhotoID, objTag.TagID)
                            End If
                        Next
                    End If

                    If (_returnUrl <> "") Then
                        Response.Redirect(_returnUrl, True)
                    Else
                        Response.Redirect(NavigateURL(Me.TabId, "", "AlbumID=" & Me.ModuleId.ToString() & "-" & drpAlbums.SelectedValue), True)
                    End If

                End If

            Catch exc As Exception    'Module failed to load
                ProcessModuleLoadException(Me, exc)
            End Try
        End Sub

        Private Sub cmdEditAlbums_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cmdEditAlbums.Click

            Response.Redirect(EditUrl("EditAlbums"), True)

        End Sub

        Private Sub cmdReplace_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cmdReplace.Click

            phReplace.Visible = False
            phReplaceUpload.Visible = True

        End Sub

        Private Sub cmdCancelReplace_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cmdCancelReplace.Click

            phReplace.Visible = True
            phReplaceUpload.Visible = False

        End Sub

        Private Sub cmdUploadReplace_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cmdUploadReplace.Click

            If (fuReplace.HasFile) Then
                If (fuReplace.PostedFile IsNot Nothing) Then

                    Dim objPhotoController As New PhotoController
                    Dim objPhoto As PhotoInfo = objPhotoController.Get(_itemID)

                    If Not objPhoto Is Nothing Then

                        Dim filePath As String = GetFilePath(objPhoto.AlbumID)

                        Dim oldPath As String = filePath & objPhoto.FileName
                        Dim oldWidth As Integer = objPhoto.Width
                        Dim oldHeight As Integer = objPhoto.Height

                        Dim fileName As String = ExtractFileName(fuReplace.PostedFile.FileName)
                        Dim fileExtension As String = ExtractFileExtension(fileName)
                        Dim fileNameWithoutExtension As String = RemoveExtension(fileName).Replace("/", "_").Replace(".", "_").Replace("%", "_")

                        If (fileExtension.ToLower() <> "jpg" And fileExtension.ToLower() <> "gif") Then
                            phReplace.Visible = True
                            phReplaceUpload.Visible = False
                            Return
                        End If

                        fileName = fileNameWithoutExtension & "." & fileExtension

                        If (File.Exists(filePath & fileName)) Then
                            For i As Integer = 1 To 1000
                                If Not (File.Exists(filePath & fileNameWithoutExtension & "_" & i.ToString() & "." & fileExtension)) Then
                                    fileName = fileNameWithoutExtension & "_" & i.ToString() & "." & fileExtension
                                    fileNameWithoutExtension = fileNameWithoutExtension & "_" & i.ToString()
                                    Exit For
                                End If
                            Next
                        End If

                        Dim photo As Drawing.Image = Drawing.Image.FromStream(fuReplace.PostedFile.InputStream)

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

                            Dim bmp As New Bitmap(width, height)
                            Dim g As Graphics = Graphics.FromImage(DirectCast(bmp, Drawing.Image))

                            If (GallerySettings.Compression = CompressionType.Quality) Then
                                g.InterpolationMode = InterpolationMode.HighQualityBicubic
                                g.SmoothingMode = SmoothingMode.HighQuality
                                g.PixelOffsetMode = PixelOffsetMode.HighQuality
                                g.CompositingQuality = CompositingQuality.HighQuality
                            End If

                            g.DrawImage(photo, 0, 0, width, height)

                            If (GallerySettings.UseWatermark And GallerySettings.WatermarkText <> "") Then
                                Dim crSize As SizeF = New SizeF
                                Dim brushColor As Brush = Brushes.Yellow
                                Dim fnt As Font = New Font("Verdana", 11, FontStyle.Bold)
                                Dim strDirection As StringFormat = New StringFormat

                                strDirection.Alignment = StringAlignment.Center
                                crSize = g.MeasureString(GallerySettings.WatermarkText, fnt)

                                Dim yPixelsFromBottom As Integer = Convert.ToInt32(Convert.ToDouble(height) * 0.05)
                                Dim yPosFromBottom As Single = Convert.ToSingle((height - yPixelsFromBottom) - (crSize.Height / 2))
                                Dim xCenterOfImage As Single = Convert.ToSingle((width / 2))

                                g.SmoothingMode = Drawing2D.SmoothingMode.AntiAlias

                                Dim semiTransBrush2 As SolidBrush = New SolidBrush(Color.FromArgb(153, 0, 0, 0))
                                g.DrawString(GallerySettings.WatermarkText, fnt, semiTransBrush2, New PointF(xCenterOfImage + 1, yPosFromBottom + 1), strDirection)

                                Dim semiTransBrush As SolidBrush = New SolidBrush(Color.FromArgb(153, 255, 255, 255))
                                g.DrawString(GallerySettings.WatermarkText, fnt, semiTransBrush, New PointF(xCenterOfImage, yPosFromBottom), strDirection)
                            End If

                            If (GallerySettings.UseWatermark And GallerySettings.WatermarkImage <> "") Then
                                Dim watermark As String = PortalSettings.HomeDirectoryMapPath & GallerySettings.WatermarkImage
                                If (File.Exists(watermark)) Then
                                    Dim imgWatermark As System.Drawing.Image = New System.Drawing.Bitmap(watermark)
                                    Dim wmWidth As Integer = imgWatermark.Width
                                    Dim wmHeight As Integer = imgWatermark.Height

                                    Dim objImageAttributes As New ImageAttributes()
                                    Dim objColorMap As New ColorMap()
                                    objColorMap.OldColor = Color.FromArgb(255, 0, 255, 0)
                                    objColorMap.NewColor = Color.FromArgb(0, 0, 0, 0)
                                    Dim remapTable As ColorMap() = {objColorMap}
                                    objImageAttributes.SetRemapTable(remapTable, ColorAdjustType.Bitmap)

                                    Dim colorMatrixElements As Single()() = {New Single() {1.0F, 0.0F, 0.0F, 0.0F, 0.0F}, New Single() {0.0F, 1.0F, 0.0F, 0.0F, 0.0F}, New Single() {0.0F, 0.0F, 1.0F, 0.0F, 0.0F}, New Single() {0.0F, 0.0F, 0.0F, 0.3F, 0.0F}, New Single() {0.0F, 0.0F, 0.0F, 0.0F, 1.0F}}
                                    Dim wmColorMatrix As New ColorMatrix(colorMatrixElements)
                                    objImageAttributes.SetColorMatrix(wmColorMatrix, ColorMatrixFlag.[Default], ColorAdjustType.Bitmap)

                                    Dim xPosOfWm As Integer = ((width - wmWidth) - 10)
                                    Dim yPosOfWm As Integer = 10

                                    Select Case GallerySettings.WatermarkImagePosition
                                        Case WatermarkPosition.TopLeft
                                            xPosOfWm = 10
                                            yPosOfWm = 10
                                            Exit Select

                                        Case WatermarkPosition.TopRight
                                            xPosOfWm = ((width - wmWidth) - 10)
                                            yPosOfWm = 10
                                            Exit Select

                                        Case WatermarkPosition.BottomLeft
                                            xPosOfWm = 10
                                            yPosOfWm = ((height - wmHeight) - 10)

                                        Case WatermarkPosition.BottomRight
                                            xPosOfWm = ((width - wmWidth) - 10)
                                            yPosOfWm = ((height - wmHeight) - 10)
                                    End Select

                                    g.DrawImage(imgWatermark, New Rectangle(xPosOfWm, yPosOfWm, wmWidth, wmHeight), 0, 0, wmWidth, wmHeight, _
                                     GraphicsUnit.Pixel, objImageAttributes)
                                    imgWatermark.Dispose()
                                End If
                            End If

                            photo.Dispose()

                            If (GallerySettings.Compression = CompressionType.Quality) Then
                                Dim info As ImageCodecInfo() = ImageCodecInfo.GetImageEncoders()
                                Dim params As New EncoderParameters
                                params.Param(0) = New EncoderParameter(Encoder.Quality, 90L)
                                bmp.Save(filePath & fileName, info(1), params)
                            Else
                                bmp.Save(filePath & fileName, ImageFormat.Jpeg)
                            End If
                            bmp.Dispose()

                        Else

                            photo.Dispose()
                            fuReplace.PostedFile.SaveAs(filePath & fileName)

                        End If

                        objPhoto.FileName = fileNameWithoutExtension & "." & fileExtension
                        objPhoto.Name = RemoveExtension(ExtractFileName(fuReplace.PostedFile.FileName))

                        objPhoto.Width = width
                        objPhoto.Height = height

                        If (chkAddAsBefore.Checked) Then

                            If (File.Exists(oldPath)) Then

                                Dim Image1 As New Bitmap(filePath & fileName)
                                Dim Image2 As New Bitmap(oldPath)

                                Dim maxHeight As Integer = objPhoto.Height
                                If (oldHeight > objPhoto.Height) Then
                                    maxHeight = oldHeight
                                End If

                                Dim Image3 As New Bitmap(objPhoto.Width + 10 + oldWidth, maxHeight)
                                Dim g As Graphics = Graphics.FromImage(Image3)
                                g.DrawImage(Image1, New Point(0, 0))
                                g.DrawImage(Image2, New Point(objPhoto.Width + 10, 0))
                                g.Dispose()
                                g = Nothing

                                ' objPhoto.Width = objPhoto.Width + 10 + oldWidth
                                objPhoto.FileName = fileNameWithoutExtension & DateTime.Now.ToString("yyyyMMddHHmmss") & "." & fileExtension

                                Image3.Save(filePath & objPhoto.FileName)

                            End If

                        End If

                        objPhotoController.Update(objPhoto)

                        litPhoto.Text = "<table class=""photo-frame"">" _
                                    & "<tr><td class=""topx--""></td><td class=""top-x-""></td><td class=""top--x""></td></tr>" _
                                    & "<tr><td class=""midx--""></td><td valign=""top""><img src=""" & GetPhotoPath(objPhoto) & """ class=""photo_198"" width=""" & GetPhotoWidth(objPhoto) & """ height=""" & GetPhotoHeight(objPhoto) & """></td><td class=""mid--x""></td></tr>" _
                                    & "<tr><td class=""botx--""></td><td class=""bot-x-""></td><td class=""bot--x""></td></tr>" _
                                    & "</table>"

                        phReplace.Visible = True
                        phReplaceUpload.Visible = False

                    End If

                Else
                    phReplace.Visible = True
                    phReplaceUpload.Visible = False
                End If
            Else
                phReplace.Visible = True
                phReplaceUpload.Visible = False
            End If

        End Sub

#End Region

    End Class

End Namespace