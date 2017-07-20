<%@ Control Language="vb" AutoEventWireup="false" CodeBehind="RandomPhoto.ascx.vb" Inherits="Ventrian.SimpleGallery.RandomPhoto" %>
<asp:PlaceHolder ID="phjQueryScripts" Runat="server" EnableViewState="False" />
<asp:PlaceHolder ID="phStyles" runat="server">
<style>
.photo-frame .topx-- { background-image: url(<%= FormatBorderPath("frame-topx--.gif") %>); }
.photo-frame .top-x- { background-image: url(<%= FormatBorderPath("frame-top-x-.gif") %>); }
.photo-frame .top--x { background-image: url(<%= FormatBorderPath("frame-top--x.gif") %>); }
.photo-frame .midx-- { background-image: url(<%= FormatBorderPath("frame-midx--.gif") %>); }
.photo-frame .mid--x { background-image: url(<%= FormatBorderPath("frame-mid--x.gif") %>); }
.photo-frame .botx-- { background-image: url(<%= FormatBorderPath("frame-botx--.gif") %>); }
.photo-frame .bot-x- { background-image: url(<%= FormatBorderPath("frame-bot-x-.gif") %>); }
.photo-frame .bot--x { background-image: url(<%= FormatBorderPath("frame-bot--x.gif") %>); }
.album-frame .top-x--- {background-image: url(<%= FormatBorderPath("album-tstretch.gif") %>);}
.album-frame .top--x-- {background-image: url(<%= FormatBorderPath("album-tstretch.gif") %>);}
.album-frame .top---x- {background-image: url(<%= FormatBorderPath("album-tstretch.gif") %>);}
.album-frame .mtpx---- {background-image: url(<%= FormatBorderPath("album-lstretch.gif") %>);}
.album-frame .mtp----x {background-image: url(<%= FormatBorderPath("album-rstretch.gif") %>);}
.album-frame .midx---- {background-image: url(<%= FormatBorderPath("album-lstretch.gif") %>);}
.album-frame .mid----x {background-image: url(<%= FormatBorderPath("album-rstretch.gif") %>);}
.album-frame .mbtx---- {background-image: url(<%= FormatBorderPath("album-lstretch.gif") %>);}
.album-frame .mbt----x {background-image: url(<%= FormatBorderPath("album-rstretch.gif") %>);}
.album-frame .bot-x--- {background-image: url(<%= FormatBorderPath("album-bstretch.gif") %>);}
.album-frame .bot--x-- {background-image: url(<%= FormatBorderPath("album-bstretch.gif") %>);}
.album-frame .bot---x- {background-image: url(<%= FormatBorderPath("album-bstretch.gif") %>);}
</style>
</asp:PlaceHolder>
<asp:PlaceHolder ID="phLightboxTop" runat="Server" />
<asp:datalist id="dlGallery" runat="server" RepeatColumns="3" HorizontalAlign="Center" ItemStyle-VerticalAlign="Middle" ItemStyle-HorizontalAlign="Center" CssClass="RandomView" EnableViewState="False">
<ItemTemplate>
	<asp:PlaceHolder ID="phPhoto" Runat="server" />
</ItemTemplate>
</asp:datalist>
<asp:Repeater ID="rptGallery" runat="server"><ItemTemplate><asp:PlaceHolder ID="phPhoto" Runat="server" /></ItemTemplate></asp:Repeater>
<asp:PlaceHolder ID="phLightboxBottom" runat="Server" />
