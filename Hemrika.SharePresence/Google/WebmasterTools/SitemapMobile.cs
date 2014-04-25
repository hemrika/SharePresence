namespace Hemrika.SharePresence.Google.WebmasterTools
{
    using Extensions;
    using System;

    public class SitemapMobile : SimpleElement
    {
        public SitemapMobile() : base("sitemap-mobile", "wt", "http://schemas.google.com/webmasters/tools/2007")
        {
        }

        public SitemapMobile(string initValue) : base("sitemap-mobile", "wt", "http://schemas.google.com/webmasters/tools/2007", initValue)
        {
        }
    }
}

