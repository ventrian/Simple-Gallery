<%@ Page Language="vb" AutoEventWireup="false" CodeBehind="SlideShowPopup.aspx.vb" Inherits="Ventrian.SimpleGallery.SlideShowPopup" %>
<%@ Register TagPrefix="dnn" Namespace="DotNetNuke.Common.Controls" Assembly="DotNetNuke" %>
<html>
<head>
<TITLE><asp:literal id="litTitle" runat="server" /></TITLE>
<LINK id="_DotNetNuke_DesktopModules_SimpleGallery" rel="stylesheet" type="text/css" href="<%= ResolveUrl("module.css") %>"></LINK>
<asp:placeholder id="CSS" runat="server"></asp:placeholder>
<meta http-equiv="imagetoolbar" content="no">
</head>
<body style="MARGIN: 2px" class="popupBG">

<style type="text/css">
.photo-frame .topx-- { background-image: url(<%= FormatBorderPath("frame-topx--.gif") %>); }
.photo-frame .top-x- { background-image: url(<%= FormatBorderPath("frame-top-x-.gif") %>); }
.photo-frame .top--x { background-image: url(<%= FormatBorderPath("frame-top--x.gif") %>); }
.photo-frame .midx-- { background-image: url(<%= FormatBorderPath("frame-midx--.gif") %>); }
.photo-frame .mid--x { background-image: url(<%= FormatBorderPath("frame-mid--x.gif") %>); }
.photo-frame .botx-- { background-image: url(<%= FormatBorderPath("frame-botx--.gif") %>); }
.photo-frame .bot-x- { background-image: url(<%= FormatBorderPath("frame-bot-x-.gif") %>); }
.photo-frame .bot--x { background-image: url(<%= FormatBorderPath("frame-bot--x.gif") %>); }
</style>

<dnn:Form id="Form1" runat="server" ENCTYPE="multipart/form-data" style=”height:100%;>
<div align="center">
<TABLE border="0" width="100%">
<tr>
	<td class="Normal" width="33%">
		<asp:HyperLink ID="lnkPrevious" Runat="server" CssClass="CommandButton">&#171; Previous</asp:HyperLink>
		&nbsp;|&nbsp;
		<asp:Label ID="lblPageCount" Runat="server" EnableViewState="False" CssClass="Normal" />
		&nbsp;|&nbsp;
		<asp:HyperLink ID="lnkNext" Runat="server" CssClass="CommandButton">Next &#187;</asp:HyperLink>
	</td>
	<td align="center" width="33%">
		<asp:HyperLink ID="lnkDownload" Runat="server" CssClass="CommandButton" Target="_Blank"></asp:HyperLink>
	</td>
	<td align="right" width="33%">
		<asp:HyperLink ID="lnkClose" Runat="server" CssClass="CommandButton" EnableViewState="False"></asp:HyperLink>
	</td>
</tr>
<tr>
	<td align="center" colspan="3">
        <table border="0" cellpadding="0" cellspacing="0" class="photo-frame">
		<tr>
			<td class="topx--"></td>
			<td class="top-x-"></td>
			<td class="top--x"></td>
		</tr>
		<tr>
			<td class="midx--"></td>
            <td><asp:HyperLink id="lnkPhoto" runat="server"><asp:Image ID="imgPhoto" Runat="server" EnableViewState="False" cssclass="photo_198" /></asp:HyperLink></td>
			<td class="mid--x"></td>
		</tr>
		<tr>
			<td class="botx--"></td>
			<td class="bot-x-"></td>
			<td class="bot--x"></td>
		</tr>
        </table>
	</td>
</tr>
<tr>
	<td align="center" colspan="3">
		<asp:Label ID="lblName" Runat="server" EnableViewState="False" CssClass="NormalBold" />
	</td>
</tr>
<tr>
	<td align="center" colspan="3"><asp:Label ID="lblDescription" Runat="server" EnableViewState="False" CssClass="Normal" /></td>
</tr>
</table>
</div>
</dnn:Form>
</body>
</html>
