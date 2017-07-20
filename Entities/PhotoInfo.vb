'
' Simple Gallery for DotNetNuke -  http://www.dotnetnuke.com
' Copyright (c) 2002-2007
' by Ventrian ( sales@ventrian.com ) ( http://www.ventrian.com )
'

Namespace Ventrian.SimpleGallery.Entities

    Public Class PhotoInfo
        Implements IComparable

#Region " Private Members "

        Dim _photoID As Integer
        Dim _albumID As Integer
        Dim _moduleID As Integer
        Dim _name As String
        Dim _description As String
        Dim _fileName As String
        Dim _dateCreated As DateTime
        Dim _dateUpdated As DateTime
        Dim _width As Integer
        Dim _height As Integer
        Dim _homeDirectory As String
        Dim _authorID As Integer
        Dim _approverID As Integer
        Dim _isApproved As Boolean
        Dim _isDefault As Boolean
        Dim _dateApproved As DateTime
        Dim _authorFirstName As String
        Dim _authorLastName As String
        Dim _authorUserName As String
        Dim _authorDisplayName As String
        Dim _approverFirstName As String
        Dim _approverLastName As String
        Dim _approverUserName As String
        Dim _approverDisplayName As String
        Dim _albumName As String
        Dim _tags As String
        Dim _batchID As String

#End Region

#Region " Shared Members "

        Shared _sortBy As String = "Name"
        Shared _sortDirection As String = "ASC"

#End Region

#Region " Public Properties "

        Public Property PhotoID() As Integer
            Get
                Return _photoID
            End Get
            Set(ByVal Value As Integer)
                _photoID = Value
            End Set
        End Property

        Public Property AlbumID() As Integer
            Get
                Return _albumID
            End Get
            Set(ByVal Value As Integer)
                _albumID = Value
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

        Public Property Name() As String
            Get
                Return _name
            End Get
            Set(ByVal Value As String)
                _name = Value
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

        Public Property FileName() As String
            Get
                Return _fileName
            End Get
            Set(ByVal Value As String)
                _fileName = Value
            End Set
        End Property

        Public Property DateCreated() As DateTime
            Get
                Return _dateCreated
            End Get
            Set(ByVal Value As DateTime)
                _dateCreated = Value
            End Set
        End Property

        Public Property DateUpdated() As DateTime
            Get
                Return _dateUpdated
            End Get
            Set(ByVal Value As DateTime)
                _dateUpdated = Value
            End Set
        End Property

        Public Property Width() As Integer
            Get
                Return _width
            End Get
            Set(ByVal Value As Integer)
                _width = Value
            End Set
        End Property

        Public Property Height() As Integer
            Get
                Return _height
            End Get
            Set(ByVal Value As Integer)
                _height = Value
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

        Public Property AuthorID() As Integer
            Get
                Return _authorID
            End Get
            Set(ByVal Value As Integer)
                _authorID = Value
            End Set
        End Property

        Public Property ApproverID() As Integer
            Get
                Return _approverID
            End Get
            Set(ByVal Value As Integer)
                _approverID = Value
            End Set
        End Property

        Public Property IsApproved() As Boolean
            Get
                Return _isApproved
            End Get
            Set(ByVal Value As Boolean)
                _isApproved = Value
            End Set
        End Property

        Public Property IsDefault() As Boolean
            Get
                Return _isDefault
            End Get
            Set(ByVal Value As Boolean)
                _isDefault = Value
            End Set
        End Property

        Public Property DateApproved() As DateTime
            Get
                Return _dateApproved
            End Get
            Set(ByVal Value As DateTime)
                _dateApproved = Value
            End Set
        End Property

        Public Property AuthorFirstName() As String
            Get
                Return _authorFirstName
            End Get
            Set(ByVal Value As String)
                _authorFirstName = Value
            End Set
        End Property

        Public Property AuthorLastName() As String
            Get
                Return _authorLastName
            End Get
            Set(ByVal Value As String)
                _authorLastName = Value
            End Set
        End Property

        Public Property AuthorUserName() As String
            Get
                Return _authorUserName
            End Get
            Set(ByVal Value As String)
                _authorUserName = Value
            End Set
        End Property

        Public Property AuthorDisplayName() As String
            Get
                Return _authorDisplayName
            End Get
            Set(ByVal Value As String)
                _authorDisplayName = Value
            End Set
        End Property

        Public Property ApproverFirstName() As String
            Get
                Return _approverFirstName
            End Get
            Set(ByVal Value As String)
                _approverFirstName = Value
            End Set
        End Property

        Public Property ApproverLastName() As String
            Get
                Return _approverLastName
            End Get
            Set(ByVal Value As String)
                _approverLastName = Value
            End Set
        End Property

        Public Property ApproverUserName() As String
            Get
                Return _approverUserName
            End Get
            Set(ByVal Value As String)
                _approverUserName = Value
            End Set
        End Property

        Public Property ApproverDisplayName() As String
            Get
                Return _approverDisplayName
            End Get
            Set(ByVal Value As String)
                _approverDisplayName = Value
            End Set
        End Property

        Public Property AlbumName() As String
            Get
                Return _albumName
            End Get
            Set(ByVal Value As String)
                _albumName = Value
            End Set
        End Property

        Public Property Tags() As String
            Get
                Return _tags
            End Get
            Set(ByVal Value As String)
                _tags = Value
            End Set
        End Property

        Public Property BatchID() As String
            Get
                Return _batchID
            End Get
            Set(ByVal Value As String)
                _batchID = Value
            End Set
        End Property

#End Region

#Region " Shared Properties "

        Public Shared Property SortBy() As String
            Get
                Return _sortBy
            End Get
            Set(ByVal Value As String)
                _sortBy = Value
            End Set
        End Property

        Public Shared Property SortDirection() As String
            Get
                Return _sortDirection
            End Get
            Set(ByVal Value As String)
                _sortDirection = Value
            End Set
        End Property

#End Region

#Region " Optional Interfaces "

        Public Function CompareTo(ByVal obj As Object) As Integer Implements System.IComparable.CompareTo

            If TypeOf obj Is PhotoInfo Then

                Dim objPhoto As PhotoInfo = CType(obj, PhotoInfo)

                If Not objPhoto Is Nothing Then

                    Select Case (SortBy.ToLower())
                        Case "name"
                            If (SortDirection = "ASC") Then
                                Return Me.Name.CompareTo(objPhoto.Name)
                            Else
                                Return Me.Name.CompareTo(objPhoto.Name) * -1
                            End If
                        Case "datecreated"
                            If (SortDirection = "ASC") Then
                                Return Me.DateCreated.CompareTo(objPhoto.DateCreated)
                            Else
                                Return Me.DateCreated.CompareTo(objPhoto.DateCreated) * -1
                            End If
                        Case "dateapproved"
                            If (SortDirection = "ASC") Then
                                Return Me.DateApproved.CompareTo(objPhoto.DateApproved)
                            Else
                                Return Me.DateApproved.CompareTo(objPhoto.DateApproved) * -1
                            End If
                        Case "filename"
                            If (SortDirection = "ASC") Then
                                Return Me.FileName.CompareTo(objPhoto.FileName)
                            Else
                                Return Me.FileName.CompareTo(objPhoto.FileName) * -1
                            End If
                        Case Else
                            Return Me.Name.CompareTo(objPhoto.Name)
                    End Select

                End If

            End If

            Return 0

        End Function

#End Region

    End Class

End Namespace
