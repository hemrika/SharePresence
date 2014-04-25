namespace Hemrika.SharePresence.Google.WebmasterTools
{
    using Extensions;
    using System;

    public class IssueDetail : SimpleElement
    {
        public IssueDetail() : base("issue-detail", "wt", "http://schemas.google.com/webmasters/tools/2007")
        {
        }

        public IssueDetail(string initValue) : base("issue-detail", "wt", "http://schemas.google.com/webmasters/tools/2007", initValue)
        {
        }
    }
}

