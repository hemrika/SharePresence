<%@ Assembly Name="Hemrika.SharePresence.WebSite, Version=1.0.0.0, Culture=neutral, PublicKeyToken=11e6604a27f32a11" %>
<%@ Assembly Name="Microsoft.Web.CommandUI, Version=15.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %>
<%@ Register Tagprefix="SharePoint" Namespace="Microsoft.SharePoint.WebControls" Assembly="Microsoft.SharePoint, Version=15.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %>
<%@ Register Tagprefix="Utilities" Namespace="Microsoft.SharePoint.Utilities" Assembly="Microsoft.SharePoint, Version=15.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %>
<%@ Register Tagprefix="asp" Namespace="System.Web.UI" Assembly="System.Web.Extensions, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35" %>
<%@ Import Namespace="Microsoft.SharePoint" %> 
<%@ Register Tagprefix="WebPartPages" Namespace="Microsoft.SharePoint.WebPartPages" Assembly="Microsoft.SharePoint, Version=15.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %>
<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="GoogleAccountUser.ascx.cs" Inherits="Hemrika.SharePresence.WebSite.ControlTemplates.GoogleAccountUser" %>
<asp:UpdateProgress ID="prg_Google_Account" runat="server" AssociatedUpdatePanelID="pnl_Google_Account">
<ProgressTemplate>
<div data-role='progress-bar' indeterminate='true' id="progressBar1" style="width:200px;"></div>
</ProgressTemplate>
</asp:UpdateProgress>
<asp:UpdatePanel ID="pnl_Google_Account" runat="server" UpdateMode="Always" RenderMode="Block">
    <ContentTemplate>
    <asp:Panel ID="pnl_Google_Account_Border" runat="server" GroupingText="Google Account Settings">
        <br />
            <asp:Label ID="lbl_verified" runat="server" ForeColor="Green" ></asp:Label>
        <br />
            <asp:Label ID="lbl_Google_Account_Name" runat="server" Text="Username :" Width="100px"></asp:Label>
            <asp:TextBox ID="tbx_Google_Account_Name" runat="server" Width="255px"></asp:TextBox>
        <br />
            <asp:Label ID="lbl_Google_Account_Password" runat="server" Text="Password :" Width="100px"></asp:Label>
            <asp:TextBox ID="tbx_Google_Account_Password" runat="server" TextMode="Password" Width="255px"></asp:TextBox>
        <br />
        <br />
            <asp:Button ID="btn_Google_Account_Submit" runat="server" onclick="btn_Google_Account_Submit_Click" Text="Check and Save" data-role="button" data-inline="true" />
            &nbsp;&nbsp;
            <asp:Button ID="btn_Goole_Account_Clear" runat="server" onclick="btn_Google_Account_Clear_Click" Text="Clear Account" data-role="button" data-inline="true" />
            &nbsp;&nbsp;
            <asp:Button ID="btn_Google_Account_Remove" runat="server" onclick="btn_Google_Account_Remove_Click" Text="Remove Account" data-role="button" data-inline="true" />
        <br />
            <asp:Label ID="lbl_error" runat="server" ForeColor="Red"></asp:Label>
        <br />
            <asp:Label ID="lbl_correct" runat="server" ForeColor="Green"></asp:Label>
        </asp:Panel>
    </ContentTemplate>
    <Triggers>
        <asp:AsyncPostBackTrigger ControlID="btn_Google_Account_Submit" 
            EventName="Click" />
        <asp:AsyncPostBackTrigger ControlID="btn_Goole_Account_Clear" 
            EventName="Click" />
        <asp:AsyncPostBackTrigger ControlID="btn_Google_Account_Remove" 
            EventName="Click" />
    </Triggers>
</asp:UpdatePanel>
