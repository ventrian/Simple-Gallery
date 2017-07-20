<%@ Control Language="vb" AutoEventWireup="false" CodeBehind="RandomPhotoOptions.ascx.vb" Inherits="Ventrian.SimpleGallery.RandomPhotoOptions" %>
<%@ Register TagPrefix="dnn" TagName="Skin" Src="~/controls/SkinControl.ascx" %>
<%@ Register TagPrefix="dnn" TagName="Label" Src="~/controls/LabelControl.ascx" %>
<%@ Register TagPrefix="dnn" TagName="SectionHead" Src="~/controls/SectionHeadControl.ascx" %>
<dnn:SectionHead id="dshDetails" cssclass="Head" runat="server" text="Basic Settings" section="tblBasic" resourcekey="BasicSettings" />
<table id="tblBasic" cellspacing="2" cellpadding="2" summary="Basic Settings Design Table" border="0" runat="server">
<tr>
	<td class="SubHead" width="200"><dnn:label id="plModuleID" runat="server" resourcekey="Module" suffix=":" controlname="drpModuleID"></dnn:label></TD>
	<td><asp:dropdownlist id="drpModuleID" Runat="server" Width="250px" datavaluefield="ModuleID" datatextfield="ModuleTitle" CssClass="NormalTextBox" AutoPostBack="True"></asp:dropdownlist></td>
</tr>
<tr>
	<td class="SubHead" width="200"><dnn:label id="plAlbums" runat="server" resourcekey="Albums" suffix=":" controlname="drpAlbums"></dnn:label></td>
	<td><asp:DropDownList ID="drpAlbums" Runat="server" DataTextField="CaptionIndented" DataValueField="AlbumID" CssClass="NormalTextBox" Width="250px" /></td>
</tr>	
<tr>
	<td class="SubHead" width="200"><dnn:label id="plMode" runat="server" resourcekey="Mode" suffix=":" controlname="rdoMode"></dnn:label></td>
	<td><asp:radiobuttonlist id="rdoMode" Runat="server" Width="250px" CssClass="Normal" AutoPostBack="False" RepeatDirection="Horizontal" RepeatLayout="Flow"></asp:radiobuttonlist></td>
</tr>
<tr>
	<td class="SubHead" width="200"><dnn:label id="plDisplay" runat="server" resourcekey="Display" suffix=":" controlname="rdoDisplay"></dnn:label></td>
	<td><asp:radiobuttonlist id="rdoDisplay" Runat="server" Width="250px" CssClass="Normal" AutoPostBack="true" RepeatDirection="Horizontal" RepeatLayout="Flow"></asp:radiobuttonlist></td>
</tr>
<tr>
	<td class="SubHead" width="200"><dnn:label id="plMaxCount" runat="server" resourcekey="MaxCount" suffix=":" controlname="txtMaxCount"></dnn:label></td>
	<td>
		<asp:textbox id="txtMaxCount" cssclass="NormalTextBox" runat="server" width="250px" maxlength="50"></asp:textbox>
			<asp:requiredfieldvalidator id="valMaxCount" cssclass="NormalRed" runat="server" resourcekey="valMaxCount.ErrorMessage"
				controltovalidate="txtMaxCount" errormessage="<br>Max Count is Required" display="Dynamic"></asp:requiredfieldvalidator>
			<asp:CompareValidator id="valMaxCountIsNumber" Runat="server" controltovalidate="txtMaxCount"
				errormessage="<br>Max Count must be a Number" Type="Integer" Operator="DataTypeCheck" Display="Dynamic"
				resourceKey="valMaxCountIsNumber.ErrorMessage" CssClass="NormalRed"></asp:CompareValidator>
	</td>
</tr>
<tr>
	<td class="SubHead" width="200"><dnn:label id="plTagFilter" runat="server" resourcekey="TagFilter" suffix=":" controlname="txtTagFilter"></dnn:label></td>
	<td>
		<asp:textbox id="txtTagFilter" cssclass="NormalTextBox" runat="server" width="250px" maxlength="255"></asp:textbox>
	</td>
</tr>
<tr>
	<td class="SubHead" width="200"><dnn:label id="plRepeatDirection" runat="server" resourcekey="RepeatDirection" suffix=":" controlname="drpRepeatDirection"></dnn:label></td>
	<td><asp:dropdownlist id="drpRepeatDirection" Runat="server" Width="250px" CssClass="NormalTextBox" AutoPostBack="False"></asp:dropdownlist></td>
