'
' Simple Gallery for DotNetNuke -  http://www.dotnetnuke.com
' Copyright (c) 2002-2007
' by Ventrian ( sales@ventrian.com ) ( http://www.ventrian.com )
'

Imports System.Web

Namespace Ventrian.SimpleGallery

    Public Class SimpleGalleryControlBase
        Inherits System.Web.UI.UserControl

#Region " Private Properties "

        Protected ReadOnly Property SimpleGalleryBase() As SimpleGalleryBase
            Get
                Return CType(Parent, SimpleGalleryBase)
            End Get
        End Property

#End Region

#Region " Public Properties "

        Public ReadOnly Property BorderStyle() As String
            Get
                Return Me.SimpleGalleryBase.GallerySettings.BorderStyle
            End Get
        End Property

#End Region

    End Class

End Namespace
