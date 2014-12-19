<%@ Assembly Name="Hemrika.SharePresence.WebSite, Version=1.0.0.0, Culture=neutral, PublicKeyToken=11e6604a27f32a11" %>
<%@ Page Language="C#" AutoEventWireup="true" DynamicMasterPageFile="~masterurl/default.master" Inherits="Hemrika.SharePresence.WebSite.CreateWebPage" %>

<%@ Register Assembly="System.Web.Extensions, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35"
    Namespace="System.Web.UI" TagPrefix="asp" %>
<%@ Register TagPrefix="SharePoint" Namespace="Microsoft.SharePoint.WebControls" Assembly="Microsoft.SharePoint, Version=15.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %>
<%@ Register TagPrefix="Utilities" Namespace="Microsoft.SharePoint.Utilities" Assembly="Microsoft.SharePoint, Version=15.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %>
<%@ Register TagPrefix="SharePresence" TagName="WebPage" Src="/_controltemplates/15/Hemrika/Content/CreateWebPage.ascx" %>

<asp:Content ContentPlaceHolderId="PlaceHolderPageTitle" runat="server">
    <SharePoint:EncodedLiteral runat="server" text="<%$Resources:Hemrika.SharePresence.WebSite.AppResources,CreateWebPage_PageTitle%>" EncodeMethod='HtmlEncode'/>
</asp:Content>
<asp:Content ContentPlaceHolderId="PlaceHolderPageTitleInTitleArea" runat="server">
        <SharePoint:EncodedLiteral runat="server" text="<%$Resources:Hemrika.SharePresence.WebSite.AppResources,CreateWebPage_PageTitle%>" EncodeMethod='HtmlEncode'/>
</asp:Content>
<asp:Content ID="PageDescription" ContentPlaceHolderID="PlaceHolderPageDescription"  runat="server">
    <SharePoint:EncodedLiteral runat="server" text="<%$Resources:Hemrika.SharePresence.WebSite.AppResources,CreateWebPage_PageDescription%>" EncodeMethod='HtmlEncode'/>
</asp:Content>
<asp:Content ID="Main" ContentPlaceHolderID="PlaceHolderMain" runat="server">
<div data-role="page" class="type-interior" data-theme="b">
    <div data-role="content" data-theme="b">
        <div class="content-primary">
                <SharePresence:WebPage ID="Create" runat="server" ></SharePresence:WebPage>
        </div><!--/content-primary -->
    </div><!-- /content -->
</div><!-- /page -->
    <SharePoint:FormDigest ID="FormDigest1" runat="server" />
</asp:Content>

