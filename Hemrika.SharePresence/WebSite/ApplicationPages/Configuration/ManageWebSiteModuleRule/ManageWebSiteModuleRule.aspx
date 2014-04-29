<%@ Assembly Name="Hemrika.SharePresence.WebSite, Version=1.0.0.0, Culture=neutral, PublicKeyToken=3421bd1d946bda6c" %>
<%@ Page Language="C#" MasterPageFile="~/_layouts/application.master" Inherits="Hemrika.SharePresence.WebSite.ManageWebSiteModuleRule" %>
<%@ Register TagPrefix="SharePoint" Namespace="Microsoft.SharePoint.WebControls" Assembly="Microsoft.SharePoint, Version=15.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %>
<%@ Register TagPrefix="Utilities" Namespace="Microsoft.SharePoint.Utilities" Assembly="Microsoft.SharePoint, Version=15.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %>
<%@ Register TagPrefix="wssuc" TagName="InputFormSection" Src="/_controltemplates/InputFormSection.ascx" %>
<%@ Register TagPrefix="wssuc" TagName="InputFormControl" Src="/_controltemplates/InputFormControl.ascx" %>
<%@ Register TagPrefix="wssuc" TagName="ButtonSection" Src="/_controltemplates/ButtonSection.ascx" %>
<%@ Register TagPrefix="wssawc" Namespace="Microsoft.SharePoint.WebControls" Assembly="Microsoft.SharePoint, Version=15.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %>

<asp:Content ContentPlaceHolderId="PlaceHolderPageTitle" runat="server">
	<SharePoint:EncodedLiteral runat="server" text="<%$Resources:Hemrika.SharePresence.WebSite.AppResources,ManageWebSiteModuleRule_PageTitle%>" EncodeMethod='HtmlEncode'/>
</asp:Content>
<asp:Content ContentPlaceHolderId="PlaceHolderPageTitleInTitleArea" runat="server">
	<SharePoint:EncodedLiteral runat="server" text="<%$Resources:Hemrika.SharePresence.WebSite.AppResources,ManageWebSiteModuleRule_PageTitle%>" EncodeMethod='HtmlEncode'/>
</asp:Content>
<asp:Content ID="PageDescription" ContentPlaceHolderID="PlaceHolderPageDescription"  runat="server">
    <SharePoint:EncodedLiteral runat="server" text="<%$Resources:Hemrika.SharePresence.WebSite.AppResources,ManageWebSiteModuleRule_PageDescription%>" EncodeMethod='HtmlEncode'/>
</asp:Content>
<asp:Content ID="Main" ContentPlaceHolderID="PlaceHolderMain" runat="server">
<table width="100%" class="propertysheet" cellpadding="0" cellspacing="0" border="0">
    <tr><td class="ms-error"><asp:Literal ID="ErrorMessageLiteral" runat="server" EnableViewState="false" /> </td></tr>
  </table>
  <table border="0" cellspacing="0" cellpadding="0" width="100%">
    <tr>
      <td>
        <asp:PlaceHolder ID="propertiesPlaceholder" runat="server" />
        <asp:PlaceHolder ID="defaultPlaceHolder" runat="server" >
        <wssuc:InputFormSection runat="server"
                                title="<%$Resources:Hemrika.SharePresence.WebSite.AppResources, pc_newentry_pageTitle%>"
                                Description="<%$Resources:Hemrika.SharePresence.WebSite.AppResources, pc_newentry_pageDesc%>">
        <Template_InputFormControls>
               <asp:Label id="webAppUrlLabel" runat="server" />
               <asp:TextBox id="pageTextBox" runat="server" columns="50"/>
               <wssawc:InputFormRequiredFieldValidator ID="pageValidator"
                  ControlToValidate="pageTextBox" Text="Error" runat="server"
                  ErrorMessage ="A Page is required" EnableClientScript="true" Display="Dynamic"/>
          </Template_InputFormControls>
        </wssuc:InputFormSection>

        <wssuc:InputFormSection runat="server"
                                title="<%$Resources:Hemrika.SharePresence.WebSite.AppResources, pc_newentry_isDisabledTitle%>"
                                Description="<%$Resources:Hemrika.SharePresence.WebSite.AppResources, pc_newentry_isDisabledDesc%>">        
            <Template_InputFormControls>
                <asp:CheckBox id="disabledCheckBox" runat="server"/>
            </Template_InputFormControls>
        </wssuc:InputFormSection>
        
        <wssuc:InputFormSection runat="server"
                                title="<%$Resources:Hemrika.SharePresence.WebSite.AppResources, pc_newentry_appliesToSslTitle%>"
                                Description="<%$Resources:Hemrika.SharePresence.WebSite.AppResources, pc_newentry_appliesToSslDesc%>">
              <Template_InputFormControls>
                   <asp:CheckBox id="appliesToSslCheckBox" runat="server"/>
              </Template_InputFormControls>
        </wssuc:InputFormSection>
        
        <wssuc:InputFormSection runat="server"
                                title="<%$Resources:Hemrika.SharePresence.WebSite.AppResources, pc_newentry_sequenceTitle%>"
                                Description="<%$Resources:Hemrika.SharePresence.WebSite.AppResources, pc_newentry_sequenceDesc%>">
            <Template_InputFormControls>
               <asp:TextBox id="sequenceTextBox" runat="server" columns="4"/>
               <asp:RangeValidator ID="RangeValidator1" runat="server" Display="Dynamic" ControlToValidate="sequenceTextBox"
                    Type="Integer" ErrorMessage="The sequence must be between 0 and 9999"
                    MinimumValue="0" MaximumValue="9999" />
            </Template_InputFormControls>
        </wssuc:InputFormSection>

        <!-- principal type -->
        <wssuc:InputFormSection runat="server"
                                  Title="<%$Resources:Hemrika.SharePresence.WebSite.AppResources, pc_newentry_principalTypeTitle%>"
                                  Description="<%$Resources:Hemrika.SharePresence.WebSite.AppResources,pc_newentry_principalTypeDesc %>"
                                  ID="principalTypeSection">
          <Template_InputFormControls>
          <tr><td>
            <asp:RadioButtonList ID="principalTypeList" runat="server">
                <asp:ListItem Selected="True" class="ms-descriptionText" Value="User" Text="<%$Resources:Hemrika.SharePresence.WebSite.AppResources,pc_newentry_principalTypeChoiceUser%>" />
                <asp:ListItem class="ms-descriptionText" Value="Group" Text="<%$Resources:Hemrika.SharePresence.WebSite.AppResources,pc_newentry_principalTypeChoiceGroup%>" />
            </asp:RadioButtonList>
            </td></tr>
          </Template_InputFormControls>
        </wssuc:InputFormSection>
        
         <!-- principal -->
        <wssuc:InputFormSection runat="server"
                                title="<%$Resources:Hemrika.SharePresence.WebSite.AppResources,pc_newentry_principalTitle %>"
                                Description="<%$Resources:Hemrika.SharePresence.WebSite.AppResources,pc_newentry_principalDesc%>">
          <Template_InputFormControls>
            <asp:TextBox id="principalTextBox" runat="server"/>
          </Template_InputFormControls>
        </wssuc:InputFormSection>
        </asp:PlaceHolder>        
      </td>
    </tr>
  </table>	    
<SharePoint:FormDigest ID="FormDigest1" runat="server" />
</asp:Content>

