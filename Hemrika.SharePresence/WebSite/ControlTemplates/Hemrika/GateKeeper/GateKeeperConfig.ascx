<%@ Assembly Name="Hemrika.SharePresence.WebSite, Version=1.0.0.0, Culture=neutral, PublicKeyToken=11e6604a27f32a11" %>
<%@ Assembly Name="Microsoft.Web.CommandUI, Version=15.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %>
<%@ Register Tagprefix="SharePoint" Namespace="Microsoft.SharePoint.WebControls" Assembly="Microsoft.SharePoint, Version=15.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %>
<%@ Register Tagprefix="Utilities" Namespace="Microsoft.SharePoint.Utilities" Assembly="Microsoft.SharePoint, Version=15.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %>
<%@ Register Tagprefix="asp" Namespace="System.Web.UI" Assembly="System.Web.Extensions, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35" %>
<%@ Import Namespace="Microsoft.SharePoint" %> 
<%@ Register Tagprefix="WebPartPages" Namespace="Microsoft.SharePoint.WebPartPages" Assembly="Microsoft.SharePoint, Version=15.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %>
<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="GateKeeperConfig.ascx.cs" Inherits="Hemrika.SharePresence.WebSite.ControlTemplates.GateKeeperConfig" %>
    <div data-role="page active" class="type-interior" data-theme="b">
		<div data-role="header" data-theme="b" data-position="fixed">
            <span class="ui-app-title">GateKeeper Settings</span>
	    </div>
	    <div data-role="content" data-theme="b">
        <div class="savebutton">
            <asp:Button runat="server" ID="btnSaveTop" Text="Save Settings" onclick="btnSaveTop_Click" data-inline="true" CssClass="ui-btn-up-b"/>
        </div>
        <fieldset data-role="controlgroup">
        <legend><h1>General</h1></legend>
            <asp:CheckBox runat="server" ID="cbenablegatekeeper" Text="Enable GateKeeper" TextAlign="Right"/>
            <asp:CheckBox runat="server" ID="cbdenyemptyuseragent" Text="Deny Empty UserAgent (not recommended)" TextAlign="Right"/>
            <label for="txtaccessdeniedmessage">Access denied message</label>
            <asp:TextBox runat="server" ID="txtaccessdeniedmessage" TextMode="MultiLine" MaxLength="500" />
            <asp:RequiredFieldValidator ID="RequiredFieldValidator3" runat="server" ControlToValidate="txtaccessdeniedmessage" ErrorMessage="Required" Display="Dynamic" />
            <asp:CheckBox runat="server" ID="cbdisplaycontactform" Text="Display Contact Form" TextAlign="Right"/>
            <label for="txtcontactformurl">Contact Page URL Path</label>
            <asp:TextBox runat="server" ID="txtcontactformurl" MaxLength="300" />
        </fieldset>
        <br />
        <fieldset data-role="controlgroup">
            <legend><h1>Hotlinking</h1></legend>
            <asp:CheckBox runat="server" ID="cbblockhotlinking" Text="Enable Hotlink Blocking" TextAlign="Right" />
            <label for="txthotlinkallowedsites">Allowed sites</label>
            <asp:TextBox runat="server" ID="txthotlinkallowedsites" />
            <label for="txthotlinkexpression">Matching Pattern</label>
            <asp:TextBox runat="server" ID="txthotlinkexpression" />
            <asp:CheckBox runat="server" ID="cbdisplayhotlinkdenyimage" Text="Display Deny Image" TextAlign="Right" />
            <label for="txthotlinkdenyimage">Deny Image Path</label>
            <asp:TextBox runat="server" ID="txthotlinkdenyimage" />
        </fieldset>
        <br />
        <fieldset data-role="controlgroup">
            <legend><h1>HoneyPot</h1></legend>
            <asp:CheckBox runat="server" ID="cbenablehoneypot" Text="Enable HoneyPot" TextAlign="Right" />
            <label for="txthoneypotpath">HoneyPot URL Path</label>
            <asp:TextBox runat="server" ID="txthoneypotpath" />
            <asp:CheckBox runat="server" ID="cbpermanantlydenyhoneypotviolator" Text="Deny violators" TextAlign="Right" />
            <asp:CheckBox runat="server" ID="cbenablehoneypotlogging" Text="Enable Logging" TextAlign="Right" />
            <asp:CheckBox runat="server" ID="cbenablehoneypotstats" Text="Enable Stats" TextAlign="Right" />
            <label for="txthoneypotstatspath">Stats URL Path</label>
            <asp:TextBox runat="server" ID="txthoneypotstatspath" />
        </fieldset>
        <br />
        <fieldset data-role="controlgroup">
            <legend><h1>HTTP BlackList</h1></legend>
            <asp:CheckBox runat="server" ID="cbenablehttpbl" Text="Enable HTTP Blacklist" TextAlign="Right" />
            <asp:CheckBox runat="server" ID="cbdenyhttpblsuspect" Text="Deny Suspects" TextAlign="Right" />
            <asp:CheckBox runat="server" ID="cbhttpblpostonly" Text="Check POST only" TextAlign="Right" />
            <asp:CheckBox runat="server" ID="cbenablehttpbllogging" Text="Enable Logging" TextAlign="Right" />
            <label for="txthttpblkeycode">Project HoneyPot KeyCode</label>
            <asp:TextBox runat="server" ID="txthttpblkeycode" />
            <label for="txthttpbltimeout">DNS Lookup Timeout in ms</label>
            <asp:TextBox runat="server" ID="txthttpbltimeout" />
            <asp:RangeValidator ID="RangeValidator4" runat="server" ErrorMessage="Number between 1000-5000" Type="Integer" MaximumValue="5000" MinimumValue="1000" ControlToValidate="txthttpbltimeout" Display="Dynamic" />
            <label for="txtthreatscorethreshold">Threat Score Threshold</label>
            <asp:TextBox runat="server" ID="txtthreatscorethreshold" />
            <asp:RangeValidator ID="RangeValidator2" runat="server" ErrorMessage="Number between 1-254" Type="Integer" MaximumValue="254" MinimumValue="1" ControlToValidate="txtthreatscorethreshold" Display="Dynamic" />

        </fieldset>
        <div class="savebutton">
            <asp:Button runat="server" ID="btnSaveBottom" Text="Save Settings" onclick="btnSaveBottom_Click" data-inline="true" CssClass="ui-btn-up-b" />
        </div>
        </div>
    </div>

    <div class="settings" style="display:none;">
        <h1>E-Mail Settings</h1>
        <div class="note">
            <h2>EMAIL NOTE:</h2>
            <p>The Email Body field is an optional field that will be appended to the default email body.</p>
            <p>The password can be encrypted to prevent casual viewing.&nbsp; However, if you 
                choose encryption be aware the password can only be read by this server.</p>
        </div>
        <table>
            <tr>
                <td class="label"><label for="cbnotifyadmin">Notify Admin</label></td>
                <td class="input"><asp:CheckBox runat="server" ID="cbnotifyadmin" />&nbsp;Send an 
                    email for HoneyPot violations</td>
            </tr>
            <tr>
                <td class="label"><label for="txtadminemailaddress">Email Address</label></td>
                <td class="input">
                    <asp:TextBox runat="server" ID="txtadminemailaddress" Width="300" />
                    <asp:RegularExpressionValidator ID="RegularExpressionValidator1" runat="server" 
                        ErrorMessage="Invalid format" ControlToValidate="txtadminemailaddress" 
                        Display="Dynamic" SetFocusOnError="True" 
                        ValidationExpression="\w+([-+.']\w+)*@\w+([-.]\w+)*\.\w+([-.]\w+)*" />
                </td>
            </tr>
            <tr>
                <td class="label"><label for="txtsmtpservername">SMTP Server</label></td>
                <td class="input"><asp:TextBox runat="server" ID="txtsmtpservername" Width="300" /></td>
            </tr>
            <tr>
                <td class="label" style="height: 29px"><label for="txtsmtpportnumber">Port Number</label></td>
                <td class="style1">
                    <asp:TextBox runat="server" ID="txtsmtpportnumber" Width="30px" />
                    <asp:RangeValidator ID="RangeValidator1" runat="server" 
                        ErrorMessage="Invalid format" ControlToValidate="txtsmtpportnumber" 
                        Display="Dynamic" MaximumValue="65000" MinimumValue="1" SetFocusOnError="True" 
                        Type="Integer"></asp:RangeValidator>&nbsp;Port 25 is the standard
                </td>
            </tr>
            <tr>
                <td class="label"><label for="txtsmtpusername">User Name</label></td>
                <td class="input"><asp:TextBox runat="server" ID="txtsmtpusername" Width="300" /></td>
            </tr>
            <tr>
                <td class="label"><label for="txtsmtppassword">Password</label></td>
                <td class="input"><asp:TextBox runat="server" ID="txtsmtppassword" Width="300" /></td>
            </tr>
            <tr>
                <td class="label"><label for="cbstorepasswordencrypted">Store Password Encrypted</label></td>
                <td class="input"><asp:CheckBox runat="server" ID="cbstorepasswordencrypted" /></td>
            </tr>
            <tr>
                <td class="label"><label for="cbsmtpenablessl">Enable SSL</label></td>
                <td class="input"><asp:CheckBox runat="server" ID="cbsmtpenablessl" /></td>
            </tr>
            
            <tr>
                <td class="label"><label for="txtsmtpmessagesubject">Email Subject</label></td>
                <td class="input"><asp:TextBox runat="server" ID="txtsmtpmessagesubject" Width="300" /> </td>
            </tr>
            <tr>
                <td class="label" valign="top"><label for="txtsmtpmessagebody">Appended </label>Email Content</td>
                <td class="input"><asp:TextBox runat="server" ID="txtsmtpmessagebody" Width="500px" MaxLength="500" Height="75px" /></td>
            </tr>
            <tr>
                <td class="label"></td>
                <td>
                    <asp:Button runat="server" CausesValidation="False" ID="btnTestSmtp" Text="Test mail settings" />
                    &nbsp;<asp:Label runat="Server" ID="lbSmtpStatus" />
                </td>
            </tr>
        </table>
    </div>
