'
' Simple Gallery for DotNetNuke -  http://www.dotnetnuke.com
' Copyright (c) 2002-2007
' by Ventrian ( sales@ventrian.com ) ( http://www.ventrian.com )
'

Imports DotNetNuke.Common
Imports DotNetNuke.Common.Utilities
Imports DotNetNuke.Entities.Modules
Imports DotNetNuke.Entities.Modules.Actions
Imports DotNetNuke.Security
Imports DotNetNuke.Services.Exceptions
Imports DotNetNuke.Services.Localization

Imports Ventrian.SimpleGallery.Common
Imports Ventrian.SimpleGallery.Entities

Namespace Ventrian.SimpleGallery

    Partial Public Class Gallery
        Inherits SimpleGalleryBase
        Implements IActionable

#Region " Private Members "

        Private _controlToLoad As String

#End Region

#Region " Private Methods "

        Private Sub LoadControlType()

            Dim objPortalModuleBase As PortalModuleBase = CType(Me.LoadControl(_controlToLoad), PortalModuleBase)

            If Not (objPortalModuleBase Is Nothing) Then

                objPortalModuleBase.ModuleConfiguration = Me.ModuleConfiguration
                objPortalModuleBase.ID = System.IO.Path.GetFileNameWithoutExtension(_controlToLoad)
                phControls.Controls.Add(objPortalModuleBase)

            End If

        End Sub

        Private Sub InitializeModule()

            If (Settings.Contains("InitializeModule") = False) Then

                Dim objAlbumController As New AlbumController
                Dim objAlbums As ArrayList = objAlbumController.List(Me.ModuleId, Null.NullInteger, False, False, AlbumSortType.Caption, SortDirection.ASC)

                If (objAlbums.Count = 0) Then
                    Dim objAlbum As New AlbumInfo

                    objAlbum.ModuleID = Me.ModuleId
                    objAlbum.ParentAlbumID = Null.NullInteger
                    objAlbum.Caption = Localization.GetString("UntitledAlbum", Me.LocalResourceFile)
                    If (objAlbum.Caption = "") Then
                        objAlbum.Caption = "Untitled Album"
                    End If
                    objAlbum.Description = ""
                    objAlbum.IsPublic = True
                    objAlbum.HomeDirectory = "Gallery/Album/[ALBUMID]"
                    objAlbum.CreateDate = DateTime.Now

                    objAlbum.AlbumID = objAlbumController.Add(objAlbum)
                    objAlbum.HomeDirectory = "Gallery/Album/" & objAlbum.AlbumID.ToString()
                    objAlbum.InheritSecurity = True
                    objAlbumController.Update(objAlbum)
                End If

                Dim objModuleController As New ModuleController
                objModuleController.UpdateModuleSetting(Me.ModuleId, "InitializeModule", "true")

            End If

        End Sub

        Private Sub ReadQueryString()

            If Not (Request("galleryType") Is Nothing) Then

                ' Load the appropriate Control
                '
                Select Case Request("galleryType").ToLower()

                    Case "slideshow"
                        _controlToLoad = "SlideShow.ascx"

                    Case "upload"
                        _controlToLoad = "Upload.ascx"

                    Case "zipalbum"
                        _controlToLoad = "ZipAlbum.ascx"

                    Case Else
                        _controlToLoad = "ViewGallery.ascx"

                End Select

            Else

                _controlToLoad = "ViewGallery.ascx"

                If (Request("Tags") = Me.TabModuleId.ToString()) Then
                    If (Request("Tag") <> "" Or Request("TagID") <> "") Then
                        _controlToLoad = "ViewTag.ascx"
                    Else
                        _controlToLoad = "Tags.ascx"
                    End If
                End If

                If (Request("SearchID") = Me.TabModuleId.ToString()) Then
                    If (Request("SearchText") <> "") Then
                        _controlToLoad = "ViewSearch.ascx"
                    Else
                        _controlToLoad = "SearchBox.ascx"
                    End If
                End If

            End If

        End Sub
#End Region

#Region " Event Handlers "

        Private Sub Page_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load

            Try

                InitializeModule()

            Catch exc As Exception    'Module failed to load
                ProcessModuleLoadException(Me, exc)
            End Try

        End Sub

        Private Sub Page_Initialize(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Init

            Try

                ReadQueryString()
                LoadControlType()

            Catch exc As Exception    'Module failed to load
                ProcessModuleLoadException(Me, exc)
            End Try

        End Sub

#End Region

#Region " Optional Interfaces "

        Public ReadOnly Property ModuleActions() As DotNetNuke.Entities.Modules.Actions.ModuleActionCollection Implements IActionable.ModuleActions
            Get
                Dim Actions As New ModuleActionCollection
                If (HasUploadPhotoPermissions) Then
                    Actions.Add(GetNextActionID, Localization.GetString("AddNewPhoto.Text", LocalResourceFile), ModuleActionType.AddContent, "", "", EditUrl("Add"), False, SecurityAccessLevel.View, True, False)
                End If
                If (HasAlbumPermissions()) Then
                    Actions.Add(GetNextActionID, Localization.GetString("EditAlbums.Text", LocalResourceFile), ModuleActionType.AddContent, "", "", EditUrl("EditAlbums"), False, SecurityAccessLevel.View, True, False)
                End If
                Actions.Add(GetNextActionID, Localization.GetString("EditTags.Text", LocalResourceFile), ModuleActionType.ContentOptions, "", "", EditUrl("EditTags"), False, SecurityAccessLevel.Edit, True, False)
                Actions.Add(GetNextActionID, Localization.GetString("EditTemplates.Text", LocalResourceFile), ModuleActionType.ContentOptions, "", "", EditUrl("EditTemplates"), False, SecurityAccessLevel.Edit, True, False)
                Return Actions
            End Get
        End Property

#End Region

    End Class

End Namespace