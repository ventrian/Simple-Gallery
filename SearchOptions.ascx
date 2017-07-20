<%@ Control Language="vb" AutoEventWireup="false" CodeBehind="SearchOptions.ascx.vb" Inherits="Ventrian.SimpleGallery.SearchOptions" %>
<%@ Register TagPrefix="dnn" TagName="Label" Src="~/controls/LabelControl.ascx" %>
<%@ Register TagPrefix="dnn" TagName="SectionHead" Src="~/controls/SectionHeadControl.ascx" %>
<table cellspacing="0" cellpadding="2" summary="Edit Links Design Table" border="0">
	<tr valign="top">
		<td class="SubHead" width="150"><dnn:label id="plModuleID" runat="server" resourcekey="Module" suffix=":" controlname="drpModuleID"></dnn:label></td>
		<td align="left" width="325"><asp:dropdownlist id="drpModuleID" Runat="server" Width="325" datavaluefield="ModuleID" datatextfield="ModuleTitle" 
							CssClass="NormalTextBox" /></td>
	</tr>
</table>
<br />
<dnn:sectionhead id="dshTemplateSettings" cssclass="Head" runat="server" section="tblTemplateSettings" resourcekey="TemplateSettings" IsExpanded="False" />
<table id="tblTemplateSettings" cellspacing="2" cellpadding="2" summary="Template Settings Design Table" border="0" runat="server">
<tr>
	<td class="SubHead" width="200"><dnn:label id="plSearchTemplate" runat="server" resourcekey="SearchTemplate" suffix=":" controlname="txtSearchTemplate"></dnn:label></td>
	<td><asp:TextBox ID="txtSearchTemplate" Runat="server" TextMode="MultiLine" CssClass="NormalTextBox" Width="250px" Rows="10" /></td>
</tr>
<tr>
    <td class="SubHead">[BUTTON]</td>
    <td><asp:Label ID="lblButton" runat="server" ResourceKey="Button" CssClass="Normal" EnableViewState="false" /></td>
</tr>
<tr>
    <td class="SubHead">[LINKBUTTON]</td>
    <td><asp:Label ID="lblLinkButton" runat="server" ResourceKey="LinkButton" CssClass="Normal" EnableViewState="false" /></td>
</tr>
<tr>
    <td class="SubHead">[TEXTBOX]</td>
    <td><asp:Label ID="lblTextBox" runat="server" ResourceKey="TextBox" CssClass="Normal" EnableViewState="false" /></td>
</tr>
</table>
