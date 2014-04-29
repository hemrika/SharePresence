// -----------------------------------------------------------------------
// <copyright file="PageLayoutRepository.cs" company="Microsoft">
// TODO: Update copyright text.
// </copyright>
// -----------------------------------------------------------------------

namespace Hemrika.SharePresence.WebSite.Master
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
    public class MasterPageRepository : IMasterPageRepository
    {
        private static ReaderWriterLockSlim rrLock = new ReaderWriterLockSlim(LockRecursionPolicy.SupportsRecursion);
        private IList<MasterPage> masterPages;
        static DateTime lastLoad = DateTime.Now.AddDays(-1);
        static int cacheInterval = 10;

        #region Get Options
        
        private IList<MasterPage> GetMasters()
        {
            rrLock.EnterUpgradeableReadLock();

            try
            {
                //Attempt to reload if the settings store is null and a load hasn't been attempted, or if the last load interval is exceeded.
                //in the case of a refresh, then _settingStore will be null and missingSettings will be false.
                if ((masterPages == null) || (DateTime.Now.Subtract(lastLoad).TotalSeconds) > cacheInterval)
                {
                    rrLock.EnterWriteLock();
                    try
                    {

                        if (masterPages == null)
                        {
                            masterPages = new List<MasterPage>();
                            SPWeb currentWeb = SPContext.Current.Web;
                            SPWeb rootWeb = currentWeb.Site.RootWeb;

                            SPList MasterPageLib = rootWeb.GetCatalog(SPListTemplateType.MasterPageCatalog);//.GetList("_catalogs/masterpage");

                            SPQuery oQuery = new SPQuery();
                            oQuery.Query = string.Format("<Where><Contains><FieldRef Name=\"FileLeafRef\" /><Value Type=\"File\">.master</Value></Contains></Where><OrderBy><FieldRef Name=\"FileLeafRef\" /></OrderBy>");
                            SPListItemCollection colListItems = MasterPageLib.GetItems(oQuery);

                            foreach (SPListItem masterPageItem in colListItems)
                            {
                                MasterPage masterpage = MasterPage.CreateMasterPage();
                                masterpage.UniqueId = masterPageItem.UniqueId;
                                masterpage.Id = masterPageItem.ID;
                                masterpage.Name = masterPageItem.Name;
                                masterpage.Title = masterpage.Name.Replace(".master", "").TrimEnd();
                                masterpage.Description = masterpage.Title + "description";
                                //TODO 
                                //pageLayout.Description = pageLayoutItem["MasterPageDescription"].ToString();
                                masterpage.Url = masterPageItem.Url;

                                masterPages.Add(masterpage);
                            }
                        }
                    }
                    finally
                    {
                        rrLock.ExitWriteLock();
                    }
                }
                return masterPages;
            }
            finally
            {
                rrLock.ExitUpgradeableReadLock();
            }

        }

        public List<MasterPage> GetMasterPages()
        {
            List<MasterPage> masterPages = new List<MasterPage>();
            IList<MasterPage> masters = GetMasters();

            if (masters == null)
                //return _found;
                return masterPages;

            rrLock.EnterReadLock();

            try
            {
                foreach (MasterPage master in masters)
                {
                    masterPages.Add(master);
                }

                return masterPages;// _found;
            }
            finally
            {
                rrLock.ExitReadLock();
            }

        }

        #region Get MasterPage
        public MasterPage GetMasterPage(Guid UniqueId)
        {
            MasterPage _found = MasterPage.CreateMasterPage();
            IList<MasterPage> masters = GetMasterPages();

            if (masters == null)
                return _found;

            rrLock.EnterReadLock();

            try
            {
                foreach (MasterPage master in masters)
                {
                    if (master.UniqueId.Equals(UniqueId))
                    {
                        _found = master;
                        break;
                    }
                }

                return _found;
            }
            finally
            {
                rrLock.ExitReadLock();
            }
        }

        public MasterPage GetMasterPage(int Id)
        {
            MasterPage _found = MasterPage.CreateMasterPage();
            IList<MasterPage> masters = GetMasterPages();

            if (masters == null)
                return _found;

            rrLock.EnterReadLock();

            try
            {
                foreach (MasterPage master in masters)
                {
                    if(master.Id.Equals(Id))
                    {
                        _found = master;
                        break;
                    }
                }

                return _found;
            }
            finally
            {
                rrLock.ExitReadLock();
            }
        }

        public MasterPage GetMasterPage(string Name)
        {
            MasterPage _found = MasterPage.CreateMasterPage();
            IList<MasterPage> masters = GetMasterPages();

            if (masters == null)
                return _found;

            rrLock.EnterReadLock();

            try
            {
                foreach (MasterPage master in masters)
                {
                    if (master.Name.Equals(Name))
                    {
                        _found = master;
                        break;
                    }
                }

                return _found;
            }
            finally
            {
                rrLock.ExitReadLock();
            }

        }
        #endregion

        #region Get MasterPage Content
        public string GetMasterPageContent(Guid UniqueId)
        {
            MasterPage layout = GetMasterPage(UniqueId);
            return GetMasterPageContent(layout);
        }

        public string GetMasterPageContent(int Id)
        {
            MasterPage layout = GetMasterPage(Id);
            return GetMasterPageContent(layout);
        }

        public string GetMasterPageContent(string Name)
        {
            MasterPage layout = GetMasterPage(Name);
            return GetMasterPageContent(layout);
        }

        private string GetMasterPageContent(MasterPage layout)
        {
            string content = String.Empty;

            //SPSecurity.RunWithElevatedPrivileges(delegate()
            //{

                SPSite site = new SPSite(SPContext.Current.Web.Url);

                SPWeb rootWeb = site.RootWeb;

                SPFile pageLayoutFile = rootWeb.GetFile(layout.UniqueId);
                byte[] binFile = pageLayoutFile.OpenBinary();

                MemoryStream m = new MemoryStream(binFile);
                StreamReader reader = new StreamReader(m);
                content = reader.ReadToEnd();
                reader.Close();
                m.Close();

                site.Dispose();
                rootWeb.Dispose();

            //});

            return content;
        }
        #endregion
        #endregion

        /*
        #region Set Options

        #region Set MasterPage

        public void SetMasterPage(Guid UniqueId,bool IncludeSubSites)
        {
        }

        public void SetMasterPage(int Id, bool IncludeSubSites)
        {
        }

        public void SetMasterPage(string Name, bool IncludeSubSites)
        {
        }

        private void SetMasterPage(MasterPage master, bool IncludeSubSites)
        {
        }
        #endregion
        #endregion
        */
    }
}
