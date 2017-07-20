<%@ Control Language="vb" AutoEventWireup="false" CodeBehind="ViewSearch.ascx.vb" Inherits="Ventrian.SimpleGallery.ViewSearch" %>
<%@ Register TagPrefix="SimpleGallery" TagName="GalleryMenu" Src="Controls\GalleryMenu.ascx"%>
<%@ Register TagPrefix="SimpleGallery" TagName="ViewPhotos" Src="Controls\ViewPhotos.ascx"%>
<SimpleGallery:GalleryMenu id="ucGalleryMenu" runat="server" />
<SimpleGallery:ViewPhotos id="ucViewPhotos" runat="server" />
