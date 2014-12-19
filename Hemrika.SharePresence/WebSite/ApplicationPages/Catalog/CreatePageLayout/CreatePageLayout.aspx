<%@ Assembly Name="Hemrika.SharePresence.WebSite, Version=1.0.0.0, Culture=neutral, PublicKeyToken=11e6604a27f32a11" %>
<%@ Page Language="C#" AutoEventWireup="true" DynamicMasterPageFile="~masterurl/default.master" Inherits="Hemrika.SharePresence.WebSite.CreatePageLayout" %>
<%@ Register TagPrefix="SharePoint" Namespace="Microsoft.SharePoint.WebControls" Assembly="Microsoft.SharePoint, Version=15.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %>
<%@ Register TagPrefix="Utilities" Namespace="Microsoft.SharePoint.Utilities" Assembly="Microsoft.SharePoint, Version=15.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %>
<%@ Register TagPrefix="SharePresence" TagName="PageLayout" Src="/_controltemplates/15/Hemrika/Catalog/CreatePageLayout.ascx" %>
<asp:Content ID="PageHead" ContentPlaceHolderID="PlaceHolderAdditionalPageHead" runat="server">
</asp:Content>
<asp:Content ID="PageTitle" ContentPlaceHolderId="PlaceHolderPageTitle" runat="server">
    <SharePoint:EncodedLiteral ID="EncodedLiteral1" runat="server" text="<%$Resources:Hemrika.SharePresence.WebSite.AppResources,CreatePageLayout_PageTitle%>" EncodeMethod='HtmlEncode'/>
</asp:Content>
<asp:Content ID="PageTitleInTitleArea" ContentPlaceHolderId="PlaceHolderPageTitleInTitleArea" runat="server">
    <SharePoint:EncodedLiteral ID="EncodedLiteral2" runat="server" text="<%$Resources:Hemrika.SharePresence.WebSite.AppResources,CreatePageLayout_PageTitle%>" EncodeMethod='HtmlEncode'/>
</asp:Content>
<asp:Content ID="PageDescription" ContentPlaceHolderID="PlaceHolderPageDescription"  runat="server">
    <SharePoint:EncodedLiteral ID="EncodedLiteral3" runat="server" text="<%$Resources:Hemrika.SharePresence.WebSite.AppResources,CreatePageLayout_PageDescription%>" EncodeMethod='HtmlEncode'/>
</asp:Content>
<asp:Content ID="Main" ContentPlaceHolderID="PlaceHolderMain" runat="server">
<div data-role="page" class="type-interior" data-theme="b">
    <div data-role="content" data-theme="b">
        <div class="content-primary">
            <SharePresence:PageLayout ID="PageLayout" runat="server" ></SharePresence:PageLayout>
        </div><!--/content-primary -->
    </div><!-- /content -->
</div><!-- /page -->
<SharePoint:FormDigest ID="FormDigest1" runat="server" />
</asp:Content>
