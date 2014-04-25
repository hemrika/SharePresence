namespace Hemrika.SharePresence.Google.WebmasterTools
{
    using Client;
    using System;

    public class SitemapsEntry : WebmasterToolsBaseEntry
    {
        public SitemapsEntry()
        {
            //Tracing.TraceMsg("Created SitemapsEntry");
            base.AddExtension(new WebmasterTools.Mobile());
            base.AddExtension(new WebmasterTools.SitemapLastDownloaded());
            base.AddExtension(new WebmasterTools.SitemapMobile());
            base.AddExtension(new WebmasterTools.SitemapMobileMarkupLanguage());
            base.AddExtension(new WebmasterTools.SitemapNews());
            base.AddExtension(new WebmasterTools.SitemapNewsPublicationLabel());
            base.AddExtension(new WebmasterTools.SitemapType());
            base.AddExtension(new WebmasterTools.SitemapUrlCount());
        }

        public string Mobile
        {
            get
            {
                return base.getWebmasterToolsValue("mobile");
            }
            set
            {
                base.setWebmasterToolsExtension("mobile", value);
            }
        }

        public string SitemapLastDownloaded
        {
            get
            {
                return base.getWebmasterToolsValue("sitemap-last-downloaded");
            }
            set
            {
                base.setWebmasterToolsExtension("sitemap-last-downloaded", value);
            }
        }

        public string SitemapMobile
        {
            get
            {
                return base.getWebmasterToolsValue("sitemap-mobile");
            }
            set
            {
                base.setWebmasterToolsExtension("sitemap-mobile", value);
            }
        }

        public string SitemapMobileMarkupLanguage
        {
            get
            {
                return base.getWebmasterToolsValue("sitemap-mobile-markup-language");
            }
            set
            {
                base.setWebmasterToolsExtension("sitemap-mobile-markup-language", value);
            }
        }

        public string SitemapNews
        {
            get
            {
                return base.getWebmasterToolsValue("sitemap-news");
            }
            set
            {
                base.setWebmasterToolsExtension("sitemap-news", value);
            }
        }

        public string SitemapNewsPublicationLabel
        {
            get
            {
                return base.getWebmasterToolsValue("sitemap-news-publication-label");
            }
            set
            {
                base.setWebmasterToolsExtension("sitemap-news-publication-label", value);
            }
        }

        public string SitemapType
        {
            get
            {
                return base.getWebmasterToolsValue("sitemap-type");
            }
            set
            {
                base.setWebmasterToolsExtension("sitemap-type", value);
            }
        }

        public string SitemapUrlCount
        {
            get
            {
                return base.getWebmasterToolsValue("sitemap-url-count");
            }
            set
            {
                base.setWebmasterToolsExtension("sitemap-url-count", value);
            }
        }
    }
}

