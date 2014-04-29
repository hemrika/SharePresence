#region using
using System;
using System.Net;
using System.Text.RegularExpressions;
using System.Web;
using System.Xml;
using System.Linq;
using Microsoft.SharePoint.Linq;
using System.ServiceModel;

#endregion // using

/// <summary>
/// proxyBL continously crawls the web for IP addresses of possible open (ie publicly accessible) proxy servers. 
/// These IP addresses are then scanned and added to the database, classed according to the result.
/// Allthough open proxies have legitimate uses, they are mostly used by trojans/bots/spammers/kiddies/etc. 
/// to conceal their true host for the purpose of abuse.
/// 
/// This DNSBL is for admins who feel that, considering the amount of abuse open proxies facillitate, the small 
/// percentage of legitimate use of these proxies is acceptable 'collateral damage' and wish to prevent all open 
/// proxies from connecting to a service they provide.
/// 
/// More information can be found at http://www.proxybl.com/docs/faq
/// </summary>
namespace Hemrika.SharePresence.WebSite.Modules.GateKeeper
{
    class Proxy
    {
        #region public methods
        public static Boolean IsProxy(HttpContext current)
        {
            //GateKeeperModule.log.Debug("Entering isProxyBLSuspect");
            proxybl_begin(current);

            // Check if ProxyBL is enabled
            if (!GateKeeperModule.config.EnableProxyBL)
            {
                //GateKeeperModule.log.Debug("ProxyBL is disabled");
                return false;
            }

            // Check if ProxyBLPostOnly is enabled and if this request is a post request
            if (GateKeeperModule.config.ProxyBLPostOnly & current.Request.RequestType.ToUpper() != "POST")
            {
                //GateKeeperModule.log.Debug("Current configuration excludes checking non-POST requests");
                return false;
            }

            // Load ProxyBL data
            //Utils.xmlProxyBLList = Utils.GetXMLDocument(XmlFileType.ProxyBLList);

            // Perform lookup and get the results
            Boolean result = performLookup(current);

            // Run eventhandler events
            proxybl_end(current);

            //GateKeeperModule.log.Debug("Leaving isProxyBLSuspect");
            return result;
        }
        #endregion // public methods

        #region private methods
        private static Boolean performLookup(HttpContext current)
        {
            //GateKeeperModule.log.Debug("Beginning lookup");

            // start the clock
            DateTime _start = DateTime.Now;

            // Use test IP if configured
            string ip = (!string.IsNullOrEmpty(GateKeeperModule.config.ProxyBLTestIPAddress)) ? GateKeeperModule.config.ProxyBLTestIPAddress : current.Request.UserHostAddress;
            string query = string.Format("{0}.{1}", GateKeeperModule.flipIPAddress(ip), "dnsbl.proxybl.org");
            // perform the lookup
            string lookup = GateKeeperModule.DnsLookup(query, GateKeeperModule.config.ProxyBLTimeout);

            // stop the clock
            TimeSpan executiontime = DateTime.Now.Subtract(_start);
            //GateKeeperModule.log.DebugFormat("Lookup completion time in ms [{0}]", executiontime.Milliseconds.ToString());

            // check if we recieved something back
            if (string.IsNullOrEmpty(lookup))
            {
                //GateKeeperModule.log.Debug("Lookup returned no matching domain");
                return false;
            }

            //GateKeeperModule.log.Debug("Requestor IP address found in lookup");
            if (GateKeeperModule.config.EnableProxyBLLogging)
                AddProxy(current);

            if (!GateKeeperModule.config.DenyProxyBLSuspect)
            {
                //GateKeeperModule.log.Debug("Skipping deny as per config settings");
                return false;
            }

            // Deny access to suspected spammer
            return true;
        }

        internal static void AddProxy(HttpContext current)
        {
            string currUrl = current.Request.Url.ToString();
            string basePath = currUrl.Substring(0, currUrl.IndexOf(current.Request.Url.Host) + current.Request.Url.Host.Length);


            // Set up proxy.
            BasicHttpBinding binding = new BasicHttpBinding();
            binding.Security.Mode = BasicHttpSecurityMode.TransportCredentialOnly;
            binding.Security.Transport.ClientCredentialType = HttpClientCredentialType.Ntlm;
            EndpointAddress endpoint = new EndpointAddress(basePath + "/_vti_bin/SharePresence/GateKeeperService.svc");

            GateKeeperService.GateKeeperServiceClient service = new GateKeeperService.GateKeeperServiceClient(binding, endpoint);
            service.ClientCredentials.Windows.AllowedImpersonationLevel = System.Security.Principal.TokenImpersonationLevel.Impersonation;
            string Referrer = current.Request.UrlReferrer == null ? string.Empty : current.Request.UrlReferrer.AbsoluteUri;
            string UserAgent = current.Request.UserAgent == null ? string.Empty : current.Request.UserAgent;

            service.Proxy(current.Request.UserHostAddress.ToString(), Referrer, UserAgent);

            // execute eventHandler
            onAddedEntry(current);
        }
        #endregion // Private Methods

        #region events
        public static event EventHandler<EventArgs> ProxyBL_Begin;
        private static void proxybl_begin(HttpContext current)
        {
            if (ProxyBL_Begin != null)
            {
                //GateKeeperModule.log.Debug("Executing ProxyBL_Begin eventHandler");
                ProxyBL_Begin(current, new EventArgs());
            }
        }

        public static event EventHandler<EventArgs> ProxyBL_End;
        private static void proxybl_end(HttpContext current)
        {
            if (ProxyBL_End != null)
            {
                //GateKeeperModule.log.Debug("Executing ProxyBL_End eventHandler");
                ProxyBL_End(current, new EventArgs());
            }
        }

        public static event EventHandler<EventArgs> AddedEntry;
        private static void onAddedEntry(HttpContext current)
        {
            if (AddedEntry != null)
            {
                //GateKeeperModule.log.Debug("Executing AddedEntry eventHandler");
                AddedEntry(current, new EventArgs());
            }
        }
        #endregion //events
    }
}
