namespace Hemrika.SharePresence.Google.WebmasterTools
{
    using Client;
    using System;

    public class SitemapsFeed : AbstractFeed
    {
        public SitemapsFeed(Uri uriBase, IService service) : base(uriBase, service)
        {
        }

        public override AtomEntry CreateFeedEntry()
        {
            return new SitemapsEntry();
        }

        protected override void HandleExtensionElements(ExtensionElementEventArgs e, AtomFeedParser parser)
        {
            base.HandleExtensionElements(e, parser);
        }
    }
}

