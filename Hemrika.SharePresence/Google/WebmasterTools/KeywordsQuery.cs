namespace Hemrika.SharePresence.Google.WebmasterTools
{
    using Client;
    using System;
    using System.Web;

    public class KeywordsQuery : FeedQuery
    {
        public KeywordsQuery() : base("https://www.google.com/webmasters/tools/feeds/")
        {
        }

        public KeywordsQuery(string queryUri) : base(queryUri)
        {
        }

        public static string CreateCustomUri(string siteId)
        {
            return Utilities.EncodeSlugHeader("https://www.google.com/webmasters/tools/feeds/" + siteId + "/keywords/");
        }
    }
}

