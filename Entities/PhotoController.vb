'
' Simple Gallery for DotNetNuke -  http://www.dotnetnuke.com
' Copyright (c) 2002-2007
' by Ventrian ( sales@ventrian.com ) ( http://www.ventrian.com )
'

Imports System
Imports System.Data
Imports System.Xml

Imports DotNetNuke
Imports DotNetNuke.Common
Imports DotNetNuke.Common.Utilities
Imports DotNetNuke.Entities.Modules
Imports DotNetNuke.Services.Search

Imports Ventrian.SimpleGallery.Data

Namespace Ventrian.SimpleGallery.Entities

    Public Class PhotoController
        Implements ISearchable
        Implements IPortable

#Region " Public Methods "

        Public Function [Get](ByVal photoID As Integer) As PhotoInfo

            Return CType(CBO.FillObject(DataProvider.Instance().GetPhoto(photoID), GetType(PhotoInfo)), PhotoInfo)

        End Function

        Public Function GetFirstFromAlbum(ByVal albumID As Integer, ByVal moduleID As Integer) As PhotoInfo

            Return CType(CBO.FillObject(DataProvider.Instance().GetFirstFromAlbum(albumID, moduleID), GetType(PhotoInfo)), PhotoInfo)

        End Function

        Public Function GetRandomPhoto(ByVal moduleID As Integer, ByVal albumID As Integer, ByVal rowCount As Integer, ByVal tagID As Integer) As ArrayList

            Return CBO.FillCollection(DataProvider.Instance().GetRandomPhoto(moduleID, albumID, rowCount, tagID), GetType(PhotoInfo))

        End Function

        Public Function List(ByVal moduleID As Integer, ByVal albumID As Integer, ByVal isApproved As Boolean, ByVal maxCount As Integer, ByVal showAll As Boolean, ByVal tagID As Integer, ByVal batchID As String, ByVal search As String, ByVal sortBy As Common.SortType, ByVal sortDirection As Common.SortDirection) As ArrayList

            Dim sort As Integer = Null.NullInteger

            Select Case sortBy
                Case Common.SortType.Name
                    sort = 0
                    Exit Select

                Case Common.SortType.DateCreated
                    sort = 1
                    Exit Select

                Case Common.SortType.DateApproved
                    sort = 2
                    Exit Select

                Case Common.SortType.FileName
                    sort = 3
                    Exit Select

            End Select

            Dim sortOrder As Integer

            Select Case sortDirection

                Case Common.SortDirection.DESC
                    sortOrder = 0
                    Exit Select

                Case Common.SortDirection.ASC
                    sortOrder = 1
                    Exit Select

            End Select

            Return CBO.FillCollection(DataProvider.Instance().ListPhoto(moduleID, albumID, isApproved, maxCount, showAll, tagID, batchID, search, sort, sortOrder), GetType(PhotoInfo))

        End Function

        Public Function Add(ByVal objPhoto As PhotoInfo) As Integer

            'AlbumController.ClearCache()
            Return CType(DataProvider.Instance().AddPhoto(objPhoto.ModuleID, objPhoto.AlbumID, objPhoto.Name, objPhoto.Description, objPhoto.FileName, objPhoto.DateCreated, objPhoto.Width, objPhoto.Height, objPhoto.AuthorID, objPhoto.ApproverID, objPhoto.IsApproved, objPhoto.DateApproved, objPhoto.DateUpdated, objPhoto.BatchID), Integer)

        End Function

        Public Sub Update(ByVal objPhoto As PhotoInfo)

            'AlbumController.ClearCache()
            DataProvider.Instance().UpdatePhoto(objPhoto.PhotoID, objPhoto.ModuleID, objPhoto.AlbumID, objPhoto.Name, objPhoto.Description, objPhoto.FileName, objPhoto.DateCreated, objPhoto.Width, objPhoto.Height, objPhoto.AuthorID, objPhoto.ApproverID, objPhoto.IsApproved, objPhoto.DateApproved, objPhoto.DateUpdated, objPhoto.BatchID)

        End Sub

        Public Sub Delete(ByVal photoID As Integer)

            'AlbumController.ClearCache()
            DataProvider.Instance().DeletePhoto(photoID)

        End Sub

        Public Sub SetDefaultPhoto(ByVal photoID As Integer, ByVal albumID As Integer)

            'AlbumController.ClearCache()
            DataProvider.Instance().SetDefaultPhoto(photoID, albumID)

        End Sub

#End Region

