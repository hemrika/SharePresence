<%@ Control Language="C#" Debug="true" %>
<%@ Assembly Name="Microsoft.SharePoint, Version=15.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %>
<%@ Register TagPrefix="SharePoint" Assembly="Microsoft.SharePoint, Version=15.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" Namespace="Microsoft.SharePoint.WebControls" %>
<SharePoint:RenderingTemplate ID="SchemaOrg_ItemType" runat="server">
  <Template>
      <asp:TextBox ID="tbx_type" runat="server" CssClass="ms-long" />
 </Template>
</SharePoint:RenderingTemplate>
<SharePoint:RenderingTemplate ID="SchemaOrg_ItemTypeDisplay" runat="server">
  <Template>
      <div itemscope="itemscope" itemtype="http://schema.org/" runat="server" id="scope">
      </div>
 </Template>
</SharePoint:RenderingTemplate>
<SharePoint:RenderingTemplate ID="SchemaOrg_ItemProperty" runat="server">
  <Template>
      <asp:TextBox ID="tbx_type" runat="server" CssClass="ms-long" />
      <asp:TextBox ID="tbx_property" runat="server" CssClass="ms-long" />
 </Template>
</SharePoint:RenderingTemplate>
<SharePoint:RenderingTemplate ID="SchemaOrg_ItemPropertyDisplay" runat="server">
  <Template>
      <div itemprop="Property" itemscope="itemscope" itemtype="http://schema.org/" runat="server" id="scope">
      </div>
 </Template>
</SharePoint:RenderingTemplate>

