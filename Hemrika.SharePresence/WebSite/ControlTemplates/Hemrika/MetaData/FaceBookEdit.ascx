<%@ Assembly Name="Hemrika.SharePresence.WebSite, Version=1.0.0.0, Culture=neutral, PublicKeyToken=3421bd1d946bda6c" %>
<%@ Assembly Name="Microsoft.Web.CommandUI, Version=15.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %>
<%@ Register TagPrefix="SharePoint" Namespace="Microsoft.SharePoint.WebControls"
    Assembly="Microsoft.SharePoint, Version=15.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %>
<%@ Register TagPrefix="Utilities" Namespace="Microsoft.SharePoint.Utilities" Assembly="Microsoft.SharePoint, Version=15.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %>
<%@ Register TagPrefix="asp" Namespace="System.Web.UI" Assembly="System.Web.Extensions, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35" %>
<%@ Import Namespace="Microsoft.SharePoint" %>
<%@ Register TagPrefix="WebPartPages" Namespace="Microsoft.SharePoint.WebPartPages"
    Assembly="Microsoft.SharePoint, Version=15.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %>
<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="FaceBookEdit.ascx.cs"
    Inherits="Hemrika.SharePresence.WebSite.ControlTemplates.FaceBookEdit" %>
<div data-role="page active" class="type-interior" data-theme="b">
    <div data-role="header" data-theme="b" data-position="fixed">
        <span class="ui-app-title">MetaData Settings</span>
    </div>
    <div data-role="content" data-theme="b">
        <div data-role="fieldcontain" id="Fields" runat="server">
        </div>
        <asp:Button ID="btn_Save" class="ms-ButtonHeightWidth" runat="server" Text="Save" data-icon="check" data-inline="true" CssClass="ui-btn-up-b" data-iconpos="left"/>
        <asp:Button ID="btn_Cancel" class="ms-ButtonHeightWidth" runat="server" Text="Cancel"
        OnClientClick="SP.UI.ModalDialog.commonModalDialogClose(SP.UI.DialogResult.CANCEL,'No changes have been saved.'); return false;"
        data-icon="back" data-inline="true" CssClass="ui-btn-up-b" data-iconpos="left"/>
    </div>
</div>
