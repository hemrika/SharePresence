<%@ Page language="C#" MasterPageFile="~masterurl/default.master"    Inherits="Microsoft.SharePoint.WebPartPages.WebPartPage,Microsoft.SharePoint, Version=15.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" meta:progid="SharePoint.WebPartPage.Document" meta:webpartpageexpansion="full"  %>
<%@ Register Tagprefix="SharePoint" Namespace="Microsoft.SharePoint.WebControls" Assembly="Microsoft.SharePoint, Version=15.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %> <%@ Register Tagprefix="Utilities" Namespace="Microsoft.SharePoint.Utilities" Assembly="Microsoft.SharePoint, Version=15.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %> <%@ Import Namespace="Microsoft.SharePoint" %> <%@ Assembly Name="Microsoft.Web.CommandUI, Version=15.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %> <%@ Register Tagprefix="WebPartPages" Namespace="Microsoft.SharePoint.WebPartPages" Assembly="Microsoft.SharePoint, Version=15.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %>
<asp:Content ContentPlaceHolderId="PlaceHolderPageTitle" runat="server">
	<SharePoint:EncodedLiteral runat="server" text="<%$Resources:wss,webpagecreation_bp_title%>" EncodeMethod='HtmlEncode'/>
</asp:Content>
<asp:Content ContentPlaceHolderId="PlaceHolderPageTitleInTitleArea" runat="server">
	<table>
		<tr>
			<td class="ms-pagetitle" style='width: 100%'>
			<WebPartPages:WebPartZone runat="server" Title="loc:TitleBar" ID="TitleBar" AllowLayoutChange="false" AllowPersonalization="false"><ZoneTemplate></ZoneTemplate></WebPartPages:WebPartZone></td>
			<td class="ms-propertysheet" nowrap="true">
			<SharePoint:BpScript runat="server"/></td>
		</tr>
	</table>
</asp:Content>
<asp:Content ContentPlaceHolderId="PlaceHolderAdditionalPageHead" runat="server">
	<meta name="GENERATOR" content="Microsoft SharePoint">
	<meta name="ProgId" content="SharePoint.WebPartPage.Document">
	<meta http-equiv="Content-Type" content="text/html; charset=utf-8">
	<meta name="CollaborationServer" content="SharePoint Team Web Site">
	<script>
// <![CDATA[
	var navBarHelpOverrideKey = "wssmain";
// ]]>
	</script>
	<SharePoint:RssLink runat="server"/>
	<SharePoint:ScriptLink language="javascript" name="bpstd.js" runat="server"/>
	<style>
	Div.ms-titleareaframe {
		height: 100%;
	}
	.ms-pagetitleareaframe table {
		background: none;
	}
	</style>
</asp:Content>
<asp:Content ContentPlaceHolderId="PlaceHolderSearchArea" runat="server">
	<SharePoint:DelegateControl runat="server"
		ControlId="SmallSearchInputBox"/>
</asp:Content>
<asp:Content ContentPlaceHolderId="PlaceHolderLeftActions" runat="server">
</asp:Content>
<asp:Content ContentPlaceHolderId="PlaceHolderPageDescription" runat="server">
	<SharePoint:ProjectProperty Property="Description" runat="server"/>
</asp:Content>
<asp:Content ContentPlaceHolderId="PlaceHolderBodyRightMargin" runat="server">
	<div height="100%" class="ms-pagemargin"><img src="/_layouts/images/blank.gif" width="10" height="1" alt="" /></div>
</asp:Content>
<asp:Content ContentPlaceHolderId="PlaceHolderPageImage" runat="server"></asp:Content>
<asp:Content ContentPlaceHolderId="PlaceHolderLeftNavBar" runat="server"></asp:Content>
<asp:Content ContentPlaceHolderId="PlaceHolderNavSpacer" runat="server"></asp:Content>
<asp:Content ContentPlaceHolderId="PlaceHolderMain" runat="server">
		<table cellpadding="4" cellspacing="0" border="0" width="100%">
				<tr valign="top">
					<td width="100%">
						<WebPartPages:WebPartZone runat="server" FrameType="TitleBarOnly" Title="loc:FullPage" ID="FullPage" AllowLayoutChange="false" AllowPersonalization="false">
<ZoneTemplate>
</ZoneTemplate></WebPartPages:WebPartZone>
					</td>
				</tr>
		</table>
</asp:Content>
