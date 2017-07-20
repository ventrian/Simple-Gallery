<%@ Control Language="vb" AutoEventWireup="false" CodeBehind="AddPhoto.ascx.vb" Inherits="Ventrian.SimpleGallery.AddPhoto" %>
<%@ Register TagPrefix="dnn" TagName="Label" Src="~/controls/LabelControl.ascx" %>
<%@ Register TagPrefix="SimpleGallery" TagName="GalleryMenu" Src="Controls\GalleryMenu.ascx"%>
<%@ Register TagPrefix="SimpleGallery" TagName="EditPhotos" Src="Controls\EditPhotos.ascx"%>
<SimpleGallery:GalleryMenu id="ucGalleryMenu" runat="server" ShowCommandBar="False" ShowSeparator="True" />

<div align="left">
<table cellspacing="0" cellpadding="0" width="600" summary="Wizard Design Table">
	<tr>
		<td width="50" height="50" align="center" valign="middle">
			<asp:Image ID="imgStep" Runat="server" ImageUrl="~/DesktopModules/SimpleGallery/Images/iconStep1.gif" Width="48" Height="48" />
		</td>
		<td>
			<asp:Label ID="lblStep" Runat="server" CssClass="NormalBold" /><br />
			<asp:Label ID="lblStepDescription" Runat="server" CssClass="Normal" /><br />
			<asp:Label ID="lblRequiresApproval" Runat="server" CssClass="NormalRed" ResourceKey="RequiresApproval" EnableViewState="False" Visible="False" />
		</td>
	</tr>
</table>
</div>
<hr size="1" />

<asp:Panel ID="pnlStep1" Runat="server">
<asp:PlaceHolder ID="phStep1a" runat="server">
<table cellspacing="0" cellpadding="2" width="600" summary="Select Album Design Table" align="center">
    <tr>
        <td colspan="2"><asp:RadioButton ID="rdoSelectExisting" runat="server" Text="Selecting Existing Album" ResourceKey="SelectExisting" CssClass="NormalBold" GroupName="Step1" /></td>
    </tr>
	<tr valign="top">
		<td class="SubHead" width="150"><dnn:label id="plAlbum" runat="server" controlname="drpAlbums" suffix=":"></dnn:label></td>
		<td>
			<asp:DropDownList ID="drpAlbums" Runat="server" DataTextField="CaptionIndented" DataValueField="AlbumID" Width="300px" />
			<asp:CustomValidator ID="valSelectExisting" runat="server" CssClass="NormalRed" ResourceKey="valSelectExisting" Display="Dynamic" 
                ControlToValidate="drpAlbums" ErrorMessage="<br>You must select an existing album."></asp:CustomValidator>
		</td>
	</tr>
</table>
<hr size="1" />	
</asp:PlaceHolder>
<asp:PlaceHolder ID="phStep1b" runat="server">
<table cellspacing="0" cellpadding="2" width="600" summary="Select Album Design Table" align="center">    
    <tr>
        <td colspan="2"><asp:RadioButton ID="rdoCreateNew" runat="server" Text="Create New Album" ResourceKey="CreateNew" CssClass="NormalBold" GroupName="Step1" /></td>
    </tr>
	<tr valign="top" runat="server" id="trParentAlbum">
		<td class="SubHead" width="150">
			<dnn:label id="plParentAlbum" runat="server" resourcekey="ParentAlbum" suffix=":" controlname="drpParentAlbum"></dnn:label></td>
		<td>
			<asp:DropDownList id="drpParentAlbum" DataValueField="AlbumID" DataTextField="CaptionIndented" Runat="server" Width="300px"></asp:DropDownList></td>
	</tr>
	<tr valign="top">
		<td class="SubHead" nowrap="nowrap" width="150">
			<dnn:label id="plCaption" runat="server" resourcekey="Caption" suffix=":" controlname="txtCaption"></dnn:label></TD>
		<td align="left" width="450">
			<asp:textbox id="txtCaption" cssclass="NormalTextBox" runat="server" width="300" maxlength="255"></asp:textbox>
			<asp:CustomValidator ID="valSelectNew" runat="server" CssClass="NormalRed" ResourceKey="valSelectNew" Display="Dynamic"
			    ErrorMessage="<br>You Must Enter a Valid Caption" ControlToValidate="txtCaption" ValidateEmptyText="true" />	
        </td>
	</tr>
	<tr valign="top">
		<td class="SubHead" width="150">
			<dnn:label id="plDescription" runat="server" suffix=":" controlname="txtDescription"></dnn:label></td>
		<td>
			<asp:textbox id="txtDescription" cssclass="NormalTextBox" runat="server" width="300" columns="30"
				maxlength="255" textmode="MultiLine" rows="5"></asp:textbox></td>
	</tr>
