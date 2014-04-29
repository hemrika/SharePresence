// -----------------------------------------------------------------------
// <copyright file="PageLayoutRepository.cs" company="Microsoft">
// TODO: Update copyright text.
// </copyright>
// -----------------------------------------------------------------------

namespace Hemrika.SharePresence.WebSite.MetaData.Keywords
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using Microsoft.SharePoint;
    using Hemrika.SharePresence.Common.ListRepository;
    using System.Threading;
    using System.IO;
    using Microsoft.SharePoint.Utilities;
    using Hemrika.SharePresence.WebSite.ContentTypes;

    /// <summary>
    /// TODO: Update summary.
    /// </summary>
    public class KeywordRepository : IKeywordRepository
    {
        private static ReaderWriterLockSlim rrLock = new ReaderWriterLockSlim(LockRecursionPolicy.SupportsRecursion);
        private IList<Noise> keywords;
        static DateTime lastLoad = DateTime.Now.AddDays(-1);
        static int cacheInterval = 10;

        public IList<Noise> GetKeywords()
        {
            rrLock.EnterUpgradeableReadLock();

            try
            {
                if ((keywords == null) || (DateTime.Now.Subtract(lastLoad).TotalSeconds) > cacheInterval)
                {
                    rrLock.EnterWriteLock();
                    try
                    {

                        if (keywords == null)
                        {
                            keywords = new List<Noise>();
                            //SPWeb currentWeb = SPContext.Current.Web;
                            //SPWeb rootWeb = currentWeb.Site.RootWeb;

                            SPList keywordsList = SPContext.Current.Site.RootWeb.Lists["Keyword"];

                            SPQuery oQuery = new SPQuery();
                            oQuery.Query = string.Format("<Where><IsNotNull><FieldRef Name=\"Title\" /></IsNotNull></Where>");
                            //oQuery.Query = string.Format("<Where><Contains><FieldRef Name=\"FileLeafRef\" /><Value Type=\"File\">.aspx</Value></Contains></Where><OrderBy><FieldRef Name=\"FileLeafRef\" /></OrderBy>");
                            SPListItemCollection colListItems = keywordsList.GetItems(oQuery);

                            foreach (SPListItem keywordItem in colListItems)
                            {
                                try
                                {
                                    Noise words = new Noise();
                                    words.ID = keywordItem.ID;
                                    words.Title =keywordItem.Title;
                                    words.UniqueId = keywordItem.UniqueId;
                                    SPFieldCollection fields = keywordItem.Fields;
                                    if (fields.ContainsField("Keywords"))
                                    {
                                        if (keywordItem["Keywords"] != null)
                                        {
                                            words.Words = keywordItem["Keywords"].ToString();
                                        }
                                    }
                                    else
                                    {
                                        words.Words = string.Empty;
                                    }

                                    keywords.Add(words);
                                }
                                catch (Exception ex)
                                {
                                    ex.ToString();
                                }
                            }
                        }
                    }
                    finally
                    {
                        rrLock.ExitWriteLock();
                    }
                }
                return keywords;
            }
            finally
            {
                rrLock.ExitUpgradeableReadLock();
            }

        }

        public List<Noise> Keywords()
        {
            List<Noise> _keywords = new List<Noise>();
            IList<Noise> ikeywords = GetKeywords();

            if (ikeywords == null)
                //return _found;
                return _keywords;

            rrLock.EnterReadLock();

            try
            {
                foreach (Noise keyword in ikeywords)
                {
                   _keywords.Add(keyword);
                }

                return _keywords;
            }
            finally
            {
                rrLock.ExitReadLock();
            }

        }
    }
}
