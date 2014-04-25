namespace Hemrika.SharePresence.Google.WebmasterTools
{
    using Client;
    using System;

    public class CrawlIssuesFeed : AbstractFeed
    {
        public CrawlIssuesFeed(Uri uriBase, IService service) : base(uriBase, service)
        {
        }

        public override AtomEntry CreateFeedEntry()
        {
            return new CrawlIssuesEntry();
        }

        protected override void HandleExtensionElements(ExtensionElementEventArgs e, AtomFeedParser parser)
        {
            base.HandleExtensionElements(e, parser);
        }
    }
}

