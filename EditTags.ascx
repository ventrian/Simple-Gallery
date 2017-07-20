<%@ Control Language="vb" AutoEventWireup="false" CodeBehind="EditTags.ascx.vb" Inherits="Ventrian.SimpleGallery.EditTags" %>
<%@ Import Namespace="DotNetNuke.Common" %>
<%@ Import Namespace="DotNetNuke.Common.Utilities" %>
<%@ Register TagPrefix="SimpleGallery" TagName="GalleryMenu" Src="Controls\GalleryMenu.ascx"%>
<%@ Register TagPrefix="dnn" TagName="SectionHead" Src="~/controls/SectionHeadControl.ascx" %>
<SimpleGallery:GalleryMenu id="ucGalleryMenu" runat="server" ShowCommandBar="False" />

<table class="Settings" cellspacing="2" cellpadding="2" summary="Edit Tags Design Table"
	border="0" width="100%">
	<tr>
		<td valign="top">
            <dnn:sectionhead id="dshTags" cssclass="Head" runat="server" includerule="True" resourcekey="EditTags"
	            section="tblEditTags" text="Edit Albums"></dnn:sectionhead>
            <div align="center">
                <table id="tblEditTags" cellspacing="0" cellpadding="2" summary="Edit Tags Design Table"
                    border="0" runat="server" width="600">
                <tr>
                    <td align="center">
                    
                        <asp:datagrid id="grdTags" Border="0" cellpadding="4" cellspacing="0" Width="600" AutoGenerateColumns="false" EnableViewState="false" runat="server" summary="Pages Design Table" GridLines="None">
	                        <Columns>
		                        <asp:TemplateColumn ItemStyle-Width="20">
			                        <ItemTemplate>
				                        <asp:HyperLink NavigateUrl='<%# NavigateURL(Me.TabID, "EditTag", "mid=" & Me.ModuleID.ToString(), "TagID=" & DataBinder.Eval(Container.DataItem,"TagID").ToString()) %>' runat="server" ID="Hyperlink1">
				                        <asp:Image ImageUrl="~/images/edit.gif" AlternateText="Edit" runat="server" ID="Hyperlink1Image" resourcekey="Edit"/></asp:HyperLink>
			                        </ItemTemplate>
		                        </asp:TemplateColumn>
		                        <asp:BoundColumn HeaderText="Name" DataField="Name" ItemStyle-CssClass="Normal" HeaderStyle-Cssclass="NormalBold" ItemStyle-HorizontalAlign="Left" HeaderStyle-HorizontalAlign="Left" />
	                        </Columns>
                        </asp:datagrid>
                        <asp:Label ID="lblNoTags" Runat="server" CssClass="Normal" />
                        <p>
                            <br />
	                        <asp:linkbutton id="cmdAddTag" resourcekey="AddTag" runat="server" cssclass="CommandButton" text="Add Tag" causesvalidation="False" borderstyle="none" />
	                        &nbsp;
	                        <asp:linkbutton id="cmdReturnToGallery" resourcekey="ReturnToGallery" runat="server" cssclass="CommandButton" text="Return To Gallery" causesvalidation="False" borderstyle="none" />
                        </p>

                    </td>
                </tr>
                </table>
            </div>
        </td>
    </tr>
</table>
