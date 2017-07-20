'
' Simple Gallery for DotNetNuke -  http://www.dotnetnuke.com
' Copyright (c) 2002-2007
' by Ventrian ( sales@ventrian.com ) ( http://www.ventrian.com )
'

Imports DotNetNuke.Common
Imports DotNetNuke.Common.Utilities
Imports DotNetNuke.Entities.Modules
Imports DotNetNuke.Entities.Modules.Actions
Imports DotNetNuke.Security
Imports DotNetNuke.Services.Exceptions
Imports DotNetNuke.Services.Localization

Imports Ventrian.SimpleGallery.Common
Imports Ventrian.SimpleGallery.Entities

Namespace Ventrian.SimpleGallery

    Partial Public Class RandomPhoto
        Inherits SimpleGalleryBase
        Implements IActionable

#Region " Private Members "

        Private _pageID As Integer = Null.NullInteger
        Private _moduleID As Integer = Null.NullInteger
        Private _albumID As Integer = Null.NullInteger

        Private _template As String = ""
        Private _templateTokens As String()

        Private _linkedGallerySettings As GallerySettings
        Private _loadScripts As Boolean = False

#End Region

#Region " Private Properties "

        Private ReadOnly Property LinkedGallerySettings(ByVal moduleID As Integer) As Entities.GallerySettings
            Get
                If (_linkedGallerySettings Is Nothing) Then
                    Dim objModuleController As New ModuleController
                    Dim settings As Hashtable = objModuleController.GetModuleSettings(_moduleID)
                    _linkedGallerySettings = New Entities.GallerySettings(settings)
                End If
                Return _linkedGallerySettings
            End Get
        End Property

#End Region

#Region " Private Methods "

        Private Function GetAlbumCount(ByVal dataItem As Object) As String

            Dim objAlbum As AlbumInfo = CType(dataItem, AlbumInfo)

            If Not (objAlbum Is Nothing) Then
                If (objAlbum.NumberOfAlbums > 0) Then
                    If (objAlbum.NumberOfAlbumPhotos > 0) Then
                        If (objAlbum.NumberOfAlbums = 1) Then
                            If (objAlbum.NumberOfAlbumPhotos = 1) Then
                                Return objAlbum.NumberOfAlbums.ToString() & " " & Localization.GetString("SubAlbum", Me.LocalResourceFile) & " (" & objAlbum.NumberOfAlbumPhotos.ToString() & " " & Localization.GetString("Photo", Me.LocalResourceFile) & ")"
                            Else
                                Return objAlbum.NumberOfAlbums.ToString() & " " & Localization.GetString("SubAlbum", Me.LocalResourceFile) & " (" & objAlbum.NumberOfAlbumPhotos.ToString() & " " & Localization.GetString("Photos", Me.LocalResourceFile) & ")"
                            End If
                        Else
                            If (objAlbum.NumberOfAlbumPhotos = 1) Then
                                Return objAlbum.NumberOfAlbums.ToString() & " " & Localization.GetString("SubAlbums", Me.LocalResourceFile) & " (" & objAlbum.NumberOfAlbumPhotos.ToString() & " " & Localization.GetString("Photo", Me.LocalResourceFile) & ")"
                            Else
                                Return objAlbum.NumberOfAlbums.ToString() & " " & Localization.GetString("SubAlbums", Me.LocalResourceFile) & " (" & objAlbum.NumberOfAlbumPhotos.ToString() & " " & Localization.GetString("Photos", Me.LocalResourceFile) & ")"
                            End If
                        End If
                    Else
                        If (objAlbum.NumberOfAlbums = 1) Then
                            Return objAlbum.NumberOfAlbums.ToString() & " " & Localization.GetString("SubAlbum", Me.LocalResourceFile)
                        Else
                            Return objAlbum.NumberOfAlbums.ToString() & " " & Localization.GetString("SubAlbums", Me.LocalResourceFile)
                        End If
                    End If
                Else
                    Return ""
                End If
            Else
                Return ""
            End If

        End Function

        Private Function GetAlbumPath(ByVal albumID As String, ByVal moduleID As Integer, ByVal homeDirectory As String) As Hashtable

            Dim objSettings As New Hashtable

            Dim objPhotoController As New PhotoController
            Dim objPhoto As PhotoInfo
            objPhoto = objPhotoController.GetFirstFromAlbum(Convert.ToInt32(albumID), moduleID)

            If Not (objPhoto Is Nothing) Then
                Dim width As Integer
                If (objPhoto.Width > GallerySettings.RandomWidth) Then
                    width = GallerySettings.RandomWidth
                Else
                    width = objPhoto.Width
                End If

                Dim height As Integer = Convert.ToInt32(objPhoto.Height / (objPhoto.Width / width))
                If (height > GallerySettings.RandomHeight) Then
                    height = GallerySettings.RandomHeight
                    width = Convert.ToInt32(objPhoto.Width / (objPhoto.Height / height))
                End If

                Dim square = ""
                If (GallerySettings.RandomThumbnail = ThumbnailType.Square) Then
                    width = GallerySettings.RandomSquare
                    height = GallerySettings.RandomSquare
                    square = "&s=1"
                End If

                objSettings.Add("AlbumWidth", width.ToString())
                objSettings.Add("AlbumHeight", height.ToString())

                If (objPhoto.HomeDirectory = "") Then
                    If (LinkedGallerySettings(_moduleID).CompressionAlbum = CompressionType.MinSize) Then
                        objSettings.Add("AlbumPath", Me.ResolveUrl("../SimpleGallery/ImageHandler.ashx?width=" & width.ToString() & "&height=" & height.ToString() & "&HomeDirectory=" & System.Uri.EscapeDataString(Me.PortalSettings.HomeDirectory & homeDirectory) & "&fileName=" & System.Uri.EscapeDataString(objPhoto.FileName) & "&portalid=" & Me.PortalSettings.PortalId.ToString() & square))
                    Else
                        objSettings.Add("AlbumPath", Me.ResolveUrl("../SimpleGallery/ImageHandler.ashx?width=" & width.ToString() & "&height=" & height.ToString() & "&HomeDirectory=" & System.Uri.EscapeDataString(Me.PortalSettings.HomeDirectory & homeDirectory) & "&fileName=" & System.Uri.EscapeDataString(objPhoto.FileName) & "&portalid=" & Me.PortalSettings.PortalId.ToString() & "&q=1" & square))
                    End If
                Else
                    If (LinkedGallerySettings(_moduleID).CompressionAlbum = CompressionType.MinSize) Then
                        objSettings.Add("AlbumPath", Me.ResolveUrl("../SimpleGallery/ImageHandler.ashx?width=" & width.ToString() & "&height=" & height.ToString() & "&HomeDirectory=" & System.Uri.EscapeDataString(Me.PortalSettings.HomeDirectory & objPhoto.HomeDirectory) & "&fileName=" & System.Uri.EscapeDataString(objPhoto.FileName) & "&portalid=" & Me.PortalSettings.PortalId.ToString() & square))
                    Else
                        objSettings.Add("AlbumPath", Me.ResolveUrl("../SimpleGallery/ImageHandler.ashx?width=" & width.ToString() & "&height=" & height.ToString() & "&HomeDirectory=" & System.Uri.EscapeDataString(Me.PortalSettings.HomeDirectory & objPhoto.HomeDirectory) & "&fileName=" & System.Uri.EscapeDataString(objPhoto.FileName) & "&portalid=" & Me.PortalSettings.PortalId.ToString() & "&q=1" & square))
                    End If
                End If
            Else
                Dim width As Integer
                If (600 > GallerySettings.RandomWidth) Then
                    width = GallerySettings.RandomWidth
                Else
                    width = 600
                End If

                Dim height As Integer = Convert.ToInt32(450 / (600 / width))
                If (height > GallerySettings.RandomHeight) Then
                    height = GallerySettings.RandomHeight
                    width = Convert.ToInt32(600 / (450 / height))
                End If

                Dim square = ""
                If (GallerySettings.RandomThumbnail = ThumbnailType.Square) Then
                    width = GallerySettings.RandomSquare
                    height = GallerySettings.RandomSquare
                    square = "&s=1"
                End If

                objSettings.Add("AlbumWidth", width.ToString())
                objSettings.Add("AlbumHeight", height.ToString())

                If (LinkedGallerySettings(_moduleID).CompressionAlbum = CompressionType.MinSize) Then
                    objSettings.Add("AlbumPath", Me.ResolveUrl("../SimpleGallery/ImageHandler.ashx?width=" & width.ToString() & "&height=" & height.ToString() & "&fileName=" & System.Uri.EscapeDataString("placeholder-600.jpg") & "&portalid=" & Me.PortalSettings.PortalId.ToString() & square))
                Else
                    objSettings.Add("AlbumPath", Me.ResolveUrl("../SimpleGallery/ImageHandler.ashx?width=" & width.ToString() & "&height=" & height.ToString() & "&fileName=" & System.Uri.EscapeDataString("placeholder-600.jpg") & "&portalid=" & Me.PortalSettings.PortalId.ToString()) & "&q=1" & square)
                End If
            End If

            Return objSettings

        End Function

        Protected Function GetAlternateTextForAlbum(ByVal dataItem As Object) As String

            Dim objAlbum As AlbumInfo = CType(dataItem, AlbumInfo)

            If Not (objAlbum Is Nothing) Then
                Return String.Format(Localization.GetString("AlternateText", Me.LocalResourceFile), objAlbum.Caption)
            Else
                Return ""
            End If

        End Function

        Private Function GetDescription(ByVal dataItem As Object) As String

            Dim objPhoto As PhotoInfo = CType(dataItem, PhotoInfo)
            Return Server.HtmlEncode(objPhoto.Description).Replace(Chr(34), "")

        End Function

        Private Function GetPhotoCount(ByVal dataItem As Object) As String

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

        Private Function GetPhotoPathFull(ByVal objPhoto As PhotoInfo) As String

            If (objPhoto.Width > LinkedGallerySettings(_moduleID).ImageWidth Or objPhoto.Height > LinkedGallerySettings(_moduleID).ImageHeight) Then
                ' Use Handler to Resize 
                Dim width As Integer = objPhoto.Width
                Dim height As Integer = objPhoto.Height

                If (width > LinkedGallerySettings(_moduleID).ImageWidth) Then
                    width = LinkedGallerySettings(_moduleID).ImageWidth
                    height = Convert.ToInt32(height / (objPhoto.Width / LinkedGallerySettings(_moduleID).ImageWidth))
                End If

                If (height > LinkedGallerySettings(_moduleID).ImageHeight) Then
                    height = LinkedGallerySettings(_moduleID).ImageHeight
                    width = Convert.ToInt32(objPhoto.Width / (objPhoto.Height / LinkedGallerySettings(_moduleID).ImageHeight))
                End If

                Return Me.ResolveUrl("../SimpleGallery/ImageHandler.ashx?width=" & width.ToString() & "&height=" & height.ToString() & "&HomeDirectory=" & System.Uri.EscapeDataString(PortalSettings.HomeDirectory & objPhoto.HomeDirectory) & "&fileName=" & System.Uri.EscapeDataString(objPhoto.FileName) & "&portalid=" & PortalId.ToString() & "&i=" & objPhoto.PhotoID & "&q=1")
            Else
                Return PortalSettings.HomeDirectory & objPhoto.HomeDirectory & "/" & objPhoto.FileName
            End If

        End Function

        Private Function GetAlbumUrl(ByVal albumID As String) As String

            Return NavigateURL(_pageID, Null.NullString, "AlbumID=" & _moduleID.ToString & "-" & albumID)

        End Function

        Private Function GetPhotoUrl(ByVal photoID As String) As String

            If (LinkedGallerySettings(_moduleID).Slideshow = SlideshowType.Popup) Then
                If (LinkedGallerySettings(_moduleID).Compression = CompressionType.MinSize) Then
                    Return "javascript:openScreenWin('smallscreen', '" & Me.ResolveUrl("../SimpleGallery/SlideShowPopup.aspx?PortalID=" & PortalId.ToString() & "&ItemID=" & photoID & "&Border=" & LinkedGallerySettings(_moduleID).BorderStyle & "&sb=" & LinkedGallerySettings(_moduleID).SortBy & "&sd=" & LinkedGallerySettings(_moduleID).SortDirection & "&tt=" & LinkedGallerySettings(_moduleID).EnableTooltip) & "', " & LinkedGallerySettings(_moduleID).PopupWidth.ToString() & ", " & LinkedGallerySettings(_moduleID).PopupHeight.ToString() & ");"
                Else
                    Return "javascript:openScreenWin('smallscreen', '" & Me.ResolveUrl("../SimpleGallery/SlideShowPopup.aspx?PortalID=" & PortalId.ToString() & "&ItemID=" & photoID & "&Border=" & LinkedGallerySettings(_moduleID).BorderStyle & "&sb=" & LinkedGallerySettings(_moduleID).SortBy & "&sd=" & LinkedGallerySettings(_moduleID).SortDirection & "&tt=" & LinkedGallerySettings(_moduleID).EnableTooltip) & "', " & LinkedGallerySettings(_moduleID).PopupWidth.ToString() & ", " & LinkedGallerySettings(_moduleID).PopupHeight.ToString() & ");"
                End If
            Else
                Return NavigateURL(_pageID, "", "galleryType=SlideShow", "ItemID=" & photoID)
            End If

        End Function

        Private Sub InitializeTemplate()

            Dim delimStr As String = "[]"
            Dim delimiter As Char() = delimStr.ToCharArray()

            If (GallerySettings.RandomDisplay = DisplayType.Album) Then
                _template = Me.GallerySettings.RandomTemplateAlbum
                _templateTokens = Me.GallerySettings.RandomTemplateAlbum.Split(delimiter)
            Else
                _template = Me.GallerySettings.RandomTemplate
                _templateTokens = Me.GallerySettings.RandomTemplate.Split(delimiter)
            End If

        End Sub

        Private Function RssUrl(ByVal albumID As String) As String

            Dim objModuleController As New ModuleController
            Dim objModule As ModuleInfo = objModuleController.GetModule(_moduleID, _pageID, False)

            If Not (objModule Is Nothing) Then
                Return Me.ResolveUrl("../SimpleGallery/RSS.aspx?t=" & _pageID.ToString() & "&m=" & _moduleID.ToString() & "&tm=" & objModule.TabModuleID & "&a=" & albumID & "&portalid=" & Me.PortalId)
            Else
                Return Me.ResolveUrl("../SimpleGallery/RSS.aspx?t=" & _pageID.ToString() & "&m=" & _moduleID.ToString() & "&tm=" & Me.TabModuleId & "&a=" & albumID & "&portalid=" & Me.PortalId)
            End If

        End Function

