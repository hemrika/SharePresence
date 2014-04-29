using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Permissions;
using System.Text;
using Microsoft.SharePoint;
using System.Web.Script.Serialization;
using System.Web;

namespace Hemrika.SharePresence.WebSite.Bookmark
{
    [Serializable]
    public class BookmarkSettings
    {
        private bool _addAnalytics;

        public bool AddAnalytics
        {
            get { return _addAnalytics; }
            set { _addAnalytics = value; }
        }

        private string _analyticsName;

        /// <summary>
        /// '/share/{r}/{s}'
        /// The page name to be passed to the Google Analytics call. 
        /// Within the name use '{u}' to mark the insertion point for the full URL of the current page, 
        /// '{r}' for the relative URL of the current page (i.e. without the host reference), 
        /// '{t}' for the current page's title, 
        /// '{s}' for the ID of the selected bookmarking site, 
        /// and/or '{n}' for the site's display name.
        /// </summary>
        public string AnalyticsName
        {
            get { return _analyticsName; }
            set { _analyticsName = value; }
        }

        private bool _addFavorite;

        public bool AddFavorite
        {
            get { return _addFavorite; }
            set { _addFavorite = value; }
        }
        private bool _addEmail;

        public bool AddEmail
        {
            get { return _addEmail; }
            set { _addEmail = value; }
        }

        private string _emailSubject;

        public string EmailSubject
        {
            get { return _emailSubject; }
            set { _emailSubject = value; }
        }

        private string _emailBody;

        /// <summary>
        /// The body for the e-mail to be sent out. 
        /// Use '{t}' to mark the position where the title of the current page is inserted, 
        /// '{u}' to mark where the current page's URL should appear, 
        /// '{d}' to mark the location of the description, 
        /// and '\n' to mark new lines.
        /// </summary>
        public string EmailBody
        {
            get { return _emailBody; }
            set { _emailBody = value; }
        }

        private string _hint;
        
        /// <summary>
        /// 'Send to {s}'
        /// A template for popup hints on the site icons. '{s}' is replaced by the site's display name.
        /// </summary>
        public string Hint
        {
            get { return _hint; }
            set { _hint = value; }
        }

        private string _target;

        public string Target
        {
            get { return _target; }
            set { _target = value; }
        }

        private bool _useCommon;

        public bool UseCommon
        {
            get { return _useCommon; }
            set { _useCommon = value; }
        }

        private List<string> _sites;

        public List<string> Sites
        {
            get { return _sites; }
            set { _sites = value; }
        }

        private bool _useExtended;

        public bool UseExtended
        {
            get { return _useExtended; }
            set { _useExtended = value; }
        }

        public void Remove()
        {
            SPWeb rootWeb = SPContext.Current.Site.RootWeb;
            rootWeb.AllowUnsafeUpdates = true;
            if (rootWeb.AllProperties.ContainsKey("SharePresenceBookmarkSettings"))
            {
                rootWeb.AllProperties.Remove("SharePresenceBookmarkSettings");
            }
            rootWeb.Update();
            rootWeb.AllowUnsafeUpdates = false;
        }

        public BookmarkSettings Save()
        {
            string value = new JavaScriptSerializer().Serialize(this);
            SPWeb rootWeb = SPContext.Current.Site.RootWeb;
            rootWeb.AllowUnsafeUpdates = true;
            if (!rootWeb.AllProperties.ContainsKey("SharePresenceBookmarkSettings"))
            {
                rootWeb.AddProperty("SharePresenceBookmarkSettings", value);
            }
            else
            {
                rootWeb.AllProperties["SharePresenceBookmarkSettings"] = value;
            }

            rootWeb.Update();
            rootWeb.AllowUnsafeUpdates = false;
            return this;

        }

        public BookmarkSettings Load()
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

            if (rootWeb.AllProperties.ContainsKey("SharePresenceBookmarkSettings"))
            {
                string value = rootWeb.AllProperties["SharePresenceBookmarkSettings"] as string;
                if (!string.IsNullOrEmpty(value))
                {
                    try
                    {
                        BookmarkSettings settings = new JavaScriptSerializer().Deserialize<BookmarkSettings>(value);
                        if (settings != null)
                        {
                            if (dispose) { rootWeb.Dispose(); };

                            return settings;
                        }
                    }
                    catch
                    {
                        return new BookmarkSettings();
                    }
                }
            }

            return this;
        }

    }
}
