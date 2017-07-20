'
' Property Agent for DotNetNuke -  http://www.dotnetnuke.com
' Copyright (c) 2002-2007
' by Ventrian ( sales@ventrian.com ) ( http://www.ventrian.com )
'

Imports DotNetNuke.Common
Imports DotNetNuke.Common.Utilities
Imports DotNetNuke.Services.Exceptions
Imports DotNetNuke.Services.Localization

Imports Ventrian.SimpleGallery.Entities

Namespace Ventrian.SimpleGallery.Controls

    Partial Public Class GalleryMenu
        Inherits SimpleGalleryControlBase

#Region " Private Members "

        Private _objBreadCrumbs As ArrayList
        Private _albumID As Integer = Null.NullInteger
        Private _tag As String = Null.NullString
        Private _tagID As Integer = Null.NullInteger

#End Region

#Region " Private Properties "

        Private ReadOnly Property BreadCrumbs() As ArrayList
            Get
                If (_objBreadCrumbs Is Nothing) Then
                    _objBreadCrumbs = New ArrayList
                End If
                Return _objBreadCrumbs
            End Get
        End Property

        Private Shadows ReadOnly Property LocalResourceFile() As String
            Get
                Return "~/DesktopModules/SimpleGallery/App_LocalResources/GalleryMenu.ascx.resx"
            End Get
        End Property

#End Region

