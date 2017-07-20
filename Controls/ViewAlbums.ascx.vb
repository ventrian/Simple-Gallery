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

Namespace Ventrian.SimpleGallery.Controls

    Partial Public Class ViewAlbums
        Inherits SimpleGalleryControlBase

#Region " Private Members "

        Private _albumID As Integer = Null.NullInteger
        Private _albumTemplate As String = ""
        Private _albumTemplateTokens As String()

#End Region

#Region " Private Properties "

        Private Shadows ReadOnly Property LocalResourceFile() As String
            Get
                Return "~/DesktopModules/SimpleGallery/App_LocalResources/ViewAlbums.ascx.resx"
            End Get
        End Property

#End Region

#Region " Private Methods "

        Private Sub BindAlbums()

            InitializeAlbumTemplate()

            dlAlbum.RepeatColumns = SimpleGalleryBase.GallerySettings.AlbumsPerRow

            Dim objAlbumController As New AlbumController
            dlAlbum.DataSource = objAlbumController.List(SimpleGalleryBase.ModuleId, _albumID, Not SimpleGalleryBase.HasEditPermissions(), False, SimpleGalleryBase.GallerySettings.AlbumSortBy, SimpleGalleryBase.GallerySettings.AlbumSortDirection)
            dlAlbum.DataBind()

        End Sub

        Protected Function GetAlbumCount(ByVal dataItem As Object) As String

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

        Protected Function GetAlbumPath(ByVal albumID As String, ByVal homeDirectory As String) As Hashtable

            Dim objSettings As New Hashtable

            Dim objPhotoController As New PhotoController
            Dim objPhoto As PhotoInfo
            objPhoto = objPhotoController.GetFirstFromAlbum(Convert.ToInt32(albumID), SimpleGalleryBase.ModuleId)

            If Not (objPhoto Is Nothing) Then
                Dim width As Integer
                If (objPhoto.Width > SimpleGalleryBase.GallerySettings.AlbumThumbnailWidth) Then
                    width = SimpleGalleryBase.GallerySettings.AlbumThumbnailWidth
                Else
                    width = objPhoto.Width
                End If

                Dim height As Integer = Convert.ToInt32(objPhoto.Height / (objPhoto.Width / width))
                If (height > SimpleGalleryBase.GallerySettings.AlbumThumbnailHeight) Then
                    height = SimpleGalleryBase.GallerySettings.AlbumThumbnailHeight
                    width = Convert.ToInt32(objPhoto.Width / (objPhoto.Height / height))
                End If

                If (SimpleGalleryBase.GallerySettings.ThumbnailAlbum = ThumbnailType.Square) Then
                    width = SimpleGalleryBase.GallerySettings.AlbumThumbnailSquare
                    height = SimpleGalleryBase.GallerySettings.AlbumThumbnailSquare
                End If

                objSettings.Add("AlbumWidth", width.ToString())
                objSettings.Add("AlbumHeight", height.ToString())

                Dim square As String = ""
                If (SimpleGalleryBase.GallerySettings.ThumbnailAlbum = ThumbnailType.Square) Then
                    square = "&s=1"
                End If

                If (objPhoto.HomeDirectory = "") Then
                    If (SimpleGalleryBase.GallerySettings.CompressionAlbum = CompressionType.MinSize) Then
                        objSettings.Add("AlbumPath", Me.ResolveUrl("../ImageHandler.ashx?width=" & width.ToString() & "&height=" & height.ToString() & "&HomeDirectory=" & System.Uri.EscapeDataString(SimpleGalleryBase.PortalSettings.HomeDirectory & homeDirectory) & "&fileName=" & System.Uri.EscapeDataString(objPhoto.FileName) & "&portalid=" & SimpleGalleryBase.PortalId.ToString() & square))
                    Else
                        objSettings.Add("AlbumPath", Me.ResolveUrl("../ImageHandler.ashx?width=" & width.ToString() & "&height=" & height.ToString() & "&HomeDirectory=" & System.Uri.EscapeDataString(SimpleGalleryBase.PortalSettings.HomeDirectory & homeDirectory) & "&fileName=" & System.Uri.EscapeDataString(objPhoto.FileName) & "&portalid=" & SimpleGalleryBase.PortalId.ToString() & "&q=1" & square))
                    End If
                Else
                    If (SimpleGalleryBase.GallerySettings.CompressionAlbum = CompressionType.MinSize) Then
                        objSettings.Add("AlbumPath", Me.ResolveUrl("../ImageHandler.ashx?width=" & width.ToString() & "&height=" & height.ToString() & "&HomeDirectory=" & System.Uri.EscapeDataString(SimpleGalleryBase.PortalSettings.HomeDirectory & objPhoto.HomeDirectory) & "&fileName=" & System.Uri.EscapeDataString(objPhoto.FileName) & "&portalid=" & SimpleGalleryBase.PortalId.ToString() & square))
                    Else
                        objSettings.Add("AlbumPath", Me.ResolveUrl("../ImageHandler.ashx?width=" & width.ToString() & "&height=" & height.ToString() & "&HomeDirectory=" & System.Uri.EscapeDataString(SimpleGalleryBase.PortalSettings.HomeDirectory & objPhoto.HomeDirectory) & "&fileName=" & System.Uri.EscapeDataString(objPhoto.FileName) & "&portalid=" & SimpleGalleryBase.PortalId.ToString() & "&q=1" & square))
                    End If
                End If
            Else
                Dim width As Integer
                If (600 > SimpleGalleryBase.GallerySettings.AlbumThumbnailWidth) Then
                    width = SimpleGalleryBase.GallerySettings.AlbumThumbnailWidth
                Else
                    width = 600
                End If

                Dim height As Integer = Convert.ToInt32(450 / (600 / width))
                If (height > SimpleGalleryBase.GallerySettings.AlbumThumbnailHeight) Then
                    height = SimpleGalleryBase.GallerySettings.AlbumThumbnailHeight
                    width = Convert.ToInt32(600 / (450 / height))
                End If

                If (SimpleGalleryBase.GallerySettings.ThumbnailAlbum = ThumbnailType.Square) Then
                    width = SimpleGalleryBase.GallerySettings.AlbumThumbnailSquare
                    height = SimpleGalleryBase.GallerySettings.AlbumThumbnailSquare
                End If

                objSettings.Add("AlbumWidth", width.ToString())
                objSettings.Add("AlbumHeight", height.ToString())

                If (SimpleGalleryBase.GallerySettings.CompressionAlbum = CompressionType.MinSize) Then
                    If (SimpleGalleryBase.GallerySettings.ThumbnailAlbum = ThumbnailType.Square) Then
                        objSettings.Add("AlbumPath", Me.ResolveUrl("../ImageHandler.ashx?width=" & width.ToString() & "&height=" & height.ToString() & "&fileName=" & System.Uri.EscapeDataString("placeholder-600.jpg") & "&portalid=" & SimpleGalleryBase.PortalId.ToString()) & "&s=1")
                    Else
                        objSettings.Add("AlbumPath", Me.ResolveUrl("../ImageHandler.ashx?width=" & width.ToString() & "&height=" & height.ToString() & "&fileName=" & System.Uri.EscapeDataString("placeholder-600.jpg") & "&portalid=" & SimpleGalleryBase.PortalId.ToString()))
                    End If
                Else

                    If (SimpleGalleryBase.GallerySettings.ThumbnailAlbum = ThumbnailType.Square) Then
                        objSettings.Add("AlbumPath", Me.ResolveUrl("../ImageHandler.ashx?width=" & width.ToString() & "&height=" & height.ToString() & "&fileName=" & System.Uri.EscapeDataString("placeholder-600.jpg") & "&portalid=" & SimpleGalleryBase.PortalId.ToString()) & "&q=1" & "&s=1")
                    Else
                        objSettings.Add("AlbumPath", Me.ResolveUrl("../ImageHandler.ashx?width=" & width.ToString() & "&height=" & height.ToString() & "&fileName=" & System.Uri.EscapeDataString("placeholder-600.jpg") & "&portalid=" & SimpleGalleryBase.PortalId.ToString()) & "&q=1")
                    End If
                End If
            End If

            Return objSettings

        End Function

        Protected Function GetAlbumUrl(ByVal albumID As String, ByVal moduleID As Integer) As String

            If (SimpleGalleryBase.GallerySettings.UseAlbumAnchors) Then
                Return NavigateURL(SimpleGalleryBase.TabId, "", "AlbumID=" & SimpleGalleryBase.ModuleId.ToString() & "-" & albumID) & "#SimpleGallery-" & moduleID.ToString()
            Else
                Return NavigateURL(SimpleGalleryBase.TabId, "", "AlbumID=" & SimpleGalleryBase.ModuleId.ToString() & "-" & albumID)
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

        Private Sub InitializeAlbumTemplate()

            Dim cacheKey As String = SimpleGalleryBase.TabModuleId.ToString() & TemplateType.AlbumInfo.ToString()

            Dim objTemplate As TemplateInfo = CType(DataCache.GetCache(cacheKey), TemplateInfo)

            If (objTemplate Is Nothing) Then
                Dim objTemplateController As New TemplateController
                objTemplate = objTemplateController.Get(SimpleGalleryBase.ModuleId, TemplateType.AlbumInfo.ToString())

                If (objTemplate Is Nothing) Then
                    objTemplate = New TemplateInfo
                    objTemplate.Template = Constants.DEFAULT_TEMPLATE_ALBUM_INFO
                End If

                DataCache.SetCache(cacheKey, objTemplate)
            End If

            _albumTemplate = objTemplate.Template
            _albumTemplateTokens = objTemplate.Tokens

        End Sub

        Private Function IsAlbumEditable() As Boolean

            Return SimpleGalleryBase.HasEditPermissions

        End Function

        Protected Function RssUrl(ByVal albumID As String) As String

            Return Me.ResolveUrl("../RSS.aspx?t=" & SimpleGalleryBase.TabId & "&m=" & SimpleGalleryBase.ModuleId & "&tm=" & SimpleGalleryBase.TabModuleId & "&a=" & albumID & "&portalid=" & SimpleGalleryBase.PortalId)

        End Function

