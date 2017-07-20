<%@ Control Language="vb" AutoEventWireup="false" CodeBehind="ViewGallery.ascx.vb" Inherits="Ventrian.SimpleGallery.ViewGallery" %>
<%@ Register TagPrefix="SimpleGallery" TagName="GalleryMenu" Src="Controls\GalleryMenu.ascx"%>
<%@ Register TagPrefix="SimpleGallery" TagName="ViewAlbums" Src="Controls\ViewAlbums.ascx"%>
<%@ Register TagPrefix="SimpleGallery" TagName="ViewPhotos" Src="Controls\ViewPhotos.ascx"%>

<SimpleGallery:GalleryMenu id="ucGalleryMenu" runat="server" />

<div align="center" runat="server" id="divDescription">
	<asp:Label ID="lblDescription" Runat="server" EnableViewState="False" Visible="False" CssClass="Normal" />
</div>

<SimpleGallery:ViewAlbums id="ucViewAlbums" runat="server" />
<SimpleGallery:ViewPhotos id="ucViewPhotos" runat="server" />

<div align="right" id="divViewCart" runat="server" enableviewstate="false" />

