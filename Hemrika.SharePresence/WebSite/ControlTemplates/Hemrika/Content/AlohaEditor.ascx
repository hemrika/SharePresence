<%@ Assembly Name="Hemrika.SharePresence.WebSite, Version=1.0.0.0, Culture=neutral, PublicKeyToken=3421bd1d946bda6c" %>
<%@ Assembly Name="Microsoft.Web.CommandUI, Version=15.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %>
<%@ Register TagPrefix="SharePoint" Namespace="Microsoft.SharePoint.WebControls"
    Assembly="Microsoft.SharePoint, Version=15.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %>
<%@ Register TagPrefix="Utilities" Namespace="Microsoft.SharePoint.Utilities" Assembly="Microsoft.SharePoint, Version=15.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %>
<%@ Register TagPrefix="asp" Namespace="System.Web.UI" Assembly="System.Web.Extensions, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35" %>
<%@ Import Namespace="Microsoft.SharePoint" %>
<%@ Register TagPrefix="WebPartPages" Namespace="Microsoft.SharePoint.WebPartPages"
    Assembly="Microsoft.SharePoint, Version=15.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %>
<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="AlohaEditor.ascx.cs"
    Inherits="Hemrika.SharePresence.WebSite.ControlTemplates.AlohaEditor" %>
<div data-role="page active" class="type-interior" data-theme="b">
    <div data-role="header" data-theme="b" data-position="fixed">
        <span class="ui-app-title">Editor Properties</span>
    </div>
    <div data-role="fieldcontain" data-theme="b">
        <h2>Common Plugins</h2>
        <div data-role="fieldcontain" >
            <asp:CheckBoxList ID="cbl_Common" runat="server" RepeatColumns="5" 
                RepeatDirection="Horizontal" >
            </asp:CheckBoxList>
        </div>
        <h2>Extra Plugins</h2>
        <div data-role="fieldcontain">
            <asp:CheckBoxList ID="cbl_Extra" runat="server" RepeatColumns="5" 
                RepeatDirection="Horizontal" >
            </asp:CheckBoxList>
        </div>
        <br />
        <asp:Button ID="Btn_Save" runat="server" OnClick="Btn_Save_Click" Text="Save" data-inline="true"
            CssClass="ui-btn-up-b" />
        <asp:Button ID="btn_Cancel" runat="server" OnClick="btn_Cancel_Click" Text="Cancel"
            data-inline="true" CssClass="ui-btn-up-b" />
    </div>
</div>
