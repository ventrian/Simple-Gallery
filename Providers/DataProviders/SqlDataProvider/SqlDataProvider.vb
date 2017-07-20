'
' Simple Gallery for DotNetNuke -  http://www.dotnetnuke.com
' Copyright (c) 2002-2005
' by Scott McCulloch ( smcculloch@iinet.net.au ) ( http://www.smcculloch.net )
'

Imports System
Imports System.Configuration
Imports System.Data
Imports Microsoft.ApplicationBlocks.Data

Imports DotNetNuke
Imports DotNetNuke.Common.Utilities

Imports Ventrian.SimpleGallery.Data

Namespace Ventrian.SimpleGallery

    Public Class SqlDataProvider

        Inherits DataProvider

#Region " Private Members "

        Private Const ProviderType As String = "data"

        Private _providerConfiguration As Framework.Providers.ProviderConfiguration = Framework.Providers.ProviderConfiguration.GetProviderConfiguration(ProviderType)
        Private _connectionString As String
        Private _providerPath As String
        Private _objectQualifier As String
        Private _databaseOwner As String

#End Region

#Region " Constructors "

        Public Sub New()

            ' Read the configuration specific information for this provider
            Dim objProvider As Framework.Providers.Provider = CType(_providerConfiguration.Providers(_providerConfiguration.DefaultProvider), Framework.Providers.Provider)

            ' Read the attributes for this provider
            _connectionString = Config.GetConnectionString()

            _providerPath = objProvider.Attributes("providerPath")

            _objectQualifier = objProvider.Attributes("objectQualifier")
            If _objectQualifier <> "" And _objectQualifier.EndsWith("_") = False Then
                _objectQualifier += "_"
            End If

            _databaseOwner = objProvider.Attributes("databaseOwner")
            If _databaseOwner <> "" And _databaseOwner.EndsWith(".") = False Then
                _databaseOwner += "."
            End If

        End Sub

#End Region

#Region " Properties "

        Public ReadOnly Property ConnectionString() As String
            Get
                Return _connectionString
            End Get
        End Property

        Public ReadOnly Property ProviderPath() As String
            Get
                Return _providerPath
            End Get
        End Property

        Public ReadOnly Property ObjectQualifier() As String
            Get
                Return _objectQualifier
            End Get
        End Property

        Public ReadOnly Property DatabaseOwner() As String
            Get
                Return _databaseOwner
            End Get
        End Property

#End Region

