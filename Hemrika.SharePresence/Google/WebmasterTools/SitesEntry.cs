namespace Hemrika.SharePresence.Google.WebmasterTools
{
    using Client;
    using System;

    public class SitesEntry : WebmasterToolsBaseEntry
    {
        public SitesEntry()
        {
            //Tracing.TraceMsg("Created SitesEntry");
            base.AddExtension(new WebmasterTools.CrawlRate());
            base.AddExtension(new WebmasterTools.GeoLocation());
            base.AddExtension(new WebmasterTools.PreferredDomain());
            base.AddExtension(new WebmasterTools.VerificationMethod());
            base.AddExtension(new WebmasterTools.Verified());
        }

        public new SitesEntry Update()
        {
            return (base.Update() as SitesEntry);
        }

        public string CrawlRate
        {
            get
            {
                return base.getWebmasterToolsValue("crawl-rate");
            }
            set
            {
                base.setWebmasterToolsExtension("crawl-rate", value);
            }
        }

        public string GeoLocation
        {
            get
            {
                return base.getWebmasterToolsValue("geolocation");
            }
            set
            {
                base.setWebmasterToolsExtension("geolocation", value);
            }
        }

        public string PreferredDomain
        {
            get
            {
                return base.getWebmasterToolsValue("preferred-domain");
            }
            set
            {
                base.setWebmasterToolsExtension("preferred-domain", value);
            }
        }

        public WebmasterTools.VerificationMethod VerificationMethod
        {
            get
            {
                return (base.getWebmasterToolsExtension("verification-method") as WebmasterTools.VerificationMethod);
            }
            set
            {
                base.ReplaceExtension("verification-method", "http://schemas.google.com/webmasters/tools/2007", value);
            }
        }

        public string Verified
        {
            get
            {
                return base.getWebmasterToolsValue("verified");
            }
            set
            {
                base.setWebmasterToolsExtension("verified", value);
            }
        }
    }
}

