#region using
using System;
using System.Net;
using System.Text.RegularExpressions;
using System.Web;
using System.Xml;
using System.ServiceModel;
using Microsoft.SharePoint.Administration;
#endregion // using

/// <summary>
/// Usage of the http:BL service is governed through the Project Honey Pot Terms of Service. 
/// By using the http:BL service you agree to abide by these terms.
/// 
/// To use http:BL a host need simply perform a DSN lookup of a web visitor's IP address. 
/// Http:BL's DNS system will return a value which indicates the status of the visitor. 
/// Visitors may be identified as search engines, suspicious, harvesters, comment spammers, 
/// or a combination thereof. The response to the DNS query, as outlined below, indicates what 
/// type of visitor is accessing your page.
///
/// Each user of http:BL is required to register with Project Honey Pot (www.projecthoneypot.org). 
/// Each user of http:BL must also request an Access Key (http://www.projecthoneypot.org/httpbl_configure.php)
/// to make use of the service. All Access Keys are 12-characters in length, lower case, and contain only alpha 
/// characters (no numbers). Generating non-assigned keys, not including a key in DNS queries, and sharing keys 
/// with other members or non-members are all violations of the 
/// Terms of Service (http://www.projecthoneypot.org/terms_of_service_use.php).
/// 
/// More information can be found at http://www.projecthoneypot.org/httpbl_api.php
/// </summary>
namespace Hemrika.SharePresence.WebSite.Modules.GateKeeper
{
    class Http
    {
        #region public methods
        public static Boolean IsHTTP(HttpContext current)
        {
            //GateKeeperModule.log.Debug("Entering isHttpBLSuspect");
            httpbl_begin(current);

            // Check if HttpBL is enabled
            if (!GateKeeperModule.config.EnableHttpBL)
            {
                //GateKeeperModule.log.Debug("HttpBL is disabled");
                return false;
            }

            // Check if KeyCode is populated
            if (string.IsNullOrEmpty(GateKeeperModule.config.HttpBLKeyCode))
            {
                //GateKeeperModule.log.Warn("HttpBL KeyCode cannot be empty");
                return false;
            }
            
            // Check if HttpBLPostOnly is enabled and if this request is a post request
            if (GateKeeperModule.config.HttpBLPostOnly & current.Request.RequestType.ToUpper() != "POST")
            {
                //GateKeeperModule.log.Debug("Current configuration excludes checking non-POST requests");
                return false;
            }

            // Load httpbl data
            //Utils.xmlHttpBLList = Utils.GetXMLDocument(XmlFileType.HttpBLList);

            // Perform lookup and get the results
            Boolean result = performLookup(current);

            // Run eventhandler events
            httpbl_end(current);

            //GateKeeperModule.log.Debug("Leaving isHttpBLSuspect");
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
            string ip = (!string.IsNullOrEmpty(GateKeeperModule.config.HttpBLTestIPAddress)) ? GateKeeperModule.config.HttpBLTestIPAddress : current.Request.UserHostAddress;
            string query = string.Format("{0}.{1}.{2}", GateKeeperModule.config.HttpBLKeyCode, GateKeeperModule.flipIPAddress(ip), "dnsbl.httpbl.org");
            // perform the lookup
            string lookup = GateKeeperModule.DnsLookup(query, GateKeeperModule.config.HttpBLTimeout);

            // stop the clock
            TimeSpan executiontime = DateTime.Now.Subtract(_start);
            //GateKeeperModule.log.DebugFormat("Lookup completion time in ms [{0}]", executiontime.Milliseconds.ToString());


            // check if we recieved something back
            if (string.IsNullOrEmpty(lookup))
            {
                //GateKeeperModule.log.Debug("Lookup returned no matching domain");
                return false;
            }

            // Split out response
            Regex parse = new Regex("(?<status>\\d+)\\.(?<days>\\d+)\\.(?<threat>\\d+)\\.(?<type>\\d+)", RegexOptions.Compiled);
            Match match = parse.Match(lookup);

            // Match found, load the lookup values
            int lastactivity = int.Parse(match.Groups["days"].Value);
            int threatscore = int.Parse(match.Groups["threat"].Value);
            int visitortype = int.Parse(match.Groups["type"].Value);

            // See if threatscore is greater than threshold set by user
            if (threatscore >= GateKeeperModule.config.ThreatScoreThreshold)
            {
                //GateKeeperModule.log.Debug("Requestor IP address equal to or above threatscore threshold");
                if (GateKeeperModule.config.EnableHttpBLLogging)
                    AddHttp(current, threatscore, visitortype, lastactivity);

                // Check if only check and no deny
                if (!GateKeeperModule.config.DenyHttpBLSuspect)
                {
                    //GateKeeperModule.log.Debug("Skipping deny as per config settings");
                    return false;
                }
                
                // Deny access to suspected spammer
                return true;
            }
            else
            {
                //GateKeeperModule.log.Debug("Requestor IP address below threatscore threshold");
                return false;
            }
        }

