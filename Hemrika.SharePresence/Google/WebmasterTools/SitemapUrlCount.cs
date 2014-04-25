namespace Hemrika.SharePresence.Google.WebmasterTools
{
    using Extensions;
    using System;

    public class SitemapUrlCount : SimpleElement
    {
        public SitemapUrlCount() : base("sitemap-url-count", "wt", "http://schemas.google.com/webmasters/tools/2007")
        {
        }

        public SitemapUrlCount(string initValue) : base("sitemap-url-count", "wt", "http://schemas.google.com/webmasters/tools/2007", initValue)
        {
        }
    }
}

