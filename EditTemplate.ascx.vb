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

    Partial Public Class EditTemplate
        Inherits SimpleGalleryBase

#Region " Private Methods "

        Private Sub BindCrumbs()

            Dim crumbs As New ArrayList

            Dim crumbAllAlbums As New CrumbInfo
            crumbAllAlbums.Caption = Localization.GetString("AllAlbums", LocalResourceFile)
            crumbAllAlbums.Url = NavigateURL()
            crumbs.Add(crumbAllAlbums)

            Dim currentCrumb As New CrumbInfo
            currentCrumb.Caption = Localization.GetString("EditTemplates", LocalResourceFile)
            currentCrumb.Url = Request.Url.ToString()
            crumbs.Add(currentCrumb)

            rptBreadCrumbs.DataSource = crumbs
            rptBreadCrumbs.DataBind()

        End Sub

        Private Sub BindTemplates()

            For Each value As Integer In System.Enum.GetValues(GetType(TemplateType))
                Dim li As New ListItem
                li.Value = System.Enum.GetName(GetType(TemplateType), value)
                li.Text = Localization.GetString(System.Enum.GetName(GetType(TemplateType), value), Me.LocalResourceFile)
                drpTemplates.Items.Add(li)
            Next

        End Sub

        Private Sub BindTemplate()

            Dim objTemplateController As New TemplateController
            Dim objTemplateInfo As TemplateInfo = objTemplateController.Get(Me.ModuleId, drpTemplates.SelectedValue)

            If (objTemplateInfo Is Nothing) Then
                DisplayDefault()
            Else
                txtTemplate.Text = objTemplateInfo.Template
            End If

        End Sub

        Private Sub BindTokens()

            Select Case CType(System.Enum.Parse(GetType(TemplateType), drpTemplates.SelectedValue), TemplateType)

                Case TemplateType.AlbumInfo
                    rptTemplateTokens.DataSource = System.Enum.GetValues(GetType(TemplateTokenAlbumInfo))
                    rptTemplateTokens.DataBind()
                    rptTemplateTokens.Visible = True

                Case TemplateType.PhotoInfo
                    rptTemplateTokens.DataSource = System.Enum.GetValues(GetType(TemplateTokenPhotoInfo))
                    rptTemplateTokens.DataBind()
                    rptTemplateTokens.Visible = True

                Case TemplateType.AddToCart
                    rptTemplateTokens.DataSource = System.Enum.GetValues(GetType(TemplateTokenAddToCartInfo))
                    rptTemplateTokens.DataBind()
                    rptTemplateTokens.Visible = True

                Case TemplateType.ViewCart
                    rptTemplateTokens.Visible = False

            End Select

        End Sub

        Private Sub DisplayDefault()

            Select Case CType(System.Enum.Parse(GetType(TemplateType), drpTemplates.SelectedValue), TemplateType)

                Case TemplateType.AlbumInfo
                    txtTemplate.Text = Constants.DEFAULT_TEMPLATE_ALBUM_INFO

                Case TemplateType.PhotoInfo
                    txtTemplate.Text = Constants.DEFAULT_TEMPLATE_PHOTO_INFO

                Case TemplateType.AddToCart
                    txtTemplate.Text = "AddToCart"

                Case TemplateType.ViewCart
                    txtTemplate.Text = "ViewCart"

            End Select

        End Sub

#End Region

#Region " Protected Methods "

        Protected Function GetLocalizedValue(ByVal key As String) As String
            If (key = "DATECREATED" AndAlso CType(System.Enum.Parse(GetType(TemplateType), drpTemplates.SelectedValue), TemplateType) = TemplateType.AlbumInfo) Then
                Return Localization.GetString("ALBUM-" & key, Me.LocalResourceFile)
            Else
                Return Localization.GetString(key, Me.LocalResourceFile)
            End If
        End Function

#End Region

#Region " Event Handlers "

        Private Sub Page_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load

            Try

                BindCrumbs()

                If (Page.IsPostBack = False) Then

                    BindTemplates()
                    BindTemplate()
                    BindTokens()

                End If

            Catch exc As Exception    'Module failed to load
                ProcessModuleLoadException(Me, exc)
            End Try

        End Sub

        Private Sub cmdCancel_Click(ByVal sender As Object, ByVal e As EventArgs) Handles cmdCancel.Click

            Response.Redirect(NavigateURL(Me.TabId), True)

        End Sub

        Private Sub cmdUpdate_Click(ByVal sender As Object, ByVal e As EventArgs) Handles cmdUpdate.Click

            If (Page.IsValid) Then
                Dim objTemplateController As New TemplateController

                Dim objTemplateInfo As TemplateInfo = objTemplateController.Get(Me.ModuleId, drpTemplates.SelectedValue)

                If (objTemplateInfo Is Nothing) Then
                    objTemplateInfo = New TemplateInfo

                    objTemplateInfo.ModuleID = Me.ModuleId
                    objTemplateInfo.Name = drpTemplates.SelectedValue
                    objTemplateInfo.Template = txtTemplate.Text

                    objTemplateController.Add(objTemplateInfo)
                Else

                    objTemplateInfo.Template = txtTemplate.Text
                    objTemplateController.Update(objTemplateInfo)

                End If

                Dim cacheKey As String = TabModuleId.ToString() & objTemplateInfo.Name
                DotNetNuke.Common.Utilities.DataCache.RemoveCache(cacheKey)

            End If

        End Sub

        Private Sub drpTemplates_SelectedIndexChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles drpTemplates.SelectedIndexChanged
            BindTemplate()
            BindTokens()
        End Sub

        Private Sub cmdRestoreDefault_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cmdRestoreDefault.Click
            DisplayDefault()
        End Sub

#End Region

    End Class

End Namespace