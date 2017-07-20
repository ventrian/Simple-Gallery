<%@ Control Language="vb" AutoEventWireup="false" CodeBehind="ViewOptions.ascx.vb" Inherits="Ventrian.SimpleGallery.ViewOptions" %>
<%@ Register TagPrefix="dnn" TagName="SectionHead" Src="~/controls/SectionHeadControl.ascx" %>
<%@ Register TagPrefix="dnn" TagName="Label" Src="~/controls/LabelControl.ascx" %>
<%@ Register TagPrefix="dnn" TagName="URL" Src="~/controls/URLControl.ascx" %>
<dnn:SectionHead id="dshDetails" cssclass="Head" runat="server" text="Basic Settings" section="tblBasic" resourcekey="BasicSettings" />
<table id="tblBasic" cellspacing="2" cellpadding="2" summary="Appearance Design Table" border="0" runat="server">
	<tr>
		<td class="SubHead" width="200"><dnn:label id="plAlbumFilter" runat="server" resourcekey="AlbumFilter" suffix=":" controlname="drpAlbumFilter"></dnn:label></td>
		<td><asp:DropDownList id="drpAlbumFilter" Runat="server" DataValueField="AlbumID" DataTextField="CaptionIndented" width="250px" CssClass="NormalTextBox"></asp:DropDownList></td>
	</tr>
	<tr>
		<td class="SubHead" width="200"><dnn:label id="plAlbumsPerRow" runat="server" resourcekey="AlbumsPerRow" suffix=":" controlname="txtAlbumsPerRow"></dnn:label></td>
		<td>
			<asp:textbox id="txtAlbumsPerRow" cssclass="NormalTextBox" runat="server" maxlength="4" width="250px"></asp:textbox>
			<asp:requiredfieldvalidator id="valAlbumsPerRow" cssclass="NormalRed" runat="server" resourcekey="valAlbumsPerRow.ErrorMessage"
				display="Dynamic" errormessage="<br>Albums Per Row is Required" controltovalidate="txtAlbumsPerRow"></asp:requiredfieldvalidator>
			<asp:CompareValidator id="valAlbumsPerRowIsNumber" Runat="server" errormessage="<br>Albums Per Row must be a Number"
				controltovalidate="txtAlbumsPerRow" CssClass="NormalRed" resourceKey="valAlbumsPerRowIsNumber.ErrorMessage"
				Display="Dynamic" Operator="DataTypeCheck" Type="Integer"></asp:CompareValidator>
		</td>
	</tr>
	<tr>
		<td class="SubHead" width="200"><dnn:label id="plAlbumSortBy" runat="server" resourcekey="AlbumSortBy" suffix=":" controlname="drpAlbumSortBy"></dnn:label></td>
		<td><asp:DropDownList id="drpAlbumSortBy" Runat="server" width="250px" CssClass="NormalTextBox"></asp:DropDownList></td>
	</tr>
	<tr>
		<td class="SubHead" width="200"><dnn:label id="plAlbumSortDirection" runat="server" resourcekey="AlbumSortDirection" suffix=":" controlname="drpAlbumSortDirection"></dnn:label></td>
		<td><asp:DropDownList id="drpAlbumSortDirection" Runat="server" width="250px" CssClass="NormalTextBox"></asp:DropDownList></td>
	</tr>
	<tr>
		<td class="SubHead" width="200"><dnn:label id="plBorderStyle" runat="server" resourcekey="BorderStyle" suffix=":" controlname="drpBorderStyle"></dnn:label></td>
		<td><asp:DropDownList id="drpBorderStyle" Runat="server" width="250px" CssClass="NormalTextBox"></asp:DropDownList></td>
	</tr>
	<tr>
		<td class="SubHead" width="200"><dnn:label id="plThumbnailsPerRow" runat="server" resourcekey="ThumbnailsPerRow" suffix=":" controlname="txtThumbnailsPerRow"></dnn:label></td>
		<td>
			<asp:textbox id="txtThumbnailsPerRow" cssclass="NormalTextBox" runat="server" maxlength="50" width="250px"></asp:textbox>
			<asp:requiredfieldvalidator id="valThumbnailsPerRow" cssclass="NormalRed" runat="server" resourcekey="valThumbnailsPerRow.ErrorMessage"
				display="Dynamic" errormessage="<br>Thumbnails Per Row is Required" controltovalidate="txtThumbnailsPerRow"></asp:requiredfieldvalidator>
			<asp:CompareValidator id="valThumbnailsPerRowIsNumber" Runat="server" errormessage="<br>Thumbnails Per Row must be a Number"
				controltovalidate="txtThumbnailsPerRow" CssClass="NormalRed" resourceKey="valThumbnailsPerRowIsNumber.ErrorMessage"
				Display="Dynamic" Operator="DataTypeCheck" Type="Integer"></asp:CompareValidator>
		</td>
	</tr>
	<tr>
		<td class="SubHead" width="200"><dnn:label id="plPhotosPerPage" runat="server" resourcekey="PhotosPerPage" suffix=":" controlname="txtPhotosPerPage"></dnn:label></td>
		<td>
			<asp:textbox id="txtPhotosPerPage" cssclass="NormalTextBox" runat="server" maxlength="50" width="250px"></asp:textbox>
			<asp:requiredfieldvalidator id="valPhotosPerPage" cssclass="NormalRed" runat="server" resourcekey="valPhotosPerPage.ErrorMessage"
				display="Dynamic" errormessage="<br>Photos Per Page is Required" controltovalidate="txtPhotosPerPage"></asp:requiredfieldvalidator>
			<asp:CompareValidator id="valPhotoPageSizeIsNumber" Runat="server" errormessage="<br>Photos Per Page must be a Number"
				controltovalidate="txtPhotosPerPage" CssClass="NormalRed" resourceKey="valPhotoPageSizeIsNumber.ErrorMessage"
				Display="Dynamic" Operator="DataTypeCheck" Type="Integer"></asp:CompareValidator>
		</td>
	</tr>
	<tr>
		<td class="SubHead" width="200"><dnn:label id="plSortBy" runat="server" resourcekey="SortBy" suffix=":" controlname="drpSortBy"></dnn:label></td>
		<td><asp:DropDownList id="drpSortBy" Runat="server" width="250px" CssClass="NormalTextBox"></asp:DropDownList></td>
	</tr>
	<tr>
		<td class="SubHead" width="200"><dnn:label id="plSortDirection" runat="server" resourcekey="SortDirection" suffix=":" controlname="drpSortDirection"></dnn:label></td>
		<td><asp:DropDownList id="drpSortDirection" Runat="server" width="250px" CssClass="NormalTextBox"></asp:DropDownList></td>
	</tr>
