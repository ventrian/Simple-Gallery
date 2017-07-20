'
' Simple Gallery for DotNetNuke -  http://www.dotnetnuke.com
' Copyright (c) 2002-2007
' by Ventrian ( sales@ventrian.com ) ( http://www.ventrian.com )
'

Imports DotNetNuke.Common
Imports DotNetNuke.Common.Utilities
Imports DotNetNuke.Services.Exceptions
Imports DotNetNuke.Services.Localization

Imports Ventrian.SimpleGallery.Common
Imports Ventrian.SimpleGallery.Entities

Namespace Ventrian.SimpleGallery

    Partial Public Class ViewTag
        Inherits SimpleGalleryBase

#Region " Private Members "

        Private _tag As String = ""
        Private _tagID As Integer = Null.NullInteger

#End Region

#Region " Private Methods "

        Private Sub BindBreadCrumbs()

            ucGalleryMenu.AddCrumb(Localization.GetString("AllAlbums", LocalResourceFile), NavigateURL())
            ucGalleryMenu.AddCrumb(Localization.GetString("Tags", LocalResourceFile), NavigateURL(Me.TabId, "", "Tags=" & TabModuleId))
            If (_tagID <> Null.NullInteger) Then
                Dim objTagController As New TagController
                Dim objTag As TagInfo = objTagController.Get(_tagID)

                If Not (objTag Is Nothing) Then
                    ucGalleryMenu.AddCrumb(objTag.Name, NavigateURL(Me.TabId, "", "Tags=" & TabModuleId, "TagID=" & _tagID.ToString()))
                End If
            Else
                ucGalleryMenu.AddCrumb(_tag, NavigateURL(Me.TabId, "", "Tags=" & TabModuleId, "Tag=" & _tag))
            End If

        End Sub

        Private Sub BindPhotos()

            If (_tagID = Null.NullInteger) Then
                Dim objTagController As New TagController
                Dim objTag As TagInfo = objTagController.Get(Me.ModuleId, _tag.ToLower())

                If (objTag Is Nothing) Then
                    Response.Redirect(NavigateURL(TabId, Null.NullString(), "Tags=" & TabModuleId), True)
                End If

                _tagID = objTag.TagID

                Dim pageTitle As String = Localization.GetString("PageTitle", LocalResourceFile)
                If (pageTitle.IndexOf("{0}") <> -1) Then
                    Me.BasePage.Title = String.Format(pageTitle, objTag.Name)
                End If
            End If

            ucViewPhotos.TagID = _tagID

            If (Request("Page") <> "") Then
                Try
                    ucViewPhotos.PageNumber = Convert.ToInt32(Request("Page"))
                Catch
                End Try
            End If

        End Sub

        Private Sub ReadQueryString()

            If (Request("Tag") <> "") Then
                _tag = Request("Tag")
            End If

            If (Request("TagID") <> "") Then
                _tagID = Convert.ToInt32(Request("TagID"))
            End If

            If (_tag = "" And _tagID = Null.NullInteger) Then
                Response.Redirect(NavigateURL(Me.TabId), True)
            End If

        End Sub

#End Region

#Region " Event Handlers "

        Private Sub Page_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load

            Try

                ReadQueryString()
                BindBreadCrumbs()
                BindPhotos()

            Catch exc As Exception    'Module failed to load
                ProcessModuleLoadException(Me, exc)
            End Try

        End Sub

#End Region

    End Class

End Namespace