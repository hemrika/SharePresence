<%@ Assembly Name="Hemrika.SharePresence.WebSite, Version=1.0.0.0, Culture=neutral, PublicKeyToken=3421bd1d946bda6c" %>
<%@ Assembly Name="Microsoft.Web.CommandUI, Version=15.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %>
<%@ Register Tagprefix="SharePoint" Namespace="Microsoft.SharePoint.WebControls" Assembly="Microsoft.SharePoint, Version=15.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %>
<%@ Register Tagprefix="Utilities" Namespace="Microsoft.SharePoint.Utilities" Assembly="Microsoft.SharePoint, Version=15.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %>
<%@ Register Tagprefix="asp" Namespace="System.Web.UI" Assembly="System.Web.Extensions, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35" %>
<%@ Import Namespace="Microsoft.SharePoint" %> 
<%@ Register Tagprefix="WebPartPages" Namespace="Microsoft.SharePoint.WebPartPages" Assembly="Microsoft.SharePoint, Version=15.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %>
<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="GoogleAccountProfiles.ascx.cs" Inherits="Hemrika.SharePresence.WebSite.ControlTemplates.GoogleAccountProfiles" %>
<asp:UpdateProgress ID="prg_Google_Account" runat="server" AssociatedUpdatePanelID="pnl_Account_Profiles">
<ProgressTemplate>
<div data-role='progress-bar' indeterminate='true' value='1' min='0' max='100' id="progressBar2"></div>
</ProgressTemplate>
</asp:UpdateProgress>
<asp:UpdatePanel ID="pnl_Account_Profiles" runat="server" UpdateMode="Always" RenderMode="Block">
<ContentTemplate>
<asp:Panel ID="pnl_Google_Account_Border" runat="server" GroupingText="Google Profile Settings" >
<br />
    <asp:DropDownList ID="ddl_Google_Account_Profiles" runat="server" AutoPostBack="true" 
        onselectedindexchanged="ddl_Google_Account_Profiles_SelectedIndexChanged">
    </asp:DropDownList>
    <asp:Button ID="btn_Google_Profiles_Reload" Text="Reload" runat="server" 
        onclick="btn_Google_Profiles_Reload_Click" />
    <br />
        <asp:Label ID="lbl_Hostname" Text="Hostname" runat="server" ></asp:Label>
        <asp:TextBox ID="tbx_Hostname" runat="server" Enabled="false" ></asp:TextBox>
    <br />
        <asp:Label ID="lbl_ProfileId" Text="ProfileId" runat="server" ></asp:Label>
        <asp:TextBox ID="tbx_ProfileId" runat="server" Enabled="false" ></asp:TextBox>
    <br />
        <asp:Label ID="lbl_AccountId" Text="AccountId" runat="server" ></asp:Label>
        <asp:TextBox ID="tbx_AccountId" runat="server" Enabled="false" ></asp:TextBox>
    <br />
        <asp:Label ID="lbl_Token" Text="Token" runat="server" ></asp:Label>
        <asp:TextBox ID="tbx_Token" runat="server" Enabled="false" ></asp:TextBox>
    <br />
    <asp:CheckBox ID="cbx_Active" Text="Active" TextAlign="Left" runat="server" />
    <br />
    <asp:CheckBox ID="cbx_Hash" Text="Hash" TextAlign="Left" runat="server" />
    <br />
    <asp:CheckBox ID="cbx_Flash" Text="Flash" TextAlign="Left" runat="server" />
    <br />
    <asp:CheckBox ID="cbx_ClientInfo" Text="ClientInfo" TextAlign="Left" runat="server" />
    <br />
    <asp:CheckBox ID="cbx_Title" Text="Title" TextAlign="Left" runat="server" />
    <br />
    <asp:CheckBox ID="cbx_Linker" Text="Linker" TextAlign="Left" runat="server" />
    <br />
    <asp:Button ID="btn_Google_Account_Profile" runat="server" Text="Save" onclick="btn_Google_Account_Profile_Click"/>
    <br />
    <asp:Label ID="lbl_Profile_error" runat="server" ForeColor="Red"></asp:Label>
    <br />
    <asp:Label ID="lbl_Profile_correct" runat="server" ForeColor="Green"></asp:Label>
</asp:Panel>
</ContentTemplate>
    <Triggers>
        <asp:AsyncPostBackTrigger ControlID="btn_Google_Profiles_Reload" 
        EventName="Click" />
        <asp:AsyncPostBackTrigger ControlID="btn_Google_Account_Profile" 
            EventName="Click" />
        <asp:AsyncPostBackTrigger ControlID="ddl_Google_Account_Profiles" EventName="SelectedIndexChanged" />
    </Triggers>
</asp:UpdatePanel>