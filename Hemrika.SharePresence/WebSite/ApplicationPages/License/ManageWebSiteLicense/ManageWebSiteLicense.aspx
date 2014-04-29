<%@ Assembly Name="Hemrika.SharePresence.WebSite, Version=1.0.0.0, Culture=neutral, PublicKeyToken=3421bd1d946bda6c" %>
<%@ Page Language="C#" MasterPageFile="~/_layouts/application.master" Inherits="Hemrika.SharePresence.WebSite.ManageWebSiteLicense" %>
<%@ Register TagPrefix="SharePoint" Namespace="Microsoft.SharePoint.WebControls" Assembly="Microsoft.SharePoint, Version=15.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %>
<%@ Register TagPrefix="Utilities" Namespace="Microsoft.SharePoint.Utilities" Assembly="Microsoft.SharePoint, Version=15.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %>
<%@ Register TagPrefix="SharePresence" TagName="Upload" Src="/_controltemplates/Hemrika/UploadLicense.ascx" %>
<%@ Register TagPrefix="SharePresence" TagName="View" Src="/_controltemplates/Hemrika/ViewLicense.ascx" %>
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
<SharePresence:Upload ID="Create" runat="server" ></SharePresence:Upload>
<SharePoint:FormDigest ID="FormDigest1" runat="server" />
</asp:Content>

