'
' Property Agent for DotNetNuke -  http://www.dotnetnuke.com
' Copyright (c) 2002-2007
' by Ventrian ( sales@ventrian.com ) ( http://www.ventrian.com )
'

Imports DotNetNuke.Common.Utilities
Imports DotNetNuke.Services.Exceptions
Imports DotNetNuke.Services.Localization

Imports Ventrian.SimpleGallery.Common
Imports Ventrian.SimpleGallery.Entities

Namespace Ventrian.SimpleGallery.Controls

    Partial Public Class EditPhotos
        Inherits SimpleGalleryControlBase

#Region " Private Members "

        Private _batchID As String = Null.NullString
        Private _albumID As Integer = Null.NullInteger
        Private _tagID As Integer = Null.NullInteger

#End Region

#Region " Private Properties "

        Private Shadows ReadOnly Property LocalResourceFile() As String
            Get
                Return "~/DesktopModules/SimpleGallery/App_LocalResources/EditPhotos.ascx.resx"
            End Get
        End Property

#End Region

#Region " Private Methods "

        Protected Function GetPhotoHeight(ByVal dataItem As Object) As String

            Dim thumbnailWidth As Integer = 175
            Dim thumbnailHeight As Integer = 175

            Dim objPhoto As PhotoInfo = CType(dataItem, PhotoInfo)

            If Not (objPhoto Is Nothing) Then
                Dim width As Integer
                If (objPhoto.Width > thumbnailWidth) Then
                    width = thumbnailWidth
                Else
                    width = objPhoto.Width
                End If

                Dim height As Integer = Convert.ToInt32(objPhoto.Height / (objPhoto.Width / width))
                If (height > thumbnailHeight) Then
                    height = thumbnailHeight
                    width = Convert.ToInt32(objPhoto.Width / (objPhoto.Height / height))
                End If

                Return height.ToString()
            Else
                Return thumbnailWidth.ToString()
            End If

        End Function

        Private Function GetPhotoPath(ByVal dataItem As Object) As String

            Dim objPhoto As PhotoInfo = CType(dataItem, PhotoInfo)

            If Not (objPhoto Is Nothing) Then
                If (SimpleGalleryBase.GallerySettings.Compression = CompressionType.MinSize) Then
                    Return Me.ResolveUrl("../ImageHandler.ashx?width=" & GetPhotoWidth(CType(objPhoto, Object)) & "&height=" & GetPhotoHeight(CType(objPhoto, Object)) & "&HomeDirectory=" & System.Uri.EscapeDataString(SimpleGalleryBase.PortalSettings.HomeDirectory & objPhoto.HomeDirectory) & "&fileName=" & System.Uri.EscapeDataString(objPhoto.FileName) & "&portalid=" & SimpleGalleryBase.PortalId.ToString() & "&i=" & objPhoto.PhotoID)
                Else
                    Return Me.ResolveUrl("../ImageHandler.ashx?width=" & GetPhotoWidth(CType(objPhoto, Object)) & "&height=" & GetPhotoHeight(CType(objPhoto, Object)) & "&HomeDirectory=" & System.Uri.EscapeDataString(SimpleGalleryBase.PortalSettings.HomeDirectory & objPhoto.HomeDirectory) & "&fileName=" & System.Uri.EscapeDataString(objPhoto.FileName) & "&portalid=" & SimpleGalleryBase.PortalId.ToString() & "&i=" & objPhoto.PhotoID & "&q=1")
                End If
            End If

            Return ""

        End Function

        Protected Function GetPhotoWidth(ByVal dataItem As Object) As String

            Dim thumbnailWidth As Integer = 175
            Dim thumbnailHeight As Integer = 175

            Dim objPhoto As PhotoInfo = CType(dataItem, PhotoInfo)

            If Not (objPhoto Is Nothing) Then
                Dim width As Integer
                If (objPhoto.Width > thumbnailWidth) Then
                    width = thumbnailWidth
                Else
                    width = objPhoto.Width
                End If

                Dim height As Integer = Convert.ToInt32(objPhoto.Height / (objPhoto.Width / width))
                If (height > thumbnailHeight) Then
                    height = thumbnailHeight
                    width = Convert.ToInt32(objPhoto.Width / (objPhoto.Height / height))
                End If

                Return width.ToString()
            Else
                Return thumbnailWidth.ToString()
            End If

        End Function

