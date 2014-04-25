namespace Hemrika.SharePresence.Google.WebmasterTools
{
    using Client;
    using WebmasterTools;
    using System;

    public class Messages : Entry
    {
        protected override void EnsureInnerObject()
        {
            if (base.AtomEntry == null)
            {
                base.AtomEntry = new WebmasterTools.MessagesEntry();
            }
        }

        public WebmasterTools.MessagesEntry MessagesEntry
        {
            get
            {
                return (base.AtomEntry as WebmasterTools.MessagesEntry);
            }
        }
    }
}