</table>
<br />
<dnn:SectionHead id="dshAdvanced" cssclass="Head" runat="server" text="Advanced Settings" section="tblAdvanced" resourcekey="AdvancedSettings" IsExpanded="False" />
<table id="tblAdvanced" cellspacing="2" cellpadding="2" summary="Appearance Design Table" border="0" runat="server">
<tr>
	<td class="SubHead" width="200"><dnn:label id="plAlbumDefaultPath" runat="server" resourcekey="AlbumDefaultPath" suffix=":" controlname="txtAlbumDefaultPath"></dnn:label></td>
	<td>
		<asp:textbox id="txtAlbumDefaultPath" cssclass="NormalTextBox" runat="server" width="250px"></asp:textbox>
	</td>
</tr>
<tr>
	<td class="SubHead" width="200"><dnn:label id="plLightboxDefaultPath" runat="server" resourcekey="LightboxDefaultPath" suffix=":" controlname="txtLightboxDefaultPath"></dnn:label></td>
	<td>
		<asp:textbox id="txtLightboxDefaultPath" cssclass="NormalTextBox" runat="server" width="250px"></asp:textbox>
	</td>
</tr>
<tr>
	<td class="SubHead" width="200"><dnn:label id="plEnableSearch" runat="server" resourcekey="EnableSearch" suffix=":" controlname="chkEnableSearch"></dnn:label></td>
	<td><asp:CheckBox id="chkEnableSearch" Runat="server"></asp:CheckBox></td>
</tr>
<tr>
	<td class="SubHead" width="200"><dnn:label id="plEnableSyndication" runat="server" resourcekey="EnableSyndication" suffix=":" controlname="chkEnableSyndication"></dnn:label></td>
	<td><asp:CheckBox id="chkEnableSyndication" Runat="server"></asp:CheckBox></td>
</tr>
<tr>
	<td class="SubHead" width="200"><dnn:label id="plEnableTooltip" runat="server" resourcekey="EnableTooltip" suffix=":" controlname="chkEnableTooltip"></dnn:label></td>
	<td><asp:CheckBox id="chkEnableTooltip" Runat="server"></asp:CheckBox></td>
</tr>
<tr>
	<td class="SubHead" width="200"><dnn:label id="plHideBreadCrumbs" runat="server" resourcekey="HideBreadCrumbs" suffix=":" controlname="chkHideBreadCrumbs"></dnn:label></td>
	<td><asp:CheckBox id="chkHideBreadCrumbs" Runat="server"></asp:CheckBox></td>
</tr>
<tr>
	<td class="SubHead" width="200"><dnn:label id="plHidePager" runat="server" resourcekey="HidePager" suffix=":" controlname="chkHidePager"></dnn:label></td>
	<td><asp:CheckBox id="chkHidePager" Runat="server"></asp:CheckBox></td>
</tr>
<tr>
	<td class="SubHead" width="200"><dnn:label id="plIncludeJQuery" runat="server" suffix=":" controlname="chkIncludeJQuery"></dnn:label></td>
	<td><asp:CheckBox id="chkIncludeJQuery" Runat="server"></asp:CheckBox></td>
</tr>
<tr>
	<td class="SubHead" width="200"><dnn:label id="plIncludeViewCart" runat="server" suffix=":" controlname="chkIncludeViewCart"></dnn:label></td>
	<td><asp:CheckBox id="chkIncludeViewCart" Runat="server"></asp:CheckBox></td>
</tr>
<tr>
	<td class="SubHead" width="200"><dnn:label id="plUseAlbumAnchors" runat="server" suffix=":" controlname="chkUseAlbumAnchors"></dnn:label></td>
	<td><asp:CheckBox id="chkUseAlbumAnchors" Runat="server"></asp:CheckBox></td>
</tr>
</table>
<br />
<dnn:SectionHead id="dshCompression" cssclass="Head" runat="server" text="Compression Settings" section="tblCompression" resourcekey="CompressionSettings" IsExpanded="False" />
<table id="tblCompression" cellspacing="2" cellpadding="2" summary="Compression Design Table" border="0" runat="server">
<tr>
    <td colspan="2"><asp:Label ID="lblResizeInstructions" runat="Server" EnableViewState="False" CssClass="Normal" ResourceKey="ResizeInstructions" /></td>
