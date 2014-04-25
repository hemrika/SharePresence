namespace Hemrika.SharePresence.Google.WebmasterTools
{
    using Extensions;
    using System;

    public class SitemapNewsPublicationLabel : SimpleElement
    {
        public SitemapNewsPublicationLabel() : base("sitemap-news-publication-label", "wt", "http://schemas.google.com/webmasters/tools/2007")
        {
        }

        public SitemapNewsPublicationLabel(string initValue) : base("sitemap-news-publication-label", "wt", "http://schemas.google.com/webmasters/tools/2007", initValue)
        {
        }
    }
}

