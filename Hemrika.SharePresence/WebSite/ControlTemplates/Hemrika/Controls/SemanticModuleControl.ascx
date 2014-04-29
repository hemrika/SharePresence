<%@ Assembly Name="Hemrika.SharePresence.WebSite, Version=1.0.0.0, Culture=neutral, PublicKeyToken=11e6604a27f32a11" %>
<%@ Assembly Name="Microsoft.Web.CommandUI, Version=15.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %>
<%@ Register Tagprefix="SharePoint" Namespace="Microsoft.SharePoint.WebControls" Assembly="Microsoft.SharePoint, Version=15.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %>
<%@ Register Tagprefix="Utilities" Namespace="Microsoft.SharePoint.Utilities" Assembly="Microsoft.SharePoint, Version=15.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %>
<%@ Register Tagprefix="asp" Namespace="System.Web.UI" Assembly="System.Web.Extensions, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35" %>
<%@ Import Namespace="Microsoft.SharePoint" %> 
<%@ Register Tagprefix="SharePoint" Namespace="Microsoft.SharePoint.WebControls" Assembly="Microsoft.SharePoint, Version=15.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %>
<%@ Register TagPrefix="wssuc" TagName="InputFormSection" src="~/_controltemplates/InputFormSection.ascx" %>
<%@ Register TagPrefix="wssawc" Namespace="Microsoft.SharePoint.WebControls" Assembly="Microsoft.SharePoint, Version=15.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %>
<%@ Register Tagprefix="WebPartPages" Namespace="Microsoft.SharePoint.WebPartPages" Assembly="Microsoft.SharePoint, Version=15.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %>
<%@ Control Language="C#" AutoEventWireup="true" Inherits="Hemrika.SharePresence.WebSite.Modules.SemanticModule.SemanticModuleControl" %>
            
<!-- Original Url -->
<wssuc:InputFormSection runat="server"
                        title="<%$Resources:Hemrika.SharePresence.WebSite.AppResources,SemanticUrl_Title%>"
                        Description="<%$Resources:Hemrika.SharePresence.WebSite.AppResources,SemanticUrl_Description%>">
    <Template_InputFormControls>
        <asp:Label id="SemanticUrlLabel" runat="server" />
        <asp:TextBox id="OriginalUrlTextBox" runat="server" columns="50"/>
        <wssawc:InputFormRequiredFieldValidator ID="InputFormRequiredFieldValidator1"
            ControlToValidate="OriginalUrlTextBox" Text="Error" runat="server"
            ErrorMessage ="A original Url is required" EnableClientScript="true" Display="Dynamic"/>
    </Template_InputFormControls>
</wssuc:InputFormSection>