</tr>
<tr>
	<td class="SubHead" width="200"><dnn:label id="plResizePhoto" runat="server" resourcekey="ResizePhoto" suffix=":" controlname="chkResizePhoto"></dnn:label></td>
	<td><asp:CheckBox id="chkResizePhoto" Runat="server"></asp:CheckBox></td>
</tr>
<tr>
	<td class="SubHead" width="200"><dnn:label id="plCompressionType" runat="server" resourcekey="CompressionType" suffix=":" controlname="drpCompressionType"></dnn:label></td>
	<td><asp:DropDownList id="drpCompressionType" Runat="server" CssClass="NormalTextBox" width="250px"></asp:DropDownList></td>
</tr>
<tr>
	<td class="SubHead" width="200"><dnn:label id="plImageWidth" runat="server" resourcekey="ImageWidth" suffix=":" controlname="txtImageWidth"></dnn:label></td>
	<td>
		<asp:textbox id="txtImageWidth" cssclass="NormalTextBox" runat="server" maxlength="50" width="250px"></asp:textbox>
		<asp:requiredfieldvalidator id="valImageWidth" cssclass="NormalRed" runat="server" resourcekey="valImageWidth.ErrorMessage"
			display="Dynamic" errormessage="<br>Image Width is Required" controltovalidate="txtImageWidth"></asp:requiredfieldvalidator>
		<asp:CompareValidator id="valImageWidthIsNumber" Runat="server" errormessage="<br>Image Width must be a Number"
			controltovalidate="txtImageWidth" CssClass="NormalRed" resourceKey="valImageWidthIsNumber.ErrorMessage"
			Display="Dynamic" Operator="DataTypeCheck" Type="Integer"></asp:CompareValidator>
	</td>
</tr>
<tr>
	<td class="SubHead" width="200"><dnn:label id="plImageHeight" runat="server" resourcekey="ImageHeight" suffix=":" controlname="txtImageHeight"></dnn:label></td>
	<td>
		<asp:textbox id="txtImageHeight" cssclass="NormalTextBox" runat="server" maxlength="50" width="250px"></asp:textbox>
		<asp:requiredfieldvalidator id="valImageHeight" cssclass="NormalRed" runat="server" resourcekey="valImageHeight.ErrorMessage"
			display="Dynamic" errormessage="<br>Image Height is Required" controltovalidate="txtImageHeight"></asp:requiredfieldvalidator>
		<asp:CompareValidator id="valImageHeightIsNumber" Runat="server" errormessage="<br>Image Height must be a Number"
			controltovalidate="txtImageHeight" CssClass="NormalRed" resourceKey="txtImageHeightIsNumber.ErrorMessage"
			Display="Dynamic" Operator="DataTypeCheck" Type="Integer"></asp:CompareValidator>
	</td>
</tr>
<tr>
	<td class="SubHead" width="200"><dnn:label id="plUseWatermark" runat="server" resourcekey="UseWatermark" suffix=":" controlname="chkUseWatermark"></dnn:label></td>
	<td><asp:CheckBox id="chkUseWatermark" Runat="server" CssClass="NormalTextBox"></asp:CheckBox></td>
</tr>
<tr>
	<td class="SubHead" width="200"><dnn:label id="plWatermarkText" runat="server" resourcekey="WatermarkText" suffix=":" controlname="txtWatermarkText"></dnn:label></td>
	<td><asp:textbox id="txtWatermarkText" cssclass="NormalTextBox" runat="server" maxlength="50" width="250px"></asp:textbox></td>
</tr>
<tr>
	<td class="SubHead" width="200"><dnn:label id="plWatermarkImage" runat="server" resourcekey="WatermarkImage" suffix=":" controlname="ctlWatermarkImage"></dnn:label></td>
	<td><dnn:url id="ctlWatermarkImage" runat="server" width="275" Required="False" showtrack="False" shownewwindow="False"
							showlog="False" urltype="F" showUrls="False" showfiles="True" showtabs="False"></dnn:url></td>
</tr>
<tr>
	<td class="SubHead" width="200"><dnn:label id="plWatermarkPosition" runat="server" resourcekey="WatermarkPosition" suffix=":" controlname="drpWatermarkPosition"></dnn:label></td>
	<td><asp:DropDownList id="drpWatermarkPosition" Runat="server" CssClass="NormalTextBox" width="250px"></asp:DropDownList></td>
</tr>
<tr>
    <td colspan="2"><br /><asp:Label ID="lblResizeAlbumInstructions" runat="Server" EnableViewState="False" CssClass="Normal" ResourceKey="ResizeAlbumInstructions" /></td>
</tr>
<tr>
	<td class="SubHead" width="200"><dnn:label id="plCompressionTypeAlbum" runat="server" resourcekey="CompressionType" suffix=":" controlname="drpCompressionTypeAlbum"></dnn:label></td>
	<td><asp:DropDownList id="drpCompressionTypeAlbum" Runat="server" CssClass="NormalTextBox" width="250px"></asp:DropDownList></td>
</tr>
<tr>
    <td class="SubHead" width="200"><dnn:label id="plThumbnailTypeAlbum" runat="server" resourcekey="ThumbnailTypeAlbum" suffix=":" controlname="rdoThumbnailTypeAlbum"></dnn:label></td>
	<td><asp:radiobuttonlist id="rdoThumbnailTypeAlbum" Runat="server" Width="250px" CssClass="Normal" AutoPostBack="true" RepeatDirection="Horizontal" RepeatLayout="Flow"></asp:radiobuttonlist></td>
