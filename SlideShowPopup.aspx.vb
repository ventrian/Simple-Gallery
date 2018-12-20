'
' Simple Gallery for DotNetNuke -  http://www.dotnetnuke.com
' Copyright (c) 2002-2007
' by Ventrian ( sales@ventrian.com ) ( http://www.ventrian.com )
'

Imports System.IO

Imports DotNetNuke.Common
Imports DotNetNuke.Common.Utilities
Imports DotNetNuke.Entities.Host
Imports DotNetNuke.Entities.Modules
Imports DotNetNuke.Entities.Modules.Actions
Imports DotNetNuke.Entities.Tabs
Imports DotNetNuke.Framework
Imports DotNetNuke.Security
Imports DotNetNuke.Services.Exceptions
Imports DotNetNuke.Services.Localization

Imports Ventrian.SimpleGallery.Entities

Namespace Ventrian.SimpleGallery

    Partial Public Class SlideShowPopup
        Inherits PageBase

#Region " Private Members "

        Private _itemID As Integer = Null.NullInteger
        Private _albumID As Integer = Null.NullInteger
        Private _portalID As Integer = Null.NullInteger
        Private _searchText As String = Null.NullString
        Private _tagID As Integer = Null.NullInteger
        Private _border As String = "White"

        Private _sortBy As Common.SortType = Common.SortType.Name
        Private _sortDirection As Common.SortDirection = Common.SortDirection.ASC

        Private _enableTooltip As Boolean = Common.Constants.DEFAULT_ENABLE_TOOLTIP

#End Region

#Region " Private Properties "

        Private ReadOnly Property MyLocalResourceFile() As String
            Get
                Return Me.TemplateSourceDirectory & "/App_LocalResources/Slideshow.ascx.resx"
            End Get
        End Property

#End Region

