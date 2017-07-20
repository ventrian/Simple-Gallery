'
' Simple Gallery for DotNetNuke -  http://www.dotnetnuke.com
' Copyright (c) 2002-2007
' by Ventrian ( sales@ventrian.com ) ( http://www.ventrian.com )
'

Imports System
Imports System.Data

Imports DotNetNuke.Common.Utilities
Imports DotNetNuke.Framework

Imports Ventrian.SimpleGallery.Data

Namespace Ventrian.SimpleGallery.Entities

    Public Class AlbumController

#Region " Public Methods "

        Public Function [Get](ByVal albumID As Integer) As AlbumInfo

            'Dim cacheKey As String = "SimpleGallery-Albums-" & albumID.ToString()

            'Dim objAlbum As AlbumInfo = CType(DataCache.GetCache(cacheKey), AlbumInfo)

            'If (objAlbum Is Nothing) Then
            Dim objAlbum As AlbumInfo = CType(CBO.FillObject(DataProvider.Instance().GetAlbum(albumID), GetType(AlbumInfo)), AlbumInfo)
            'If (objAlbum IsNot Nothing) Then
            'DataCache.SetCache(cacheKey, objAlbum)
            'End If
            'End If

            Return objAlbum

        End Function

        Public Function GetByPath(ByVal moduleID As Integer, ByVal homeDirectory As String) As AlbumInfo

            Return CType(CBO.FillObject(DataProvider.Instance().GetAlbumByPath(moduleID, homeDirectory), GetType(AlbumInfo)), AlbumInfo)

        End Function

        Public Function List(ByVal moduleID As Integer, ByVal parentAlbumID As Integer, ByVal ShowPublicOnly As Boolean, ByVal showChildren As Boolean, ByVal sortBy As Common.AlbumSortType, ByVal sortDirection As Common.SortDirection) As ArrayList

            Dim sort As Integer = Null.NullInteger

            Select Case sortBy
                Case Common.AlbumSortType.Caption
                    sort = 0
                    Exit Select

                Case Common.AlbumSortType.CreateDate
                    sort = 1
                    Exit Select

                Case Common.AlbumSortType.Custom
                    sort = 2
                    Exit Select

                Case Common.AlbumSortType.Random
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

            'Dim cacheKey As String = "SimpleGallery-Albums-" & moduleID.ToString() & "-" & parentAlbumID.ToString() & "-" & ShowPublicOnly.ToString() & "-" & showChildren.ToString() & "-" & sort.ToString() & "-" & sortOrder.ToString()

            'Dim objAlbums As ArrayList = CType(DataCache.GetCache(cacheKey), ArrayList)

            'If (objAlbums Is Nothing) Then
            Dim objAlbums As ArrayList = CBO.FillCollection(DataProvider.Instance().ListAlbumAll(moduleID, parentAlbumID, ShowPublicOnly, showChildren, sort, sortOrder), GetType(AlbumInfo))
            'DataCache.SetCache(cacheKey, objAlbums)
            'End If

            Return objAlbums

        End Function

        Public Function Add(ByVal objAlbum As AlbumInfo) As Integer

            If (objAlbum.CreateDate = Null.NullDate) Then
                objAlbum.CreateDate = DateTime.Now()
            End If

            Dim albumID As Integer = CType(DataProvider.Instance().AddAlbum(objAlbum.ModuleID, objAlbum.ParentAlbumID, objAlbum.Caption, objAlbum.Description, objAlbum.IsPublic, objAlbum.HomeDirectory, objAlbum.Password, objAlbum.AlbumOrder, objAlbum.CreateDate, objAlbum.InheritSecurity), Integer)

            'ClearCache()

            Return albumID

        End Function

        Public Sub Update(ByVal objAlbum As AlbumInfo)

            If (objAlbum.CreateDate = Null.NullDate) Then
                objAlbum.CreateDate = DateTime.Now()
            End If

            DataProvider.Instance().UpdateAlbum(objAlbum.AlbumID, objAlbum.ModuleID, objAlbum.ParentAlbumID, objAlbum.Caption, objAlbum.Description, objAlbum.IsPublic, objAlbum.HomeDirectory, objAlbum.Password, objAlbum.AlbumOrder, objAlbum.CreateDate, objAlbum.InheritSecurity)

            'ClearCache()

        End Sub

        Public Sub Delete(ByVal albumID As Integer)

            DataProvider.Instance().DeleteAlbum(albumID)
            'ClearCache()

        End Sub

        'Public Shared Sub ClearCache()

        '    Dim ide As IDictionaryEnumerator = System.Web.HttpContext.Current.Cache.GetEnumerator()

        '    ide.Reset()
        '    While ide.MoveNext()
        '        If DirectCast(ide.Key, String).StartsWith("SimpleGallery-Albums-") Then
        '            System.Web.HttpContext.Current.Cache.Remove(DirectCast(ide.Key, String))
        '        End If
        '    End While

        'End Sub

#End Region

    End Class

End Namespace
