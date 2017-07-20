'
' Simple Gallery for DotNetNuke -  http://www.dotnetnuke.com
' Copyright (c) 2002-2007
' by Ventrian ( sales@ventrian.com ) ( http://www.ventrian.com )
'

Imports DotNetNuke.Common
Imports DotNetNuke.Common.Utilities
Imports DotNetNuke.Entities.Modules
Imports DotNetNuke.Entities.Modules.Actions
Imports DotNetNuke.Entities.Tabs
Imports DotNetNuke.Security
Imports DotNetNuke.Services.Exceptions
Imports DotNetNuke.Services.Localization

Imports Ventrian.SimpleGallery.Entities

Namespace Ventrian.SimpleGallery

    Partial Public Class SlideShow
        Inherits SimpleGalleryBase

#Region " Private Members "

        Private _itemID As Integer = Null.NullInteger
        Private _albumID As Integer = Null.NullInteger
        Private _searchText As String = Null.NullString
        Private _tagID As Integer = Null.NullInteger

#End Region

#Region " Private Methods "

        Private Sub BindBreadCrumbs()

            ucGalleryMenu.AddCrumb(Localization.GetString("AllAlbums", LocalResourceFile), NavigateURL())

            If (_tagID <> Null.NullInteger) Then
                ucGalleryMenu.AddCrumb(Localization.GetString("Tags", LocalResourceFile), NavigateURL(Me.TabId, "", "Tags=" & TabModuleId))

                Dim objTagController As New TagController
                Dim objTag As TagInfo = objTagController.Get(_tagID)
                If Not (objTag Is Nothing) Then
                    ucGalleryMenu.AddCrumb(objTag.Name, NavigateURL(Me.TabId, "", "Tag=" & objTag.NameLowered, "Tags=" & TabModuleId))
                End If
            End If

            If (_searchText <> Null.NullString) Then
                ucGalleryMenu.AddCrumb(Localization.GetString("Search", LocalResourceFile), NavigateURL(Me.TabId, "", "SearchID=" & TabModuleId))
                ucGalleryMenu.AddCrumb(_searchText, NavigateURL(Me.TabId, "", "SearchID=" & TabModuleId, "SearchText=" & System.Uri.EscapeDataString(_searchText)))
            End If
        End Sub

        Private Sub BindNav()

            Dim objPhotoController As New PhotoController
            Dim photoList As ArrayList

            If (_tagID <> Null.NullInteger) Then
                photoList = objPhotoController.List(Me.ModuleId, Null.NullInteger, True, Null.NullInteger, False, _tagID, Null.NullString, Null.NullString, Me.GallerySettings.SortBy, Me.GallerySettings.SortDirection)
            Else
                If (_searchText <> "") Then
                    photoList = objPhotoController.List(Me.ModuleId, Null.NullInteger, True, Null.NullInteger, Null.NullBoolean, Null.NullInteger, Null.NullString, _searchText, Me.GallerySettings.SortBy, Me.GallerySettings.SortDirection)
                Else
                    photoList = objPhotoController.List(Me.ModuleId, _albumID, True, Null.NullInteger, Null.NullBoolean, Null.NullInteger, Null.NullString, Null.NullString, Me.GallerySettings.SortBy, Me.GallerySettings.SortDirection)
                End If
            End If

            If (photoList.Count > 0) Then

                Dim i As Integer = 0
                For Each objPhoto As PhotoInfo In photoList

                    If (objPhoto.PhotoID = _itemID) Then

                        If (i) = 0 Then
                            lnkPrevious.Visible = False
                            lnkPreviousTop.Visible = False
                        Else
                            If (_tagID <> Null.NullInteger) Then
                                lnkPrevious.NavigateUrl = NavigateURL(Me.TabId, "", "galleryType=SlideShow", "ItemID=" & CType(photoList(i - 1), PhotoInfo).PhotoID.ToString(), "TagID=" & _tagID.ToString())
                                lnkPreviousTop.NavigateUrl = NavigateURL(Me.TabId, "", "galleryType=SlideShow", "ItemID=" & CType(photoList(i - 1), PhotoInfo).PhotoID.ToString(), "TagID=" & _tagID.ToString())
                            Else
                                If (_searchText <> "") Then
                                    lnkPrevious.NavigateUrl = NavigateURL(Me.TabId, "", "galleryType=SlideShow", "ItemID=" & CType(photoList(i - 1), PhotoInfo).PhotoID.ToString(), "SearchText=" & System.Uri.EscapeDataString(_searchText))
                                    lnkPreviousTop.NavigateUrl = NavigateURL(Me.TabId, "", "galleryType=SlideShow", "ItemID=" & CType(photoList(i - 1), PhotoInfo).PhotoID.ToString(), "SearchText=" & System.Uri.EscapeDataString(_searchText))
                                Else
                                    lnkPrevious.NavigateUrl = NavigateURL(Me.TabId, "", "galleryType=SlideShow", "ItemID=" & CType(photoList(i - 1), PhotoInfo).PhotoID.ToString())
                                    lnkPreviousTop.NavigateUrl = NavigateURL(Me.TabId, "", "galleryType=SlideShow", "ItemID=" & CType(photoList(i - 1), PhotoInfo).PhotoID.ToString())
                                End If
                            End If
                        End If

                        If (i + 1) = photoList.Count Then
                            lnkNext.Visible = False
                            lnkNextTop.Visible = False
                        Else
                            If (_tagID <> Null.NullInteger) Then
                                lnkNext.NavigateUrl = NavigateURL(Me.TabId, "", "galleryType=SlideShow", "ItemID=" & CType(photoList(i + 1), PhotoInfo).PhotoID.ToString(), "TagID=" & _tagID.ToString())
                                lnkNextTop.NavigateUrl = NavigateURL(Me.TabId, "", "galleryType=SlideShow", "ItemID=" & CType(photoList(i + 1), PhotoInfo).PhotoID.ToString(), "TagID=" & _tagID.ToString())
                            Else
                                If (_searchText <> "") Then
                                    lnkNext.NavigateUrl = NavigateURL(Me.TabId, "", "galleryType=SlideShow", "ItemID=" & CType(photoList(i + 1), PhotoInfo).PhotoID.ToString(), "SearchText=" & System.Uri.EscapeDataString(_searchText))
                                    lnkNextTop.NavigateUrl = NavigateURL(Me.TabId, "", "galleryType=SlideShow", "ItemID=" & CType(photoList(i + 1), PhotoInfo).PhotoID.ToString(), "SearchText=" & System.Uri.EscapeDataString(_searchText))
                                Else
                                    lnkNext.NavigateUrl = NavigateURL(Me.TabId, "", "galleryType=SlideShow", "ItemID=" & CType(photoList(i + 1), PhotoInfo).PhotoID.ToString())
                                    lnkNextTop.NavigateUrl = NavigateURL(Me.TabId, "", "galleryType=SlideShow", "ItemID=" & CType(photoList(i + 1), PhotoInfo).PhotoID.ToString())
                                End If
                            End If
                        End If

                        lblPageCount.Text = String.Format(Localization.GetString("PhotoCount.Text", LocalResourceFile), (i + 1).ToString(), photoList.Count.ToString())
                    End If

                    i = i + 1
                Next

            End If

            lnkReturnToOrigin.NavigateUrl = NavigateURL(Me.TabId, "", "AlbumID=" & Me.ModuleId.ToString() & "-" & _albumID)

        End Sub

        Private Function GetTagUrl(ByVal tag As String, ByVal tabID As Integer, ByVal moduleID As Integer, ByVal tabModuleID As Integer) As String

            If (AllLetters(tag) = False) Then
                Dim objTagController As New TagController()
                Dim objTag As TagInfo = objTagController.Get(moduleID, tag.ToLower())

                If Not (objTag Is Nothing) Then
                    Return NavigateURL(tabID, "", "TagID=" & objTag.TagID.ToString(), "Tags=" & tabModuleID.ToString())
                End If
            Else
                Return NavigateURL(tabID, "", "Tag=" & tag, "Tags=" & tabModuleID.ToString())
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


        Private Sub BindPhoto()

            Dim objPhotoController As New PhotoController

            Dim objPhoto As PhotoInfo = objPhotoController.Get(_itemID)

            If Not (objPhoto Is Nothing) Then
                lblName.Text = objPhoto.Name
                If (objPhoto.Tags <> "") Then
                    Dim tags As String() = objPhoto.Tags.Split(","c)
                    lblTags.Text = "<b>Tags:</b>"
                    For Each tag As String In tags
                        lblTags.Text = lblTags.Text & " <a href='" & GetTagUrl(tag, TabId, ModuleId, TabModuleId) & "'>" & tag & "</a>"
                    Next
                End If
                lblDescription.Text = objPhoto.Description

                If (objPhoto.Name <> "") Then
                    Me.BasePage.Title = objPhoto.Name
                End If

                If (objPhoto.Description <> "") Then
                    Me.BasePage.Description = objPhoto.Description
                End If

                If (objPhoto.Tags <> "") Then
                    Me.BasePage.KeyWords = objPhoto.Tags.Replace(" ", ",")
                End If

                If (objPhoto.Width > Me.GallerySettings.ImageWidth Or objPhoto.Height > Me.GallerySettings.ImageHeight) Then
                    ' Use Handler to Resize 
                    Dim width As Integer = objPhoto.Width
                    Dim height As Integer = objPhoto.Height

                    If (width > GallerySettings.ImageWidth) Then
                        width = GallerySettings.ImageWidth
                        height = Convert.ToInt32(height / (objPhoto.Width / GallerySettings.ImageWidth))
                    End If

                    If (height > GallerySettings.ImageHeight) Then
                        height = GallerySettings.ImageHeight
                        width = Convert.ToInt32(objPhoto.Width / (objPhoto.Height / GallerySettings.ImageHeight))
                    End If

                    imgPhoto.Width = Unit.Pixel(width)
                    imgPhoto.Height = Unit.Pixel(height)

                    If (Me.GallerySettings.Compression = Common.CompressionType.Quality) Then
                        imgPhoto.ImageUrl = Me.ResolveUrl("ImageHandler.ashx?width=" & width.ToString() & "&height=" & height.ToString() & "&HomeDirectory=" & System.Uri.EscapeDataString(Me.PortalSettings.HomeDirectory & objPhoto.HomeDirectory) & "&fileName=" & System.Uri.EscapeDataString(objPhoto.FileName) & "&portalid=" & Me.PortalId.ToString() & "&q=1")
                    Else
                        imgPhoto.ImageUrl = Me.ResolveUrl("ImageHandler.ashx?width=" & width.ToString() & "&height=" & height.ToString() & "&HomeDirectory=" & System.Uri.EscapeDataString(Me.PortalSettings.HomeDirectory & objPhoto.HomeDirectory) & "&fileName=" & System.Uri.EscapeDataString(objPhoto.FileName) & "&portalid=" & Me.PortalId.ToString())
                    End If
                    lnkDownload.NavigateUrl = Me.PortalSettings.HomeDirectory & objPhoto.HomeDirectory & "/" & System.Uri.EscapeDataString(objPhoto.FileName)
                Else
                    imgPhoto.Width = Unit.Pixel(objPhoto.Width)
                    imgPhoto.Height = Unit.Pixel(objPhoto.Height)
                    imgPhoto.ImageUrl = Me.PortalSettings.HomeDirectory & objPhoto.HomeDirectory & "/" & System.Uri.EscapeDataString(objPhoto.FileName)
                    lnkDownload.NavigateUrl = Me.PortalSettings.HomeDirectory & objPhoto.HomeDirectory & "/" & System.Uri.EscapeDataString(objPhoto.FileName)
                End If

                If (Me.GallerySettings.EnableTooltip) Then
                    If (objPhoto.Description.Trim().Length > 0) Then
                        imgPhoto.AlternateText = objPhoto.Description().Replace(Chr(34), "")
                    Else
                        imgPhoto.AlternateText = objPhoto.Name.Replace(Chr(34), "")
                    End If
                End If

                _albumID = objPhoto.AlbumID()

                If (_tagID = Null.NullInteger And _searchText = Null.NullString) Then

                    Dim objAlbumController As New AlbumController
                    Dim objAlbum As AlbumInfo = objAlbumController.Get(objPhoto.AlbumID)

                    While Not objAlbum Is Nothing
                        If (GallerySettings.AlbumFilter <> objAlbum.AlbumID.ToString()) Then
                            ucGalleryMenu.InsertCrumb(1, objAlbum.Caption, NavigateURL(Me.TabId, "", "AlbumID=" & Me.ModuleId.ToString() & "-" & objAlbum.AlbumID.ToString()))
                        End If

                        If (objAlbum.ParentAlbumID = Null.NullInteger) Then
                            objAlbum = Nothing
                        Else
                            objAlbum = objAlbumController.Get(objAlbum.ParentAlbumID)
                        End If
                    End While

                End If

                If (_tagID <> Null.NullInteger) Then
                    ucGalleryMenu.AddCrumb(objPhoto.Name, NavigateURL(Me.TabId, "", "galleryType=SlideShow", "ItemID=" & objPhoto.PhotoID.ToString(), "TagID=" & _tagID.ToString()))
                Else
                    ucGalleryMenu.AddCrumb(objPhoto.Name, NavigateURL(Me.TabId, "", "galleryType=SlideShow", "ItemID=" & objPhoto.PhotoID.ToString()))
                End If

            Else
                Response.Redirect(NavigateURL, True)
            End If

        End Sub

        Private Sub ReadQueryString()

            If Not (Request.QueryString("ItemId") Is Nothing) Then
                _itemID = Int32.Parse(Request.QueryString("ItemId"))
            Else
                Response.Redirect(NavigateURL, True)
            End If

            If Not (Request.QueryString("TagID") Is Nothing) Then
                _tagID = Int32.Parse(Request.QueryString("TagID"))
            End If

            If Not (Request.QueryString("SearchText") Is Nothing) Then
                _searchText = Server.UrlDecode(Request.QueryString("SearchText"))
            End If

        End Sub

#End Region

#Region " Protected Methods "

        Protected Function GetWidth() As String

            Return GallerySettings.StandardWidth.ToString() & "px"

        End Function

#End Region

#Region " Event Handlers "

        Private Sub Page_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load

            Try

                ReadQueryString()
                BindBreadCrumbs()
                BindPhoto()
                BindNav()

            Catch exc As Exception    'Module failed to load
                ProcessModuleLoadException(Me, exc)
            End Try

        End Sub

#End Region

    End Class

End Namespace