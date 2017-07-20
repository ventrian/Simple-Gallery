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

    Partial Public Class ViewGallery
        Inherits SimpleGalleryBase

#Region " Private Members "

        Private _albumID As Integer = Null.NullInteger

#End Region

#Region " Private Methods "

        Private Sub ReadQueryString()

            If Not (GallerySettings.AlbumFilter = "-1") Then
                Try
                    _albumID = Convert.ToInt32(GallerySettings.AlbumFilter)
                Catch
                End Try
            End If

            If Not (Request("AlbumID") Is Nothing) Then
                Dim param As String = Request("AlbumID")
                If (param.IndexOf("-") <> -1) Then
                    Dim arr As String() = param.Split(Convert.ToChar("-"))
                    If (arr(0) = Me.ModuleId.ToString()) Then
                        If (_albumID = Null.NullInteger) Then
                            _albumID = Convert.ToInt32(arr(1))
                        Else
                            ' Check that the album belongs in the filter
                            Dim objAlbumController As New AlbumController
                            Dim objAlbums As ArrayList = objAlbumController.List(Me.ModuleId, _albumID, True, True, AlbumSortType.Caption, SortDirection.ASC)

                            For Each objAlbum As AlbumInfo In objAlbums
                                If (objAlbum.AlbumID = Convert.ToInt32(arr(1))) Then
                                    _albumID = Convert.ToInt32(arr(1))
                                    Exit For
                                End If
                            Next
                        End If
                    End If
                End If
            End If

        End Sub

        Private Sub BindBreadCrumbs()

            ucGalleryMenu.AddCrumb(Localization.GetString("AllAlbums", LocalResourceFile), NavigateURL())

        End Sub

        Private Sub BindPhotos()

            ucGalleryMenu.AddCrumb(Localization.GetString("AllAlbums", LocalResourceFile), NavigateURL())

            Dim viewAlbum As Boolean = False
            If (_albumID <> Null.NullInteger) Then
                viewAlbum = True
            End If

            If (viewAlbum = False) Then


            Else

                ucViewPhotos.AlbumID = _albumID

                If (Request("Page") <> "") Then
                    Try
                        ucViewPhotos.PageNumber = Convert.ToInt32(Request("Page"))
                    Catch
                    End Try
                End If

                Dim objAlbumController As New AlbumController
                Dim objAlbum As AlbumInfo = objAlbumController.Get(_albumID)

                If Not (objAlbum Is Nothing) Then
                    If (objAlbum.Description.Trim.Length > 0) Then
                        lblDescription.Text = objAlbum.Description
                        lblDescription.Visible = True
                    Else
                        lblDescription.Visible = False
                    End If

                    If (Request("AlbumID") IsNot Nothing) Then
                        Dim pageTitle As String = Localization.GetString("PageTitle", LocalResourceFile)
                        If (pageTitle.IndexOf("{0}") <> -1) Then
                            Me.BasePage.Title = String.Format(pageTitle, objAlbum.Caption)
                        End If
                    End If

                End If

                If (lblDescription.Visible = False) Then
                    divDescription.Visible = False
                End If

                While Not objAlbum Is Nothing
                    If (GallerySettings.AlbumFilter <> objAlbum.AlbumID.ToString()) Then
                        If (GallerySettings.UseAlbumAnchors) Then
                            ucGalleryMenu.InsertCrumb(1, objAlbum.Caption, NavigateURL(Me.TabId, "", "AlbumID=" & Me.ModuleId.ToString() & "-" & objAlbum.AlbumID.ToString()) & "#SimpleGallery-" & Me.ModuleId)
                        Else
                            ucGalleryMenu.InsertCrumb(1, objAlbum.Caption, NavigateURL(Me.TabId, "", "AlbumID=" & Me.ModuleId.ToString() & "-" & objAlbum.AlbumID.ToString()))
                        End If
                    End If

                    If (objAlbum.ParentAlbumID = Null.NullInteger) Then
                        objAlbum = Nothing
                    Else
                        objAlbum = objAlbumController.Get(objAlbum.ParentAlbumID)
                    End If
                End While

                ucViewAlbums.AlbumID = _albumID


            End If

        End Sub

#End Region

#Region " Protected Methods "

        Protected Function RssUrl(ByVal albumID As String) As String

            Return Me.ResolveUrl("RSS.aspx?t=" & Me.TabId & "&m=" & Me.ModuleId & "&tm=" & Me.TabModuleId & "&a=" & albumID & "&portalid=" & Me.PortalId)

        End Function

#End Region

#Region " Event Handlers "

        Private Sub Page_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load

            Try

                ReadQueryString()
                BindPhotos()

                If (GallerySettings.IncludeViewCart) Then

                    divViewCart.Visible = True
                    Dim objLiteral As New Literal
                    objLiteral.Text = "<iframe src=""" & ResolveUrl("~/DesktopModules/SimpleGallery/ViewCart.ashx?mid=" & ModuleId.ToString()) & """ frameborder=""0"" width=""150px"" height=""80px""></iframe>"
                    divViewCart.Controls.Add(objLiteral)

                End If

            Catch exc As Exception    'Module failed to load
                ProcessModuleLoadException(Me, exc)
            End Try

        End Sub

#End Region

    End Class

End Namespace