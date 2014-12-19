<%@ Assembly Name="Hemrika.SharePresence.WebSite, Version=1.0.0.0, Culture=neutral, PublicKeyToken=11e6604a27f32a11" %>
<%@ Import Namespace="Microsoft.SharePoint.ApplicationPages" %>
<%@ Register Tagprefix="SharePoint" Namespace="Microsoft.SharePoint.WebControls" Assembly="Microsoft.SharePoint, Version=15.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %>
<%@ Register Tagprefix="Utilities" Namespace="Microsoft.SharePoint.Utilities" Assembly="Microsoft.SharePoint, Version=15.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %>
<%@ Register Tagprefix="asp" Namespace="System.Web.UI" Assembly="System.Web.Extensions, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35" %>
<%@ Register TagPrefix="SharePresence" TagName="gkipblacklist" Src="/_controltemplates/15/Hemrika/GateKeeper/IPBlackList.ascx" %>
<%@ Register TagPrefix="SharePresence" TagName="gkuablacklist" Src="/_controltemplates/15/Hemrika/GateKeeper/UABlackList.ascx" %>
<%@ Import Namespace="Microsoft.SharePoint" %>
<%@ Assembly Name="Microsoft.Web.CommandUI, Version=15.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %>
<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="GateKeeperBlacklist.aspx.cs" Inherits="Hemrika.SharePresence.WebSite.GateKeeper.GateKeeperBlacklist" DynamicMasterPageFile="~masterurl/default.master" %>
<asp:Content ID="PageHead" ContentPlaceHolderID="PlaceHolderAdditionalPageHead" runat="server">
    <link href="GateKeeper.css" rel="Stylesheet" />
    <script src="http://ajax.googleapis.com/ajax/libs/jquery/1.2.6/jquery.min.js" type="text/javascript"></script>
    <script type="text/javascript" language="javascript">
        (function ($) {
            $(function () {
                var tabContainers = $('div.tabs > div');
                $('div.tabs ul.tabNavigation a').click(function () {
                    tabContainers.hide().filter(this.hash).slideDown("slow");
                    $('div.tabs ul.tabNavigation a').removeClass('selected');
                    $(this).addClass('selected');
                    return false;
                }).filter(':eq(1)').click();
            });
        })(jQuery);
    </script>

</asp:Content>
<asp:Content ID="PageTitle" ContentPlaceHolderID="PlaceHolderPageTitle" runat="server">
	<SharePoint:EncodedLiteral ID="EncodedLiteral2" runat="server" text="GateKeeper Settings" EncodeMethod='HtmlEncode'/>
</asp:Content>
<asp:Content ID="PageDescription" ContentPlaceHolderID="PlaceHolderPageDescription"  runat="server">
	<SharePoint:EncodedLiteral ID="EncodedLiteral3" runat="server" text="GateKeeper Settings for the Site." EncodeMethod='HtmlEncode'/>
</asp:Content>

<asp:Content ID="PageTitleInTitleArea" ContentPlaceHolderID="PlaceHolderPageTitleInTitleArea" runat="server" >
	<a href="../settings.aspx"><SharePoint:EncodedLiteral ID="EncodedLiteral1" runat="server" Text="<%$Resources:wss,settings_pagetitle%>" EncodeMethod="HtmlEncode" /></a>&#32;<SharePoint:ClusteredDirectionalSeparatorArrow ID="ClusteredDirectionalSeparatorArrow1" runat="server" />&#32;<SharePoint:EncodedLiteral ID="EncodedLiteral4" runat="server" text="GateKeeper Settings" EncodeMethod='HtmlEncode'/>
</asp:Content>
<asp:Content ID="Main" ContentPlaceHolderID="PlaceHolderMain" runat="server">
<div data-role="page" class="type-interior" data-theme="b">
    <div data-role="content" data-theme="b">
        <div class="content-primary">
    <div class="tabs" style="width: 1000px; margin: 30px auto">
        <h2 style="color: #333">GateKeeper Web Access Management</h2>
        <!-- tabs -->
        <ul class="tabNavigation">
            <li style="float: right;"><a href="#config">Configure GateKeeper</a></li>
            <li><a href="#ipbl">IP Blacklist</a></li>
            <li><a href="#uabl">UA Blacklist</a></li>
        </ul>

        <!-- tab containers -->
        <div id="ipbl">
            <SharePresence:gkipblacklist ID="gkipblacklist" runat="server" />
        </div>
        <div id="uabl">
            <SharePresence:gkuablacklist ID="gkuablacklist" runat="server" />
        </div>
    </div>
        </div><!--/content-primary -->
    </div><!-- /content -->
</div><!-- /page -->

<SharePoint:FormDigest ID="FormDigest1" runat="server" />
</asp:Content>
