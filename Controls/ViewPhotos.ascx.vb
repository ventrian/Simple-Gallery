'
' Property Agent for DotNetNuke -  http://www.dotnetnuke.com
' Copyright (c) 2002-2007
' by Ventrian ( sales@ventrian.com ) ( http://www.ventrian.com )
'

Imports DotNetNuke.Common
Imports DotNetNuke.Common.Utilities
Imports DotNetNuke.Services.Exceptions
Imports DotNetNuke.Services.Localization

Imports Ventrian.SimpleGallery.Common
Imports Ventrian.SimpleGallery.Entities
Imports DotNetNuke.Security

Namespace Ventrian.SimpleGallery.Controls

    Partial Public Class ViewPhotos
        Inherits SimpleGalleryControlBase

#Region " Private Members "

        Private _albumID As Integer = Null.NullInteger
        Private _searchText As String = Null.NullString
        Private _tagID As Integer = Null.NullInteger
        Private _tag As TagInfo
        Private _pageNumber As Integer = 0
        Private _photoTemplate As String = ""
        Private _photoTemplateTokens As String()

#End Region

#Region " Private Properties "

        Private ReadOnly Property AlbumAnchor() As String
            Get
                If (SimpleGalleryBase.GallerySettings.UseAlbumAnchors) Then
                    Return "#SimpleGallery-" & SimpleGalleryBase.ModuleId.ToString()
                End If
                Return ""
            End Get
        End Property

        Private Shadows ReadOnly Property LocalResourceFile() As String
            Get
                Return "~/DesktopModules/SimpleGallery/App_LocalResources/ViewPhotos.ascx.resx"
            End Get
        End Property

        Public ReadOnly Property Tag() As TagInfo
            Get
                If (_tag Is Nothing) Then
                    Dim objTagController As New TagController
                    _tag = objTagController.Get(TagID)
                End If
                Return _tag
            End Get
        End Property

#End Region

