namespace Hemrika.SharePresence.Google.WebmasterTools
{
    using Client;
    using System;

    public class KeywordsFeed : AbstractFeed
    {
        public KeywordsFeed(Uri uriBase, IService service) : base(uriBase, service)
        {
        }

        public override AtomEntry CreateFeedEntry()
        {
            return new KeywordsEntry();
        }

        protected override void HandleExtensionElements(ExtensionElementEventArgs e, AtomFeedParser parser)
        {
            base.HandleExtensionElements(e, parser);
        }
    }
}

