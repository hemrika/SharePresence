namespace Hemrika.SharePresence.Google.WebmasterTools
{
    using System;
    using Hemrika.SharePresence.Google.Client;

    public class CrawlIssues : Entry
    {
        protected override void EnsureInnerObject()
        {
            if (base.AtomEntry == null)
            {
                base.AtomEntry = new CrawlIssuesEntry();
            }
        }

        public WebmasterTools.CrawlIssuesEntry CrawlIssuesEntry
        {
            get
            {
                return (base.AtomEntry as WebmasterTools.CrawlIssuesEntry);
            }
        }
    }
}

