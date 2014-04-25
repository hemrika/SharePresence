namespace Hemrika.SharePresence.Google.WebmasterTools
{
    using Client;
    using WebmasterTools;
    using System;

    public class Sites : Entry
    {
        public Sites()
        {
        }

        public Sites(string src, string type)
        {
            this.EnsureInnerObject();
            base.AtomEntry.Content.Src = src;
            base.AtomEntry.Content.Type = type;
        }

        protected override void EnsureInnerObject()
        {
            if (base.AtomEntry == null)
            {
                base.AtomEntry = new WebmasterTools.SitesEntry();
            }
        }

        public string CrawlRate
        {
            get
            {
                this.EnsureInnerObject();
                return this.SitesEntry.CrawlRate;
            }
            set
            {
                this.EnsureInnerObject();
                this.SitesEntry.CrawlRate = value;
            }
        }

        public string GeoLocation
        {
            get
            {
                this.EnsureInnerObject();
                return this.SitesEntry.GeoLocation;
            }
            set
            {
                this.EnsureInnerObject();
                this.SitesEntry.GeoLocation = value;
            }
        }

        public string PreferredDomain
        {
            get
            {
                this.EnsureInnerObject();
                return this.SitesEntry.PreferredDomain;
            }
            set
            {
                this.EnsureInnerObject();
                this.SitesEntry.PreferredDomain = value;
            }
        }

        public WebmasterTools.SitesEntry SitesEntry
        {
            get
            {
                return (base.AtomEntry as WebmasterTools.SitesEntry);
            }
        }

        public WebmasterTools.VerificationMethod VerificationMethod
        {
            get
            {
                this.EnsureInnerObject();
                return this.SitesEntry.VerificationMethod;
            }
            set
            {
                this.EnsureInnerObject();
                this.SitesEntry.VerificationMethod = value;
            }
        }

        public string Verified
        {
            get
            {
                this.EnsureInnerObject();
                return this.SitesEntry.Verified;
            }
            set
            {
                this.EnsureInnerObject();
                this.SitesEntry.Verified = value;
            }
        }
    }
}

