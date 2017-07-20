'
' Simple Gallery for DotNetNuke -  http://www.dotnetnuke.com
' Copyright (c) 2002-2007
' by Ventrian ( sales@ventrian.com ) ( http://www.ventrian.com )
'

Imports System.Collections

Imports Ventrian.SimpleGallery.Common

Namespace Ventrian.SimpleGallery.Entities

    Public Class SettingController

#Region " Private Methods "

        Private Function GetDefaultSetting(ByVal key As String) As String

            Select Case key

                Case Constants.SETTING_WIDTH
                    Return Constants.DEFAULT_WIDTH.ToString()

                Case Constants.SETTING_THUMBNAIL_WIDTH
                    Return Constants.DEFAULT_THUMBNAIL_WIDTH.ToString()

                Case Constants.SETTING_PHOTOS_PER_ROW
                    Return Constants.DEFAULT_PHOTOS_PER_ROW.ToString()

                Case Else
                    Return ""

            End Select

        End Function

#End Region

#Region " Public Methods "

        Public Function GetSetting(ByVal key As String, ByVal settings As Hashtable) As String

            If (settings.Contains(key)) Then
                Return settings(key).ToString()
            Else
                Return GetDefaultSetting(key)
            End If

        End Function

#End Region

    End Class

End Namespace