</tr>
<tr runat="server" id="trAlbumThumbnailWidth">
	<td class="SubHead" width="200"><dnn:label id="plAlbumThumbnailWidth" runat="server" resourcekey="AlbumThumbnailWidth" suffix=":" controlname="txtAlbumThumbnailWidth"></dnn:label></td>
	<td>
		<asp:textbox id="txtAlbumThumbnailWidth" cssclass="NormalTextBox" runat="server" maxlength="50" width="250px"></asp:textbox>
		<asp:requiredfieldvalidator id="valAlbumThumbnailWidth" cssclass="NormalRed" runat="server" resourcekey="valAlbumThumbnailWidth.ErrorMessage"
			display="Dynamic" errormessage="<br>Album Thumbnail Width is Required" controltovalidate="txtAlbumThumbnailWidth"></asp:requiredfieldvalidator>
		<asp:CompareValidator id="valAlbumThumbnailWidthIsNumber" Runat="server" errormessage="<br>Album Thumbnail Width must be a Number"
			controltovalidate="txtAlbumThumbnailWidth" CssClass="NormalRed" resourceKey="valAlbumThumbnailWidthIsNumber.ErrorMessage"
			Display="Dynamic" Operator="DataTypeCheck" Type="Integer"></asp:CompareValidator>
	</td>
</tr>
<tr runat="server" id="trAlbumThumbnailHeight">
	<td class="SubHead" width="200"><dnn:label id="plAlbumThumbnailHeight" runat="server" resourcekey="AlbumThumbnailHeight" suffix=":" controlname="txtAlbumThumbnailHeight"></dnn:label></td>
	<td>
		<asp:textbox id="txtAlbumThumbnailHeight" cssclass="NormalTextBox" runat="server" maxlength="50" width="250px"></asp:textbox>
		<asp:requiredfieldvalidator id="valAlbumThumbnailHeight" cssclass="NormalRed" runat="server" resourcekey="valAlbumThumbnailHeight.ErrorMessage"
			display="Dynamic" errormessage="<br>Album Thumbnail Height is Required" controltovalidate="txtAlbumThumbnailHeight"></asp:requiredfieldvalidator>
		<asp:CompareValidator id="valAlbumThumbnailHeightIsNumber" Runat="server" errormessage="<br>Album Thumbnail Height must be a Number"
			controltovalidate="txtAlbumThumbnailHeight" CssClass="NormalRed" resourceKey="valAlbumThumbnailHeightIsNumber.ErrorMessage"
			Display="Dynamic" Operator="DataTypeCheck" Type="Integer"></asp:CompareValidator>
	</td>
</tr>
<tr runat="server" id="trAlbumThumbnailSquare">
	<td class="SubHead" width="200"><dnn:label id="plAlbumThumbnailSquare" runat="server" resourcekey="AlbumThumbnailSquare" suffix=":" controlname="txtAlbumThumbnailSquare"></dnn:label></td>
	<td>
		<asp:textbox id="txtAlbumThumbnailSquare" cssclass="NormalTextBox" runat="server" maxlength="50" width="250px"></asp:textbox>
		<asp:requiredfieldvalidator id="valAlbumThumbnailSquare" cssclass="NormalRed" runat="server" resourcekey="valAlbumThumbnailSquare.ErrorMessage"
			display="Dynamic" errormessage="<br>Album Thumbnail Square is Required" controltovalidate="txtAlbumThumbnailSquare"></asp:requiredfieldvalidator>
		<asp:CompareValidator id="valAlbumThumbnailSquareIsNumber" Runat="server" errormessage="<br>Album Thumbnail Square must be a Number"
			controltovalidate="txtAlbumThumbnailSquare" CssClass="NormalRed" resourceKey="valAlbumThumbnailSquareIsNumber.ErrorMessage"
			Display="Dynamic" Operator="DataTypeCheck" Type="Integer"></asp:CompareValidator>
	</td>
</tr>
<tr>
    <td colspan="2"><br /><asp:Label ID="lblResizePhotoInstructions" runat="Server" EnableViewState="False" CssClass="Normal" ResourceKey="ResizePhotoInstructions" /></td>
</tr>
<tr>
	<td class="SubHead" width="200"><dnn:label id="plCompressionTypePhoto" runat="server" resourcekey="CompressionType" suffix=":" controlname="drpCompressionTypePhoto"></dnn:label></td>
	<td><asp:DropDownList id="drpCompressionTypePhoto" Runat="server" CssClass="NormalTextBox" width="250px"></asp:DropDownList></td>
</tr>
<tr>
    <td class="SubHead" width="200"><dnn:label id="plThumbnailTypePhoto" runat="server" resourcekey="ThumbnailTypePhoto" suffix=":" controlname="rdoThumbnailTypePhoto"></dnn:label></td>
	<td><asp:radiobuttonlist id="rdoThumbnailTypePhoto" Runat="server" Width="250px" CssClass="Normal" AutoPostBack="true" RepeatDirection="Horizontal" RepeatLayout="Flow"></asp:radiobuttonlist></td>