#Region " Private Methods "

        Private Sub AssignLocalization()

            lnkAddNewPhoto.Text = Localization.GetString("AddNewPhoto", Me.LocalResourceFile)
            lnkAddNewAlbum.Text = Localization.GetString("AddNewAlbum", Me.LocalResourceFile)
            lnkApprovePhotos.Text = Localization.GetString("ApprovePhotos", Me.LocalResourceFile)
            lnkBulkEdit.Text = Localization.GetString("BulkEdit", Me.LocalResourceFile)
            lnkTags.Text = Localization.GetString("Tags", Me.LocalResourceFile)
            lnkSearch.Text = Localization.GetString("Search", Me.LocalResourceFile)

        End Sub

        Private Sub AssignLinks()

            lnkAddNewPhoto.Visible = SimpleGalleryBase.HasUploadPhotoPermissions()

            If (SimpleGalleryBase.HasEditPermissions = False And SimpleGalleryBase.HasUploadPhotoPermissions = True) Then
                Dim objAlbumController As New AlbumController
                Dim objAlbums As ArrayList = objAlbumController.List(SimpleGalleryBase.ModuleId, Null.NullInteger, True, False, Common.AlbumSortType.Caption, Common.SortDirection.ASC)
                lnkAddNewPhoto.Visible = (objAlbums.Count > 0)
            End If

            If (_albumID <> Null.NullInteger) Then
                lnkAddNewPhoto.NavigateUrl = SimpleGalleryBase.EditUrl("ReturnUrl", SimpleGalleryBase.ReturnUrl, "Add", "SelAlbumID=" & _albumID.ToString())
            Else
                lnkAddNewPhoto.NavigateUrl = SimpleGalleryBase.EditUrl("ReturnUrl", SimpleGalleryBase.ReturnUrl, "Add")
            End If

            lnkAddNewAlbum.Visible = SimpleGalleryBase.HasAlbumPermissions
            If (_albumID <> Null.NullInteger) Then
                lnkAddNewAlbum.NavigateUrl = SimpleGalleryBase.EditUrl("ReturnUrl", SimpleGalleryBase.ReturnUrl, "EditAlbum", "SelAlbumID=" & _albumID.ToString())
            Else
                lnkAddNewAlbum.NavigateUrl = SimpleGalleryBase.EditUrl("ReturnUrl", SimpleGalleryBase.ReturnUrl, "EditAlbum")
            End If

            lnkApprovePhotos.Visible = (SimpleGalleryBase.HasApprovePhotoPermissions And SimpleGalleryBase.GallerySettings.PhotoModeration)
            lnkApprovePhotos.NavigateUrl = SimpleGalleryBase.EditUrl("ReturnUrl", SimpleGalleryBase.ReturnUrl, "Approve")

            If (lnkApprovePhotos.Visible) Then

                Dim approve As String = ""
                approve = Localization.GetString("ApprovePhotos", Me.LocalResourceFile)

                If (approve.IndexOf("{0}") <> -1) Then
                    Dim objPhotoController As New PhotoController

                    Dim photos As ArrayList
                    If (SimpleGalleryBase.GallerySettings.AlbumFilter <> "-1") Then
                        photos = objPhotoController.List(SimpleGalleryBase.ModuleId, Convert.ToInt32(SimpleGalleryBase.GallerySettings.AlbumFilter), False, Null.NullInteger, Null.NullBoolean, Null.NullInteger, Null.NullString, Null.NullString, Me.SimpleGalleryBase.GallerySettings.SortBy, Me.SimpleGalleryBase.GallerySettings.SortDirection)
                    Else
                        photos = objPhotoController.List(SimpleGalleryBase.ModuleId, Null.NullInteger, False, Null.NullInteger, Null.NullBoolean, Null.NullInteger, Null.NullString, Null.NullString, Me.SimpleGalleryBase.GallerySettings.SortBy, Me.SimpleGalleryBase.GallerySettings.SortDirection)
                    End If

                    lnkApprovePhotos.Text = String.Format(approve, photos.Count.ToString())
                Else
                    lnkApprovePhotos.Text = approve
                End If

            End If

            lnkBulkEdit.Visible = False
            If (_albumID <> Null.NullInteger) Then
                lnkBulkEdit.Visible = SimpleGalleryBase.IsEditable
                lnkBulkEdit.NavigateUrl = SimpleGalleryBase.EditUrl("AlbumID", _albumID.ToString(), "BulkEdit", "ReturnUrl=" & SimpleGalleryBase.ReturnUrl)
            Else
                If (_tag <> "") Then
                    Dim objTagController As New TagController
                    Dim objTag As TagInfo = objTagController.Get(SimpleGalleryBase.ModuleId, _tag.ToLower())

                    If Not (objTag Is Nothing) Then
                        lnkBulkEdit.Visible = SimpleGalleryBase.IsEditable
                        lnkBulkEdit.NavigateUrl = SimpleGalleryBase.EditUrl("TagID", objTag.TagID.ToString(), "BulkEdit", "ReturnUrl=" & SimpleGalleryBase.ReturnUrl)
                    End If
                End If

                If (_tagID <> Null.NullInteger) Then
                    lnkBulkEdit.Visible = SimpleGalleryBase.IsEditable
                    lnkBulkEdit.NavigateUrl = SimpleGalleryBase.EditUrl("TagID", _tagID.ToString(), "BulkEdit", "ReturnUrl=" & SimpleGalleryBase.ReturnUrl)
                End If
            End If

            lnkTags.Visible = SimpleGalleryBase.GallerySettings.EnableTags
            lnkTags.NavigateUrl = NavigateURL(SimpleGalleryBase.TabId, "", "Tags=" & SimpleGalleryBase.TabModuleId)

            phSearch.Visible = SimpleGalleryBase.GallerySettings.EnableSearch
            lnkSearch.NavigateUrl = NavigateURL(SimpleGalleryBase.TabId, "", "SearchID=" & SimpleGalleryBase.TabModuleId)

            If (lnkAddNewPhoto.Visible = False And lnkAddNewAlbum.Visible = False And lnkBulkEdit.Visible = False And lnkApprovePhotos.Visible = False) Then
                pnlCommandBar.Visible = False
            End If

        End Sub

        Private Sub BindBreadCrumbs()

            rptBreadCrumbs.Visible = Not SimpleGalleryBase.GallerySettings.HideBreadCrumbs

            If (BreadCrumbs.Count > 1) Then
                rptBreadCrumbs.DataSource = BreadCrumbs
                rptBreadCrumbs.DataBind()
            End If

        End Sub

        Private Sub BindRSS()

            If Me.SimpleGalleryBase.GallerySettings.EnableSyndication Then

                Dim albumID As Integer = Null.NullInteger
                Dim tagID As Integer = Null.NullInteger

                If (Request("AlbumID") <> "") Then
                    Dim split As String() = Request("AlbumID").Split("-"c)
                    If (split.Length > 1) Then
                        albumID = Convert.ToInt32(split(1))
                    End If
                End If

                If (Request("Tag") <> "" Or Request("TagID") <> "") Then
                    If (Request("TagID") <> "") Then
                        tagID = Convert.ToInt32(Request("TagID"))
                    Else
                        Dim objTagController As New TagController
                        Dim objTag As TagInfo = objTagController.Get(SimpleGalleryBase.ModuleId, Request("Tag").ToLower())

                        If Not (objTag Is Nothing) Then
                            tagID = objTag.TagID
                        End If
                    End If
                End If

                If (albumID <> Null.NullInteger) Then
                    lnkRSS.NavigateUrl = Page.ResolveUrl("~/DesktopModules/SimpleGallery/RSS.aspx?t=" & SimpleGalleryBase.TabId.ToString() & "&m=" & SimpleGalleryBase.ModuleId.ToString() & "&tm=" & SimpleGalleryBase.TabModuleId.ToString() & "&a=" + albumID.ToString() & "&portalid=" & SimpleGalleryBase.PortalId)

                    Dim latestPhotosFor As String = Localization.GetString("LatestPhotosFor", Me.LocalResourceFile)
                    If (latestPhotosFor.IndexOf("{0}") <> -1) Then
                        Dim objAlbumController As New AlbumController
                        Dim objAlbum As AlbumInfo = objAlbumController.Get(albumID)

                        If Not (objAlbum Is Nothing) Then
                            lnkRSS.ToolTip = String.Format(latestPhotosFor, objAlbum.Caption)
                        End If
                    Else
                        lnkRSS.ToolTip = latestPhotosFor
                    End If
                Else

                    If (tagID <> Null.NullInteger) Then
                        lnkRSS.NavigateUrl = Page.ResolveUrl("~/DesktopModules/SimpleGallery/RSS.aspx?t=" & SimpleGalleryBase.TabId.ToString() & "&m=" & SimpleGalleryBase.ModuleId.ToString() & "&tm=" & SimpleGalleryBase.TabModuleId.ToString() & "&TagID=" + tagID.ToString() & "&portalid=" & SimpleGalleryBase.PortalId)

                        Dim latestPhotosFor As String = Localization.GetString("LatestPhotosFor", Me.LocalResourceFile)
                        If (latestPhotosFor.IndexOf("{0}") <> -1) Then
                            lnkRSS.ToolTip = String.Format(latestPhotosFor, Request("Tag"))
                        Else
                            lnkRSS.ToolTip = latestPhotosFor
                        End If
                    Else
                        lnkRSS.NavigateUrl = Page.ResolveUrl("~/DesktopModules/SimpleGallery/RSS.aspx?t=" & SimpleGalleryBase.TabId.ToString() & "&m=" & SimpleGalleryBase.ModuleId.ToString() & "&tm=" & SimpleGalleryBase.TabModuleId.ToString() & "&MaxCount=10&portalid=" & SimpleGalleryBase.PortalId)
                        lnkRSS.ToolTip = Localization.GetString("LatestPhotos", Me.LocalResourceFile)
                    End If

                End If

                imgRSS.ToolTip = Me.SimpleGalleryBase.ModuleConfiguration.ModuleTitle & ": " & lnkRSS.ToolTip

                If (lnkRSS.NavigateUrl <> "") Then
                    Me.SimpleGalleryBase.AddRSSFeed(lnkRSS.NavigateUrl, lnkRSS.ToolTip)
                End If

            Else

                lnkRSS.Visible = False

            End If

        End Sub

        Private Sub ReadQueryString()

            If (Request("AlbumID") <> "") Then
                Try
                    _albumID = Convert.ToInt32(Request("AlbumID").Split("-"c)(1))
                Catch
                End Try
            End If

            If (Request("Tag") <> "") Then
                _tag = Request("Tag")
            End If

            If (Request("TagID") <> "") Then
                _tagID = Convert.ToInt32(Request("TagID"))
            End If

        End Sub

