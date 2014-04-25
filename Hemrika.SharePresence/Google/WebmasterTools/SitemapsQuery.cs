namespace Hemrika.SharePresence.Google.WebmasterTools
{
    using Client;
    using System;

    public class SitemapsQuery : FeedQuery
    {
        public const string HttpsFeedUrl = "https://www.google.com/webmasters/tools/feeds/sitemaps/";

        public SitemapsQuery() : base("https://www.google.com/webmasters/tools/feeds/sitemaps/")
        {
        }

        public SitemapsQuery(string queryUri) : base(queryUri)
        {
        }

        public static string CreateCustomUri(string siteId)
        {
            return Utilities.EncodeSlugHeader("https://www.google.com/webmasters/tools/feeds/" + siteId + "/sitemaps/");
        }
    }
}

