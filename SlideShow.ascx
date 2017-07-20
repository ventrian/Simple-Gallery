<%@ Control Language="vb" AutoEventWireup="false" CodeBehind="SlideShow.ascx.vb" Inherits="Ventrian.SimpleGallery.SlideShow" %>
<%@ Register TagPrefix="SimpleGallery" TagName="GalleryMenu" Src="Controls\GalleryMenu.ascx"%>
<SimpleGallery:GalleryMenu id="ucGalleryMenu" runat="server" ShowCommandBar="False" />

<script runat="server">
	Protected Overrides Sub OnLoad(ByVal e As EventArgs)
		MyBase.OnLoad(e)
		Dim myString as string =""
		Dim myUrl as string = ""
		
		
		myString = myString & "<style type=""text/css"">"
        myUrl = ResolveUrl("images/borders/" & Me.GallerySettings.BorderStyle & "/frame-topx--.gif")
		myString = myString & ".photo-frame .topx-- { background-image: url(" & myUrl & "); }"
        myUrl = ResolveUrl("images/borders/" & Me.GallerySettings.BorderStyle & "/frame-top-x-.gif")
		myString = myString & ".photo-frame .top-x- { background-image: url(" & myUrl & "); }"
        myUrl = ResolveUrl("images/borders/" & Me.GallerySettings.BorderStyle & "/frame-top--x.gif")
		myString = myString & ".photo-frame .top--x { background-image: url(" & myUrl & "); }"
        myUrl = ResolveUrl("images/borders/" & Me.GallerySettings.BorderStyle & "/frame-midx--.gif")
		myString = myString & ".photo-frame .midx-- { background-image: url(" & myUrl & "); }"
        myUrl = ResolveUrl("images/borders/" & Me.GallerySettings.BorderStyle & "/frame-mid--x.gif")
		myString = myString & ".photo-frame .mid--x { background-image: url(" & myUrl & "); }"
        myUrl = ResolveUrl("images/borders/" & Me.GallerySettings.BorderStyle & "/frame-botx--.gif")
		myString = myString & ".photo-frame .botx-- { background-image: url(" & myUrl & "); }"
        myUrl = ResolveUrl("images/borders/" & Me.GallerySettings.BorderStyle & "/frame-bot-x-.gif")
		myString = myString & ".photo-frame .bot-x- { background-image: url(" & myUrl & "); }"
        myUrl = ResolveUrl("images/borders/" & Me.GallerySettings.BorderStyle & "/frame-bot--x.gif")
		myString = myString & ".photo-frame .bot--x { background-image: url(" & myUrl & "); }"
		myString = myString & "</style>"


		AttachCustomHeader(myString)
	End Sub


	Sub AttachCustomHeader(ByVal CustomHeader As String)
		Dim HtmlHead As HtmlHead = Page.FindControl("Head")
		If Not (HtmlHead Is Nothing) Then
			HtmlHead.Controls.Add(New LiteralControl(CustomHeader))
		End If
	End Sub
</script>

<div align="center">
	<table width="<%= GetWidth() %>" border="0">
	<tr>
		<td class="normalbold" vAlign="top" align="left" width="33%">&nbsp;</td>
		<td class="normalbold" vAlign="top" align="center" width="33%">
			<asp:HyperLink ID="lnkDownload" Runat="server" CssClass="CommandButton" Target="_Blank" ResourceKey="DownloadPhoto" ></asp:HyperLink>
		</td>
		<td class="normalbold" vAlign="top" align="right" width="33%">
			<asp:Label ID="lblPageCount" Runat="server" EnableViewState="False" CssClass="Normal" />
		</td>
	</tr>
	<tr>
		<td class="normalbold" vAlign="top" align="left" width="33%">
			<asp:HyperLink ID="lnkPreviousTop" Runat="server" ResourceKey="Previous" CssClass="CommandButton">&#171; Previous</asp:HyperLink>
		</td>
		<td class="normalbold" align="center" width="33%">
			<asp:Label ID="lblName" Runat="server" EnableViewState="False" CssClass="NormalBold" /><br />
			<asp:Label ID="lblTags" Runat="server" EnableViewState="False" CssClass="Normal" />
		</td>
		<td class="normalbold" vAlign="top" align="right" width="33%">
			<asp:HyperLink ID="lnkNextTop" Runat="server" ResourceKey="Next" CssClass="CommandButton">Next &#187;</asp:HyperLink>
		</td>
	</tr>
	</table>
<br />
	<table width="602" cellspacing="0" cellpadding="0" border="0"><tbody>
	<tr><td valign="top" class="jSG_album_title" style="display: table-cell; "></td></tr>
	<tr><td valign="top" class="jSG_gallery"><asp:Image ID="imgPhoto" Runat="server" EnableViewState="False" cssclass="detail" /></td></tr></tbody></table>
    <asp:Label ID="lblDescription" Runat="server" EnableViewState="False" CssClass="Normal" Visible="false" />
    <table border="0" align="center">
	<tr><td class="jSG_btnBar" align="center">
	    <div class="jSG_btnBar_prev" style="display: block; "><asp:HyperLink ID="lnkPrevious" Runat="server" ResourceKey="Previous" CssClass="jSG_btn_Utility_prev">&#171; Previous</asp:HyperLink> </div>
	    <div class="jSG_btnBar_close" style="display: block; "><asp:HyperLink ID="lnkReturnToOrigin" Runat="server" CssClass="jSG_btn_Utility_close">Close</asp:HyperLink></div>
	    <div class="jSG_btnBar_next" style="display: block; "><asp:HyperLink ID="lnkNext" Runat="server" ResourceKey="Next" CssClass="jSG_btn_Utility_next">Next &#187;</asp:HyperLink></div></td></tr>
	</table>
</div>