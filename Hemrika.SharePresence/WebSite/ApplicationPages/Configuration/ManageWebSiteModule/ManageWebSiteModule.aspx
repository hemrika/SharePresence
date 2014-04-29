<%@ Assembly Name="Hemrika.SharePresence.WebSite, Version=1.0.0.0, Culture=neutral, PublicKeyToken=3421bd1d946bda6c" %>
<%@ Page Language="C#" MasterPageFile="~/_layouts/application.master" Inherits="Hemrika.SharePresence.WebSite.ManageWebSiteModule" %>
<%@ Register TagPrefix="SharePoint" Namespace="Microsoft.SharePoint.WebControls" Assembly="Microsoft.SharePoint, Version=15.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %>
<%@ Register TagPrefix="Utilities" Namespace="Microsoft.SharePoint.Utilities" Assembly="Microsoft.SharePoint, Version=15.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %>
<%@ Register TagPrefix="wssuc" TagName="InputFormSection" Src="/_controltemplates/InputFormSection.ascx" %>
<%@ Register TagPrefix="wssuc" TagName="InputFormControl" Src="/_controltemplates/InputFormControl.ascx" %>
<%@ Register TagPrefix="wssuc" TagName="ButtonSection" Src="/_controltemplates/ButtonSection.ascx" %>
<%@ Register TagPrefix="wssawc" Namespace="Microsoft.SharePoint.WebControls" Assembly="Microsoft.SharePoint, Version=15.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %>

<asp:Content ContentPlaceHolderId="PlaceHolderPageTitle" runat="server">
	<SharePoint:EncodedLiteral runat="server" text="<%$Resources:Hemrika.SharePresence.WebSite.AppResources,ManageWebSiteModule_PageTitle%>" EncodeMethod='HtmlEncode'/>
</asp:Content>
<asp:Content ContentPlaceHolderId="PlaceHolderPageTitleInTitleArea" runat="server">
	<SharePoint:EncodedLiteral runat="server" text="<%$Resources:Hemrika.SharePresence.WebSite.AppResources,ManageWebSiteModule_PageTitle%>" EncodeMethod='HtmlEncode'/>
</asp:Content>
<asp:Content ID="PageDescription" ContentPlaceHolderID="PlaceHolderPageDescription"  runat="server">
    <SharePoint:EncodedLiteral runat="server" text="<%$Resources:Hemrika.SharePresence.WebSite.AppResources,ManageWebSiteModule_PageDescription%>" EncodeMethod='HtmlEncode'/>
</asp:Content>
<asp:Content ID="Main" ContentPlaceHolderID="PlaceHolderMain" runat="server">
  <table width="100%" class="propertysheet" cellpadding="0" cellspacing="0" border="0">
    <tr><td class="ms-error"><asp:Literal ID="ErrorMessageLiteral" runat="server" EnableViewState="false" /> </td></tr>
  </table>
  <table border="0" cellspacing="0" cellpadding="0" width="100%">
    <tr>
      <td>          
         <!-- action class -->
        <wssuc:InputFormSection runat="server"
                                title="<%$Resources:Hemrika.SharePresence.WebSite.AppResources,pc_newmodule_classTitle%>"
                                Description="<%$Resources:Hemrika.SharePresence.WebSite.AppResources,pc_newmodule_classDesc%>">
                                
        <Template_InputFormControls>
           <asp:TextBox id="moduleTextBox" runat="server" Width="100%"/>
                    <wssawc:InputFormRequiredFieldValidator ID="InputFormRequiredFieldValidator1"
                        ControlToValidate="moduleTextBox" Text="Error" runat="server"
                        ErrorMessage ="A Module name is required" EnableClientScript="true" Display="Dynamic"/>
               
          </Template_InputFormControls>
        </wssuc:InputFormSection>
       </td>
       <td>          
         <!-- action class -->
        <wssuc:InputFormSection runat="server"
                                title="<%$Resources:Hemrika.SharePresence.WebSite.AppResources,pc_newmodule_classTitle%>"
                                Description="<%$Resources:Hemrika.SharePresence.WebSite.AppResources,pc_newmodule_classDesc%>">
                                
        <Template_InputFormControls>
           <asp:TextBox id="assemblyTextbox" runat="server" Width="100%"/>
                    <wssawc:InputFormRequiredFieldValidator ID="InputFormRequiredFieldValidator2"
                        ControlToValidate="assemblyTextbox" Text="Error" runat="server"
                        ErrorMessage ="A fully qualified class name is required" EnableClientScript="true" Display="Dynamic"/>
               
          </Template_InputFormControls>
        </wssuc:InputFormSection>
       </td>
    </tr>
  </table>
  	<asp:Table runat="server" border="0" cellspacing="4" cellpadding="0" width="100%" id="entriesTable">
       	<asp:TableHeaderRow>
       	    <asp:TableHeaderCell HorizontalAlign="Left"><asp:Label ID="Label3" runat="server" Text="<%$Resources:Hemrika.SharePresence.WebSite.AppResources,pc_adminpage_headerRuleType%>"/></asp:TableHeaderCell>
       	    <asp:TableHeaderCell HorizontalAlign="Left"><asp:Label ID="Label4" runat="server" Text="<%$Resources:Hemrika.SharePresence.WebSite.AppResources,pc_adminpage_headerPage%>"/></asp:TableHeaderCell>
       	    <asp:TableHeaderCell HorizontalAlign="Left"><asp:Label ID="Label6" runat="server" Text="<%$Resources:Hemrika.SharePresence.WebSite.AppResources,pc_adminpage_headerPrincipal%>"/></asp:TableHeaderCell>
       	    <asp:TableHeaderCell HorizontalAlign="Center"><asp:Label ID="Label7" runat="server" Text="<%$Resources:Hemrika.SharePresence.WebSite.AppResources,pc_adminpage_headerPrincipalType%>"/></asp:TableHeaderCell>
       	    <asp:TableHeaderCell HorizontalAlign="Center"><asp:Label ID="Label1" runat="server" Text="<%$Resources:Hemrika.SharePresence.WebSite.AppResources,pc_adminpage_headerDisabled%>"/></asp:TableHeaderCell>
       	    <asp:TableHeaderCell HorizontalAlign="Center"><asp:Label ID="Label2" runat="server" Text="<%$Resources:Hemrika.SharePresence.WebSite.AppResources,pc_adminpage_headerAppliesToSsl%>"/></asp:TableHeaderCell>
       	    <asp:TableHeaderCell HorizontalAlign="Center"><asp:Label ID="Label5" runat="server" Text="<%$Resources:Hemrika.SharePresence.WebSite.AppResources,pc_adminpage_headerSequence%>"/></asp:TableHeaderCell>
       	    <asp:TableHeaderCell HorizontalAlign="Left"><asp:Label ID="Label8" runat="server" Text="<%$Resources:Hemrika.SharePresence.WebSite.AppResources,pc_adminpage_headerProperties%>"/></asp:TableHeaderCell>
       </asp:TableHeaderRow>
   </asp:Table>
    <SharePoint:FormDigest ID="FormDigest1" runat="server" />
</asp:Content>

