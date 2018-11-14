'
' Simple Gallery for DotNetNuke -  http://www.dotnetnuke.com
' Copyright (c) 2002-2007
' by Ventrian ( sales@ventrian.com ) ( http://www.ventrian.com )
'

Imports System.Web
Imports System.IO
Imports System.Text
Imports System.Xml

Imports DotNetNuke.Common
Imports DotNetNuke.Common.Utilities
Imports DotNetNuke.Entities.Modules
Imports DotNetNuke.Entities.Portals
Imports DotNetNuke.Framework
Imports DotNetNuke.Services.Exceptions
Imports DotNetNuke.Services.Localization

Imports Ventrian.SimpleGallery.Common
Imports Ventrian.SimpleGallery.Entities

Namespace Ventrian.SimpleGallery

    Partial Public Class RSS
        Inherits System.Web.UI.Page

#Region " Private Members "

        Dim _tabID As Integer = Null.NullInteger
        Dim _moduleID As Integer = Null.NullInteger
        Dim _tabModuleID As Integer = Null.NullInteger
        Dim _albumID As Integer = Null.NullInteger
        Dim _maxCount As Integer = Null.NullInteger
        Dim _tagID As Integer = Null.NullInteger

#End Region

#Region " Private Methods "

        Private Sub ReadQueryString()

            If Not (Request("T") Is Nothing) Then
                _tabID = Convert.ToInt32(Request("T"))
            End If

            If Not (Request("M") Is Nothing) Then
                _moduleID = Convert.ToInt32(Request("M"))
            End If

            If Not (Request("TM") Is Nothing) Then
                _tabModuleID = Convert.ToInt32(Request("TM"))
            End If

            If Not (Request("A") Is Nothing) Then
                _albumID = Convert.ToInt32(Request("A"))
            End If

            If Not (Request("MaxCount") Is Nothing) Then
                _maxCount = Convert.ToInt32(Request("MaxCount"))
            End If

            If Not (Request("TagID") Is Nothing) Then
                _tagID = Convert.ToInt32(Request("TagID"))
            End If

        End Sub

#End Region

