<%@ Control Language="vb" AutoEventWireup="false" CodeBehind="EditTag.ascx.vb" Inherits="Ventrian.SimpleGallery.EditTag" %>
<%@ Register TagPrefix="dnn" TagName="Label" Src="~/controls/LabelControl.ascx" %>
<%@ Register TagPrefix="dnn" TagName="SectionHead" Src="~/controls/SectionHeadControl.ascx" %>
<%@ Register TagPrefix="SimpleGallery" TagName="GalleryMenu" Src="Controls\GalleryMenu.ascx"%>
<SimpleGallery:GalleryMenu id="ucGalleryMenu" runat="server" ShowCommandBar="False" />
<table class="Settings" cellspacing="2" cellpadding="2" summary="Edit Tags Design Table"
	border="0" width="100%">
	<tr>
		<td valign="top">
            <dnn:sectionhead id="dshTags" cssclass="Head" runat="server" includerule="True" resourcekey="EditTag"
	            section="tblEditTag" text="Edit Tag"></dnn:sectionhead>
            <div align="center">
                <table id="tblEditTag" cellspacing="0" cellpadding="2" summary="Edit Tags Design Table"
                    border="0" runat="server" width="600">
                <tr>
                    <td align="center">
                    
                        <table id="tblTag" cellspacing="0" cellpadding="2" width="100%" summary="Tag Settings Design Table"
					        border="0" runat="server">
					        <tr valign="top">
						        <td width="25"><IMG height=1 src='<%= Page.ResolveUrl("~/Images/Spacer.gif") %>' width=25></td>
						        <td class="SubHead" nowrap="nowrap" width="150">
							        <dnn:label id="plName" runat="server" resourcekey="Name" suffix=":" controlname="txtName"></dnn:label></td>
						        <td align="left" width="100%">
							        <asp:textbox id="txtName" cssclass="NormalTextBox" runat="server" width="325" columns="30"
								        maxlength="255"></asp:textbox>
							        <asp:requiredfieldvalidator id="valName" cssclass="NormalRed" runat="server" resourcekey="valName" display="Dynamic"
								        errormessage="<br>You Must Enter a Valid Name" controltovalidate="txtName"></asp:requiredfieldvalidator></td>
					        </tr>
				        </table>
				        
                        <p>
                            <br />
	                        <asp:linkbutton id="cmdUpdate" resourcekey="cmdUpdate" runat="server" cssclass="CommandButton" text="Update"
		                        borderstyle="none" />
	                        &nbsp;
	                        <asp:linkbutton id="cmdCancel" resourcekey="cmdCancel" runat="server" cssclass="CommandButton" text="Cancel"
		                        causesvalidation="False" borderstyle="none" />
	                        &nbsp;
	                        <asp:linkbutton id="cmdDelete" resourcekey="cmdDelete" runat="server" cssclass="CommandButton" text="Delete"
		                        causesvalidation="False" borderstyle="none" />
                        </p>

                    </td>
                </tr>
                </table>
            </div>
        </td>
    </tr>
</table>
