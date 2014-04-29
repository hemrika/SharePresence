#region using
using System;
using System.Net;
using System.Text.RegularExpressions;
using System.Web;
using System.Xml;
using System.Linq;
using System.ServiceModel;
#endregion // using

/// <summary>
/// Usage of the DroneBL service is governed through the DroneBL Terms of Service. 
/// By using the DroneBL service you agree to abide by these terms.
/// 
/// To use DroneBL a host need simply perform a DSN lookup of a web visitor's IP address. 
/// DroneBL's DNS system will return a value which indicates the status of the visitor. 
///
/// More information can be found at http://dronebl.org/docs/what
/// </summary>
namespace Hemrika.SharePresence.WebSite.Modules.GateKeeper
{
    class Drone
    {
        #region public methods
        public static Boolean IsDrone(HttpContext current)
        {
            //GateKeeperModule.log.Debug("Entering isDroneBLSuspect");
            dronebl_begin(current);

            // Check if DroneBL is enabled
            if (!GateKeeperModule.config.EnableDroneBL)
            {
                //GateKeeperModule.log.Debug("DroneBL is disabled");
                return false;
            }

            // Check if DroneBLPostOnly is enabled and if this request is a post request
            if (GateKeeperModule.config.DroneBLPostOnly & current.Request.RequestType.ToUpper() != "POST")
            {
                //GateKeeperModule.log.Debug("Current configuration excludes checking non-POST requests");
                return false;
            }

            // Perform lookup and get the results
            Boolean result = performLookup(current);

            // Run eventhandler events
            dronebl_end(current);

            //GateKeeperModule.log.Debug("Leaving isDroneBLSuspect");
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
            string ip = (!string.IsNullOrEmpty(GateKeeperModule.config.DroneBLTestIPAddress)) ? GateKeeperModule.config.DroneBLTestIPAddress : current.Request.UserHostAddress;
            string query = string.Format("{0}.{1}", GateKeeperModule.flipIPAddress(ip), "dnsbl.dronebl.org");
            // perform the lookup
            string lookup = GateKeeperModule.DnsLookup(query, GateKeeperModule.config.DroneBLTimeout);

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
            if (GateKeeperModule.config.EnableDroneBLLogging)
                AddDrone(current);

            if (!GateKeeperModule.config.DenyDroneBLSuspect)
            {
                //GateKeeperModule.log.Debug("Skipping deny as per config settings");
                return false;
            }

            // Deny access to suspected spammer
            return true;
        }

        internal static void AddDrone(HttpContext current)
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

            service.Drone(current.Request.UserHostAddress.ToString(), Referrer, UserAgent);

            // execute eventHandler
            onAddedEntry(current);
        }
        #endregion // Private Methods

        #region events
        public static event EventHandler<EventArgs> DroneBL_Begin;
        private static void dronebl_begin(HttpContext current)
        {
            if (DroneBL_Begin != null)
            {
                DroneBL_Begin(current, new EventArgs());
            }
        }

        public static event EventHandler<EventArgs> DroneBL_End;
        private static void dronebl_end(HttpContext current)
        {
            if (DroneBL_End != null)
            {
                DroneBL_End(current, new EventArgs());
            }
        }

        public static event EventHandler<EventArgs> AddedEntry;
        private static void onAddedEntry(HttpContext current)
        {
            if (AddedEntry != null)
            {
                AddedEntry(current, new EventArgs());
            }
        }
        #endregion //events
    }
}
