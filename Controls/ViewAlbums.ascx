<%@ Control Language="vb" AutoEventWireup="false" CodeBehind="ViewAlbums.ascx.vb" Inherits="Ventrian.SimpleGallery.Controls.ViewAlbums" %>
<style>
.album-frame .top-x--- {background-image: url(<%= ResolveUrl("../images/borders/" & BorderStyle & "/album-tstretch.gif") %>);}
.album-frame .top--x-- {background-image: url(<%= ResolveUrl("../images/borders/" & BorderStyle & "/album-tstretch.gif") %>);}
.album-frame .top---x- {background-image: url(<%= ResolveUrl("../images/borders/" & BorderStyle & "/album-tstretch.gif") %>);}
.album-frame .mtpx---- {background-image: url(<%= ResolveUrl("../images/borders/" & BorderStyle & "/album-lstretch.gif") %>);}
.album-frame .mtp----x {background-image: url(<%= ResolveUrl("../images/borders/" & BorderStyle & "/album-rstretch.gif") %>);}
.album-frame .midx---- {background-image: url(<%= ResolveUrl("../images/borders/" & BorderStyle & "/album-lstretch.gif") %>);}
.album-frame .mid----x {background-image: url(<%= ResolveUrl("../images/borders/" & BorderStyle & "/album-rstretch.gif") %>);}
.album-frame .mbtx---- {background-image: url(<%= ResolveUrl("../images/borders/" & BorderStyle & "/album-lstretch.gif") %>);}
.album-frame .mbt----x {background-image: url(<%= ResolveUrl("../images/borders/" & BorderStyle & "/album-rstretch.gif") %>);}
.album-frame .bot-x--- {background-image: url(<%= ResolveUrl("../images/borders/" & BorderStyle & "/album-bstretch.gif") %>);}
.album-frame .bot--x-- {background-image: url(<%= ResolveUrl("../images/borders/" & BorderStyle & "/album-bstretch.gif") %>);}
.album-frame .bot---x- {background-image: url(<%= ResolveUrl("../images/borders/" & BorderStyle & "/album-bstretch.gif") %>);}
</style>

<asp:DataList ID="dlAlbum" runat="Server" cssclass="view" repeatColumns="2"
    repeatdirection="Horizontal" borderwidth="0" cellpadding="0" cellspacing="0" EnableViewState="False" ItemStyle-HorizontalAlign="Center" HorizontalAlign="Center">
    <ItemStyle cssClass="album-item" VerticalAlign="Top" />
    <ItemTemplate>
		<asp:PlaceHolder ID="phAlbum" Runat="server" EnableViewState="False" />
    </ItemTemplate>
</asp:DataList>
