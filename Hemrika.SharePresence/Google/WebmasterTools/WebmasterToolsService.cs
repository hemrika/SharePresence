namespace Hemrika.SharePresence.Google.WebmasterTools
{
    using Client;
    using System;

    public class WebmasterToolsService : Service
    {
        public const string GWebmasterToolsService = "sitemaps";

        public WebmasterToolsService(string applicationName) : base("sitemaps", applicationName)
        {
            base.NewFeed += new ServiceEventHandler(this.OnNewFeed);
        }

        protected override void InitVersionInformation()
        {
            base.ProtocolMajor = 2;
        }

        protected void OnNewFeed(object sender, ServiceEventArgs e)
        {
            //Tracing.TraceMsg("Created new Webmaster Tools Feed");
            if (e == null)
            {
                throw new ArgumentNullException("e");
            }
            if ((e.Uri.AbsolutePath.IndexOf("feeds") != -1) && (e.Uri.AbsolutePath.IndexOf("keywords") != -1))
            {
                e.Feed = new KeywordsFeed(e.Uri, e.Service);
            }
            else if (e.Uri.AbsolutePath.IndexOf("feeds/messages/") != -1)
            {
                e.Feed = new MessagesFeed(e.Uri, e.Service);
            }
            else if (e.Uri.AbsolutePath.IndexOf("feeds/sites/") != -1)
            {
                e.Feed = new SitesFeed(e.Uri, e.Service);
            }
            else if ((e.Uri.AbsolutePath.IndexOf("feeds") != -1) && (e.Uri.AbsolutePath.IndexOf("crawlissues") != -1))
            {
                e.Feed = new CrawlIssuesFeed(e.Uri, e.Service);
            }
            else if ((e.Uri.AbsolutePath.IndexOf("feeds") != -1) && (e.Uri.AbsolutePath.IndexOf("sitemaps") != -1))
            {
                e.Feed = new SitemapsFeed(e.Uri, e.Service);
            }
        }

        public CrawlIssuesFeed Query(CrawlIssuesQuery feedQuery)
        {
            return (base.Query(feedQuery) as CrawlIssuesFeed);
        }

        public KeywordsFeed Query(KeywordsQuery feedQuery)
        {
            return (base.Query(feedQuery) as KeywordsFeed);
        }

        public MessagesFeed Query(MessagesQuery feedQuery)
        {
            return (base.Query(feedQuery) as MessagesFeed);
        }

        public SitemapsFeed Query(SitemapsQuery feedQuery)
        {
            return (base.Query(feedQuery) as SitemapsFeed);
        }

        public SitesFeed Query(SitesQuery feedQuery)
        {
            return (base.Query(feedQuery) as SitesFeed);
        }
    }
}

