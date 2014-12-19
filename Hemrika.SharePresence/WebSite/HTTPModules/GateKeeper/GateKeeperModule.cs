// -----------------------------------------------------------------------
// <copyright file="ErrorModule.cs" company="">
// TODO: Update copyright text.
// </copyright>
// -----------------------------------------------------------------------

namespace Hemrika.SharePresence.WebSite.Modules.GateKeeper
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using Hemrika.SharePresence.Common.WebSiteController;
    using System.Web;
    using System.Text.RegularExpressions;
    using System.Globalization;
    using Microsoft.SharePoint;
    using System.IO;
    using System.Web.SessionState;
    using System.Net;
    using Hemrika.SharePresence.WebSite.ContentTypes;
    using System.Web.Script.Serialization;
    using System.ServiceModel;
    using Hemrika.SharePresence.WebSite.GateKeeperService;
    using Microsoft.SharePoint.Administration;
    using System.Diagnostics;

    /// <summary>
    /// TODO: Update summary.
    /// </summary>
    public class GateKeeperModule : IWebSiteControllerModule, IDisposable
    {
        /// <summary>
        /// The GUID to identify this module
        /// </summary>
        //private const string ID = "FE6B2E3E-2979-4016-9848-C98322E50E55";
        private Guid ID;
        private HttpApplication application;
        internal static GateKeeperSettings config = new GateKeeperSettings();
        //public static Settings config = new Settings();
        //public static GateKeeperSettings config = new GateKeeperSettings();
        /// <summary>
        /// Initializes a new instance of the <see cref="SubscriptionModule"/> class.
        /// </summary>
        public GateKeeperModule()
        {
        }

        /// <summary>
        /// Gets the type of the rule.
        /// </summary>
        /// <value>The type of the rule.</value>
        public string RuleType
        {
            get { return this.GetType().FullName; }
        }

        /// <summary>
        /// Gets the short type of the rule for display purposes.
        /// </summary>
        /// <value>The short type of the rule for display purposes.</value>
        public string ShortRuleType
        {
            get { return this.GetType().Name; }
        }

        /// <summary>
        /// Gets the id associated with this module.
        /// </summary>
        /// <value>The id associated with this module.</value>
        public Guid Id
        {
            get { return ID; }
            set { ID = value; }
        }

        /// <summary>
        /// Gets the virutal path to the control used to update properties for this rule type.
        /// </summary>
        /// <value>The control.</value>
        public string Control
        {
            get { return "~/_controltemplates/15/Hemrika/Controls/GateKeeperModuleControl.ascx"; }
        }

        /// <summary>
        /// Attaches to appropriate request pipeline events
        /// </summary>
        /// <param name="pageControllerModule">The PageControllerModule whose event the module can attach to</param>
        public void Init(WebSiteControllerModule WebSiteControllerModule)
        {
            WebSiteControllerModule.OnBeginRequest += new EventHandler(WebSiteControllerModule_OnBeginRequest);
            WebSiteControllerModule.OnEndRequest += new EventHandler(WebSiteControllerModule_OnEndRequest);
        }

        void WebSiteControllerModule_OnBeginRequest(object sender, EventArgs e)
        {
            //Uri _url = (sender as HttpApplication).Context.Request.Url;
            //Debug.WriteLine("Start GateKeeper:" + DateTime.Now + " : " + _url.OriginalString);

            application = (HttpApplication)sender;

            string absolutePath = application.Request.Url.AbsolutePath.ToLower();
            if (absolutePath.Contains(".dll") ||
                absolutePath.Contains(".asmx") ||
                absolutePath.Contains(".svc") ||
                absolutePath.Contains("favicon.ico"))
            {
                return;
            }

            HttpContext current = ((HttpApplication)sender).Context;
            HttpRequest request = current.Request;

            string currUrl = current.Request.Url.ToString();
            string basePath = currUrl.Substring(0, currUrl.IndexOf(current.Request.Url.Host) + current.Request.Url.Host.Length);
            Uri url = new Uri(basePath);

            config = config.Load();

            if (config != null && config._guid != Guid.Empty)
            {

                // Check if GK is enabled and is not a system handle
                if (!config.EnableGateKeeper || currUrl.ToLower().Contains("/error/") || currUrl.ToLower().Contains("/style library/") || currUrl.ToLower().Contains("/_layouts/") || currUrl.ToLower().Contains("/_vti_bin/") || currUrl.ToLower().Contains("gatekeeperservice.svc"))
                {
                    return;
                }

                // Checking if UserAgent is empty and if GK should block empty UserAgents
                if (string.IsNullOrEmpty(request.UserAgent) && config.DenyEmptyUserAgent)
                {
                    application.Server.ClearError();
                    application.Response.StatusCode = (int)HttpStatusCode.Forbidden;
                    throw (new HttpException((int)HttpStatusCode.Forbidden, "Empty UserAgents are not Allowed."));
                }


                // Check if request matches a whitelist
                if (Whitelist.OnWhiteList(current))
                {
                    //log.Debug("Request passed WhiteList check - let them pass");
                    return;
                }

                // Check if request matches a whitelist
                if(Blacklist.OnBlackList(current))
                {
                    application.Server.ClearError();
                    application.Response.StatusCode = (int)HttpStatusCode.Forbidden;
                    throw (new HttpException((int)HttpStatusCode.Forbidden, "Your Black Listed on this Site."));

                }

                // Check IP Address against HttpBL
                if (Http.IsHTTP(current))
                {
                    application.Server.ClearError();
                    application.Response.StatusCode = (int)HttpStatusCode.Forbidden;
                    throw (new HttpException((int)HttpStatusCode.Forbidden, "This Computer has been flagged as dangerous."));
                }

                // Check IP Address against DroneBL
                if (Drone.IsDrone(current))
                {
                    application.Server.ClearError();
                    application.Response.StatusCode = (int)HttpStatusCode.Forbidden;
                    throw (new HttpException((int)HttpStatusCode.Forbidden, "This Computer is a Drone."));
                }

                // Check if IP is an Open Proxy
                if (Proxy.IsProxy(current))
                {
                    application.Server.ClearError();
                    application.Response.StatusCode = (int)HttpStatusCode.Forbidden;
                    throw (new HttpException((int)HttpStatusCode.Forbidden, "This Proxy is not allowed."));
                }

                // Check if request is hotlinked
                if (config.BlockHotLinking && Hotlink.IsHotLink(current))
                {
                    application.Server.ClearError();
                    application.Response.StatusCode = (int)HttpStatusCode.Forbidden;
                    throw (new HttpException((int)HttpStatusCode.Forbidden, "Hotlinking is not allowed."));
                }
                
                // Check if request is a honeypot violator
                if (config.EnableHoneyPot && Honeypot.IsHoney(current))
                {
                    application.Server.ClearError();
                    application.Response.StatusCode = (int)HttpStatusCode.Forbidden;
                    throw (new HttpException((int)HttpStatusCode.Forbidden,"Your visting the Honeypot."));
                }
                
                // Check if request is for virtual honeypot stats url
                if (config.EnableHoneyPotStats &&
                    !string.IsNullOrEmpty(config.HoneyPotStatsPath) &&
                        Regex.IsMatch(request.Url.AbsolutePath, config.HoneyPotStatsPath, RegexOptions.IgnoreCase))
                { HoneyPotStats.Display(); }
                
                // Request passed all checks - let them pass
            }

            //return;
        }

        void WebSiteControllerModule_OnEndRequest(object sender, EventArgs e)
        {
            /*
            application = sender as HttpApplication;
            string absolutePath = application.Request.Url.AbsolutePath.ToLower();
            if (absolutePath.Contains(".dll") ||
                absolutePath.Contains(".asmx") ||
                absolutePath.Contains(".svc") ||
                absolutePath.Contains("favicon.ico"))
            {
                return;
            }
            */
            //Uri url = (sender as HttpApplication).Context.Request.Url;
            //Debug.WriteLine("End GateKeeper:" + DateTime.Now + " : " + url.OriginalString);

        }

        /// <summary>
        /// Gets the user-friendly name of a property.
        /// </summary>
        /// <param name="propertyName">Name of the property.</param>
        /// <returns>
        /// The friendly name of the property to be used for display purposes
        /// </returns>
        public string GetFriendlyName(string propertyName)
        {
            switch (propertyName)
            {
                case "GateKeeper":
                    return "GateKeeper";
                default:
                    return propertyName;
            }

        }

        public bool CanBeRemoved
        {
            get { return false; }
        }


        public bool AlwaysRun
        {
            get { return true; }
        }

        /// <summary>
        /// Performs a DNS Lookup using an async call and terminates
        /// after specified period of time has passed
        /// </summary>
        /// <param name="hostName">www.dscoduc.com</param>
        /// <returns>192.168.1.100</returns>
        private delegate IPHostEntry DnsLookupHandler(string hostName);
        public static string DnsLookup(string hostName, int timeout)
        {
            string response = null;
            try
            {
                DnsLookupHandler callback = new DnsLookupHandler(Dns.GetHostEntry);
                IAsyncResult result = callback.BeginInvoke(hostName, null, null);
                if (result.AsyncWaitHandle.WaitOne(timeout, false))
                {   // Received response within timeout limit
                    IPHostEntry he = callback.EndInvoke(result);
                    IPAddress[] ip_addrs = he.AddressList;
                    if (ip_addrs.Length > 0)
                        response = ip_addrs[0].ToString();
                }
                return response;
            }
            catch (Exception ex)
            {
                SPDiagnosticsService.Local.WriteTrace(0, new SPDiagnosticsCategory(ex.Source, TraceSeverity.High, EventSeverity.Error), TraceSeverity.High, ex.Message, ex.Data);
                //GateKeeperModule.log.WarnFormat("DnsLookup : [{0}]", ex.Message);
                //GateKeeperModule.log.Error(ex.StackTrace);
                return response;
            }
        }
        /// <summary>
        /// Converts a forward IP address into a reversed IP Address
        /// suitable for dnsBl to process in the query
        /// </summary>
        /// <param name="ipaddress">ex. 192.168.1.100</param>
        /// <returns>ex. 100.1.168.192</returns>
        public static string flipIPAddress(string ipaddress)
        {
            string returnString = null;
            try
            {
                // Split string into four octects
                string[] octects = ipaddress.Split(new char[] { '.' }, 4);
                // Build new string in reverse
                for (int i = octects.Length - 1; i >= 0; i--)
                    returnString += string.Format("{0}.", octects[i]);

                // Remove trailing period
                returnString = returnString.Trim(new char[] { '.' });
                return returnString;
            }
            catch (Exception ex)
            {
                SPDiagnosticsService.Local.WriteTrace(0, new SPDiagnosticsCategory(ex.Source, TraceSeverity.High, EventSeverity.Error), TraceSeverity.High, ex.Message, ex.Data);
                //GateKeeperModule.log.WarnFormat("flipIPAddress : [{0}]", ex.Message);
                //GateKeeperModule.log.Error(ex.StackTrace);
                return returnString;
            }
        }

        internal static List<SPListItem> GetGateKeeperItems(GateKeeperType type, GateKeeperListing node,string value)
        {
            
            List<SPListItem> items = new List<SPListItem>();

            /*
            SPListItemCollection nodes = null;
            SPWeb web = SPContext.Current.Web;

            SPList gatekeeper = web.Lists["GateKeeper"];

            SPQuery query = new SPQuery();
            query.Query = "<Where><And><Eq><FieldRef Name=\"GateKeeper_Type\" /><Value Type=\"Choice\">" + type.ToString() + "</Value></Eq><Eq><FieldRef Name=\"" + node.ToString() + "\" /><Value Type=\"Text\">" + value + "</Value></Eq></And></Where>";
            query.ViewFields = "<FieldRef Name=\"GateKeeper_Type\" /><FieldRef Name=\""+node+"\" />";
            nodes = gatekeeper.GetItems(query);

            if (nodes.Count > 0)
            {
                foreach (SPListItem item in nodes)
                {
                    items.Add(item);
                }   
            }
            */
            return items;
        }

        internal static List<SPListItem> GetGateKeeperItems(GateKeeperType type, GateKeeperListing node)
        {
            List<SPListItem> items = new List<SPListItem>();

            SPListItemCollection nodes = null;
            SPWeb web = SPContext.Current.Web;

            SPList gatekeeper = web.Lists["GateKeeper"];

            SPQuery query = new SPQuery();
            query.Query = "<Where><And><Eq><FieldRef Name=\"GateKeeper_Type\" /><Value Type=\"Choice\">" + type.ToString() + "</Value></Eq><IsNotNull><FieldRef Name=\"" + node.ToString() + "\" /></IsNotNull></And></Where>";
            query.ViewFields = "<FieldRef Name=\"GateKeeper_Type\" /><FieldRef Name=\"GateKeeper_Date\" /><FieldRef Name=\"GateKeeper_Comment\" /><FieldRef Name=\"" + node + "\" />";
            nodes = gatekeeper.GetItems(query);

            if (nodes.Count > 0)
            {
                foreach (SPListItem item in nodes)
                {
                    items.Add(item);
                }
            }

            return items;
        }

        internal static List<SPListItem> GetGateKeeperItems(GateKeeperType type)
        {
            List<SPListItem> items = new List<SPListItem>();

            SPListItemCollection nodes = null;
            SPWeb web = SPContext.Current.Web;

            SPList gatekeeper = web.Lists["GateKeeper"];

            SPQuery query = new SPQuery();
            query.Query = "<Where><Eq><FieldRef Name=\"GateKeeper_Type\" /><Value Type=\"Choice\">" + type.ToString() + "</Value></Eq></Where>";
            query.ViewFields = "<FieldRef Name=\"GateKeeper_Type\" /><FieldRef Name=\"GateKeeper_Date\" /><FieldRef Name=\"GateKeeper_Comment\" /><FieldRef Name=\"GateKeeper_IPAddress\" /><FieldRef Name=\"GateKeeper_Referrer\" /><FieldRef Name=\"GateKeeper_Useragent\" /><FieldRef Name=\"GateKeeper_Url\" /><FieldRef Name=\"GateKeeper_Threatlevel\" /><FieldRef Name=\"GateKeeper_Visitortype\" /><FieldRef Name=\"GateKeeper_LastActivity\" />"; 
            nodes = gatekeeper.GetItems(query);

            if (nodes.Count > 0)
            {
                foreach (SPListItem item in nodes)
                {
                    items.Add(item);
                }
            }

            return items;
        }

        internal static void DeleteGateKeeperItem(int id)
        {
            SPWeb web = null;
            string url = SPContext.Current.Web.Url;

            SPSecurity.RunWithElevatedPrivileges(delegate()
            {
                using (SPSite site = new SPSite(url, SPUserToken.SystemAccount))
                {
                    site.AllowUnsafeUpdates = true;
                    web = site.OpenWeb();

                    if (web != null)
                    {
                        web.AllowUnsafeUpdates = true;

                        SPList gatekeeper = web.Lists["GateKeeper"];
                        SPListItem item = gatekeeper.Items.GetItemById(id);
                        if (item != null)
                        {
                            item.Delete();
                            gatekeeper.Update();
                        }
                        web.AllowUnsafeUpdates = false;
                    }

                    site.AllowUnsafeUpdates = false;
                }
            });


        }

        private bool _disposed = false;

        protected virtual void Dispose(bool disposing)
        {
            if (!_disposed)
            {
                if (disposing)
                {
                    application.Dispose();
                    application = null;
                    config = null;
                }
                _disposed = true;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        ~GateKeeperModule()
        {
            Dispose(false);
        }
    }
}
