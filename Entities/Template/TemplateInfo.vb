'
' Simple Gallery for DotNetNuke -  http://www.dotnetnuke.com
' Copyright (c) 2002-2007
' by Ventrian ( sales@ventrian.com ) ( http://www.ventrian.com )
'

Namespace Ventrian.SimpleGallery.Entities

    Public Class TemplateInfo

#Region " Private Members "

        Dim _templateID As Integer
        Dim _moduleID As Integer
        Dim _name As String
        Dim _template As String
        Dim _tokens As String()

#End Region

#Region " Public Properties "

        Public Property TemplateID() As Integer
            Get
                Return _templateID
            End Get
            Set(ByVal Value As Integer)
                _templateID = Value
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

        Public Property Template() As String
            Get
                Return _template
            End Get
            Set(ByVal Value As String)
                _template = Value
            End Set
        End Property

        Public ReadOnly Property Tokens() As String()
            Get
                Dim delimStr As String = "[]"
                Dim delimiter As Char() = delimStr.ToCharArray()

                Return Template.Split(delimiter)
            End Get
        End Property

#End Region

    End Class

End Namespace
