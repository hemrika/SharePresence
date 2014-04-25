namespace Hemrika.SharePresence.Google.WebmasterTools
{
    using Extensions;
    using System;

    public class PreferredDomain : SimpleElement
    {
        public PreferredDomain() : base("preferred-domain", "wt", "http://schemas.google.com/webmasters/tools/2007")
        {
        }

        public PreferredDomain(string initValue) : base("preferred-domain", "wt", "http://schemas.google.com/webmasters/tools/2007", initValue)
        {
        }
    }
}

