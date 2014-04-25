namespace Hemrika.SharePresence.Google.WebmasterTools
{
    using Extensions;
    using System;

    public class SitemapType : SimpleElement
    {
        public SitemapType() : base("sitemap-type", "wt", "http://schemas.google.com/webmasters/tools/2007")
        {
        }

        public SitemapType(string initValue) : base("sitemap-type", "wt", "http://schemas.google.com/webmasters/tools/2007", initValue)
        {
        }
    }
}

