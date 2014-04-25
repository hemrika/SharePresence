namespace Hemrika.SharePresence.Google.WebmasterTools
{
    using Extensions;
    using System;

    public class DateDetected : SimpleElement
    {
        public DateDetected() : base("date-detected", "wt", "http://schemas.google.com/webmasters/tools/2007")
        {
        }

        public DateDetected(string initValue) : base("date-detected", "wt", "http://schemas.google.com/webmasters/tools/2007", initValue)
        {
        }
    }
}

