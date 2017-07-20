'
' Simple Gallery for DotNetNuke -  http://www.dotnetnuke.com
' Copyright (c) 2002-2007
' by Ventrian ( sales@ventrian.com ) ( http://www.ventrian.com )
'

Namespace Ventrian.SimpleGallery.Entities

    Public Class CrumbInfo

#Region " Constructors "

        Public Sub New()

        End Sub

        Public Sub New(ByVal caption As String, ByVal url As String)

            _caption = caption
            _url = url

        End Sub

#End Region

#Region " Private Members "

        Dim _caption As String
        Dim _url As String

#End Region

#Region " Public Properties "

        Public Property Caption() As String
            Get
                Return _caption
            End Get
            Set(ByVal Value As String)
                _caption = Value
            End Set
        End Property

        Public Property Url() As String
            Get
                Return _url
            End Get
            Set(ByVal Value As String)
                _url = Value
            End Set
        End Property

#End Region

    End Class

End Namespace