#End Region

#Region " Protected Methods "

        Protected Overloads Function FormatBorderPath(ByVal image As String) As String

            If (_moduleID <> Null.NullInteger) Then
                Return Me.ResolveUrl("~/DesktopModules/SimpleGallery/Images/Borders/" & LinkedGallerySettings(_moduleID).BorderStyle & "/" & image).Replace(" ", "%20")
            Else
                Return ""
            End If
            
        End Function

        Protected Function GetPhotoPath(ByVal dataItem As Object) As String

            Dim square = ""
            If (GallerySettings.RandomThumbnail = ThumbnailType.Square) Then
                square = "&s=1"
            End If

            Dim objPhoto As PhotoInfo = CType(dataItem, PhotoInfo)

            If Not (objPhoto Is Nothing) Then
                If (GallerySettings.RandomCompression = CompressionType.MinSize) Then
                    Return Me.ResolveUrl("../SimpleGallery/ImageHandler.ashx?width=" & GetPhotoWidth(CType(objPhoto, Object)) & "&height=" & GetPhotoHeight(CType(objPhoto, Object)) & "&HomeDirectory=" & System.Uri.EscapeDataString(Me.PortalSettings.HomeDirectory & objPhoto.HomeDirectory) & "&fileName=" & System.Uri.EscapeDataString(objPhoto.FileName) & "&portalid=" & Me.PortalId.ToString() & square)
                Else
                    Return Me.ResolveUrl("../SimpleGallery/ImageHandler.ashx?width=" & GetPhotoWidth(CType(objPhoto, Object)) & "&height=" & GetPhotoHeight(CType(objPhoto, Object)) & "&HomeDirectory=" & System.Uri.EscapeDataString(Me.PortalSettings.HomeDirectory & objPhoto.HomeDirectory) & "&fileName=" & System.Uri.EscapeDataString(objPhoto.FileName) & "&portalid=" & Me.PortalId.ToString() & "&q=1" & square)
                End If
            End If

            Return ""

        End Function

        Protected Function GetPhotoWidth(ByVal dataItem As Object) As String

            If (Me.GallerySettings.RandomThumbnail = ThumbnailType.Square) Then
                Return GallerySettings.RandomSquare
            End If

            Dim objPhoto As PhotoInfo = CType(dataItem, PhotoInfo)

            If Not (objPhoto Is Nothing) Then
                Dim width As Integer
                If (objPhoto.Width > Me.GallerySettings.RandomWidth) Then
                    width = Me.GallerySettings.RandomWidth
                Else
                    width = objPhoto.Width
                End If

                Dim height As Integer = Convert.ToInt32(objPhoto.Height / (objPhoto.Width / width))
                If (height > Me.GallerySettings.RandomHeight) Then
                    height = GallerySettings.RandomHeight
                    width = Convert.ToInt32(objPhoto.Width / (objPhoto.Height / height))
                End If

                Return width.ToString()
            Else
                Return Me.GallerySettings.RandomWidth.ToString()
            End If

        End Function

        Protected Function GetPhotoHeight(ByVal dataItem As Object) As String

            If (Me.GallerySettings.RandomThumbnail = ThumbnailType.Square) Then
                Return GallerySettings.RandomSquare
            End If

            Dim objPhoto As PhotoInfo = CType(dataItem, PhotoInfo)

            If Not (objPhoto Is Nothing) Then
                Dim width As Integer
                If (objPhoto.Width > Me.GallerySettings.RandomWidth) Then
                    width = Me.GallerySettings.RandomWidth
                Else
                    width = objPhoto.Width
                End If

                Dim height As Integer = Convert.ToInt32(objPhoto.Height / (objPhoto.Width / width))
                If (height > Me.GallerySettings.RandomHeight) Then
                    height = GallerySettings.RandomHeight
                    width = Convert.ToInt32(objPhoto.Width / (objPhoto.Height / height))
                End If

                Return height.ToString()
            Else
                Return Me.GallerySettings.RandomHeight.ToString()
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

        Protected Function GetTitle(ByVal dataItem As Object) As String

            Dim objPhoto As PhotoInfo = CType(dataItem, PhotoInfo)

            Return Server.HtmlEncode(objPhoto.Name).Replace(Chr(34), "")

        End Function

#End Region