#Region " Event Handlers "

        Private Sub Page_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load

            Try

                ReadQueryString()

                Dim _portalSettings As PortalSettings = CType(HttpContext.Current.Items("PortalSettings"), PortalSettings)

                Response.ContentType = "text/xml"
                Response.ContentEncoding = Encoding.UTF8

                Dim sw As StringWriter = New StringWriter
                Dim writer As XmlTextWriter = New XmlTextWriter(sw)

                writer.WriteStartElement("rss")
                writer.WriteAttributeString("version", "2.0")
                writer.WriteAttributeString("xmlns:dc", "http://purl.org/dc/elements/1.1/")
                writer.WriteAttributeString("xmlns:cf", "http://www.microsoft.com/schemas/rss/core/2005")
                writer.WriteAttributeString("xmlns:simplegallery", "http://www.example.com/simplegallery")

                writer.WriteStartElement("channel")

                writer.WriteElementString("cf:treatAs", "list")

                Dim objModuleController As New ModuleController
                Dim objModule As ModuleInfo = objModuleController.GetModule(_moduleID, _tabID)

                Dim objAlbumController As New AlbumController
                Dim objAlbum As New AlbumInfo

                If (_albumID <> Null.NullInteger) Then
                    objAlbum = objAlbumController.Get(_albumID)
                End If

                If (objAlbum Is Nothing) Then
                    writer.WriteEndElement()
                    writer.WriteEndElement()
                    Response.Write(sw.ToString())
                    Response.End()
                End If

                Dim objTagController As New TagController
                Dim objTag As New TagInfo

                If (_tagID <> Null.NullInteger) Then
                    objTag = objTagController.Get(_tagID)
                End If

                Dim title As String = ""
                If Not (objModule Is Nothing) Then
                    If (_albumID <> Null.NullInteger) Then
                        title = objModule.ModuleTitle & ": Latest Photos for " & objAlbum.Caption
                    Else
                        If (_tagID <> Null.NullInteger) Then
                            title = objModule.ModuleTitle & ": Latest Photos for " & objTag.Name
                        Else
                            title = objModule.ModuleTitle & ": Latest Photos"
                        End If
                    End If
                End If


                Dim objPhotoController As New PhotoController
                Dim objPhotos As ArrayList

                If (_maxCount <> Null.NullInteger) Then

                    objPhotos = objPhotoController.List(_moduleID, _albumID, True, _maxCount, False, _tagID, Null.NullString(), Null.NullString, Common.SortType.DateApproved, Common.SortDirection.DESC)
                    PhotoInfo.SortDirection = Common.SortDirection.DESC.ToString()
                    PhotoInfo.SortBy = Common.SortType.DateApproved.ToString()

                Else

                    Dim objSettings As Hashtable = objModuleController.GetTabModuleSettings(_tabModuleID)

                    If (objSettings.Contains(Common.Constants.SETTING_SORT_DIRECTION)) Then
                        PhotoInfo.SortDirection = objSettings(Common.Constants.SETTING_SORT_DIRECTION).ToString()
                    Else
                        PhotoInfo.SortDirection = Common.SortDirection.DESC.ToString()
                    End If

                    If (objSettings.Contains(Common.Constants.SETTING_SORT_BY)) Then
                        PhotoInfo.SortBy = objSettings(Common.Constants.SETTING_SORT_BY).ToString()
                    Else
                        PhotoInfo.SortBy = Common.SortType.DateApproved.ToString()
                    End If

                    objPhotos = objPhotoController.List(_moduleID, _albumID, True, _maxCount, False, _tagID, Null.NullString(), Null.NullString, CType(System.Enum.Parse(GetType(Common.SortType), PhotoInfo.SortBy), Common.SortType), CType(System.Enum.Parse(GetType(Common.SortDirection), PhotoInfo.SortDirection), Common.SortDirection))

                End If

                writer.WriteStartElement("cf:listinfo")

                If (PhotoInfo.SortBy <> Common.SortType.DateApproved.ToString()) Then
                    writer.WriteStartElement("cf:sort")
                    writer.WriteAttributeString("element", "pubDate")
                    writer.WriteAttributeString("data-type", "date")
                    writer.WriteAttributeString("label", "Date")
                    writer.WriteEndElement()
                End If

                writer.WriteStartElement("cf:sort")
                Select Case PhotoInfo.SortBy.ToLower()

                    Case Common.SortType.DateApproved.ToString().ToLower()
                        writer.WriteAttributeString("label", "Date")
                        Exit Select

                    Case Common.SortType.DateCreated.ToString().ToLower()
                        writer.WriteAttributeString("label", "Date")
                        Exit Select

                    Case Common.SortType.FileName.ToString().ToLower()
                        writer.WriteAttributeString("label", "Filename")
                        Exit Select

                    Case Common.SortType.Name.ToString().ToLower()
                        writer.WriteAttributeString("label", "Title")
                        Exit Select

                End Select
                writer.WriteAttributeString("default", "true")
                writer.WriteEndElement()

                If (PhotoInfo.SortBy <> Common.SortType.Name.ToString()) Then
                    writer.WriteStartElement("cf:sort")
                    writer.WriteAttributeString("element", "title")
                    writer.WriteAttributeString("data-type", "text")
                    writer.WriteAttributeString("label", "Title")
                    writer.WriteEndElement()
                End If

                writer.WriteStartElement("cf:sort")
                writer.WriteAttributeString("element", "dc:creator")
                writer.WriteAttributeString("data-type", "text")
                writer.WriteAttributeString("label", "Author")
                writer.WriteEndElement()

                writer.WriteStartElement("cf:group")
                writer.WriteAttributeString("ns", "http://www.example.com/simplegallery")
                writer.WriteAttributeString("element", "album")
                writer.WriteAttributeString("label", "Album")
                writer.WriteEndElement()

                writer.WriteStartElement("cf:group")
                writer.WriteAttributeString("ns", "http://www.example.com/simplegallery")
                writer.WriteAttributeString("element", "tag")
                writer.WriteAttributeString("label", "Tag")
                writer.WriteEndElement()

                writer.WriteEndElement()

                Dim objPagedDataSource As New System.Web.UI.WebControls.PagedDataSource
                objPagedDataSource.DataSource = objPhotos

                If (_maxCount <> Null.NullInteger) Then
                    objPagedDataSource.AllowPaging = True
                    objPagedDataSource.PageSize = _maxCount
                End If

                writer.WriteElementString("title", title)

                Dim link As String = ""
                If (_albumID <> Null.NullInteger) Then
                    link = (NavigateURL(_tabID, Null.NullString, "AlbumID=" & _moduleID.ToString() & "-" & _albumID.ToString()))
                Else
                    If (_tagID <> Null.NullInteger) Then
                        link = (NavigateURL(_tabID, Null.NullString, "TagID=" & _tagID.ToString()))
                    Else
                        link = (NavigateURL(_tabID))
                    End If
                End If

                If Not (link.StartsWith("http://") Or link.StartsWith("https://")) Then
                    link = AddHTTP(System.Web.HttpContext.Current.Request.Url.Host & link)
                End If

                writer.WriteElementString("link", link)

                writer.WriteElementString("description", "")

                If (objPhotos.Count > 0) Then
                    Dim maxDate As DateTime = DateTime.MinValue
                    For Each objPhoto As PhotoInfo In objPagedDataSource
                        If objPhoto.DateApproved > maxDate Then
                            maxDate = objPhoto.DateApproved
                        End If
                    Next
                    writer.WriteElementString("pubDate", maxDate.ToUniversalTime.ToString("r"))
                    writer.WriteElementString("lastBuildDate", maxDate.ToUniversalTime.ToString("r"))
                Else
                    writer.WriteElementString("pubDate", DateTime.Now.ToUniversalTime.ToString("r"))
                    writer.WriteElementString("lastBuildDate", DateTime.Now.ToUniversalTime.ToString("r"))
                End If

                writer.WriteStartElement("image")
                writer.WriteElementString("url", "http://" & System.Web.HttpContext.Current.Request.Url.Host & Me.ResolveUrl("Images\IconRSS.gif"))
                writer.WriteElementString("title", title)
                writer.WriteElementString("link", link)
                writer.WriteEndElement()

                For Each objPhoto As PhotoInfo In objPagedDataSource

                    writer.WriteStartElement("item")

                    writer.WriteStartElement("title")
                    writer.WriteString(objPhoto.Name)
                    writer.WriteEndElement()

                    Dim photoLink As String = ""
                    If (_tagID <> Null.NullInteger) Then
                        photoLink = NavigateURL(_tabID, "", "galleryType=SlideShow", "ItemID=" & objPhoto.PhotoID, "TagID=" & _tagID.ToString())
                    Else
                        photoLink = NavigateURL(_tabID, "", "galleryType=SlideShow", "ItemID=" & objPhoto.PhotoID.ToString(), "AlbumID=" & objPhoto.AlbumID.ToString())
                    End If

                    If Not (photoLink.StartsWith("http://") Or photoLink.StartsWith("https://")) Then
                        photoLink = AddHTTP(System.Web.HttpContext.Current.Request.Url.Host & photoLink)
                    End If
                    writer.WriteElementString("link", photoLink)

                    writer.WriteElementString("dc:creator", objPhoto.AuthorDisplayName)

                    writer.WriteStartElement("guid")
                    writer.WriteAttributeString("isPermaLink", "false")
                    writer.WriteString(objPhoto.PhotoID.ToString())
                    writer.WriteEndElement()

                    Dim tagsText As String = ""
                    If (objPhoto.Tags <> "") Then
                        Dim tags As String() = objPhoto.Tags.Split(","c)
                        tagsText = "<br /><b>Tags:</b>"
                        For Each tag As String In tags
                            tagsText = tagsText & " <a href='" & NavigateURL(_tabID, "", "Tag=" & tag, "Tags=" & _tabModuleID) & "'>" & tag & "</a>"
                        Next
                    End If

                    Dim portalHomeDirectory As String = _portalSettings.HomeDirectory & "/"
                    If (objPhoto.Description <> "") Then
                        writer.WriteElementString("description", objPhoto.Description & "<br /><a href=""" & photoLink & """><img src=""http://" & System.Web.HttpContext.Current.Request.Url.Host & Me.ResolveUrl("ImageHandler.ashx?width=" & GetPhotoWidth(CType(objPhoto, Object)) & "&height=" & GetPhotoHeight(CType(objPhoto, Object)) & "&HomeDirectory=" & System.Uri.EscapeDataString(portalHomeDirectory & objPhoto.HomeDirectory) & "&fileName=" & System.Uri.EscapeDataString(objPhoto.FileName) & "&portalid=" & objModule.PortalID.ToString) & """ /></a>" & tagsText)
                    Else
                        writer.WriteElementString("description", "<a href=""" & photoLink & """><img src=""http://" & System.Web.HttpContext.Current.Request.Url.Host & Me.ResolveUrl("ImageHandler.ashx?width=" & GetPhotoWidth(CType(objPhoto, Object)) & "&height=" & GetPhotoHeight(CType(objPhoto, Object)) & "&HomeDirectory=" & System.Uri.EscapeDataString(portalHomeDirectory & objPhoto.HomeDirectory) & "&fileName=" & System.Uri.EscapeDataString(objPhoto.FileName) & "&portalid=" & objModule.PortalID.ToString) & """ width=""" & GetPhotoWidth(CType(objPhoto, Object)) & """ height=""" & GetPhotoHeight(CType(objPhoto, Object)) & """ /></a>" & tagsText)
                    End If
                    writer.WriteElementString("pubDate", objPhoto.DateApproved.ToUniversalTime.ToString("r"))
                    writer.WriteElementString("simplegallery:album", objPhoto.AlbumName)

                    For Each tag As String In objPhoto.Tags.Split(","c)
                        If (tag <> "") Then
                            writer.WriteElementString("simplegallery:tag", tag)
                        End If
                    Next

                    writer.WriteStartElement("enclosure")
                    writer.WriteAttributeString("url", "http://" & System.Web.HttpContext.Current.Request.Url.Host & PortalSettings.Current.HomeDirectory & objPhoto.HomeDirectory & "/" & objPhoto.FileName)
                    writer.WriteAttributeString("type", "image/jpg")
                    writer.WriteEndElement()

                    writer.WriteEndElement()

                Next

                writer.WriteEndElement()
                writer.WriteEndElement()

                Response.Write(sw.ToString)

            Catch exc As Exception    'Module failed to load
                Response.End()
            End Try

        End Sub

        Protected Function GetPhotoWidth(ByVal dataItem As Object) As String

            Dim objPhoto As PhotoInfo = CType(dataItem, PhotoInfo)

            If Not (objPhoto Is Nothing) Then
                Dim width As Integer
                If (objPhoto.Width > 240) Then
                    width = 240
                Else
                    width = objPhoto.Width
                End If

                Dim height As Integer = Convert.ToInt32(objPhoto.Height / (objPhoto.Width / width))
                If (height > 180) Then
                    height = 180
                    width = Convert.ToInt32(objPhoto.Width / (objPhoto.Height / height))
                End If

                Return width.ToString()
            Else
                Return 240.ToString()
            End If

        End Function

        Protected Function GetPhotoHeight(ByVal dataItem As Object) As String

            Dim objPhoto As PhotoInfo = CType(dataItem, PhotoInfo)

            If Not (objPhoto Is Nothing) Then
                Dim width As Integer
                If (objPhoto.Width > 240) Then
                    width = 240
                Else
                    width = objPhoto.Width
                End If

                Dim height As Integer = Convert.ToInt32(objPhoto.Height / (objPhoto.Width / width))
                If (height > 180) Then
                    height = 180
                    width = Convert.ToInt32(objPhoto.Width / (objPhoto.Height / height))
                End If

                Return height.ToString()
            Else
                Return 180.ToString()
            End If

        End Function

#End Region

    End Class

End Namespace