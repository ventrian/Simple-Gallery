<%@ Control Language="vb" AutoEventWireup="false" CodeBehind="EditAlbum.ascx.vb" Inherits="Ventrian.SimpleGallery.EditAlbum" %>
<%@ Register TagPrefix="dnn" TagName="Label" Src="~/controls/LabelControl.ascx" %>
<%@ Register TagPrefix="dnn" TagName="SectionHead" Src="~/controls/SectionHeadControl.ascx" %>
<%@ Register TagPrefix="SimpleGallery" TagName="GalleryMenu" Src="Controls\GalleryMenu.ascx"%>
<SimpleGallery:GalleryMenu id="ucGalleryMenu" runat="server" ShowCommandBar="False" />

<table class="Settings" cellspacing="2" cellpadding="2" summary="Edit Album Design Table"
	border="0" width="100%">
	<tr>
		<td valign="top">
			<asp:panel id="pnlSettings" runat="server" cssclass="WorkPanel" visible="True">
				<dnn:sectionhead id="dshAlbum" cssclass="Head" runat="server" includerule="True" resourcekey="AlbumSettings"
					section="tblAlbum" text="Album Settings"></dnn:sectionhead>
				<asp:label id="lblAlbumSettingsHelp" cssclass="Normal" runat="server" resourcekey="AlbumSettingsDescription"
								enableviewstate="False"></asp:label>
			    <div align="center">
				    <table id="tblAlbum" cellspacing="0" cellpadding="2" width="600" summary="Album Settings Design Table"
					    border="0" runat="server">
					    <tr valign="top">
						    <td class="SubHead" width="150">
							    <dnn:label id="plParentAlbum" runat="server" resourcekey="ParentAlbum" suffix=":" controlname="drpParentAlbum"></dnn:label></td>
						    <td align="left">
							    <asp:DropDownList id="drpParentAlbum" DataValueField="AlbumID" DataTextField="CaptionIndented" Runat="server" width="300"></asp:DropDownList>
							    <asp:CustomValidator id="valInvalidParentAlbum" runat="server" ErrorMessage="<br>Invalid Parent Album. Possible Loop Detected."
								    ResourceKey="valInvalidParentAlbum" ControlToValidate="drpParentAlbum" CssClass="NormalRed" Display="Dynamic"></asp:CustomValidator></td>
					    </tr>
					    <tr valign="top">
						    <td class="SubHead" nowrap="nowrap" width="150">
							    <dnn:label id="plCaption" runat="server" resourcekey="Caption" suffix=":" controlname="txtCaption"></dnn:label></td>
						    <td align="left">
							    <asp:textbox id="txtCaption" cssclass="NormalTextBox" runat="server" width="300" columns="30"
								    maxlength="255"></asp:textbox>
							    <asp:requiredfieldvalidator id="valCaption" cssclass="NormalRed" runat="server" resourcekey="valCaption" display="Dynamic"
								    errormessage="<br>You Must Enter a Valid Caption" controltovalidate="txtCaption"></asp:requiredfieldvalidator></td>
					    </tr>
					    <tr valign="top">
						    <TD class="SubHead" width="150">
							    <dnn:label id="plDescription" runat="server" suffix=":" controlname="txtDescription"></dnn:label></TD>
						    <TD align="left">
							    <asp:textbox id="txtDescription" cssclass="NormalTextBox" runat="server" width="300" columns="30"
								    textmode="MultiLine" rows="5"></asp:textbox></TD>
					    </tr>
					    <tr>
	                        <td class="SubHead" width="150"><dnn:label id="plInheritSecurity" resourcekey="InheritSecurity" runat="server" controlname="chkInheritSecurity" suffix=":"></dnn:label></td>
	                        <td>
	                            <asp:CheckBox ID="chkInheritSecurity" runat="server" AutoPostBack="true" Checked="true" />
	                        </td>
	                    </tr>
	                    <tr runat="server" id="trPermissions" visible="false">
	                        <td class="SubHead" width="150"><dnn:label id="plPermissions" resourcekey="Permissions" runat="server" controlname="chkPermissions" suffix=":"></dnn:label></td>
	                        <td>
                                <asp:DataGrid ID="grdAlbumPermissions" Runat="server" AutoGenerateColumns="False" ItemStyle-CssClass="Normal"
                                    ItemStyle-HorizontalAlign="Center" ItemStyle-VerticalAlign="Middle" HeaderStyle-HorizontalAlign="Center"
                                    HeaderStyle-CssClass="NormalBold" CellSpacing="0" CellPadding="0" GridLines="None" BorderWidth="1"
                                    BorderStyle="None" DataKeyField="Value">
                                    <Columns>
                                        <asp:TemplateColumn>
	                                        <ItemStyle HorizontalAlign="Left" Wrap="False"/>
	                                        <ItemTemplate>
		                                        <%# DataBinder.Eval(Container.DataItem, "Text") %>
	                                        </ItemTemplate>
                                        </asp:TemplateColumn>
                                        <asp:TemplateColumn>
	                                        <HeaderTemplate>
		                                        &nbsp;
		                                        <asp:Label ID="lblAdd" Runat="server" EnableViewState="False" ResourceKey="Add" />&nbsp;
	                                        </HeaderTemplate>
	                                        <ItemTemplate>
		                                        <asp:CheckBox ID="chkAdd" Runat="server" />
	                                        </ItemTemplate>
                                        </asp:TemplateColumn>	
                                        <asp:TemplateColumn>
	                                        <HeaderTemplate>
		                                        &nbsp;
		                                        <asp:Label ID="lblEdit" Runat="server" EnableViewState="False" ResourceKey="Edit" />&nbsp;
	                                        </HeaderTemplate>
	                                        <ItemTemplate>
		                                        <asp:CheckBox ID="chkEdit" Runat="server" />
	                                        </ItemTemplate>
                                        </asp:TemplateColumn>	
                                        <asp:TemplateColumn>
	                                        <HeaderTemplate>
		                                        &nbsp;
		                                        <asp:Label ID="lblDelete" Runat="server" EnableViewState="False" ResourceKey="Delete" />&nbsp;
	                                        </HeaderTemplate>
	                                        <ItemTemplate>
		                                        <asp:CheckBox ID="chkDelete" Runat="server" />
	                                        </ItemTemplate>
                                        </asp:TemplateColumn>	
                                    </Columns>
                                </asp:DataGrid>
	                        </td>
	                    </tr>
                    </table>
				</div>
				<dnn:sectionhead id="dshAdvanced" cssclass="Head" runat="server" includerule="True" resourcekey="AdvancedSettings"
					section="tblAdvanced" text="Advanced Settings" IsExpanded="false"></dnn:sectionhead>
				<div align="center">
                <table id="tblAdvanced" cellspacing="0" cellpadding="2" width="600" summary="Album Settings Design Table"
					border="0" runat="server">
					<tr valign="top">
						<td class="SubHead" nowrap="nowrap" width="150">
							<dnn:label id="plIsPublic" runat="server" resourcekey="IsPublic" suffix=":" controlname="chkIsPublic"></dnn:label></td>
						<td align="left">
							<asp:CheckBox id="chkIsPublic" cssclass="NormalTextBox" runat="server" Checked="True"></asp:CheckBox></td>
					</tr>
					<TR valign="top">
						<TD class="SubHead" nowrap="nowrap" width="150">
							<dnn:label id="plHomeDirectory" runat="server" resourcekey="HomeDirectory" suffix=":" controlname="txtHomeDirectory"></dnn:label></TD>
						<td align="left">
							<asp:textbox id="txtHomeDirectory" cssclass="NormalTextBox" runat="server" width="300" 
								maxlength="255" Enabled="False"></asp:textbox>&nbsp;<asp:linkbutton id="cmdCustomize" cssclass="CommandButton" runat="server" resourcekey="cmdCustomize"
								text="Customize" CausesValidation="False"></asp:linkbutton>
							<asp:requiredfieldvalidator id="valHomeDirectory" cssclass="NormalRed" runat="server" resourcekey="valHomeDirectory"
								display="Dynamic" errormessage="<br>You Must Enter a Home Directory" controltovalidate="txtHomeDirectory"></asp:requiredfieldvalidator>
							</td>
					</TR>
					<tr id="trDefaultPhoto" runat="server">
						<td class="SubHead" width="150" valign="top"><dnn:label id="plDefaultPhoto" runat="server" suffix=":" controlname="drpDefaultPhoto"></dnn:label></td>
						<td align="left"><asp:DropDownList id="drpDefaultPhoto" Runat="server" CssClass="NormalTextBox" DataTextField="Name" DataValueField="PhotoID"></asp:DropDownList></td>
					</tr>
					<tr>
						<td class="SubHead" width="150" valign="top"><dnn:label id="plCreateDate" runat="server" controlname="txtCreateDate" text="Create Date:"></dnn:label></td>
						<td align="left">
						    <asp:DropDownList ID="drpCreateTimeHour" Runat="server">
							    <asp:ListItem Value="0">0</asp:ListItem>
							    <asp:ListItem Value="1">1</asp:ListItem>
							    <asp:ListItem Value="2">2</asp:ListItem>
							    <asp:ListItem Value="3">3</asp:ListItem>
							    <asp:ListItem Value="4">4</asp:ListItem>
							    <asp:ListItem Value="5">5</asp:ListItem>
							    <asp:ListItem Value="6">6</asp:ListItem>
							    <asp:ListItem Value="7">7</asp:ListItem>
							    <asp:ListItem Value="8">8</asp:ListItem>
							    <asp:ListItem Value="9">9</asp:ListItem>
							    <asp:ListItem Value="10">10</asp:ListItem>
							    <asp:ListItem Value="11">11</asp:ListItem>
							    <asp:ListItem Value="12">12</asp:ListItem>
							    <asp:ListItem Value="13">13</asp:ListItem>
							    <asp:ListItem Value="14">14</asp:ListItem>
							    <asp:ListItem Value="15">15</asp:ListItem>
							    <asp:ListItem Value="16">16</asp:ListItem>
							    <asp:ListItem Value="17">17</asp:ListItem>
							    <asp:ListItem Value="18">18</asp:ListItem>
							    <asp:ListItem Value="19">19</asp:ListItem>
							    <asp:ListItem Value="20">20</asp:ListItem>
							    <asp:ListItem Value="21">21</asp:ListItem>
							    <asp:ListItem Value="22">22</asp:ListItem>
							    <asp:ListItem Value="23">23</asp:ListItem>
						    </asp:DropDownList>
						    :
						    <asp:DropDownList ID="drpCreateTimeMinute" Runat="server">
							    <asp:ListItem Value="0">00</asp:ListItem>
							    <asp:ListItem Value="1">01</asp:ListItem>
							    <asp:ListItem Value="2">02</asp:ListItem>
							    <asp:ListItem Value="3">03</asp:ListItem>
							    <asp:ListItem Value="4">04</asp:ListItem>
							    <asp:ListItem Value="5">05</asp:ListItem>
							    <asp:ListItem Value="6">06</asp:ListItem>
							    <asp:ListItem Value="7">07</asp:ListItem>
							    <asp:ListItem Value="8">08</asp:ListItem>
							    <asp:ListItem Value="9">09</asp:ListItem>
							    <asp:ListItem Value="10">10</asp:ListItem>
							    <asp:ListItem Value="11">11</asp:ListItem>
							    <asp:ListItem Value="12">12</asp:ListItem>
							    <asp:ListItem Value="13">13</asp:ListItem>
							    <asp:ListItem Value="14">14</asp:ListItem>
							    <asp:ListItem Value="15">15</asp:ListItem>
							    <asp:ListItem Value="16">16</asp:ListItem>
							    <asp:ListItem Value="17">17</asp:ListItem>
							    <asp:ListItem Value="18">18</asp:ListItem>
							    <asp:ListItem Value="19">19</asp:ListItem>
							    <asp:ListItem Value="20">20</asp:ListItem>
							    <asp:ListItem Value="21">21</asp:ListItem>
							    <asp:ListItem Value="22">22</asp:ListItem>
							    <asp:ListItem Value="23">23</asp:ListItem>
							    <asp:ListItem Value="24">24</asp:ListItem>
							    <asp:ListItem Value="25">25</asp:ListItem>
							    <asp:ListItem Value="26">26</asp:ListItem>
							    <asp:ListItem Value="27">27</asp:ListItem>
							    <asp:ListItem Value="28">28</asp:ListItem>
							    <asp:ListItem Value="29">29</asp:ListItem>
							    <asp:ListItem Value="30">30</asp:ListItem>
							    <asp:ListItem Value="31">31</asp:ListItem>
							    <asp:ListItem Value="32">32</asp:ListItem>
							    <asp:ListItem Value="33">33</asp:ListItem>
							    <asp:ListItem Value="34">34</asp:ListItem>
							    <asp:ListItem Value="35">35</asp:ListItem>
							    <asp:ListItem Value="36">36</asp:ListItem>
							    <asp:ListItem Value="37">37</asp:ListItem>
							    <asp:ListItem Value="38">38</asp:ListItem>
							    <asp:ListItem Value="39">39</asp:ListItem>
							    <asp:ListItem Value="40">40</asp:ListItem>
							    <asp:ListItem Value="41">41</asp:ListItem>
							    <asp:ListItem Value="42">42</asp:ListItem>
							    <asp:ListItem Value="43">43</asp:ListItem>
							    <asp:ListItem Value="44">44</asp:ListItem>
							    <asp:ListItem Value="45">45</asp:ListItem>
							    <asp:ListItem Value="46">46</asp:ListItem>
							    <asp:ListItem Value="47">47</asp:ListItem>
							    <asp:ListItem Value="48">48</asp:ListItem>
							    <asp:ListItem Value="49">49</asp:ListItem>
							    <asp:ListItem Value="50">50</asp:ListItem>
							    <asp:ListItem Value="51">51</asp:ListItem>
							    <asp:ListItem Value="52">52</asp:ListItem>
							    <asp:ListItem Value="53">53</asp:ListItem>
							    <asp:ListItem Value="54">54</asp:ListItem>
							    <asp:ListItem Value="55">55</asp:ListItem>
							    <asp:ListItem Value="56">56</asp:ListItem>
							    <asp:ListItem Value="57">57</asp:ListItem>
							    <asp:ListItem Value="58">58</asp:ListItem>
							    <asp:ListItem Value="59">59</asp:ListItem>
						    </asp:DropDownList>
							<asp:textbox id="txtCreateDate" runat="server" cssclass="NormalTextBox" width="120" columns="30" maxlength="11"></asp:textbox>&nbsp;
							<asp:hyperlink id="cmdCreateCalendar" cssclass="CommandButton" runat="server" resourcekey="Calendar">Calendar</asp:hyperlink>
							<asp:CompareValidator ID="valtxtCreateDate" ControlToValidate="txtCreateDate" Operator="DataTypeCheck" Type="Date"
								Runat="server" Display="Dynamic" ErrorMessage="<br>Invalid Create Date" resourcekey="valCreateDate.ErrorMessage" />
						</td>
					</tr>
				</table>
				</div>
			</asp:panel>
		</td>
	</tr>
