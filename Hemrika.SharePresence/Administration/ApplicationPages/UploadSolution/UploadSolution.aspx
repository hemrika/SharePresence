<%@ Assembly Name="Hemrika.SharePresence.Administration, Version=1.0.0.0, Culture=neutral, PublicKeyToken=11e6604a27f32a11" %>
<%@ Page Language="C#" MasterPageFile="~/_admin/admin.master" Inherits="Hemrika.SharePresence.Administration.UploadSolution" %>
<%@ Register TagPrefix="SharePoint" Namespace="Microsoft.SharePoint.WebControls" Assembly="Microsoft.SharePoint, Version=15.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %>
<%@ Register TagPrefix="Utilities" Namespace="Microsoft.SharePoint.Utilities" Assembly="Microsoft.SharePoint, Version=15.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %>
<%@ Register TagPrefix="wssuc" TagName="InputFormSection" Src="/_controltemplates/InputFormSection.ascx" %>
<%@ Register TagPrefix="wssuc" TagName="InputFormControl" Src="/_controltemplates/InputFormControl.ascx" %>
<%@ Register TagPrefix="wssuc" TagName="ButtonSection" Src="/_controltemplates/ButtonSection.ascx" %>

<asp:Content ContentPlaceHolderId="PlaceHolderPageTitle" runat="server">
	<SharePoint:EncodedLiteral runat="server" text="<%$Resources:Hemrika.SharePresence.Administration.AppResources,UploadSolution_PageTitle%>" EncodeMethod="HtmlEncode"/>
</asp:Content>
<asp:Content ContentPlaceHolderId="PlaceHolderPageTitleInTitleArea" runat="server">
	<SharePoint:EncodedLiteral runat="server" text="<%$Resources:Hemrika.SharePresence.Administration.AppResources,UploadSolution_PageTitle%>" EncodeMethod="HtmlEncode"/>
</asp:Content>
<asp:Content ID="PageDescription" ContentPlaceHolderID="PlaceHolderPageDescription"  runat="server">
	<SharePoint:EncodedLiteral runat="server" text="<%$Resources:Hemrika.SharePresence.Administration.AppResources,UploadSolution_PageDescription%>" EncodeMethod="HtmlEncode"/>
</asp:Content>
<asp:Content ID="Main" ContentPlaceHolderID="PlaceHolderMain" runat="server">

	<table id="maintable" border="0" cellspacing="0" cellpadding="0" class="ms-propertysheet" width="100%">
	  <wssuc:InputFormSection Title="<%$Resources:Hemrika.SharePresence.Administration.AppResources,UploadSolution_Title%>" runat="server">
		<Template_Description>
		  <SharePoint:EncodedLiteral ID="EncodedLiteral1" runat="server" text="<%$Resources:Hemrika.SharePresence.Administration.AppResources,UploadSolution_Description%>" EncodeMethod="HtmlEncode"/>
		</Template_Description>
		<Template_InputFormControls>
		  <wssuc:InputFormControl runat="server">
			<Template_Control>
			  <table border="0" cellspacing="1">
				<tr>
				  <td class="ms-authoringcontrols" colspan="2" nowrap="nowrap">
					<SharePoint:EncodedLiteral ID="lbl_instruction" runat="server" text="<%$Resources:Hemrika.SharePresence.Administration.AppResources,UploadSolution_Instruction%>" EncodeMethod="HtmlEncode"/>:
					<font size="3">&#160;</font><br />
				  </td>
				</tr>
				<tr>
				  <td dir="ltr">
					  <asp:FileUpload ID="fup_wsp" runat="server" CssClass="ms-input" />               
				  </td>
				</tr>
			  </table>
			</Template_Control>
		  </wssuc:InputFormControl>
		</Template_InputFormControls>
	  </wssuc:InputFormSection>
	  <wssuc:ButtonSection runat="server" ShowStandardCancelButton="False">
		<Template_Buttons>
		  <asp:placeholder ID="Placeholder1" runat="server">
			<asp:Button runat="server" class="ms-ButtonHeightWidth" ID="BtnOk" Text="Ok" />
			<SeparatorHtml>
				<span id="idSpace" class="ms-SpaceBetButtons" />
			</SeparatorHtml>
			<asp:Button runat="server" class="ms-ButtonHeightWidth" ID="BtnCancel" Text="Cancel" />           
		  </asp:PlaceHolder>
		</Template_Buttons>
	  </wssuc:ButtonSection>
	</table>     
		
	<SharePoint:FormDigest ID="FormDigest1" runat="server" />
</asp:Content>