#End Region

#Region " Public Methods "

        Public Sub AddCrumb(ByVal caption As String, ByVal url As String)

            BreadCrumbs.Add(New CrumbInfo(caption, url))

        End Sub

        Public Sub InsertCrumb(ByVal index As Integer, ByVal caption As String, ByVal url As String)

            BreadCrumbs.Insert(index, New CrumbInfo(caption, url))

        End Sub

#End Region

#Region " Public Properties "

        Public Property ShowCommandBar() As Boolean
            Get
                Return pnlCommandBar.Visible
            End Get
            Set(ByVal Value As Boolean)
                pnlCommandBar.Visible = Value
            End Set
        End Property

        Public Property ShowSeparator() As Boolean
            Get
                Return trSeparator.Visible
            End Get
            Set(ByVal Value As Boolean)
                trSeparator.Visible = Value
            End Set
        End Property

#End Region

#Region " Event Handlers "

        Private Sub Page_Initialization(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Init

            Try

                ReadQueryString()

            Catch exc As Exception    'Module failed to load
                ProcessModuleLoadException(Me, exc)
            End Try

        End Sub

        Private Sub Page_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load

            Try

                AssignLocalization()
                AssignLinks()

            Catch exc As Exception    'Module failed to load
                ProcessModuleLoadException(Me, exc)
            End Try

        End Sub

        Private Sub Page_PreRender(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.PreRender

            Try

                BindBreadCrumbs()
                BindRSS()

            Catch exc As Exception    'Module failed to load
                ProcessModuleLoadException(Me, exc)
            End Try

        End Sub

#End Region

    End Class

End Namespace