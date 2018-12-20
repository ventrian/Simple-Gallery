'
' Simple Gallery for DotNetNuke -  http://www.dotnetnuke.com
' Copyright (c) 2002-2007
' by Ventrian ( sales@ventrian.com ) ( http://www.ventrian.com )
'

Imports System
Imports System.ComponentModel
Imports System.Web
Imports System.Web.UI
Imports System.Web.UI.HtmlControls
Imports System.Web.UI.WebControls
Imports System.Xml

Imports DotNetNuke
Imports DotNetNuke.Application
Imports DotNetNuke.Common
Imports DotNetNuke.Common.Utilities
Imports DotNetNuke.Entities.Modules
Imports DotNetNuke.Entities.Portals
Imports DotNetNuke.Entities.Tabs
Imports DotNetNuke.Entities.Users
Imports DotNetNuke.Security
Imports DotNetNuke.Security.Permissions

Imports Ventrian.SimpleGallery.Common
Imports Ventrian.SimpleGallery.Entities

Namespace Ventrian.SimpleGallery

    Public Class SimpleGalleryBase

        Inherits PortalModuleBase

#Region " Private Members "

        Private _gallerySettings As GallerySettings

#End Region

#Region " Public Properties "

        Public ReadOnly Property BasePage() As DotNetNuke.Framework.CDefault
            Get
                Return CType(Me.Page, DotNetNuke.Framework.CDefault)
            End Get
        End Property

        Public ReadOnly Property GallerySettings() As Entities.GallerySettings
            Get
                If (_gallerySettings Is Nothing) Then
                    _gallerySettings = New Entities.GallerySettings(Settings)
                End If
                Return _gallerySettings
            End Get
        End Property

#End Region

#Region " Public Methods "

        Public Sub AddRSSFeed(ByVal link As String, ByVal title As String)

            Dim objCSS As System.Web.UI.Control = BasePage.FindControl("CSS")

            If Not (objCSS Is Nothing) Then
                Dim litLink As New Literal
                litLink.Text = "<link rel=""alternate"" type=""application/rss+xml"" title=""" & Me.ModuleConfiguration.ModuleTitle & ": " & title & """ href=""" & link & """ />"
                objCSS.Controls.Add(litLink)
            End If

        End Sub

        Public Function GetButtonStyle(ByVal isPrimary As Boolean) As String
            If (isPrimary) Then
                Return "dnnPrimaryAction"
            Else
                Return "dnnSecondaryAction"
            End If
        End Function

        Public Function FormatBorderPath(ByVal image As String) As String

            Return Me.ResolveUrl("Images/Borders/" & Me.GallerySettings.BorderStyle & "/" & image).Replace(" ", "%20")

        End Function

        Public Function HasAlbumPermissions() As Boolean

            If (Request.IsAuthenticated = False) Then
                Return False
            End If

            If (HasEditPermissions()) Then
                Return True
            End If

            If (Settings.Contains(Constants.SETTING_ALBUM_ROLES)) Then
                Return PortalSecurity.IsInRoles(Settings(Constants.SETTING_ALBUM_ROLES).ToString())
            Else
                Return False
            End If

        End Function

        Public Function HasApprovePhotoPermissions() As Boolean

            If (Request.IsAuthenticated = False) Then
                Return False
            End If

            If (HasEditPermissions()) Then
                Return True
            End If

            If (Settings.Contains(Constants.SETTING_APPROVE_ROLES)) Then
                Return PortalSecurity.IsInRoles(Settings(Constants.SETTING_APPROVE_ROLES).ToString())
            Else
                Return False
            End If

        End Function

        Public Function HasDeletePhotoPermissions() As Boolean

            If (Request.IsAuthenticated = False) Then
                Return False
            End If

            If (HasEditPermissions()) Then
                Return True
            End If

            If (Settings.Contains(Constants.SETTING_DELETE_ROLES)) Then
                Return PortalSecurity.IsInRoles(Settings(Constants.SETTING_DELETE_ROLES).ToString())
            Else
                Return False
            End If

        End Function

        Public Function HasEditPermissions() As Boolean

            If (Request.IsAuthenticated = False) Then
                Return False
            End If

            Return ModulePermissionController.CanEditModuleContent(ModuleConfiguration)

            'Return _
            '    (PortalSecurity.IsInRoles(ModuleConfiguration.AuthorizedEditRoles) = True) Or _
            '    (PortalSecurity.IsInRoles(PortalSettings.ActiveTab.AdministratorRoles) = True) Or _
            '    (PortalSecurity.IsInRoles(PortalSettings.AdministratorRoleName) = True)

        End Function

        Public Function HasEditPhotoPermissions() As Boolean

            If (Request.IsAuthenticated = False) Then
                Return False
            End If

            If (HasEditPermissions()) Then
                Return True
            End If

            If (Settings.Contains(Constants.SETTING_EDIT_ROLES)) Then
                Return PortalSecurity.IsInRoles(Settings(Constants.SETTING_EDIT_ROLES).ToString())
            Else
                Return False
            End If

        End Function

        Public Function HasUploadPhotoPermissions() As Boolean

            If (Request.IsAuthenticated = False) Then
                Return False
            End If

            If (HasEditPermissions()) Then
                Return True
            End If

            If (Settings.Contains(Constants.SETTING_UPLOAD_ROLES)) Then
                Return PortalSecurity.IsInRoles(Settings(Constants.SETTING_UPLOAD_ROLES).ToString())
            Else
                Return False
            End If

        End Function

        Public ReadOnly Property ReturnUrl() As String
            Get
                Dim url As String = HttpContext.Current.Request.RawUrl
                If url.IndexOf("?returnurl=") <> -1 Then
                    url = url.Substring(0, url.IndexOf("?returnurl="))
                End If
                Return HttpUtility.UrlEncode(url)
            End Get
        End Property

#End Region

    End Class

End Namespace
