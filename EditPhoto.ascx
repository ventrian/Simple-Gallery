<%@ Control Language="vb" AutoEventWireup="false" CodeBehind="EditPhoto.ascx.vb" Inherits="Ventrian.SimpleGallery.EditPhoto" %>
<%@ Register TagPrefix="dnn" TagName="Label" Src="~/controls/LabelControl.ascx" %>
<%@ Register TagPrefix="dnn" TagName="SectionHead" Src="~/controls/SectionHeadControl.ascx" %>
<%@ Register TagPrefix="SimpleGallery" TagName="GalleryMenu" Src="Controls\GalleryMenu.ascx"%>
<SimpleGallery:GalleryMenu id="ucGalleryMenu" runat="server" ShowCommandBar="False" />


<table class="Settings" cellspacing="2" cellpadding="2" summary="Edit Album Design Table"
	border="0" width="100%">
	<tr>
		<td valign="top">
			<asp:panel id="pnlSettings" runat="server" cssclass="WorkPanel" visible="True">
				<dnn:sectionhead id="dshPhoto" cssclass="Head" runat="server" includerule="True" resourcekey="PhotoSettings"
					section="tblPhoto" text="Photo Settings"></dnn:sectionhead>
				<asp:label id="lblPhotoSettingsHelp" cssclass="Normal" runat="server" resourcekey="PhotoSettingsDescription"
								enableviewstate="False"></asp:label>
			    <div align="center">

                    <table id="tblPhoto" cellspacing="0" cellpadding="2" width="600" summary="Album Settings Design Table"
					    border="0" runat="server">
	                    <tr valign="top">
		                    <td class="SubHead" width="150"><dnn:label id="plAlbum" runat="server" controlname="drpAlbums" suffix=":"></dnn:label></td>
		                    <td>
			                    <asp:DropDownList ID="drpAlbums" Runat="server" DataTextField="CaptionIndented" DataValueField="AlbumID" CssClass="NormalTextBox" />
			                    <asp:linkbutton id="cmdEditAlbums" resourcekey="cmdEditAlbums" runat="server" CssClass="CommandButton"
				                    Text="Edit Albums" BorderStyle="none" CausesValidation="False"></asp:linkbutton>
			                    <asp:requiredfieldvalidator id="valAlbums" resourcekey="Albums.ErrorMessage" runat="server" CssClass="NormalRed"
				                    ControlToValidate="drpAlbums" ErrorMessage="You Must Select An Album For The Photo" Display="Dynamic" InitialValue="-1"></asp:requiredfieldvalidator>
		                    </td>
	                    </tr>
	                    <tr valign="top" id="trName" runat="server">
		                    <td class="SubHead" width="150"><dnn:label id="plName" runat="server" controlname="txtName" suffix=":"></dnn:label></td>
		                    <td width="450">
			                    <asp:textbox id="txtName" cssclass="NormalTextBox" width="300" maxlength="100" runat="server" />
			                    <asp:requiredfieldvalidator id="valName" resourcekey="Name.ErrorMessage" runat="server" CssClass="NormalRed"
				                    ControlToValidate="txtName" ErrorMessage="You Must Enter A Name For The Photo" Display="Dynamic" Enabled="False"></asp:requiredfieldvalidator>
		                    </td>
	                    </tr>
	                    <tr valign="top" id="trDescription" runat="server">
		                    <td class="SubHead" width="150"><dnn:label id="plDescription" runat="server" controlname="txtDescription" suffix=":"></dnn:label></td>
		                    <td>
			                    <asp:textbox id="txtDescription" cssclass="NormalTextBox" width="300" rows="5" textmode="MultiLine"
				                    runat="server" />
		                    </td>
	                    </tr>
	                    <tr valign="top" id="trTags" runat="server">
		                    <td class="SubHead" width="150"><dnn:label id="plTags" runat="server" controlname="txtTags" suffix=":"></dnn:label></td>
		                    <td width="450">
			                    <asp:textbox id="txtTags" cssclass="NormalTextBox" width="300" maxlength="255" runat="server" />
		                    </td>
	                    </tr>
	                    <tr valign="middle">
		                    <td class="SubHead" width="150"><dnn:label id="plPhoto" runat="server" controlname="txtTags" suffix=":"></dnn:label></td>
		                    <td width="450">
		                        <asp:PlaceHolder ID="phReplace" runat="server">
		                        <table cellpadding="0" cellspacing="0">
		                        <tr>
		                            <td><asp:Literal ID="litPhoto" runat="server" /></td>
		                            <td valign="middle">
		                                <asp:linkbutton id="cmdReplace" resourcekey="cmdReplace" runat="server" CssClass="CommandButton" Text="Replace Photo" BorderStyle="none" CausesValidation="false"></asp:linkbutton>
		                            </td>
		                        </tr>
		                        </table>
		                        </asp:PlaceHolder>
		                        <asp:PlaceHolder ID="phReplaceUpload" runat="server" Visible="false">
		                            <div align="center" style="width: 300px;">
		                                <br />
		                                <asp:FileUpload ID="fuReplace" runat="server" />&nbsp;<br />
		                                <asp:CheckBox ID="chkAddAsBefore" Text="Combine with existing photo" runat="server" CssClass="Normal" ResourceKey="chkAddAsBefore" /><br />
                                        <asp:linkbutton id="cmdUploadReplace" runat="server" CssClass="CommandButton" Text="Upload" BorderStyle="none" CausesValidation="false"></asp:linkbutton>&nbsp;
                                        <asp:linkbutton id="cmdCancelReplace" resourcekey="cmdCancel" runat="server" CssClass="CommandButton" Text="Cancel" BorderStyle="none" CausesValidation="false"></asp:linkbutton>
		                            </div>
		                        </asp:PlaceHolder>
		                    </td>
	                    </tr>
                    </table>

            </div>
			</asp:panel>
		</td>
	</tr>
</table>
<p align="center">
    <br />
	<asp:linkbutton id="cmdUpdate" resourcekey="cmdUpdate" runat="server" CssClass="CommandButton" Text="Update"
		BorderStyle="none"></asp:linkbutton>&nbsp;
	<asp:linkbutton id="cmdCancel" resourcekey="cmdCancel" runat="server" CssClass="CommandButton" Text="Cancel"
		BorderStyle="none" CausesValidation="False"></asp:linkbutton>&nbsp;
	<asp:linkbutton id="cmdDelete" resourcekey="cmdDelete" runat="server" CssClass="CommandButton" Text="Delete"
		BorderStyle="none" CausesValidation="False"></asp:linkbutton>&nbsp;
	<asp:linkbutton id="cmdSetDefault" resourcekey="cmdSetDefault" runat="server" CssClass="CommandButton"
		Text="Set Default Album Photo" BorderStyle="none" CausesValidation="False"></asp:linkbutton>
</p>