#End Region

#Region " Public Methods "

        Public Sub BindPhotos()

            Dim objPhotoController As New PhotoController

            dlGallery.DataSource = objPhotoController.List(SimpleGalleryBase.ModuleId, AlbumID, True, Null.NullInteger, True, TagID, BatchID, Null.NullString, Me.SimpleGalleryBase.GallerySettings.SortBy, Me.SimpleGalleryBase.GallerySettings.SortDirection)
            dlGallery.DataBind()

        End Sub

        Public Sub Save()

            If (Page.IsValid()) Then

                Dim objPhotoController As New PhotoController
                Dim objTagController As New TagController

                For Each item As DataListItem In dlGallery.Items
                    Dim litPhotoID As Literal = CType(item.FindControl("litPhotoID"), Literal)
                    Dim objPhoto As PhotoInfo = objPhotoController.Get(Convert.ToInt32(litPhotoID.Text))

                    If Not (objPhoto Is Nothing) Then

                        Dim txtTitle As TextBox = CType(item.FindControl("txtTitle"), TextBox)
                        Dim txtDescription As TextBox = CType(item.FindControl("txtDescription"), TextBox)
                        Dim txtTags As TextBox = CType(item.FindControl("txtTags"), TextBox)

                        objPhoto.Name = txtTitle.Text
                        objPhoto.Description = txtDescription.Text
                        objPhotoController.Update(objPhoto)

                        objTagController.DeletePhotoTag(objPhoto.PhotoID)

                        Dim tags As String() = txtTags.Text.Split(","c)
                        For Each tag As String In tags
                            If (tag <> "") Then
                                Dim objTag As TagInfo = objTagController.Get(SimpleGalleryBase.ModuleId, tag.ToLower())

                                If (objTag Is Nothing) Then
                                    objTag = New TagInfo
                                    objTag.ModuleID = SimpleGalleryBase.ModuleId
                                    objTag.Name = tag
                                    objTag.NameLowered = tag.ToLower()
                                    objTag.Usages = 0
                                    objTag.TagID = objTagController.Add(objTag)
                                End If

                                objTagController.Add(objPhoto.PhotoID, objTag.TagID)
                            End If
                        Next

                    End If
                Next
            End If

        End Sub

#End Region

#Region " Public Properties "

        Public Property AlbumID() As Integer
            Get
                Return _albumID
            End Get
            Set(ByVal Value As Integer)
                _albumID = Value
            End Set
        End Property

        Public Property BatchID() As String
            Get
                Return _batchID
            End Get
            Set(ByVal Value As String)
                _batchID = Value
            End Set
        End Property

        Public Property TagID() As Integer
            Get
                Return _tagID
            End Get
            Set(ByVal Value As Integer)
                _tagID = Value
            End Set
        End Property

#End Region

#Region " Protected Methods "

        Public Function GetClearTagsMessage() As String

            Return Localization.GetString("ClearTags", Me.LocalResourceFile)

        End Function

#End Region

