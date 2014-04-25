// -----------------------------------------------------------------------
// <copyright file="webproperties.cs" company="">
// TODO: Update copyright text.
// </copyright>
// -----------------------------------------------------------------------

namespace Hemrika.SharePresence.Google.Analytics
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using Hemrika.SharePresence.Google.Client;

    /// <summary>
    /// TODO: Update summary.
    /// </summary>
    public class WebPropertiesQuery : FeedQuery
    {
        /// <summary>
        /// Analytics account service url, http and https 
        /// </summary>
        
        public const string HttpsFeedUrl = "https://www.googleapis.com/analytics/v2.4/management/accounts";
        /// <summary>
        /// default constructor, does nothing 
        /// </summary>
        public WebPropertiesQuery()
            : base(HttpsFeedUrl)
        {
        }

        /// <summary>
        /// base constructor, with initial queryUri
        /// </summary>
        /// <param name="queryUri">the query to use</param>
        public WebPropertiesQuery(string queryUri)
        : base(queryUri)
        {
        }
    }
}
