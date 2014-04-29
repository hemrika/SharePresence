<%@ Assembly Name="Hemrika.SharePresence.WebSite, Version=1.0.0.0, Culture=neutral, PublicKeyToken=11e6604a27f32a11" %>
<%@ Assembly Name="Microsoft.Web.CommandUI, Version=15.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %>
<%@ Register Tagprefix="SharePoint" Namespace="Microsoft.SharePoint.WebControls" Assembly="Microsoft.SharePoint, Version=15.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %>
<%@ Register Tagprefix="Utilities" Namespace="Microsoft.SharePoint.Utilities" Assembly="Microsoft.SharePoint, Version=15.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %>
<%@ Register Tagprefix="asp" Namespace="System.Web.UI" Assembly="System.Web.Extensions, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35" %>
<%@ Register TagPrefix="HTML5" Assembly="Hemrika.SharePresence.HTML5, Version=1.0.0.0, Culture=neutral, PublicKeyToken=11e6604a27f32a11" Namespace="Hemrika.SharePresence.Html5.WebControls" %>
<%@ Import Namespace="Microsoft.SharePoint" %> 
<%@ Register Tagprefix="WebPartPages" Namespace="Microsoft.SharePoint.WebPartPages" Assembly="Microsoft.SharePoint, Version=15.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %>
<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="ManageBookMarkSettings.ascx.cs" Inherits="Hemrika.SharePresence.WebSite.ControlTemplates.ManageBookMarkSettings" %>
<div data-role="page active" class="type-interior" data-theme="b">
	<div data-role="content" data-theme="b">
        <p>
			Configure the Rating Settings for the site.
        </p>
        <div data-role="fieldcontain">
        <label for="Minimum" title="Minimum Rating Value" class="required">Minimum Rating Value</label>
        <HTML5:NumberInput ID="rtg_minimum" runat="server" Width="100px" ></HTML5:NumberInput>
        <br /><br />
        <label for="Maximum" title="Maximum Rating Value" class="required">Maximum Rating Value</label>
        <HTML5:NumberInput ID="rtg_maximum" runat="server" Width="100px" ></HTML5:NumberInput>            
        <br />
            <asp:Button ID="btn_Save" class="ms-ButtonHeightWidth" runat="server" Text="Save" data-icon="check" data-inline="true" CssClass="ui-btn-up-b" data-iconpos="left"/>
            <asp:Button ID="btn_Cancel" class="ms-ButtonHeightWidth" runat="server" Text="Cancel" OnClientClick="SP.UI.ModalDialog.commonModalDialogClose(SP.UI.DialogResult.CANCEL,'No changes have been saved.'); return false;" data-icon="back" data-inline="true" CssClass="ui-btn-up-b" data-iconpos="left"/>
        </div>
    </div>
</div>