#End Region

#Region " Protected Properties "

#End Region

#Region " Public Methods "

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

#End Region

#Region " Event Handlers "

        Private Sub Page_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load

            Try

            Catch exc As Exception    'Module failed to load
                ProcessModuleLoadException(Me, exc)
            End Try

        End Sub

        Private Sub Page_PreRender(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.PreRender

            Try

                BindAlbums()

            Catch exc As Exception    'Module failed to load
                ProcessModuleLoadException(Me, exc)
            End Try

        End Sub

        Private Sub dlAlbum_OnItemDataBound(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.DataListItemEventArgs) Handles dlAlbum.ItemDataBound

            If (e.Item.ItemType = ListItemType.Item Or e.Item.ItemType = ListItemType.AlternatingItem) Then

                Dim phAlbum As PlaceHolder = CType(e.Item.FindControl("phAlbum"), PlaceHolder)
                Dim objAlbum As AlbumInfo = CType(e.Item.DataItem, AlbumInfo)

                If Not (phAlbum Is Nothing) Then

                    For iPtr As Integer = 0 To _albumTemplateTokens.Length - 1 Step 2

                        phAlbum.Controls.Add(New LiteralControl(_albumTemplateTokens(iPtr).ToString()))

                        If iPtr < _albumTemplateTokens.Length - 1 Then
                            Select Case _albumTemplateTokens(iPtr + 1)
                                Case "ALBUM"
                                    Dim objSettings As Hashtable = GetAlbumPath(objAlbum.AlbumID.ToString(), objAlbum.HomeDirectory)

                                    Dim objLiteral As New Literal
                                    objLiteral.ID = Globals.CreateValidID("Album" & objAlbum.AlbumID.ToString() & "-" & iPtr.ToString())
                                    objLiteral.Text = "<img src=""" & objSettings("AlbumPath").ToString() & """ class=""photo_198"" alt=""" & GetAlternateTextForAlbum(objAlbum) & """ width=""" & objSettings("AlbumWidth").ToString() & """ height=""" & objSettings("AlbumHeight").ToString() & """>"
                                    phAlbum.Controls.Add(objLiteral)
                                Case "ALBUMCOUNT"
                                    Dim objLiteral As New Literal
                                    objLiteral.ID = Globals.CreateValidID("Album" & objAlbum.AlbumID.ToString() & "-" & iPtr.ToString())
                                    objLiteral.Text = GetAlbumCount(objAlbum)
                                    phAlbum.Controls.Add(objLiteral)
                                Case "ALBUMLINK"
                                    Dim objLiteral As New Literal
                                    objLiteral.ID = Globals.CreateValidID("Album" & objAlbum.AlbumID.ToString() & "-" & iPtr.ToString())
                                    objLiteral.Text = GetAlbumUrl(objAlbum.AlbumID.ToString(), objAlbum.ModuleID)
                                    phAlbum.Controls.Add(objLiteral)
                                Case "ALBUMWITHBORDER"
                                    Dim objSettings As Hashtable = GetAlbumPath(objAlbum.AlbumID.ToString(), objAlbum.HomeDirectory)
                                    Dim objLiteral As New Literal
                                    objLiteral.ID = Globals.CreateValidID("Album" & objAlbum.AlbumID.ToString() & "-" & iPtr.ToString())
                                    If (SimpleGalleryBase.GallerySettings.BorderStyle = "White") Then
                                        objLiteral.Text = "" _
                                            & "<table border=""0"" cellpadding=""0"" cellspacing=""0"" class=""album-frame"">" _
                                            & "<tr><td class=""topx----""><img src=""" & SimpleGalleryBase.FormatBorderPath("album-l1.gif") & """ border=""0"" width=""14"" height=""14""></td><td class=""top-x---""><img src=""" & SimpleGalleryBase.FormatBorderPath("album-mtl.png") & """ border=""0"" height=""14""></td><td class=""top--x--""></td><td class=""top---x-""><img src=""" & SimpleGalleryBase.FormatBorderPath("album-mtr.png") & """ border=""0"" height=""14""></td><td class=""top----""><img src=""" & SimpleGalleryBase.FormatBorderPath("album-r1.gif") & """ border=""0"" width=""14"" height=""14""></td></tr>" _
                                            & "<tr><td class=""mtpx----""><img src=""" & SimpleGalleryBase.FormatBorderPath("album-l2.png") & """ border=""0"" width=""14""></td><td colspan=""3"" rowspan=""3"" align=""center""><a href=""" & GetAlbumUrl(objAlbum.AlbumID.ToString(), objAlbum.ModuleID) & """><img src=""" & objSettings("AlbumPath").ToString() & """ class=""photo_198"" alt=""" & GetAlternateTextForAlbum(objAlbum) & """ width=""" & objSettings("AlbumWidth").ToString() & """ height=""" & objSettings("AlbumHeight").ToString() & """ /></a></td><td class=""mtp----x""><img src=""" & SimpleGalleryBase.FormatBorderPath("album-r2.png") & """ border=""0"" width=""14""></td></tr>" _
                                            & "<tr><td class=""midx----""></td><td class=""mid----x""></td></tr>" _
                                            & "<tr><td class=""mbtx----""><img src=""" & SimpleGalleryBase.FormatBorderPath("album-l3.png") & """ border=""0"" width=""14""></td><td class=""mbt----x""><img src=""" & SimpleGalleryBase.FormatBorderPath("album-r3.png") & """ border=""0"" width=""14""></td></tr>" _
                                            & "<tr><td class=""botx----""><img src=""" & SimpleGalleryBase.FormatBorderPath("album-l4.gif") & """ border=""0"" width=""14"" height=""14""></td><td class=""bot-x---""><img src=""" & SimpleGalleryBase.FormatBorderPath("album-mbl.png") & """ border=""0"" height=""14""></td><td class=""bot--x--""></td><td class=""bot---x-""><img src=""" & SimpleGalleryBase.FormatBorderPath("album-mbr.png") & """ border=""0"" height=""14""></td><td class=""bot----x""><img src=""" & SimpleGalleryBase.FormatBorderPath("album-r4.gif") & """ border=""0"" width=""14"" height=""14""></td></tr>" _
                                            & "</table>"
                                    Else
                                        objLiteral.Text = "" _
                                            & "<table border=""0"" cellpadding=""0"" cellspacing=""0"" class=""album-frame"">" _
                                            & "<tr><td class=""topx----""><img src=""" & SimpleGalleryBase.FormatBorderPath("album-l1.gif") & """ border=""0"" width=""14"" height=""14""></td><td class=""top-x---""><img src=""" & SimpleGalleryBase.FormatBorderPath("album-mtl.gif") & """ border=""0"" height=""14""></td><td class=""top--x--""></td><td class=""top---x-""><img src=""" & SimpleGalleryBase.FormatBorderPath("album-mtr.gif") & """ border=""0"" height=""14""></td><td class=""top----""><img src=""" & SimpleGalleryBase.FormatBorderPath("album-r1.gif") & """ border=""0"" width=""14"" height=""14""></td></tr>" _
                                            & "<tr><td class=""mtpx----""><img src=""" & SimpleGalleryBase.FormatBorderPath("album-l2.gif") & """ border=""0"" width=""14""></td><td colspan=""3"" rowspan=""3"" align=""center""><a href=""" & GetAlbumUrl(objAlbum.AlbumID.ToString(), objAlbum.ModuleID) & """><img src=""" & objSettings("AlbumPath").ToString() & """ class=""photo_198"" alt=""" & GetAlternateTextForAlbum(objAlbum) & """ width=""" & objSettings("AlbumWidth").ToString() & """ height=""" & objSettings("AlbumHeight").ToString() & """ /></a></td><td class=""mtp----x""><img src=""" & SimpleGalleryBase.FormatBorderPath("album-r2.gif") & """ border=""0"" width=""14""></td></tr>" _
                                            & "<tr><td class=""midx----""></td><td class=""mid----x""></td></tr>" _
                                            & "<tr><td class=""mbtx----""><img src=""" & SimpleGalleryBase.FormatBorderPath("album-l3.gif") & """ border=""0"" width=""14""></td><td class=""mbt----x""><img src=""" & SimpleGalleryBase.FormatBorderPath("album-r3.gif") & """ border=""0"" width=""14""></td></tr>" _
                                            & "<tr><td class=""botx----""><img src=""" & SimpleGalleryBase.FormatBorderPath("album-l4.gif") & """ border=""0"" width=""14"" height=""14""></td><td class=""bot-x---""><img src=""" & SimpleGalleryBase.FormatBorderPath("album-mbl.gif") & """ border=""0"" height=""14""></td><td class=""bot--x--""></td><td class=""bot---x-""><img src=""" & SimpleGalleryBase.FormatBorderPath("album-mbr.gif") & """ border=""0"" height=""14""></td><td class=""bot----x""><img src=""" & SimpleGalleryBase.FormatBorderPath("album-r4.gif") & """ border=""0"" width=""14"" height=""14""></td></tr>" _
                                            & "</table>"
                                    End If
                                    phAlbum.Controls.Add(objLiteral)
                                Case "DATECREATED"
                                    Dim objLiteral As New Literal
                                    objLiteral.ID = Globals.CreateValidID("Album" & objAlbum.AlbumID.ToString() & "-" & iPtr.ToString())
                                    objLiteral.Text = objAlbum.CreateDate.ToShortDateString()
                                    phAlbum.Controls.Add(objLiteral)
                                Case "DESCRIPTION"
                                    Dim objLiteral As New Literal
                                    objLiteral.ID = Globals.CreateValidID("Album" & objAlbum.AlbumID.ToString() & "-" & iPtr.ToString())
                                    objLiteral.Text = objAlbum.Description.ToString()
                                    phAlbum.Controls.Add(objLiteral)
                                Case "EDIT"
                                    If (SimpleGalleryBase.IsEditable) Then
                                        If (IsAlbumEditable()) Then
                                            Dim objLiteral As New Literal
                                            objLiteral.ID = Globals.CreateValidID("Album" & objAlbum.AlbumID.ToString() & "-" & iPtr.ToString())
                                            objLiteral.Text = "<a href=""" & SimpleGalleryBase.EditUrl("AlbumID", objAlbum.AlbumID.ToString(), "EditAlbum", "ReturnUrl=" & SimpleGalleryBase.ReturnUrl) & """><img src=""" & Me.ResolveUrl("~/images/edit.gif") & """ alt=""" & Localization.GetString("Edit", Me.LocalResourceFile) & """ border=""0"" /></a>"
                                            phAlbum.Controls.Add(objLiteral)
                                        End If
                                    End If
                                Case "PHOTOCOUNT"
                                    Dim objLiteral As New Literal
                                    objLiteral.ID = Globals.CreateValidID("Album" & objAlbum.AlbumID.ToString() & "-" & iPtr.ToString())
                                    objLiteral.Text = GetPhotoCount(objAlbum)
                                    phAlbum.Controls.Add(objLiteral)
                                Case "RSS"
                                    If (SimpleGalleryBase.GallerySettings.EnableSyndication) Then
                                        Dim rssText As String = Localization.GetString("RssText", Me.LocalResourceFile)
                                        If (rssText.IndexOf("{0}") <> -1) Then
                                            rssText = String.Format(rssText, objAlbum.Caption)
                                        Else
                                            rssText = String.Format(rssText, objAlbum.Caption)
                                        End If
                                        Me.SimpleGalleryBase.AddRSSFeed(RssUrl(objAlbum.AlbumID.ToString()), rssText)
                                        Dim objLiteral As New Literal
                                        objLiteral.ID = Globals.CreateValidID("Album" & objAlbum.AlbumID.ToString() & "-" & iPtr.ToString())
                                        objLiteral.Text = "<a href=""" & RssUrl(objAlbum.AlbumID.ToString()) & """><img src=""" & Me.ResolveUrl("~/DesktopModules/SimpleGallery/images/xml_small.gif") & """ alt=""" & Localization.GetString("Syndicate", Me.LocalResourceFile) & """ align=""absmiddle"" border=""0""></a>"
                                        phAlbum.Controls.Add(objLiteral)
                                    End If
                                Case "RSSLINK"
                                    Dim rssText As String = Localization.GetString("RssText", Me.LocalResourceFile)
                                    If (rssText.IndexOf("{0}") <> -1) Then
                                        rssText = String.Format(rssText, objAlbum.Caption)
                                    Else
                                        rssText = String.Format(rssText, objAlbum.Caption)
                                    End If
                                    Me.SimpleGalleryBase.AddRSSFeed(RssUrl(objAlbum.AlbumID.ToString()), rssText)
                                    Dim objLiteral As New Literal
                                    objLiteral.ID = Globals.CreateValidID("Album" & objAlbum.AlbumID.ToString() & "-" & iPtr.ToString())
                                    objLiteral.Text = RssUrl(objAlbum.AlbumID.ToString())
                                    phAlbum.Controls.Add(objLiteral)
                                Case "TITLE"
                                    Dim objLiteral As New Literal
                                    objLiteral.ID = Globals.CreateValidID("Album" & objAlbum.AlbumID.ToString() & "-" & iPtr.ToString())
                                    objLiteral.Text = objAlbum.Caption.ToString()
                                    phAlbum.Controls.Add(objLiteral)
                                Case "ZIP"
                                    If (SimpleGalleryBase.GallerySettings.ZipEnabled) Then
                                        Dim url As String = NavigateURL(SimpleGalleryBase.TabId, "", "galleryType=ZipAlbum", "AlbumID=" & objAlbum.AlbumID.ToString())
                                        Dim objLiteral As New Literal
                                        objLiteral.ID = Globals.CreateValidID("Album" & objAlbum.AlbumID.ToString() & "-" & iPtr.ToString())
                                        objLiteral.Text = "<a href=""" & url & """ rel=""nofollow""><img src=""" & Me.ResolveUrl("~/Images/Save.gif") & """ alt=""" & Localization.GetString("DownloadZip", Me.LocalResourceFile) & """ align=""absmiddle"" border=""0""></a>"
                                        phAlbum.Controls.Add(objLiteral)
                                    End If
                                Case "ZIPLINK"
                                    Dim url As String = NavigateURL(SimpleGalleryBase.TabId, "", "galleryType=ZipAlbum", "AlbumID=" & objAlbum.AlbumID.ToString())
                                    Dim objLiteral As New Literal
                                    objLiteral.ID = Globals.CreateValidID("Album" & objAlbum.AlbumID.ToString() & "-" & iPtr.ToString())
                                    objLiteral.Text = url
                                    phAlbum.Controls.Add(objLiteral)
                                Case Else
                                    If (_albumTemplateTokens(iPtr + 1).ToUpper().StartsWith("DATECREATED:")) Then
                                        Dim formatExpression As String = _albumTemplateTokens(iPtr + 1).Substring(12, _albumTemplateTokens(iPtr + 1).Length - 12)
                                        If (objAlbum.CreateDate <> Null.NullDate) Then
                                            Dim objLiteral As New Literal
                                            objLiteral.ID = Globals.CreateValidID("Album" & objAlbum.AlbumID.ToString() & "-" & iPtr.ToString())
                                            objLiteral.Text = objAlbum.CreateDate.ToString(formatExpression)
                                            objLiteral.EnableViewState = False
                                            phAlbum.Controls.Add(objLiteral)
                                        End If
                                        Exit Select
                                    End If

                                    Dim objLiteralOther As New Literal
                                    objLiteralOther.Text = "[" & _albumTemplateTokens(iPtr + 1) & "]"
                                    objLiteralOther.EnableViewState = False
                                    phAlbum.Controls.Add(objLiteralOther)
                            End Select

                        End If
                    Next

                End If

            End If

        End Sub

#End Region

    End Class

End Namespace