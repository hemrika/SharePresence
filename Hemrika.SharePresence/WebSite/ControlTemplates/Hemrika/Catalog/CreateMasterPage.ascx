<%@ Assembly Name="Hemrika.SharePresence.WebSite, Version=1.0.0.0, Culture=neutral, PublicKeyToken=3421bd1d946bda6c" %>
<%@ Assembly Name="Microsoft.Web.CommandUI, Version=15.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %>
<%@ Register Tagprefix="SharePoint" Namespace="Microsoft.SharePoint.WebControls" Assembly="Microsoft.SharePoint, Version=15.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %>
<%@ Register Tagprefix="Utilities" Namespace="Microsoft.SharePoint.Utilities" Assembly="Microsoft.SharePoint, Version=15.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %>
<%@ Register Tagprefix="asp" Namespace="System.Web.UI" Assembly="System.Web.Extensions, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35" %>
<%@ Import Namespace="Microsoft.SharePoint" %> 
<%@ Register Tagprefix="WebPartPages" Namespace="Microsoft.SharePoint.WebPartPages" Assembly="Microsoft.SharePoint, Version=15.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %>
<%@ Register TagPrefix="wssuc" TagName="InputFormSection" Src="/_controltemplates/InputFormSection.ascx" %>
<%@ Register TagPrefix="wssuc" TagName="InputFormControl" Src="/_controltemplates/InputFormControl.ascx" %>
<%@ Register TagPrefix="wssuc" TagName="ButtonSection" Src="/_controltemplates/ButtonSection.ascx" %>
<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="CreateMasterPage.ascx.cs" Inherits="Hemrika.SharePresence.WebSite.ControlTemplates.CreateMasterPage" %>
				<fieldset id="Properties">
					<legend>PageLayout Properties</legend>
					<ol>
						<li><label for="forename" title="Enter your forename" class="required">Name</label>
                            <asp:TextBox ID="tbx_pagename" name="forename" runat="server"></asp:TextBox>.master
						</li>
						<li><label for="title" title="Enter your title" class="required">Title</label>
                            <asp:TextBox ID="tbx_title" name="title" runat="server"></asp:TextBox>
						</li>
						<li>
							<label for="description" title="Enter your description" class="required">Description</label>	
							<asp:TextBox ID="tbx_description" name="description" TextMode="MultiLine" runat="server"></asp:TextBox>
						</li>
					</ol>
				</fieldset>
				<fieldset id="ContentType">
					<legend>PageLayout ContentType</legend>
					<ol>
						<li><label for="contentgroup" title="Select your contentgroup" class="required">contentgroup<span>*</span></label>
                            <asp:DropDownList ID="ddl_contentgroup" name="contentgroup" runat="server" 
                                AutoPostBack="true"
                                onselectedindexchanged="ddl_contentgroup_SelectedIndexChanged" >
                            </asp:DropDownList>
						</li>
						<li><label for="contentname" title="Select your contentname" class="required">contentname<span>*</span></label>
                            <asp:DropDownList ID="ddl_contentname" name="contentname" runat="server" 
                                AutoPostBack="true" 
                                onselectedindexchanged="ddl_contentname_SelectedIndexChanged" >
                            </asp:DropDownList>
						</li>
   						<li>
							<label for="description" title="Enter your description" class="required">Description</label>	
							<asp:Label ID="lbl_description" name="description" TextMode="MultiLine"  runat="server"></asp:Label>
						</li>
					</ol>
				</fieldset>
                <asp:Button ID="Btn_Save" runat="server" onclick="Btn_Save_Click" />
                <asp:Button ID="btn_Cancel" runat="server" onclick="btn_Cancel_Click" />