        internal static void AddHttp(HttpContext current, int threatscore, int visitortype, int lastactivity)
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
            string LastActivity = string.Format("{0} [{1}]", getLastActivity(lastactivity), lastactivity);
            string ThreatLevel = string.Format("{0} [{1}]", getThreatLevel(threatscore), threatscore);
            string VisitorType = string.Format("{0} [{1}]", getVisitorType(visitortype), visitortype);

            service.HTTP(current.Request.UserHostAddress.ToString(), LastActivity, Referrer, ThreatLevel, UserAgent,VisitorType);

            // execute eventHandler
            onAddedEntry(current);
        }        
        private static string getThreatLevel(int value)
        {
            try
            {
                if (value >= 0 & value <= 10)
                    return "Very low";
                else if (value >= 11 & value <= 25)
                    return "Low";
                else if (value >= 26 & value <= 40)
                    return "Medium-Low";
                else if (value >= 41 & value <= 60)
                    return "Medium";
                else if (value >= 61 & value <= 75)
                    return "Medium-High";
                else if (value >= 76 & value <= 125)
                    return "High";
                else if (value >= 126 & value <= 255)
                    return "Toxic";
                else
                    return string.Empty;
            }
            catch (Exception ex)
            {
                SPDiagnosticsService.Local.WriteTrace(0, new SPDiagnosticsCategory(ex.Source, TraceSeverity.High, EventSeverity.Error), TraceSeverity.High, ex.Message, ex.Data);
                //GateKeeperModule.log.WarnFormat("getThreatLevel : [{0}]", ex.Message);
                //GateKeeperModule.log.Error(ex.StackTrace);
                return string.Empty;
            }
        }
        private static string getVisitorType(int value)
        {
            string type = Enum.GetName(typeof(visitorType), value);
            return type.Replace("_", " & ").Replace("CommentSpammer", "Comment Spammer");
        }
        private static string getLastActivity(int value)
        {
            string stream = string.Format("{0} days ago", value);
            if (value == 0)
                stream = "Today";
            if (value == 1)
                stream = "Yesterday";
            if (value >= 2 & value <= 7)
                stream = "This week";
            if (value >= 8 & value <= 14)
                stream = "Last week";
            if (value >= 15)
                stream = string.Format("{0} days ago", value);
            return stream;
        }
        #endregion // private methods

        #region enum
        enum visitorType
        {
            SearchEngine,
            Suspicious,
            Harvester,
            Suspicious_Harvester,
            CommentSpammer,
            Suspicious_CommentSpammer,
            Harvester_CommentSpammer,
            Suspicious_Harvester_CommentSpammer
        }
        #endregion // enum

        #region events
        public static event EventHandler<EventArgs> HttpBL_Begin;
        private static void httpbl_begin(HttpContext current)
        {
            if (HttpBL_Begin != null)
            {
                //GateKeeperModule.log.Debug("Executing HttpBL_Begin eventHandler");
                HttpBL_Begin(current, new EventArgs());
            }
        }

        public static event EventHandler<EventArgs> HttpBL_End;
        private static void httpbl_end(HttpContext current)
        {
            if (HttpBL_End != null)
            {
                //GateKeeperModule.log.Debug("Executing HttpBL_End eventHandler");
                HttpBL_End(current, new EventArgs());
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