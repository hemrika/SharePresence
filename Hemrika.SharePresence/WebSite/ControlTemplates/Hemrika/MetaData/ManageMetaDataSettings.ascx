<%@ Assembly Name="Hemrika.SharePresence.WebSite, Version=1.0.0.0, Culture=neutral, PublicKeyToken=3421bd1d946bda6c" %>
<%@ Assembly Name="Microsoft.Web.CommandUI, Version=15.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %>
<%@ Register Tagprefix="SharePoint" Namespace="Microsoft.SharePoint.WebControls" Assembly="Microsoft.SharePoint, Version=15.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %>
<%@ Register Tagprefix="Utilities" Namespace="Microsoft.SharePoint.Utilities" Assembly="Microsoft.SharePoint, Version=15.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %>
<%@ Register Tagprefix="asp" Namespace="System.Web.UI" Assembly="System.Web.Extensions, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35" %>
<%@ Register TagPrefix="HTML5" Assembly="Hemrika.SharePresence.HTML5, Version=1.0.0.0, Culture=neutral, PublicKeyToken=3421bd1d946bda6c" Namespace="Hemrika.SharePresence.Html5.WebControls" %>
<%@ Import Namespace="Microsoft.SharePoint" %> 
<%@ Register Tagprefix="WebPartPages" Namespace="Microsoft.SharePoint.WebPartPages" Assembly="Microsoft.SharePoint, Version=15.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %>
<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="ManageMetaDataSettings.ascx.cs" Inherits="Hemrika.SharePresence.WebSite.ControlTemplates.ManageMetaDataSettings" %>
<div data-role="page active" class="type-interior" data-theme="b">
	<div data-role="content" data-theme="b">
        <p>
			Configure the global MetaData for the site.
        </p>
        <div data-role="fieldcontain">            
            <asp:CheckBox ID="cbx_msnbot" runat="server" Text="Use DMOZ" TextAlign="Right" />
            <label for="DMOZ" title="Do not use DMOZ description for Bing search results." class="required">Do not use DMOZ description for Bing search results. <a href="http://www.dmoz.org/"  target="_blank" title="Open Directory Project">Open Directory Project</a></label>
            <hr />
            <label for="Keywords" title="Keywords settings." class="required">Keywords settings</label>
            <asp:CheckBox ID="cbx_keywords" runat="server" AutoPostBack="true" Text="Auto generate Keywords" TextAlign="Right"  />
            <label for="KeywordsAmount" title="Keywords amount." class="required">Set the number of keywords to auto generate ( 0- 30 )</label>
            <HTML5:RangeInput Maximum="30" Minimum="0" runat="server" Step="1" ID="rng_keywords" Width="300px" />
            <br /><br />
            <div style="display:none;" >
            <hr />
            <label for="language" title="language settings." class="required">Language Settings</label>
            <asp:CheckBox ID="cbx_language" runat="server" Text="Use site language" TextAlign="Right" />
            <hr />
            <label for="Author" title="Author Settings." class="required">Author Settings</label>
            <asp:RadioButtonList ID="rbl_author" runat="server" AutoPostBack="true" RepeatColumns="2" ></asp:RadioButtonList>
            <hr />
            <label for="Web Author Settings" title="Wb Author Settings." class="required">Web Author Settings</label>
            <asp:RadioButtonList ID="rbl_web_author" runat="server" AutoPostBack="true" RepeatColumns="2" ></asp:RadioButtonList>
            <hr />
            </div>
            <asp:Button ID="btn_Save" class="ms-ButtonHeightWidth" runat="server" Text="Save" data-icon="check" data-inline="true" CssClass="ui-btn-up-b" data-iconpos="left"/>
            <asp:Button ID="btn_Cancel" class="ms-ButtonHeightWidth" runat="server" Text="Cancel" OnClientClick="SP.UI.ModalDialog.commonModalDialogClose(SP.UI.DialogResult.CANCEL,'No changes have been saved.'); return false;" data-icon="back" data-inline="true" CssClass="ui-btn-up-b" data-iconpos="left"/>
        </div>
    </div>
</div>