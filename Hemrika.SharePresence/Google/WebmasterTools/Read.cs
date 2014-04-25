namespace Hemrika.SharePresence.Google.WebmasterTools
{
    using Extensions;
    using System;

    public class Read : SimpleElement
    {
        public Read() : base("read", "wt", "http://schemas.google.com/webmasters/tools/2007")
        {
        }

        public Read(string initValue) : base("read", "wt", "http://schemas.google.com/webmasters/tools/2007", initValue)
        {
        }
    }
}

