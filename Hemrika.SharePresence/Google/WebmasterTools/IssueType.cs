namespace Hemrika.SharePresence.Google.WebmasterTools
{
    using Extensions;
    using System;

    public class IssueType : SimpleElement
    {
        public IssueType() : base("issue-type", "wt", "http://schemas.google.com/webmasters/tools/2007")
        {
        }

        public IssueType(string initValue) : base("issue-type", "wt", "http://schemas.google.com/webmasters/tools/2007", initValue)
        {
        }
    }
}

