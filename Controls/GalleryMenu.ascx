<%@ Control Language="vb" AutoEventWireup="false" CodeBehind="GalleryMenu.ascx.vb" Inherits="Ventrian.SimpleGallery.Controls.GalleryMenu" %>
<table cellpadding="0" cellspacing="0" width="100%">
<tr>
	<td align="left">
		<asp:Repeater ID="rptBreadCrumbs" Runat="server">
		<ItemTemplate><a href='<%# DataBinder.Eval(Container.DataItem, "Url") %>' class="NormalBold"><%# DataBinder.Eval(Container.DataItem, "Caption") %></a></ItemTemplate>
		<SeparatorTemplate>&nbsp;&#187;&nbsp;</SeparatorTemplate>
		</asp:Repeater>
	</td>
	<td align="right">
	    <asp:PlaceHolder ID="phSearch" runat="server" EnableViewState="false"><asp:HyperLink ID="lnkSearch" Runat="server" CssClass="CommandButton" EnableViewState="False" />&nbsp;</asp:PlaceHolder><asp:HyperLink ID="lnkTags" Runat="server" CssClass="CommandButton" EnableViewState="False" />&nbsp;<asp:HyperLink ID="lnkRSS" Runat="server" EnableViewState="False"><asp:Image ID="imgRSS" Runat="server" ImageUrl="~/DesktopModules/SimpleGallery/images/xml.gif" EnableViewState="False" ImageAlign="AbsBottom" BorderWidth="0" /></asp:HyperLink>
	</td>
</tr>
<tr runat="server" id="trSeparator" visible="false" enableviewstate="False">
	<td height="10px" colspan="2"></td>
</tr>
</table>

<asp:Panel HorizontalAlign="Center" ID="pnlCommandBar" Runat="server">
	<asp:HyperLink ID="lnkAddNewPhoto" Runat="server" CssClass="CommandButton" EnableViewState="False" />&nbsp;<asp:HyperLink ID="lnkAddNewAlbum" Runat="server" CssClass="CommandButton" EnableViewState="False" />&nbsp;<asp:HyperLink ID="lnkApprovePhotos" Runat="server" CssClass="CommandButton" EnableViewState="False" />&nbsp;<asp:HyperLink ID="lnkBulkEdit" Runat="server" CssClass="CommandButton" EnableViewState="False" />
</asp:Panel>