#Region " Optional Interfaces "

        Public Function GetSearchItems(ByVal ModInfo As DotNetNuke.Entities.Modules.ModuleInfo) As DotNetNuke.Services.Search.SearchItemInfoCollection Implements DotNetNuke.Entities.Modules.ISearchable.GetSearchItems

            Dim SearchItemCollection As New SearchItemInfoCollection

            Dim objAlbumController As New AlbumController
            Dim objAlbums As ArrayList = objAlbumController.List(ModInfo.ModuleID, Null.NullInteger, True, True, Common.AlbumSortType.Caption, Common.SortDirection.ASC)

            For Each objAlbum As AlbumInfo In objAlbums
                If (objAlbum.IsPublic) Then
                    Dim objPhotoController As New PhotoController
                    Dim objPhotos As ArrayList = objPhotoController.List(ModInfo.ModuleID, objAlbum.AlbumID, True, Null.NullInteger, Null.NullBoolean, Null.NullInteger, Null.NullString, Null.NullString, Common.SortType.Name, Common.SortDirection.ASC)

                    For Each objPhoto As PhotoInfo In objPhotos

                        Dim strContent As String = System.Web.HttpUtility.HtmlDecode(objPhoto.Name & " " & objPhoto.Description)
                        Dim strDescription As String = HtmlUtils.Shorten(HtmlUtils.Clean(System.Web.HttpUtility.HtmlDecode(objPhoto.Description), False), 100, "...")

                        If (objPhoto.Description.Length = 0) Then
                            Dim SearchItem As New SearchItemInfo(objPhoto.Name, "A photo called " & objPhoto.Name, Null.NullInteger, objPhoto.DateUpdated, objPhoto.ModuleID, objPhoto.PhotoID.ToString(), strContent, "ctl=SlideShow&mid=" & ModInfo.ModuleID.ToString() & "&ItemID=" & objPhoto.PhotoID.ToString())
                            SearchItemCollection.Add(SearchItem)
                        Else
                            Dim SearchItem As New SearchItemInfo(objPhoto.Name, strDescription, Null.NullInteger, objPhoto.DateUpdated, objPhoto.ModuleID, objPhoto.PhotoID.ToString(), strContent, "ctl=SlideShow&mid=" & ModInfo.ModuleID.ToString() & "&ItemID=" & objPhoto.PhotoID.ToString())
                            SearchItemCollection.Add(SearchItem)
                        End If
                    Next
                End If
            Next

            Return SearchItemCollection

        End Function

        Public Function ExportModule(ByVal ModuleID As Integer) As String Implements IPortable.ExportModule

            Dim strXML As String = ""
            strXML = WriteAlbum(ModuleID, Null.NullInteger)
            Return strXML

        End Function

        Public Function WriteAlbum(ByVal ModuleID As Integer, ByVal parentID As Integer) As String

            Dim strXML As String = ""

            Dim objAlbumController As New AlbumController
            Dim objAlbums As ArrayList = objAlbumController.List(ModuleID, parentID, False, False, Common.AlbumSortType.Caption, Common.SortDirection.ASC)

            If objAlbums.Count <> 0 Then
                strXML += "<albums>"
                For Each objAlbum As AlbumInfo In objAlbums
                    strXML += "<album>"
                    strXML += "<caption>" & XmlUtils.XMLEncode(objAlbum.Caption) & "</caption>"
                    strXML += "<description>" & XmlUtils.XMLEncode(objAlbum.Description) & "</description>"
                    strXML += "<homeDirectory>" & XmlUtils.XMLEncode(objAlbum.HomeDirectory) & "</homeDirectory>"
                    strXML += "<isPublic>" & XmlUtils.XMLEncode(objAlbum.IsPublic.ToString) & "</isPublic>"
                    strXML += "<parentAlbumID>" & XmlUtils.XMLEncode(objAlbum.ParentAlbumID.ToString) & "</parentAlbumID>"
                    strXML += "<password>" & XmlUtils.XMLEncode(objAlbum.Password) & "</password>"
                    strXML += WriteAlbum(ModuleID, objAlbum.AlbumID)
                    strXML += WritePhotos(ModuleID, objAlbum.AlbumID)
                    strXML += "</album>"
                Next
                strXML += "</albums>"
            End If

            Return strXML

        End Function

        Public Function WritePhotos(ByVal ModuleID As Integer, ByVal parentID As Integer) As String

            Dim strXML As String = ""

            Dim objPhotoController As New PhotoController
            Dim objPhotos As ArrayList = objPhotoController.List(ModuleID, parentID, True, Null.NullInteger, True, Null.NullInteger, Null.NullString(), Null.NullString, Common.SortType.Name, Common.SortDirection.ASC)

            If objPhotos.Count <> 0 Then
                strXML += "<photos>"
                For Each objPhoto As PhotoInfo In objPhotos
                    strXML += "<photo>"
                    strXML += "<approverID>" & XmlUtils.XMLEncode(objPhoto.ApproverID.ToString) & "</approverID>"
                    strXML += "<authorID>" & XmlUtils.XMLEncode(objPhoto.AuthorID.ToString) & "</authorID>"
                    strXML += "<dateApproved>" & XmlUtils.XMLEncode(objPhoto.DateApproved.ToString) & "</dateApproved>"
                    strXML += "<dateCreated>" & XmlUtils.XMLEncode(objPhoto.DateCreated.ToString) & "</dateCreated>"
                    strXML += "<dateUpdated>" & XmlUtils.XMLEncode(objPhoto.DateUpdated.ToString) & "</dateUpdated>"
                    strXML += "<name>" & XmlUtils.XMLEncode(objPhoto.Name.ToString) & "</name>"
                    strXML += "<description>" & XmlUtils.XMLEncode(objPhoto.Description) & "</description>"
                    strXML += "<fileName>" & XmlUtils.XMLEncode(objPhoto.FileName) & "</fileName>"
                    strXML += "<height>" & XmlUtils.XMLEncode(objPhoto.Height.ToString) & "</height>"
                    strXML += "<width>" & XmlUtils.XMLEncode(objPhoto.Width.ToString) & "</width>"
                    strXML += "<isApproved>" & XmlUtils.XMLEncode(objPhoto.IsApproved.ToString) & "</isApproved>"
                    strXML += "</photo>"
                Next
                strXML += "</photos>"
            End If

            Return strXML

        End Function

        Public Sub ImportModule(ByVal ModuleID As Integer, ByVal Content As String, ByVal Version As String, ByVal UserId As Integer) Implements IPortable.ImportModule

            Dim xmlAlbums As XmlNode = GetContent(Content, "albums")
            For Each xmlAlbum As XmlNode In xmlAlbums
                ReadAlbum(ModuleID, xmlAlbum, Null.NullInteger)
            Next

        End Sub

        Public Sub ReadAlbum(ByVal ModuleID As Integer, ByVal xmlAlbum As XmlNode, ByVal parentAlbumID As Integer)

            Dim objAlbum As New AlbumInfo
            objAlbum.ModuleID = ModuleID
            objAlbum.Caption = xmlAlbum.Item("caption").InnerText
            objAlbum.Description = xmlAlbum.Item("description").InnerText
            objAlbum.HomeDirectory = xmlAlbum.Item("homeDirectory").InnerText
            objAlbum.IsPublic = Boolean.Parse(xmlAlbum.Item("isPublic").InnerText)
            objAlbum.ParentAlbumID = parentAlbumID
            objAlbum.Password = xmlAlbum.Item("password").InnerText

            Dim objAlbumController As New AlbumController
            objAlbum.AlbumID = objAlbumController.Add(objAlbum)

            For Each xmlChildNode As XmlNode In xmlAlbum.ChildNodes
                If (xmlChildNode.Name = "albums") Then
                    ReadAlbum(ModuleID, xmlChildNode.ChildNodes(0), objAlbum.AlbumID)
                End If
                If (xmlChildNode.Name = "photos") Then
                    ReadPhoto(ModuleID, xmlChildNode, objAlbum.AlbumID)
                End If
            Next

        End Sub

        Public Sub ReadPhoto(ByVal ModuleID As Integer, ByVal xmlPhotos As XmlNode, ByVal albumID As Integer)

            For Each xmlPhoto As XmlNode In xmlPhotos.ChildNodes
                Dim objPhoto As New PhotoInfo
                objPhoto.ModuleID = ModuleID
                objPhoto.AlbumID = albumID
                objPhoto.ApproverID = Integer.Parse(xmlPhoto.Item("approverID").InnerText)
                objPhoto.AuthorID = Integer.Parse(xmlPhoto.Item("authorID").InnerText)
                objPhoto.DateApproved = DateTime.Parse(xmlPhoto.Item("dateApproved").InnerText)
                objPhoto.DateCreated = DateTime.Parse(xmlPhoto.Item("dateCreated").InnerText)
                objPhoto.DateUpdated = DateTime.Parse(xmlPhoto.Item("dateUpdated").InnerText)
                objPhoto.Name = xmlPhoto.Item("name").InnerText
                objPhoto.Description = xmlPhoto.Item("description").InnerText
                objPhoto.FileName = xmlPhoto.Item("fileName").InnerText
                objPhoto.Height = Integer.Parse(xmlPhoto.Item("height").InnerText)
                objPhoto.Width = Integer.Parse(xmlPhoto.Item("width").InnerText)
                objPhoto.IsApproved = Boolean.Parse(xmlPhoto.Item("isApproved").InnerText)

                Add(objPhoto)
            Next

        End Sub

#End Region

    End Class

End Namespace