</tr>
<tr runat="server" id="trThumbnailWidth">
	<td class="SubHead" width="200"><dnn:label id="plThumbnailWidth" runat="server" resourcekey="ThumbnailWidth" suffix=":" controlname="txtThumbnailWidth"></dnn:label></td>
	<td>
		<asp:textbox id="txtThumbnailWidth" cssclass="NormalTextBox" runat="server" maxlength="50" width="250px"></asp:textbox>
		<asp:requiredfieldvalidator id="valThumbnailWidth" cssclass="NormalRed" runat="server" resourcekey="valThumbnailWidth.ErrorMessage"
			display="Dynamic" errormessage="<br>Thumbnail Width is Required" controltovalidate="txtThumbnailWidth"></asp:requiredfieldvalidator>
		<asp:CompareValidator id="valThumbnailWidthIsNumber" Runat="server" errormessage="<br>Thumbnail Width must be a Number"
			controltovalidate="txtThumbnailWidth" CssClass="NormalRed" resourceKey="valThumbnailWidthIsNumber.ErrorMessage"
			Display="Dynamic" Operator="DataTypeCheck" Type="Integer"></asp:CompareValidator>
	</td>
</tr>
<tr runat="server" id="trThumbnailHeight">
	<td class="SubHead" width="200"><dnn:label id="plThumbnailHeight" runat="server" resourcekey="ThumbnailHeight" suffix=":" controlname="txtThumbnailHeight"></dnn:label></TD>
	<td>
		<asp:textbox id="txtThumbnailHeight" cssclass="NormalTextBox" runat="server" maxlength="50" width="250px"></asp:textbox>
		<asp:requiredfieldvalidator id="valThumbnailHeight" cssclass="NormalRed" runat="server" resourcekey="valThumbnailHeight.ErrorMessage"
			display="Dynamic" errormessage="<br>Thumbnail Height is Required" controltovalidate="txtThumbnailHeight"></asp:requiredfieldvalidator>
		<asp:CompareValidator id="Comparevalidator1" Runat="server" errormessage="<br>Thumbnail Height must be a Number"
			controltovalidate="txtThumbnailHeight" CssClass="NormalRed" resourceKey="valThumbnailHeightIsNumber.ErrorMessage"
			Display="Dynamic" Operator="DataTypeCheck" Type="Integer"></asp:CompareValidator>
    </td>
</tr>
<tr runat="server" id="trThumbnailSquare">
	<td class="SubHead" width="200"><dnn:label id="plThumbnailSquare" runat="server" resourcekey="ThumbnailSquare" suffix=":" controlname="txtThumbnailSquare"></dnn:label></td>
	<td>
		<asp:textbox id="txtThumbnailSquare" cssclass="NormalTextBox" runat="server" maxlength="50" width="250px"></asp:textbox>
		<asp:requiredfieldvalidator id="valThumbnailSquare" cssclass="NormalRed" runat="server" resourcekey="valThumbnailSquare.ErrorMessage"
			display="Dynamic" errormessage="<br>Thumbnail Square is Required" controltovalidate="txtThumbnailSquare"></asp:requiredfieldvalidator>
		<asp:CompareValidator id="valThumbnailSquareIsNumber" Runat="server" errormessage="<br>Thumbnail square must be a Number"
			controltovalidate="txtThumbnailSquare" CssClass="NormalRed" resourceKey="valThumbnailSquareIsNumber.ErrorMessage"
			Display="Dynamic" Operator="DataTypeCheck" Type="Integer"></asp:CompareValidator>
	</td>
</tr>
</table>
<br />
<dnn:SectionHead id="dshSecurity" cssclass="Head" runat="server" text="Security Settings" section="tblSecurity" resourcekey="SecuritySettings" IsExpanded="False" />
<table id="tblSecurity" cellspacing="2" cellpadding="2" summary="Security Design Table" border="0" runat="server">
<tr>
	<td class="SubHead" width="200"><dnn:label id="plPhotoModeration" runat="server" resourcekey="PhotoModeration" suffix=":" controlname="chkPhotoModeration"></dnn:label></td>
	<td><asp:CheckBox id="chkPhotoModeration" Runat="server" AutoPostBack="True"></asp:CheckBox></TD>
</tr>
<tr>
	<td class="SubHead" width="200"><dnn:label id="plPhotoPermissions" runat="server" resourcekey="PhotoPermissions" suffix=":" controlname="chkUploadPhotoRoles"></dnn:label></td>
	<td>
		<asp:DataGrid id="dgPhotoPermissions" Runat="server" CssClass="Normal" DataKeyField="RoleID" Width="325"
			GridLines="None" BorderStyle="None" BorderWidth="1px" cellspacing="0" AutoGenerateColumns="False"
			BackColor="Transparent">
			<HeaderStyle Font-Bold="True" HorizontalAlign="center" ForeColor="Black" BackColor="Transparent"></HeaderStyle>
			<FooterStyle BackColor="White"></FooterStyle>
			<Columns>
				<asp:TemplateColumn>
					<ItemTemplate>
						<%#Eval("RoleName")%>
					</ItemTemplate>
				</asp:TemplateColumn>
				<asp:TemplateColumn>
					<HeaderTemplate>
						<asp:Label ID="lblUpload" runat="server" ResourceKey="Upload" EnableViewState="False" />
					</HeaderTemplate>
					<ItemStyle HorizontalAlign="center" Width="50px" />
					<ItemTemplate>
						<asp:CheckBox ID="chkUpload" Runat="server" />
					</ItemTemplate>
				</asp:TemplateColumn>
				<asp:TemplateColumn>
					<HeaderTemplate>
						<asp:Label ID="lblEdit" runat="server" ResourceKey="Edit" EnableViewState="False" />
					</HeaderTemplate>
					<ItemStyle HorizontalAlign="center" Width="50px" />
					<ItemTemplate>
						<asp:CheckBox ID="chkEdit" Runat="server" />
					</ItemTemplate>
				</asp:TemplateColumn>
				<asp:TemplateColumn>
					<HeaderTemplate>
						<asp:Label ID="lblDelete" runat="server" ResourceKey="Delete" EnableViewState="False" />
					</HeaderTemplate>
					<ItemStyle HorizontalAlign="center" Width="50px" />
					<ItemTemplate>
						<asp:CheckBox ID="chkDelete" Runat="server" />
					</ItemTemplate>
				</asp:TemplateColumn>
				<asp:TemplateColumn>
					<HeaderTemplate>
						<asp:Label ID="lblApprove" runat="server" ResourceKey="Approve" EnableViewState="False" />
					</HeaderTemplate>
					<ItemStyle HorizontalAlign="center" Width="50px" />
					<ItemTemplate>
						<asp:CheckBox ID="chkApprove" Runat="server" />
					</ItemTemplate>
				</asp:TemplateColumn>
				<asp:TemplateColumn>
					<HeaderTemplate>
						<asp:Label ID="lblAlbum" runat="server" ResourceKey="Album" EnableViewState="False" />
					</HeaderTemplate>
					<ItemStyle HorizontalAlign="center" Width="50px" />
					<ItemTemplate>
						<asp:CheckBox ID="chkAlbum" Runat="server" />
					</ItemTemplate>
				</asp:TemplateColumn>
			</Columns>
		</asp:DataGrid>
	</td>