</tr>
<tr>
	<td class="SubHead" width="200"><dnn:label id="plRepeatColumns" runat="server" resourcekey="RepeatColumns" suffix=":" controlname="txtRepeatColumns"></dnn:label></td>
	<td>
		<asp:textbox id="txtRepeatColumns" cssclass="NormalTextBox" runat="server" width="250px" maxlength="50"></asp:textbox>
		<asp:requiredfieldvalidator id="valRepeatColumns" cssclass="NormalRed" runat="server" resourcekey="valRepeatColumns.ErrorMessage"
			controltovalidate="txtRepeatColumns" errormessage="<br>Repeat Columns is Required" display="Dynamic"></asp:requiredfieldvalidator>
		<asp:CompareValidator id="valRepeatColumnsIsNumber" Runat="server" controltovalidate="txtRepeatColumns"
			errormessage="<br>Repeat Columns must be a Number" Type="Integer" Operator="DataTypeCheck" Display="Dynamic"
			resourceKey="valRepeatColumnsIsNumber.ErrorMessage" CssClass="NormalRed"></asp:CompareValidator>
	</td>
</tr>
<tr>
	<td class="SubHead" width="200"><dnn:label id="plBorderStyle" runat="server" resourcekey="BorderStyle" controlname="drpBorderStyle" suffix=":"></dnn:label></td>
	<td><asp:DropDownList id="drpBorderStyle" Runat="server" Width="250px" CssClass="NormalTextBox"></asp:DropDownList></td>
</tr>
</table>
<br />
<dnn:SectionHead id="dshAdvanced" cssclass="Head" runat="server" text="Advanced Settings" section="tblAdvanced" resourcekey="AdvancedSettings" IsExpanded="False" />
<table id="tblAdvanced" cellspacing="2" cellpadding="2" summary="Appearance Design Table" border="0" runat="server">
<tr>
	<td class="SubHead" width="200"><dnn:label id="plLaunchSlideshow" runat="server" resourcekey="LaunchSlideshow" suffix=":" controlname="chkLaunchSlideshow"></dnn:label></td>
	<td><asp:CheckBox id="chkLaunchSlideshow" Runat="server"></asp:CheckBox></td>
</tr>
<tr>
	<td class="SubHead" width="200"><dnn:label id="plAlbumSlideshow" runat="server" resourcekey="AlbumSlideshow" suffix=":" controlname="chkAlbumSlideshow"></dnn:label></td>
	<td><asp:CheckBox id="chkAlbumSlideshow" Runat="server"></asp:CheckBox></td>
</tr>
<tr>
	<td class="SubHead" width="200"><dnn:label id="plIncludeJQuery" runat="server" suffix=":" controlname="chkIncludeJQuery"></dnn:label></td>
	<td><asp:CheckBox id="chkIncludeJQuery" Runat="server"></asp:CheckBox></td>
</tr>
</table>
<br />
<dnn:sectionhead id="dshCompressionSettings" cssclass="Head" runat="server" section="tblCompressionSettings" resourcekey="CompressionSettings" IsExpanded="False" />
<table id="tblCompressionSettings" cellspacing="2" cellpadding="2" summary="Template Settings Design Table" border="0" runat="server">
<tr>
	<td class="SubHead" width="200"><dnn:label id="plCompressionType" runat="server" resourcekey="CompressionType" suffix=":" controlname="drpCompressionType"></dnn:label></td>
	<td><asp:DropDownList id="drpCompressionType" Runat="server" CssClass="NormalTextBox" width="250px"></asp:DropDownList></td>
</tr>
<tr>
    <td class="SubHead" width="200"><dnn:label id="plThumbnailType" runat="server" suffix=":" controlname="rdoThumbnailType"></dnn:label></td>
	<td><asp:radiobuttonlist id="rdoThumbnailType" Runat="server" Width="250px" CssClass="Normal" AutoPostBack="true" RepeatDirection="Horizontal" RepeatLayout="Flow"></asp:radiobuttonlist></td>
