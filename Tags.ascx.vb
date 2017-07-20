'
' Simple Gallery for DotNetNuke -  http://www.dotnetnuke.com
' Copyright (c) 2002-2007
' by Ventrian ( sales@ventrian.com ) ( http://www.ventrian.com )
'

Imports DotNetNuke.Common
Imports DotNetNuke.Common.Utilities
Imports DotNetNuke.Services.Localization
Imports DotNetNuke.Services.Exceptions

Imports Ventrian.SimpleGallery.Common
Imports Ventrian.SimpleGallery.Entities

Namespace Ventrian.SimpleGallery

    Partial Public Class Tags
        Inherits SimpleGalleryBase

#Region " Private Methods "

        Private Sub BindBreadCrumbs()

            ucGalleryMenu.AddCrumb(Localization.GetString("AllAlbums", LocalResourceFile), NavigateURL())
            ucGalleryMenu.AddCrumb(Localization.GetString("Tags", LocalResourceFile), NavigateURL(Me.TabId, "", "Tags=" & TabModuleId))

        End Sub

        Private Sub BindTags()

            Dim objTagController As New TagController
            Dim objTags As ArrayList = objTagController.List(Me.ModuleId, Convert.ToInt32(Me.GallerySettings.AlbumFilter), Me.GallerySettings.TagCount, True)

            objTags.Sort()

            If (objTags.Count > 0) Then

                Dim minWeight As Decimal = Decimal.MaxValue, maxWeight As Decimal = Decimal.MinValue
                Dim FontScale() As String = {"weight5", "weight4", "weight3", "weight2", "weight1"}
                Const SpacerMarkup As String = " "      'The markup injected between each item in the cloud

                For Each objTag As TagInfo In objTags
                    Dim numProductsDec As Decimal = Convert.ToDecimal(objTag.Usages)

                    If numProductsDec < minWeight Then minWeight = numProductsDec
                    If numProductsDec > maxWeight Then maxWeight = numProductsDec
                Next

                Dim scaleUnitLength As Decimal = (maxWeight - minWeight + 1) / Convert.ToDecimal(FontScale.Length)

                litCloudMarkup.Text &= "<div id=""tagCloud"" class=""tagCloud"">"
                For Each objTag As TagInfo In objTags
                    Dim numProductsDec As Decimal = Convert.ToDecimal(objTag.Usages)

                    Dim scaleValue As Integer = CType(Decimal.Truncate((numProductsDec - minWeight) / scaleUnitLength), Int32)
                    litCloudMarkup.Text &= String.Format("<a class=""{1}"" href=""{0}"">{2}</a>{3}", _
                                                GetTagUrl(objTag, Me.TabId, TabModuleId), FontScale(scaleValue), objTag.Name, SpacerMarkup)
                Next
                litCloudMarkup.Text &= "</div>"

            Else

                litCloudMarkup.Text = "<div class=""normal"">" & Localization.GetString("NoTags", Me.LocalResourceFile) & "</div>"

            End If

        End Sub

        Private Function GetTagUrl(ByVal objTag As TagInfo, ByVal tabID As Integer, ByVal moduleID As Integer) As String

            If (AllLetters(objTag.Name) = True) Then
                Return NavigateURL(tabID, "", "Tag=" & objTag.Name, "Tags=" & moduleID.ToString())
            Else
                Return NavigateURL(tabID, "", "TagID=" & objTag.TagID.ToString(), "Tags=" & moduleID.ToString())
            End If

        End Function

        Private Function AllLetters(ByVal txt As String) As Boolean

            Dim reg As New Regex("^[A-Za-z]+$")
            Dim ok As Boolean = True

            If Not (reg.IsMatch(txt)) Then
                ok = False
            Else
                ok = True
            End If

            Return ok

        End Function

#End Region

#Region " Event Handlers "

        Private Sub Page_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load

            Try

                BindBreadCrumbs()
                BindTags()

            Catch exc As Exception    'Module failed to load
                ProcessModuleLoadException(Me, exc)
            End Try

        End Sub

#End Region

    End Class

End Namespace