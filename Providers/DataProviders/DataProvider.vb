'
' Simple Gallery for DotNetNuke -  http://www.dotnetnuke.com
' Copyright (c) 2002-2007
' by Ventrian ( sales@ventrian.com ) ( http://www.ventrian.com )
'

Imports System
Imports System.Web.Caching
Imports System.Reflection

Imports DotNetNuke
Imports DotNetNuke.Common.Utilities

Namespace Ventrian.SimpleGallery.Data

    Public MustInherit Class DataProvider

#Region " Shared/Static Methods "

        ' singleton reference to the instantiated object 
        Private Shared objProvider As DataProvider = Nothing

        ' constructor
        Shared Sub New()
            CreateProvider()
        End Sub

        ' dynamically create provider
        Private Shared Sub CreateProvider()
            objProvider = CType(Framework.Reflection.CreateObject("data", "Ventrian.SimpleGallery", "Ventrian.SimpleGallery"), DataProvider)
        End Sub

        ' return the provider
        Public Shared Shadows Function Instance() As DataProvider
            Return objProvider
        End Function

#End Region

#Region " Abstract Methods "

        Public MustOverride Function GetPhoto(ByVal photoID As Integer) As IDataReader
        Public MustOverride Function GetFirstFromAlbum(ByVal albumID As Integer, ByVal moduleID As Integer) As IDataReader
        Public MustOverride Function GetRandomPhoto(ByVal albumID As Integer, ByVal moduleID As Integer, ByVal rowCount As Integer, ByVal tagID As Integer) As IDataReader
        Public MustOverride Function ListPhoto(ByVal moduleID As Integer, ByVal albumID As Integer, ByVal isApproved As Boolean, ByVal maxCount As Integer, ByVal showAll As Boolean, ByVal tagID As Integer, ByVal batchID As String, ByVal search As String, ByVal sortBy As Integer, ByVal sortOrder As Integer) As IDataReader
        Public MustOverride Function AddPhoto(ByVal moduleID As Integer, ByVal albumID As Integer, ByVal name As String, ByVal description As String, ByVal fileName As String, ByVal dateCreated As DateTime, ByVal width As Integer, ByVal height As Integer, ByVal authorID As Integer, ByVal approverID As Integer, ByVal isApproved As Boolean, ByVal dateApproved As DateTime, ByVal dateUpdated As DateTime, ByVal batchID As String) As Integer
        Public MustOverride Sub UpdatePhoto(ByVal photoID As Integer, ByVal moduleID As Integer, ByVal albumID As Integer, ByVal name As String, ByVal description As String, ByVal fileName As String, ByVal dateCreated As DateTime, ByVal width As Integer, ByVal height As Integer, ByVal authorID As Integer, ByVal approverID As Integer, ByVal isApproved As Boolean, ByVal dateApproved As DateTime, ByVal dateUpdated As DateTime, ByVal batchID As String)
        Public MustOverride Sub DeletePhoto(ByVal photoID As Integer)
        Public MustOverride Sub SetDefaultPhoto(ByVal photoID As Integer, ByVal albumID As Integer)

        Public MustOverride Function GetAlbum(ByVal albumID As Integer) As IDataReader
        Public MustOverride Function GetAlbumByPath(ByVal moduleID As Integer, ByVal homeDirectory As String) As IDataReader
        Public MustOverride Function ListAlbumAll(ByVal moduleID As Integer, ByVal parentAlbumID As Integer, ByVal showPublicOnly As Boolean, ByVal showChildren As Boolean, ByVal sortBy As Integer, ByVal sortOrder As Integer) As IDataReader
        Public MustOverride Function AddAlbum(ByVal moduleID As Integer, ByVal parentAlbumID As Integer, ByVal caption As String, ByVal description As String, ByVal isPublic As Boolean, ByVal homeDirectory As String, ByVal password As String, ByVal albumOrder As Integer, ByVal createDate As DateTime, ByVal inheritSecurity As Boolean) As Integer
        Public MustOverride Sub UpdateAlbum(ByVal albumID As Integer, ByVal moduleID As Integer, ByVal parentAlbumID As Integer, ByVal caption As String, ByVal description As String, ByVal isPublic As Boolean, ByVal homeDirectory As String, ByVal password As String, ByVal albumOrder As Integer, ByVal createDate As DateTime, ByVal inheritSecurity As Boolean)
        Public MustOverride Sub DeleteAlbum(ByVal albumID As Integer)

        Public MustOverride Function GetTemplate(ByVal moduleID As Integer, ByVal name As String) As IDataReader
        Public MustOverride Function AddTemplate(ByVal moduleID As Integer, ByVal name As String, ByVal template As String) As Integer
        Public MustOverride Sub UpdateTemplate(ByVal templateID As Integer, ByVal moduleID As Integer, ByVal name As String, ByVal template As String)

        Public MustOverride Function GetTag(ByVal tagID As Integer) As IDataReader
        Public MustOverride Function GetTagByName(ByVal moduleID As Integer, ByVal nameLowered As String) As IDataReader
        Public MustOverride Function ListTag(ByVal moduleID As Integer, ByVal albumID As Integer, ByVal maxCount As Integer, ByVal showApprovedOnly As Boolean) As IDataReader
        Public MustOverride Function AddTag(ByVal moduleID As Integer, ByVal name As String, ByVal nameLowered As String) As Integer
        Public MustOverride Sub UpdateTag(ByVal tagID As Integer, ByVal moduleID As Integer, ByVal name As String, ByVal nameLowered As String, ByVal usages As Integer)
        Public MustOverride Sub DeleteTag(ByVal tagID As Integer)

        Public MustOverride Sub AddPhotoTag(ByVal photoID As Integer, ByVal tagID As Integer)
        Public MustOverride Sub DeletePhotoTag(ByVal photoID As Integer)
        Public MustOverride Sub DeletePhotoTagByTag(ByVal tagID As Integer)

#End Region

    End Class

End Namespace