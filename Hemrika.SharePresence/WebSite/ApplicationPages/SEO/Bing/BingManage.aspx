<%@ Assembly Name="Hemrika.SharePresence.WebSite, Version=1.0.0.0, Culture=neutral, PublicKeyToken=11e6604a27f32a11" %>
<%@ Import Namespace="Microsoft.SharePoint.ApplicationPages" %>
<%@ Register Tagprefix="SharePoint" Namespace="Microsoft.SharePoint.WebControls" Assembly="Microsoft.SharePoint, Version=15.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %>
<%@ Register Tagprefix="Utilities" Namespace="Microsoft.SharePoint.Utilities" Assembly="Microsoft.SharePoint, Version=15.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %>
<%@ Register Tagprefix="asp" Namespace="System.Web.UI" Assembly="System.Web.Extensions, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35" %>
<%@ Import Namespace="Microsoft.SharePoint" %>
<%@ Assembly Name="Microsoft.Web.CommandUI, Version=15.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %>
<%@ Register TagPrefix="wssuc" TagName="InputFormSection" Src="/_controltemplates/InputFormSection.ascx" %>
<%@ Register TagPrefix="wssuc" TagName="ButtonSection" Src="/_controltemplates/ButtonSection.ascx" %>
<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="BingManage.aspx.cs" Inherits="Hemrika.SharePresence.WebSite.Bing.BingManage" DynamicMasterPageFile="~masterurl/default.master" %>
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
            <h2>Bing Settings</h2>
            <p>
				Copy the ID provided by Microsoft Bing Webmaster Tools
                <br />
                <a href="http://www.bing.com/toolbox/webmasters/" id="Bing_Webmaster" target="_blank" title="Go to Bing Webmaster Toolbox">>> Go to Bing Webmaster Tools</a>
            </p>
            <div data-role="fieldcontain">
            <label for="tbx_BingUser" title="Bing UserId" class="required ui-input-text">Bing UserId</label>
            <asp:TextBox runat="server" ID="tbx_BingUser" class="ui-input-text"/>
            <br /><br />
            <label for="tbx_BingAPI" title="Bing API" class="required ui-input-text">Bing API</label>
            <asp:TextBox runat="server" ID="tbx_BingAPI" class="ui-input-text"/><br />
            </div>
        <asp:Button ID="btn_Save" class="ms-ButtonHeightWidth" runat="server" Text="Save" data-icon="check" data-inline="true" CssClass="ui-btn-up-b" data-iconpos="left"/>
        <asp:Button ID="btn_Cancel" class="ms-ButtonHeightWidth" runat="server" Text="Cancel" data-icon="back" data-inline="true" CssClass="ui-btn-up-b" data-iconpos="left"/>
        </div><!--/content-primary -->
    </div><!-- /content -->
</div><!-- /page -->
<SharePoint:FormDigest ID="FormDigest1" runat="server" />
</asp:Content>
