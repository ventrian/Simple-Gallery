<%@ Control Language="vb" AutoEventWireup="false" CodeBehind="ViewPhotos.ascx.vb" Inherits="Ventrian.SimpleGallery.Controls.ViewPhotos" %>
<asp:PlaceHolder ID="phPopupScripts" Runat="server" EnableViewState="False" Visible="False">
<script language="JavaScript" type="text/javascript">
<!--
function openScreenWin(winName, loc, width, height){					// Opens a new window
	var newWindow;
	newWindow = window.open(loc,winName,"location=no,status=no,scrollbars=<%= GetScrollbarSetting() %>,toolbar=no,menubar=no,directories=no,resizable=yes,width="+width+",height="+height);		
	newWindow.focus();
}

//-->
</script>
</asp:PlaceHolder>


<asp:PlaceHolder ID="phjQueryScripts" Runat="server" EnableViewState="False" />
<asp:PlaceHolder ID="phLightboxScripts" Runat="server" EnableViewState="False" Visible="False">
<script type="text/javascript">
    // Use jQuery via jQuery(...)
    jQuery(document).ready(function () {
        jQuery('a[rel*=lightbox<%= SimpleGalleryBase.TabModuleID %>]').lightBox({
		    imageLoading: '<%= Me.ResolveUrl("../images/lightbox/lightbox-ico-loading.gif") %>',
		    imageBtnPrev: '<%= Me.ResolveUrl("../images/lightbox/lightbox-btn-prev.gif") %>',
		    imageBtnNext: '<%= Me.ResolveUrl("../images/lightbox/lightbox-btn-next.gif") %>',
		    imageBtnClose: '<%= Me.ResolveUrl("../images/lightbox/lightbox-btn-close.gif") %>',
		    imageBlank: '<%= Page.ResolveUrl("~/images/spacer.gif") %>',
		    txtImage: '<%= GetLocalizedValue("Image") %>',
		    txtOf: '<%= GetLocalizedValue("Of") %>',
		    next: '<%= GetLocalizedValue("Next") %>',
		    previous: '<%= GetLocalizedValue("Previous") %>',
		    close: '<%= GetLocalizedValue("Close") %>',
		    download: '<%= GetLocalizedValue("Download") %>',
		    txtPlay: '<%= GetLocalizedValue("Play") %>',
		    txtPause: '<%= GetLocalizedValue("Pause") %>',
		    keyToClose: '<%= GetShortcutKey("c") %>',
		    keyToPrev: '<%= GetShortcutKey("p") %>',
		    keyToNext: '<%= GetShortcutKey("n") %>',
		    keyToDownload: '<%= GetShortcutKey("d") %>',
		    slideInterval: '<%= GetSlideInterval() %>',
		    hideTitle: <%= GetHideTitle() %>,
		    hideDescription: <%= GetHideDescription() %>,
		    hidePaging: <%= GetHidePaging() %>,
		    hideTags: <%= GetHideTags() %>,
		    hideDownload: <%= GetHideDownload() %>,
		    downloadLink: '<%= Page.ResolveUrl("~/DesktopModules/SimpleGallery/DownloadPhoto.ashx?PhotoID={0}&PortalID=" & SimpleGalleryBase.PortalID) %>'
	    });
    });
</script>
</asp:PlaceHolder>
<style type="text/css">
.photo-frame .topx-- { background-image: url(<%= ResolveUrl("../images/borders/" & BorderStyle & "/frame-topx--.gif") %>); }
.photo-frame .top-x- { background-image: url(<%= ResolveUrl("../images/borders/" & BorderStyle & "/frame-top-x-.gif") %>); }
.photo-frame .top--x { background-image: url(<%= ResolveUrl("../images/borders/" & BorderStyle & "/frame-top--x.gif") %>); }
.photo-frame .midx-- { background-image: url(<%= ResolveUrl("../images/borders/" & BorderStyle & "/frame-midx--.gif") %>); }
.photo-frame .mid--x { background-image: url(<%= ResolveUrl("../images/borders/" & BorderStyle & "/frame-mid--x.gif") %>); }
.photo-frame .botx-- { background-image: url(<%= ResolveUrl("../images/borders/" & BorderStyle & "/frame-botx--.gif") %>); }
.photo-frame .bot-x- { background-image: url(<%= ResolveUrl("../images/borders/" & BorderStyle & "/frame-bot-x-.gif") %>); }
.photo-frame .bot--x { background-image: url(<%= ResolveUrl("../images/borders/" & BorderStyle & "/frame-bot--x.gif") %>); }
</style>

<asp:PlaceHolder ID="phLightboxTop" Runat="server" EnableViewState="False" />

<asp:datalist id="dlGallery" runat="server" RepeatColumns="4" RepeatDirection="Horizontal" HorizontalAlign="Center" ItemStyle-VerticalAlign="Top" ItemStyle-HorizontalAlign="Center" CellPadding="2" CellSpacing="4" CssClass="View" EnableViewState="False">
<ItemTemplate>
	<asp:PlaceHolder ID="phPhoto" Runat="server" EnableViewState="False" />
</ItemTemplate>
</asp:datalist>

<asp:Panel ID="pnlPaging" Runat="server" HorizontalAlign="Center" visible="false" EnableViewState="False">
	<asp:HyperLink ID="lnkPrev" Runat="server" CssClass="CommandButton"><< Prev</asp:HyperLink>	&nbsp;&nbsp;	<asp:label id="lblCurrentPage" runat="server" cssclass="Normal"></asp:label>	&nbsp;&nbsp;
	<asp:HyperLink ID="lnkNext" Runat="server" CssClass="CommandButton">Next >></asp:HyperLink>
</asp:Panel>

<asp:PlaceHolder ID="phLightboxBottom" Runat="server" EnableViewState="False" />
