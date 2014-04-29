// -----------------------------------------------------------------------
// <copyright file="PageLayoutRepository.cs" company="Microsoft">
// TODO: Update copyright text.
// </copyright>
// -----------------------------------------------------------------------

namespace Hemrika.SharePresence.WebSite.Layout
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
    using Hemrika.SharePresence.WebSite.Fields;

    /// <summary>
    /// TODO: Update summary.
    /// </summary>
    public class PageLayoutRepository : IPageLayoutRepository
    {
        private static ReaderWriterLockSlim rrLock = new ReaderWriterLockSlim(LockRecursionPolicy.SupportsRecursion);
        private IList<PageLayout> pageLayouts;
        static DateTime lastLoad = DateTime.Now.AddDays(-1);
        static int cacheInterval = 10;

        public IList<PageLayout> GetPageLayouts()
        {
            rrLock.EnterUpgradeableReadLock();

            try
            {
                if ((pageLayouts == null) || (DateTime.Now.Subtract(lastLoad).TotalSeconds) > cacheInterval)
                {
                    rrLock.EnterWriteLock();
                    try
                    {

                        if (pageLayouts == null)
                        {
                            pageLayouts = new List<PageLayout>();
                            //SPWeb currentWeb = SPContext.Current.Web;
                            //SPWeb rootWeb = currentWeb.Site.RootWeb;

                            SPList PageLayoutLib = SPContext.Current.Site.GetCatalog(SPListTemplateType.MasterPageCatalog);

                            SPQuery oQuery = new SPQuery();
                            oQuery.Query = string.Format("<Where><Contains><FieldRef Name=\"FileLeafRef\" /><Value Type=\"File\">.aspx</Value></Contains></Where><OrderBy><FieldRef Name=\"FileLeafRef\" /></OrderBy>");
                            SPListItemCollection colListItems = PageLayoutLib.GetItems(oQuery);

                            foreach (SPListItem pageLayoutItem in colListItems)
                            {
                                try
                                {
                                    PageLayout pageLayout = PageLayout.CreatePageLayout();
                                    pageLayout.UniqueId = pageLayoutItem.UniqueId;
                                    pageLayout.Id = pageLayoutItem.ID;
                                    pageLayout.Name = pageLayoutItem.Name;
                                    pageLayout.Title = pageLayout.Name.Replace(".aspx", "").TrimEnd();
                                    pageLayout.Url = pageLayoutItem.Url;
                                    pageLayout.Icon = "/_catalogs/masterpage/Preview Images/" + pageLayout.Title + "_32.png";
                                    pageLayout.Preview = "/_catalogs/masterpage/Preview Images/" + pageLayout.Title + "_preview.png";
                                    pageLayout.Description = "Description:\n "+pageLayout.Description;
                                    ContentTypeIdFieldValue contentType = pageLayoutItem[BuildFieldId.PublishingAssociatedContentType] as ContentTypeIdFieldValue;
                                    //ContentTypeIdFieldValue contentType = pageLayoutItem["ows_ContentTypeId"] as ContentTypeIdFieldValue;                                    
                                    if (contentType != null)
                                    {
                                        pageLayout.ContenTypeId = contentType.Id;
                                        pageLayout.ContentTypeGroup = contentType.StoredGroup;
                                        pageLayouts.Add(pageLayout);
                                    }
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
                return pageLayouts;
            }
            finally
            {
                rrLock.ExitUpgradeableReadLock();
            }

        }

        public List<PageLayout> GetPageLayouts(SPContentTypeId contentTypeId)
        {
            List<PageLayout> pagelayouts = new List<PageLayout>();
            PageLayout _found = PageLayout.CreatePageLayout();
            IList<PageLayout> layouts = GetPageLayouts();

            if (layouts == null)
                //return _found;
                return pagelayouts;

            rrLock.EnterReadLock();

            try
            {
                foreach (PageLayout layout in layouts)
                {
                    if (layout.ContenTypeId.Equals(contentTypeId))
                    {
                        pagelayouts.Add(layout);
                        //_found = layout;
                        //break;
                    }
                }

                return pagelayouts;// _found;
            }
            finally
            {
                rrLock.ExitReadLock();
            }

        }

        public PageLayout GetPageLayout(Guid UniqueId)
        {
            PageLayout _found = PageLayout.CreatePageLayout();
            IList<PageLayout> layouts = GetPageLayouts();

            if (layouts == null)
                return _found;

            rrLock.EnterReadLock();

            try
            {
                foreach (PageLayout layout in layouts)
                {
                    if (layout.UniqueId.Equals(UniqueId))
                    {
                        _found = layout;
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
        public PageLayout GetPageLayout(int Id)
        {
            PageLayout _found = PageLayout.CreatePageLayout();
            IList<PageLayout> layouts = GetPageLayouts();

            if (layouts == null)
                return _found;

            rrLock.EnterReadLock();

            try
            {
                foreach (PageLayout layout in layouts)
                {
                    if(layout.Id.Equals(Id))
                    {
                        _found = layout;
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

        public PageLayout GetPageLayout(string Name)
        {
            PageLayout _found = PageLayout.CreatePageLayout();
            IList<PageLayout> layouts = GetPageLayouts();

            if (layouts == null)
                return _found;

            rrLock.EnterReadLock();

            try
            {
                foreach (PageLayout layout in layouts)
                {
                    if (layout.Name.Equals(Name))
                    {
                        _found = layout;
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

        public string GetPageLayoutContent(Guid UniqueId)
        {
            PageLayout layout = GetPageLayout(UniqueId);
            return GetPageLayoutContent(layout);
        }

        public string GetPageLayoutContent(int Id)
        {
            PageLayout layout = GetPageLayout(Id);
            return GetPageLayoutContent(layout);
        }

        public string GetPageLayoutContent(string Name)
        {
            PageLayout layout = GetPageLayout(Name);
            return GetPageLayoutContent(layout);
        }

        private string GetPageLayoutContent(PageLayout layout)
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
    }
}
