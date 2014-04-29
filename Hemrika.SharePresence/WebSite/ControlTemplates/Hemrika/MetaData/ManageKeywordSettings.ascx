<%@ Assembly Name="Hemrika.SharePresence.WebSite, Version=1.0.0.0, Culture=neutral, PublicKeyToken=3421bd1d946bda6c" %>
<%@ Assembly Name="Microsoft.Web.CommandUI, Version=15.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %>
<%@ Register Tagprefix="SharePoint" Namespace="Microsoft.SharePoint.WebControls" Assembly="Microsoft.SharePoint, Version=15.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %>
<%@ Register Tagprefix="Utilities" Namespace="Microsoft.SharePoint.Utilities" Assembly="Microsoft.SharePoint, Version=15.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %>
<%@ Register Tagprefix="asp" Namespace="System.Web.UI" Assembly="System.Web.Extensions, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35" %>
<%@ Register TagPrefix="HTML5" Assembly="Hemrika.SharePresence.HTML5, Version=1.0.0.0, Culture=neutral, PublicKeyToken=3421bd1d946bda6c" Namespace="Hemrika.SharePresence.Html5.WebControls" %>
<%@ Import Namespace="Microsoft.SharePoint" %> 
<%@ Register Tagprefix="WebPartPages" Namespace="Microsoft.SharePoint.WebPartPages" Assembly="Microsoft.SharePoint, Version=15.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %>
<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="ManageKeywordSettings.ascx.cs" Inherits="Hemrika.SharePresence.WebSite.ControlTemplates.ManageKeywordSettings" %>
<div data-role="page active" class="type-interior" data-theme="b">
	<div data-role="content" data-theme="b">
        <p>
			Configure the Keyword Settings for the site.
        </p>
        <div data-role="fieldcontain">
                <asp:GridView ID="gridKeywords" 
                      runat="server"
                      AutoGenerateColumns="False"
                      HeaderStyle-HorizontalAlign="Left" 
                      HeaderStyle-BackColor="#666666" 
                      HeaderStyle-ForeColor="#ffffff"
                       Width="100%" >
            <HeaderStyle HorizontalAlign="Left" BackColor="#666666" ForeColor="White"></HeaderStyle>
            <EmptyDataTemplate> No Keywords found. </EmptyDataTemplate>
            <EmptyDataRowStyle />
            <Columns>
                <asp:BoundField DataField="Id" ItemStyle-Width="1px" ShowHeader="false" />
                <asp:BoundField HeaderText="Words" DataField="keywords" />
                <asp:TemplateField ShowHeader="False" ItemStyle-VerticalAlign="middle" ItemStyle-HorizontalAlign="Center" >
                    <ItemTemplate>
                	    <asp:Button ID="btnEdit" 
                                        runat="server" 
                                        Width="50px"
                                        OnClick="btnEdit_Click"
                                        CausesValidation="false" 
                                        CommandName="btnEdit" 
                                        Text="Edit" data-role="button" data-inline="true" CssClass="ui-btn-up-b" />
                    </ItemTemplate>
                </asp:TemplateField>
            </Columns>
        </asp:GridView>

            <hr />
            <asp:Button ID="btn_Save" class="ms-ButtonHeightWidth" runat="server" Text="Save" data-role="button" data-inline="true" CssClass="ui-btn-up-b" />
            <asp:Button ID="btn_Cancel" class="ms-ButtonHeightWidth" runat="server" Text="Cancel" OnClientClick="SP.UI.ModalDialog.commonModalDialogClose(SP.UI.DialogResult.CANCEL,'No changes have been saved.'); return false;" data-role="button" data-inline="true" CssClass="ui-btn-up-b" />
        </div>
    </div>
</div>