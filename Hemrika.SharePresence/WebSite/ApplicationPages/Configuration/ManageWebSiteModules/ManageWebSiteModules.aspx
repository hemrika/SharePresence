<%@ Assembly Name="Hemrika.SharePresence.WebSite, Version=1.0.0.0, Culture=neutral, PublicKeyToken=3421bd1d946bda6c" %>
<%@ Page Language="C#" MasterPageFile="~/_layouts/application.master" Inherits="Hemrika.SharePresence.WebSite.ManageWebSiteModules" %>
<%@ Register TagPrefix="SharePoint" Namespace="Microsoft.SharePoint.WebControls" Assembly="Microsoft.SharePoint, Version=15.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %>
<%@ Register TagPrefix="Utilities" Namespace="Microsoft.SharePoint.Utilities" Assembly="Microsoft.SharePoint, Version=15.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %>
<%@ Register TagPrefix="wssuc" TagName="InputFormSection" Src="/_controltemplates/InputFormSection.ascx" %>
<%@ Register TagPrefix="wssuc" TagName="InputFormControl" Src="/_controltemplates/InputFormControl.ascx" %>
<%@ Register TagPrefix="wssuc" TagName="ButtonSection" Src="/_controltemplates/ButtonSection.ascx" %>
<%@ Register TagPrefix="wssuc" TagName="ToolBar" src="~/_controltemplates/ToolBar.ascx" %>
<%@ Register TagPrefix="wssuc" TagName="ToolBarButton" src="~/_controltemplates/ToolBarButton.ascx" %>

<asp:Content ContentPlaceHolderId="PlaceHolderPageTitle" runat="server">
	<SharePoint:EncodedLiteral runat="server" text="<%$Resources:Hemrika.SharePresence.WebSite.AppResources,ManageWebSiteModules_PageTitle%>" EncodeMethod='HtmlEncode'/>
</asp:Content>
<asp:Content ContentPlaceHolderId="PlaceHolderPageTitleInTitleArea" runat="server">
	<SharePoint:EncodedLiteral runat="server" text="<%$Resources:Hemrika.SharePresence.WebSite.AppResources,ManageWebSiteModules_PageTitle%>" EncodeMethod='HtmlEncode'/>
</asp:Content>
<asp:Content ID="PageDescription" ContentPlaceHolderID="PlaceHolderPageDescription"  runat="server">
    <SharePoint:EncodedLiteral runat="server" text="<%$Resources:Hemrika.SharePresence.WebSite.AppResources,ManageWebSiteModules_PageDescription%>" EncodeMethod='HtmlEncode'/>
</asp:Content>
<asp:Content ID="Main" ContentPlaceHolderID="PlaceHolderMain" runat="server">
<table width="100%" class="propertysheet" cellspacing="0" cellpadding="0" border="0"> 
    <tr> 
        <td class="ms-descriptionText"> 
            <asp:Label ID="LabelMessage" Runat="server" EnableViewState="False" class="ms-descriptionText"/>
        </td> 
    </tr> 
    <tr> 
        <td class="ms-error"><asp:Label ID="LabelErrorMessage" Runat="server" EnableViewState="False" />
        </td> 
    </tr> 
    <tr> 
        <td class="ms-descriptionText"> 
            <asp:ValidationSummary ID="ValSummary" HeaderText="<%$Resources:Hemrika.SharePresence.WebSite.AppResources,pc_adminpage_ValidationSummaryHeaderText%>" DisplayMode="BulletList" ShowSummary="True" runat="server"></asp:ValidationSummary>
        </td> 
    </tr>
    <tr> 
        <td>
            <img src="/_layouts/images/blank.gif" width="10" height="1" alt="" />
        </td> 
     </tr> 
</table>
<asp:Table runat="server" border="0" cellspacing="4" cellpadding="0" width="100%" id="entriesTable">
    <asp:TableHeaderRow>
       	<asp:TableHeaderCell HorizontalAlign="Left">
            <asp:Label ID="Label1" runat="server" Text="<%$Resources:Hemrika.SharePresence.WebSite.AppResources,pc_adminpageModule_headerClassName%>"/>
        </asp:TableHeaderCell>
    </asp:TableHeaderRow>
</asp:TABLE>
<SharePoint:FormDigest ID="FormDigest1" runat="server" />
</asp:Content>