</table>
<p align="center">
    <br />
	<asp:linkbutton id="cmdUpdate" resourcekey="cmdUpdate" runat="server" cssclass="CommandButton" text="Update"
		borderstyle="none" />
	&nbsp;
	<asp:linkbutton id="cmdCancel" resourcekey="cmdCancel" runat="server" cssclass="CommandButton" text="Cancel"
		causesvalidation="False" borderstyle="none" />
</p>
<asp:Panel ID="pnlDelete" runat="server">
<table class="Settings" cellspacing="2" cellpadding="2" summary="Delete Album Design Table"
	border="0" width="100%">
	<tr>
		<td width="100%" valign="top">
			<asp:panel id="pnlDeleteTable" runat="server" cssclass="WorkPanel" visible="True">
				<dnn:sectionhead id="dshDelete" cssclass="Head" runat="server" includerule="True" resourcekey="DeleteAlbum"
					section="tblDeleteAlbum" text="Delete Album" isexpanded="false"></dnn:sectionhead>
				<div align="center">
				    <table id="tblDeleteAlbum" cellspacing="0" cellpadding="2" width="600" summary="Delete Album Design Table"
					    border="0" runat="server">
                    <tr>
                        <td class="SubHead" width="150">
							    <dnn:label id="plDeletePhysicalFiles" runat="server" suffix=":" controlname="chkDeletePhysicalFiles"></dnn:label></td>
                        <td align="left"><asp:CheckBox id="chkDeletePhysicalFiles" runat="server" Checked="True" /></td>
                    </tr>
                    <tr>
                        <td colspan="2" align="center">
                            <asp:linkbutton id="cmdDelete" resourcekey="cmdDelete" runat="server" cssclass="CommandButton" text="Delete"
		                        causesvalidation="False" borderstyle="none" />
                        </td>
                    </tr>
                    </table>
                </div>
            </asp:panel>
        </td>
    </tr>
