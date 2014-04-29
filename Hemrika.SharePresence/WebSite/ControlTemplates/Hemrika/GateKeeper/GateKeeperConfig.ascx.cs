using System;
using System.IO;
using System.Net.Mail;
using System.Text.RegularExpressions;
using System.Web.UI;
using System.Xml;
using Hemrika.SharePresence.WebSite.Modules.GateKeeper;

namespace Hemrika.SharePresence.WebSite.ControlTemplates
{
    public partial class GateKeeperConfig : UserControl
    {
        private GateKeeperSettings config = new GateKeeperSettings();
        //private Settings config = GateKeeperModule.config;

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                LoadSettings();
            }

            Page.MaintainScrollPositionOnPostBack = true;
            Page.Title = "GateKeeper Settings";

            btnSaveBottom.Click += new EventHandler(btnSaveBottom_Click);
            btnSaveTop.Click += new EventHandler(btnSaveTop_Click);
            //btnTestSmtp.Click += new EventHandler(btnTestSmtp_Click);

        }

        private void LoadSettings()
        {
            config = config.Load();
            // General Settings
            cbenablegatekeeper.Checked = config.EnableGateKeeper;
            cbdenyemptyuseragent.Checked = config.DenyEmptyUserAgent;
            txtaccessdeniedmessage.Text = config.AccessDeniedMessage;
            cbdisplaycontactform.Checked = config.DisplayContactForm;
            txtcontactformurl.Text = config.ContactFormUrl;


            // Hotlink Settings
            cbblockhotlinking.Checked = config.BlockHotLinking;
            cbdisplayhotlinkdenyimage.Checked = config.DisplayHotLinkDenyImage;
            txthotlinkallowedsites.Text = config.HotLinkAllowedSites;
            txthotlinkexpression.Text = config.HotLinkExpression;
            txthotlinkdenyimage.Text = config.HotLinkDenyImage;

            // HoneyPot Settings
            cbenablehoneypot.Checked = config.EnableHoneyPot;
            txthoneypotpath.Text = config.HoneyPotPath;
            cbpermanantlydenyhoneypotviolator.Checked = config.PersistHoneyPotDeny;
            cbnotifyadmin.Checked = config.NotifyAdmin;
            cbenablehoneypotlogging.Checked = config.EnableHoneyPotLogging;
            cbenablehoneypotstats.Checked = config.EnableHoneyPotStats;
            txthoneypotstatspath.Text = config.HoneyPotStatsPath;

            // HttpBL Settings
            cbenablehttpbl.Checked = config.EnableHttpBL;
            cbdenyhttpblsuspect.Checked = config.DenyHttpBLSuspect;
            cbenablehttpbllogging.Checked = config.EnableHttpBLLogging;
            cbhttpblpostonly.Checked = config.HttpBLPostOnly;
            txthttpblkeycode.Text = config.HttpBLKeyCode;
            txthttpbltimeout.Text = config.HttpBLTimeout.ToString();
            txtthreatscorethreshold.Text = config.ThreatScoreThreshold.ToString();

            // Email Settings
            txtadminemailaddress.Text = config.SmtpEmailAddress;
            txtsmtpservername.Text = config.SmtpServerName;
            txtsmtpportnumber.Text = config.SmtpServerPort.ToString();
            txtsmtpusername.Text = config.SmtpUserName;
            cbstorepasswordencrypted.Checked = config.StorePasswordEncrypted;
            txtsmtppassword.Text = config.StorePasswordEncrypted ? Encryption.Decrypt(config.SmtpPassword) : config.SmtpPassword;
            cbsmtpenablessl.Checked = config.SmtpEnableSSL;
            txtsmtpmessagesubject.Text = config.SmtpMessageSubject;
            txtsmtpmessagebody.Text = config.SmtpMessageBody;

        }

        private void UpdateSettings()
        {
            config = config.Load();

            config.EnableGateKeeper = cbenablegatekeeper.Checked;
            config.DisplayContactForm = cbdisplaycontactform.Checked;
            config.ContactFormUrl = txtcontactformurl.Text;
            config.DenyEmptyUserAgent = cbdenyemptyuseragent.Checked;
            config.BlockHotLinking = cbblockhotlinking.Checked;
            config.HotLinkExpression = txthotlinkexpression.Text;
            config.DisplayHotLinkDenyImage = cbdisplayhotlinkdenyimage.Checked;
            config.HotLinkDenyImage = txthotlinkdenyimage.Text;
            config.HotLinkAllowedSites = txthotlinkallowedsites.Text;
            config.EnableHoneyPot = cbenablehoneypot.Checked;
            config.HoneyPotPath = txthoneypotpath.Text;
            config.EnableHoneyPotLogging = cbenablehoneypotlogging.Checked;
            config.PersistHoneyPotDeny = cbpermanantlydenyhoneypotviolator.Checked;
            config.EnableHoneyPotStats = cbenablehoneypotstats.Checked;
            config.HoneyPotStatsPath = txthoneypotstatspath.Text;
            config.NotifyAdmin = cbnotifyadmin.Checked;
            config.SmtpEmailAddress = txtadminemailaddress.Text;
            config.SmtpMessageSubject = txtsmtpmessagesubject.Text;
            config.SmtpMessageBody = txtsmtpmessagebody.Text;
            config.SmtpServerName = txtsmtpservername.Text;
            config.SmtpServerPort = int.Parse(txtsmtpportnumber.Text);
            config.SmtpEnableSSL = cbsmtpenablessl.Checked;
            config.SmtpUserName = txtsmtpusername.Text;
            config.StorePasswordEncrypted = cbstorepasswordencrypted.Checked;
            config.SmtpPassword = cbstorepasswordencrypted.Checked ? Encryption.Encrypt(txtsmtppassword.Text) : txtsmtppassword.Text;
            config.AccessDeniedMessage = txtaccessdeniedmessage.Text;

            config.EnableHttpBL = cbenablehttpbl.Checked;
            config.DenyHttpBLSuspect = cbdenyhttpblsuspect.Checked;
            config.EnableHttpBLLogging = cbenablehttpbllogging.Checked;
            config.HttpBLPostOnly = cbhttpblpostonly.Checked;
            config.HttpBLKeyCode = txthttpblkeycode.Text;
            config.HttpBLTimeout = int.Parse(txthttpbltimeout.Text);
            config.ThreatScoreThreshold = int.Parse(txtthreatscorethreshold.Text);

            config.Save();
            Response.Redirect(Request.RawUrl, true);
        }

        private void btnTestSmtp_Click(object sender, EventArgs e)
        {
            try
            {
                MailMessage mail = new MailMessage();
                mail.From = new MailAddress(txtadminemailaddress.Text);
                mail.To.Add(mail.From);
                mail.Subject = "Test mail from GateKeeper";
                mail.Body = "If you are seeing this email than you have successfully configured the mail settings for GateKeeper";
                SmtpClient smtp = new SmtpClient(txtsmtpservername.Text);
                smtp.Credentials = new System.Net.NetworkCredential(txtsmtpusername.Text, txtsmtppassword.Text);
                smtp.EnableSsl = cbsmtpenablessl.Checked;
                smtp.Port = int.Parse(txtsmtpportnumber.Text);
                smtp.Send(mail);
                lbSmtpStatus.Text = "Test successfull";
                lbSmtpStatus.Style.Add(HtmlTextWriterStyle.Color, "green");
            }
            catch
            {
                lbSmtpStatus.Text = "Could not connect";
                lbSmtpStatus.Style.Add(HtmlTextWriterStyle.Color, "red");
            }
        }

        protected void btnSaveBottom_Click(object sender, EventArgs e)
        { UpdateSettings(); }

        protected void btnSaveTop_Click(object sender, EventArgs e)
        { UpdateSettings(); }
    }
}