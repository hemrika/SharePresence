// -----------------------------------------------------------------------
// <copyright file="WebSitePagePathProvider.cs" company="">
// TODO: Update copyright text.
// </copyright>
// -----------------------------------------------------------------------

namespace Hemrika.SharePresence.WebSite.Modules.WebPageModule
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Web.Hosting;
    using Microsoft.SharePoint;
    using Hemrika.SharePresence.WebSite.Page;
    using Microsoft.SharePoint.Utilities;
    using Microsoft.SharePoint.WebControls;
    using System.Web;
    using Microsoft.SharePoint.Administration;

    /// <summary>
    /// TODO: Update summary.
    /// </summary>
    public class WebSitePathProvider : VirtualPathProvider
    {
        public WebSitePathProvider()
            : base()
        {
        }

        #region "Methods

        /// <summary>
        ///   Determines whether a specified virtual path is within
        ///   the virtual file system.
        /// </summary>
        /// <param name="absolutePath">An absolute virtual path.</param>
        /// <returns>
        ///   true if the virtual path is within the 
        ///   virtual file sytem; otherwise, false.
        /// </returns>
        private bool IsPathVirtual(string virtualPath)
        {
            string requestUrl = virtualPath.ToLower();

            if ((((((requestUrl.IndexOf("/_") == -1) && (requestUrl.IndexOf("/_layouts/") == -1) && (requestUrl.IndexOf("/forms/") == -1)) && ((requestUrl.IndexOf("/lists/") == -1) && (requestUrl.IndexOf("/_controltemplates/") == -1))) && (requestUrl.IndexOf("/_vti_bin/") == -1)) && (requestUrl.IndexOf("/_wpresources/") == -1) && (requestUrl.IndexOf("/_app_bin/") == -1) && (requestUrl.IndexOf("/app_localresources/") == -1) && (requestUrl.IndexOf("/_catalogs/") == -1) && (requestUrl.IndexOf("/_forms/") == -1) && (requestUrl.IndexOf("/_login/") == -1) && (requestUrl.IndexOf("/_windows/") == -1) && (requestUrl.IndexOf("/_forms/") == -1)))
            {
                return true;
            }
            else
            {
                /*
                if (requestUrl.IndexOf("/_catalogs/") == -1)
                {
                    return true;
                }
                */
                return false;
            }
        }

        public override string CombineVirtualPaths(string basePath, string relativePath)
        {
            if (relativePath.ToLower() == "~masterurl/website.master")
            {
                try
                {
                    return "website.master";
                }
                catch (Exception ex)
                {
                    ex.ToString();
                }
            }

            if (relativePath.ToLower() == "~masterurl/default.master")
            {
                try
                {
                    //SPControl.SetContextSite(HttpContext.Current, web);
                    //string incorrect = SPContext.Current.Site.RootWeb.MasterUrl;
                    //string rootsite = SPContext.Current.Site.RootWeb.Url.Replace(SPContext.Current.Site.ServerRelativeUrl, string.Empty);
                    //string correct = SPUtility.ConcatUrls(rootsite, incorrect);
                    //SPControl.SetContextSite(HttpContext.Current, new SPSite(rootsite));
                    //string correct = "~" + incorrect;
                    return SPContext.Current.Site.RootWeb.CustomMasterUrl;
                }
                catch (Exception ex)
                {
                    ex.ToString();
                }
            }

            if (relativePath.ToLower() == "~masterurl/custom.master")
            {
                try
                {
                    //string incorrect = SPContext.Current.Site.RootWeb.CustomMasterUrl;
                    //string rootsite = SPContext.Current.Site.RootWeb.Url.Replace(SPContext.Current.Site.ServerRelativeUrl,string.Empty);
                    //string correct = SPUtility.ConcatUrls(rootsite, incorrect);
                    //SPControl.SetContextSite(HttpContext.Current, new SPSite(rootsite));
                    //string correct = "~" + incorrect;
                    return SPContext.Current.Site.RootWeb.CustomMasterUrl;
                }
                catch (Exception ex)
                {
                    ex.ToString();
                }
            }

            if (relativePath.ToLower() == "~TemplatePageUrl")
            {
                try
                {
                    //string incorrect = SPContext.Current.Site.RootWeb.WelcomePage;
                    //string corrent = incorrect.Replace(SPContext.Current.Site.ServerRelativeUrl, string.Empty);

                    return SPContext.Current.Site.RootWeb.RootFolder.WelcomePage;
                }
                catch (Exception ex)
                {
                    ex.ToString();
                }
            }
            if (relativePath.ToLower().EndsWith(".master"))
            {
                string what = relativePath;
            }
            
            WebSitePage.DetermineDisplayControlModes();
             return base.Previous.CombineVirtualPaths(basePath, relativePath);
        }

        /// <summary>
        /// Check whether virtual path exists or not.
        /// </summary>
        /// <param name="absolutePath"></param>
        /// <returns></returns>
        public override bool FileExists(string virtualPath)
        {
            bool exists = false;

            try
            {
                if (this.IsPathVirtual(virtualPath))
                {
                    SPSecurity.RunWithElevatedPrivileges(delegate()
                    {
                        SPWeb web = SPContext.Current.Web;
                        SPSite site = SPContext.Current.Site;
                        SPWeb isRootweb = site.OpenWeb(virtualPath);
                        if (!web.Exists && (isRootweb.Exists && web != isRootweb))
                        {
                            web = site.RootWeb;
                        }

                        SPFile file = web.GetFile(virtualPath);
                        exists = (file != null);
                    });
                }
                else
                {
                    exists = base.Previous.FileExists(virtualPath);
                }
            }
            catch { };

            return exists;

        }

        /// <summary>
        /// Check whether virtual directory exists.
        /// </summary>
        /// <param name="virtualDir"></param>
        /// <returns></returns>
        public override bool DirectoryExists(string virtualDir)
        {
            bool exists = false;
            if (IsPathVirtual(virtualDir))
            {
                SPFolder folder = SPContext.Current.Web.GetFolder(virtualDir);
                Guid _guid = folder.ContainingDocumentLibrary;
                SPDocumentLibrary lib = folder.DocumentLibrary;
                exists = (lib != null);
                //return SPContext.Current.Web.Lists.;// this.siteService.PublishingPageDirectoryExists();
            }
            else
            {
                exists = base.Previous.DirectoryExists(virtualDir);
            }
            return exists;
        }

        /// <summary>
        /// Get publishing page from Pages library.
        /// </summary>
        /// <param name="absolutePath"></param>
        /// <returns></returns>
        public override VirtualFile GetFile(string virtualPath)
        {
            VirtualFile file = null;

            try
            {

                if (this.IsPathVirtual(virtualPath))
                {
                    file = new WebSiteFile(virtualPath, this);
                    //return base.Previous.GetFile(virtualPath);
                }
            }
            catch (Exception ex)
            {
                SPDiagnosticsService.Local.WriteTrace(0, new SPDiagnosticsCategory(ex.Source, TraceSeverity.High, EventSeverity.Error), TraceSeverity.High, ex.Message, ex.Data);
                file = base.Previous.GetFile(virtualPath);
            }
            finally
            {
                if (file == null)
                {
                    file = base.Previous.GetFile(virtualPath);
                }
            }
            return file;
        }

        /// <summary>
        /// Get virtual directory instance.
        /// </summary>
        /// <param name="virtualDir"></param>
        /// <returns></returns>
        public override VirtualDirectory GetDirectory(string virtualDir)
        {
            VirtualDirectory directory = null;
            if (this.IsPathVirtual(virtualDir))
            {
                return new WebSiteDirectory(virtualDir, this);
            }
            else
            {
                directory = base.Previous.GetDirectory(virtualDir);
            }
            return directory;
        }

        public override System.Web.Caching.CacheDependency GetCacheDependency(string virtualPath, System.Collections.IEnumerable virtualPathDependencies, DateTime utcStart)
        {
            return base.GetCacheDependency(virtualPath, virtualPathDependencies, utcStart);
        }

        public override string GetCacheKey(string virtualPath)
        {
            return base.GetCacheKey(virtualPath);
        }

        public override string GetFileHash(string virtualPath, System.Collections.IEnumerable virtualPathDependencies)
        {
            return base.GetFileHash(virtualPath, virtualPathDependencies);
        }

        public override int GetHashCode()
        {
            return base.GetHashCode();
        }

        #endregion
    }
}
