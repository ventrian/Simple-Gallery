<%@ Control Language="vb" AutoEventWireup="false" CodeBehind="EditPhotos.ascx.vb" Inherits="Ventrian.SimpleGallery.Controls.EditPhotos" %>
<%@ Register TagPrefix="dnn" TagName="Label" Src="~/controls/LabelControl.ascx" %>
<style>
.photo-frame .topx-- { background-image: url(<%= ResolveUrl("../images/borders/" & BorderStyle & "/frame-topx--.gif") %>); }
.photo-frame .top-x- { background-image: url(<%= ResolveUrl("../images/borders/" & BorderStyle & "/frame-top-x-.gif") %>); }
.photo-frame .top--x { background-image: url(<%= ResolveUrl("../images/borders/" & BorderStyle & "/frame-top--x.gif") %>); }
.photo-frame .midx-- { background-image: url(<%= ResolveUrl("../images/borders/" & BorderStyle & "/frame-midx--.gif") %>); }
.photo-frame .mid--x { background-image: url(<%= ResolveUrl("../images/borders/" & BorderStyle & "/frame-mid--x.gif") %>); }
.photo-frame .botx-- { background-image: url(<%= ResolveUrl("../images/borders/" & BorderStyle & "/frame-botx--.gif") %>); }
.photo-frame .bot-x- { background-image: url(<%= ResolveUrl("../images/borders/" & BorderStyle & "/frame-bot-x-.gif") %>); }
.photo-frame .bot--x { background-image: url(<%= ResolveUrl("../images/borders/" & BorderStyle & "/frame-bot--x.gif") %>); }
</style>

<script>
function AddTags()
{
	<asp:literal id="litCommandsAdd" runat="server" />
	return false;
}

function ClearTags()
{
    if( confirm("<%= GetClearTagsMessage() %>") )
    {
	    <asp:literal id="litCommandsDelete" runat="server" />
    }
	return false;
}
</script>

<asp:PlaceHolder ID="phBatchOperations" runat="Server">
<table cellspacing="0" cellpadding="0" width="100%" summary="Select Album Design Table">
<tr valign="top">
	<td align="center">
		<asp:label id="plBatchOperations" runat="server" CssClass="SubHead" EnableViewState="False" />&nbsp;<asp:TextBox id="txtTagsAdd" Runat="server" Width="200px" CssClass="NormalTextBox" />&nbsp;<a href="#" onClick="return AddTags();" class="CommandButton"><asp:Label ID="lblTagsAdd" Runat="server" EnableViewState="False" /></a>&nbsp;<a href="#" onClick="return ClearTags();" class="CommandButton"><asp:Label ID="lblTagsClear" Runat="server" EnableViewState="False" /></a>
	</td>
</tr>
</table>
<hr size="1" noshade />
</asp:PlaceHolder>

<asp:datalist id="dlGallery" runat="server" RepeatColumns="3" RepeatDirection="Horizontal" HorizontalAlign="Center" ItemStyle-VerticalAlign="Top" ItemStyle-HorizontalAlign="Center" CellPadding="2" CellSpacing="4">
<ItemTemplate>
	<asp:Literal ID="litPhotoID" Runat="server" Visible="False" />
	<asp:Literal ID="phThumbnail" Runat="server" />
	<div align="left" class="Normal">
		&nbsp;<asp:Label ID="lblTitle" Runat="server" EnableViewState="False" CssClass="Normal" />
	</div>
	<asp:TextBox id="txtTitle" Runat="server" Width="190px" CssClass="NormalTextBox" />
	<asp:RequiredFieldValidator ID="valTitle" Runat="server" ControlToValidate="txtTitle" Display="Dynamic" CssClass="NormalRed" />
	<div align="left">
		&nbsp;<asp:Label ID="lblDescription" Runat="server" EnableViewState="False" CssClass="Normal" />
	</div>
	<asp:TextBox id="txtDescription" Runat="server" Width="190px" TextMode="MultiLine" CssClass="NormalTextBox" />
	<asp:PlaceHolder ID="phTags" Runat="server">
		<div align="left">
			&nbsp;<asp:Label ID="lblTags" Runat="server" EnableViewState="False" CssClass="Normal" />
		</div>
		<asp:TextBox id="txtTags" Runat="server" Width="190px" CssClass="NormalTextBox" />
		<asp:RequiredFieldValidator ID="valTags" runat="server" Display="Dynamic" ControlToValidate="txtTags" CssClass="NormalRed" />
	</asp:PlaceHolder>
</ItemTemplate>
</asp:datalist>
