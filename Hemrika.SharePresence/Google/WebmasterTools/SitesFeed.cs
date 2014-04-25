namespace Hemrika.SharePresence.Google.WebmasterTools
{
    using Client;
    using System;

    public class SitesFeed : AbstractFeed
    {
        public SitesFeed(Uri uriBase, IService service) : base(uriBase, service)
        {
        }

        public override AtomEntry CreateFeedEntry()
        {
            return new SitesEntry();
        }

        protected override void HandleExtensionElements(ExtensionElementEventArgs e, AtomFeedParser parser)
        {
            base.HandleExtensionElements(e, parser);
        }
    }
}

