namespace Hemrika.SharePresence.Google.WebmasterTools
{
    using Client;
    using System;

    public class MessagesFeed : AbstractFeed
    {
        public MessagesFeed(Uri uriBase, IService service) : base(uriBase, service)
        {
        }

        public override AtomEntry CreateFeedEntry()
        {
            return new MessagesEntry();
        }

        protected override void HandleExtensionElements(ExtensionElementEventArgs e, AtomFeedParser parser)
        {
            base.HandleExtensionElements(e, parser);
        }
    }
}

