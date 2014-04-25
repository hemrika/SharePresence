namespace Hemrika.SharePresence.Google.WebmasterTools
{
    using Client;
    using System;

    public class CrawlIssuesEntry : WebmasterToolsBaseEntry
    {
        public CrawlIssuesEntry()
        {
            //Tracing.TraceMsg("Created CrawlIssuesEntry");
            base.AddExtension(new WebmasterTools.CrawlType());
            base.AddExtension(new WebmasterTools.IssueType());
            base.AddExtension(new WebmasterTools.IssueDetail());
            base.AddExtension(new WebmasterTools.LinkedFrom());
            base.AddExtension(new WebmasterTools.DateDetected());
        }

        public string CrawlType
        {
            get
            {
                return base.getWebmasterToolsValue("crawl-type");
            }
            set
            {
                base.setWebmasterToolsExtension("crawl-type", value);
            }
        }

        public string DateDetected
        {
            get
            {
                return base.getWebmasterToolsValue("date-detected");
            }
            set
            {
                base.setWebmasterToolsExtension("date-detected", value);
            }
        }

        public string IssueDetail
        {
            get
            {
                return base.getWebmasterToolsValue("issue-detail");
            }
            set
            {
                base.setWebmasterToolsExtension("issue-detail", value);
            }
        }

        public string IssueType
        {
            get
            {
                return base.getWebmasterToolsValue("issue-type");
            }
            set
            {
                base.setWebmasterToolsExtension("issue-type", value);
            }
        }

        public string LinkedFrom
        {
            get
            {
                return base.getWebmasterToolsValue("linked-from");
            }
            set
            {
                base.setWebmasterToolsExtension("linked-from", value);
            }
        }
    }
}

