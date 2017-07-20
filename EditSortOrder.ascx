<%@ Control Language="vb" AutoEventWireup="false" CodeBehind="EditSortOrder.ascx.vb" Inherits="Ventrian.SimpleGallery.EditSortOrder" %>
<%@ Register TagPrefix="dnn" TagName="Label" Src="~/controls/LabelControl.ascx" %>
<%@ Register TagPrefix="dnn" TagName="HelpButton" Src="~/controls/HelpButtonControl.ascx" %>
<%@ Register TagPrefix="dnn" TagName="SectionHead" Src="~/controls/SectionHeadControl.ascx" %>
<%@ Register TagPrefix="SimpleGallery" TagName="GalleryMenu" Src="Controls\GalleryMenu.ascx"%>

<SimpleGallery:GalleryMenu id="ucGalleryMenu" runat="server" ShowCommandBar="False" />
<table class="Settings" cellspacing="2" cellpadding="2" summary="Edit Sort Order Design Table"
	border="0" width="100%">
	<tr>
		<td valign="top">
            <dnn:sectionhead id="dshAlbums" cssclass="Head" runat="server" includerule="True" resourcekey="EditSortOrder"
	            section="tblEditSortOrder" text="Edit Albums"></dnn:sectionhead>
            <div align="center">
                <table id="tblEditSortOrder" cellspacing="0" cellpadding="2" summary="Edit Albums Design Table"
                    border="0" runat="server">
                <tr>
                    <td align="center">
                        <table cellspacing="0" cellpadding="2" summary="Album Sort Order Design Table">
                        <tr valign="top">
	                        <td class="SubHead" width="150"><dnn:label id="plParentAlbum" runat="server" resourcekey="ParentAlbum" controlname="drpParentAlbum" suffix=":"></dnn:label></td>
	                        <td align="left"><asp:DropDownList ID="drpParentAlbum" Runat="server" DataTextField="CaptionIndented" DataValueField="AlbumID" AutoPostBack="True" Width="300" /></td>
                        </tr>
                        <tr valign="top">
	                        <td class="SubHead" width="150"><dnn:label id="plChildAlbums" runat="server" resourcekey="ChildAlbums" controlname="lstChildAlbums" suffix=":"></dnn:label></td>
	                        <td align="left">
		                        <asp:Label ID="lblNoAlbums" Runat="server" CssClass="Normal" Visible="False" />
		                        <asp:Panel ID="pnlSortOrder" Runat="server">
			                        <table cellspacing="0" cellpadding="0" width="450">
			                        <tr>
				                        <td>	
					                        <asp:listbox id="lstAlbums" runat="server" rows="22" datatextfield="Caption" datavaluefield="AlbumID" cssclass="NormalTextBox" width="300"></asp:listbox>
					                        <asp:Label ID="lblAlbumUpdated" Runat="server" CssClass="NormalBold" Visible="False" ResourceKey="AlbumUpdated" EnableViewState="False" />
				                        </td>
				                        <td>&nbsp;</td>
				                        <td width="150px" valign="top">
					                        <table summary="Tabs Design Table">
						                        <tr>
							                        <td colspan="2" valign="top" class="SubHead">
								                        <asp:label id="lblMovePage" runat="server" resourcekey="MovePage">Move Page</asp:label>
								                        <hr noshade size="1">
							                        </td>
						                        </tr>
						                        <tr>
							                        <td valign="top" width="10%">
								                        <asp:imagebutton id="cmdUp" resourcekey="cmdUp.Help" runat="server" alternatetext="Move Tab Up In Current Level" commandname="up" imageurl="~/images/up.gif"></asp:imagebutton>
							                        </td>
							                        <td valign="top" width="90%">
								                        <dnn:HelpButton id="hbtnUpHelp" resourcekey="cmdUp" runat="server" /></dnn:helpbutton>
							                        </td>
						                        </tr>
						                        <tr>
							                        <td valign="top" width="10%">
								                        <asp:imagebutton id="cmdDown" resourcekey="cmdDown.Help" runat="server" alternatetext="Move Tab Down In Current Level" commandname="down" imageurl="~/images/dn.gif"></asp:imagebutton>
							                        </td>
							                        <td valign="top" width="90%">
								                        <dnn:helpbutton id="hbtnDownHelp" resourcekey="cmdDown" runat="server" /></dnn:helpbutton>
							                        </td>
						                        </tr>
						                        <tr>
							                        <td colspan="2" height="25">&nbsp;</td>
						                        </tr>
						                        <tr>
							                        <td colspan="2" valign="top" class="SubHead">
								                        <asp:label id="lblActions" runat="server" resourcekey="Actions">Actions</asp:label>
								                        <hr noshade size="1">
							                        </td>
						                        </tr>
						                        <tr>
							                        <td valign="top" width="10%">
								                        <asp:imagebutton id="cmdEdit" resourcekey="cmdEdit.Help" runat="server" alternatetext="Edit Tab" imageurl="~/images/edit.gif"></asp:imagebutton>
							                        </td>
							                        <td valign="top" width="90%">
								                        <dnn:helpbutton id="hbtnEditHelp" resourcekey="cmdEdit" runat="server" /></dnn:helpbutton>
							                        </td>
						                        </tr>
						                        <tr>
							                        <td valign="top" width="10%">
								                        <asp:imagebutton id="cmdView" resourcekey="cmdView.Help" runat="server" alternatetext="View Tab" imageurl="~/images/view.gif"></asp:imagebutton>
							                        </td>
							                        <td valign="top" width="90%">
								                        <dnn:helpbutton id="hbtnViewHelp" resourcekey="cmdView" runat="server" /></dnn:helpbutton>
							                        </td>
						                        </tr>
					                        </table>
				                        </td>
			                        </tr>
			                        </table>
		                        </asp:Panel>
	                        </td>
                        </tr>
                        </table>
                        <p>
                            <br />
	                        <asp:linkbutton id="cmdUpdate" resourcekey="UpdateSortOrder" runat="server" cssclass="CommandButton" text="Update Sort Order" borderstyle="none" />
	                        &nbsp;
	                        <asp:linkbutton id="cmdReturnToGallery" resourcekey="ReturnToAlbums" runat="server" cssclass="CommandButton" text="Return To Edit Albums" causesvalidation="False" borderstyle="none" />
                        </p>
                </td>
            </tr>
            </table>
        </div>
    </td>
</tr>
</table>
