// -----------------------------------------------------------------------
// <copyright file="RootObject.cs" company="">
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
    public class RootObject
    {
        public string kind { get; set; }
        public string username { get; set; }
        public int totalResults { get; set; }
        public int startIndex { get; set; }
        public int itemsPerPage { get; set; }
        public List<Item> items { get; set; }
    }
}
