<%@ Assembly Name="Hemrika.SharePresence.WebSite, Version=1.0.0.0, Culture=neutral, PublicKeyToken=11e6604a27f32a11" %>
<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="ManageGoogleAccountSettings.aspx.cs" DynamicMasterPageFile="~masterurl/default.master" Inherits="Hemrika.SharePresence.WebSite.ManageGoogleAccountSettings" %>

<%@ Register Assembly="System.Web.Extensions, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35"
    Namespace="System.Web.UI" TagPrefix="asp" %>
<%@ Register TagPrefix="SharePoint" Namespace="Microsoft.SharePoint.WebControls" Assembly="Microsoft.SharePoint, Version=15.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %>
<%@ Register TagPrefix="Utilities" Namespace="Microsoft.SharePoint.Utilities" Assembly="Microsoft.SharePoint, Version=15.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %>
<%@ Register TagPrefix="SharePresence" TagName="GoogleAccountUser" Src="/_controltemplates/15/Hemrika/Analytics/GoogleAccountUser.ascx" %>
<%@ Register TagPrefix="SharePresence" TagName="GoogleAccountProfiles" Src="/_controltemplates/15/Hemrika/Analytics/GoogleAccountProfiles.ascx" %>
<asp:Content ContentPlaceHolderId="PlaceHolderPageTitle" runat="server">
	<SharePoint:EncodedLiteral runat="server" text="<%$Resources:Hemrika.SharePresence.WebSite.AppResources,ManageGoogleAccountSettings_PageTitle%>" EncodeMethod='HtmlEncode'/>
</asp:Content>
<asp:Content ContentPlaceHolderId="PlaceHolderPageTitleInTitleArea" runat="server">
	<SharePoint:EncodedLiteral runat="server" text="<%$Resources:Hemrika.SharePresence.WebSite.AppResources,ManageGoogleAccountSettings_PageTitle%>" EncodeMethod='HtmlEncode'/>
</asp:Content>
<asp:Content ID="PageDescription" ContentPlaceHolderID="PlaceHolderPageDescription"  runat="server">
    <SharePoint:EncodedLiteral runat="server" text="<%$Resources:Hemrika.SharePresence.WebSite.AppResources,ManageGoogleAccountSettings_PageDescription%>" EncodeMethod='HtmlEncode'/>
</asp:Content>
<asp:Content ID="PageHead" ContentPlaceHolderID="PlaceHolderAdditionalPageHead" runat="server">
<link rel="stylesheet" type="text/css" media="all" href="/_layouts/Hemrika/GoogleStyle.css" />
</asp:Content>

<asp:Content ID="Main" ContentPlaceHolderID="PlaceHolderMain" runat="server">
    <div data-role="page" class="type-interior" data-theme="b">
        <div data-role="content" data-theme="b">
            <div class="content-primary">
                <div data-role="page active" class="type-interior" data-theme="b">
                    <div data-role="header" data-theme="b" data-position="fixed">
                        <span class="ui-app-title">Manage Google Analytics Settings</span>
                    </div>
                    <div data-role="content" data-theme="b">
                        <h2>
                            Google Analytics Settings</h2>
                        <div data-role="fieldcontain">
                            <SharePresence:GoogleAccountUser ID="GoogleAccountUser" runat="server">
                            </SharePresence:GoogleAccountUser>
                            <SharePresence:GoogleAccountProfiles ID="GoogleAccountProfiles" runat="server">
                            </SharePresence:GoogleAccountProfiles>
                        </div>
                    </div>
                </div>
            </div>
            <!--/content-primary -->
        </div>
        <!-- /content -->
    </div>
    <!-- /page -->
 <!-- 
<div style="display:none;">
<asp:Panel ID="pnl_Account2" GroupingText="Connector Settings" runat="server" >
<asp:TextBox ID="tbx_username" runat="server"></asp:TextBox>
<asp:TextBox ID="tbx_password" runat="server" TextMode="Password"></asp:TextBox><br />
<asp:Button ID="btn_save" runat="server" Text="Save" /><br />
</asp:Panel>
<br />
<asp:Panel ID="pnl_Connector" GroupingText="Connector Settings" runat="server" >
<asp:CheckBox ID="cbx_crossdomein" Text="Use CrossDomain" runat="server" />
<asp:Button ID="btn_connector" Text="Save" runat="server" />
</asp:Panel>
<br />
<asp:Panel ID="pnl_Authorize" GroupingText="Authorize your Site for Google" runat="server" >
<asp:HyperLink ID="hl_analytics" runat="server" Visible="true" Text="Authorize Analytics" NavigateUrl="https://www.google.com/accounts/AuthSubRequest?&amp;next=http://www.hemrika.nl/_layouts/Hemrika/ManageGoogleAccountSettings.aspx&amp;scope=https://www.google.com/analytics/feeds/&amp;secure=0&amp;session=1" /><br />
<asp:HyperLink ID="hl_webmaster" runat="server" Visible="true" Text="Authorize Webmastertools" NavigateUrl="https://www.google.com/accounts/AuthSubRequest?&amp;next=http://www.hemrika.nl/_layouts/Hemrika/ManageGoogleAccountSettings.aspx&amp;scope=https://www.google.com/webmasters/tools/feeds/&amp;secure=0&amp;session=1" /><br />
</asp:Panel>
<br />
<asp:Label ID="lbl_google" runat="server"/>
<asp:Label ID="lbl_error" runat="server"/>
</div>
-->
    <SharePoint:FormDigest ID="FormDigest1" runat="server" />
</asp:Content>

