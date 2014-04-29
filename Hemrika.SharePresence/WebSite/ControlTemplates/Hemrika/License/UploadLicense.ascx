<%@ Assembly Name="$SharePoint.Project.AssemblyFullName$" %>
<%@ Assembly Name="Microsoft.Web.CommandUI, Version=15.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %>
<%@ Register Tagprefix="SharePoint" Namespace="Microsoft.SharePoint.WebControls" Assembly="Microsoft.SharePoint, Version=15.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %>
<%@ Register Tagprefix="Utilities" Namespace="Microsoft.SharePoint.Utilities" Assembly="Microsoft.SharePoint, Version=15.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %>
<%@ Register Tagprefix="asp" Namespace="System.Web.UI" Assembly="System.Web.Extensions, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35" %>
<%@ Import Namespace="Microsoft.SharePoint" %> 
<%@ Register Tagprefix="WebPartPages" Namespace="Microsoft.SharePoint.WebPartPages" Assembly="Microsoft.SharePoint, Version=15.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %>
<%@ Register TagPrefix="wssuc" TagName="InputFormSection" Src="/_controltemplates/InputFormSection.ascx" %>
<%@ Register TagPrefix="wssuc" TagName="InputFormControl" Src="/_controltemplates/InputFormControl.ascx" %>
<%@ Register TagPrefix="wssuc" TagName="ButtonSection" Src="/_controltemplates/ButtonSection.ascx" %>
<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="UploadLicense.ascx.cs" Inherits="Hemrika.SharePresence.WebSite.ControlTemplates.UploadLicense" %>
<style type="text/css">
    fieldset {  
		margin: 			10px 0 0 0;  
		padding: 			0;
		width: 720px;
		background:			transparent url(fieldsetbg.gif) no-repeat bottom right;
	}

	legend {  
		text-transform:		capitalize;
		font-size:			1.3em;
		padding:			5px;
		margin-left:		1em;
		color:				#ffffff;
		background:			#493F0B;
	}
	
	fieldset ol {  
		padding: 			10px 10px 0 10px;  
		list-style: 		none;
	}
	
	fieldset li {  
		position:			relative;
		padding-bottom: 	1em;
		line-height:		3.4em;
	}
	
	fieldset#submitform {  
		background-image:	none;
		border-style: 		none;
	}
	
	label {
		position:			relative;
		clear:				left;
		float:				left;
		width:				15em;
		margin-right:		5px;
		padding-right:		30px;
		line-height:		3.4em;
		text-align:			right;
	}
	
	label.required {
		background:			transparent url(required.gif) no-repeat center right;
	}
	
	label span {
		position:			absolute;
		left:				-10000px;
		top:				0px;
	}
	
	p span.required {
		display:			inline-block;
		vertical-align: 	middle;
		line-height:		3.4em;
		width:				25px;
		line-height:		3.4em;
		text-indent:		-10000px;
		overflow:			hidden;
		background:			transparent url(required.gif) no-repeat center right;		
	}
	
	input {
		padding:			5px;
		font-size:			1.4em;
		border:				1px solid #493F0B; 
		color:				#1E1903; 
		background:			#F5F6D4;
	}
	
	input.radio {
		border:				none;
		background:			transparent;
	}

    textarea {
		padding:			5px;
		font-size:			1.4em;
		border:				1px solid #493F0B; 
		color:				#1E1903; 
		background:			#F5F6D4;
	}
    select {
		padding:			5px;
		font-size:			1.4em;
		border:				1px solid #493F0B; 
		color:				#1E1903; 
		width:              250px;
		background:			#F5F6D4;
	}
	fieldset span {  
		padding:			5px;
		font-size:			1.4em;
		border:				1px solid #493F0B; 
		color:				#1E1903; 
		width:              250px;
		height:             250px;
		background:			#F5F6D4;
	}
</style>
<fieldset id="personal">
	<legend>Upload License</legend>
	<ol>
		<li><label for="license" title="Select License File." class="required">Select 
            License</label>
            <asp:FileUpload ID="ful_license" runat="server" />
		</li>
		<li>
            <label for="upload" title="Action" class="required">Action</label>
            <asp:Button ID="Btn_Save" runat="server" onclick="Btn_Save_Click" Text="Save" />
            &nbsp;
            <asp:Button ID="btn_Cancel" runat="server" Text="Cancel" />
		</li>
        <li>
        <label for="license_result" title="License File." >Result</label>
		<asp:TextBox ID="tbx_Result" runat="server" Columns="25" Rows="20" 
                TextMode="MultiLine"></asp:TextBox>
        </li>
        
	</ol>
</fieldset>
