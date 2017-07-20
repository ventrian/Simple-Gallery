<%@ Control Language="vb" AutoEventWireup="false" CodeBehind="SearchBox.ascx.vb" Inherits="Ventrian.SimpleGallery.SearchBox" %>
<%@ Register TagPrefix="SimpleGallery" TagName="GalleryMenu" Src="Controls\GalleryMenu.ascx"%>
<SimpleGallery:GalleryMenu id="ucGalleryMenu" runat="server" />
<br />
<div align="center">
    <asp:Panel ID="pnlSearch" runat="server" DefaultButton="btnSearch">
        <asp:TextBox ID="txtSearch" runat="server" CssClass="NormalTextBox" ValidationGroup="SearchBox" /><asp:Button ID="btnSearch" runat="server" CssClass="Normal" ResourceKey="Search" Text="Search" ValidationGroup="SearchBox" />
    </asp:Panel>
</div>