#Region " Public Methods "

        Private Function GetNull(ByVal Field As Object) As Object
            Return DotNetNuke.Common.Utilities.Null.GetNull(Field, DBNull.Value)
        End Function

        Public Overrides Function GetPhoto(ByVal photoID As Integer) As IDataReader
            Return CType(SqlHelper.ExecuteReader(ConnectionString, DatabaseOwner & ObjectQualifier & "DnnForge_SimpleGallery_PhotoGet", photoID), IDataReader)
        End Function

        Public Overrides Function GetFirstFromAlbum(ByVal albumID As Integer, ByVal moduleID As Integer) As IDataReader
            Return CType(SqlHelper.ExecuteReader(ConnectionString, DatabaseOwner & ObjectQualifier & "DnnForge_SimpleGallery_GetFirstFromAlbum", albumID, moduleID), IDataReader)
        End Function

        Public Overrides Function GetRandomPhoto(ByVal moduleID As Integer, ByVal albumID As Integer, ByVal rowCount As Integer, ByVal tagID As Integer) As IDataReader
            Return CType(SqlHelper.ExecuteReader(ConnectionString, DatabaseOwner & ObjectQualifier & "DnnForge_SimpleGallery_GetRandomPhoto", albumID, moduleID, rowCount, GetNull(tagID)), IDataReader)
        End Function

        Public Overrides Function ListPhoto(ByVal moduleID As Integer, ByVal albumID As Integer, ByVal isApproved As Boolean, ByVal maxCount As Integer, ByVal showAll As Boolean, ByVal tagID As Integer, ByVal batchID As String, ByVal search As String, ByVal sortBy As Integer, ByVal sortOrder As Integer) As IDataReader
            Return CType(SqlHelper.ExecuteReader(ConnectionString, DatabaseOwner & ObjectQualifier & "DnnForge_SimpleGallery_PhotoList", moduleID, GetNull(albumID), isApproved, GetNull(maxCount), showAll, GetNull(tagID), GetNull(batchID), GetNull(search), sortBy, sortOrder), IDataReader)
        End Function

        Public Overrides Function AddPhoto(ByVal moduleID As Integer, ByVal albumID As Integer, ByVal name As String, ByVal description As String, ByVal fileName As String, ByVal dateCreated As DateTime, ByVal width As Integer, ByVal height As Integer, ByVal authorID As Integer, ByVal approverID As Integer, ByVal isApproved As Boolean, ByVal dateApproved As DateTime, ByVal dateUpdated As DateTime, ByVal batchID As String) As Integer
            Return CType(SqlHelper.ExecuteScalar(ConnectionString, DatabaseOwner & ObjectQualifier & "DnnForge_SimpleGallery_PhotoAdd", moduleID, albumID, name, GetNull(description), fileName, dateCreated, width, height, GetNull(authorID), GetNull(approverID), isApproved, GetNull(dateApproved), GetNull(dateUpdated), GetNull(batchID)), Integer)
        End Function

        Public Overrides Sub UpdatePhoto(ByVal photoID As Integer, ByVal moduleID As Integer, ByVal albumID As Integer, ByVal name As String, ByVal description As String, ByVal fileName As String, ByVal dateCreated As DateTime, ByVal width As Integer, ByVal height As Integer, ByVal authorID As Integer, ByVal approverID As Integer, ByVal isApproved As Boolean, ByVal dateApproved As DateTime, ByVal dateUpdated As DateTime, ByVal batchID As String)
            SqlHelper.ExecuteNonQuery(ConnectionString, DatabaseOwner & ObjectQualifier & "DnnForge_SimpleGallery_PhotoUpdate", photoID, moduleID, albumID, name, GetNull(description), fileName, dateCreated, width, height, GetNull(authorID), GetNull(approverID), isApproved, GetNull(dateApproved), GetNull(dateUpdated), GetNull(batchID))
        End Sub

        Public Overrides Sub DeletePhoto(ByVal photoID As Integer)
            SqlHelper.ExecuteNonQuery(ConnectionString, DatabaseOwner & ObjectQualifier & "DnnForge_SimpleGallery_PhotoDelete", photoID)
        End Sub

        Public Overrides Sub SetDefaultPhoto(ByVal photoID As Integer, ByVal albumID As Integer)
            SqlHelper.ExecuteNonQuery(ConnectionString, DatabaseOwner & ObjectQualifier & "DnnForge_SimpleGallery_SetDefaultPhoto", photoID, albumID)
        End Sub

        Public Overrides Function GetAlbum(ByVal albumID As Integer) As IDataReader
            Return CType(SqlHelper.ExecuteReader(ConnectionString, DatabaseOwner & ObjectQualifier & "DnnForge_SimpleGallery_AlbumGet", albumID), IDataReader)
        End Function

        Public Overrides Function GetAlbumByPath(ByVal moduleID As Integer, ByVal homeDirectory As String) As IDataReader
            Return CType(SqlHelper.ExecuteReader(ConnectionString, DatabaseOwner & ObjectQualifier & "DnnForge_SimpleGallery_AlbumGetByPath", moduleID, homeDirectory), IDataReader)
        End Function

        Public Overrides Function ListAlbumAll(ByVal moduleID As Integer, ByVal parentAlbumID As Integer, ByVal showPublicOnly As Boolean, ByVal showChildren As Boolean, ByVal sortBy As Integer, ByVal sortOrder As Integer) As IDataReader
            Return CType(SqlHelper.ExecuteReader(ConnectionString, DatabaseOwner & ObjectQualifier & "DnnForge_SimpleGallery_AlbumListAll", moduleID, parentAlbumID, showPublicOnly, showChildren, sortBy, sortOrder), IDataReader)
        End Function

        Public Overrides Function AddAlbum(ByVal moduleID As Integer, ByVal parentModuleID As Integer, ByVal caption As String, ByVal description As String, ByVal isPublic As Boolean, ByVal homeDirectory As String, ByVal password As String, ByVal albumOrder As Integer, ByVal createDate As DateTime, ByVal inheritSecurity As Boolean) As Integer
            Return CType(SqlHelper.ExecuteScalar(ConnectionString, DatabaseOwner & ObjectQualifier & "DnnForge_SimpleGallery_AlbumAdd", moduleID, parentModuleID, caption, description, isPublic, homeDirectory, password, albumOrder, createDate, inheritSecurity), Integer)
        End Function

        Public Overrides Sub UpdateAlbum(ByVal albumID As Integer, ByVal moduleID As Integer, ByVal parentModuleID As Integer, ByVal caption As String, ByVal description As String, ByVal isPublic As Boolean, ByVal homeDirectory As String, ByVal password As String, ByVal albumOrder As Integer, ByVal createDate As DateTime, ByVal inheritSecurity As Boolean)
            SqlHelper.ExecuteNonQuery(ConnectionString, DatabaseOwner & ObjectQualifier & "DnnForge_SimpleGallery_AlbumUpdate", albumID, moduleID, parentModuleID, caption, description, isPublic, homeDirectory, password, albumOrder, createDate, inheritSecurity)
        End Sub

        Public Overrides Sub DeleteAlbum(ByVal albumID As Integer)
            SqlHelper.ExecuteNonQuery(ConnectionString, DatabaseOwner & ObjectQualifier & "DnnForge_SimpleGallery_AlbumDelete", albumID)
        End Sub

        Public Overrides Function GetTemplate(ByVal moduleID As Integer, ByVal name As String) As IDataReader
            Return CType(SqlHelper.ExecuteReader(ConnectionString, DatabaseOwner & ObjectQualifier & "DnnForge_SimpleGallery_TemplateGet", moduleID, name), IDataReader)
        End Function

        Public Overrides Function AddTemplate(ByVal moduleID As Integer, ByVal name As String, ByVal template As String) As Integer
            Return CType(SqlHelper.ExecuteScalar(ConnectionString, DatabaseOwner & ObjectQualifier & "DnnForge_SimpleGallery_TemplateAdd", moduleID, name, template), Integer)
        End Function

        Public Overrides Sub UpdateTemplate(ByVal templateID As Integer, ByVal moduleID As Integer, ByVal name As String, ByVal template As String)
            SqlHelper.ExecuteNonQuery(ConnectionString, DatabaseOwner & ObjectQualifier & "DnnForge_SimpleGallery_TemplateUpdate", templateID, moduleID, name, template)
        End Sub

        Public Overrides Function GetTag(ByVal tagID As Integer) As IDataReader
            Return CType(SqlHelper.ExecuteReader(ConnectionString, DatabaseOwner & ObjectQualifier & "DnnForge_SimpleGallery_TagGet", tagID), IDataReader)
        End Function

        Public Overrides Function GetTagByName(ByVal moduleID As Integer, ByVal name As String) As IDataReader
            Return CType(SqlHelper.ExecuteReader(ConnectionString, DatabaseOwner & ObjectQualifier & "DnnForge_SimpleGallery_TagGetByName", moduleID, name), IDataReader)
        End Function

        Public Overrides Function ListTag(ByVal moduleID As Integer, ByVal albumID As Integer, ByVal maxCount As Integer, ByVal showApprovedOnly As Boolean) As IDataReader
            Return CType(SqlHelper.ExecuteReader(ConnectionString, DatabaseOwner & ObjectQualifier & "DnnForge_SimpleGallery_TagList", moduleID, GetNull(albumID), GetNull(maxCount), showApprovedOnly), IDataReader)
        End Function

        Public Overrides Function AddTag(ByVal moduleID As Integer, ByVal name As String, ByVal nameLowered As String) As Integer
            Return CType(SqlHelper.ExecuteScalar(ConnectionString, DatabaseOwner & ObjectQualifier & "DnnForge_SimpleGallery_TagAdd", moduleID, name, nameLowered), Integer)
        End Function

        Public Overrides Sub UpdateTag(ByVal tagID As Integer, ByVal moduleID As Integer, ByVal name As String, ByVal nameLowered As String, ByVal usages As Integer)
            SqlHelper.ExecuteNonQuery(ConnectionString, DatabaseOwner & ObjectQualifier & "DnnForge_SimpleGallery_TagUpdate", tagID, moduleID, name, nameLowered, usages)
        End Sub

        Public Overrides Sub DeleteTag(ByVal tagID As Integer)
            SqlHelper.ExecuteNonQuery(ConnectionString, DatabaseOwner & ObjectQualifier & "DnnForge_SimpleGallery_TagDelete", tagID)
        End Sub

        Public Overrides Sub AddPhotoTag(ByVal photoID As Integer, ByVal tagID As Integer)
            SqlHelper.ExecuteNonQuery(ConnectionString, DatabaseOwner & ObjectQualifier & "DnnForge_SimpleGallery_PhotoTagAdd", photoID, tagID)
        End Sub

        Public Overrides Sub DeletePhotoTag(ByVal photoID As Integer)
            SqlHelper.ExecuteNonQuery(ConnectionString, DatabaseOwner & ObjectQualifier & "DnnForge_SimpleGallery_PhotoTagDelete", photoID)
        End Sub

        Public Overrides Sub DeletePhotoTagByTag(ByVal tagID As Integer)
            SqlHelper.ExecuteNonQuery(ConnectionString, DatabaseOwner & ObjectQualifier & "DnnForge_SimpleGallery_PhotoTagDeleteByTag", tagID)
        End Sub

#End Region

    End Class

End Namespace