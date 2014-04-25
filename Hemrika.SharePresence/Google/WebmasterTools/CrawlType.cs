namespace Hemrika.SharePresence.Google.WebmasterTools
{
    using Extensions;
    using System;

    public class CrawlType : SimpleElement
    {
        public CrawlType() : base("crawl-type", "wt", "http://schemas.google.com/webmasters/tools/2007")
        {
        }

        public CrawlType(string initValue) : base("crawl-type", "wt", "http://schemas.google.com/webmasters/tools/2007", initValue)
        {
        }
    }
}

