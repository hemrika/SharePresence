<%@ Assembly Name="Hemrika.SharePresence.WebSite, Version=1.0.0.0, Culture=neutral, PublicKeyToken=11e6604a27f32a11" %>
<%@ Assembly Name="Microsoft.Web.CommandUI, Version=15.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %>
<%@ Register TagPrefix="SharePoint" Namespace="Microsoft.SharePoint.WebControls"
    Assembly="Microsoft.SharePoint, Version=15.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %>
<%@ Register TagPrefix="Utilities" Namespace="Microsoft.SharePoint.Utilities" Assembly="Microsoft.SharePoint, Version=15.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %>
<%@ Register TagPrefix="asp" Namespace="System.Web.UI" Assembly="System.Web.Extensions, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35" %>
<%@ Import Namespace="Microsoft.SharePoint" %>
<%@ Register TagPrefix="WebPartPages" Namespace="Microsoft.SharePoint.WebPartPages"
    Assembly="Microsoft.SharePoint, Version=15.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %>
<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="CreateWebPage.ascx.cs"
    Inherits="Hemrika.SharePresence.WebSite.ControlTemplates.CreateWebPage" %>
<div data-role="page active" class="type-interior" data-theme="b">
    <div data-role="header" data-theme="b" data-position="fixed">
        <span class="ui-app-title">Create a Webpage</span>
    </div>
    <div data-role="content" data-theme="b">
        <h2>
            WebPage Properties</h2>
        <div data-role="fieldcontain">
            <label for="library" title="Select the library" class="required">
                library</label>
            <asp:DropDownList ID="ddl_library" name="library" runat="server" AutoPostBack="true"
                OnSelectedIndexChanged="ddl_library_SelectedIndexChanged">
            </asp:DropDownList>
            <label for="forename" title="Enter your forename" class="required">
                Name</label>
            <asp:TextBox ID="tbx_pagename" name="forename" runat="server"></asp:TextBox>.aspx
            <label for="title" title="Enter your title" class="required">
                Title</label>
            <asp:TextBox ID="tbx_title" name="title" runat="server"></asp:TextBox>
            <label for="description" title="Enter your description" class="required">
                Description</label>
            <asp:TextBox ID="tbx_description" name="description" TextMode="MultiLine" runat="server"></asp:TextBox>
        </div>
        <br />
        <h2>
            Webpage ContentType and Design</h2>
        <div data-role="fieldcontain">
            <label for="contentgroup" title="Select your contentgroup" class="required">
                contentgroup<span>*</span></label>
            <asp:DropDownList ID="ddl_contentgroup" name="contentgroup" runat="server" AutoPostBack="true"
                OnSelectedIndexChanged="ddl_contentgroup_SelectedIndexChanged">
            </asp:DropDownList>
            <label for="contentname" title="Select your contentname" class="required">
                contentname<span>*</span></label>
            <asp:DropDownList ID="ddl_contentname" name="contentname" runat="server" AutoPostBack="true"
                OnSelectedIndexChanged="ddl_contentname_SelectedIndexChanged">
            </asp:DropDownList>
            <label for="pagelayout" title="Select your pagelayout" class="required">
                Pagelayout<span>*</span></label>
            <asp:DropDownList ID="ddl_pagelayout" name="pagelayout" runat="server" OnSelectedIndexChanged="ddl_pagelayout_SelectedIndexChanged">
            </asp:DropDownList>
            <label for="description" title="Enter your description" class="required">
                Description</label>
            <asp:Label ID="lbl_description" name="description" TextMode="MultiLine" runat="server"></asp:Label>
        </div>
        <asp:Button ID="Btn_Save" runat="server" OnClick="Btn_Save_Click" Text="Save" data-inline="true"
            CssClass="ui-btn-up-b" />
        <asp:Button ID="btn_Cancel" runat="server" OnClick="btn_Cancel_Click" Text="Cancel"
            data-inline="true" CssClass="ui-btn-up-b" />
    </div>
</div>
