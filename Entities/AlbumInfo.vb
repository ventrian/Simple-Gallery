'
' Simple Gallery for DotNetNuke -  http://www.dotnetnuke.com
' Copyright (c) 2002-2007
' by Ventrian ( sales@ventrian.com ) ( http://www.ventrian.com )
'

Namespace Ventrian.SimpleGallery.Entities

    Public Class AlbumInfo

#Region " Private Members "

        Dim _albumID As Integer
        Dim _parentAlbumID As Integer
        Dim _moduleID As Integer
        Dim _caption As String
        Dim _captionIndented As String
        Dim _description As String
        Dim _password As String
        Dim _isPublic As Boolean
        Dim _homeDirectory As String
        Dim _albumOrder As Integer
        Dim _createDate As DateTime
        Dim _inheritSecurity As Boolean

        Dim _numberOfPhotos As Integer
        Dim _numberOfAlbums As Integer
        Dim _numberOfAlbumPhotos As Integer

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

        Public Property ParentAlbumID() As Integer
            Get
                Return _parentAlbumID
            End Get
            Set(ByVal Value As Integer)
                _parentAlbumID = Value
            End Set
        End Property

        Public Property ModuleID() As Integer
            Get
                Return _moduleID
            End Get
            Set(ByVal Value As Integer)
                _moduleID = Value
            End Set
        End Property

        Public Property Caption() As String
            Get
                Return _caption
            End Get
            Set(ByVal Value As String)
                _caption = Value
            End Set
        End Property

        Public Property CaptionIndented() As String
            Get
                Return _captionIndented
            End Get
            Set(ByVal Value As String)
                _captionIndented = Value
            End Set
        End Property

        Public Property Description() As String
            Get
                Return _description
            End Get
            Set(ByVal Value As String)
                _description = Value
            End Set
        End Property

        Public Property Password() As String
            Get
                Return _password
            End Get
            Set(ByVal Value As String)
                _password = Value
            End Set
        End Property

        Public Property IsPublic() As Boolean
            Get
                Return _isPublic
            End Get
            Set(ByVal Value As Boolean)
                _isPublic = Value
            End Set
        End Property

        Public Property HomeDirectory() As String
            Get
                Return _homeDirectory
            End Get
            Set(ByVal Value As String)
                _homeDirectory = Value
            End Set
        End Property

        Public Property AlbumOrder() As Integer
            Get
                Return _albumOrder
            End Get
            Set(ByVal Value As Integer)
                _albumOrder = Value
            End Set
        End Property

        Public Property CreateDate() As DateTime
            Get
                Return _createDate
            End Get
            Set(ByVal Value As DateTime)
                _createDate = Value
            End Set
        End Property

        Public Property InheritSecurity() As Boolean
            Get
                Return _inheritSecurity
            End Get
            Set(ByVal Value As Boolean)
                _inheritSecurity = Value
            End Set
        End Property

        Public Property NumberOfPhotos() As Integer
            Get
                Return _numberOfPhotos
            End Get
            Set(ByVal Value As Integer)
                _numberOfPhotos = Value
            End Set
        End Property

        Public Property NumberOfAlbums() As Integer
            Get
                Return _numberOfAlbums
            End Get
            Set(ByVal Value As Integer)
                _numberOfAlbums = Value
            End Set
        End Property

        Public Property NumberOfAlbumPhotos() As Integer
            Get
                Return _numberOfAlbumPhotos
            End Get
            Set(ByVal Value As Integer)
                _numberOfAlbumPhotos = Value
            End Set
        End Property

#End Region

    End Class

End Namespace
