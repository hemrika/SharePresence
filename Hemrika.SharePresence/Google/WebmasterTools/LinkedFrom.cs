namespace Hemrika.SharePresence.Google.WebmasterTools
{
    using Extensions;
    using System;

    public class LinkedFrom : SimpleElement
    {
        public LinkedFrom() : base("linked-from", "wt", "http://schemas.google.com/webmasters/tools/2007")
        {
        }

        public LinkedFrom(string initValue) : base("linked-from", "wt", "http://schemas.google.com/webmasters/tools/2007", initValue)
        {
        }
    }
}

