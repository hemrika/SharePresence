using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.SharePoint;
using System.Web.Script.Serialization;
using Hemrika.SharePresence.Google.Analytics;

namespace Hemrika.SharePresence.Google
{
    [Serializable]
    public class GoogleSettings : IDisposable
    {

        public GoogleSettings()
        {

        }

        private string _APIkey;

        public string APIkey
        {
            get
            {
                if (string.IsNullOrEmpty(_APIkey))
                {
                    return "<APIkey>";
                }
                else
                {
                    return _APIkey;
                }
            }
            set { _APIkey = value; }
        }

        private string _Username;

        public string Username
        {
            get { return _Username; }
            set { _Username = value; }
        }

        private string _Password;

        public string Password
        {
            get { return _Password; }
            set { _Password = value; }
        }

        private GoogleWebmasterToolsSettings[] _WebmasterTools;

        public GoogleWebmasterToolsSettings[] WebmasterTools
        {
            get
            {
                if (_WebmasterTools == null)
                {
                    _WebmasterTools = new GoogleWebmasterToolsSettings[0];
                }

                return _WebmasterTools;
            }
            set { _WebmasterTools = value; }
        }

        private GoogleWebmasterToolsSettings _Webmaster;

        public GoogleWebmasterToolsSettings Webmaster
        {
            get
            {
                if (_Webmaster == null)
                {
                    foreach (GoogleWebmasterToolsSettings currentitem in WebmasterTools)
                    {
                        Uri domain = new Uri(currentitem.Domain);
                        Uri uri = new Uri(SPContext.Current.Site.Url);
                        if (uri.Host == domain.Host)
                        {
                            _Webmaster = currentitem;
                            break;
                        }
                    }

                    if (_Webmaster == null)
                    {
                        foreach (GoogleWebmasterToolsSettings currentitem in WebmasterTools)
                        {
                            if ("http://www.hemrika.nl/" == currentitem.Domain)
                            {
                                _Webmaster = currentitem;
                                break;
                            }
                        }

                        if (_Webmaster == null)
                        {
                            _Webmaster = new GoogleWebmasterToolsSettings();
                        }
                    }

                }
                return _Webmaster;
            }
            set { _Webmaster = value; }
        }

        private GoogleAnalyticsSettings[] _Analytics;

        public GoogleAnalyticsSettings[] Analytics
        {
            get
            {
                if (_Analytics == null)
                {
                    _Analytics = new GoogleAnalyticsSettings[0];
                }

                return _Analytics;
            }
            set { _Analytics = value; }
        }

        private GoogleAnalyticsSettings _Current;

        public GoogleAnalyticsSettings Current
        {
            get
            {
                if (_Current == null)
                {
                    foreach (GoogleAnalyticsSettings currentitem in Analytics)
                    {
                        if (currentitem.WebsiteUrl != null)
                        {
                            if (currentitem.WebsiteUrl.IndexOf(SPContext.Current.Site.HostName) != -1)// == currentitem.WebsiteUrl)
                            {
                                _Current = currentitem;
                                break;
                            }
                        }
                    }

                    if (_Current == null)
                    {
                        foreach (GoogleAnalyticsSettings currentitem in Analytics)
                        {
                            if (currentitem.WebsiteUrl != null )
                            {
                                if (currentitem.WebsiteUrl.IndexOf("www.hemrika.nl") != -1)
                                {
                                    _Current = currentitem;
                                    break;
                                }
                            }
                        }

                        if (_Current == null)
                        {
                            _Current = new GoogleAnalyticsSettings();
                        }
                    }
                }

                return _Current;
            }
        }

        private bool _AnalyticsAllowed;

        public bool AnalyticsAllowed
        {
            get { return _AnalyticsAllowed; }
            set { _AnalyticsAllowed = value; }
        }

        private bool _WebmastertoolsAllowed;

        public bool WebmastertoolsAllowed
        {
            get { return _WebmastertoolsAllowed; }
            set { _WebmastertoolsAllowed = value; }
        }

        public void Remove()//(SPSite site)
        {
            SPSite site = SPContext.Current.Site;
            SPWeb rootWeb = site.RootWeb;
            rootWeb.AllowUnsafeUpdates = true;
            if (rootWeb.AllProperties.ContainsKey("SharePresenceGoogleSettings"))
            {
                rootWeb.AllProperties.Remove("SharePresenceGoogleSettings");
            }
            if (rootWeb.AllProperties.ContainsKey("SharePresenceGoogleAnalyticsSettings"))
            {
                rootWeb.AllProperties.Remove("SharePresenceGoogleAnalyticsSettings");
            }

            rootWeb.Update();
            rootWeb.AllowUnsafeUpdates = false;
        }

        public GoogleSettings Save()//(SPSite site)
        {
            SPSite site = SPContext.Current.Site;
            string value = new JavaScriptSerializer().Serialize(this);
            SPWeb rootWeb = site.RootWeb;
            rootWeb.AllowUnsafeUpdates = true;
            rootWeb.AllProperties["SharePresenceGoogleSettings"] = value;
            rootWeb.Update();
            rootWeb.AllowUnsafeUpdates = false;
            return this;
        }

        public GoogleSettings Load() //(SPSite site)
        {
            SPSite site = SPContext.Current.Site;
            SPWeb rootWeb = site.RootWeb;

            if (rootWeb.AllProperties.ContainsKey("SharePresenceGoogleSettings"))
            {
                string value = rootWeb.AllProperties["SharePresenceGoogleSettings"] as string;
                if (!string.IsNullOrEmpty(value))
                {
                    try
                    {
                        GoogleSettings settings = new JavaScriptSerializer().Deserialize<GoogleSettings>(value);
                        if (settings != null)
                        {
                            /*
                            this.Password = settings.Password;
                            this.Username = settings.Username;
                            this.Analytics = settings.Analytics;
                            this.WebmasterTools = settings.WebmasterTools;
                            this.AnalyticsAllowed = settings.AnalyticsAllowed;
                            this.WebmastertoolsAllowed = settings.WebmastertoolsAllowed;
                            */
                            return settings;
                        }
                    }
                    catch
                    {
                        return new GoogleSettings();
                    }
                }
            }

            return this;
        }

                private bool _disposed = false;

        protected virtual void Dispose(bool disposing)
        {
            if (!_disposed)
            {
                if (disposing)
                {
                    _Webmaster.Dispose();
                }
                _disposed = true;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        ~GoogleSettings()
        {
            Dispose(false);
        }
    }
}
