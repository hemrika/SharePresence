namespace Hemrika.SharePresence.Google.WebmasterTools
{
    using Client;
    using System;

    public class MessagesQuery : FeedQuery
    {
        public const string HttpsFeedUrl = "https://www.google.com/webmasters/tools/feeds/messages/";

        public MessagesQuery() : base("https://www.google.com/webmasters/tools/feeds/messages/")
        {
        }

        public MessagesQuery(string queryUri) : base(queryUri)
        {
        }
    }
}