#Region " Private Methods "

        Private Sub BindPhotos()

            If (_albumID <> Null.NullInteger Or _tagID <> Null.NullInteger Or _searchText <> Null.NullString) Then

                InitializePhotoTemplate()

                dlGallery.RepeatColumns = SimpleGalleryBase.GallerySettings.PhotosPerRow

                Dim objPhotoController As New PhotoController
                Dim objPhotos As ArrayList = objPhotoController.List(SimpleGalleryBase.ModuleId, AlbumID, True, Null.NullInteger, Null.NullBoolean, TagID, Null.NullString(), SearchText, Me.SimpleGalleryBase.GallerySettings.SortBy, Me.SimpleGalleryBase.GallerySettings.SortDirection)

                Dim objPagedDataSource As New PagedDataSource

                objPagedDataSource.AllowPaging = True
                objPagedDataSource.DataSource = objPhotos
                objPagedDataSource.PageSize = SimpleGalleryBase.GallerySettings.PhotosPerPage

                If (objPagedDataSource.PageCount > 1) Then

                    objPagedDataSource.CurrentPageIndex = Me.PageNumber

                    Dim count As Integer = 0

                    Dim min As Integer = (objPagedDataSource.CurrentPageIndex) * SimpleGalleryBase.GallerySettings.PhotosPerPage + 1
                    Dim max As Integer = (objPagedDataSource.CurrentPageIndex + 1) * SimpleGalleryBase.GallerySettings.PhotosPerPage

                    For Each objPhoto As PhotoInfo In objPhotos
                        count = count + 1

                        Dim tags As String = ""

                        If (SimpleGalleryBase.GallerySettings.EnableTags) Then
                            If (objPhoto.Tags <> "") Then

                                For Each tag As String In objPhoto.Tags.Split(","c)
                                    If (tags.Length = 0) Then
                                        tags = "(" & Localization.GetString("Tags", Me.LocalResourceFile) & ": <a href='" & GetTagUrl(tag, SimpleGalleryBase.TabId, SimpleGalleryBase.ModuleId, SimpleGalleryBase.TabModuleId, Null.NullInteger) & "'>" & tag & "</a>"
                                    Else
                                        tags = tags & ", <a href='" & GetTagUrl(tag, SimpleGalleryBase.TabId, SimpleGalleryBase.ModuleId, SimpleGalleryBase.TabModuleId, Null.NullInteger) & "'>" & tag & "</a>"
                                    End If
                                Next

                                If (tags.Length > 0) Then
                                    tags = tags & ")"
                                End If

                            End If
                        End If

                        If (SimpleGalleryBase.GallerySettings.Slideshow = SlideshowType.Lightbox) Then
                            If (count < min) Then
                                Dim objLiteral As New Literal
                                objLiteral.Text = "<a href=""" & Me.GetPhotoPathFull(objPhoto) & """ rel=""lightbox" & SimpleGalleryBase.TabModuleId & """ title=""" & GetTitle(objPhoto) & """  tags=""" & tags & """ description=""" & GetDescription(objPhoto) & """></a>"
                                phLightboxTop.Controls.Add(objLiteral)
                            End If

                            If (count > max) Then
                                Dim objLiteral As New Literal
                                objLiteral.Text = "<a href=""" & Me.GetPhotoPathFull(objPhoto) & """ rel=""lightbox" & SimpleGalleryBase.TabModuleId & """ title=""" & GetTitle(objPhoto) & """  tags=""" & tags & """ description=""" & GetDescription(objPhoto) & """></a>"
                                phLightboxBottom.Controls.Add(objLiteral)
                            End If
                        End If

                    Next

                    pnlPaging.Visible = True

                    lnkNext.Text = Localization.GetString("NextPager", LocalResourceFile)
                    lnkNext.Enabled = Not objPagedDataSource.IsLastPage
                    If (lnkNext.Enabled) Then
                        If (_albumID <> Null.NullInteger) Then
                            lnkNext.NavigateUrl = NavigateURL(SimpleGalleryBase.TabId, "", "AlbumID=" & SimpleGalleryBase.ModuleId.ToString() & "-" & _albumID, "Page=" & (objPagedDataSource.CurrentPageIndex + 1).ToString()) & AlbumAnchor
                        Else
                            If Not (Tag Is Nothing) Then
                                lnkNext.NavigateUrl = GetTagUrl(Tag.NameLowered, SimpleGalleryBase.TabId, SimpleGalleryBase.ModuleId, SimpleGalleryBase.TabModuleId, (objPagedDataSource.CurrentPageIndex + 1).ToString()) & AlbumAnchor
                            Else
                                If (_searchText <> "") Then
                                    lnkNext.NavigateUrl = NavigateURL(SimpleGalleryBase.TabId, "", "SearchID=" & SimpleGalleryBase.TabModuleId, "SearchText=" & System.Uri.EscapeDataString(_searchText), "Page=" & (objPagedDataSource.CurrentPageIndex + 1).ToString()) & AlbumAnchor
                                End If
                            End If
                        End If
                    End If

                    lnkPrev.Text = Localization.GetString("PrevPager", LocalResourceFile)
                    lnkPrev.Enabled = Not objPagedDataSource.IsFirstPage
                    If (lnkPrev.Enabled) Then
                        If (_albumID <> Null.NullInteger) Then
                            lnkPrev.NavigateUrl = NavigateURL(SimpleGalleryBase.TabId, "", "AlbumID=" & SimpleGalleryBase.ModuleId.ToString() & "-" & _albumID, "Page=" & (objPagedDataSource.CurrentPageIndex - 1).ToString()) & AlbumAnchor
                        Else
                            If Not (Tag Is Nothing) Then
                                lnkPrev.NavigateUrl = GetTagUrl(Tag.NameLowered, SimpleGalleryBase.TabId, SimpleGalleryBase.ModuleId, SimpleGalleryBase.TabModuleId, (objPagedDataSource.CurrentPageIndex - 1).ToString()) & AlbumAnchor
                            Else
                                If (_searchText <> "") Then
                                    lnkPrev.NavigateUrl = NavigateURL(SimpleGalleryBase.TabId, "", "SearchID=" & SimpleGalleryBase.TabModuleId, "SearchText=" & System.Uri.EscapeDataString(_searchText), "Page=" & (objPagedDataSource.CurrentPageIndex - 1).ToString()) & AlbumAnchor
                                End If
                            End If
                        End If
                    End If

                    Dim strPages As String
                    If objPagedDataSource.PageCount = 1 Then
                        strPages = Localization.GetString("Page", LocalResourceFile)
                    Else
                        strPages = Localization.GetString("Pages", LocalResourceFile)
                    End If
                    For i As Integer = 1 To objPagedDataSource.PageCount
                        If i = (objPagedDataSource.CurrentPageIndex + 1) Then
                            strPages = strPages + i.ToString() & " "
                        Else
                            If (_albumID <> Null.NullInteger) Then
                                strPages = strPages & "<a href=" & NavigateURL(SimpleGalleryBase.TabId, "", "AlbumID=" & SimpleGalleryBase.ModuleId.ToString() & "-" & _albumID, "Page=" & (i - 1).ToString()) & AlbumAnchor & ">" & i.ToString() & "</a> "
                            Else
                                If Not (Tag Is Nothing) Then
                                    strPages = strPages & "<a href=" & GetTagUrl(Tag.NameLowered, SimpleGalleryBase.TabId, SimpleGalleryBase.ModuleId, SimpleGalleryBase.TabModuleId, (i - 1)) & AlbumAnchor & """>" & i.ToString() & "</a> "
                                Else
                                    If (_searchText <> "") Then
                                        strPages = strPages & "<a href=" & NavigateURL(SimpleGalleryBase.TabId, "", "SearchID=" & SimpleGalleryBase.TabModuleId, "SearchText=" & System.Uri.EscapeDataString(_searchText), "Page=" & (i - 1).ToString()) & AlbumAnchor & ">" & i.ToString() & "</a> "
                                    End If
                                End If
                            End If
                        End If
                    Next

                    lblCurrentPage.Text = strPages
                Else
                    pnlPaging.Visible = False
                End If

                If (pnlPaging.Visible) Then
                    pnlPaging.Visible = Not SimpleGalleryBase.GallerySettings.HidePager
                End If

                dlGallery.DataSource = objPagedDataSource
                dlGallery.DataBind()

            End If

        End Sub

        Private Function GetTagUrl(ByVal tag As String, ByVal tabID As Integer, ByVal moduleID As Integer, ByVal tabModuleID As Integer, ByVal pageNumber As Integer) As String

            If (AllLetters(tag) = False) Then
                Dim objTagController As New TagController()
                Dim objTag As TagInfo = objTagController.Get(moduleID, tag.ToLower())

                If Not (objTag Is Nothing) Then
                    If (pageNumber = Null.NullInteger) Then
                        Return NavigateURL(tabID, "", "TagID=" & objTag.TagID.ToString(), "Tags=" & tabModuleID.ToString())
                    Else
                        Return NavigateURL(tabID, "", "TagID=" & objTag.TagID.ToString(), "Tags=" & tabModuleID.ToString(), "Page=" & pageNumber.ToString())
                    End If
                End If
            Else
                If (pageNumber = Null.NullInteger) Then
                    Return NavigateURL(tabID, "", "Tag=" & tag, "Tags=" & tabModuleID.ToString())
                Else
                    Return NavigateURL(tabID, "", "Tag=" & tag, "Tags=" & tabModuleID.ToString(), "Page=" & pageNumber.ToString())
                End If
            End If

            Return ""

        End Function

        Private Function AllLetters(ByVal txt As String) As Boolean

            Dim reg As New Regex("^[A-Za-z]+$")
            Dim ok As Boolean = True

            If Not (reg.IsMatch(txt)) Then
                ok = False
            Else
                ok = True
            End If

            Return ok

        End Function

        Protected Function GetAlbumUrl(ByVal albumID As String) As String

            Return NavigateURL(SimpleGalleryBase.TabId, "", "AlbumID=" & SimpleGalleryBase.ModuleId.ToString() & "-" & albumID)

        End Function

        Protected Function GetAlternateText(ByVal dataItem As Object) As String

            If (SimpleGalleryBase.GallerySettings.EnableTooltip) Then
                Dim objPhoto As PhotoInfo = CType(dataItem, PhotoInfo)

                If (objPhoto.Description.Trim().Length > 0) Then
                    Return Server.HtmlEncode(objPhoto.Description).Replace(Chr(34), "")
                Else
                    Return Server.HtmlEncode(objPhoto.Name).Replace(Chr(34), "")
                End If
            Else
                Return ""
            End If

        End Function

        Protected Function GetTitle(ByVal dataItem As Object) As String

            Dim objPhoto As PhotoInfo = CType(dataItem, PhotoInfo)
            Return Server.HtmlEncode(objPhoto.Name).Replace(Chr(34), "")

        End Function

        Protected Function GetDescription(ByVal dataItem As Object) As String

            Dim objPhoto As PhotoInfo = CType(dataItem, PhotoInfo)
            Return Server.HtmlEncode(objPhoto.Description).Replace(Chr(34), "")

        End Function

        Protected Function GetPhotoHeight(ByVal dataItem As Object) As String

            If (SimpleGalleryBase.GallerySettings.ThumbnailPhoto = ThumbnailType.Square) Then
                Return SimpleGalleryBase.GallerySettings.ThumbnailSquare
            End If

            Dim objPhoto As PhotoInfo = CType(dataItem, PhotoInfo)

            If Not (objPhoto Is Nothing) Then
                Dim width As Integer
                If (objPhoto.Width > SimpleGalleryBase.GallerySettings.ThumbnailWidth) Then
                    width = SimpleGalleryBase.GallerySettings.ThumbnailWidth
                Else
                    width = objPhoto.Width
                End If

                Dim height As Integer = Convert.ToInt32(objPhoto.Height / (objPhoto.Width / width))
                If (height > SimpleGalleryBase.GallerySettings.ThumbnailHeight) Then
                    height = SimpleGalleryBase.GallerySettings.ThumbnailHeight
                    width = Convert.ToInt32(objPhoto.Width / (objPhoto.Height / height))
                End If

                Return height.ToString()
            Else
                Return SimpleGalleryBase.GallerySettings.ThumbnailWidth.ToString()
            End If

        End Function

        Protected Function GetPhotoPath(ByVal dataItem As Object) As String

            Dim objPhoto As PhotoInfo = CType(dataItem, PhotoInfo)

            If Not (objPhoto Is Nothing) Then
                If (SimpleGalleryBase.GallerySettings.CompressionPhoto = CompressionType.MinSize) Then
                    If (SimpleGalleryBase.GallerySettings.ThumbnailPhoto = ThumbnailType.Proportion) Then
                        Return Me.ResolveUrl("../ImageHandler.ashx?width=" & GetPhotoWidth(CType(objPhoto, Object)) & "&height=" & GetPhotoHeight(CType(objPhoto, Object)) & "&HomeDirectory=" & System.Uri.EscapeDataString(SimpleGalleryBase.PortalSettings.HomeDirectory & objPhoto.HomeDirectory) & "&fileName=" & System.Uri.EscapeDataString(objPhoto.FileName) & "&portalid=" & SimpleGalleryBase.PortalId.ToString() & "&i=" & objPhoto.PhotoID)
                    Else
                        Return Me.ResolveUrl("../ImageHandler.ashx?width=" & GetPhotoWidth(CType(objPhoto, Object)) & "&height=" & GetPhotoHeight(CType(objPhoto, Object)) & "&HomeDirectory=" & System.Uri.EscapeDataString(SimpleGalleryBase.PortalSettings.HomeDirectory & objPhoto.HomeDirectory) & "&fileName=" & System.Uri.EscapeDataString(objPhoto.FileName) & "&portalid=" & SimpleGalleryBase.PortalId.ToString() & "&i=" & objPhoto.PhotoID & "&s=1")
                    End If
                Else
                    If (SimpleGalleryBase.GallerySettings.ThumbnailPhoto = ThumbnailType.Proportion) Then
                        Return Me.ResolveUrl("../ImageHandler.ashx?width=" & GetPhotoWidth(CType(objPhoto, Object)) & "&height=" & GetPhotoHeight(CType(objPhoto, Object)) & "&HomeDirectory=" & System.Uri.EscapeDataString(SimpleGalleryBase.PortalSettings.HomeDirectory & objPhoto.HomeDirectory) & "&fileName=" & System.Uri.EscapeDataString(objPhoto.FileName) & "&portalid=" & SimpleGalleryBase.PortalId.ToString() & "&i=" & objPhoto.PhotoID & "&q=1")
                    Else
                        Return Me.ResolveUrl("../ImageHandler.ashx?width=" & GetPhotoWidth(CType(objPhoto, Object)) & "&height=" & GetPhotoHeight(CType(objPhoto, Object)) & "&HomeDirectory=" & System.Uri.EscapeDataString(SimpleGalleryBase.PortalSettings.HomeDirectory & objPhoto.HomeDirectory) & "&fileName=" & System.Uri.EscapeDataString(objPhoto.FileName) & "&portalid=" & SimpleGalleryBase.PortalId.ToString() & "&i=" & objPhoto.PhotoID & "&q=1" & "&s=1")
                    End If
                End If
            End If

            Return ""

        End Function

        Protected Function GetPhotoPathFull(ByVal objPhoto As PhotoInfo) As String

            If (objPhoto.Width > SimpleGalleryBase.GallerySettings.ImageWidth Or objPhoto.Height > SimpleGalleryBase.GallerySettings.ImageHeight) Then
                ' Use Handler to Resize 
                Dim width As Integer = objPhoto.Width
                Dim height As Integer = objPhoto.Height

                If (width > SimpleGalleryBase.GallerySettings.ImageWidth) Then
                    width = SimpleGalleryBase.GallerySettings.ImageWidth
                    height = Convert.ToInt32(height / (objPhoto.Width / SimpleGalleryBase.GallerySettings.ImageWidth))
                End If

                If (height > SimpleGalleryBase.GallerySettings.ImageHeight) Then
                    height = SimpleGalleryBase.GallerySettings.ImageHeight
                    width = Convert.ToInt32(objPhoto.Width / (objPhoto.Height / SimpleGalleryBase.GallerySettings.ImageHeight))
                End If

                Return Me.ResolveUrl("../ImageHandler.ashx?width=" & width.ToString() & "&height=" & height.ToString() & "&HomeDirectory=" & System.Uri.EscapeDataString(SimpleGalleryBase.PortalSettings.HomeDirectory & objPhoto.HomeDirectory) & "&fileName=" & System.Uri.EscapeDataString(objPhoto.FileName) & "&portalid=" & SimpleGalleryBase.PortalId.ToString() & "&i=" & objPhoto.PhotoID & "&q=1")
            Else
                Return SimpleGalleryBase.PortalSettings.HomeDirectory & objPhoto.HomeDirectory & "/" & System.Uri.EscapeDataString(objPhoto.FileName)
            End If

        End Function

        Protected Function GetPhotoUrl(ByVal photoID As String) As String

            If (_tagID <> Null.NullInteger) Then
                If (SimpleGalleryBase.GallerySettings.Slideshow = SlideshowType.Popup) Then
                    If (SimpleGalleryBase.GallerySettings.Compression = CompressionType.MinSize) Then
                        Return "javascript:openScreenWin('smallscreen', '" & Me.ResolveUrl("../SlideShowPopup.aspx?PortalID=" & SimpleGalleryBase.PortalId.ToString() & "&TagID=" & _tagID.ToString() & "&ItemID=" & photoID & "&Border=" & SimpleGalleryBase.GallerySettings.BorderStyle & "&sb=" & SimpleGalleryBase.GallerySettings.SortBy & "&sd=" & SimpleGalleryBase.GallerySettings.SortDirection & "&tt=" & SimpleGalleryBase.GallerySettings.EnableTooltip) & "', " & SimpleGalleryBase.GallerySettings.PopupWidth.ToString() & ", " & SimpleGalleryBase.GallerySettings.PopupHeight.ToString() & ");"
                    Else
                        Return "javascript:openScreenWin('smallscreen', '" & Me.ResolveUrl("../SlideShowPopup.aspx?PortalID=" & SimpleGalleryBase.PortalId.ToString() & "&TagID=" & _tagID.ToString() & "&ItemID=" & photoID & "&Border=" & SimpleGalleryBase.GallerySettings.BorderStyle & "&sb=" & SimpleGalleryBase.GallerySettings.SortBy & "&sd=" & SimpleGalleryBase.GallerySettings.SortDirection & "&tt=" & SimpleGalleryBase.GallerySettings.EnableTooltip) & "', " & SimpleGalleryBase.GallerySettings.PopupWidth.ToString() & ", " & SimpleGalleryBase.GallerySettings.PopupHeight.ToString() & ");"
                    End If
                Else
                    Return NavigateURL(SimpleGalleryBase.TabId, "", "galleryType=SlideShow", "ItemID=" & photoID, "TagID=" & _tagID.ToString())
                End If
            Else
                If (SimpleGalleryBase.GallerySettings.Slideshow = SlideshowType.Popup) Then
                    If (_searchText <> "") Then
                        If (SimpleGalleryBase.GallerySettings.Compression = CompressionType.MinSize) Then
                            Return "javascript:openScreenWin('smallscreen', '" & Me.ResolveUrl("../SlideShowPopup.aspx?PortalID=" & SimpleGalleryBase.PortalId.ToString() & "&ItemID=" & photoID & "&SearchText=" & System.Uri.EscapeDataString(_searchText) & "&Border=" & SimpleGalleryBase.GallerySettings.BorderStyle & "&sb=" & SimpleGalleryBase.GallerySettings.SortBy & "&sd=" & SimpleGalleryBase.GallerySettings.SortDirection & "&tt=" & SimpleGalleryBase.GallerySettings.EnableTooltip) & "', " & SimpleGalleryBase.GallerySettings.PopupWidth.ToString() & ", " & SimpleGalleryBase.GallerySettings.PopupHeight.ToString() & ");"
                        Else
                            Return "javascript:openScreenWin('smallscreen', '" & Me.ResolveUrl("../SlideShowPopup.aspx?PortalID=" & SimpleGalleryBase.PortalId.ToString() & "&ItemID=" & photoID & "&SearchText=" & System.Uri.EscapeDataString(_searchText) & "&Border=" & SimpleGalleryBase.GallerySettings.BorderStyle & "&sb=" & SimpleGalleryBase.GallerySettings.SortBy & "&sd=" & SimpleGalleryBase.GallerySettings.SortDirection & "&tt=" & SimpleGalleryBase.GallerySettings.EnableTooltip) & "', " & SimpleGalleryBase.GallerySettings.PopupWidth.ToString() & ", " & SimpleGalleryBase.GallerySettings.PopupHeight.ToString() & ");"
                        End If
                    Else
                        If (SimpleGalleryBase.GallerySettings.Compression = CompressionType.MinSize) Then
                            Return "javascript:openScreenWin('smallscreen', '" & Me.ResolveUrl("../SlideShowPopup.aspx?PortalID=" & SimpleGalleryBase.PortalId.ToString() & "&ItemID=" & photoID & "&Border=" & SimpleGalleryBase.GallerySettings.BorderStyle & "&sb=" & SimpleGalleryBase.GallerySettings.SortBy & "&sd=" & SimpleGalleryBase.GallerySettings.SortDirection & "&tt=" & SimpleGalleryBase.GallerySettings.EnableTooltip) & "', " & SimpleGalleryBase.GallerySettings.PopupWidth.ToString() & ", " & SimpleGalleryBase.GallerySettings.PopupHeight.ToString() & ");"
                        Else
                            Return "javascript:openScreenWin('smallscreen', '" & Me.ResolveUrl("../SlideShowPopup.aspx?PortalID=" & SimpleGalleryBase.PortalId.ToString() & "&ItemID=" & photoID & "&Border=" & SimpleGalleryBase.GallerySettings.BorderStyle & "&sb=" & SimpleGalleryBase.GallerySettings.SortBy & "&sd=" & SimpleGalleryBase.GallerySettings.SortDirection & "&tt=" & SimpleGalleryBase.GallerySettings.EnableTooltip) & "', " & SimpleGalleryBase.GallerySettings.PopupWidth.ToString() & ", " & SimpleGalleryBase.GallerySettings.PopupHeight.ToString() & ");"
                        End If
                    End If
                Else
                    If (_searchText <> "") Then
                        Return NavigateURL(SimpleGalleryBase.TabId, "", "galleryType=SlideShow", "ItemID=" & photoID, "SearchText=" & System.Uri.EscapeDataString(_searchText))
                    Else
                        Return NavigateURL(SimpleGalleryBase.TabId, "", "galleryType=SlideShow", "ItemID=" & photoID)
                    End If
                End If
            End If

        End Function

        Protected Function GetPhotoWidth(ByVal dataItem As Object) As String

            If (SimpleGalleryBase.GallerySettings.ThumbnailPhoto = ThumbnailType.Square) Then
                Return SimpleGalleryBase.GallerySettings.ThumbnailSquare
            End If

            Dim objPhoto As PhotoInfo = CType(dataItem, PhotoInfo)

            If Not (objPhoto Is Nothing) Then
                Dim width As Integer
                If (objPhoto.Width > SimpleGalleryBase.GallerySettings.ThumbnailWidth) Then
                    width = SimpleGalleryBase.GallerySettings.ThumbnailWidth
                Else
                    width = objPhoto.Width
                End If

                Dim height As Integer = Convert.ToInt32(objPhoto.Height / (objPhoto.Width / width))
                If (height > SimpleGalleryBase.GallerySettings.ThumbnailHeight) Then
                    height = SimpleGalleryBase.GallerySettings.ThumbnailHeight
                    width = Convert.ToInt32(objPhoto.Width / (objPhoto.Height / height))
                End If

                Return width.ToString()
            Else
                Return SimpleGalleryBase.GallerySettings.ThumbnailWidth.ToString()
            End If

        End Function

        Private Sub InitializePhotoTemplate()

            Dim cacheKey As String = SimpleGalleryBase.TabModuleId.ToString() & TemplateType.PhotoInfo.ToString()

            Dim objTemplate As TemplateInfo = CType(DataCache.GetCache(cacheKey), TemplateInfo)

            If (objTemplate Is Nothing) Then
                Dim objTemplateController As New TemplateController
                objTemplate = objTemplateController.Get(SimpleGalleryBase.ModuleId, TemplateType.PhotoInfo.ToString())

                If (objTemplate Is Nothing) Then
                    objTemplate = New TemplateInfo
                    objTemplate.Template = Constants.DEFAULT_TEMPLATE_PHOTO_INFO
                End If

                DataCache.SetCache(cacheKey, objTemplate)
            End If

            _photoTemplate = objTemplate.Template
            _photoTemplateTokens = objTemplate.Tokens

        End Sub

        Protected Function IsPhotoEditable() As Boolean

            Return (SimpleGalleryBase.HasEditPhotoPermissions Or SimpleGalleryBase.HasDeletePhotoPermissions)

        End Function

#End Region

#Region " Protected Methods "

        Protected Function GetLocalizedValue(ByVal key As String) As String
            Return Localization.GetString(key, Me.LocalResourceFile)
        End Function

        Protected Function GetHideDownload() As String

            Return SimpleGalleryBase.GallerySettings.LightboxHideDownload.ToString().ToLower()

        End Function

        Protected Function GetHidePaging() As String

            Return SimpleGalleryBase.GallerySettings.LightboxHidePaging.ToString().ToLower()

        End Function

        Protected Function GetHideTags() As String

            Return SimpleGalleryBase.GallerySettings.LightboxHideTags.ToString().ToLower()

        End Function

        Protected Function GetHideDescription() As String

            Return SimpleGalleryBase.GallerySettings.LightboxHideDescription.ToString().ToLower()

        End Function

        Protected Function GetHideTitle() As String

            Return SimpleGalleryBase.GallerySettings.LightboxHideTitle.ToString().ToLower()

        End Function

        Protected Function GetScrollbarSetting() As String

            If (SimpleGalleryBase.GallerySettings.EnableScrollbar) Then
                Return "yes"
            Else
                Return "no"
            End If

        End Function

        Protected Function GetShortcutKey(ByVal action As String) As String

            Select Case action.ToLower()
                Case "c"
                    Return SimpleGalleryBase.GallerySettings.LightboxCloseKey

                Case "p"
                    Return SimpleGalleryBase.GallerySettings.LightboxPreviousKey

                Case "n"
                    Return SimpleGalleryBase.GallerySettings.LightboxNextKey

                Case "d"
                    Return SimpleGalleryBase.GallerySettings.LightboxDownloadKey
            End Select

            Return ""

        End Function

        Protected Function GetSlideInterval() As String

            Return (SimpleGalleryBase.GallerySettings.LightboxSlideInterval * 1000).ToString()

        End Function

#End Region

#Region " Public Properties "

        Public Property AlbumID() As Integer
            Get
                Return _albumID
            End Get
            Set(ByVal Value As Integer)
                _albumID = Value
            End Set
        End Property

        Public ReadOnly Property BasePage() As DotNetNuke.Framework.CDefault
            Get
                Return CType(Me.Page, DotNetNuke.Framework.CDefault)
            End Get
        End Property

        Public Property PageNumber() As Integer
            Get
                Return _pageNumber
            End Get
            Set(ByVal Value As Integer)
                _pageNumber = Value
            End Set
        End Property

        Public Property SearchText() As String
            Get
                Return _searchText
            End Get
            Set(ByVal Value As String)
                _searchText = Value
            End Set
        End Property

        Public Property TagID() As Integer
            Get
                Return _tagID
            End Get
            Set(ByVal Value As Integer)
                _tagID = Value
            End Set
        End Property

#End Region

#Region " Event Handlers "

        Private Sub Page_PreRender(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.PreRender

            Try

                If (SimpleGalleryBase.GallerySettings.Slideshow = SlideshowType.Lightbox) Then
                    If (HttpContext.Current.Items("SimpleGallery-ScriptsRegistered") Is Nothing) Then

                        If (HttpContext.Current.Items("jquery_registered") Is Nothing And HttpContext.Current.Items("jQueryRequested") Is Nothing And SimpleGalleryBase.GallerySettings.IncludeJQuery) Then

                            Dim version As Integer = Convert.ToInt32(SimpleGalleryBase.PortalSettings.Version.Split("."c)(0))
                            If (version < 6) Then
                                Dim jLink As New Literal
                                jLink.Text = "" & vbCrLf _
                                    & "<script type=""text/javascript"" src='" & Me.ResolveUrl("../js/lightbox/jquery.js?v=" & SimpleGalleryBase.GallerySettings.JavascriptVersion) & "'></script>" & vbCrLf
                                Page.Header.Controls.Add(jLink)
                            End If

                            Dim litLink As New Literal
                            litLink.Text = "" & vbCrLf _
                                & "<script type=""text/javascript"" src='" & Page.ResolveUrl(SimpleGalleryBase.GallerySettings.LightboxDefaultPath & "?v=" & SimpleGalleryBase.GallerySettings.JavascriptVersion) & "'></script>" & vbCrLf
                            'Page.Header.Controls.Add(litLink)
                            phjQueryScripts.Controls.Add(litLink)
                            'Page.ClientScript.RegisterStartupScript(Me.GetType(), "VentrianGallery", litLink.Text.ToString())

                        Else

                            Dim litLink As New Literal
                            litLink.Text = "" & vbCrLf _
                                & "<script type=""text/javascript"" src='" & Page.ResolveUrl(SimpleGalleryBase.GallerySettings.LightboxDefaultPath & "?v=" & SimpleGalleryBase.GallerySettings.JavascriptVersion) & "'></script>" & vbCrLf
                            'Page.Header.Controls.Add(litLink)
                            phjQueryScripts.Controls.Add(litLink)
                            'Page.ClientScript.RegisterStartupScript(Me.GetType(), "VentrianGallery", litLink.Text.ToString())

                        End If

                        HttpContext.Current.Items.Add("SimpleGallery-ScriptsRegistered", "true")

                    End If

                End If

                phLightboxScripts.Visible = (SimpleGalleryBase.GallerySettings.Slideshow = SlideshowType.Lightbox)

                If (HttpContext.Current.Items("SimpleGallery-ScriptsRegisteredPopup") Is Nothing) Then
                    phPopupScripts.Visible = (SimpleGalleryBase.GallerySettings.Slideshow = SlideshowType.Popup)
                    HttpContext.Current.Items.Add("SimpleGallery-ScriptsRegisteredPopup", "true")
                End If

                BindPhotos()

            Catch exc As Exception    'Module failed to load
                ProcessModuleLoadException(Me, exc)
            End Try

        End Sub

        Private Sub dlGallery_OnItemDataBound(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.DataListItemEventArgs) Handles dlGallery.ItemDataBound

            If (e.Item.ItemType = ListItemType.Item Or e.Item.ItemType = ListItemType.AlternatingItem) Then

                Dim phPhoto As PlaceHolder = CType(e.Item.FindControl("phPhoto"), PlaceHolder)
                Dim objPhoto As PhotoInfo = CType(e.Item.DataItem, PhotoInfo)

                If Not (phPhoto Is Nothing) Then

                    Dim tags As String = ""

                    If (SimpleGalleryBase.GallerySettings.EnableTags) Then
                        If (objPhoto.Tags <> "") Then

                            For Each tag As String In objPhoto.Tags.Split(","c)
                                If (tags.Length = 0) Then
                                    tags = "(" & Localization.GetString("Tags", Me.LocalResourceFile) & ": <a href='" & GetTagUrl(tag, SimpleGalleryBase.TabId, SimpleGalleryBase.ModuleId, SimpleGalleryBase.TabModuleId, Null.NullInteger) & "'>" & tag & "</a>"
                                Else
                                    tags = tags & ", <a href='" & GetTagUrl(tag, SimpleGalleryBase.TabId, SimpleGalleryBase.ModuleId, SimpleGalleryBase.TabModuleId, Null.NullInteger) & "'>" & tag & "</a>"
                                End If
                            Next

                            If (tags.Length > 0) Then
                                tags = tags & ")"
                            End If

                        End If
                    End If

                    For iPtr As Integer = 0 To _photoTemplateTokens.Length - 1 Step 2

                        phPhoto.Controls.Add(New LiteralControl(_photoTemplateTokens(iPtr).ToString()))

                        If iPtr < _photoTemplateTokens.Length - 1 Then
                            Select Case _photoTemplateTokens(iPtr + 1)
                                Case "ADDTOCARTLINK"
                                    Dim objLiteral As New Literal
                                    objLiteral.ID = Globals.CreateValidID("Gallery" & objPhoto.PhotoID.ToString() & "-" & iPtr.ToString())
                                    objLiteral.Text = ResolveUrl("~/DesktopModules/SimpleGallery/AddToCart.ashx?mid=" & objPhoto.ModuleID.ToString() & "&ItemName=" & Server.UrlEncode(objPhoto.Name) & "&ItemID=" & objPhoto.PhotoID.ToString())
                                    phPhoto.Controls.Add(objLiteral)
                                Case "ALBUMLINK"
                                    Dim objLiteral As New Literal
                                    objLiteral.ID = Globals.CreateValidID("Gallery" & objPhoto.PhotoID.ToString() & "-" & iPtr.ToString())
                                    objLiteral.Text = Me.GetAlbumUrl(objPhoto.AlbumID.ToString())
                                    phPhoto.Controls.Add(objLiteral)
                                Case "ALBUMID"
                                    Dim objLiteral As New Literal
                                    objLiteral.ID = Globals.CreateValidID("Gallery" & objPhoto.PhotoID.ToString() & "-" & iPtr.ToString())
                                    objLiteral.Text = objPhoto.AlbumID.ToString()
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
                                    objLiteral.ID = Globals.CreateValidID("Article" & objPhoto.PhotoID.ToString() & "-" & iPtr.ToString())
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
                                    objLiteral.Text = objPhoto.DateApproved.ToShortDateString().ToString()
                                    phPhoto.Controls.Add(objLiteral)
                                Case "DATECREATED"
                                    Dim objLiteral As New Literal
                                    objLiteral.ID = Globals.CreateValidID("Gallery" & objPhoto.PhotoID.ToString() & "-" & iPtr.ToString())
                                    objLiteral.Text = objPhoto.DateCreated.ToShortDateString().ToString()
                                    phPhoto.Controls.Add(objLiteral)
                                Case "DATEUPDATED"
                                    Dim objLiteral As New Literal
                                    objLiteral.ID = Globals.CreateValidID("Gallery" & objPhoto.PhotoID.ToString() & "-" & iPtr.ToString())
                                    objLiteral.Text = objPhoto.DateUpdated.ToShortDateString().ToString()
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
                                Case "EDIT"
                                    If (Me.IsPhotoEditable) Then
                                        Dim objLiteral As New Literal
                                        objLiteral.ID = Globals.CreateValidID("Gallery" & objPhoto.PhotoID.ToString() & "-" & iPtr.ToString())
                                        objLiteral.Text = "<a href=""" & SimpleGalleryBase.EditUrl("ItemID", objPhoto.PhotoID.ToString(), "Edit", "ReturnUrl=" & SimpleGalleryBase.ReturnUrl) & """><img src=""" & Me.ResolveUrl("~/images/edit.gif") & """ alt=""" & Localization.GetString("Edit", Me.LocalResourceFile) & """ border=""0"" /></a>"
                                        phPhoto.Controls.Add(objLiteral)
                                    Else
                                        If (Request.IsAuthenticated) Then
                                            Dim objAlbumController As New AlbumController()
                                            Dim objAlbum As AlbumInfo = objAlbumController.Get(objPhoto.AlbumID)

                                            If (objAlbum IsNot Nothing) Then
                                                If (objAlbum.InheritSecurity = False) Then
                                                    If (SimpleGalleryBase.Settings.Contains(objAlbum.AlbumID & "-" & Constants.SETTING_PERMISSION_EDIT_ALBUM)) Then
                                                        If (PortalSecurity.IsInRoles(SimpleGalleryBase.Settings(objAlbum.AlbumID & "-" & Constants.SETTING_PERMISSION_EDIT_ALBUM).ToString())) Then
                                                            Dim objLiteral As New Literal
                                                            objLiteral.ID = Globals.CreateValidID("Gallery" & objPhoto.PhotoID.ToString() & "-" & iPtr.ToString())
                                                            objLiteral.Text = "<a href=""" & SimpleGalleryBase.EditUrl("ItemID", objPhoto.PhotoID.ToString(), "Edit", "ReturnUrl=" & SimpleGalleryBase.ReturnUrl) & """><img src=""" & Me.ResolveUrl("~/images/edit.gif") & """ alt=""" & Localization.GetString("Edit", Me.LocalResourceFile) & """ border=""0"" /></a>"
                                                            phPhoto.Controls.Add(objLiteral)
                                                        End If
                                                    End If
                                                End If
                                            End If
                                        End If
                                    End If
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
                                    If (SimpleGalleryBase.GallerySettings.Slideshow <> SlideshowType.Lightbox) Then
                                        objLiteral.Text = "<img src=""" & GetPhotoPath(objPhoto) & """ class=""photo_198"" width=""" & GetPhotoWidth(objPhoto) & """ height=""" & GetPhotoHeight(objPhoto) & """ alt=""" & GetAlternateText(objPhoto) & """>"
                                    Else
                                        objLiteral.Text = "<a href=""" & Me.GetPhotoPathFull(objPhoto) & """ rel=""lightbox" & Me.SimpleGalleryBase.TabModuleId.ToString() & """ title=""" & GetTitle(objPhoto) & """ tags=""" & tags & """ description=""" & GetDescription(objPhoto) & """><img src=""" & GetPhotoPath(objPhoto) & """ class=""photo_198"" width=""" & GetPhotoWidth(objPhoto) & """ height=""" & GetPhotoHeight(objPhoto) & """ alt=""" & GetAlternateText(objPhoto) & """></a>"
                                    End If
                                    phPhoto.Controls.Add(objLiteral)
                                Case "PHOTOLINK"
                                    Dim objLiteral As New Literal
                                    objLiteral.ID = Globals.CreateValidID("Gallery" & objPhoto.PhotoID.ToString() & "-" & iPtr.ToString())
                                    objLiteral.Text = Me.GetPhotoUrl(objPhoto.PhotoID.ToString())
                                    phPhoto.Controls.Add(objLiteral)
                                Case "PHOTOPATH"
                                    Dim objLiteral As New Literal
                                    objLiteral.ID = Globals.CreateValidID("Gallery" & objPhoto.PhotoID.ToString() & "-" & iPtr.ToString())
                                    objLiteral.Text = GetPhotoPathFull(objPhoto)
                                    phPhoto.Controls.Add(objLiteral)
                                Case "PHOTOWITHBORDER"
                                    Dim objLiteral As New Literal
                                    objLiteral.ID = Globals.CreateValidID("Gallery" & objPhoto.PhotoID.ToString() & "-" & iPtr.ToString())
                                    If (SimpleGalleryBase.GallerySettings.Slideshow <> SlideshowType.Lightbox) Then
                                        objLiteral.Text = "<table class=""photo-frame"">" _
                                        & "<tr><td class=""topx--""></td><td class=""top-x-""></td><td class=""top--x""></td></tr>" _
                                        & "<tr><td class=""midx--""></td><td valign=""top""><a href=""" & Me.GetPhotoUrl(objPhoto.PhotoID.ToString()) & """><img src=""" & GetPhotoPath(objPhoto) & """ class=""photo_198"" width=""" & GetPhotoWidth(objPhoto) & """ height=""" & GetPhotoHeight(objPhoto) & """ alt=""" & GetAlternateText(objPhoto) & """></a></td><td class=""mid--x""></td></tr>" _
                                        & "<tr><td class=""botx--""></td><td class=""bot-x-""></td><td class=""bot--x""></td></tr>" _
                                        & "</table>"
                                    Else
                                        objLiteral.Text = "<table class=""photo-frame"">" _
                                        & "<tr><td class=""topx--""></td><td class=""top-x-""></td><td class=""top--x""></td></tr>" _
                                        & "<tr><td class=""midx--""></td><td valign=""top""><a href=""" & Me.GetPhotoPathFull(objPhoto) & """ rel=""lightbox" & Me.SimpleGalleryBase.TabModuleId.ToString() & """ title=""" & GetTitle(objPhoto) & """ tags=""" & tags & """ description=""" & GetDescription(objPhoto) & """ pid=""" & objPhoto.PhotoID.ToString() & """><img src=""" & GetPhotoPath(objPhoto) & """ class=""photo_198"" width=""" & GetPhotoWidth(objPhoto) & """ height=""" & GetPhotoHeight(objPhoto) & """ alt=""" & GetAlternateText(objPhoto) & """></a></td><td class=""mid--x""></td></tr>" _
                                        & "<tr><td class=""botx--""></td><td class=""bot-x-""></td><td class=""bot--x""></td></tr>" _
                                        & "</table>"
                                    End If
                                    phPhoto.Controls.Add(objLiteral)
                                Case "PHOTOWITHBORDERNOLINK"
                                    Dim objLiteral As New Literal
                                    objLiteral.ID = Globals.CreateValidID("Gallery" & objPhoto.PhotoID.ToString() & "-" & iPtr.ToString())
                                    objLiteral.Text = "<table class=""photo-frame"">" _
                                    & "<tr><td class=""topx--""></td><td class=""top-x-""></td><td class=""top--x""></td></tr>" _
                                    & "<tr><td class=""midx--""></td><td valign=""top""><img src=""" & GetPhotoPath(objPhoto) & """ class=""photo_198"" width=""" & GetPhotoWidth(objPhoto) & """ height=""" & GetPhotoHeight(objPhoto) & """ alt=""" & GetAlternateText(objPhoto) & """ tags=""" & tags & """ pid=""" & objPhoto.PhotoID.ToString() & """></td><td class=""mid--x""></td></tr>" _
                                    & "<tr><td class=""botx--""></td><td class=""bot-x-""></td><td class=""bot--x""></td></tr>" _
                                    & "</table>"
                                    phPhoto.Controls.Add(objLiteral)
                                Case "PHOTOWITHOUTBORDER"
                                    Dim objLiteral As New Literal
                                    objLiteral.ID = Globals.CreateValidID("Gallery" & objPhoto.PhotoID.ToString() & "-" & iPtr.ToString())
                                    If (SimpleGalleryBase.GallerySettings.Slideshow <> SlideshowType.Lightbox) Then
                                        objLiteral.Text = "<a href=""" & Me.GetPhotoUrl(objPhoto.PhotoID.ToString()) & """><img src=""" & GetPhotoPath(objPhoto) & """ class=""photo_198"" width=""" & GetPhotoWidth(objPhoto) & """ height=""" & GetPhotoHeight(objPhoto) & """ alt=""" & GetAlternateText(objPhoto) & """></a>"
                                    Else
                                        objLiteral.Text = "<a href=""" & Me.GetPhotoPathFull(objPhoto) & """ rel=""lightbox" & Me.SimpleGalleryBase.TabModuleId.ToString() & """ title=""" & GetTitle(objPhoto) & """ tags=""" & tags & """ description=""" & GetDescription(objPhoto) & """ pid=""" & objPhoto.PhotoID.ToString() & """><img src=""" & GetPhotoPath(objPhoto) & """ class=""photo_198"" width=""" & GetPhotoWidth(objPhoto) & """ height=""" & GetPhotoHeight(objPhoto) & """ alt=""" & GetAlternateText(objPhoto) & """></a>"
                                    End If
                                    phPhoto.Controls.Add(objLiteral)
                                Case "TITLE"
                                    Dim objLiteral As New Literal
                                    objLiteral.ID = Globals.CreateValidID("Gallery" & objPhoto.PhotoID.ToString() & "-" & iPtr.ToString())
                                    objLiteral.Text = objPhoto.Name.ToString()
                                    phPhoto.Controls.Add(objLiteral)
                                Case "THUMBNAILLINK"
                                    Dim objLiteral As New Literal
                                    objLiteral.ID = Globals.CreateValidID("Gallery" & objPhoto.PhotoID.ToString() & "-" & iPtr.ToString())
                                    objLiteral.Text = Me.GetPhotoPath(objPhoto)
                                    phPhoto.Controls.Add(objLiteral)
                                Case "WIDTH"
                                    Dim objLiteral As New Literal
                                    objLiteral.ID = Globals.CreateValidID("Gallery" & objPhoto.PhotoID.ToString() & "-" & iPtr.ToString())
                                    objLiteral.Text = objPhoto.Width.ToString()
                                    phPhoto.Controls.Add(objLiteral)
                                Case Else
                                    If (_photoTemplateTokens(iPtr + 1).ToUpper().StartsWith("ISINROLE:")) Then
                                        Dim field As String = _photoTemplateTokens(iPtr + 1).Substring(9, _photoTemplateTokens(iPtr + 1).Length - 9)
                                        If (PortalSecurity.IsInRole(field) = False) Then
                                            Dim endToken As String = "/" & _photoTemplateTokens(iPtr + 1)
                                            While (iPtr < _photoTemplateTokens.Length - 1)
                                                If (_photoTemplateTokens(iPtr + 1) = endToken) Then
                                                    Exit While
                                                End If
                                                iPtr = iPtr + 1
                                            End While
                                        End If
                                        Exit Select
                                    End If

                                    If (_photoTemplateTokens(iPtr + 1).ToUpper().StartsWith("/ISINROLE:")) Then
                                        ' Do Nothing
                                        Exit Select
                                    End If

                                    If (_photoTemplateTokens(iPtr + 1).ToUpper().StartsWith("ISNOTINROLE:")) Then
                                        Dim field As String = _photoTemplateTokens(iPtr + 1).Substring(12, _photoTemplateTokens(iPtr + 1).Length - 12)
                                        If (PortalSecurity.IsInRole(field) = True) Then
                                            Dim endToken As String = "/" & _photoTemplateTokens(iPtr + 1)
                                            While (iPtr < _photoTemplateTokens.Length - 1)
                                                If (_photoTemplateTokens(iPtr + 1) = endToken) Then
                                                    Exit While
                                                End If
                                                iPtr = iPtr + 1
                                            End While
                                        End If
                                        Exit Select
                                    End If

                                    If (_photoTemplateTokens(iPtr + 1).ToUpper().StartsWith("/ISNOTINROLE:")) Then
                                        ' Do Nothing
                                        Exit Select
                                    End If

                                    Dim objLiteralOther As New Literal
                                    objLiteralOther.Text = "[" & _photoTemplateTokens(iPtr + 1) & "]"
                                    objLiteralOther.EnableViewState = False
                                    phPhoto.Controls.Add(objLiteralOther)
                            End Select

                        End If
                    Next

                End If

            End If

        End Sub

#End Region

    End Class

End Namespace