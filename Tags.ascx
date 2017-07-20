<%@ Control Language="vb" AutoEventWireup="false" CodeBehind="Tags.ascx.vb" Inherits="Ventrian.SimpleGallery.Tags" %>
<%@ Register TagPrefix="SimpleGallery" TagName="GalleryMenu" Src="Controls\GalleryMenu.ascx"%>
<SimpleGallery:GalleryMenu id="ucGalleryMenu" runat="server" />
<asp:Literal ID="litCloudMarkup" runat="server"></asp:Literal>
