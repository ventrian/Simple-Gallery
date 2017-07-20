'
' Simple Gallery for DotNetNuke -  http://www.dotnetnuke.com
' Copyright (c) 2002-2007
' by Ventrian ( sales@ventrian.com ) ( http://www.ventrian.com )
'

Imports System
Imports System.Data

Imports DotNetNuke.Common.Utilities
Imports DotNetNuke.Framework

Imports Ventrian.SimpleGallery.Data

Namespace Ventrian.SimpleGallery.Entities

    Public Class TemplateController

#Region " Public Methods "

        Public Function [Get](ByVal moduleID As Integer, ByVal name As String) As TemplateInfo

            Return CType(CBO.FillObject(DataProvider.Instance().GetTemplate(moduleID, name), GetType(TemplateInfo)), TemplateInfo)

        End Function

        Public Function Add(ByVal objTemplate As TemplateInfo) As Integer

            Return CType(DataProvider.Instance().AddTemplate(objTemplate.ModuleID, objTemplate.Name, objTemplate.Template), Integer)

        End Function

        Public Sub Update(ByVal objTemplate As TemplateInfo)

            DataProvider.Instance().UpdateTemplate(objTemplate.TemplateID, objTemplate.ModuleID, objTemplate.Name, objTemplate.Template)

        End Sub

#End Region

    End Class

End Namespace
