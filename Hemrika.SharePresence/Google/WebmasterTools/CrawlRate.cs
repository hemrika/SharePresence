namespace Hemrika.SharePresence.Google.WebmasterTools
{
    using Extensions;
    using System;

    public class CrawlRate : SimpleElement
    {
        public CrawlRate() : base("crawl-rate", "wt", "http://schemas.google.com/webmasters/tools/2007")
        {
        }

        public CrawlRate(string initValue) : base("crawl-rate", "wt", "http://schemas.google.com/webmasters/tools/2007", initValue)
        {
        }
    }
}