#Region " Event Handlers "

        Private Sub Page_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load

            Try

                phBatchOperations.Visible = SimpleGalleryBase.GallerySettings.EnableTags
                plBatchOperations.Text = Localization.GetString("plBatchOperations", Me.LocalResourceFile)
                lblTagsAdd.Text = Localization.GetString("cmdTagsAdd", Me.LocalResourceFile)
                lblTagsClear.Text = Localization.GetString("cmdTagsClear", Me.LocalResourceFile)

            Catch exc As Exception    'Module failed to load
                ProcessModuleLoadException(Me, exc)
            End Try

        End Sub

        Private Sub Page_PreRender(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.PreRender

            Try

                Dim commandsAdd As String = ""
                Dim commandsClear As String = ""
                For Each item As DataListItem In dlGallery.Items
                    Dim txtTags As TextBox = CType(item.FindControl("txtTags"), TextBox)

                    If Not (txtTags Is Nothing) Then
                        commandsAdd = commandsAdd & "document.getElementById('" & txtTags.ClientID & "').value = " & "document.getElementById('" & txtTagsAdd.ClientID & "').value;" & vbCrLf
                        commandsClear = commandsClear & "document.getElementById('" & txtTags.ClientID & "').value = '';" & vbCrLf
                    End If
                Next

                litCommandsAdd.Text = commandsAdd
                litCommandsDelete.Text = commandsClear

            Catch exc As Exception    'Module failed to load
                ProcessModuleLoadException(Me, exc)
            End Try

        End Sub

        Private Sub dlGallery_OnItemDataBound(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.DataListItemEventArgs) Handles dlGallery.ItemDataBound

            If (e.Item.ItemType = ListItemType.Item Or e.Item.ItemType = ListItemType.AlternatingItem) Then

                Dim objPhoto As PhotoInfo = CType(e.Item.DataItem, PhotoInfo)
                Dim phThumbnail As WebControls.Literal = CType(e.Item.FindControl("phThumbnail"), WebControls.Literal)

                If Not (phThumbnail Is Nothing) Then

                    phThumbnail.Text = "<table class=""photo-frame"">" _
                                            & "<tr><td class=""topx--""></td><td class=""top-x-""></td><td class=""top--x""></td></tr>" _
                                            & "<tr><td class=""midx--""></td><td valign=""top""><img src=""" & GetPhotoPath(objPhoto) & """ class=""photo_198"" width=""" & GetPhotoWidth(objPhoto) & """ height=""" & GetPhotoHeight(objPhoto) & """><br></a></td><td class=""mid--x""></td></tr>" _
                                            & "<tr><td class=""botx--""></td><td class=""bot-x-""></td><td class=""bot--x""></td></tr>" _
                                            & "</table>"

                End If

                Dim txtTitle As WebControls.TextBox = CType(e.Item.FindControl("txtTitle"), WebControls.TextBox)
                Dim txtDescription As WebControls.TextBox = CType(e.Item.FindControl("txtDescription"), WebControls.TextBox)
                Dim txtTags As WebControls.TextBox = CType(e.Item.FindControl("txtTags"), WebControls.TextBox)

                txtTitle.Text = objPhoto.Name
                txtDescription.Text = objPhoto.Description
                txtTags.Text = objPhoto.Tags

                Dim lblTitle As WebControls.Label = CType(e.Item.FindControl("lblTitle"), WebControls.Label)
                Dim valTitle As WebControls.RequiredFieldValidator = CType(e.Item.FindControl("valTitle"), WebControls.RequiredFieldValidator)
                Dim lblDescription As WebControls.Label = CType(e.Item.FindControl("lblDescription"), WebControls.Label)
                Dim lblTags As WebControls.Label = CType(e.Item.FindControl("lblTags"), WebControls.Label)

                lblTitle.Text = Localization.GetString("Title", Me.LocalResourceFile)
                valTitle.ErrorMessage = Localization.GetString("valTitle.ErrorMessage", Me.LocalResourceFile)
                lblDescription.Text = Localization.GetString("Description", Me.LocalResourceFile)
                lblTags.Text = Localization.GetString("Tags", Me.LocalResourceFile)

                Dim litPhotoID As WebControls.Literal = CType(e.Item.FindControl("litPhotoID"), WebControls.Literal)

                litPhotoID.Text = objPhoto.PhotoID.ToString()

                Dim phTags As WebControls.PlaceHolder = CType(e.Item.FindControl("phTags"), WebControls.PlaceHolder)
                phTags.Visible = SimpleGalleryBase.GallerySettings.EnableTags

                Dim valTags As RequiredFieldValidator = CType(e.Item.FindControl("valTags"), RequiredFieldValidator)
                valTags.Enabled = SimpleGalleryBase.GallerySettings.RequireTags
                valTags.ErrorMessage = Localization.GetString("valTags.ErrorMessage", Me.LocalResourceFile)

            End If

        End Sub

#End Region

    End Class

End Namespace