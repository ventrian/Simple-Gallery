'
' Simple Gallery for DotNetNuke -  http://www.dotnetnuke.com
' Copyright (c) 2002-2007
' by Ventrian ( sales@ventrian.com ) ( http://www.ventrian.com )
'

Imports DotNetNuke.Common
Imports DotNetNuke.Common.Utilities
Imports DotNetNuke.Services.Exceptions
Imports DotNetNuke.Services.Localization

Imports Ventrian.SimpleGallery.Entities

Namespace Ventrian.SimpleGallery

    Partial Public Class EditTags
        Inherits SimpleGalleryBase

#Region " Private Methods "

        Private Sub BindCrumbs()

            ucGalleryMenu.AddCrumb(Localization.GetString("AllAlbums", LocalResourceFile), NavigateURL())
            ucGalleryMenu.AddCrumb(Localization.GetString("EditTags", LocalResourceFile), EditUrl("EditTags"))

        End Sub

        Private Sub BindTags()

            Dim objTagController As New TagController

            Localization.LocalizeDataGrid(grdTags, Me.LocalResourceFile)

            grdTags.DataSource = objTagController.List(Me.ModuleId, Null.NullInteger, Null.NullInteger, False)
            grdTags.DataBind()

            If (grdTags.Items.Count > 0) Then
                grdTags.Visible = True
                lblNoTags.Visible = False
            Else
                grdTags.Visible = False
                lblNoTags.Visible = True
                lblNoTags.Text = Localization.GetString("NoTagsMessage.Text", LocalResourceFile)
            End If

        End Sub

#End Region

#Region " Event Handlers "

        Private Sub Page_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load

            Try

                BindCrumbs()
                BindTags()

            Catch exc As Exception    'Module failed to load
                ProcessModuleLoadException(Me, exc)
            End Try

        End Sub

        Private Sub cmdAddTag_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cmdAddTag.Click

            Response.Redirect(EditUrl("EditTag"), True)

        End Sub

        Private Sub cmdReturnToGallery_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cmdReturnToGallery.Click

            Response.Redirect(NavigateURL(), True)

        End Sub

#End Region

    End Class

End Namespace