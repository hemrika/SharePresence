namespace Hemrika.SharePresence.Google.WebmasterTools
{
    using System;
    using Hemrika.SharePresence.Google.Client;

    public class Keywords : Entry
    {
        protected override void EnsureInnerObject()
        {
            if (base.AtomEntry == null)
            {
                base.AtomEntry = new WebmasterTools.KeywordsEntry();
            }
        }

        public WebmasterTools.KeywordsEntry KeywordsEntry
        {
            get
            {
                return (base.AtomEntry as WebmasterTools.KeywordsEntry);
            }
        }
    }
}

