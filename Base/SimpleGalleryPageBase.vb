Imports DotNetNuke.Entities.Portals

'
' Simple Gallery for DotNetNuke -  http://www.dotnetnuke.com
' Copyright (c) 2002-2007
' by Ventrian ( sales@ventrian.com ) ( http://www.ventrian.com )
'

Namespace Ventrian.SimpleGallery

    Public Class SimpleGalleryPageBase
        Inherits System.Web.UI.Page

#Region " Private Members "

        Private _gallerySettings As Entities.GallerySettings

#End Region

#Region " Public Properties "

        Public ReadOnly Property GallerySettings(ByVal settings As Hashtable) As Entities.GallerySettings
            Get
                If (_gallerySettings Is Nothing) Then
                    _gallerySettings = New Entities.GallerySettings(settings)
                End If
                Return _gallerySettings
            End Get
        End Property

        Public ReadOnly Property PortalSettings() As PortalSettings
            Get
                Return PortalSettings.Current
            End Get
        End Property

#End Region

    End Class

End Namespace

