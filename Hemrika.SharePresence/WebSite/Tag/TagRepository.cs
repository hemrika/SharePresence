// -----------------------------------------------------------------------
// <copyright file="PageLayoutRepository.cs" company="Microsoft">
// TODO: Update copyright text.
// </copyright>
// -----------------------------------------------------------------------

namespace Hemrika.SharePresence.WebSite.Tag
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using Microsoft.SharePoint;
    using Hemrika.SharePresence.Common.ListRepository;
    using System.Threading;
    using System.IO;

    /// <summary>
    /// TODO: Update summary.
    /// </summary>
    public class TagRepository : ITagRepository
    {
        private static ReaderWriterLockSlim rrLock = new ReaderWriterLockSlim(LockRecursionPolicy.SupportsRecursion);
        private IList<Tag> tags;
        static DateTime lastLoad = DateTime.Now.AddDays(-1);
        static int cacheInterval = 10;

        private IList<Tag> GetITags()
        {
            rrLock.EnterUpgradeableReadLock();

            try
            {
                //Attempt to reload if the settings store is null and a load hasn't been attempted, or if the last load interval is exceeded.
                //in the case of a refresh, then _settingStore will be null and missingSettings will be false.
                if ((tags == null) || (DateTime.Now.Subtract(lastLoad).TotalSeconds) > cacheInterval)
                {
                    rrLock.EnterWriteLock();
                    try
                    {

                        if (tags == null)
                        {
                            tags = new List<Tag>();
                            SPWeb currentWeb = SPContext.Current.Web;
                            SPWeb rootWeb = currentWeb.Site.RootWeb;

                            SPList MasterPageLib = rootWeb.GetCatalog(SPListTemplateType.MasterPageCatalog);//.GetList("_catalogs/masterpage");

                            SPQuery oQuery = new SPQuery();
                            oQuery.Query = string.Format("<Where><Contains><FieldRef Name=\"FileLeafRef\" /><Value Type=\"File\">.master</Value></Contains></Where><OrderBy><FieldRef Name=\"FileLeafRef\" /></OrderBy>");
                            SPListItemCollection colListItems = MasterPageLib.GetItems(oQuery);

                            foreach (SPListItem masterPageItem in colListItems)
                            {
                                Tag tag = Tag.CreateTag();
                                tag.UniqueId = masterPageItem.UniqueId;
                                tag.Id = masterPageItem.ID;
                                tag.Name = masterPageItem.Name;
                                tag.Title = tag.Name.Replace(".master", "").TrimEnd();
                                tag.Description = tag.Title + "description";
                                //TODO 
                                //pageLayout.Description = pageLayoutItem["MasterPageDescription"].ToString();
                                tag.Url = masterPageItem.Url;

                                tags.Add(tag);
                            }
                        }
                    }
                    finally
                    {
                        rrLock.ExitWriteLock();
                    }
                }
                return tags;
            }
            finally
            {
                rrLock.ExitUpgradeableReadLock();
            }

        }

        public List<Tag> GetTags()
        {
            List<Tag> tags = new List<Tag>();
            IList<Tag> itags = GetITags();

            if (itags == null)
                //return _found;
                return tags;

            rrLock.EnterReadLock();

            try
            {
                foreach (Tag tag in itags)
                {
                    tags.Add(tag);
                }

                return tags;// _found;
            }
            finally
            {
                rrLock.ExitReadLock();
            }

        }
    }
}
