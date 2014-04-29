<%@ Page language="C#" MasterPageFile="~masterurl/custom.master"   Inherits="Hemrika.SharePresence.WebSite.Page.WebSitePage,Hemrika.SharePresence.WebSite,Version=1.0.0.0,Culture=neutral,PublicKeyToken=3421bd1d946bda6c" meta:progid="WebSite.WebSitePageLayout"  %>
<%@ Assembly Name="Microsoft.Web.CommandUI, Version=15.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %>
<%@ Import Namespace="Microsoft.SharePoint" %>
<%@ Import Namespace="Hemrika.SharePresence.Common" %>
<%@ Import Namespace="Hemrika.SharePresence.WebSite" %>
<%@ Register Tagprefix="SharePoint" Namespace="Microsoft.SharePoint.WebControls" Assembly="Microsoft.SharePoint, Version=15.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %>
<%@ Register Tagprefix="Utilities" Namespace="Microsoft.SharePoint.Utilities" Assembly="Microsoft.SharePoint, Version=15.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %>
<%@ Register Tagprefix="WebPartPages" Namespace="Microsoft.SharePoint.WebPartPages" Assembly="Microsoft.SharePoint, Version=15.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %>
<asp:Content ContentPlaceholderID="PlaceHolderPageTitle" runat="server">
	<SharePoint:FieldValue id="PageTitle" FieldName="Title" runat="server"/></asp:Content>
<asp:Content ContentPlaceholderID="PlaceHolderMain" runat="server">
<WebPartPages:WebPartZone runat="server" FrameType="TitleBarOnly" ID="Left" Title="loc:Left"><ZoneTemplate></ZoneTemplate></WebPartPages:WebPartZone>
<WebPartPages:WebPartZone runat="server" FrameType="TitleBarOnly" ID="Right" Title="loc:Right"><ZoneTemplate></ZoneTemplate></WebPartPages:WebPartZone>
</asp:Content>
