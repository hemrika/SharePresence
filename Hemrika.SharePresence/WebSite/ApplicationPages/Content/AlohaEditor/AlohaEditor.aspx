<%@ Assembly Name="Hemrika.SharePresence.WebSite, Version=1.0.0.0, Culture=neutral, PublicKeyToken=11e6604a27f32a11" %>
<%@ Page Language="C#" AutoEventWireup="true" DynamicMasterPageFile="~masterurl/default.master" Inherits="Hemrika.SharePresence.WebSite.AlohaEditor" %>
<%@ Register Assembly="System.Web.Extensions, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35"
    Namespace="System.Web.UI" TagPrefix="asp" %>
<%@ Register TagPrefix="SharePoint" Namespace="Microsoft.SharePoint.WebControls" Assembly="Microsoft.SharePoint, Version=15.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %>
<%@ Register TagPrefix="Utilities" Namespace="Microsoft.SharePoint.Utilities" Assembly="Microsoft.SharePoint, Version=15.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %>
<%@ Register TagPrefix="SharePresence" TagName="Editor" Src="/_controltemplates/15/Hemrika/Content/AlohaEditor.ascx" %>
<asp:Content ID="Main" ContentPlaceHolderID="PlaceHolderMain" runat="server">
<div data-role="page" class="type-interior" data-theme="b">
    <div data-role="content" data-theme="b">
        <div class="content-primary">
                <SharePresence:Editor ID="Aloha" runat="server" ></SharePresence:Editor>
        </div><!--/content-primary -->
    </div><!-- /content -->
</div><!-- /page -->
    <SharePoint:FormDigest ID="FormDigest1" runat="server" />
</asp:Content>