#Region " Private Methods "

        Private Sub ReadQueryString()

            If Not (Request.QueryString("ItemId") Is Nothing) Then
                _itemID = Int32.Parse(Request.QueryString("ItemId"))
            End If

            If Not (Request.QueryString("PortalID") Is Nothing) Then
                _portalID = Int32.Parse(Request.QueryString("PortalID"))
            End If

            If Not (Request.QueryString("SearchText") Is Nothing) Then
                _searchText = Server.UrlDecode(Request.QueryString("SearchText"))
            End If

            If Not (Request.QueryString("TagID") Is Nothing) Then
                _tagID = Int32.Parse(Request.QueryString("TagID"))
            End If

            If Not (Request.QueryString("sb") Is Nothing) Then
                Try
                    _sortBy = CType(Request.QueryString("sb"), Common.SortType)
                Catch
                End Try
            End If

            If Not (Request.QueryString("sd") Is Nothing) Then
                Try
                    _sortDirection = CType(Request.QueryString("sd"), Common.SortDirection)
                Catch
                End Try
            End If

            If Not (Request.QueryString("Border") Is Nothing) Then
                _border = Request.QueryString("Border")
            End If

            If Not (Request.QueryString("tt") Is Nothing) Then
                If (Request("tt") = "False") Then
                    _enableTooltip = False
                End If
            End If

        End Sub

        Private Sub AssignLocalization()

            lnkPrevious.Text = Localization.GetString("Previous", MyLocalResourceFile)
            lnkNext.Text = Localization.GetString("Next", MyLocalResourceFile)

            lnkClose.Text = Localization.GetString("CloseWindow", MyLocalResourceFile)
            litTitle.Text = Localization.GetString("ViewingAlbum", MyLocalResourceFile)
            lnkDownload.Text = Localization.GetString("DownloadPhoto", MyLocalResourceFile)

        End Sub

        Private Sub BindStyles()

            Dim objCSS As Control = Me.FindControl("CSS")

            Dim objCSSCache As Hashtable = CType(DataCache.GetCache("CSS"), Hashtable)
            If objCSSCache Is Nothing Then
                objCSSCache = New Hashtable
            End If

            If Not (objCSS Is Nothing) Then

                Dim objLink As HtmlGenericControl

                ID = CreateValidID(DotNetNuke.Common.Globals.HostPath)
                objLink = New HtmlGenericControl("LINK")
                objLink.ID = ID
                objLink.Attributes("rel") = "stylesheet"
                objLink.Attributes("type") = "text/css"
                objLink.Attributes("href") = DotNetNuke.Common.Globals.HostPath & "default.css"
                objCSS.Controls.Add(objLink)

                ' skin package style sheet
                ID = CreateValidID(PortalSettings.ActiveTab.SkinPath)
                If objCSSCache.ContainsKey(ID) = False Then
                    If File.Exists(Server.MapPath(PortalSettings.ActiveTab.SkinPath) & "skin.css") Then
                        objCSSCache(ID) = PortalSettings.ActiveTab.SkinPath & "skin.css"
                    Else
                        objCSSCache(ID) = ""
                    End If
                    If Not Host.PerformanceSetting = PerformanceSettings.NoCaching Then
                        DataCache.SetCache("CSS", objCSSCache)
                    End If
                End If
                If objCSSCache(ID).ToString <> "" Then
                    objLink = New HtmlGenericControl("LINK")
                    objLink.ID = ID
                    objLink.Attributes("rel") = "stylesheet"
                    objLink.Attributes("type") = "text/css"
                    objLink.Attributes("href") = objCSSCache(ID).ToString
                    objCSS.Controls.Add(objLink)
                End If

                ' skin file style sheet
                ID = CreateValidID(Replace(PortalSettings.ActiveTab.SkinSrc, ".ascx", ".css"))
                If objCSSCache.ContainsKey(ID) = False Then
                    If File.Exists(Server.MapPath(Replace(PortalSettings.ActiveTab.SkinSrc, ".ascx", ".css"))) Then
                        objCSSCache(ID) = Replace(PortalSettings.ActiveTab.SkinSrc, ".ascx", ".css")
                    Else
                        objCSSCache(ID) = ""
                    End If
                    If Not Host.PerformanceSetting = PerformanceSettings.NoCaching Then
                        DataCache.SetCache("CSS", objCSSCache)
                    End If
                End If
                If objCSSCache(ID).ToString <> "" Then
                    objLink = New HtmlGenericControl("LINK")
                    objLink.ID = ID
                    objLink.Attributes("rel") = "stylesheet"
                    objLink.Attributes("type") = "text/css"
                    objLink.Attributes("href") = objCSSCache(ID).ToString
                    objCSS.Controls.Add(objLink)
                End If

                ' portal style sheet
                ID = CreateValidID(PortalSettings.HomeDirectory)
                objLink = New HtmlGenericControl("LINK")
                objLink.ID = ID
                objLink.Attributes("rel") = "stylesheet"
                objLink.Attributes("type") = "text/css"
                objLink.Attributes("href") = PortalSettings.HomeDirectory & "portal.css"
                objCSS.Controls.Add(objLink)

            End If

        End Sub

        Private Sub BindPhoto()

            Dim objPhotoController As New PhotoController

            Dim objPhoto As PhotoInfo = objPhotoController.Get(_itemID)

            If Not (objPhoto Is Nothing) Then

                lblName.Text = objPhoto.Name
                lblDescription.Text = objPhoto.Description

                Dim objModuleController As New ModuleController
                Dim settings As Hashtable = objModuleController.GetModuleSettings(objPhoto.ModuleID)

                Dim slideShowWidth As Integer = Common.Constants.DEFAULT_WIDTH
                Dim slideShowHeight As Integer = Common.Constants.DEFAULT_HEIGHT

                If (settings.Contains(Common.Constants.SETTING_WIDTH)) Then
                    slideShowWidth = Convert.ToInt32(settings(Common.Constants.SETTING_WIDTH))
                End If

                If (settings.Contains(Common.Constants.SETTING_HEIGHT)) Then
                    slideShowHeight = Convert.ToInt32(settings(Common.Constants.SETTING_HEIGHT))
                End If

                If (objPhoto.Width > slideShowWidth Or objPhoto.Height > slideShowHeight) Then
                    ' Use Handler to Resize 
                    Dim width As Integer = objPhoto.Width
                    Dim height As Integer = objPhoto.Height

                    If (width > slideShowWidth) Then
                        width = slideShowWidth
                        height = Convert.ToInt32(height / (objPhoto.Width / slideShowWidth))
                    End If

                    If (height > slideShowHeight) Then
                        height = slideShowHeight
                        width = Convert.ToInt32(objPhoto.Width / (objPhoto.Height / slideShowHeight))
                    End If

                    imgPhoto.Width = Unit.Pixel(width)
                    imgPhoto.Height = Unit.Pixel(height)

                    imgPhoto.ImageUrl = Me.ResolveUrl("ImageHandler.ashx?width=" & width.ToString() & "&height=" & height.ToString() & "&HomeDirectory=" & System.Uri.EscapeDataString(Me.PortalSettings.HomeDirectory & objPhoto.HomeDirectory) & "&fileName=" & System.Uri.EscapeDataString(objPhoto.FileName) & "&portalid=" & _portalID.ToString() & "q=1")
                    lnkDownload.NavigateUrl = Me.PortalSettings.HomeDirectory & objPhoto.HomeDirectory & "/" & System.Uri.EscapeDataString(objPhoto.FileName)
                Else
                    imgPhoto.Width = Unit.Pixel(objPhoto.Width)
                    imgPhoto.Height = Unit.Pixel(objPhoto.Height)

                    imgPhoto.ImageUrl = Me.PortalSettings.HomeDirectory & objPhoto.HomeDirectory & "/" & System.Uri.EscapeDataString(objPhoto.FileName)
                    lnkDownload.NavigateUrl = Me.PortalSettings.HomeDirectory & objPhoto.HomeDirectory & "/" & System.Uri.EscapeDataString(objPhoto.FileName)
                End If

                _albumID = objPhoto.AlbumID

                lnkClose.Attributes.Add("OnClick", "javascript:window.close()")
                lnkClose.NavigateUrl = "#"

                Dim objAlbumController As New AlbumController
                Dim objAlbum As AlbumInfo = objAlbumController.Get(objPhoto.AlbumID)

                If Not (objAlbum Is Nothing) Then
                    litTitle.Text = litTitle.Text & " " & objAlbum.Caption
                End If

            End If

        End Sub

        Private Sub BindNav()

            Dim objPhotoController As New PhotoController

            Dim objPhotoShown As PhotoInfo = objPhotoController.Get(_itemID)

            If Not (objPhotoShown Is Nothing) Then

                Dim photoList As ArrayList

                If (_tagID <> Null.NullInteger) Then
                    photoList = objPhotoController.List(objPhotoShown.ModuleID, Null.NullInteger, True, Null.NullInteger, False, _tagID, Null.NullString, Null.NullString, _sortBy, _sortDirection)
                Else
                    If (_searchText <> "") Then
                        photoList = objPhotoController.List(objPhotoShown.ModuleID, Null.NullInteger, True, Null.NullInteger, Null.NullBoolean, Null.NullInteger, Null.NullString, _searchText, _sortBy, _sortDirection)
                    Else
                        photoList = objPhotoController.List(objPhotoShown.ModuleID, _albumID, True, Null.NullInteger, Null.NullBoolean, Null.NullInteger, Null.NullString, Null.NullString, _sortBy, _sortDirection)
                    End If
                End If

                If (photoList.Count > 0) Then

                    Dim i As Integer = 0
                    For Each objPhoto As PhotoInfo In photoList

                        If (objPhoto.PhotoID = _itemID) Then
                            If (i) = 0 Then
                                lnkPrevious.Enabled = False
                            Else
                                If (_tagID <> Null.NullInteger) Then
                                    lnkPrevious.NavigateUrl = Me.ResolveUrl("SlideShowPopup.aspx?PortalID=" & Me.PortalSettings.PortalId.ToString() & "&ItemID=" & CType(photoList(i - 1), PhotoInfo).PhotoID.ToString() & "&TagID=" & _tagID.ToString() & "&Border=" & _border & "&sb=" & Convert.ToInt32(_sortBy).ToString() & "&sd=" & Convert.ToInt32(_sortDirection).ToString())
                                Else
                                    If (_searchText <> "") Then
                                        lnkPrevious.NavigateUrl = Me.ResolveUrl("SlideShowPopup.aspx?PortalID=" & Me.PortalSettings.PortalId.ToString() & "&ItemID=" & CType(photoList(i - 1), PhotoInfo).PhotoID.ToString() & "&SearchText=" & System.Uri.EscapeDataString(_searchText) & "&Border=" & _border & "&sb=" & Convert.ToInt32(_sortBy).ToString() & "&sd=" & Convert.ToInt32(_sortDirection).ToString())
                                    Else
                                        lnkPrevious.NavigateUrl = Me.ResolveUrl("SlideShowPopup.aspx?PortalID=" & Me.PortalSettings.PortalId.ToString() & "&ItemID=" & CType(photoList(i - 1), PhotoInfo).PhotoID.ToString() & "&Border=" & _border & "&sb=" & Convert.ToInt32(_sortBy).ToString() & "&sd=" & Convert.ToInt32(_sortDirection).ToString())
                                    End If
                                End If
                            End If

                            If (i + 1) = photoList.Count Then
                                lnkNext.Enabled = False
                                lnkPhoto.Enabled = False
                            Else
                                If (_tagID <> Null.NullInteger) Then
                                    lnkNext.NavigateUrl = Me.ResolveUrl("SlideShowPopup.aspx?PortalID=" & Me.PortalSettings.PortalId.ToString() & "&ItemID=" & CType(photoList(i + 1), PhotoInfo).PhotoID.ToString() & "&TagID=" & _tagID.ToString() & "&Border=" & _border & "&sb=" & Convert.ToInt32(_sortBy).ToString() & "&sd=" & Convert.ToInt32(_sortDirection).ToString())
                                    lnkPhoto.NavigateUrl = Me.ResolveUrl("SlideShowPopup.aspx?PortalID=" & Me.PortalSettings.PortalId.ToString() & "&ItemID=" & CType(photoList(i + 1), PhotoInfo).PhotoID.ToString() & "&TagID=" & _tagID.ToString() & "&Border=" & _border & "&sb=" & Convert.ToInt32(_sortBy).ToString() & "&sd=" & Convert.ToInt32(_sortDirection).ToString())
                                Else
                                    If (_searchText <> "") Then
                                        lnkNext.NavigateUrl = Me.ResolveUrl("SlideShowPopup.aspx?PortalID=" & Me.PortalSettings.PortalId.ToString() & "&ItemID=" & CType(photoList(i + 1), PhotoInfo).PhotoID.ToString() & "&SearchText=" & System.Uri.EscapeDataString(_searchText) & "&Border=" & _border & "&sb=" & Convert.ToInt32(_sortBy).ToString() & "&sd=" & Convert.ToInt32(_sortDirection).ToString())
                                        lnkPhoto.NavigateUrl = Me.ResolveUrl("SlideShowPopup.aspx?PortalID=" & Me.PortalSettings.PortalId.ToString() & "&ItemID=" & CType(photoList(i + 1), PhotoInfo).PhotoID.ToString() & "&SearchText=" & System.Uri.EscapeDataString(_searchText) & "&Border=" & _border & "&sb=" & Convert.ToInt32(_sortBy).ToString() & "&sd=" & Convert.ToInt32(_sortDirection).ToString())
                                    Else
                                        lnkNext.NavigateUrl = Me.ResolveUrl("SlideShowPopup.aspx?PortalID=" & Me.PortalSettings.PortalId.ToString() & "&ItemID=" & CType(photoList(i + 1), PhotoInfo).PhotoID.ToString() & "&Border=" & _border & "&sb=" & Convert.ToInt32(_sortBy).ToString() & "&sd=" & Convert.ToInt32(_sortDirection).ToString())
                                        lnkPhoto.NavigateUrl = Me.ResolveUrl("SlideShowPopup.aspx?PortalID=" & Me.PortalSettings.PortalId.ToString() & "&ItemID=" & CType(photoList(i + 1), PhotoInfo).PhotoID.ToString() & "&Border=" & _border & "&sb=" & Convert.ToInt32(_sortBy).ToString() & "&sd=" & Convert.ToInt32(_sortDirection).ToString())
                                    End If
                                 End If
                            End If

                            If (_enableTooltip) Then
                                If (objPhoto.Description.Trim().Length > 0) Then
                                    imgPhoto.AlternateText = objPhoto.Description().Replace(Chr(34), "")
                                Else
                                    imgPhoto.AlternateText = objPhoto.Name.Replace(Chr(34), "")
                                End If
                            End If

                            lblPageCount.Text = String.Format(Localization.GetString("PhotoCount.Text", MyLocalResourceFile), (i + 1).ToString(), photoList.Count.ToString())
                        End If

                        i = i + 1
                    Next

                End If

            End If

        End Sub

#End Region

#Region " Protected Methods "

        Protected Function FormatBorderPath(ByVal image As String) As String

            Return Me.ResolveUrl("Images/Borders/" & Request("Border") & "/" & image)

        End Function

#End Region

#Region " Event Handlers "

        Private Sub Page_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load

            Try

                ReadQueryString()
                AssignLocalization()
                BindStyles()
                BindPhoto()
                BindNav()

            Catch exc As Exception
                ProcessModuleLoadException(Me, exc)
            End Try

        End Sub

#End Region

    End Class

End Namespace