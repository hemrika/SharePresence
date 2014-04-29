<%@ Assembly Name="Hemrika.SharePresence.WebSite, Version=1.0.0.0, Culture=neutral, PublicKeyToken=11e6604a27f32a11" %>
<%@ Assembly Name="Microsoft.Web.CommandUI, Version=15.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %>
<%@ Register Tagprefix="SharePoint" Namespace="Microsoft.SharePoint.WebControls" Assembly="Microsoft.SharePoint, Version=15.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %>
<%@ Register Tagprefix="Utilities" Namespace="Microsoft.SharePoint.Utilities" Assembly="Microsoft.SharePoint, Version=15.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %>
<%@ Register Tagprefix="asp" Namespace="System.Web.UI" Assembly="System.Web.Extensions, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35" %>
<%@ Import Namespace="Microsoft.SharePoint" %> 
<%@ Register Tagprefix="WebPartPages" Namespace="Microsoft.SharePoint.WebPartPages" Assembly="Microsoft.SharePoint, Version=15.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %>
<%@ Register TagPrefix="wssuc" TagName="InputFormSection" Src="/_controltemplates/InputFormSection.ascx" %>
<%@ Register TagPrefix="wssuc" TagName="InputFormControl" Src="/_controltemplates/InputFormControl.ascx" %>
<%@ Register TagPrefix="wssuc" TagName="ButtonSection" Src="/_controltemplates/ButtonSection.ascx" %>
<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="ManageMasterPages.ascx.cs" Inherits="Hemrika.SharePresence.WebSite.ControlTemplates.ManageMasterPages" %>
<div data-role="page active" class="type-interior" data-theme="b">
	<div data-role="header" data-theme="b" data-position="fixed">
        <span class="ui-app-title">MasterPages</span>
	</div>

	<div data-role="content" data-theme="b">
        <h2>MasterPages</h2>
        <p>
			Select the MasterPage for your site.
        </p>
        <div data-role="fieldcontain">

                <asp:GridView ID="gridMasterPages" 
                      runat="server"
                      AutoGenerateColumns="False"
                      HeaderStyle-HorizontalAlign="Left" 
                      HeaderStyle-BackColor="#666666" 
                      HeaderStyle-ForeColor="#ffffff"
                       Width="100%" EnableModelValidation="True" >
            <HeaderStyle HorizontalAlign="Left" BackColor="#666666" ForeColor="White"></HeaderStyle>
            <EmptyDataTemplate> No Entries found. </EmptyDataTemplate>
            <EmptyDataRowStyle />
            <Columns>
                <asp:BoundField DataField="UniqueId" ItemStyle-Width="1px" ShowHeader="false" />
                <asp:BoundField HeaderText="Name" DataField="Name" ItemStyle-Width="25px" />
                <asp:BoundField HeaderText="Description" DataField="Description" />
                <asp:TemplateField ShowHeader="False" ItemStyle-VerticalAlign="middle" ItemStyle-HorizontalAlign="Center" >
                    <ItemTemplate>
                	    <asp:Button ID="btnUse" 
                                        runat="server" 
                                        Width="50px"
                                        OnClick="btnUse_Click"
                                        CausesValidation="false" 
                                        CommandName="btnUse" 
                                        Text="Use" data-role="button" data-inline="true" CssClass="ui-btn-up-b" />
                    </ItemTemplate>
                </asp:TemplateField>
            </Columns>
        </asp:GridView>
        </div>
        <div data-role="fieldcontain">
		<asp:Button ID="btn_Save" runat="server" Text="Finished"  onclick="btn_Save_Click" data-icon="check" data-inline="true" CssClass="ui-btn-up-b" data-iconpos="left" />
        </div>
    </div>
</div>