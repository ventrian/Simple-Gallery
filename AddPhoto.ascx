<%@ Control Language="vb" AutoEventWireup="false" CodeBehind="AddPhoto.ascx.vb" Inherits="Ventrian.SimpleGallery.AddPhoto" %>
<%@ Register TagPrefix="dnn" TagName="Label" Src="~/controls/LabelControl.ascx" %>
<%@ Register TagPrefix="SimpleGallery" TagName="GalleryMenu" Src="Controls\GalleryMenu.ascx" %>
<%@ Register TagPrefix="SimpleGallery" TagName="EditPhotos" Src="Controls\EditPhotos.ascx" %>
<SimpleGallery:GalleryMenu ID="ucGalleryMenu" runat="server" ShowCommandBar="False" ShowSeparator="True" />

<div align="left">
    <table cellspacing="0" cellpadding="0" width="600" summary="Wizard Design Table">
        <tr>
            <td width="50" height="50" align="center" valign="middle">
                <asp:Image ID="imgStep" runat="server" ImageUrl="~/DesktopModules/SimpleGallery/Images/iconStep1.gif" Width="48" Height="48" />
            </td>
            <td>
                <asp:Label ID="lblStep" runat="server" CssClass="NormalBold" /><br />
                <asp:Label ID="lblStepDescription" runat="server" CssClass="Normal" /><br />
                <asp:Label ID="lblRequiresApproval" runat="server" CssClass="NormalRed" ResourceKey="RequiresApproval" EnableViewState="False" Visible="False" />
            </td>
        </tr>
    </table>
</div>
<hr size="1" />

<asp:Panel ID="pnlStep1" runat="server">
    <asp:PlaceHolder ID="phStep1a" runat="server">
        <table cellspacing="0" cellpadding="2" width="600" summary="Select Album Design Table" align="center">
            <tr>
                <td colspan="2">
                    <asp:RadioButton ID="rdoSelectExisting" runat="server" Text="Selecting Existing Album" ResourceKey="SelectExisting" CssClass="NormalBold" GroupName="Step1" /></td>
            </tr>
            <tr valign="top">
                <td class="SubHead" width="150">
                    <dnn:Label ID="plAlbum" runat="server" ControlName="drpAlbums" Suffix=":"></dnn:Label>
                </td>
                <td>
                    <asp:DropDownList ID="drpAlbums" runat="server" DataTextField="CaptionIndented" DataValueField="AlbumID" Width="300px" />
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
                <td colspan="2">
                    <asp:RadioButton ID="rdoCreateNew" runat="server" Text="Create New Album" ResourceKey="CreateNew" CssClass="NormalBold" GroupName="Step1" /></td>
            </tr>
            <tr valign="top" runat="server" id="trParentAlbum">
                <td class="SubHead" width="150">
                    <dnn:Label ID="plParentAlbum" runat="server" ResourceKey="ParentAlbum" Suffix=":" ControlName="drpParentAlbum"></dnn:Label>
                </td>
                <td>
                    <asp:DropDownList ID="drpParentAlbum" DataValueField="AlbumID" DataTextField="CaptionIndented" runat="server" Width="300px"></asp:DropDownList></td>
            </tr>
            <tr valign="top">
                <td class="SubHead" nowrap="nowrap" width="150">
                    <dnn:Label ID="plCaption" runat="server" ResourceKey="Caption" Suffix=":" ControlName="txtCaption"></dnn:Label>
                </td>
                <td align="left" width="450">
                    <asp:TextBox ID="txtCaption" CssClass="NormalTextBox" runat="server" Width="300" MaxLength="255"></asp:TextBox>
                    <asp:CustomValidator ID="valSelectNew" runat="server" CssClass="NormalRed" ResourceKey="valSelectNew" Display="Dynamic"
                        ErrorMessage="<br>You Must Enter a Valid Caption" ControlToValidate="txtCaption" ValidateEmptyText="true" />
                </td>
            </tr>
            <tr valign="top">
                <td class="SubHead" width="150">
                    <dnn:Label ID="plDescription" runat="server" Suffix=":" ControlName="txtDescription"></dnn:Label>
                </td>
                <td>
                    <asp:TextBox ID="txtDescription" CssClass="NormalTextBox" runat="server" Width="300" Columns="30"
                        MaxLength="255" TextMode="MultiLine" Rows="5"></asp:TextBox></td>
            </tr>
        </table>
    </asp:PlaceHolder>
</asp:Panel>
<asp:Panel ID="pnlStep2" runat="server">
    <asp:HiddenField ID="litBatchID" runat="server" />
    <div>
        <asp:FileUpload ID="fupFile" runat="server" AllowMultiple="true" />
        <asp:Button runat="server" ID="btnUploadFiles" OnClick="btnUploadFiles_OnClick" resourcekey="btnUploadFiles" />
    </div>
    <asp:Repeater runat="server" ID="addedPhotosRepeater" EnableViewState="True" OnItemDataBound="addedPhotosRepeater_OnItemDataBound">
        <ItemTemplate>
            <asp:Image runat="server" ID="addedPhoto" />
        </ItemTemplate>
    </asp:Repeater>
</asp:Panel>

<div style="width: 100%;">
    <SimpleGallery:EditPhotos ID="ucEditPhotos" runat="server" />
</div>

<asp:Panel ID="pnlWizard" runat="server">
    <div align="center">
        <br />
        <asp:ImageButton ID="imgPrevious" runat="server" ImageUrl="~\images\lt.gif" ImageAlign="AbsBottom" />
        <asp:LinkButton ID="cmdPrevious" resourcekey="PreviousStep" runat="server" CssClass="CommandButton" Text="Previous"
            BorderStyle="none" />
        <asp:ImageButton ID="imgCancel" runat="server" ImageUrl="~\DesktopModules\SimpleGallery\images\iconCancel.gif" ImageAlign="AbsBottom" CausesValidation="False" Style="padding-left: 20px;" />
        <asp:LinkButton ID="cmdCancel" runat="server" CssClass="CommandButton" ResourceKey="cmdCancel" Text="Cancel" BorderStyle="none" CausesValidation="False" Style="padding-right: 20px;" />
        <asp:LinkButton ID="cmdNext" resourcekey="NextStep" runat="server" CssClass="CommandButton" Text="Next" BorderStyle="none" />
        <asp:ImageButton ID="imgNext" runat="server" ImageUrl="~\images\rt.gif" ImageAlign="AbsBottom" />
    </div>
</asp:Panel>

<asp:Panel ID="pnlSave" runat="server">
    <p align="center">
        <br />
        <asp:ImageButton ID="imgSave" runat="server" ImageUrl="~\DesktopModules\SimpleGallery\images\iconSave.gif" ImageAlign="AbsBottom" />
        <asp:LinkButton ID="cmdSave" runat="server" CssClass="CommandButton" ResourceKey="cmdSave" Text="Save this batch" BorderStyle="none" />
    </p>
</asp:Panel>


<ul id="thumbnails" class="sg_photolist"></ul>
