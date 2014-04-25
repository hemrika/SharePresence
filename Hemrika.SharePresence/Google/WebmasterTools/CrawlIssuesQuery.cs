namespace Hemrika.SharePresence.Google.WebmasterTools
{
    using Client;
    using System;

    public class CrawlIssuesQuery : FeedQuery
    {
        public const string HttpsFeedUrl = "https://www.google.com/webmasters/tools/feeds/siteID/crawlissues/";

        public CrawlIssuesQuery() : base("https://www.google.com/webmasters/tools/feeds/siteID/crawlissues/")
        {
        }

        public CrawlIssuesQuery(string queryUri) : base(queryUri)
        {
        }

        public static string CreateCustomUri(string siteID)
        {
            return Utilities.EncodeSlugHeader("https://www.google.com/webmasters/tools/feeds/" + siteID + "/crawlissues/");
        }
    }
}

