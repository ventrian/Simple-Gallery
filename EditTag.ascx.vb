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

    Partial Public Class EditTag
        Inherits SimpleGalleryBase

#Region " Private Members "

        Private _tagID As Integer = Null.NullInteger
        Private _photoCount As Integer = 0
        Private _albumCount As Integer = 0

#End Region

#Region " Private Methods "

        Private Sub BindCrumbs()

            ucGalleryMenu.AddCrumb(Localization.GetString("AllAlbums", LocalResourceFile), NavigateURL())
            ucGalleryMenu.AddCrumb(Localization.GetString("EditTags", LocalResourceFile), EditUrl("EditTags"))
            ucGalleryMenu.AddCrumb(Localization.GetString("EditTag", LocalResourceFile), EditUrl("TagID", _tagID.ToString(), "EditTag"))

        End Sub

        Private Sub ReadQueryString()

            If Not (Request("TagID") Is Nothing) Then
                _tagID = Convert.ToInt32(Request("TagID"))
            End If

        End Sub

        Private Sub BindTag()

            If (_tagID = Null.NullInteger) Then

                cmdDelete.Visible = False

            Else

                cmdDelete.Visible = True
                cmdDelete.Attributes.Add("onClick", "javascript:return confirm('" & Localization.GetString("Confirmation", LocalResourceFile) & "');")

                Dim objTagController As New TagController
                Dim objTag As TagInfo = objTagController.Get(_tagID)

                If Not (objTag Is Nothing) Then
                    txtName.Text = objTag.Name
                End If

            End If

        End Sub

#End Region

#Region " Event Handlers "

        Private Sub Page_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load

            Try

                ReadQueryString()
                BindCrumbs()

                If (IsPostBack = False) Then

                    BindTag()

                End If

            Catch exc As Exception    'Module failed to load
                ProcessModuleLoadException(Me, exc)
            End Try

        End Sub

        Private Sub cmdUpdate_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cmdUpdate.Click


            Try

                If (Page.IsValid) Then

                    Dim objTagController As New TagController

                    Dim objTag As New TagInfo

                    If (_tagID <> Null.NullInteger) Then
                        objTag = objTagController.Get(_tagID)
                    Else
                        objTag = CType(CBO.InitializeObject(objTag, GetType(TagInfo)), TagInfo)
                    End If

                    objTag.ModuleID = Me.ModuleId
                    objTag.Name = txtName.Text
                    objTag.NameLowered = txtName.Text.ToLower()

                    If (_tagID = Null.NullInteger) Then
                        objTagController.Add(objTag)
                    Else
                        objTagController.Update(objTag)
                    End If

                    Response.Redirect(EditUrl("EditTags"), True)

                End If

            Catch exc As Exception    'Module failed to load
                ProcessModuleLoadException(Me, exc)
            End Try

        End Sub

        Private Sub cmdDelete_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cmdDelete.Click

            Try

                Dim objTagController As New TagController
                objTagController.DeletePhotoTagByTag(_tagID)
                objTagController.Delete(_tagID)

                Response.Redirect(EditUrl("EditTags"), True)

            Catch exc As Exception    'Module failed to load
                ProcessModuleLoadException(Me, exc)
            End Try

        End Sub

        Private Sub cmdCancel_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cmdCancel.Click

            Try

                Response.Redirect(EditUrl("EditTags"), True)

            Catch exc As Exception    'Module failed to load
                ProcessModuleLoadException(Me, exc)
            End Try

        End Sub

#End Region

    End Class

End Namespace