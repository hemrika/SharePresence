namespace Hemrika.SharePresence.Google.WebmasterTools
{
    using Extensions;
    using System;

    public class SitemapMobileMarkupLanguage : SimpleElement
    {
        public SitemapMobileMarkupLanguage() : base("sitemap-mobile-markup-language", "wt", "http://schemas.google.com/webmasters/tools/2007")
        {
        }

        public SitemapMobileMarkupLanguage(string initValue) : base("sitemap-mobile-markup-language", "wt", "http://schemas.google.com/webmasters/tools/2007", initValue)
        {
        }
    }
}

