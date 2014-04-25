namespace Hemrika.SharePresence.Google.WebmasterTools
{
    using Extensions;
    using System;

    public class Verified : SimpleElement
    {
        public Verified() : base("verified", "wt", "http://schemas.google.com/webmasters/tools/2007")
        {
        }

        public Verified(string initValue) : base("verified", "wt", "http://schemas.google.com/webmasters/tools/2007", initValue)
        {
        }
    }
}

