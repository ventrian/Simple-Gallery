<%@ Control Language="vb" AutoEventWireup="false" CodeBehind="Approve.ascx.vb" Inherits="Ventrian.SimpleGallery.Approve" %>

<script language="JavaScript" type="text/javascript">
<!--
function openScreenWin(winName, loc, width, height){					// Opens a new window
	var newWindow;
	newWindow = window.open(loc,winName,"location=no,status=no,scrollbars=no,toolbar=no,menubar=no,directories=no,resizable=no,width="+width+",height="+height);		
	newWindow.focus();
}
//-->
</script>

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

<table cellpadding="0" cellspacing="0" width="100%">
<tr>
	<td align="left">
		<asp:Repeater ID="rptBreadCrumbs" Runat="server">
		<ItemTemplate><a href='<%# DataBinder.Eval(Container.DataItem, "Url") %>' class="NormalBold"><%# DataBinder.Eval(Container.DataItem, "Caption") %></a></ItemTemplate>
		<SeparatorTemplate>&nbsp;&#187;&nbsp;</SeparatorTemplate>
		</asp:Repeater>
	</td>
</tr>
</table>

<asp:Label ID="lblNoPhotos" Runat="server" ResourceKey="NoPhotos" EnableViewState="False" Visible="False" CssClass="NormalBold" />

<asp:datalist id="dlGallery" runat="server" RepeatColumns="1" RepeatDirection="Vertical" ItemStyle-VerticalAlign="Middle" ItemStyle-HorizontalAlign="center" cellpadding="2" cellspacing="4" CssClass="View" width="100%" borderwidth="0" DataKeyField="PhotoID">
<ItemTemplate>
<table border="0" cellpadding="0" cellspacing="0" width="100%">
<tr>
	<td width="25" align="center" valign="middle">
		<asp:checkbox id="chkApprove" runat="server" />
	</td>
	<td width='<%# GallerySettings.ThumbnailWidth + 25 %>' align="center" valign="middle">
		<table border="0" cellpadding="0" cellspacing="0">
		<tr>
			<td class="topx--"></td>
			<td class="top-x-"></td>
			<td class="top--x"></td>
		</tr>
		<tr>
			<td class="midx--"></td>
			<td><a href="<%# Me.GetPhotoUrl(DataBinder.Eval(Container.DataItem, "PhotoID").ToString()) %>"><img src="<%# GetPhotoPath(Container.DataItem) %>" class="photo_198" style="border:4px solid white" width="<%# GetPhotoWidth(Container.DataItem) %>" height="<%# GetPhotoHeight(Container.DataItem) %>" alt="<%# GetAlternateText(Container.DataItem) %>"><br></a></td>
			<td class="mid--x"></td>
		</tr>
		<tr>
			<td class="botx--"></td>
			<td class="bot-x-"></td>
			<td class="bot--x"></td>
		</tr>
		</table>
	</td>
	<td class="Normal" valign="top">
		<asp:HyperLink id="editLink" NavigateUrl='<%# EditURL("ItemID",DataBinder.Eval(Container.DataItem,"PhotoID")) %>' Visible="<%# IsPhotoEditable %>" runat="server"><asp:Image id="editLinkImage" AlternateText="Edit" Visible="<%# IsPhotoEditable %>" ImageUrl="~/images/edit.gif" runat="Server" resourcekey="Edit"/></asp:HyperLink>
		<span class="NormalBold"><%# DataBinder.Eval(Container.DataItem, "Name") %> </span>(<%# DataBinder.Eval(Container.DataItem, "AlbumName") %>)<br />
		<%# DataBinder.Eval(Container.DataItem, "Description") %><br /><br />
		<span class="Normal">Submitted by <%# DataBinder.Eval(Container.DataItem, "AuthorUserName") %> on <%# DataBinder.Eval(Container.DataItem, "DateCreated") %></span>
	</td>
</tr>
</table>
</ItemTemplate>
</asp:datalist>

<p>
	<asp:linkbutton id="cmdApproveSelected" resourcekey="cmdApproveSelected" runat="server" cssclass="CommandButton" text="Approve Selected" borderstyle="none" />
	&nbsp;
	<asp:linkbutton id="cmdApproveAll" resourcekey="cmdApproveAll" runat="server" cssclass="CommandButton" text="Approve All" causesvalidation="False" borderstyle="none" />
	&nbsp;
	<asp:linkbutton id="cmdRejectSelected" resourcekey="cmdRejectSelected" runat="server" cssclass="CommandButton" text="Reject Selected" causesvalidation="False" borderstyle="none" />	
	&nbsp;
	<asp:linkbutton id="cmdRejectAll" resourcekey="cmdRejectAll" runat="server" cssclass="CommandButton" text="Reject All" causesvalidation="False" borderstyle="none" />
</p>

<p class="NormalBold">
	<asp:Label ID="lblRejectMessage" Runat="server" ResourceKey="OptionalRejectMessage" EnableViewState="False" /> <br />
	<asp:textbox id="txtRejectMessage" cssclass="NormalTextBox" width="300" rows="5" textmode="MultiLine"
				maxlength="255" runat="server" />
</p>