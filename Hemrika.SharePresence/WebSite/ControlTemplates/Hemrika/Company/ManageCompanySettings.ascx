<%@ Assembly Name="Hemrika.SharePresence.WebSite, Version=1.0.0.0, Culture=neutral, PublicKeyToken=3421bd1d946bda6c" %>
<%@ Assembly Name="Microsoft.Web.CommandUI, Version=15.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %>
<%@ Register Tagprefix="SharePoint" Namespace="Microsoft.SharePoint.WebControls" Assembly="Microsoft.SharePoint, Version=15.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %>
<%@ Register Tagprefix="Utilities" Namespace="Microsoft.SharePoint.Utilities" Assembly="Microsoft.SharePoint, Version=15.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %>
<%@ Register Tagprefix="asp" Namespace="System.Web.UI" Assembly="System.Web.Extensions, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35" %>
<%@ Import Namespace="Microsoft.SharePoint" %> 
<%@ Register Tagprefix="WebPartPages" Namespace="Microsoft.SharePoint.WebPartPages" Assembly="Microsoft.SharePoint, Version=15.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %>
<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="ManageCompanySettings.ascx.cs" Inherits="Hemrika.SharePresence.WebSite.ControlTemplates.ManageCompanySettings" %>
    <div data-role="page active" class="type-interior" data-theme="b">
	    <div data-role="content" data-theme="b">
            <div data-role="fieldcontain">
            <label for="Name" title="Company Name" class="required"   >Company name </label>
			<asp:Textbox ID="tbx_name" runat="server" />
            <br />
            <label for="Street" title="Street" class="required" >Street</label>
			<asp:Textbox ID="tbx_street" runat="server" />
            <br />
            <label for="Postal Code" title="Postal Code" class="required" >Postal Code</label>
			<asp:Textbox ID="tbx_postalcode" runat="server" />
            <br />
            <label for="City" title="City" class="required">City</label>
			<asp:Textbox ID="tbx_city" runat="server" />
            <br />
            <label for="State" title="State" class="required">State</label>
			<asp:Textbox ID="tbx_state" runat="server" />
            <br />
            <label for="Country" title="Country" class="required">Country</label>
			<asp:Textbox ID="tbx_country" runat="server" />
            <br />
            <label for="Phone" title="Phone" class="required">Phone</label>
			<asp:Textbox ID="tbx_phone" runat="server" />
            <br />
            <label for="Email" title="Email" class="required">Email</label>
			<asp:Textbox ID="tbx_email" runat="server" />
            <br />
            <label for="LinkedIn" title="LinkedIn" class="required">LinkedIn</label>
			<asp:Textbox ID="tbx_linkedin" runat="server" />
            <br />
            <label for="FaceBook" title="FaceBook" class="required">FaceBook</label>
			<asp:Textbox ID="tbx_facebook" runat="server" />
            <br />
            <label for="Twitter" title="Twitter" class="required">Twitter</label>
			<asp:Textbox ID="tbx_twitter" runat="server" />
            <br />
            <label for="Stock" title="Stock" class="required">Stock</label>
			<asp:Textbox ID="tbx_stock" runat="server" />
        </div>						
		<asp:Button ID="btn_Save" class="ms-ButtonHeightWidth" runat="server" Text="Save" data-icon="gear" onclick="btn_Save_Click" data-inline="true" CssClass="ui-btn-up-b" />
		<asp:Button ID="btn_Cancel" class="ms-ButtonHeightWidth" runat="server" Text="Cancel" data-icon="back" OnClientClick="SP.UI.ModalDialog.commonModalDialogClose(SP.UI.DialogResult.CANCEL,'No changes have been saved.'); return false;" data-inline="true" CssClass="ui-btn-up-b" />
        </div>
    </div>