</table>
</asp:PlaceHolder>
</asp:Panel>
<asp:Panel ID="pnlStep2" runat="server">
<div style="width: 100%;" align="center">
<asp:PlaceHolder ID="phjQueryScripts" Runat="server" EnableViewState="False" />
<script type="text/javascript" src='<%= ResolveUrl("JS/SWFUpload/swfupload.2.2.0.js") %>'></script>
<script type="text/javascript" src='<%= ResolveUrl("JS/SWFUpload/handlers.2.2.0.js") %>'></script>
<script type="text/javascript">

    jQuery(document).ready(function() {        LoadUploader();
    });
    
	var swfu;
	function LoadUploader() {
		swfu = new SWFUpload({
			// Backend Settings
			upload_url: "<%= GetUploadUrl() %>",	// Relative to the SWF file
			post_params: {
			    "AlbumID" : '<%= Request("AlbumID") %>', 
			    "TabID" : '<%= Request("TabID") %>', 
			    "ModuleID" : '<asp:literal id="litModuleID" runat="server" />', 
			    "BatchID" : '<asp:literal id="litBatchID" runat="server" />', 
			    "Ticket" : '<asp:literal id="litTicketID" runat="server" />'
            },	

			// File Upload Settings
			file_size_limit : "<%= GetMaximumFileSize() %>",	
			file_types : "*.gif;*.jpg;*.jpeg,*.png",
			file_types_description : "Common Web Image Formats (gif, jpg, png)",
			file_upload_limit : "0",    // Zero means unlimited

			// Event Handler Settings - these functions as defined in Handlers.js
			//  The handlers are not part of SWFUpload but are part of my website and control how
			//  my website reacts to the SWFUpload events.
			file_queue_error_handler : fileQueueError,
			file_dialog_complete_handler : fileDialogComplete,
			upload_progress_handler : uploadProgress,
			upload_error_handler : uploadError,
			upload_success_handler : uploadSuccess,
			upload_complete_handler : uploadComplete,

			// Button Settings
			button_image_url : '<%= ResolveUrl("Images/SWFUpload/XPButtonNoText_160x22.png") %>',	// Relative to the SWF file
			button_placeholder_id : "spanButtonPlaceholder",
			button_width: 160,
			button_height: 22,
			button_text : '<span class="button"><asp:Label ID="lblSelectImages" Runat="server" EnableViewState="False" ResourceKey="SelectImages" /></span>',
			button_text_style : '.button { font-family: Tahoma,Arial,Helvetica; font-size: 11px; font-weight:bold; text-align: center; }',
			button_text_top_padding: 2,
			button_text_left_padding: 5,

			// Flash Settings
			flash_url : '<%= ResolveUrl("JS/SWFUpload/swfupload.2.2.0.swf") %>',	// Relative to this file

			custom_settings : {
				upload_target : "sg_progress_container",
				image_path : '<%= ResolveUrl("Images/SWFUpload/") %>'
			},

			// Debug Settings
			debug: false
		});
	}
		
</script>
    <div id="sg_upload_container" style="margin: 0px 10px;margin-top:20px;">
        <div>
            <span id="spanButtonPlaceholder"></span>
        </div>
	    <div id="sg_progress_container" class="Normal" style="margin-top: 10px;"></div>
    </div>
</div>
</asp:Panel>

<div style="width: 100%;">
    <SimpleGallery:EditPhotos id="ucEditPhotos" runat="server" />
</div>

<asp:Panel ID="pnlWizard" Runat="server">
<div align="center">
    <br />
    <asp:ImageButton ID="imgPrevious" Runat="server" ImageUrl="~\images\lt.gif" ImageAlign="AbsBottom" />
	<asp:linkbutton id="cmdPrevious" resourcekey="PreviousStep" runat="server" cssclass="CommandButton" text="Previous"
		borderstyle="none" />
    <asp:ImageButton ID="imgCancel" Runat="server" ImageUrl="~\DesktopModules\SimpleGallery\images\iconCancel.gif" ImageAlign="AbsBottom" CausesValidation="False" style="padding-left: 20px;" />
	<asp:linkbutton id="cmdCancel" runat="server" cssclass="CommandButton" ResourceKey="cmdCancel" text="Cancel" borderstyle="none" CausesValidation="False" style="padding-right: 20px;" />
    <asp:linkbutton id="cmdNext" resourcekey="NextStep" runat="server" cssclass="CommandButton" text="Next" borderstyle="none" />
	<asp:ImageButton ID="imgNext" Runat="server" ImageUrl="~\images\rt.gif" ImageAlign="AbsBottom" />
</div>
</asp:Panel>

<asp:Panel ID="pnlSave" runat="server">
<p align="center">	
    <br />
    <asp:ImageButton ID="imgSave" Runat="server" ImageUrl="~\DesktopModules\SimpleGallery\images\iconSave.gif" ImageAlign="AbsBottom" />
	<asp:linkbutton id="cmdSave" runat="server" cssclass="CommandButton" ResourceKey="cmdSave" text="Save this batch" borderstyle="none" />	
</p>
</asp:Panel>


<ul id="thumbnails" class="sg_photolist"></ul>