</table> 
</asp:Panel>
<asp:Panel ID="pnlSynch" Runat="server">
<table class="Settings" cellspacing="2" cellpadding="2" summary="Synchronize Design Table"
	border="0" width="100%">
	<tr>
		<td width="100%" valign="top">
			<asp:panel id="pnlSynchronize" runat="server" cssclass="WorkPanel" visible="True">
				<dnn:sectionhead id="dshSynchronize" cssclass="Head" runat="server" includerule="True" resourcekey="SynchronizeSettings"
					section="tblSynchronize" text="Synchronize Settings" IsExpanded="false"></dnn:sectionhead>
				
				<div align="center">
				    <table id="tblSynchronize" cellspacing="0" cellpadding="2" width="600" summary="Synchronize Settings Design Table"
					    border="0" runat="server">
					    <TR valign="top">
						    <TD class="SubHead" width="150">
							    <dnn:label id="plIncludeSubFolders" runat="server" resourcekey="IncludeSubFolders" controlname="chkIncludeSubFolders"></dnn:label></TD>
						    <TD align="left">
							    <asp:CheckBox ID="chkIncludeSubFolders" Runat="server" Checked="True" />
						    </TD>
					    </TR>
					    <TR valign="top">
						    <TD class="SubHead" width="150">
							    <dnn:label id="plAbsoluteSync" runat="server" resourcekey="plAbsoluteSync" controlname="chkAbsoluteSync"></dnn:label></TD>
						    <TD align="left">
							    <asp:CheckBox ID="chkAbsoluteSync" Runat="server" /><IMG height=1 src='<%= Page.ResolveUrl("~/Images/Spacer.gif") %>' width=350>
						    </TD>
					    </TR>
					    <TR valign="top">
						    <TD class="SubHead" width="150">
							    <dnn:label id="plResizeImages" runat="server" resourcekey="ResizeImages" controlname="chkResizeImages"></dnn:label></TD>
						    <TD align="left">
							    <asp:CheckBox ID="chkResizeImages" Runat="server" Checked="False" /><IMG height=1 src='<%= Page.ResolveUrl("~/Images/Spacer.gif") %>' width=350>
						    </TD>
					    </TR>
					    <TR valign="top">
						    <TD class="SubHead" width="150">
							    <dnn:label id="plAsynchronous" runat="server" resourcekey="Asynchronous" controlname="chkAsynchronous"></dnn:label></TD>
						    <TD align="left">
							    <asp:CheckBox ID="chkAsynchronous" Runat="server" Checked="False" />
						    </TD>
					    </TR>
					    <tr>
					        <td colspan="2" align="center">
				        	    <asp:linkbutton id="cmdSynchronize" resourcekey="cmdSynchronize" runat="server" cssclass="CommandButton"
	                                text="Synchronize Folder" causesvalidation="False" borderstyle="none" />
                                &nbsp;
                                <asp:Label ID="lblSynchResults" Runat="server" CssClass="Normal" EnableViewState="False" />
					        </td>
					    </tr>
				    </table>
				</div>
			</asp:Panel>
		</td>
	</tr>
</table>
</asp:Panel>
