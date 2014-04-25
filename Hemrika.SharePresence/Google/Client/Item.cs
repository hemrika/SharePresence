// -----------------------------------------------------------------------
// <copyright file="Item.cs" company="">
// TODO: Update copyright text.
// </copyright>
// -----------------------------------------------------------------------

namespace Hemrika.SharePresence.Google.Client
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;

    /// <summary>
    /// TODO: Update summary.
    /// </summary>
    public class Item
    {
        public string id { get; set; }
        public string kind { get; set; }
        public string selfLink { get; set; }
        public string name { get; set; }
        public string accountId { get; set; }
        public string internalWebPropertyId { get; set; }
        public string websiteUrl { get; set; }
        public string webPropertyId { get; set; }
        public string currency { get; set; }
        public string timezone { get; set; }
        public string defaultPage { get; set; }
        public string siteSearchQueryParameters { get; set; }
        public string profileId { get; set; }
        public double value { get; set; }
        public bool active { get; set; }
        public string type { get; set; }
        public string created { get; set; }
        public string updated { get; set; }
        public bool eCommerceTracking { get; set; }
        public ParentLink parentLink { get; set; }
        public ChildLink childLink { get; set; }
        public UrlDestinationDetails urlDestinationDetails { get; set; }

    }

}
