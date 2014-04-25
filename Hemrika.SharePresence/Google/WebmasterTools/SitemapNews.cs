namespace Hemrika.SharePresence.Google.WebmasterTools
{
    using Extensions;
    using System;

    public class SitemapNews : SimpleElement
    {
        public SitemapNews() : base("sitemap-news", "wt", "http://schemas.google.com/webmasters/tools/2007")
        {
        }

        public SitemapNews(string initValue) : base("sitemap-news", "wt", "http://schemas.google.com/webmasters/tools/2007", initValue)
        {
        }
    }
}