#Region " Event Handlers "

        Private Sub Page_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load

            Try

                InitializeTemplate()

                If (Settings.Contains(Constants.SETTING_RANDOM_PHOTO_TAB_ID)) Then
                    If (IsNumeric(Settings(Constants.SETTING_RANDOM_PHOTO_TAB_ID).ToString())) Then
                        _pageID = Convert.ToInt32(Settings(Constants.SETTING_RANDOM_PHOTO_TAB_ID).ToString())
                    End If
                End If

                If (Settings.Contains(Constants.SETTING_RANDOM_PHOTO_MODULE_ID)) Then
                    If (IsNumeric(Settings(Constants.SETTING_RANDOM_PHOTO_MODULE_ID).ToString())) Then
                        _moduleID = Convert.ToInt32(Settings(Constants.SETTING_RANDOM_PHOTO_MODULE_ID).ToString())
                    End If
                End If

                If (Settings.Contains(Constants.SETTING_RANDOM_PHOTO_ALBUM_ID)) Then
                    If (IsNumeric(Settings(Constants.SETTING_RANDOM_PHOTO_ALBUM_ID).ToString())) Then
                        _albumID = Convert.ToInt32(Settings(Constants.SETTING_RANDOM_PHOTO_ALBUM_ID).ToString())
                    End If
                End If

                If (_moduleID = Null.NullInteger) Then
                    phStyles.Visible = False
                End If

                dlGallery.RepeatDirection = Me.GallerySettings.RandomRepeatDirection
                dlGallery.RepeatColumns = Me.GallerySettings.RandomRepeatColumns

                If (Me.GallerySettings.RandomDisplay = DisplayType.Photo) Then

                    Dim objPhotoController As New PhotoController
                    Dim objPhotos As ArrayList

                    If (Me.GallerySettings.RandomMode = ModeType.Random) Then
                        objPhotos = objPhotoController.GetRandomPhoto(_moduleID, _albumID, Me.GallerySettings.RandomMaxCount, Me.GallerySettings.RandomTagFilter)

                        If (GallerySettings.RandomTemplateMode = TemplateModeType.Simple) Then
                            dlGallery.DataSource = objPhotos
                            dlGallery.DataBind()
                        Else
                            rptGallery.DataSource = objPhotos
                            rptGallery.DataBind()
                        End If
                    Else
                        If (Me.GallerySettings.RandomMode = ModeType.Latest) Then
                            objPhotos = objPhotoController.List(_moduleID, _albumID, True, Null.NullInteger, False, Me.GallerySettings.RandomTagFilter, Null.NullString(), Null.NullString, SortType.DateApproved, SortDirection.DESC)
                        Else
                            objPhotos = objPhotoController.List(_moduleID, _albumID, True, Null.NullInteger, False, Me.GallerySettings.RandomTagFilter, Null.NullString(), Null.NullString, SortType.Name, SortDirection.DESC)
                        End If
                       
                        Dim objPagedDataSource As New PagedDataSource

                        objPagedDataSource.DataSource = objPhotos

                        objPagedDataSource.AllowPaging = True
                        objPagedDataSource.PageSize = Me.GallerySettings.RandomMaxCount

                        If (GallerySettings.RandomTemplateMode = TemplateModeType.Simple) Then
                            dlGallery.DataSource = objPagedDataSource
                            dlGallery.DataBind()
                        Else
                            rptGallery.DataSource = objPagedDataSource
                            rptGallery.DataBind()
                        End If
                    End If

                    If (GallerySettings.RandomLaunchSlideshow) Then
                        _loadScripts = True
                    End If

                Else

                    Dim objAlbumController As New AlbumController
                    Dim objAlbums As New ArrayList

                    Select Case GallerySettings.RandomMode

                        Case ModeType.Latest
                            objAlbums = objAlbumController.List(_moduleID, _albumID, True, False, AlbumSortType.CreateDate, SortDirection.DESC)
                            Exit Select

                        Case ModeType.Random
                            objAlbums = objAlbumController.List(_moduleID, _albumID, True, False, AlbumSortType.Random, SortDirection.DESC)
                            Exit Select

                        Case ModeType.Caption
                            objAlbums = objAlbumController.List(_moduleID, _albumID, True, False, AlbumSortType.Caption, SortDirection.ASC)
                            Exit Select

                    End Select

                    Dim objPagedDataSource As New PagedDataSource

                    objPagedDataSource.DataSource = objAlbums

                    objPagedDataSource.AllowPaging = True
                    objPagedDataSource.PageSize = Me.GallerySettings.RandomMaxCount

                    If (GallerySettings.RandomTemplateMode = TemplateModeType.Simple) Then
                        dlGallery.DataSource = objPagedDataSource
                        dlGallery.DataBind()
                    Else
                        rptGallery.DataSource = objPagedDataSource
                        rptGallery.DataBind()
                    End If

                End If

            Catch exc As Exception
                ProcessModuleLoadException(Me, exc)
            End Try

        End Sub

        Private Sub Page_PreRender(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.PreRender

            Try

                If (_loadScripts) Then

                    If (HttpContext.Current.Items("SimpleGallery-ScriptsRegistered") Is Nothing) Then

                        If (HttpContext.Current.Items("jquery_registered") Is Nothing And HttpContext.Current.Items("jQueryRequested") Is Nothing And GallerySettings.RandomIncludeJQuery) Then

                            Dim version As Integer = Convert.ToInt32(PortalSettings.Version.Split("."c)(0))
                            If (version >= 6) Then

                                Dim litLink As New Literal
                                litLink.Text = "" & vbCrLf _
                                    & "<script type=""text/javascript"" src='" & Page.ResolveUrl(GallerySettings.LightboxDefaultPath & "?v=" & GallerySettings.JavascriptVersion) & "'></script>" & vbCrLf
                                phjQueryScripts.Controls.Add(litLink)
                            Else


                                Dim jLink As New Literal
                                jLink.Text = "" & vbCrLf _
                                    & "<script type=""text/javascript"" src='" & Me.ResolveUrl("../simplegallery/js/lightbox/jquery.js?v=" & Me.GallerySettings.JavascriptVersion) & "'></script>" & vbCrLf
                                Page.Header.Controls.Add(jLink)

                                Dim litLink As New Literal
                                litLink.Text = "" & vbCrLf _
                                    & "<script type=""text/javascript"" src='" & Page.ResolveUrl(GallerySettings.LightboxDefaultPath & "?v=" & GallerySettings.JavascriptVersion) & "'></script>" & vbCrLf
                                phjQueryScripts.Controls.Add(litLink)
                            End If

                        Else

                            Dim litLink As New Literal
                            litLink.Text = "" & vbCrLf _
                                & "<script type=""text/javascript"" src='" & Page.ResolveUrl(GallerySettings.LightboxDefaultPath & "?v=" & GallerySettings.JavascriptVersion) & "'></script>" & vbCrLf
                            phjQueryScripts.Controls.Add(litLink)

                        End If

                        HttpContext.Current.Items.Add("SimpleGallery-ScriptsRegistered", "true")

                    End If

                End If

                If (GallerySettings.RandomLaunchSlideshow And GallerySettings.RandomAlbumSlideshow = False And GallerySettings.RandomDisplay = DisplayType.Photo) Then
                    Dim script As String = "" _
                        & "<script type=""text/javascript"">" & vbCrLf _
                        & "jQuery(function() {" & vbCrLf _
                        & "jQuery('a[rel*=lightbox" & TabModuleId.ToString() & "-" & ModuleId.ToString() & "]').lightBox({" & vbCrLf _
                        & "imageLoading: '" & Me.ResolveUrl("../simplegallery/images/lightbox/lightbox-ico-loading.gif") & "'," & vbCrLf _
                        & "imageBtnPrev: '" & Me.ResolveUrl("../simplegallery/images/lightbox/lightbox-btn-prev.gif") & "'," & vbCrLf _
                        & "imageBtnNext: '" & Me.ResolveUrl("../simplegallery/images/lightbox/lightbox-btn-next.gif") & "'," & vbCrLf _
                        & "imageBtnClose: '" & Me.ResolveUrl("../simplegallery/images/lightbox/lightbox-btn-close.gif") & "'," & vbCrLf _
                        & "imageBlank: '" & Page.ResolveUrl("~/images/spacer.gif") & "'," & vbCrLf _
                        & "txtImage: '" & Localization.GetString("Image", Me.LocalResourceFile) & "'," & vbCrLf _
                        & "txtOf: '" & Localization.GetString("Of", Me.LocalResourceFile) & "'," & vbCrLf _
                        & "next: '" & Localization.GetString("Next", Me.LocalResourceFile) & "'," & vbCrLf _
                        & "previous: '" & Localization.GetString("Previous", Me.LocalResourceFile) & "'," & vbCrLf _
                        & "close: '" & Localization.GetString("Close", Me.LocalResourceFile) & "'," & vbCrLf _
                        & "download: '" & Localization.GetString("Download", Me.LocalResourceFile) & "'," & vbCrLf _
                        & "txtPlay: '" & Localization.GetString("Play", Me.LocalResourceFile) & "'," & vbCrLf _
                        & "txtPause: '" & Localization.GetString("Pause", Me.LocalResourceFile) & "'," & vbCrLf _
                        & "keyToClose: '" & LinkedGallerySettings(_moduleID).LightboxCloseKey & "'," & vbCrLf _
                        & "keyToPrev: '" & LinkedGallerySettings(_moduleID).LightboxPreviousKey & "'," & vbCrLf _
                        & "keyToNext: '" & LinkedGallerySettings(_moduleID).LightboxNextKey & "'," & vbCrLf _
                        & "keyToDownload: '" & LinkedGallerySettings(_moduleID).LightboxDownloadKey & "'," & vbCrLf _
                        & "slideInterval: '" & (LinkedGallerySettings(_moduleID).LightboxSlideInterval * 1000).ToString() & "'," & vbCrLf _
                        & "hideTitle: " & LinkedGallerySettings(_moduleID).LightboxHideTitle.ToString().ToLower() & "," & vbCrLf _
                        & "hideDescription: " & LinkedGallerySettings(_moduleID).LightboxHideDescription.ToString().ToLower() & "," & vbCrLf _
                        & "hidePaging: " & LinkedGallerySettings(_moduleID).LightboxHidePaging.ToString().ToLower() & "," & vbCrLf _
                        & "hideTags: " & LinkedGallerySettings(_moduleID).LightboxHideTags.ToString().ToLower() & "," & vbCrLf _
                        & "hideDownload: " & LinkedGallerySettings(_moduleID).LightboxHideDownload.ToString().ToLower() & "," & vbCrLf _
                        & "downloadLink: '" & Page.ResolveUrl("~/DesktopModules/SimpleGallery/DownloadPhoto.ashx?PhotoID={0}") & "'" & vbCrLf _
                        & "});" & vbCrLf _
                        & "});" & vbCrLf _
                        & "</script>" & vbCrLf

                    Dim objScript As New Literal
                    objScript.Text = script
                    phLightboxTop.Controls.AddAt(0, objScript)
                End If

            Catch exc As Exception
                ProcessModuleLoadException(Me, exc)
            End Try

        End Sub

        Private Sub BindPhoto(ByVal phPhoto As PlaceHolder, ByVal objPhoto As PhotoInfo)

            If (LinkedGallerySettings(_moduleID).Slideshow = SlideshowType.Lightbox) Then

                If (GallerySettings.RandomLaunchSlideshow And GallerySettings.RandomAlbumSlideshow) Then

                    Dim objSortBy As SortType = LinkedGallerySettings(_moduleID).SortBy
                    Dim objSortDirection As SortDirection = LinkedGallerySettings(_moduleID).SortDirection

                    If (GallerySettings.RandomMode = ModeType.Latest) Then
                        objSortBy = SortType.DateApproved
                        objSortDirection = SortDirection.DESC
                    End If

                    Dim objPhotoController As New PhotoController
                    Dim objPhotoList As ArrayList = objPhotoController.List(_moduleID, objPhoto.AlbumID, True, Null.NullInteger, Null.NullBoolean, GallerySettings.RandomTagFilter, Null.NullString, Null.NullString, objSortBy, objSortDirection)

                    Dim isBefore As Boolean = True
                    For Each objPhotoSelected As PhotoInfo In objPhotoList
                        Dim tags As String = ""

                        If (objPhotoSelected.Tags <> "") Then

                            Dim objModuleController As New ModuleController
                            Dim objTabModules As Dictionary(Of Integer, ModuleInfo) = objModuleController.GetTabModules(_pageID)

                            Dim tabModuleID As Integer = Null.NullInteger
                            If (objTabModules.ContainsKey(_moduleID)) Then
                                tabModuleID = objTabModules(_moduleID).TabModuleID
                            End If

                            For Each tag As String In objPhotoSelected.Tags.Split(","c)
                                If (tags.Length = 0) Then
                                    tags = "(" & Localization.GetString("Tags", Me.LocalResourceFile) & ": <a href='" & NavigateURL(_pageID, "", "tag=" & tag, "Tags=" & tabModuleID) & "'>" & tag & "</a>"
                                Else
                                    tags = tags & ", <a href='" & NavigateURL(_pageID, "", "tag=" & tag, "Tags=" & tabModuleID) & "'>" & tag & "</a>"
                                End If
                            Next

                            If (tags.Length > 0) Then
                                tags = tags & ")"
                            End If

                        End If

                        If (objPhoto.PhotoID = objPhotoSelected.PhotoID) Then
                            isBefore = False
                        Else
                            Dim objLiteral As New Literal
                            objLiteral.Text = "<a href=""" & Me.GetPhotoPathFull(objPhotoSelected) & """ rel=""lightbox" & TabModuleId.ToString() & "-" & objPhoto.PhotoID.ToString() & """ title=""" & GetAlternateText(objPhotoSelected) & """  tags=""" & tags & """></a>"
                            If (isBefore) Then
                                phLightboxTop.Controls.Add(objLiteral)
                            Else
                                phLightboxBottom.Controls.Add(objLiteral)
                            End If
                        End If

                    Next

                    Dim script As String = "" _
                        & "<script type=""text/javascript"">" & vbCrLf _
                        & "jQuery(function() {" & vbCrLf _
                        & "jQuery('a[rel*=lightbox" & TabModuleId.ToString() & "-" & objPhoto.PhotoID.ToString() & "]').lightBox({" & vbCrLf _
                        & "imageLoading: '" & Me.ResolveUrl("../simplegallery/images/lightbox/lightbox-ico-loading.gif") & "'," & vbCrLf _
                        & "imageBtnPrev: '" & Me.ResolveUrl("../simplegallery/images/lightbox/lightbox-btn-prev.gif") & "'," & vbCrLf _
                        & "imageBtnNext: '" & Me.ResolveUrl("../simplegallery/images/lightbox/lightbox-btn-next.gif") & "'," & vbCrLf _
                        & "imageBtnClose: '" & Me.ResolveUrl("../simplegallery/images/lightbox/lightbox-btn-close.gif") & "'," & vbCrLf _
                        & "imageBlank: '" & Page.ResolveUrl("~/images/spacer.gif") & "'," & vbCrLf _
                        & "txtImage: '" & Localization.GetString("Image", Me.LocalResourceFile) & "'," & vbCrLf _
                        & "txtOf: '" & Localization.GetString("Of", Me.LocalResourceFile) & "'," & vbCrLf _
                        & "next: '" & Localization.GetString("Next", Me.LocalResourceFile) & "'," & vbCrLf _
                        & "previous: '" & Localization.GetString("Previous", Me.LocalResourceFile) & "'," & vbCrLf _
                        & "close: '" & Localization.GetString("Close", Me.LocalResourceFile) & "'," & vbCrLf _
                        & "download: '" & Localization.GetString("Download", Me.LocalResourceFile) & "'," & vbCrLf _
                        & "txtPlay: '" & Localization.GetString("Play", Me.LocalResourceFile) & "'," & vbCrLf _
                        & "txtPause: '" & Localization.GetString("Pause", Me.LocalResourceFile) & "'," & vbCrLf _
                        & "keyToClose: '" & LinkedGallerySettings(_moduleID).LightboxCloseKey & "'," & vbCrLf _
                        & "keyToPrev: '" & LinkedGallerySettings(_moduleID).LightboxPreviousKey & "'," & vbCrLf _
                        & "keyToNext: '" & LinkedGallerySettings(_moduleID).LightboxNextKey & "'," & vbCrLf _
                        & "keyToDownload: '" & LinkedGallerySettings(_moduleID).LightboxDownloadKey & "'," & vbCrLf _
                        & "slideInterval: '" & (LinkedGallerySettings(_moduleID).LightboxSlideInterval * 1000).ToString() & "'," & vbCrLf _
                        & "hideTitle: " & LinkedGallerySettings(_moduleID).LightboxHideTitle.ToString().ToLower() & "," & vbCrLf _
                        & "hideDescription: " & LinkedGallerySettings(_moduleID).LightboxHideDescription.ToString().ToLower() & "," & vbCrLf _
                        & "hidePaging: " & LinkedGallerySettings(_moduleID).LightboxHidePaging.ToString().ToLower() & "," & vbCrLf _
                        & "hideTags: " & LinkedGallerySettings(_moduleID).LightboxHideTags.ToString().ToLower() & "," & vbCrLf _
                        & "hideDownload: " & LinkedGallerySettings(_moduleID).LightboxHideDownload.ToString().ToLower() & "," & vbCrLf _
                        & "downloadLink: '" & Page.ResolveUrl("~/DesktopModules/SimpleGallery/DownloadPhoto.ashx?PhotoID={0}") & "'" & vbCrLf _
                        & "});" & vbCrLf _
                        & "});" & vbCrLf _
                        & "</script>" & vbCrLf

                    Dim objScript As New Literal
                    objScript.Text = script
                    phLightboxTop.Controls.AddAt(0, objScript)
                End If

            End If

            For iPtr As Integer = 0 To _templateTokens.Length - 1 Step 2

                phPhoto.Controls.Add(New LiteralControl(_templateTokens(iPtr).ToString()))

                If iPtr < _templateTokens.Length - 1 Then

                    Select Case _templateTokens(iPtr + 1)
                        Case "ALBUMID"
                            Dim objLiteral As New Literal
                            objLiteral.ID = Globals.CreateValidID("Gallery" & objPhoto.PhotoID.ToString() & "-" & iPtr.ToString())
                            objLiteral.Text = objPhoto.AlbumID.ToString()
                            phPhoto.Controls.Add(objLiteral)
                        Case "ALBUMLINK"
                            Dim objLiteral As New Literal
                            objLiteral.ID = Globals.CreateValidID("Gallery" & objPhoto.PhotoID.ToString() & "-" & iPtr.ToString())
                            objLiteral.Text = Me.GetAlbumUrl(objPhoto.AlbumID.ToString())
                            phPhoto.Controls.Add(objLiteral)
                        Case "ALBUMNAME"
                            Dim objLiteral As New Literal
                            objLiteral.ID = Globals.CreateValidID("Gallery" & objPhoto.PhotoID.ToString() & "-" & iPtr.ToString())
                            objLiteral.Text = objPhoto.AlbumName
                            phPhoto.Controls.Add(objLiteral)
                        Case "AUTHORID"
                            Dim objLiteral As New Literal
                            objLiteral.ID = Globals.CreateValidID("Gallery" & objPhoto.PhotoID.ToString() & "-" & iPtr.ToString())
                            objLiteral.Text = objPhoto.AuthorID.ToString()
                            phPhoto.Controls.Add(objLiteral)
                        Case "AUTHORDISPLAYNAME"
                            Dim objLiteral As New Literal
                            objLiteral.ID = Globals.CreateValidID("Gallery" & objPhoto.PhotoID.ToString() & "-" & iPtr.ToString())
                            objLiteral.Text = objPhoto.AuthorDisplayName.ToString()
                            phPhoto.Controls.Add(objLiteral)
                        Case "AUTHORFIRSTNAME"
                            Dim objLiteral As New Literal
                            objLiteral.ID = Globals.CreateValidID("Gallery" & objPhoto.PhotoID.ToString() & "-" & iPtr.ToString())
                            objLiteral.Text = objPhoto.AuthorFirstName.ToString()
                            phPhoto.Controls.Add(objLiteral)
                        Case "AUTHORLASTNAME"
                            Dim objLiteral As New Literal
                            objLiteral.ID = Globals.CreateValidID("Gallery" & objPhoto.PhotoID.ToString() & "-" & iPtr.ToString())
                            objLiteral.Text = objPhoto.AuthorLastName.ToString()
                            phPhoto.Controls.Add(objLiteral)
                        Case "AUTHORUSERNAME"
                            Dim objLiteral As New Literal
                            objLiteral.ID = Globals.CreateValidID("Gallery" & objPhoto.PhotoID.ToString() & "-" & iPtr.ToString())
                            objLiteral.Text = objPhoto.AuthorUserName.ToString()
                            phPhoto.Controls.Add(objLiteral)
                        Case "APPROVERID"
                            Dim objLiteral As New Literal
                            objLiteral.ID = Globals.CreateValidID("Gallery" & objPhoto.PhotoID.ToString() & "-" & iPtr.ToString())
                            objLiteral.Text = objPhoto.ApproverID.ToString()
                            phPhoto.Controls.Add(objLiteral)
                        Case "APPROVERFIRSTNAME"
                            Dim objLiteral As New Literal
                            objLiteral.ID = Globals.CreateValidID("Gallery" & objPhoto.PhotoID.ToString() & "-" & iPtr.ToString())
                            objLiteral.Text = objPhoto.ApproverFirstName.ToString()
                            phPhoto.Controls.Add(objLiteral)
                        Case "APPROVERLASTNAMED"
                            Dim objLiteral As New Literal
                            objLiteral.ID = Globals.CreateValidID("Gallery" & objPhoto.PhotoID.ToString() & "-" & iPtr.ToString())
                            objLiteral.Text = objPhoto.ApproverLastName.ToString()
                            phPhoto.Controls.Add(objLiteral)
                        Case "APPROVERUSERNAME"
                            Dim objLiteral As New Literal
                            objLiteral.ID = Globals.CreateValidID("Gallery" & objPhoto.PhotoID.ToString() & "-" & iPtr.ToString())
                            objLiteral.Text = objPhoto.ApproverUserName.ToString()
                            phPhoto.Controls.Add(objLiteral)
                        Case "DATEAPPROVED"
                            Dim objLiteral As New Literal
                            objLiteral.ID = Globals.CreateValidID("Gallery" & objPhoto.PhotoID.ToString() & "-" & iPtr.ToString())
                            objLiteral.Text = objPhoto.DateApproved.ToString.ToString()
                            phPhoto.Controls.Add(objLiteral)
                        Case "DATECREATED"
                            Dim objLiteral As New Literal
                            objLiteral.ID = Globals.CreateValidID("Gallery" & objPhoto.PhotoID.ToString() & "-" & iPtr.ToString())
                            objLiteral.Text = objPhoto.DateCreated.ToString.ToString()
                            phPhoto.Controls.Add(objLiteral)
                        Case "DATEUPDATED"
                            Dim objLiteral As New Literal
                            objLiteral.ID = Globals.CreateValidID("Gallery" & objPhoto.PhotoID.ToString() & "-" & iPtr.ToString())
                            objLiteral.Text = objPhoto.DateUpdated.ToString.ToString()
                            phPhoto.Controls.Add(objLiteral)
                        Case "DESCRIPTION"
                            Dim objLiteral As New Literal
                            objLiteral.ID = Globals.CreateValidID("Gallery" & objPhoto.PhotoID.ToString() & "-" & iPtr.ToString())
                            objLiteral.Text = objPhoto.Description.ToString()
                            phPhoto.Controls.Add(objLiteral)
                        Case "DOWNLOADLINK"
                            Dim objLiteral As New Literal
                            objLiteral.ID = Globals.CreateValidID("Gallery" & objPhoto.PhotoID.ToString() & "-" & iPtr.ToString())
                            objLiteral.Text = Page.ResolveUrl("~/DesktopModules/SimpleGallery/DownloadPhoto.ashx?PhotoID=" & objPhoto.PhotoID.ToString())
                            phPhoto.Controls.Add(objLiteral)
                        Case "FILENAME"
                            Dim objLiteral As New Literal
                            objLiteral.ID = Globals.CreateValidID("Gallery" & objPhoto.PhotoID.ToString() & "-" & iPtr.ToString())
                            objLiteral.Text = objPhoto.FileName.ToString()
                            phPhoto.Controls.Add(objLiteral)
                        Case "HEIGHT"
                            Dim objLiteral As New Literal
                            objLiteral.ID = Globals.CreateValidID("Gallery" & objPhoto.PhotoID.ToString() & "-" & iPtr.ToString())
                            objLiteral.Text = objPhoto.Height.ToString()
                            phPhoto.Controls.Add(objLiteral)
                        Case "HOMEDIRECTORY"
                            Dim objLiteral As New Literal
                            objLiteral.ID = Globals.CreateValidID("Gallery" & objPhoto.PhotoID.ToString() & "-" & iPtr.ToString())
                            objLiteral.Text = objPhoto.HomeDirectory.ToString()
                            phPhoto.Controls.Add(objLiteral)
                        Case "PHOTO"
                            Dim objLiteral As New Literal
                            objLiteral.ID = Globals.CreateValidID("Gallery" & objPhoto.PhotoID.ToString() & "-" & iPtr.ToString())
                            objLiteral.Text = "<img src=""" & GetPhotoPath(objPhoto) & """ class=""photo_198"" width=""" & GetPhotoWidth(objPhoto) & """ height=""" & GetPhotoHeight(objPhoto) & """ alt=""" & GetAlternateText(objPhoto) & """>"
                            phPhoto.Controls.Add(objLiteral)
                        Case "PHOTOLINK"
                            Dim objLiteral As New Literal
                            objLiteral.ID = Globals.CreateValidID("Gallery" & objPhoto.AlbumID.ToString() & "-" & iPtr.ToString())
                            objLiteral.Text = GetPhotoPath(objPhoto)
                            phPhoto.Controls.Add(objLiteral)
                        Case "PHOTOWITHBORDER"
                            Dim objLiteral As New Literal
                            objLiteral.ID = Globals.CreateValidID("Gallery" & objPhoto.PhotoID.ToString() & "-" & iPtr.ToString())

                            Select Case LinkedGallerySettings(_moduleID).Slideshow
                                Case SlideshowType.Lightbox
                                    Dim tags As String = ""

                                    If (objPhoto.Tags <> "") Then

                                        Dim objModuleController As New ModuleController
                                        Dim objTabModules As Dictionary(Of Integer, ModuleInfo) = objModuleController.GetTabModules(_pageID)

                                        Dim tabModuleID As Integer = Null.NullInteger
                                        If (objTabModules.ContainsKey(_moduleID)) Then
                                            tabModuleID = objTabModules(_moduleID).TabModuleID
                                        End If

                                        For Each tag As String In objPhoto.Tags.Split(","c)
                                            If (tags.Length = 0) Then
                                                tags = "(" & Localization.GetString("Tags", Me.LocalResourceFile) & ": <a href='" & NavigateURL(_pageID, "", "tag=" & tag, "Tags=" & tabModuleID) & "'>" & tag & "</a>"
                                            Else
                                                tags = tags & ", <a href='" & NavigateURL(_pageID, "", "tag=" & tag, "Tags=" & tabModuleID) & "'>" & tag & "</a>"
                                            End If
                                        Next

                                        If (tags.Length > 0) Then
                                            tags = tags & ")"
                                        End If

                                    End If

                                    If (GallerySettings.RandomLaunchSlideshow) Then
                                        If (GallerySettings.RandomAlbumSlideshow) Then
                                            objLiteral.Text = "<table class=""photo-frame"">" _
                                               & "<tr><td class=""topx--""></td><td class=""top-x-""></td><td class=""top--x""></td></tr>" _
                                               & "<tr><td class=""midx--""></td><td valign=""top""><a href=""" & Me.GetPhotoPathFull(objPhoto) & """ rel=""lightbox" & TabModuleId.ToString() & "-" & objPhoto.PhotoID.ToString() & """ title=""" & GetTitle(objPhoto) & """  tags=""" & tags & """ description=""" & GetDescription(objPhoto) & """ pid=""" & objPhoto.PhotoID.ToString() & """><img src=""" & GetPhotoPath(objPhoto) & """ class=""photo_198"" width=""" & GetPhotoWidth(objPhoto) & """ height=""" & GetPhotoHeight(objPhoto) & """ alt=""" & GetAlternateText(objPhoto) & """></a></td><td class=""mid--x""></td></tr>" _
                                               & "<tr><td class=""botx--""></td><td class=""bot-x-""></td><td class=""bot--x""></td></tr>" _
                                               & "</table>"
                                        Else
                                            objLiteral.Text = "<table class=""photo-frame"">" _
                                               & "<tr><td class=""topx--""></td><td class=""top-x-""></td><td class=""top--x""></td></tr>" _
                                               & "<tr><td class=""midx--""></td><td valign=""top""><a href=""" & Me.GetPhotoPathFull(objPhoto) & """ rel=""lightbox" & TabModuleId.ToString() & "-" & ModuleId.ToString() & """ title=""" & GetTitle(objPhoto) & """  tags=""" & tags & """ description=""" & GetDescription(objPhoto) & """ pid=""" & objPhoto.PhotoID.ToString() & """><img src=""" & GetPhotoPath(objPhoto) & """ class=""photo_198"" width=""" & GetPhotoWidth(objPhoto) & """ height=""" & GetPhotoHeight(objPhoto) & """ alt=""" & GetAlternateText(objPhoto) & """></a></td><td class=""mid--x""></td></tr>" _
                                               & "<tr><td class=""botx--""></td><td class=""bot-x-""></td><td class=""bot--x""></td></tr>" _
                                               & "</table>"
                                        End If
                                    Else
                                        objLiteral.Text = "<table class=""photo-frame"">" _
                                           & "<tr><td class=""topx--""></td><td class=""top-x-""></td><td class=""top--x""></td></tr>" _
                                           & "<tr><td class=""midx--""></td><td valign=""top""><a href=""" & Me.GetAlbumUrl(objPhoto.AlbumID.ToString()) & """><img src=""" & GetPhotoPath(objPhoto) & """ class=""photo_198"" width=""" & GetPhotoWidth(objPhoto) & """ height=""" & GetPhotoHeight(objPhoto) & """ alt=""" & GetAlternateText(objPhoto) & """></a></td><td class=""mid--x""></td></tr>" _
                                           & "<tr><td class=""botx--""></td><td class=""bot-x-""></td><td class=""bot--x""></td></tr>" _
                                           & "</table>"
                                    End If

                                Case Else
                                    If (GallerySettings.RandomLaunchSlideshow) Then
                                        objLiteral.Text = "<table class=""photo-frame"">" _
                                           & "<tr><td class=""topx--""></td><td class=""top-x-""></td><td class=""top--x""></td></tr>" _
                                           & "<tr><td class=""midx--""></td><td valign=""top""><a href=""" & Me.GetPhotoUrl(objPhoto.PhotoID.ToString()) & """><img src=""" & GetPhotoPath(objPhoto) & """ class=""photo_198"" width=""" & GetPhotoWidth(objPhoto) & """ height=""" & GetPhotoHeight(objPhoto) & """ alt=""" & GetAlternateText(objPhoto) & """></a></td><td class=""mid--x""></td></tr>" _
                                           & "<tr><td class=""botx--""></td><td class=""bot-x-""></td><td class=""bot--x""></td></tr>" _
                                           & "</table>"
                                    Else
                                        objLiteral.Text = "<table class=""photo-frame"">" _
                                           & "<tr><td class=""topx--""></td><td class=""top-x-""></td><td class=""top--x""></td></tr>" _
                                           & "<tr><td class=""midx--""></td><td valign=""top""><a href=""" & Me.GetAlbumUrl(objPhoto.AlbumID.ToString()) & """><img src=""" & GetPhotoPath(objPhoto) & """ class=""photo_198"" width=""" & GetPhotoWidth(objPhoto) & """ height=""" & GetPhotoHeight(objPhoto) & """ alt=""" & GetAlternateText(objPhoto) & """></a></td><td class=""mid--x""></td></tr>" _
                                           & "<tr><td class=""botx--""></td><td class=""bot-x-""></td><td class=""bot--x""></td></tr>" _
                                           & "</table>"
                                    End If

                            End Select
                            phPhoto.Controls.Add(objLiteral)
                        Case "PHOTOWITHBORDERNOALT"
                            Dim objLiteral As New Literal
                            objLiteral.ID = Globals.CreateValidID("Gallery" & objPhoto.PhotoID.ToString() & "-" & iPtr.ToString())

                            Select Case LinkedGallerySettings(_moduleID).Slideshow
                                Case SlideshowType.Lightbox
                                    Dim tags As String = ""

                                    If (objPhoto.Tags <> "") Then

                                        Dim objModuleController As New ModuleController
                                        Dim objTabModules As Dictionary(Of Integer, ModuleInfo) = objModuleController.GetTabModules(_pageID)

                                        Dim tabModuleID As Integer = Null.NullInteger
                                        If (objTabModules.ContainsKey(_moduleID)) Then
                                            tabModuleID = objTabModules(_moduleID).TabModuleID
                                        End If

                                        For Each tag As String In objPhoto.Tags.Split(","c)
                                            If (tags.Length = 0) Then
                                                tags = "(" & Localization.GetString("Tags", Me.LocalResourceFile) & ": <a href='" & NavigateURL(_pageID, "", "tag=" & tag, "Tags=" & tabModuleID) & "'>" & tag & "</a>"
                                            Else
                                                tags = tags & ", <a href='" & NavigateURL(_pageID, "", "tag=" & tag, "Tags=" & tabModuleID) & "'>" & tag & "</a>"
                                            End If
                                        Next

                                        If (tags.Length > 0) Then
                                            tags = tags & ")"
                                        End If

                                    End If

                                    If (GallerySettings.RandomLaunchSlideshow) Then
                                        If (GallerySettings.RandomAlbumSlideshow) Then
                                            objLiteral.Text = "<table class=""photo-frame"">" _
                                               & "<tr><td class=""topx--""></td><td class=""top-x-""></td><td class=""top--x""></td></tr>" _
                                               & "<tr><td class=""midx--""></td><td valign=""top""><a href=""" & Me.GetPhotoPathFull(objPhoto) & """ rel=""lightbox" & TabModuleId.ToString() & "-" & objPhoto.PhotoID.ToString() & """ tags=""" & tags & """ pid=""" & objPhoto.PhotoID.ToString() & """><img src=""" & GetPhotoPath(objPhoto) & """ class=""photo_198"" width=""" & GetPhotoWidth(objPhoto) & """ height=""" & GetPhotoHeight(objPhoto) & """></a></td><td class=""mid--x""></td></tr>" _
                                               & "<tr><td class=""botx--""></td><td class=""bot-x-""></td><td class=""bot--x""></td></tr>" _
                                               & "</table>"
                                        Else
                                            objLiteral.Text = "<table class=""photo-frame"">" _
                                               & "<tr><td class=""topx--""></td><td class=""top-x-""></td><td class=""top--x""></td></tr>" _
                                               & "<tr><td class=""midx--""></td><td valign=""top""><a href=""" & Me.GetPhotoPathFull(objPhoto) & """ rel=""lightbox" & TabModuleId.ToString() & "-" & ModuleId.ToString() & """ tags=""" & tags & """ pid=""" & objPhoto.PhotoID.ToString() & """><img src=""" & GetPhotoPath(objPhoto) & """ class=""photo_198"" width=""" & GetPhotoWidth(objPhoto) & """ height=""" & GetPhotoHeight(objPhoto) & """></a></td><td class=""mid--x""></td></tr>" _
                                               & "<tr><td class=""botx--""></td><td class=""bot-x-""></td><td class=""bot--x""></td></tr>" _
                                               & "</table>"
                                        End If
                                    Else
                                        objLiteral.Text = "<table class=""photo-frame"">" _
                                           & "<tr><td class=""topx--""></td><td class=""top-x-""></td><td class=""top--x""></td></tr>" _
                                           & "<tr><td class=""midx--""></td><td valign=""top""><a href=""" & Me.GetAlbumUrl(objPhoto.AlbumID.ToString()) & """><img src=""" & GetPhotoPath(objPhoto) & """ class=""photo_198"" width=""" & GetPhotoWidth(objPhoto) & """ height=""" & GetPhotoHeight(objPhoto) & """></a></td><td class=""mid--x""></td></tr>" _
                                           & "<tr><td class=""botx--""></td><td class=""bot-x-""></td><td class=""bot--x""></td></tr>" _
                                           & "</table>"
                                    End If

                                Case Else
                                    If (GallerySettings.RandomLaunchSlideshow) Then
                                        objLiteral.Text = "<table class=""photo-frame"">" _
                                           & "<tr><td class=""topx--""></td><td class=""top-x-""></td><td class=""top--x""></td></tr>" _
                                           & "<tr><td class=""midx--""></td><td valign=""top""><a href=""" & Me.GetPhotoUrl(objPhoto.PhotoID.ToString()) & """><img src=""" & GetPhotoPath(objPhoto) & """ class=""photo_198"" width=""" & GetPhotoWidth(objPhoto) & """ height=""" & GetPhotoHeight(objPhoto) & """></a></td><td class=""mid--x""></td></tr>" _
                                           & "<tr><td class=""botx--""></td><td class=""bot-x-""></td><td class=""bot--x""></td></tr>" _
                                           & "</table>"
                                    Else
                                        objLiteral.Text = "<table class=""photo-frame"">" _
                                           & "<tr><td class=""topx--""></td><td class=""top-x-""></td><td class=""top--x""></td></tr>" _
                                           & "<tr><td class=""midx--""></td><td valign=""top""><a href=""" & Me.GetAlbumUrl(objPhoto.AlbumID.ToString()) & """><img src=""" & GetPhotoPath(objPhoto) & """ class=""photo_198"" width=""" & GetPhotoWidth(objPhoto) & """ height=""" & GetPhotoHeight(objPhoto) & """></a></td><td class=""mid--x""></td></tr>" _
                                           & "<tr><td class=""botx--""></td><td class=""bot-x-""></td><td class=""bot--x""></td></tr>" _
                                           & "</table>"
                                    End If

                            End Select
                            phPhoto.Controls.Add(objLiteral)
                        Case "PHOTOWITHBORDERNOLINK"
                            Dim objLiteral As New Literal
                            objLiteral.ID = Globals.CreateValidID("Gallery" & objPhoto.PhotoID.ToString() & "-" & iPtr.ToString())
                            objLiteral.Text = "<table class=""photo-frame"">" _
                            & "<tr><td class=""topx--""></td><td class=""top-x-""></td><td class=""top--x""></td></tr>" _
                            & "<tr><td class=""midx--""></td><td valign=""top""><img src=""" & GetPhotoPath(objPhoto) & """ class=""photo_198"" width=""" & GetPhotoWidth(objPhoto) & """ height=""" & GetPhotoHeight(objPhoto) & """ alt=""" & GetAlternateText(objPhoto) & """></td><td class=""mid--x""></td></tr>" _
                            & "<tr><td class=""botx--""></td><td class=""bot-x-""></td><td class=""bot--x""></td></tr>" _
                            & "</table>"
                            phPhoto.Controls.Add(objLiteral)
                        Case "PHOTOWITHOUTBORDER"
                            Dim objLiteral As New Literal
                            objLiteral.ID = Globals.CreateValidID("Gallery" & objPhoto.PhotoID.ToString() & "-" & iPtr.ToString())

                            Select Case LinkedGallerySettings(_moduleID).Slideshow
                                Case SlideshowType.Lightbox
                                    Dim tags As String = ""

                                    If (objPhoto.Tags <> "") Then

                                        Dim objModuleController As New ModuleController
                                        Dim objTabModules As Dictionary(Of Integer, ModuleInfo) = objModuleController.GetTabModules(_pageID)

                                        Dim tabModuleID As Integer = Null.NullInteger
                                        If (objTabModules.ContainsKey(_moduleID)) Then
                                            tabModuleID = objTabModules(_moduleID).TabModuleID
                                        End If

                                        For Each tag As String In objPhoto.Tags.Split(","c)
                                            If (tags.Length = 0) Then
                                                tags = "(" & Localization.GetString("Tags", Me.LocalResourceFile) & ": <a href='" & NavigateURL(_pageID, "", "tag=" & tag, "Tags=" & tabModuleID) & "'>" & tag & "</a>"
                                            Else
                                                tags = tags & ", <a href='" & NavigateURL(_pageID, "", "tag=" & tag, "Tags=" & tabModuleID) & "'>" & tag & "</a>"
                                            End If
                                        Next

                                        If (tags.Length > 0) Then
                                            tags = tags & ")"
                                        End If

                                    End If

                                    If (GallerySettings.RandomLaunchSlideshow) Then
                                        If (GallerySettings.RandomAlbumSlideshow) Then
                                            objLiteral.Text = "<a href=""" & Me.GetPhotoPathFull(objPhoto) & """ rel=""lightbox" & TabModuleId.ToString() & "-" & objPhoto.PhotoID.ToString() & """ title=""" & GetAlternateText(objPhoto) & """  tags=""" & tags & """ description=""" & GetDescription(objPhoto) & """ pid=""" & objPhoto.PhotoID.ToString() & """><img src=""" & GetPhotoPath(objPhoto) & """ class=""photo_198"" width=""" & GetPhotoWidth(objPhoto) & """ height=""" & GetPhotoHeight(objPhoto) & """ alt=""" & GetAlternateText(objPhoto) & """></a>"
                                        Else
                                            objLiteral.Text = "<a href=""" & Me.GetPhotoPathFull(objPhoto) & """ rel=""lightbox" & TabModuleId.ToString() & "-" & ModuleId.ToString() & """ title=""" & GetAlternateText(objPhoto) & """  tags=""" & tags & """ description=""" & GetDescription(objPhoto) & """ pid=""" & objPhoto.PhotoID.ToString() & """><img src=""" & GetPhotoPath(objPhoto) & """ class=""photo_198"" width=""" & GetPhotoWidth(objPhoto) & """ height=""" & GetPhotoHeight(objPhoto) & """ alt=""" & GetAlternateText(objPhoto) & """></a>"
                                        End If
                                    Else
                                        objLiteral.Text = "<a href=""" & Me.GetAlbumUrl(objPhoto.AlbumID.ToString()) & """><img src=""" & GetPhotoPath(objPhoto) & """ class=""photo_198"" width=""" & GetPhotoWidth(objPhoto) & """ height=""" & GetPhotoHeight(objPhoto) & """ alt=""" & GetAlternateText(objPhoto) & """></a>"
                                    End If

                                Case Else
                                    If (GallerySettings.RandomLaunchSlideshow) Then
                                        objLiteral.Text = "<a href=""" & Me.GetPhotoUrl(objPhoto.PhotoID.ToString()) & """><img src=""" & GetPhotoPath(objPhoto) & """ class=""photo_198"" width=""" & GetPhotoWidth(objPhoto) & """ height=""" & GetPhotoHeight(objPhoto) & """ alt=""" & GetAlternateText(objPhoto) & """></a>"
                                    Else
                                        objLiteral.Text = "<a href=""" & Me.GetAlbumUrl(objPhoto.AlbumID.ToString()) & """><img src=""" & GetPhotoPath(objPhoto) & """ class=""photo_198"" width=""" & GetPhotoWidth(objPhoto) & """ height=""" & GetPhotoHeight(objPhoto) & """ alt=""" & GetAlternateText(objPhoto) & """></a>"
                                    End If

                            End Select
                            phPhoto.Controls.Add(objLiteral)
                        Case "THUMBNAILLINK"
                            Dim objLiteral As New Literal
                            objLiteral.ID = Globals.CreateValidID("Gallery" & objPhoto.PhotoID.ToString() & "-" & iPtr.ToString())
                            objLiteral.Text = Me.GetPhotoPath(objPhoto)
                            phPhoto.Controls.Add(objLiteral)
                        Case "TITLE"
                            Dim objLiteral As New Literal
                            objLiteral.ID = Globals.CreateValidID("Gallery" & objPhoto.PhotoID.ToString() & "-" & iPtr.ToString())
                            objLiteral.Text = objPhoto.Name.ToString()
                            phPhoto.Controls.Add(objLiteral)
                        Case "WIDTH"
                            Dim objLiteral As New Literal
                            objLiteral.ID = Globals.CreateValidID("Gallery" & objPhoto.PhotoID.ToString() & "-" & iPtr.ToString())
                            objLiteral.Text = objPhoto.Width.ToString()
                            phPhoto.Controls.Add(objLiteral)
                        Case Else
                            If (_templateTokens(iPtr + 1).ToUpper().StartsWith("ISINROLE:")) Then
                                Dim field As String = _templateTokens(iPtr + 1).Substring(9, _templateTokens(iPtr + 1).Length - 9)
                                If (PortalSecurity.IsInRole(field) = False) Then
                                    Dim endToken As String = "/" & _templateTokens(iPtr + 1)
                                    While (iPtr < _templateTokens.Length - 1)
                                        If (_templateTokens(iPtr + 1) = endToken) Then
                                            Exit While
                                        End If
                                        iPtr = iPtr + 1
                                    End While
                                End If
                                Exit Select
                            End If

                            If (_templateTokens(iPtr + 1).ToUpper().StartsWith("/ISINROLE:")) Then
                                ' Do Nothing
                                Exit Select
                            End If

                            If (_templateTokens(iPtr + 1).ToUpper().StartsWith("ISNOTINROLE:")) Then
                                Dim field As String = _templateTokens(iPtr + 1).Substring(12, _templateTokens(iPtr + 1).Length - 12)
                                If (PortalSecurity.IsInRole(field) = True) Then
                                    Dim endToken As String = "/" & _templateTokens(iPtr + 1)
                                    While (iPtr < _templateTokens.Length - 1)
                                        If (_templateTokens(iPtr + 1) = endToken) Then
                                            Exit While
                                        End If
                                        iPtr = iPtr + 1
                                    End While
                                End If
                                Exit Select
                            End If

                            If (_templateTokens(iPtr + 1).ToUpper().StartsWith("/ISNOTINROLE:")) Then
                                ' Do Nothing
                                Exit Select
                            End If

                            Dim objLiteralOther As New Literal
                            objLiteralOther.Text = "[" & _templateTokens(iPtr + 1) & "]"
                            objLiteralOther.EnableViewState = False
                            phPhoto.Controls.Add(objLiteralOther)
                    End Select
                End If
            Next

        End Sub

        Private Sub BindAlbum(ByVal phPhoto As PlaceHolder, ByVal objAlbum As AlbumInfo)

            For iPtr As Integer = 0 To _templateTokens.Length - 1 Step 2

                phPhoto.Controls.Add(New LiteralControl(_templateTokens(iPtr).ToString()))

                If iPtr < _templateTokens.Length - 1 Then

                    Select Case _templateTokens(iPtr + 1)
                        Case "ALBUM"
                            Dim objSettings As Hashtable = GetAlbumPath(objAlbum.AlbumID.ToString(), objAlbum.ModuleID, objAlbum.HomeDirectory)

                            Dim objLiteral As New Literal
                            objLiteral.ID = Globals.CreateValidID("Article" & objAlbum.AlbumID.ToString() & "-" & iPtr.ToString())
                            objLiteral.Text = "<img src=""" & objSettings("AlbumPath").ToString() & """ class=""photo_198"" alt=""" & GetAlternateTextForAlbum(objAlbum) & """ width=""" & objSettings("AlbumWidth").ToString() & """ height=""" & objSettings("AlbumHeight").ToString() & """>"
                            phPhoto.Controls.Add(objLiteral)
                        Case "ALBUMCOUNT"
                            Dim objLiteral As New Literal
                            objLiteral.ID = Globals.CreateValidID("Article" & objAlbum.AlbumID.ToString() & "-" & iPtr.ToString())
                            objLiteral.Text = GetAlbumCount(objAlbum)
                            phPhoto.Controls.Add(objLiteral)
                        Case "ALBUMLINK"
                            Dim objLiteral As New Literal
                            objLiteral.ID = Globals.CreateValidID("Article" & objAlbum.AlbumID.ToString() & "-" & iPtr.ToString())
                            objLiteral.Text = GetAlbumUrl(objAlbum.AlbumID.ToString())
                            phPhoto.Controls.Add(objLiteral)
                        Case "ALBUMWITHBORDER"
                            Dim objSettings As Hashtable = GetAlbumPath(objAlbum.AlbumID.ToString(), objAlbum.ModuleID, objAlbum.HomeDirectory)

                            Dim objLiteral As New Literal
                            objLiteral.ID = Globals.CreateValidID("Article" & objAlbum.AlbumID.ToString() & "-" & iPtr.ToString())
                            If (GallerySettings.BorderStyle = "White") Then
                                objLiteral.Text = "" _
                                    & "<table border=""0"" cellpadding=""0"" cellspacing=""0"" class=""album-frame"">" _
                                    & "<tr><td class=""topx----""><img src=""" & Me.FormatBorderPath("album-l1.gif") & """ border=""0"" width=""14"" height=""14""></td><td class=""top-x---""><img src=""" & Me.FormatBorderPath("album-mtl.png") & """ border=""0"" height=""14""></td><td class=""top--x--""></td><td class=""top---x-""><img src=""" & Me.FormatBorderPath("album-mtr.png") & """ border=""0"" height=""14""></td><td class=""top----""><img src=""" & Me.FormatBorderPath("album-r1.gif") & """ border=""0"" width=""14"" height=""14""></td></tr>" _
                                    & "<tr><td class=""mtpx----""><img src=""" & Me.FormatBorderPath("album-l2.png") & """ border=""0"" width=""14""></td><td colspan=""3"" rowspan=""3"" align=""center""><a href=""" & GetAlbumUrl(objAlbum.AlbumID.ToString()) & """><img src=""" & objSettings("AlbumPath").ToString() & """ class=""photo_198"" alt=""" & GetAlternateTextForAlbum(objAlbum) & """ width=""" & objSettings("AlbumWidth").ToString() & """ height=""" & objSettings("AlbumHeight").ToString() & """ /></a></td><td class=""mtp----x""><img src=""" & Me.FormatBorderPath("album-r2.png") & """ border=""0"" width=""14""></td></tr>" _
                                    & "<tr><td class=""midx----""></td><td class=""mid----x""></td></tr>" _
                                    & "<tr><td class=""mbtx----""><img src=""" & Me.FormatBorderPath("album-l3.png") & """ border=""0"" width=""14""></td><td class=""mbt----x""><img src=""" & Me.FormatBorderPath("album-r3.png") & """ border=""0"" width=""14""></td></tr>" _
                                    & "<tr><td class=""botx----""><img src=""" & Me.FormatBorderPath("album-l4.gif") & """ border=""0"" width=""14"" height=""14""></td><td class=""bot-x---""><img src=""" & Me.FormatBorderPath("album-mbl.png") & """ border=""0"" height=""14""></td><td class=""bot--x--""></td><td class=""bot---x-""><img src=""" & Me.FormatBorderPath("album-mbr.png") & """ border=""0"" height=""14""></td><td class=""bot----x""><img src=""" & Me.FormatBorderPath("album-r4.gif") & """ border=""0"" width=""14"" height=""14""></td></tr>" _
                                    & "</table>"
                            Else
                                objLiteral.Text = "" _
                                    & "<table border=""0"" cellpadding=""0"" cellspacing=""0"" class=""album-frame"">" _
                                    & "<tr><td class=""topx----""><img src=""" & Me.FormatBorderPath("album-l1.gif") & """ border=""0"" width=""14"" height=""14""></td><td class=""top-x---""><img src=""" & Me.FormatBorderPath("album-mtl.gif") & """ border=""0"" height=""14""></td><td class=""top--x--""></td><td class=""top---x-""><img src=""" & Me.FormatBorderPath("album-mtr.gif") & """ border=""0"" height=""14""></td><td class=""top----""><img src=""" & Me.FormatBorderPath("album-r1.gif") & """ border=""0"" width=""14"" height=""14""></td></tr>" _
                                    & "<tr><td class=""mtpx----""><img src=""" & Me.FormatBorderPath("album-l2.gif") & """ border=""0"" width=""14""></td><td colspan=""3"" rowspan=""3"" align=""center""><a href=""" & GetAlbumUrl(objAlbum.AlbumID.ToString()) & """><img src=""" & objSettings("AlbumPath").ToString() & """ class=""photo_198"" alt=""" & GetAlternateTextForAlbum(objAlbum) & """ width=""" & objSettings("AlbumWidth").ToString() & """ height=""" & objSettings("AlbumHeight").ToString() & """ /></a></td><td class=""mtp----x""><img src=""" & Me.FormatBorderPath("album-r2.gif") & """ border=""0"" width=""14""></td></tr>" _
                                    & "<tr><td class=""midx----""></td><td class=""mid----x""></td></tr>" _
                                    & "<tr><td class=""mbtx----""><img src=""" & Me.FormatBorderPath("album-l3.gif") & """ border=""0"" width=""14""></td><td class=""mbt----x""><img src=""" & Me.FormatBorderPath("album-r3.gif") & """ border=""0"" width=""14""></td></tr>" _
                                    & "<tr><td class=""botx----""><img src=""" & Me.FormatBorderPath("album-l4.gif") & """ border=""0"" width=""14"" height=""14""></td><td class=""bot-x---""><img src=""" & Me.FormatBorderPath("album-mbl.gif") & """ border=""0"" height=""14""></td><td class=""bot--x--""></td><td class=""bot---x-""><img src=""" & Me.FormatBorderPath("album-mbr.gif") & """ border=""0"" height=""14""></td><td class=""bot----x""><img src=""" & Me.FormatBorderPath("album-r4.gif") & """ border=""0"" width=""14"" height=""14""></td></tr>" _
                                    & "</table>"
                            End If
                            phPhoto.Controls.Add(objLiteral)
                        Case "DESCRIPTION"
                            Dim objLiteral As New Literal
                            objLiteral.ID = Globals.CreateValidID("Article" & objAlbum.AlbumID.ToString() & "-" & iPtr.ToString())
                            objLiteral.Text = objAlbum.Description.ToString()
                            phPhoto.Controls.Add(objLiteral)
                        Case "EDIT"
                            If (Me.IsEditable) Then
                                If (Me.HasEditPermissions()) Then
                                    Dim objLiteral As New Literal
                                    objLiteral.ID = Globals.CreateValidID("Article" & objAlbum.AlbumID.ToString() & "-" & iPtr.ToString())
                                    objLiteral.Text = "<a href=""" & NavigateURL(_pageID, "EditAlbum", "mid=" & _moduleID.ToString(), "AlbumID=" & objAlbum.AlbumID.ToString(), "ReturnUrl=" & Me.ReturnUrl) & """><img src=""" & Me.ResolveUrl("~/images/edit.gif") & """ alt=""" & Localization.GetString("Edit", Me.LocalResourceFile) & """ border=""0"" /></a>"
                                    phPhoto.Controls.Add(objLiteral)
                                End If
                            End If
                        Case "ISSELECTED"
                            If (Request("AlbumID") <> (_moduleID.ToString() & "-" & objAlbum.AlbumID.ToString())) Then
                                While (iPtr < _templateTokens.Length - 1)
                                    If (_templateTokens(iPtr + 1) = "/ISSELECTED") Then
                                        Exit While
                                    End If
                                    iPtr = iPtr + 1
                                End While
                            End If
                        Case "/ISSELECTED"
                            ' Do Nothing
                        Case "ISNOTSELECTED"
                            If (Request("AlbumID") = (_moduleID.ToString() & "-" & objAlbum.AlbumID.ToString())) Then
                                While (iPtr < _templateTokens.Length - 1)
                                    If (_templateTokens(iPtr + 1) = "/ISNOTSELECTED") Then
                                        Exit While
                                    End If
                                    iPtr = iPtr + 1
                                End While
                            End If
                        Case "/ISNOTSELECTED"
                            ' Do Nothing
                        Case "PHOTOCOUNT"
                            Dim objLiteral As New Literal
                            objLiteral.ID = Globals.CreateValidID("Article" & objAlbum.AlbumID.ToString() & "-" & iPtr.ToString())
                            objLiteral.Text = GetPhotoCount(objAlbum)
                            phPhoto.Controls.Add(objLiteral)
                        Case "RSS"
                            If (LinkedGallerySettings(_moduleID).EnableSyndication) Then
                                Dim rssText As String = Localization.GetString("RssText", Me.LocalResourceFile)
                                If (rssText.IndexOf("{0}") <> -1) Then
                                    rssText = String.Format(rssText, objAlbum.Caption)
                                Else
                                    rssText = String.Format(rssText, objAlbum.Caption)
                                End If
                                Me.AddRSSFeed(RssUrl(objAlbum.AlbumID.ToString()), rssText)
                                Dim objLiteral As New Literal
                                objLiteral.ID = Globals.CreateValidID("Article" & objAlbum.AlbumID.ToString() & "-" & iPtr.ToString())
                                objLiteral.Text = "<a href=""" & RssUrl(objAlbum.AlbumID.ToString()) & """><img src=""" & Me.ResolveUrl("~/DesktopModules/SimpleGallery/images/xml_small.gif") & """ alt=""" & Localization.GetString("Syndicate", Me.LocalResourceFile) & """ align=""absmiddle"" border=""0""></a>"
                                phPhoto.Controls.Add(objLiteral)
                            End If
                        Case "RSSLINK"
                            Dim rssText As String = Localization.GetString("RssText", Me.LocalResourceFile)
                            If (rssText.IndexOf("{0}") <> -1) Then
                                rssText = String.Format(rssText, objAlbum.Caption)
                            Else
                                rssText = String.Format(rssText, objAlbum.Caption)
                            End If
                            Me.AddRSSFeed(RssUrl(objAlbum.AlbumID.ToString()), rssText)
                            Dim objLiteral As New Literal
                            objLiteral.ID = Globals.CreateValidID("Article" & objAlbum.AlbumID.ToString() & "-" & iPtr.ToString())
                            objLiteral.Text = RssUrl(objAlbum.AlbumID.ToString())
                            phPhoto.Controls.Add(objLiteral)
                        Case "TITLE"
                            Dim objLiteral As New Literal
                            objLiteral.ID = Globals.CreateValidID("Article" & objAlbum.AlbumID.ToString() & "-" & iPtr.ToString())
                            objLiteral.Text = objAlbum.Caption.ToString()
                            phPhoto.Controls.Add(objLiteral)
                        Case Else
                            Dim objLiteralOther As New Literal
                            objLiteralOther.Text = "[" & _templateTokens(iPtr + 1) & "]"
                            objLiteralOther.EnableViewState = False
                            phPhoto.Controls.Add(objLiteralOther)
                    End Select

                End If
            Next

        End Sub

        Private Sub dlGallery_OnItemDataBound(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.DataListItemEventArgs) Handles dlGallery.ItemDataBound

            Dim phPhoto As PlaceHolder = CType(e.Item.FindControl("phPhoto"), PlaceHolder)
            If Not (phPhoto Is Nothing) Then

                If (GallerySettings.RandomDisplay = DisplayType.Photo) Then

                    Dim objPhoto As PhotoInfo = CType(e.Item.DataItem, PhotoInfo)
                    BindPhoto(phPhoto, objPhoto)

                Else

                    Dim objAlbum As AlbumInfo = CType(e.Item.DataItem, AlbumInfo)
                    BindAlbum(phPhoto, objAlbum)

                End If

            End If

        End Sub

        Private Sub rptGallery_OnItemDataBound(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.RepeaterItemEventArgs) Handles rptGallery.ItemDataBound

            Dim phPhoto As PlaceHolder = CType(e.Item.FindControl("phPhoto"), PlaceHolder)
            If Not (phPhoto Is Nothing) Then

                If (GallerySettings.RandomDisplay = DisplayType.Photo) Then

                    Dim objPhoto As PhotoInfo = CType(e.Item.DataItem, PhotoInfo)
                    BindPhoto(phPhoto, objPhoto)

                Else

                    Dim objAlbum As AlbumInfo = CType(e.Item.DataItem, AlbumInfo)
                    BindAlbum(phPhoto, objAlbum)

                End If

            End If

        End Sub

#End Region

#Region " Optional Interfaces "

        Public ReadOnly Property ModuleActions() As DotNetNuke.Entities.Modules.Actions.ModuleActionCollection Implements DotNetNuke.Entities.Modules.IActionable.ModuleActions
            Get
                Dim Actions As New DotNetNuke.Entities.Modules.Actions.ModuleActionCollection
                Return Actions
            End Get
        End Property

#End Region

    End Class

End Namespace