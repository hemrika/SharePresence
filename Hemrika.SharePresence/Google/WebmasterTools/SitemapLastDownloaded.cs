namespace Hemrika.SharePresence.Google.WebmasterTools
{
    using Extensions;
    using System;

    public class SitemapLastDownloaded : SimpleElement
    {
        public SitemapLastDownloaded() : base("sitemap-last-downloaded", "wt", "http://schemas.google.com/webmasters/tools/2007")
        {
        }

        public SitemapLastDownloaded(string initValue) : base("sitemap-last-downloaded", "wt", "http://schemas.google.com/webmasters/tools/2007", initValue)
        {
        }
    }
}

