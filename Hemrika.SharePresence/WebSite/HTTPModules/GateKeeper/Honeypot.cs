using System;
using System.Text;
using System.Text.RegularExpressions;
using System.Web;
using System.Xml;
using System.ServiceModel;

namespace Hemrika.SharePresence.WebSite.Modules.GateKeeper
{
    public static class Honeypot
    {
        private static Regex honeypotUrl = new Regex(GateKeeperModule.config.HoneyPotPath, RegexOptions.IgnoreCase);
        
        public static Boolean IsHoney(HttpContext current)
        {
            //GateKeeperModule.log.Debug("Entering isHoneyPotVoilator");
            
            if (!honeypotUrl.IsMatch(current.Request.Url.AbsolutePath))
            {
                //GateKeeperModule.log.Debug("Honeypot url path was not found in the request path");
                return false; 
            }

            if (GateKeeperModule.config.EnableHoneyPotLogging)
            {
                //GateKeeperModule.log.Debug("Adding violator into HoneyPot log");
                AddHoneyPot(current); 
            }

            // Add new entry in the ipaddress deny list
            if (GateKeeperModule.config.PersistHoneyPotDeny)
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
                service.GateKeeper(GateKeeperService.GateKeeperType.Black, GateKeeperService.GateKeeperListing.GateKeeper_IPAddress, current.Request.UserHostAddress.ToString());
            }

            // if NotifyAdmin is enabled then send an email
            if (GateKeeperModule.config.NotifyAdmin)
            { Smtp.SendNotification(current); }

            onViolation(current);
            //GateKeeperModule.log.Debug("Leaving isHoneyPotVoilator");
            return true;
        }

        internal static void AddHoneyPot(HttpContext current)
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
            service.HoneyPot(current.Request.UserHostAddress.ToString(), Referrer, UserAgent);

            // execute eventHandler
            onAddedEntry(current);
        }

        #region events
        public static event EventHandler<EventArgs> Violation;
        private static void onViolation(HttpContext current)
        {
            if (Violation != null)
            {
                //GateKeeperModule.log.Debug("Executing Violation eventHandler");
                Violation(current, new EventArgs()); 
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
