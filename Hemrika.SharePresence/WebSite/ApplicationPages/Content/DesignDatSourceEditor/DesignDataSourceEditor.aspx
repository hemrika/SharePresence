<%@ Assembly Name="Hemrika.SharePresence.WebSite, Version=1.0.0.0, Culture=neutral, PublicKeyToken=11e6604a27f32a11" %>
<%@ Page Language="C#" MasterPageFile="~/_layouts/application.master" Inherits="Hemrika.SharePresence.WebSite.DesignDataSourceEditor" %>
<%@ Register TagPrefix="SharePoint" Namespace="Microsoft.SharePoint.WebControls" Assembly="Microsoft.SharePoint, Version=15.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %>
<%@ Register TagPrefix="Utilities" Namespace="Microsoft.SharePoint.Utilities" Assembly="Microsoft.SharePoint, Version=15.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %>
<%@ Register TagPrefix="SharePresence" TagName="DesignDataSourceEditor" Src="/_controltemplates/15/Hemrika/Content/DesignDataSourceEditor.ascx" %>
<asp:Content ID="PageHead" ContentPlaceHolderID="PlaceHolderAdditionalPageHead" runat="server">
</asp:Content>
<asp:Content ContentPlaceHolderId="PlaceHolderPageTitle" runat="server">
</asp:Content>
<asp:Content ContentPlaceHolderId="PlaceHolderPageTitleInTitleArea" runat="server">
</asp:Content>
<asp:Content ID="PageDescription" ContentPlaceHolderID="PlaceHolderPageDescription"  runat="server">
</asp:Content>
<asp:Content ID="Main" ContentPlaceHolderID="PlaceHolderMain" runat="server">
    <SharePresence:DesignDataSourceEditor ID="DesignDataSourceEditor" runat="server" ></SharePresence:DesignDataSourceEditor>
    <SharePoint:FormDigest ID="FormDigest1" runat="server" />
</asp:Content>

