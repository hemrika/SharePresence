<%@ Assembly Name="Hemrika.SharePresence.WebSite, Version=1.0.0.0, Culture=neutral, PublicKeyToken=11e6604a27f32a11" %>
<%@ Assembly Name="Microsoft.Web.CommandUI, Version=15.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %>
<%@ Register Tagprefix="SharePoint" Namespace="Microsoft.SharePoint.WebControls" Assembly="Microsoft.SharePoint, Version=15.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %>
<%@ Register Tagprefix="Utilities" Namespace="Microsoft.SharePoint.Utilities" Assembly="Microsoft.SharePoint, Version=15.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %>
<%@ Register Tagprefix="asp" Namespace="System.Web.UI" Assembly="System.Web.Extensions, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35" %>
<%@ Import Namespace="Microsoft.SharePoint" %> 
<%@ Register Tagprefix="WebPartPages" Namespace="Microsoft.SharePoint.WebPartPages" Assembly="Microsoft.SharePoint, Version=15.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %>
<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="ManageSiteMapSettings.ascx.cs" Inherits="Hemrika.SharePresence.WebSite.ControlTemplates.ManageSiteMapSettings" %>
<div data-role="page active" class="type-interior" data-theme="b">
	<div data-role="content" data-theme="b">
        <div data-role="fieldcontain">
        <asp:CheckBox ID="cbx_index" runat="server" Checked="true" Enabled="false" Text="Create Index Map for all maps (always)" />
        <label for="cbx_index" title="Info" class="required">The root site and each sub-site wil have their own sitemaps,<br /> split on 50.000 records into multiple sitemaps per site.</label>
        <br /><br />
        <asp:CheckBox ID="cbx_news" runat="server" Text="Include News Map in index" />
        <label for="cbx_index" title="Info" class="required">The News Map syndicates your latest content ( <3 days ) in compliant format.</label>
        <br /><br />
        <asp:CheckBox ID="cbx_image_include" runat="server" Checked="false" Enabled="false" Text="Include images in sitemaps (not yet implemented)" />
        <asp:CheckBox ID="cbx_docs_include" runat="server" Checked="false" Enabled="false" Text="Include documents in sitemaps (not yet implemented)"/>
        <asp:CheckBox ID="cbx_mobile" runat="server" Checked="false" Enabled="false" Text="Include Mobile Map in index (not yet implemented)" />
        <asp:CheckBox ID="cbx_video" runat="server" Checked="false" Enabled="false" Text="Include Video Map in index (not yet implemented)"/>
        <asp:CheckBox ID="cbx_image" runat="server" Checked="false" Enabled="false" Text="Include Image Map in index (not yet implemented)"/>
        <br />
        <label for="Search Ping Url's" title="Search Ping Url's" class="required">Search Engine Ping Url's</label>
		<asp:TextBox ID="tbx_ping" runat="server" TextMode="MultiLine" Rows="6" Width="100%"></asp:TextBox>
        <br />
        <asp:Button ID="btn_Save" runat="server" OnClick="btn_Save_Click" Text="Save" data-inline="true"
            CssClass="ui-btn-up-b" data-icon="check" />
        <asp:Button ID="btn_Cancel" runat="server" OnClientClick="SP.UI.ModalDialog.commonModalDialogClose(SP.UI.DialogResult.CANCEL,'No changes have been saved.'); return false;" Text="Cancel"
            data-inline="true" CssClass="ui-btn-up-b" data-icon="back"/>
        </div>
    </div>
</div>