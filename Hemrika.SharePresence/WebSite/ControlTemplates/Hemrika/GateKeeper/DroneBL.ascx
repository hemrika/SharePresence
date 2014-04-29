<%@ Assembly Name="Hemrika.SharePresence.WebSite, Version=1.0.0.0, Culture=neutral, PublicKeyToken=3421bd1d946bda6c" %>
<%@ Assembly Name="Microsoft.Web.CommandUI, Version=15.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %>
<%@ Register Tagprefix="SharePoint" Namespace="Microsoft.SharePoint.WebControls" Assembly="Microsoft.SharePoint, Version=15.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %>
<%@ Register Tagprefix="Utilities" Namespace="Microsoft.SharePoint.Utilities" Assembly="Microsoft.SharePoint, Version=15.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %>
<%@ Register Tagprefix="asp" Namespace="System.Web.UI" Assembly="System.Web.Extensions, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35" %>
<%@ Import Namespace="Microsoft.SharePoint" %> 
<%@ Register Tagprefix="WebPartPages" Namespace="Microsoft.SharePoint.WebPartPages" Assembly="Microsoft.SharePoint, Version=15.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %>
<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="DroneBL.ascx.cs" Inherits="Hemrika.SharePresence.WebSite.ControlTemplates.DroneBL" %>

<div class="GateKeeper" id="DroneBL">
    <div class="settings">
        <h1>Drone BlackList History</h1>
        <asp:GridView ID="gridDroneBL" 
                      runat="server"
                      BorderColor="Silver"
                      BorderStyle="Solid"
                      BorderWidth="1px"
                      AutoGenerateColumns="False"
                      HeaderStyle-HorizontalAlign="Left" 
                      HeaderStyle-BackColor="#666666" 
                      HeaderStyle-ForeColor="#ffffff"
                      ItemStyle-Wrap="false" >
            <HeaderStyle HorizontalAlign="Left" BackColor="#666666" ForeColor="White"></HeaderStyle>
            <Columns>
                <asp:BoundField DataField="id" ItemStyle-Width="1px" ShowHeader="false" />
                <asp:BoundField HeaderText="Entry Date" DataField="date" ItemStyle-Width="150px" />
                <asp:BoundField HeaderText="IP Address" DataField="ipaddress" ItemStyle-Width="100px" />
                <asp:BoundField HeaderText="UserAgent" DataField="useragent" ItemStyle-Wrap="true" />
                <asp:TemplateField ShowHeader="False" ItemStyle-VerticalAlign="middle">
                    <ItemTemplate>
                	    <asp:Button ID="btnDelete"
                	                    Width="50px"
                                        runat="server" 
                                        OnClick="btnDelete_Click"
                                        CausesValidation="false" 
                                        CommandName="btnDelete" 
                                        Text="Delete" />
                    </ItemTemplate>
                </asp:TemplateField>
            </Columns>
        </asp:GridView>
    </div>
</div>