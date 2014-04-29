<%@ Assembly Name="Hemrika.SharePresence.WebSite, Version=1.0.0.0, Culture=neutral, PublicKeyToken=3421bd1d946bda6c" %>
<%@ Assembly Name="Microsoft.Web.CommandUI, Version=15.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %>
<%@ Register TagPrefix="SharePoint" Namespace="Microsoft.SharePoint.WebControls"
    Assembly="Microsoft.SharePoint, Version=15.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %>
<%@ Register TagPrefix="Utilities" Namespace="Microsoft.SharePoint.Utilities" Assembly="Microsoft.SharePoint, Version=15.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %>
<%@ Register TagPrefix="asp" Namespace="System.Web.UI" Assembly="System.Web.Extensions, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35" %>
<%@ Import Namespace="Microsoft.SharePoint" %>
<%@ Register TagPrefix="WebPartPages" Namespace="Microsoft.SharePoint.WebPartPages"
    Assembly="Microsoft.SharePoint, Version=15.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %>
<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="UploadImage.ascx.cs"
    Inherits="Hemrika.SharePresence.WebSite.ControlTemplates.UploadImage" %>
<div data-role="page active" class="type-interior" data-theme="b">
    <div data-role="header" data-theme="b" data-position="fixed">
        <span class="ui-app-title">Create a Webpage</span>
    </div>
    <div data-role="content" data-theme="b">
        <fieldset id="Properties">
            <legend>Image Properties</legend>
            <ol>
                <li>
                    <label for="title" title="Enter your title" class="required">
                        File</label>
                    <asp:FileUpload ID="fup_file" runat="server" />
                </li>
                <li>
                    <label for="library" title="Select the library" class="required">
                        library</label>
                    <asp:DropDownList ID="ddl_library" name="library" runat="server" AutoPostBack="true"
                        OnSelectedIndexChanged="ddl_library_SelectedIndexChanged">
                    </asp:DropDownList>
                </li>
                <li>
                    <label for="content" title="Select the ContentType" class="required">
                        ContentType</label>
                    <asp:DropDownList ID="ddl_contenttype" name="contenttype" runat="server" AutoPostBack="false">
                    </asp:DropDownList>
                </li>
                <li>
                    <label for="title" title="Enter your title" class="required">
                        Title</label>
                    <asp:TextBox ID="tbx_title" name="title" runat="server"></asp:TextBox>*
                    <asp:RequiredFieldValidator ID="rfv_title" runat="server" ErrorMessage="Required"
                        ControlToValidate="tbx_title"></asp:RequiredFieldValidator>
                </li>
                <li>
                    <label for="keuwords" title="Enter your keywords" class="required">
                        Keywords ( , )</label>
                    <asp:TextBox ID="tbx_keywords" name="keywords" TextMode="SingleLine" runat="server" />
                </li>
            </ol>
        <asp:Button ID="Btn_Save" runat="server" OnClick="Btn_Save_Click" Text="Save" data-inline="true"
            CssClass="ui-btn-up-b" />
        <asp:Button ID="Btn_Cancel" runat="server" OnClick="Btn_Cancel_Click" CausesValidation="False"
            Text="Cancel" data-inline="true" CssClass="ui-btn-up-b" />
        </fieldset>
    </div>
</div>
