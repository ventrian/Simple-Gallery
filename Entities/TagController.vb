'
' Simple Gallery for DotNetNuke -  http://www.dotnetnuke.com
' Copyright (c) 2002-2007
' by Ventrian ( sales@ventrian.com ) ( http://www.ventrian.com )
'

Imports System
Imports System.Data
Imports System.Xml

Imports DotNetNuke
Imports DotNetNuke.Common
Imports DotNetNuke.Common.Utilities
Imports DotNetNuke.Entities.Modules

Imports Ventrian.SimpleGallery.Data

Namespace Ventrian.SimpleGallery.Entities

    Public Class TagController

#Region " Private Methods "

        Private Sub RemoveCache(ByVal tagID As Integer)

            Dim objTag As TagInfo = [Get](tagID)

            If Not (objTag Is Nothing) Then
                RemoveCache(objTag.ModuleID, objTag.TagID)
            End If

        End Sub

        Private Sub RemoveCache(ByVal moduleID As Integer, ByVal nameLowered As String)

            If Not (DataCache.GetCache("Tag-" & moduleID.ToString() & "-" & nameLowered) Is Nothing) Then
                DataCache.RemoveCache("Tag-" & moduleID.ToString() & "-" & nameLowered)
            End If

        End Sub

#End Region

#Region " Public Methods "

        Public Function [Get](ByVal tagID As Integer) As TagInfo

            Return CType(CBO.FillObject(DataProvider.Instance().GetTag(tagID), GetType(TagInfo)), TagInfo)

        End Function

        Public Function [Get](ByVal moduleID As Integer, ByVal nameLowered As String) As TagInfo

            Dim objTag As TagInfo = DataCache.GetCache("Tag-" & moduleID.ToString() & "-" & nameLowered)

            If (objTag Is Nothing) Then
                objTag = CType(CBO.FillObject(DataProvider.Instance().GetTagByName(moduleID, nameLowered), GetType(TagInfo)), TagInfo)
                If Not (objTag Is Nothing) Then
                    DataCache.SetCache("Tag-" & moduleID.ToString() & "-" & nameLowered, objTag)
                End If
            End If

            Return objTag

        End Function

        Public Function List(ByVal moduleID As Integer, ByVal albumID As Integer, ByVal maxCount As Integer, ByVal showApprovedOnly As Boolean) As ArrayList

            Return CBO.FillCollection(DataProvider.Instance().ListTag(moduleID, albumID, maxCount, showApprovedOnly), GetType(TagInfo))

        End Function

        Public Function Add(ByVal objTag As TagInfo) As Integer

            Return CType(DataProvider.Instance().AddTag(objTag.ModuleID, objTag.Name, objTag.NameLowered), Integer)

        End Function

        Public Sub Update(ByVal objTag As TagInfo)

            RemoveCache(objTag.ModuleID, objTag.NameLowered)
            DataProvider.Instance().UpdateTag(objTag.TagID, objTag.ModuleID, objTag.Name, objTag.NameLowered, objTag.Usages)

        End Sub

        Public Sub Delete(ByVal tagID As Integer)

            RemoveCache(tagID)
            DataProvider.Instance().DeleteTag(tagID)

        End Sub

        Public Sub DeletePhotoTag(ByVal photoID As Integer)

            Dim objPhotoController As New PhotoController()
            Dim objPhoto As PhotoInfo = objPhotoController.Get(photoID)

            If Not (objPhoto Is Nothing) Then
                For Each tag As String In objPhoto.Tags.Split(","c)
                    RemoveCache(objPhoto.ModuleID, tag.ToLower())
                Next
            End If
            DataProvider.Instance().DeletePhotoTag(photoID)

        End Sub

        Public Sub DeletePhotoTagByTag(ByVal tagID As Integer)

            RemoveCache(tagID)
            DataProvider.Instance().DeletePhotoTag(tagID)

        End Sub

        Public Sub Add(ByVal photoID As Integer, ByVal tagID As Integer)

            RemoveCache(tagID)
            DataProvider.Instance().AddPhotoTag(photoID, tagID)

        End Sub

#End Region

    End Class

End Namespace

