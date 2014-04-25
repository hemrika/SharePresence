namespace Hemrika.SharePresence.Google.WebmasterTools
{
    using Extensions;
    using System;

    public class Keyword : SimpleNameValueAttribute
    {
        public Keyword() : base("keyword", "wt", "http://schemas.google.com/webmasters/tools/2007")
        {
            base.Attributes.Add("source", null);
        }

        public Keyword(string value) : base("keyword", "wt", "http://schemas.google.com/webmasters/tools/2007")
        {
            base.Attributes.Add("source", value);
        }
    }
}

