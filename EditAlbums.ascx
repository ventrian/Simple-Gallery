<%@ Control Language="vb" AutoEventWireup="false" CodeBehind="EditAlbums.ascx.vb" Inherits="Ventrian.SimpleGallery.EditAlbums" %>
<%@ Import Namespace="DotNetNuke.Common" %>
<%@ Import Namespace="DotNetNuke.Common.Utilities" %>
<%@ Register TagPrefix="SimpleGallery" TagName="GalleryMenu" Src="Controls\GalleryMenu.ascx"%>
<%@ Register TagPrefix="dnn" TagName="SectionHead" Src="~/controls/SectionHeadControl.ascx" %>
<SimpleGallery:GalleryMenu id="ucGalleryMenu" runat="server" ShowCommandBar="False" />

<table class="Settings" cellspacing="2" cellpadding="2" summary="Edit Album Design Table"
	border="0" width="100%">
	<tr>
		<td valign="top">
            <dnn:sectionhead id="dshAlbums" cssclass="Head" runat="server" includerule="True" resourcekey="EditAlbums"
	            section="tblEditAlbums" text="Edit Albums"></dnn:sectionhead>
            <div align="center">
                <table id="tblEditAlbums" cellspacing="0" cellpadding="2" width="500" summary="Edit Albums Design Table"
                    border="0" runat="server">
                <tr>
                    <td align="center">
                        <asp:datagrid id="grdAlbums" Border="0" cellpadding="4" cellspacing="0" Width="100%" AutoGenerateColumns="false" EnableViewState="false" runat="server" summary="Pages Design Table" GridLines="None">
	                        <Columns>
		                        <asp:TemplateColumn ItemStyle-Width="20">
			                        <ItemTemplate>
				                        <asp:HyperLink NavigateUrl='<%# NavigateURL(Me.TabID, "EditAlbum", "mid=" & Me.ModuleID.ToString(), "AlbumID=" & DataBinder.Eval(Container.DataItem,"AlbumID").ToString()) %>' runat="server" ID="Hyperlink1">
				                        <asp:Image ImageUrl="~/images/edit.gif" AlternateText="Edit" runat="server" ID="Hyperlink1Image" resourcekey="Edit"/></asp:HyperLink>
			                        </ItemTemplate>
		                        </asp:TemplateColumn>
		                        <asp:BoundColumn HeaderText="Caption" DataField="CaptionIndented" ItemStyle-CssClass="Normal" HeaderStyle-Cssclass="NormalBold" ItemStyle-HorizontalAlign="Left" HeaderStyle-HorizontalAlign="Left" />
		                        <asp:BoundColumn HeaderText="IsPublic" DataField="IsPublic" ItemStyle-CssClass="Normal" HeaderStyle-Cssclass="NormalBold" ItemStyle-HorizontalAlign="Left" HeaderStyle-HorizontalAlign="Left" ItemStyle-Width="100px" />
	                        </Columns>
                        </asp:datagrid>
                        <asp:Label ID="lblNoAlbums" Runat="server" CssClass="Normal" />
                        <p>
                            <br />
	                        <asp:linkbutton id="cmdAddAlbum" resourcekey="AddAlbum" runat="server" cssclass="CommandButton" text="Add Album" causesvalidation="False" borderstyle="none" />
	                        &nbsp;
	                        <asp:linkbutton id="cmdEditSortOrder" resourcekey="EditSortOrder" runat="server" cssclass="CommandButton" text="Edit Sort Order" causesvalidation="False" borderstyle="none" />
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