</tr>
</table>
<br />
<dnn:SectionHead id="dshSlideShow" cssclass="Head" runat="server" text="SlideShow Settings" section="tblSlideShow" resourcekey="SlideShowSettings" IsExpanded="False" />
<table id="tblSlideShow" cellspacing="2" cellpadding="2" summary="SlideShow Design Table" border="0" runat="server">
<tr>
	<td class="SubHead" width="200"><dnn:label id="plSlideshowType" runat="server" resourcekey="SlideshowType" suffix=":" controlname="drpSlideshowType"></dnn:label></td>
	<td><asp:DropDownList id="drpSlideshowType" Runat="server" AutoPostBack="True" CssClass="NormalTextBox" Width="250px"></asp:DropDownList></td>
</tr>
<tr id="trStandardWidth" runat="Server">
	<td class="SubHead" width="200"><dnn:label id="Label1" runat="server" resourcekey="StandardWidth" suffix=":" controlname="txtStandardWidth"></dnn:label></td>
	<td>
		<asp:textbox id="txtStandardWidth" cssclass="NormalTextBox" runat="server" maxlength="50" width="250px"></asp:textbox>
		<asp:requiredfieldvalidator id="valStandardWidth" cssclass="NormalRed" runat="server" resourcekey="valStandardWidth.ErrorMessage"
			display="Dynamic" errormessage="<br>Standard Width is Required" controltovalidate="txtStandardWidth"></asp:requiredfieldvalidator>
		<asp:CompareValidator id="valStandardWidthIsNumber" Runat="server" errormessage="<br>Standard Width must be a Number"
			controltovalidate="txtStandardWidth" CssClass="NormalRed" resourceKey="valStandardWidthIsNumber.ErrorMessage"
			Display="Dynamic" Operator="DataTypeCheck" Type="Integer"></asp:CompareValidator>
    </td>
</tr>
<tr id="trPopupWidth" runat="Server">
	<td class="SubHead" width="200"><dnn:label id="plPopupWidth" runat="server" resourcekey="PopupWidth" suffix=":" controlname="txtPopupWidth"></dnn:label></td>
	<td>
		<asp:textbox id="txtPopupWidth" cssclass="NormalTextBox" runat="server" maxlength="50" width="250px"></asp:textbox>
		<asp:requiredfieldvalidator id="valPopupWidth" cssclass="NormalRed" runat="server" resourcekey="valPopupWidth.ErrorMessage"
			display="Dynamic" errormessage="<br>Popup Width is Required" controltovalidate="txtPopupWidth"></asp:requiredfieldvalidator>
		<asp:CompareValidator id="valPopupWidthIsNumber" Runat="server" errormessage="<br>Popup Width must be a Number"
			controltovalidate="txtPopupWidth" CssClass="NormalRed" resourceKey="valPopupWidthIsNumber.ErrorMessage"
			Display="Dynamic" Operator="DataTypeCheck" Type="Integer"></asp:CompareValidator>
    </td>
</tr>
<tr id="trPopupHeight" runat="Server">
	<td class="SubHead" width="200"><dnn:label id="plPopupHeight" runat="server" resourcekey="PopupHeight" suffix=":" controlname="txtPopupHeight"></dnn:label></td>
	<td>
		<asp:textbox id="txtPopupHeight" cssclass="NormalTextBox" runat="server" maxlength="50" width="250px"></asp:textbox>
		<asp:requiredfieldvalidator id="valPopupHeight" cssclass="NormalRed" runat="server" resourcekey="valPopupHeight.ErrorMessage"
			display="Dynamic" errormessage="<br>Popup Height is Required" controltovalidate="txtPopupHeight"></asp:requiredfieldvalidator>
		<asp:CompareValidator id="valPopupHeightIsNumber" Runat="server" errormessage="<br>Popup Height must be a Number"
			controltovalidate="txtPopupHeight" CssClass="NormalRed" resourceKey="valPopupHeightIsNumber.ErrorMessage"
			Display="Dynamic" Operator="DataTypeCheck" Type="Integer"></asp:CompareValidator>
	</td>
