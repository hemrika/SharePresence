// -----------------------------------------------------------------------
// <copyright file="SiteMapSettings.cs" company="">
// TODO: Update copyright text.
// </copyright>
// -----------------------------------------------------------------------

namespace Hemrika.SharePresence.WebSite.SiteMap
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using Microsoft.SharePoint;
    using System.Web.Script.Serialization;
using System.Collections.Specialized;
    using System.Web;

    [Serializable]
    public class SiteMapSettings
    {
        private bool _UseIndex;

        public bool UseIndex
        {
            get { return _UseIndex; }
            set { _UseIndex = value; }
        }

        private bool _UseVideo;

        public bool UseVideo
        {
            get { return _UseVideo; }
            set { _UseVideo = value; }
        }

        private bool _UseNews;

        public bool UseNews
        {
            get { return _UseNews; }
            set { _UseNews = value; }
        }

        private bool _IncludeImages;

        public bool IncludeImages
        {
            get { return _IncludeImages; }
            set { _IncludeImages = value; }
        }

        private bool _IncludeDocuments;

        public bool IncludeDocuments
        {
            get { return _IncludeDocuments; }
            set { _IncludeDocuments = value; }
        }

        private bool _UseMobile;

        public bool UseMobile
        {
            get { return _UseMobile; }
            set { _UseMobile = value; }
        }

        private bool _UseImage;

        public bool UseImage
        {
            get { return _UseImage; }
            set { _UseImage = value; }
        }

        private StringCollection _SearchEngines;

        public StringCollection SearchEngines
        {
            get { return _SearchEngines; }
            set { _SearchEngines = value; }
        }

        private StringCollection _contentTypes;

        public StringCollection ContentTypes
        {
            get { return _contentTypes; }
            set { _contentTypes = value; }
        }

        public void Remove(SPSite site)
        {
            SPWeb rootWeb = site.RootWeb;
            rootWeb.AllowUnsafeUpdates = true;
            if (rootWeb.AllProperties.ContainsKey("SharePresenceSiteMapSettings"))
            {
                rootWeb.AllProperties.Remove("SharePresenceSiteMapSettings");
            }
            rootWeb.Update();
            rootWeb.AllowUnsafeUpdates = false;
        }

        public SiteMapSettings Save()
        {
            string value = new JavaScriptSerializer().Serialize(this);
            SPWeb rootWeb = SPContext.Current.Site.RootWeb;
            rootWeb.AllowUnsafeUpdates = true;
            if (rootWeb.AllProperties.ContainsKey("SharePresenceSiteMapSettings"))
            {
                rootWeb.AllProperties["SharePresenceSiteMapSettings"] = value;
            }
            else
            {
                rootWeb.AddProperty("SharePresenceSiteMapSettings", value);
            }

            rootWeb.Update();
            rootWeb.AllowUnsafeUpdates = false;
            return this;
        }

        public SiteMapSettings Load()
        {
            SPWeb rootWeb;
            bool dispose = false;
            if (SPContext.Current != null)
            {
                rootWeb = SPContext.Current.Site.RootWeb;
            }
            else
            {
                HttpContext context = HttpContext.Current;
                SPSite site = new SPSite(context.Request.Url.ToString());
                rootWeb = site.RootWeb;
                dispose = true;
            }

            if (rootWeb.AllProperties.ContainsKey("SharePresenceSiteMapSettings"))
            {
                string value = rootWeb.AllProperties["SharePresenceSiteMapSettings"] as string;
                if (!string.IsNullOrEmpty(value))
                {
                    try
                    {
                        SiteMapSettings settings = new JavaScriptSerializer().Deserialize<SiteMapSettings>(value);
                        if (settings != null)
                        {
                            if (dispose) { rootWeb.Dispose(); };

                            return settings;
                        }
                    }
                    catch
                    {
                        return new SiteMapSettings();
                    }
                }
            }
            else
            {
                this.ContentTypes = new StringCollection();
                this.SearchEngines = new StringCollection();
                this.SearchEngines.Add("http://www.google.com/webmasters/tools/ping?sitemap=");
                this.SearchEngines.Add("http://submissions.ask.com/ping?sitemap=");
                this.SearchEngines.Add("http://www.bing.com/webmaster/ping.aspx?siteMap=");
                this.SearchEngines.Add("http://webmaster.live.com/ping.aspx?siteMap=");
                this.UseIndex = true;
                this.UseNews = true;
                if (HttpContext.Current.User.Identity.IsAuthenticated)
                {
                    Save();// (site);
                }
            }

            return this;
        }

        internal SiteMapSettings Load(SPSite site)
        {
            SPWeb rootWeb = site.RootWeb;
            bool dispose = false;
            if (rootWeb.AllProperties.ContainsKey("SharePresenceSiteMapSettings"))
            {
                string value = rootWeb.AllProperties["SharePresenceSiteMapSettings"] as string;
                if (!string.IsNullOrEmpty(value))
                {
                    try
                    {
                        SiteMapSettings settings = new JavaScriptSerializer().Deserialize<SiteMapSettings>(value);
                        if (settings != null)
                        {
                            if (dispose) { rootWeb.Dispose(); };

                            return settings;
                        }
                    }
                    catch
                    {
                        return new SiteMapSettings();
                    }
                }
            }
            else
            {
                this.ContentTypes = new StringCollection();
                this.SearchEngines = new StringCollection();
                this.SearchEngines.Add("http://www.google.com/webmasters/tools/ping?sitemap=");
                this.SearchEngines.Add("http://submissions.ask.com/ping?sitemap=");
                this.SearchEngines.Add("http://www.bing.com/webmaster/ping.aspx?siteMap=");
                this.SearchEngines.Add("http://webmaster.live.com/ping.aspx?siteMap=");
                this.UseIndex = true;
                this.UseNews = true;
                if (HttpContext.Current.User.Identity.IsAuthenticated)
                {
                    Save();// (site);
                }
            }

            return this;
        }
    }
}
