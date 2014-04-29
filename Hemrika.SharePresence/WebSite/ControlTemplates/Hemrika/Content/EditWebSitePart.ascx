<%@ Assembly Name="Hemrika.SharePresence.WebSite, Version=1.0.0.0, Culture=neutral, PublicKeyToken=3421bd1d946bda6c" %>
<%@ Assembly Name="Microsoft.Web.CommandUI, Version=15.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %>
<%@ Register Tagprefix="SharePoint" Namespace="Microsoft.SharePoint.WebControls" Assembly="Microsoft.SharePoint, Version=15.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %>
<%@ Register Tagprefix="Utilities" Namespace="Microsoft.SharePoint.Utilities" Assembly="Microsoft.SharePoint, Version=15.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %>
<%@ Register Tagprefix="asp" Namespace="System.Web.UI" Assembly="System.Web.Extensions, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35" %>
<%@ Register TagPrefix="HTML5" Assembly="Hemrika.SharePresence.HTML5, Version=1.0.0.0, Culture=neutral, PublicKeyToken=3421bd1d946bda6c" Namespace="Hemrika.SharePresence.Html5.WebControls" %>
<%@ Import Namespace="Microsoft.SharePoint" %> 
<%@ Register Tagprefix="WebPartPages" Namespace="Microsoft.SharePoint.WebPartPages" Assembly="Microsoft.SharePoint, Version=15.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %>
<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="EditWebSitePart.ascx.cs" Inherits="Hemrika.SharePresence.WebSite.ControlTemplates.EditWebSitePart" %>
<HTML5:Section ID="section" runat="server"></HTML5:Section>
	<asp:Button ID="Btn_Save" runat="server" onclick="Btn_Save_Click" Text="Save" data-inline="true" CssClass="ui-btn-up-b" />
    <asp:Button ID="Btn_Cancel" runat="server" onclick="Btn_Cancel_Click" CausesValidation="False" Text="Cancel" data-inline="true" CssClass="ui-btn-up-b"/>
