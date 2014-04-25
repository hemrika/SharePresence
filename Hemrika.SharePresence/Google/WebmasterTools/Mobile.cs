namespace Hemrika.SharePresence.Google.WebmasterTools
{
    using Extensions;
    using System;

    public class Mobile : SimpleElement
    {
        public Mobile() : base("mobile", "mobile", "http://schemas.google.com/webmasters/tools/2007")
        {
        }

        public Mobile(string initValue) : base("mobile", "mobile", "http://schemas.google.com/webmasters/tools/2007", initValue)
        {
        }
    }
}

