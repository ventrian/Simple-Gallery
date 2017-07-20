<%@ Control Language="vb" AutoEventWireup="false" CodeBehind="BulkEdit.ascx.vb" Inherits="Ventrian.SimpleGallery.BulkEdit" %>
<%@ Register TagPrefix="SimpleGallery" TagName="EditPhotos" Src="Controls\EditPhotos.ascx"%>
<%@ Register TagPrefix="SimpleGallery" TagName="GalleryMenu" Src="Controls\GalleryMenu.ascx"%>
<SimpleGallery:GalleryMenu id="ucGalleryMenu" runat="server" ShowCommandBar="False" ShowSeparator="True" />
<table cellspacing="0" cellpadding="0" width="600" summary="Wizard Design Table">
	<tr>
		<td width="50" height="50" align="center" valign="middle">
			<asp:Image ID="imgStep" Runat="server" ImageUrl="~/DesktopModules/SimpleGallery/Images/iconStep3.gif"
				Width="48" Height="48" />
		</td>
		<td>
			<asp:Label ID="lblTitle" Runat="server" CssClass="NormalBold" ResourceKey="Title" /><br>
			<asp:Label ID="lblDescription" Runat="server" CssClass="Normal" ResourceKey="Description" />
		</td>
	</tr>
</table>
<hr size="1">
<SimpleGallery:EditPhotos id="ucEditPhotos" runat="server" />
<p align="center">	
    <br />
	<asp:ImageButton ID="imgSave" Runat="server" ImageUrl="~\DesktopModules\SimpleGallery\images\iconSave.gif"
		ImageAlign="AbsBottom" />
	<asp:linkbutton id="cmdSave" runat="server" cssclass="CommandButton" ResourceKey="cmdSave" text="Save this batch"
		borderstyle="none" />
</p>
