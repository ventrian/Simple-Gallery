Imports DotNetNuke.Common
Imports DotNetNuke.Services.Exceptions
Imports DotNetNuke.Services.Localization

Namespace Ventrian.SimpleGallery

    Partial Public Class SearchBox
        Inherits SimpleGalleryBase

#Region " Private Methods "

        Private Sub BindBreadCrumbs()

            ucGalleryMenu.AddCrumb(Localization.GetString("AllAlbums", LocalResourceFile), NavigateURL())
            ucGalleryMenu.AddCrumb(Localization.GetString("Search", LocalResourceFile), NavigateURL(Me.TabId, "", "SearchID=" & TabModuleId))

        End Sub

#End Region

#Region " Event Handlers "

        Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load

            Try

                If (GallerySettings.EnableSearch = False) Then
                    Response.Redirect(NavigateURL(), True)
                End If

                BindBreadCrumbs()

                If (Page.IsPostBack = False) Then
                    txtSearch.Focus()
                End If

            Catch exc As Exception    'Module failed to load
                ProcessModuleLoadException(Me, exc)
            End Try

        End Sub

#End Region

#Region " Event Handlers "

        Protected Sub btnSearch_Click(ByVal sender As Object, ByVal e As EventArgs) Handles btnSearch.Click

            Try

                If (txtSearch.Text <> "") Then
                    Response.Redirect(NavigateURL(Me.TabId, "", "SearchID=" & TabModuleId, "SearchText=" & Server.UrlEncode(txtSearch.Text)), True)
                End If

            Catch exc As Exception    'Module failed to load
                ProcessModuleLoadException(Me, exc)
            End Try

        End Sub

#End Region

    End Class

End Namespace