</tr>
<tr id="trEnableScrollbar" runat="Server">
	<td class="SubHead" width="200"><dnn:label id="plEnableScrollbar" runat="server" resourcekey="EnableScrollbar" suffix=":" controlname="chkEnableScrollbar"></dnn:label></td>
	<td><asp:CheckBox id="chkEnableScrollbar" Runat="server"></asp:CheckBox></td>
</tr>
<tr id="trLightboxNextKey" runat="Server">
	<td class="SubHead" width="200"><dnn:label id="plNextKey" runat="server" resourcekey="NextKey" suffix=":" controlname="txtNextKey"></dnn:label></td>
	<td>
		<asp:textbox id="txtNextKey" cssclass="NormalTextBox" runat="server" maxlength="1" width="250px"></asp:textbox>
		<asp:requiredfieldvalidator id="valNextKey" cssclass="NormalRed" runat="server" resourcekey="valNextKey.ErrorMessage"
			display="Dynamic" errormessage="<br>Next Key is Required" controltovalidate="txtNextKey"></asp:requiredfieldvalidator>
	</td>
</tr>
<tr id="trLightboxPreviousKey" runat="Server">
	<td class="SubHead" width="200"><dnn:label id="plPreviousKey" runat="server" resourcekey="PreviousKey" suffix=":" controlname="txtPreviousKey"></dnn:label></td>
	<td>
		<asp:textbox id="txtPreviousKey" cssclass="NormalTextBox" runat="server" maxlength="1" width="250px"></asp:textbox>
		<asp:requiredfieldvalidator id="valPreviousKey" cssclass="NormalRed" runat="server" resourcekey="valPreviousKey.ErrorMessage"
			display="Dynamic" errormessage="<br>Previous Key is Required" controltovalidate="txtPreviousKey"></asp:requiredfieldvalidator>
	</td>
</tr>
<tr id="trLightboxCloseKey" runat="Server">
	<td class="SubHead" width="200"><dnn:label id="plCloseKey" runat="server" resourcekey="CloseKey" suffix=":" controlname="txtCloseKey"></dnn:label></td>
	<td>
		<asp:textbox id="txtCloseKey" cssclass="NormalTextBox" runat="server" maxlength="1" width="250px"></asp:textbox>
		<asp:requiredfieldvalidator id="valCloseKey" cssclass="NormalRed" runat="server" resourcekey="valCloseKey.ErrorMessage"
			display="Dynamic" errormessage="<br>Close Key is Required" controltovalidate="txtCloseKey"></asp:requiredfieldvalidator>
	</td>
</tr>
<tr id="trLightboxDownloadKey" runat="Server">
	<td class="SubHead" width="200"><dnn:label id="plDownloadKey" runat="server" resourcekey="DownloadKey" suffix=":" controlname="txtDownloadKey"></dnn:label></td>
	<td>
		<asp:textbox id="txtDownloadKey" cssclass="NormalTextBox" runat="server" maxlength="1" width="250px"></asp:textbox>
		<asp:requiredfieldvalidator id="valDownloadKey" cssclass="NormalRed" runat="server" resourcekey="valDownloadKey.ErrorMessage"
			display="Dynamic" errormessage="<br>Download Key is Required" controltovalidate="txtDownloadKey"></asp:requiredfieldvalidator>
	</td>
</tr>
<tr id="trLightboxSlideInterval" runat="Server">
	<td class="SubHead" width="200"><dnn:label id="plSlideInterval" runat="server" resourcekey="SlideInterval" suffix=":" controlname="txtSlideInterval"></dnn:label></td>
	<td>
		<asp:textbox id="txtSlideInterval" cssclass="NormalTextBox" runat="server" maxlength="50" width="250px"></asp:textbox>
		<asp:requiredfieldvalidator id="valSlideInterval" cssclass="NormalRed" runat="server" resourcekey="valSlideInterval.ErrorMessage"
			display="Dynamic" errormessage="<br>Slide Interval is Required" controltovalidate="txtSlideInterval"></asp:requiredfieldvalidator>
		<asp:CompareValidator id="valSlideIntervalIsNumber" Runat="server" errormessage="<br>Slide Interval must be a Number"
			controltovalidate="txtSlideInterval" CssClass="NormalRed" resourceKey="valSlideIntervalIsNumber.ErrorMessage"
			Display="Dynamic" Operator="DataTypeCheck" Type="Integer"></asp:CompareValidator>
	</td>
</tr>
<tr id="trLightboxHideTitle" runat="Server">
	<td class="SubHead" width="200"><dnn:label id="plSlideHideTitle" runat="server" suffix=":" controlname="chkSlideHideTitle"></dnn:label></td>
	<td><asp:CheckBox id="chkSlideHideTitle" Runat="server"></asp:CheckBox></td>
</tr>
<tr id="trLightboxHideDescription" runat="Server">
	<td class="SubHead" width="200"><dnn:label id="plSlideHideDescription" runat="server" suffix=":" controlname="chkSlideHideDescription"></dnn:label></td>
	<td><asp:CheckBox id="chkSlideHideDescription" Runat="server"></asp:CheckBox></td>
</tr>
<tr id="trLightboxHidePaging" runat="Server">
	<td class="SubHead" width="200"><dnn:label id="plSlideHidePaging" runat="server" suffix=":" controlname="chkSlideHidePaging"></dnn:label></td>
	<td><asp:CheckBox id="chkSlideHidePaging" Runat="server"></asp:CheckBox></td>
