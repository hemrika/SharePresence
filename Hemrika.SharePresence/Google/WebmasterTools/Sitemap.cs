namespace Hemrika.SharePresence.Google.WebmasterTools
{
    using Client;
    using WebmasterTools;
    using System;

    public class Sitemap : Entry
    {
        protected override void EnsureInnerObject()
        {
            if (base.AtomEntry == null)
            {
                base.AtomEntry = new WebmasterTools.SitemapsEntry();
            }
        }

        public string Mobile
        {
            get
            {
                this.EnsureInnerObject();
                return this.SitemapsEntry.Mobile;
            }
            set
            {
                this.EnsureInnerObject();
                this.SitemapsEntry.Mobile = value;
            }
        }

        public string SitemapLastDownloaded
        {
            get
            {
                this.EnsureInnerObject();
                return this.SitemapsEntry.SitemapLastDownloaded;
            }
            set
            {
                this.EnsureInnerObject();
                this.SitemapsEntry.SitemapLastDownloaded = value;
            }
        }

        public string SitemapMobile
        {
            get
            {
                this.EnsureInnerObject();
                return this.SitemapsEntry.SitemapMobile;
            }
            set
            {
                this.EnsureInnerObject();
                this.SitemapsEntry.SitemapMobile = value;
            }
        }

        public string SitemapMobileMarkupLanguage
        {
            get
            {
                this.EnsureInnerObject();
                return this.SitemapsEntry.SitemapMobileMarkupLanguage;
            }
            set
            {
                this.EnsureInnerObject();
                this.SitemapsEntry.SitemapMobileMarkupLanguage = value;
            }
        }

        public string SitemapNews
        {
            get
            {
                this.EnsureInnerObject();
                return this.SitemapsEntry.SitemapNews;
            }
            set
            {
                this.EnsureInnerObject();
                this.SitemapsEntry.SitemapNews = value;
            }
        }

        public string SitemapNewsPublicationLabel
        {
            get
            {
                this.EnsureInnerObject();
                return this.SitemapsEntry.SitemapNewsPublicationLabel;
            }
            set
            {
                this.EnsureInnerObject();
                this.SitemapsEntry.SitemapNewsPublicationLabel = value;
            }
        }

        public WebmasterTools.SitemapsEntry SitemapsEntry
        {
            get
            {
                return (base.AtomEntry as WebmasterTools.SitemapsEntry);
            }
        }

        public string SitemapType
        {
            get
            {
                this.EnsureInnerObject();
                return this.SitemapsEntry.SitemapType;
            }
            set
            {
                this.EnsureInnerObject();
                this.SitemapsEntry.SitemapType = value;
            }
        }

        public string SitemapUrlCount
        {
            get
            {
                this.EnsureInnerObject();
                return this.SitemapsEntry.SitemapUrlCount;
            }
            set
            {
                this.EnsureInnerObject();
                this.SitemapsEntry.SitemapUrlCount = value;
            }
        }
    }
}

