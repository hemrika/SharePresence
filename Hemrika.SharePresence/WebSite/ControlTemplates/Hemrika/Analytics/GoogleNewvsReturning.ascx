<%@ Assembly Name="Hemrika.SharePresence.WebSite, Version=1.0.0.0, Culture=neutral, PublicKeyToken=11e6604a27f32a11" %>
<%@ Assembly Name="Microsoft.Web.CommandUI, Version=15.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %>
<%@ Register Tagprefix="SharePoint" Namespace="Microsoft.SharePoint.WebControls" Assembly="Microsoft.SharePoint, Version=15.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %>
<%@ Register Tagprefix="Utilities" Namespace="Microsoft.SharePoint.Utilities" Assembly="Microsoft.SharePoint, Version=15.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %>
<%@ Register Tagprefix="asp" Namespace="System.Web.UI" Assembly="System.Web.Extensions, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35" %>
<%@ Import Namespace="Microsoft.SharePoint" %> 
<%@ Register Tagprefix="WebPartPages" Namespace="Microsoft.SharePoint.WebPartPages" Assembly="Microsoft.SharePoint, Version=15.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %>
<%@ Import Namespace="Hemrika.SharePresence.Google" %> 
<%@ Import Namespace="Hemrika.SharePresence.Google.Visualization" %> 
<%@ Register tagprefix="visual" namespace="Hemrika.SharePresence.Google.Visualization" Assembly="Hemrika.SharePresence.Google, Version=1.0.0.0, Culture=neutral, PublicKeyToken=11e6604a27f32a11" %>
<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="GoogleNewvsReturning.ascx.cs" Inherits="Hemrika.SharePresence.WebSite.ControlTemplates.GoogleNewvsReturning" %>
<visual:GVPieChart ID="pie_newvsreturning" runat="server" Width="500" Height="300" GviTitle="New vs Returning" GviLegend="Right" GviIs3D="true" />
</BR>
<visual:GVAreaChart ID="area_newvsreturning" runat="server" Width="100%" Height="400" />