// -----------------------------------------------------------------------
// <copyright file="GateKeeperSettings.cs" company="">
// TODO: Update copyright text.
// </copyright>
// -----------------------------------------------------------------------

namespace Hemrika.SharePresence.WebSite.Modules.GateKeeper
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using Microsoft.SharePoint;
    using System.Web.Script.Serialization;
    using System.Web;
    using System.Runtime.Serialization;
    using Hemrika.SharePresence.Common.WebSiteController;

    /// <summary>
    /// TODO: Update summary.
    /// </summary>
    [Serializable]
    public class GateKeeperSettings
    {

        #region Private Declatations
        #region General
        private Boolean enablegatekeeper = true;
        private string accessdeniedmessage = string.Empty;
        private Boolean denyemptyuseragent = false;
        private Boolean displaycontactform = true;
        private string contactformurl = string.Empty;
        #endregion // General

        #region hotlinking
        private Boolean blockhotlinking = true;
        private string hotlinkexpression = string.Empty;
        private Boolean displayhotlinkdenyimage = false;
        private string hotlinkdenyimage = string.Empty;
        private string hotlinkallowedsites = string.Empty;
        #endregion // hotlinking

        #region honeypot
        private Boolean enablehoneypot = false;
        private string honeypotpath = string.Empty; // Virtual url path for the honeypot ex. honey/gottcha.aspx
        private Boolean enablehoneypotlogging = true; // Record each violation into the honeypot log
        private Boolean persisthoneypotdeny = false; // insert violators into IPAddress blacklist
        private Boolean enablehoneypotstats = false; // Listen for stats page request
        private string honeypotstatspath = string.Empty; // url to dynmically display stats ex. gatekeeper/stats.aspx
        private Boolean notifyadmin = false; // Send an email to the admin when the trap has been sprung
        #endregion // honeypot

        #region HttpBL
        private Boolean enablehttpbl = false;
        private Boolean enablehttpbllogging = false;
        private Boolean denyhttpblsuspect = false;
        private Boolean httpblpostonly = false;
        private string httpblkeycode = string.Empty;
        private string httpbltestipaddress = string.Empty;
        private int httpbltimeout = 3000;
        private int threatscorethreshold = 254;
        #endregion // HttpBL

        #region ProxyBL
        private Boolean enableproxybl = false;
        private Boolean enableproxybllogging = false;
        private Boolean denyproxyblsuspect = false;
        private Boolean proxyblpostonly = false;
        private string proxybltestipaddress = string.Empty;
        private int proxybltimeout = 3000;
        #endregion // ProxyBL

        #region DroneBL
        private Boolean enabledronebl = false;
        private Boolean enabledronebllogging = false;
        private Boolean denydroneblsuspect = false;
        private Boolean droneblpostonly = false;
        private string dronebltestipaddress = string.Empty;
        private int dronebltimeout = 3000;
        #endregion // DroneBL

        #region mail
        private string smtpemailaddress = string.Empty;
        private string smtpmessagesubject = string.Empty;
        private string smtpmessagebody = string.Empty;
        private string smtpservername = string.Empty;
        private string smtpusername = string.Empty;
        private string smtppassword = string.Empty;
        private Boolean storepasswordencrypted = false;
        private int smtpserverport = 25;
        private string virtualpath = string.Empty;
        private Boolean smtpenablessl = false;
        #endregion //mail

        #endregion // Private Declatations

        #region Public Declarations
        #region General
        public bool EnableGateKeeper
        {
            get { return enablegatekeeper; }
            set { enablegatekeeper = value; }
        }
        public string AccessDeniedMessage
        {
            get { return accessdeniedmessage; }
            set { accessdeniedmessage = value ?? string.Empty; }
        }
        public bool DenyEmptyUserAgent
        {
            get { return denyemptyuseragent; }
            set { denyemptyuseragent = value; }
        }
        public bool DisplayContactForm
        {
            get { return displaycontactform; }
            set { displaycontactform = value; }
        }
        public string ContactFormUrl
        {
            get { return contactformurl; }
            set { contactformurl = value ?? string.Empty; }
        }
        #endregion //General

        #region Hotlinking
        public bool BlockHotLinking
        {
            get { return blockhotlinking; }
            set { blockhotlinking = value; }
        }
        public bool DisplayHotLinkDenyImage
        {
            get { return displayhotlinkdenyimage; }
            set { displayhotlinkdenyimage = value; }
        }
        public string HotLinkExpression
        {
            get { return hotlinkexpression; }
            set { hotlinkexpression = value ?? string.Empty; }
        }
        public string HotLinkDenyImage
        {
            get { return hotlinkdenyimage; }
            set { hotlinkdenyimage = value ?? string.Empty; }
        }
        public string HotLinkAllowedSites
        {
            get { return hotlinkallowedsites; }
            set { hotlinkallowedsites = value ?? string.Empty; }
        }
        #endregion // Hotlinking

        #region Honeypot
        public bool EnableHoneyPot
        {
            get { return enablehoneypot; }
            set { enablehoneypot = value; }
        }
        public bool EnableHoneyPotLogging
        {
            get { return enablehoneypotlogging; }
            set { enablehoneypotlogging = value; }
        }
        public bool PersistHoneyPotDeny
        {
            get { return persisthoneypotdeny; }
            set { persisthoneypotdeny = value; }
        }
        public bool EnableHoneyPotStats
        {
            get { return enablehoneypotstats; }
            set { enablehoneypotstats = value; }
        }
        public bool NotifyAdmin
        {
            get { return notifyadmin; }
            set { notifyadmin = value; }
        }
        public string HoneyPotPath
        {
            get { return honeypotpath; }
            set { honeypotpath = value ?? string.Empty; }
        }
        public string HoneyPotStatsPath
        {
            get { return honeypotstatspath; }
            set { honeypotstatspath = value ?? string.Empty; }
        }

        #endregion // Honeypot

        #region HttpBL
        public Boolean EnableHttpBL
        {
            get { return enablehttpbl; }
            set { enablehttpbl = value; }
        }
        public Boolean EnableHttpBLLogging
        {
            get { return enablehttpbllogging; }
            set { enablehttpbllogging = value; }
        }
        public Boolean DenyHttpBLSuspect
        {
            get { return denyhttpblsuspect; }
            set { denyhttpblsuspect = value; }
        }
        public Boolean HttpBLPostOnly
        {
            get { return httpblpostonly; }
            set { httpblpostonly = value; }
        }
        public string HttpBLKeyCode
        {
            get { return httpblkeycode; }
            set { httpblkeycode = value; }
        }
        public string HttpBLTestIPAddress
        {
            get { return httpbltestipaddress; }
            set { httpbltestipaddress = value; }
        }
        public int HttpBLTimeout
        {
            get { return httpbltimeout; }
            set { httpbltimeout = value; }
        }
        public int ThreatScoreThreshold
        {
            get { return threatscorethreshold; }
            set { threatscorethreshold = value; }
        }
        #endregion // HttpBL

        #region ProxyBL
        public Boolean EnableProxyBL
        {
            get { return enableproxybl; }
            set { enableproxybl = value; }
        }
        public Boolean EnableProxyBLLogging
        {
            get { return enableproxybllogging; }
            set { enableproxybllogging = value; }
        }
        public Boolean DenyProxyBLSuspect
        {
            get { return denyproxyblsuspect; }
            set { denyproxyblsuspect = value; }
        }
        public Boolean ProxyBLPostOnly
        {
            get { return proxyblpostonly; }
            set { proxyblpostonly = value; }
        }
        public string ProxyBLTestIPAddress
        {
            get { return proxybltestipaddress; }
            set { proxybltestipaddress = value; }
        }
        public int ProxyBLTimeout
        {
            get { return proxybltimeout; }
            set { proxybltimeout = value; }
        }
        #endregion // ProxyBL

        #region DroneBL
        public Boolean EnableDroneBL
        {
            get { return enabledronebl; }
            set { enabledronebl = value; }
        }
        public Boolean EnableDroneBLLogging
        {
            get { return enabledronebllogging; }
            set { enabledronebllogging = value; }
        }
        public Boolean DenyDroneBLSuspect
        {
            get { return denydroneblsuspect; }
            set { denydroneblsuspect = value; }
        }
        public Boolean DroneBLPostOnly
        {
            get { return droneblpostonly; }
            set { droneblpostonly = value; }
        }
        public string DroneBLTestIPAddress
        {
            get { return dronebltestipaddress; }
            set { dronebltestipaddress = value; }
        }
        public int DroneBLTimeout
        {
            get { return dronebltimeout; }
            set { dronebltimeout = value; }
        }
        #endregion // DroneBL

        #region Mail
        public string SmtpEmailAddress
        {
            get { return smtpemailaddress; }
            set { smtpemailaddress = value ?? string.Empty; }
        }
        public string SmtpMessageSubject
        {
            get { return smtpmessagesubject; }
            set { smtpmessagesubject = value ?? string.Empty; }
        }
        public string SmtpMessageBody
        {
            get { return smtpmessagebody; }
            set { smtpmessagebody = value ?? string.Empty; }
        }
        public string SmtpServerName
        {
            get { return smtpservername; }
            set { smtpservername = value ?? string.Empty; }
        }
        public string SmtpUserName
        {
            get { return smtpusername; }
            set { smtpusername = value ?? string.Empty; }
        }
        public string SmtpPassword
        {
            get { return smtppassword; }
            set { smtppassword = value ?? string.Empty; }
        }
        public bool StorePasswordEncrypted
        {
            get { return storepasswordencrypted; }
            set { storepasswordencrypted = value; }
        }
        public bool SmtpEnableSSL
        {
            get { return smtpenablessl; }
            set { smtpenablessl = value; }
        }
        public int SmtpServerPort
        {
            get { return smtpserverport; }
            set { smtpserverport = value; }
        }

        public string Virtualpath
        {
            get { return virtualpath; }
            set { virtualpath = value; }
        }
        #endregion // Mail

        #endregion // Public Declarations

        [NonSerialized]
        internal Guid _guid = Guid.Empty;

        public void Save()
        {
            CreateWorkItem(SPContext.Current.Site.RootWeb);
        }

        private void CreateWorkItem(SPWeb web)//, HttpStatusCode code)
        {
            Guid siteId = web.Site.ID;
            Guid webId = web.ID;

            bool disabled = false;
            WebSiteControllerPrincipalType principalType = WebSiteControllerPrincipalType.None;
            bool appliesToSSL = true;
            int sequence = 1;
            String pricipal = string.Empty;

            StringBuilder builder = new StringBuilder();
            builder.Append("/;");
            builder.Append(disabled.ToString() + ";");
            builder.Append(appliesToSSL.ToString() + ";");
            builder.Append(sequence.ToString() + ";");
            builder.Append(principalType.ToString() + ";");
            builder.Append(pricipal + ";");
            builder.Append("#");

            string value = new JavaScriptSerializer().Serialize(this);

            builder.Append(String.Format("{0}:{1};", "GateKeeper",Encryption.Encrypt(value)));
            //builder.Append(value);

            string full = builder.ToString();

            GateKeeperModule mod = new GateKeeperModule();
            IWebSiteControllerModule imod = null;  //WebSiteControllerConfig.GetModule(web.Site.WebApplication, mod.RuleType);

            while (imod == null)
            {
                
                try
                {
                    imod = WebSiteControllerConfig.GetModule(web.Site.WebApplication, mod.RuleType);
                }
                catch (Exception ex)
                {
                    ex.ToString();
                }
                System.Threading.Thread.Sleep(1000);
            }

            Guid itemGuid = _guid;
            int item = 0;

            if (itemGuid.Equals(Guid.Empty))
            {
                itemGuid = mod.Id;
                item = -1;
            }
            else
            {
                item = 2;
            }

            SPSecurity.RunWithElevatedPrivileges(() =>
            {
                using (SPSite site = new SPSite(siteId))
                {
                    site.AddWorkItem(
                        Guid.NewGuid(),
                        DateTime.Now.ToUniversalTime(),
                        WebSiteControllerRuleWorkItem.WorkItemTypeId,
                        webId,
                        siteId,
                        item,
                        true,
                        itemGuid,
                        Guid.Empty,
                        site.SystemAccount.ID,
                        null,
                        builder.ToString(),
                        Guid.Empty
                        );
                }
            });
        }

        public GateKeeperSettings Load()
        {
            GateKeeperSettings config = null;
            GateKeeperModule module = new GateKeeperModule();

            HttpContext current = HttpContext.Current;
            HttpRequest request = current.Request;

            string currUrl = current.Request.Url.ToString();
            string basePath = currUrl.Substring(0, currUrl.IndexOf(current.Request.Url.Host) + current.Request.Url.Host.Length);
            Uri url = new Uri(basePath);

            List<WebSiteControllerRule> rules = WebSiteControllerConfig.GetRulesForSiteCollection(url, module.RuleType);

            foreach (WebSiteControllerRule rule in rules)
            {
               //string props = rule.Properties.ToString();

                if (rule.Properties.ContainsKey("GateKeeper"))
                {
                    if (config == null)
                    {
                        try
                        {
                            string gateconfig = rule.Properties["GateKeeper"].ToString();
                            config = new JavaScriptSerializer().Deserialize<GateKeeperSettings>(Encryption.Decrypt(gateconfig));
                            config._guid = rule.Id;
                            break;

                        }
                        catch (Exception ex)
                        {
                            ex.ToString();
                            //throw;
                        }
                    }

                }
            }
            if (config != null)
            {
                return config;
            }
            else
            {
                return new GateKeeperSettings();
            }
        }

        /*
        public void Remove(SPSite site)
        {
            SPWeb rootWeb = site.RootWeb;
            rootWeb.AllowUnsafeUpdates = true;
            if (rootWeb.AllProperties.ContainsKey("SharePresenceGateKeeperSettings"))
            {
                rootWeb.AllProperties.Remove("SharePresenceGateKeeperSettings");
            }
            rootWeb.Update();
            rootWeb.AllowUnsafeUpdates = false;
        }
        */

        /*
        public GateKeeperSettings Save()
        {
            string value = new JavaScriptSerializer().Serialize(this);
            SPWeb rootWeb = SPContext.Current.Site.RootWeb;
            rootWeb.AllowUnsafeUpdates = true;

            if (!rootWeb.AllProperties.ContainsKey("SharePresenceGateKeeperSettings"))
            {
                rootWeb.AddProperty("SharePresenceGateKeeperSettings", value);
            }
            else
            {
                rootWeb.AllProperties["SharePresenceGateKeeperSettings"] = value;
            }
            
            rootWeb.Update();
            rootWeb.AllowUnsafeUpdates = false;
            return this;
        }
        */

        /*
        public GateKeeperSettings Load()
        {
            if (SPContext.Current != null)
            {
                return LoadSettings(SPContext.Current.Site.RootWeb);
            }
            else
            {
                HttpContext context = HttpContext.Current;
                using (SPSite site = new SPSite(context.Request.Url.ToString()))
                {
                    return LoadSettings(site.RootWeb);
                }
            }
        }

        internal GateKeeperSettings LoadSettings(SPWeb rootWeb)
        {
            if (rootWeb.AllProperties.ContainsKey("SharePresenceGateKeeperSettings"))
            {
                string value = rootWeb.AllProperties["SharePresenceGateKeeperSettings"] as string;
                if (!string.IsNullOrEmpty(value))
                {
                    try
                    {
                        GateKeeperSettings settings = new JavaScriptSerializer().Deserialize<GateKeeperSettings>(value);
                        if (settings != null)
                        {
                            return settings;
                        }
                    }
                    catch
                    {
                        return new GateKeeperSettings();
                    }
                }
            }
            else
            {
                enablehoneypotlogging = true;
                hotlinkallowedsites = "(google\\.|search\\?q=cache)";
                enablegatekeeper = true;
                displaycontactform = false;
                contactformurl = "GateKeeper/AccessDenied.aspx";
                enablehoneypotstats = true;
                smtpenablessl = false;
                honeypotpath = "honeypot/trap.aspx";
                smtpusername = "";
                honeypotstatspath = "GateKeeper/Stats.aspx";
                notifyadmin = false;
                denyemptyuseragent = false;
                smtpserverport = 25;
                virtualpath = "/";
                storepasswordencrypted = false;
                smtpmessagesubject = "Suspected SPAM-Bot has been harvested";
                hotlinkexpression = "\\.(gif|jpg|png|bmp)$";
                persisthoneypotdeny = true;
                displayhotlinkdenyimage = false;
                hotlinkdenyimage = "";
                enablehoneypot = true;
                blockhotlinking = true;
                smtppassword = "";
                smtpmessagebody = "";
                smtpemailaddress = "";
                smtpservername = "";
                accessdeniedmessage = "<div style='text-align: center; color: maroon'><h1>Access Denied</h1><h3>Unfortunately, due to your abuse the request has been denied...</h3><p>If you feel this is an error, send an email to the webmaster of this domain,<br /> or if you are an ill-behaving Bot, Crawler, or Spider then just go away.</p><p style='color: navy; padding-top: 20px;'>Have a nice day!</p></div>";

                enablehttpbl = false;
                enablehttpbllogging = false;
                denyhttpblsuspect = false;
                httpblpostonly = false;
                httpblkeycode = "";
                httpbltestipaddress = "";
                httpbltimeout = 3000;
                threatscorethreshold = 254;

                enableproxybl = false;
                enableproxybllogging = false;
                denyproxyblsuspect = false;
                proxyblpostonly = false;
                proxybltestipaddress = "";
                proxybltimeout = 3000;

                enabledronebl = false;
                enabledronebllogging = false;
                denydroneblsuspect = false;
                droneblpostonly = false;
                dronebltestipaddress = "";
                dronebltimeout = 3000;

            }
            return this;

        }
        */
    }

    #region public enums
    public enum GateKeeperListing
    { GateKeeper_IPAddress, GateKeeper_Url, GateKeeper_Useragent }

    public enum AttributeName
    { ID, Date, IPAddress, UserAgent, Comment }

    public enum Violation
    { IPAddress, EmptyUserAgent, UserAgent, Hotlink, HoneyPot, HttpBL, ProxyBL, DroneBL }

    public enum GateKeeperType
    { Black, White, HoneyPot, HTTP, Proxy, Drone }

    #endregion
}