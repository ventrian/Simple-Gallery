<%@ Control Language="vb" AutoEventWireup="false" CodeBehind="TagCloudOptions.ascx.vb" Inherits="Ventrian.SimpleGallery.TagCloudOptions" %>
<%@ Register TagPrefix="dnn" TagName="Label" Src="~/controls/LabelControl.ascx" %>
<table cellSpacing="0" cellPadding="2" summary="Edit Links Design Table" border="0">
	<TR vAlign="top">
		<TD class="SubHead" width="150"><dnn:label id="plModuleID" runat="server" resourcekey="Module" suffix=":" controlname="drpModuleID"></dnn:label></TD>
		<TD align="left" width="325"><asp:dropdownlist id="drpModuleID" Runat="server" Width="325" datavaluefield="ModuleID" datatextfield="ModuleTitle" 
							CssClass="NormalTextBox" AutoPostBack="True"></asp:dropdownlist></TD>
	</TR>
	<TR vAlign="top">
		<TD class="SubHead" width="150">
			<dnn:label id="plAlbums" runat="server" resourcekey="Albums" suffix=":" controlname="drpAlbums"></dnn:label>
		</TD>
		<TD align="left" width="325">
			<asp:DropDownList ID="drpAlbums" Runat="server" DataTextField="CaptionIndented" DataValueField="AlbumID" />
		</TD>
	</TR>	
	<TR vAlign="top">
		<TD class="SubHead" width="150">
			<dnn:label id="plMaxCount" runat="server" resourcekey="MaxCount" suffix=":" controlname="txtMaxCount"></dnn:label></TD>
		<TD align="left" width="325">
			<asp:textbox id="txtMaxCount" cssclass="NormalTextBox" runat="server" width="300" maxlength="50"></asp:textbox>
				<asp:requiredfieldvalidator id="valMaxCount" cssclass="NormalRed" runat="server" resourcekey="valMaxCount.ErrorMessage"
					controltovalidate="txtMaxCount" errormessage="<br>Max Count is Required" display="Dynamic"></asp:requiredfieldvalidator>
				<asp:CompareValidator id="valMaxCountIsNumber" Runat="server" controltovalidate="txtMaxCount"
					errormessage="<br>Max Count must be a Number" Type="Integer" Operator="DataTypeCheck" Display="Dynamic"
					resourceKey="valMaxCountIsNumber.ErrorMessage" CssClass="NormalRed"></asp:CompareValidator></TD>
	</TR>
</table>
