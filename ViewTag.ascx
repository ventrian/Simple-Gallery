<%@ Control Language="vb" AutoEventWireup="false" CodeBehind="ViewTag.ascx.vb" Inherits="Ventrian.SimpleGallery.ViewTag" %>
<%@ Register TagPrefix="SimpleGallery" TagName="GalleryMenu" Src="Controls\GalleryMenu.ascx"%>
<%@ Register TagPrefix="SimpleGallery" TagName="ViewPhotos" Src="Controls\ViewPhotos.ascx"%>
<SimpleGallery:GalleryMenu id="ucGalleryMenu" runat="server" />
<SimpleGallery:ViewPhotos id="ucViewPhotos" runat="server" />
