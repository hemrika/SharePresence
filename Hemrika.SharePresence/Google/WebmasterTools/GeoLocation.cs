namespace Hemrika.SharePresence.Google.WebmasterTools
{
    using Extensions;
    using System;

    public class GeoLocation : SimpleElement
    {
        public GeoLocation() : base("geolocation", "wt", "http://schemas.google.com/webmasters/tools/2007")
        {
        }

        public GeoLocation(string initValue) : base("geolocation", "wt", "http://schemas.google.com/webmasters/tools/2007", initValue)
        {
        }
    }
}

