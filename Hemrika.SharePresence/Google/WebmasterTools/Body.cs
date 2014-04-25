namespace Hemrika.SharePresence.Google.WebmasterTools
{
    using Extensions;
    using System;

    public class Body : SimpleElement
    {
        public Body() : base("body", "wt", "http://schemas.google.com/webmasters/tools/2007")
        {
        }

        public Body(string initValue) : base("body", "wt", "http://schemas.google.com/webmasters/tools/2007", initValue)
        {
        }
    }
}

