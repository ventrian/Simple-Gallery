'
' Simple Gallery for DotNetNuke -  http://www.dotnetnuke.com
' Copyright (c) 2002-2007
' by Ventrian ( sales@ventrian.com ) ( http://www.ventrian.com )
'

Imports System.Drawing
Imports System.Drawing.Drawing2d
Imports System.Drawing.Imaging
Imports System.Web
Imports System.Web.Services

Imports DotNetNuke.Common
Imports DotNetNuke.Common.Utilities

Namespace Ventrian.SimpleGallery

    Public Class ImageHandlerLarge
        Implements System.Web.IHttpHandler

#Region " Private Members "

#End Region

#Region " Properties "

        ReadOnly Property IsReusable() As Boolean Implements IHttpHandler.IsReusable
            Get
                Return False
            End Get
        End Property

#End Region

#Region " Event Handlers "

        Public Sub ProcessRequest(ByVal context As HttpContext) Implements IHttpHandler.ProcessRequest

        End Sub

#End Region

    End Class

End Namespace