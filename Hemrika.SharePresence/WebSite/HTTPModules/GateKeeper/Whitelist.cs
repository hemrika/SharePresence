using System;
using System.Collections.Generic;
using System.Text;
using System.Text.RegularExpressions;
using System.Web;
using System.Xml;
using System.Linq;
using System.ServiceModel;
using Hemrika.SharePresence.WebSite.GateKeeperService;
using System.Net;
namespace Hemrika.SharePresence.WebSite.Modules.GateKeeper
{
    public static class Whitelist
    {
        public static Boolean OnWhiteList(HttpContext current)
        {
            onBegin_Check(current);
            // Using try catch in order to ensure eventhandler executes
            try
            {
                // Check if request is for contact page url specified in config file
                if (!string.IsNullOrEmpty(GateKeeperModule.config.ContactFormUrl) &&
                                Regex.IsMatch(current.Request.Url.AbsolutePath, GateKeeperModule.config.ContactFormUrl, RegexOptions.IgnoreCase))
                {
                    //GateKeeperModule.log.Debug("Request for ContactPage - let them pass");
                    return true;
                }

                if (current.Request.Url.AbsolutePath.Contains("GateKeeperService.svc"))
                {
                    return true;
                }

                string currUrl = current.Request.Url.ToString();
                string basePath = currUrl.Substring(0, currUrl.IndexOf(current.Request.Url.Host) + current.Request.Url.Host.Length);


                // Set up proxy.
                BasicHttpBinding binding = new BasicHttpBinding();
                binding.Security.Mode = BasicHttpSecurityMode.TransportCredentialOnly;
                binding.Security.Transport.ClientCredentialType = HttpClientCredentialType.Ntlm;
                EndpointAddress endpoint = new EndpointAddress(basePath + "/_vti_bin/SharePresence/GateKeeperService.svc");

                GateKeeperService.GateKeeperServiceClient service = new GateKeeperService.GateKeeperServiceClient(binding, endpoint);
                service.ClientCredentials.Windows.AllowedImpersonationLevel = System.Security.Principal.TokenImpersonationLevel.Impersonation;


                if (service.HasListing(GateKeeperService.GateKeeperType.White, GateKeeperService.GateKeeperListing.GateKeeper_IPAddress, current.Request.UserHostAddress)) { return true; }
                if (service.HasListing(GateKeeperService.GateKeeperType.White, GateKeeperService.GateKeeperListing.GateKeeper_Useragent, current.Request.UserAgent)) { return true; }
                if (service.HasListing(GateKeeperService.GateKeeperType.White, GateKeeperService.GateKeeperListing.GateKeeper_Url, current.Request.Url.AbsoluteUri)) { return true; }


                // Request did not match any of the whitelist checks
                return false;
            }
            catch (Exception ex)
            {
                ex.ToString();
            }
            finally
            { onEnd_Check(current); }

            return false;
        }

        internal static void AddWhiteListing(HttpContext current)
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

            service.GateKeeper(GateKeeperService.GateKeeperType.White, GateKeeperService.GateKeeperListing.GateKeeper_IPAddress, current.Request.UserHostAddress.ToString());
            
            if (!string.IsNullOrEmpty(Referrer))
            {
                service.GateKeeper(GateKeeperService.GateKeeperType.White, GateKeeperService.GateKeeperListing.GateKeeper_Url, Referrer);
            }

            if (!string.IsNullOrEmpty(UserAgent))
            {
                service.GateKeeper(GateKeeperService.GateKeeperType.White, GateKeeperService.GateKeeperListing.GateKeeper_Useragent, UserAgent);
            }

        }

        internal static void AddWhiteListing(HttpContext current,GateKeeperService.GateKeeperListing listing, string value)
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
            service.GateKeeper(GateKeeperService.GateKeeperType.White, listing, value);
        }

        #region eventhandlers
        public static event EventHandler<EventArgs> Begin_Check;
        private static void onBegin_Check(HttpContext current)
        {
            if (Begin_Check != null)
            {
                //GateKeeperModule.log.Debug("Executing Begin_Check eventHandler");
                Begin_Check(current, new EventArgs());
            }
        }

        public static event EventHandler<EventArgs> End_Check;
        private static void onEnd_Check(HttpContext current)
        {
            if (End_Check != null)
            {
                //GateKeeperModule.log.Debug("Executing End_Check eventHandler");
                End_Check(current, new EventArgs());
            }
        }
        #endregion //events
    }
}
