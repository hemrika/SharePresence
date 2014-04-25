namespace Hemrika.SharePresence.Google.WebmasterTools
{
    using Client;
    using System;

    public class SitesQuery : FeedQuery
    {
        public const string HttpsFeedUrl = "https://www.google.com/webmasters/tools/feeds/sites/";

        public SitesQuery() : base("https://www.google.com/webmasters/tools/feeds/sites/")
        {
        }

        public SitesQuery(string queryUri) : base(queryUri)
        {
        }

        public static Uri CreateCustomUri(string siteId)
        {
            siteId = Utilities.EncodeString(siteId);
            siteId = Utilities.UriEncodeReserved(siteId);
            siteId = Utilities.UriEncodeReserved(siteId);
            siteId = Utilities.EncodeSlugHeader("https://www.google.com/webmasters/tools/feeds/sites/" + siteId);
            return new Uri(siteId);
        }
    }
}