</tr>
<tr runat="server" id="trWidth">
	<td class="SubHead" width="200"><dnn:label id="plWidth" runat="server" resourcekey="MaxWidth" suffix=":" controlname="txtWidth"></dnn:label></TD>
	<TD>
		<asp:textbox id="txtWidth" cssclass="NormalTextBox" runat="server" maxlength="50" width="250px"></asp:textbox>
		<asp:requiredfieldvalidator id="valWidth" runat="server" cssclass="NormalRed"		
			resourcekey="valWidth.ErrorMessage" display="Dynamic" errormessage="<br> Width is Required" controltovalidate="txtWidth"></asp:requiredfieldvalidator>						
		<asp:CompareValidator ID="valWidthIsNumber" Runat="server" CssClass="NormalRed"
			resourceKey="valWidthIsNumber.ErrorMessage" Display="Dynamic" errormessage="<br> Width must be a Number" controltovalidate="txtWidth" Operator="DataTypeCheck" Type="Integer"></asp:CompareValidator>						
	</TD>
</tr>
<tr runat="server" id="trHeight">
	<td class="SubHead" width="200"><dnn:label id="plHeight" runat="server" resourcekey="MaxHeight" suffix=":" controlname="txtHeight"></dnn:label></td>
	<td>
		<asp:textbox id="txtHeight" cssclass="NormalTextBox" runat="server" maxlength="50" width="250px"></asp:textbox>
		<asp:requiredfieldvalidator id="valHeight" runat="server" cssclass="NormalRed"		
			resourcekey="valHeight.ErrorMessage" display="Dynamic" errormessage="<br> Height is Required" controltovalidate="txtHeight"></asp:requiredfieldvalidator>						
		<asp:CompareValidator ID="valHeightIsNumber" Runat="server" CssClass="NormalRed"
			resourceKey="valHeightIsNumber.ErrorMessage" Display="Dynamic" errormessage="<br> Height must be a Number" controltovalidate="txtHeight" Operator="DataTypeCheck" Type="Integer"></asp:CompareValidator>						
	</td>
</tr>
<tr runat="server" id="trSquare">
	<td class="SubHead" width="200"><dnn:label id="plSquare" runat="server" suffix=":" controlname="txtSquare"></dnn:label></td>
	<td>
		<asp:textbox id="txtSquare" cssclass="NormalTextBox" runat="server" maxlength="50" width="250px"></asp:textbox>
		<asp:requiredfieldvalidator id="valSquare" runat="server" cssclass="NormalRed"		
			resourcekey="valSquare.ErrorMessage" display="Dynamic" errormessage="<br> Square is Required" controltovalidate="txtSquare"></asp:requiredfieldvalidator>						
		<asp:CompareValidator ID="valSquareIsNumber" Runat="server" CssClass="NormalRed"
			resourceKey="valSquareIsNumber.ErrorMessage" Display="Dynamic" errormessage="<br> Square must be a Number" controltovalidate="txtSquare" Operator="DataTypeCheck" Type="Integer"></asp:CompareValidator>						
	</td>
</tr>
</table>
<br />
<dnn:sectionhead id="dshTemplateSettings" cssclass="Head" runat="server" section="tblTemplateSettings" resourcekey="TemplateSettings" IsExpanded="False" />
<table id="tblTemplateSettings" cellspacing="2" cellpadding="2" summary="Template Settings Design Table" border="0" runat="server">
<tr>
    <td class="SubHead" width="200"><dnn:label id="plTemplateType" runat="server" suffix=":" controlname="rdoTemplateType"></dnn:label></td>
	<td><asp:radiobuttonlist id="rdoTemplateType" Runat="server" Width="250px" CssClass="Normal" RepeatDirection="Horizontal" RepeatLayout="Flow"></asp:radiobuttonlist></td>
</tr>
<tr runat="Server" id="trPhotoTemplate">
	<td class="SubHead" width="200"><dnn:label id="plPhotoTemplate" runat="server" resourcekey="PhotoTemplate" suffix=":" controlname="txtPhotoTemplate"></dnn:label></td>
	<td><asp:TextBox ID="txtPhotoTemplate" Runat="server" TextMode="MultiLine" CssClass="NormalTextBox" Width="250px" Rows="10" /></td>
</tr>
<tr runat="Server" id="trAlbumTemplate">
	<td class="SubHead" width="200"><dnn:label id="plAlbumTemplate" runat="server" resourcekey="AlbumTemplate" suffix=":" controlname="txtAlbumTemplate"></dnn:label></td>
	<td><asp:TextBox ID="txtAlbumTemplate" Runat="server" TextMode="MultiLine" CssClass="NormalTextBox" Width="250px" Rows="10" /></td>
</tr>
</table>