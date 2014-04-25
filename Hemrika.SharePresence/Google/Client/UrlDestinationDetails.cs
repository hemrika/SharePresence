// -----------------------------------------------------------------------
// <copyright file="UrlDestinationDetails.cs" company="">
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
    public class UrlDestinationDetails
    {
        public string url { get; set; }
        public bool caseSensitive { get; set; }
        public string matchType { get; set; }
        public bool firstStepRequired { get; set; }
    }
}
