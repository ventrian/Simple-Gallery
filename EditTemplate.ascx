<%@ Control Language="vb" AutoEventWireup="false" CodeBehind="EditTemplate.ascx.vb" Inherits="Ventrian.SimpleGallery.EditTemplate" %>
<%@ Register TagPrefix="dnn" TagName="Label" Src="~/controls/LabelControl.ascx" %>
<%@ Register TagPrefix="dnn" TagName="SectionHead" Src="~/controls/SectionHeadControl.ascx" %>
<table cellpadding="0" cellspacing="0" width="100%">
	<tr>
		<td align="left">
			<asp:Repeater ID="rptBreadCrumbs" Runat="server" EnableViewState="False">
				<ItemTemplate><a href='<%# DataBinder.Eval(Container.DataItem, "Url") %>' class="NormalBold"><%# DataBinder.Eval(Container.DataItem, "Caption") %></a></ItemTemplate>
				<SeparatorTemplate>&nbsp;&#187;&nbsp;</SeparatorTemplate>
			</asp:Repeater>
		</td>
	</tr>
</table>

<table class="Settings" cellspacing="2" cellpadding="2" summary="Edit Tags Design Table"
	border="0" width="100%">
	<tr>
		<td valign="top">
            <dnn:sectionhead id="dshEditTemplate" cssclass="Head" runat="server" includerule="True" resourcekey="EditTemplate"
	            section="tblEditTemplate" text="Edit Template"></dnn:sectionhead>
            <div align="center">
                <table id="tblEditTemplate" cellspacing="0" cellpadding="2" summary="Edit Template Design Table"
                    border="0" runat="server" width="600">
                <tr>
                    <td align="center">

                        <table cellspacing="0" cellpadding="2" width="600" summary="Edit Template Design Table">
	                        <tr valign="top">
		                        <td class="SubHead" width="150"><dnn:label id="plTemplate" runat="server" controlname="drpTemplates" suffix=":"></dnn:label></td>
		                        <td align="left">
			                        <asp:DropDownList ID="drpTemplates" Runat="server" AutoPostBack="True" Width="300" />
		                        </td>
	                        </tr>
	                        <tr>
		                        <td class="SubHead" width="150"><dnn:label id="plTemplateBody" runat="server" controlname="txtTemplateBody" suffix=":"></dnn:label></td>
		                        <td align="left">
			                        <asp:textbox id="txtTemplate" runat="server" cssclass="NormalTextBox" width="300" rows="20"
				                        textmode="MultiLine"></asp:textbox>
		                        </td>
	                        </tr>
                        </table>
                        <p>
                            <br />
	                        <asp:linkbutton id="cmdUpdate" resourcekey="cmdUpdate" runat="server" CssClass="CommandButton" Text="Update"
		                        BorderStyle="none"></asp:linkbutton>&nbsp;
	                        <asp:linkbutton id="cmdCancel" resourcekey="cmdCancel" runat="server" CssClass="CommandButton" Text="Cancel"
		                        BorderStyle="none" CausesValidation="False"></asp:linkbutton>&nbsp;
	                        <asp:linkbutton id="cmdRestoreDefault" resourcekey="cmdRestoreDefault" runat="server" CssClass="CommandButton"
		                        Text="Restore Default Template" BorderStyle="none" CausesValidation="False"></asp:linkbutton>
                        </p>

                    </td>
                </tr>
                </table>
            </div>
            
            <dnn:sectionhead id="dshTokens" cssclass="Head" runat="server" includerule="True" resourcekey="Tokens"
	            section="tblTokens" text="Tokens"></dnn:sectionhead>
            <div align="center">
                <table id="tblTokens" cellspacing="0" cellpadding="2" summary="Tokens Design Table"
                    border="0" runat="server" width="600">
                <tr>
                    <td align="center">
                        <asp:Repeater ID="rptTemplateTokens" Runat="server">
	                        <HeaderTemplate>
		                        <table cellspacing="0" cellpadding="2" width="600" summary="Edit Template Token Design Table">
	                        </HeaderTemplate>
	                        <ItemTemplate>
		                        <tr valign="top">
			                        <td class="SubHead" width="150" align="left">[<%# Container.DataItem.ToString() %>]</td>
			                        <td class="Normal" align="left">
				                        <%# GetLocalizedValue(Container.DataItem.ToString()) %>
			                        </td>
		                        </tr>
	                        </ItemTemplate>
	                        <FooterTemplate>
		                        </table>
	                        </FooterTemplate>
                        </asp:Repeater>
                    </td>
                </tr>
                </table>
            </div>
            
        </td>
    </tr>
</table>