</tr>
<tr id="trLightboxHideTags" runat="Server">
	<td class="SubHead" width="200"><dnn:label id="plSlideHideTags" runat="server" suffix=":" controlname="chkSlideHideTags"></dnn:label></td>
	<td><asp:CheckBox id="chkSlideHideTags" Runat="server"></asp:CheckBox></td>
</tr>
<tr id="trLightboxHideDownload" runat="Server">
	<td class="SubHead" width="200"><dnn:label id="plSlideHideDownload" runat="server" suffix=":" controlname="chkSlideHideDownload"></dnn:label></td>
	<td><asp:CheckBox id="chkSlideHideDownload" Runat="server"></asp:CheckBox></td>
</tr>
</table>
<br />
<dnn:sectionhead id="dshTags" cssclass="Head" runat="server" text="Tag Settings" section="tblTags" resourcekey="TagSettings" IsExpanded="False"></dnn:sectionhead>
<table id="tblTags" cellspacing="2" cellpadding="2" summary="Tags Design Table" border="0" runat="server">
	<tr>
		<td class="SubHead" width="200"> <dnn:label id="plTags" runat="server" resourcekey="EnableTags" suffix=":" controlname="chkEnableTags"></dnn:label></td>
		<td><asp:CheckBox id="chkEnableTags" Runat="server"></asp:CheckBox></td>
	</tr>
	<tr>
		<td class="SubHead" width="200"> <dnn:label id="plRequireTags" runat="server" suffix=":" controlname="chkRequireTags"></dnn:label></td>
		<td><asp:CheckBox id="chkRequireTags" Runat="server"></asp:CheckBox></td>
	</tr>
	<tr>
		<td class="SubHead" width="200"><dnn:label id="plTagCount" runat="server" resourcekey="TagCount" suffix=":" controlname="txtTagCount"></dnn:label></td>
		<td>
			<asp:textbox id="txtTagCount" cssclass="NormalTextBox" runat="server" maxlength="50" width="250px"></asp:textbox>
			<asp:requiredfieldvalidator id="valTagCount" cssclass="NormalRed" runat="server" resourcekey="valTagCount.ErrorMessage"
				display="Dynamic" errormessage="<br>Tag Count is Required" controltovalidate="txtTagCount"></asp:requiredfieldvalidator>
			<asp:CompareValidator id="valTagCountIsNumber" Runat="server" errormessage="<br>Tag Count must be a Number"
				controltovalidate="txtTagCount" CssClass="NormalRed" resourceKey="valTagCountIsNumber.ErrorMessage"
				Display="Dynamic" Operator="DataTypeCheck" Type="Integer"></asp:CompareValidator>
		</td>
	</tr>
</table>
<br />
<dnn:sectionhead id="dshUploader" cssclass="Head" runat="server" text="Uploader Settings" section="tblUploader" resourcekey="UploaderSettings" IsExpanded="False"></dnn:sectionhead>
<table id="tblUploader" cellspacing="2" cellpadding="2" summary="Uploader Design Table" border="0" runat="server">
	<tr>
		<td class="SubHead" width="200"><dnn:label id="plUploaderFileSize" runat="server" resourcekey="UploaderFileSize" suffix=":" controlname="txtUploaderFileSize"></dnn:label></td>
		<td>
			<asp:textbox id="txtUploaderFileSize" cssclass="NormalTextBox" runat="server" maxlength="50" width="250px"></asp:textbox>
			<asp:requiredfieldvalidator id="valUploaderFileSize" cssclass="NormalRed" runat="server" resourcekey="valUploaderFileSize.ErrorMessage"
				display="Dynamic" errormessage="<br>Maximum file size is Required" controltovalidate="txtUploaderFileSize"></asp:requiredfieldvalidator>
			<asp:CompareValidator id="valUploaderFileSizeIsNumber" Runat="server" errormessage="<br>Maximum file size must be a Number"
				controltovalidate="txtUploaderFileSize" CssClass="NormalRed" resourceKey="valUploaderFileSizeIsNumber.ErrorMessage"
				Display="Dynamic" Operator="DataTypeCheck" Type="Integer"></asp:CompareValidator>
		</td>
	</tr>
	<tr>
		<td class="SubHead" width="200"> <dnn:label id="plUseXmpExif" runat="server" resourcekey="UseXmpExif" suffix=":" controlname="chkUseXmpExif"></dnn:label></td>
		<td><asp:CheckBox id="chkUseXmpExif" Runat="server"></asp:CheckBox></td>
	</tr>
</table>
<br />
<dnn:sectionhead id="dshZip" cssclass="Head" runat="server" text="Zip Settings" section="tblZip" resourcekey="ZipSettings" IsExpanded="False"></dnn:sectionhead>
<table id="tblZip" cellSpacing="2" cellPadding="2" summary="Zip Design Table" border="0" runat="server">
	<tr>
		<td class="SubHead" width="200"><dnn:label id="plEnableZip" runat="server" resourcekey="EnableZip" suffix=":" controlname="chkEnableZip"></dnn:label></td>
		<td><asp:CheckBox id="chkEnableZip" Runat="server"></asp:CheckBox></td>
	</tr>
	<tr>
		<td class="SubHead" width="200"><dnn:label id="plIncludeSubFolders" runat="server" resourcekey="IncludeSubFolders" suffix=":" controlname="chkIncludeSubFolders"></dnn:label></td>
		<td><asp:CheckBox id="chkIncludeSubFolders" Runat="server"></asp:CheckBox></td>
	</tr>
</table>
