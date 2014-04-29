<%@ Assembly Name="Hemrika.SharePresence.WebSite, Version=1.0.0.0, Culture=neutral, PublicKeyToken=11e6604a27f32a11" %>
<%@ Assembly Name="Microsoft.Web.CommandUI, Version=15.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %>
<%@ Register TagPrefix="SharePoint" Namespace="Microsoft.SharePoint.WebControls" Assembly="Microsoft.SharePoint, Version=15.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %>
<%@ Register TagPrefix="Utilities" Namespace="Microsoft.SharePoint.Utilities" Assembly="Microsoft.SharePoint, Version=15.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %>
<%@ Register TagPrefix="asp" Namespace="System.Web.UI" Assembly="System.Web.Extensions, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35" %>
<%@ Import Namespace="Microsoft.SharePoint" %>
<%@ Register TagPrefix="WebPartPages" Namespace="Microsoft.SharePoint.WebPartPages"
    Assembly="Microsoft.SharePoint, Version=15.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %>
<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="FormDataSourceEditor.ascx.cs"
    Inherits="Hemrika.SharePresence.WebSite.ControlTemplates.FormDataSourceEditor" %>
<div data-role="page" class="type-interior" data-theme="b">
    <div data-role="header" data-theme="b" data-position="fixed">
        <span class="ui-app-title">DataSource Editor</span>
    </div>
    <div data-role="content" data-theme="b">
        <h2>
            DataScource Properties</h2>
        <div data-role="fieldcontain">
            <label for="name" title="Name" class="required">
                Name</label>
            <asp:TextBox ID="tbx_name" name="name" runat="server"></asp:TextBox>
            <label for="webs" title="Webs" class="required">
                Webs</label>
            <asp:DropDownList ID="ddl_webs" name="webs" runat="server" OnSelectedIndexChanged="ddl_webs_SelectedIndexChanged"
                AutoPostBack="True">
            </asp:DropDownList>
            <label for="lists" title="Lists" class="required">
                Lists</label>
            <asp:DropDownList ID="ddl_lists" name="lists" runat="server" OnSelectedIndexChanged="ddl_lists_SelectedIndexChanged"
                AutoPostBack="True">
            </asp:DropDownList>
            <label for="views" title="Views" class="required">
                Views</label>
            <asp:DropDownList ID="ddl_views" name="views" runat="server" OnSelectedIndexChanged="ddl_views_SelectedIndexChanged"
                AutoPostBack="True">
            </asp:DropDownList>
            <label for="fields" title="Fields" class="required">
                Fields</label>
            <asp:CheckBoxList ID="cbl_fields" name="fields" runat="server" OnSelectedIndexChanged="cbl_fields_SelectedIndexChanged"
                RepeatLayout="Flow" RepeatDirection="Horizontal">
            </asp:CheckBoxList>
            <label for="caml" title="CAML" class="required">
                CAML</label>
            <asp:TextBox ID="tbx_caml" name="calm" runat="server" TextMode="MultiLine"></asp:TextBox><br />
        </div>
        <br />
        <asp:Button ID="Btn_Save" runat="server" OnClick="Btn_Save_Click" Text="Save" data-inline="true"
            CssClass="ui-btn-up-b" />
        <asp:Button ID="Btn_Cancel" runat="server" OnClick="Btn_Cancel_Click" CausesValidation="False"
            Text="Cancel" data-inline="true" CssClass="ui-btn-up-b" />
    </div>
</div>
