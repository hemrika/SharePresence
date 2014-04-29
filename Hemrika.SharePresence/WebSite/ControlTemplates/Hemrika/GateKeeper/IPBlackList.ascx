<%@ Assembly Name="Hemrika.SharePresence.WebSite, Version=1.0.0.0, Culture=neutral, PublicKeyToken=11e6604a27f32a11" %>
<%@ Assembly Name="Microsoft.Web.CommandUI, Version=15.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %>
<%@ Register Tagprefix="SharePoint" Namespace="Microsoft.SharePoint.WebControls" Assembly="Microsoft.SharePoint, Version=15.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %>
<%@ Register Tagprefix="Utilities" Namespace="Microsoft.SharePoint.Utilities" Assembly="Microsoft.SharePoint, Version=15.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %>
<%@ Register Tagprefix="asp" Namespace="System.Web.UI" Assembly="System.Web.Extensions, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35" %>
<%@ Import Namespace="Microsoft.SharePoint" %> 
<%@ Register Tagprefix="WebPartPages" Namespace="Microsoft.SharePoint.WebPartPages" Assembly="Microsoft.SharePoint, Version=15.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %>
<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="IPBlackList.ascx.cs" Inherits="Hemrika.SharePresence.WebSite.ControlTemplates.IPBlackList" %>
<div class="GateKeeper" id="IPBlackList">
    <div class="settings">
        <h1>IP Address Blacklist</h1>
        <table>
            <tr>
                <td>IP Address</td>
                <td>
                    <asp:TextBox ID="txtIPAddress" runat="server" MaxLength="50" ValidationGroup="ipaddress"/>
                    <asp:RegularExpressionValidator ID="rfvIPAddress"
                                    runat="server"
                                    ValidationGroup="ipaddress"
                                    ErrorMessage="* Enter a valid IP Address"
                                    ControlToValidate="txtIPAddress"
                                    Display="Dynamic" 
                                    ForeColor="Maroon"
                                    ValidationExpression="(2[0-4]\d|25[0-5]|[01]?\d\d?)\.(2[0-4]\d|25[0-5]|[01]?\d\d?)\.(2[0-4]\d|25[0-5]|[01]?\d\d?)\.(2[0-4]\d|25[0-5]|[01]?\d\d?)" />
                </td>
            </tr>
            <tr>
                <td>Comment</td>
                <td>
                    <asp:TextBox ID="txtComment" runat="server" MaxLength="150" />&nbsp;
                    <asp:Button ID="btnAdd" runat="server" Text="Add" Width="50px" ValidationGroup="ipaddress" />
                </td>
            </tr>
        </table>
        <br />
        <asp:GridView ID="gridIPAddress" 
                      runat="server"
                      BorderColor="Silver"
                      BorderStyle="Solid"
                      BorderWidth="1px"
                      AutoGenerateColumns="False"
                      HeaderStyle-HorizontalAlign="Left" 
                      HeaderStyle-BackColor="#666666" 
                      HeaderStyle-ForeColor="#ffffff" >
            <HeaderStyle HorizontalAlign="Left" BackColor="#666666" ForeColor="White"></HeaderStyle>
            <Columns>
                <asp:BoundField DataField="id" ItemStyle-Width="1px" ShowHeader="false" />
                <asp:BoundField HeaderText="Date Added" DataField="date" ItemStyle-Width="225px" />
                <asp:BoundField HeaderText="IP Address" DataField="ipaddress" ItemStyle-Width="150px" />
                <asp:BoundField HeaderText="Comments" DataField="comment" ItemStyle-Wrap="true" />
                <asp:TemplateField ShowHeader="False" ItemStyle-VerticalAlign="middle" ItemStyle-HorizontalAlign="Center" >
                    <ItemTemplate>
                	    <asp:Button ID="btnDelete" 
                                        runat="server" 
                                        Width="50px"
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