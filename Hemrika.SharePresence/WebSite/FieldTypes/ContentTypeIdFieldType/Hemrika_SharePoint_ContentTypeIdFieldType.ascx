<%@ Control Language="C#" Debug="true" %>
<%@ Assembly Name="Microsoft.SharePoint, Version=15.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %>
<%@ Register TagPrefix="SharePoint" Assembly="Microsoft.SharePoint, Version=15.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" Namespace="Microsoft.SharePoint.WebControls" %>
<SharePoint:RenderingTemplate ID="ContentTypeIdField" runat="server">
    <Template>
    <asp:UpdatePanel ID="UpdatePanel1" runat="server">
   <ContentTemplate>
        Content Type Group<br />
        <asp:DropDownList ID="ddl_contentgroup" runat="server">
           
        </asp:DropDownList>
        <br />
       
        Content Type Name<br />
        <asp:DropDownList ID="ddl_contentname" runat="server">
        </asp:DropDownList>
        <br />
        Description<br />
        <asp:Label ID="lbl_Description" runat="server"></asp:Label>
        </ContentTemplate>
         </asp:UpdatePanel>
    </Template>
</SharePoint:RenderingTemplate>
<SharePoint:RenderingTemplate ID="ContentTypeIdDisplay" runat="server">
    <Template>
        <asp:Label ID="lbl_ContentType" runat="server"></asp:Label>
    </Template>
</SharePoint:RenderingTemplate>
