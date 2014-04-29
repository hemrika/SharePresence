<%@ Page language="C#" MasterPageFile="~masterurl/custom.master"   Inherits="Hemrika.SharePresence.WebSite.Page.WebSitePage,Hemrika.SharePresence.WebSite,Version=1.0.0.0,Culture=neutral,PublicKeyToken=11e6604a27f32a11" meta:progid="WebSite.WebSitePageLayout"  %>
<%@ Assembly Name="Microsoft.Web.CommandUI, Version=15.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %>
<%@ Import Namespace="Microsoft.SharePoint" %>
<%@ Import Namespace="Hemrika.SharePresence.Common" %>
<%@ Import Namespace="Hemrika.SharePresence.WebSite" %>
<%@ Register Tagprefix="SharePoint" Namespace="Microsoft.SharePoint.WebControls" Assembly="Microsoft.SharePoint, Version=15.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %>
<%@ Register Tagprefix="Utilities" Namespace="Microsoft.SharePoint.Utilities" Assembly="Microsoft.SharePoint, Version=15.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %>
<%@ Register Tagprefix="WebPartPages" Namespace="Microsoft.SharePoint.WebPartPages" Assembly="Microsoft.SharePoint, Version=15.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %>
<%@ Register Tagprefix="SharePresence" Namespace="Hemrika.SharePresence.WebSite.FieldTypes" Assembly="Hemrika.SharePresence.WebSite,Version=1.0.0.0,Culture=neutral,PublicKeyToken=11e6604a27f32a11" %>
<asp:Content ContentPlaceholderID="PlaceHolderAdditionalPageHead" runat="server" >
<style type="text/css">
#col1{display:block; float:left; width:630px; color:#6C6E72; background-color:#DDDDDD;}
#col2{display:block; float:right; width:300px; color:#979797; background-color:#FFFFFF;}
#col2 .box1{display:block; width:300px; color:#FFFFFF; background-color:#666666;}
</style>
</asp:Content>
<asp:Content ContentPlaceholderID="PlaceHolderPageTitle" runat="server">
	<SharePoint:FieldValue id="PageTitle" FieldName="Title" runat="server"/></asp:Content>
<asp:Content ContentPlaceholderID="PlaceHolderMain" runat="server">
    <div id="col1">
        <SharePresence:HTML5SectionControl FieldName="HTML5 Section" runat="server"></SharePresence:HTML5SectionControl>
    </div>
    <div id="col2">
      <div class="box1">
        <SharePresence:HTML5SectionControl FieldName="HTML5 Section 1" runat="server"></SharePresence:HTML5SectionControl>
      </div>
    </div>
</asp:Content>
