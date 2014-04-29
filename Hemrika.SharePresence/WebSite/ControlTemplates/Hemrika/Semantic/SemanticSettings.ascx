<%@ Assembly Name="Hemrika.SharePresence.WebSite, Version=1.0.0.0, Culture=neutral, PublicKeyToken=3421bd1d946bda6c" %>
<%@ Assembly Name="Microsoft.Web.CommandUI, Version=15.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %>
<%@ Register Tagprefix="SharePoint" Namespace="Microsoft.SharePoint.WebControls" Assembly="Microsoft.SharePoint, Version=15.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %>
<%@ Register Tagprefix="Utilities" Namespace="Microsoft.SharePoint.Utilities" Assembly="Microsoft.SharePoint, Version=15.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %>
<%@ Register Tagprefix="asp" Namespace="System.Web.UI" Assembly="System.Web.Extensions, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35" %>
<%@ Import Namespace="Microsoft.SharePoint" %> 
<%@ Register Tagprefix="WebPartPages" Namespace="Microsoft.SharePoint.WebPartPages" Assembly="Microsoft.SharePoint, Version=15.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %>
<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="SemanticSettings.ascx.cs" Inherits="Hemrika.SharePresence.WebSite.ControlTemplates.SemanticSettings" %>
<%@ Register assembly="System.Web.Extensions, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35" namespace="System.Web.UI.WebControls" tagprefix="asp" %>
<%@ Register assembly="Hemrika.SharePresence.WebSite, Version=1.0.0.0, Culture=neutral, PublicKeyToken=3421bd1d946bda6c" namespace="Hemrika.SharePresence.WebSite.Navigation" tagprefix="cc1" %>
<div data-role="page active" class="type-interior" data-theme="b">
	<div data-role="header" data-theme="b" data-position="fixed">
        <span class="ui-app-title">Semantic Settings</span>
	</div>

	<div data-role="content" data-theme="b">
        <h2>Semantic Settings</h2>
        <p>
			Add and Remove Semantic Url's for this page.
        </p>
        <div data-role="fieldcontain">

                <asp:GridView ID="gridSemantic" 
                      runat="server"
                      AutoGenerateColumns="False"
                      HeaderStyle-HorizontalAlign="Left" 
                      HeaderStyle-BackColor="#666666" 
                      HeaderStyle-ForeColor="#ffffff"
                       Width="100%" >
            <HeaderStyle HorizontalAlign="Left" BackColor="#666666" ForeColor="White"></HeaderStyle>
            <EmptyDataTemplate> No Entries found. </EmptyDataTemplate>
            <EmptyDataRowStyle />
            <Columns>
                <asp:BoundField DataField="Id" ItemStyle-Width="1px" ShowHeader="false" />
                <asp:BoundField HeaderText="Semantic Url" DataField="Semantic" />
                <asp:BoundField HeaderText="Disabled" DataField="Disabled" ItemStyle-Width="25px" />
                <asp:BoundField DataField="OriginalUrl" ItemStyle-Width="1px" ShowHeader="false" />
                <asp:TemplateField ShowHeader="False" ItemStyle-VerticalAlign="middle" ItemStyle-HorizontalAlign="Center" >
                    <ItemTemplate>
                	    <asp:Button ID="btnDelete" 
                                        runat="server" 
                                        Width="50px"
                                        OnClick="btnDelete_Click"
                                        CausesValidation="false" 
                                        CommandName="btnDelete" 
                                        Text="Delete" data-role="button" data-inline="true" CssClass="ui-btn-up-b" />
                    </ItemTemplate>
                </asp:TemplateField>
            </Columns>
        </asp:GridView>
        </div>
        <div data-role="fieldcontain">
                <label for="Url" title="Semantic Url" id="lbl_url" runat="server" >Semantic Url :</label>
                <asp:TextBox ID="tbx_url" runat="server"></asp:TextBox>
                <asp:Button ID="btn_Add" runat="server" Text="Add" data-icon="gear"  onclick="btn_Add_Click" data-inline="true" CssClass="ui-btn-up-b" data-iconpos="left"/>
                <br />
                <asp:Panel runat="server" ID="pnl_wait" >
                    <img src="/_layouts/Hemrika/Content/images/wait-indicator.gif" />
                </asp:Panel>
                <label after="Url" title="Semantic Error" id="lbl_error" runat="server" style="font-weight: bold; color: #FF0000" visible="false" >This url is already in use.</label>
        </div>
        <div data-role="fieldcontain">
		<asp:Button ID="btn_Save" runat="server" Text="Finished"  onclick="btn_Save_Click" data-icon="check" data-inline="true" CssClass="ui-btn-up-b" data-iconpos="left" />
		<asp:Button ID="btn_Cancel" runat="server" Text="Refresh" data-icon="refresh" data-inline="true" CssClass="ui-btn-up-b" data-iconpos="left"/>
        </div>
    </div>
</div>