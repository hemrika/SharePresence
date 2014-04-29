<%@ Assembly Name="Hemrika.SharePresence.WebSite, Version=1.0.0.0, Culture=neutral, PublicKeyToken=3421bd1d946bda6c" %>
<%@ Assembly Name="Microsoft.Web.CommandUI, Version=15.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %>
<%@ Register Tagprefix="SharePoint" Namespace="Microsoft.SharePoint.WebControls" Assembly="Microsoft.SharePoint, Version=15.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %>
<%@ Register Tagprefix="Utilities" Namespace="Microsoft.SharePoint.Utilities" Assembly="Microsoft.SharePoint, Version=15.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %>
<%@ Register Tagprefix="asp" Namespace="System.Web.UI" Assembly="System.Web.Extensions, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35" %>
<%@ Import Namespace="Microsoft.SharePoint" %> 
<%@ Register Tagprefix="WebPartPages" Namespace="Microsoft.SharePoint.WebPartPages" Assembly="Microsoft.SharePoint, Version=15.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %>
<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="GoogleAccountInfo.ascx.cs" Inherits="Hemrika.SharePresence.WebSite.ControlTemplates.GoogleAccountInfo" %>
<asp:Panel ID="pnl_AccountInfo" runat="server" 
    GroupingText="Google Account Info" Height="117px" HorizontalAlign="Left" 
    Width="412px">
    <br />
    <asp:Label ID="lbl_ProfileID" runat="server" Text="ProfileID :"></asp:Label>
    <asp:Literal ID="ltl_ProfileID" runat="server"></asp:Literal>
    <br />
    <asp:Label ID="lbl_AccountID" runat="server" Text="AccountID :"></asp:Label>
    <asp:Literal ID="ltl_AccountID" runat="server"></asp:Literal>
    <br />
    <asp:Label ID="lbl_Token" runat="server" Text="Token :"></asp:Label>
    <asp:Literal ID="ltl_Token" runat="server"></asp:Literal>
    <br />
    <br />
</asp:Panel>



