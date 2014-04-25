namespace Hemrika.SharePresence.Google.WebmasterTools
{
    using Extensions;
    using System;

    public class Date : SimpleElement
    {
        public Date() : base("date", "wt", "http://schemas.google.com/webmasters/tools/2007")
        {
        }

        public Date(string initValue) : base("date", "wt", "http://schemas.google.com/webmasters/tools/2007", initValue)
        {
        }
    }
}

