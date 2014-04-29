<%@ Assembly Name="Hemrika.SharePresence.WebSite, Version=1.0.0.0, Culture=neutral, PublicKeyToken=3421bd1d946bda6c" %>
<%@ Assembly Name="Microsoft.Web.CommandUI, Version=15.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %>
<%@ Import Namespace="Microsoft.SharePoint.ApplicationPages" %>
<%@ Register Tagprefix="SharePoint" Namespace="Microsoft.SharePoint.WebControls" Assembly="Microsoft.SharePoint, Version=15.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %>
<%@ Register Tagprefix="Utilities" Namespace="Microsoft.SharePoint.Utilities" Assembly="Microsoft.SharePoint, Version=15.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %>
<%@ Register Tagprefix="asp" Namespace="System.Web.UI" Assembly="System.Web.Extensions, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35" %>
<%@ Register TagPrefix="SharePresence" TagName="Keywords" Src="/_controltemplates/Hemrika/MetaData/ManageKeywordSettings.ascx" %>
<%@ Import Namespace="Microsoft.SharePoint" %>
<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="KeywordManage.aspx.cs"Inherits="Hemrika.SharePresence.WebSite.MetaData.KeywordManage" DynamicMasterPageFile="~masterurl/default.master" %>
<asp:Content ID="PageHead" ContentPlaceHolderID="PlaceHolderAdditionalPageHead" runat="server">
</asp:Content>
<asp:Content ID="PageTitle" ContentPlaceHolderID="PlaceHolderPageTitle" runat="server">
	<SharePoint:EncodedLiteral ID="EncodedLiteral2" runat="server" text="MetaData Settings" EncodeMethod='HtmlEncode'/>
</asp:Content>
<asp:Content ID="PageDescription" ContentPlaceHolderID="PlaceHolderPageDescription"  runat="server">
	<SharePoint:EncodedLiteral ID="EncodedLiteral3" runat="server" text="MetaData Settings for the Site." EncodeMethod='HtmlEncode'/>
</asp:Content>
<asp:Content ID="PageTitleInTitleArea" ContentPlaceHolderID="PlaceHolderPageTitleInTitleArea" runat="server" >
	<a href="../settings.aspx"><SharePoint:EncodedLiteral ID="EncodedLiteral1" runat="server" Text="<%$Resources:wss,settings_pagetitle%>" EncodeMethod="HtmlEncode" /></a>&#32;<SharePoint:ClusteredDirectionalSeparatorArrow ID="ClusteredDirectionalSeparatorArrow1" runat="server" />&#32;<SharePoint:EncodedLiteral ID="EncodedLiteral4" runat="server" text="MetaData Settings" EncodeMethod='HtmlEncode'/>
</asp:Content>
<asp:Content ID="Main" ContentPlaceHolderID="PlaceHolderMain" runat="server">
<div data-role="page" class="type-interior" data-theme="b">
    <div data-role="content" data-theme="b">
        <div class="content-primary">
            <SharePresence:Keywords ID="ManageKeyword" runat="server" ></SharePresence:Keywords>
        </div><!--/content-primary -->
    </div><!-- /content -->
</div><!-- /page -->
<SharePoint:FormDigest ID="FormDigest1" runat="server" />
</asp:Content>

