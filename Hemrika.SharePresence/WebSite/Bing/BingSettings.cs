using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Permissions;
using System.Text;
using Microsoft.SharePoint;
using System.Web.Script.Serialization;

namespace Hemrika.SharePresence.WebSite.Bing
{
    [Serializable]
    public class BingSettings
    {
        private string _User;

        public string User
        {
            get { return _User; }
            set { _User = value; }
        }

        private string _API;

        public string API
        {
            get { return _API; }
            set { _API = value; }
        }


        public void Remove()//(SPSite site)
        {
            SPWeb rootWeb = SPContext.Current.Site.RootWeb;
            rootWeb.AllowUnsafeUpdates = true;
            if (rootWeb.AllProperties.ContainsKey("SharePresenceBingSettings"))
            {
                rootWeb.AllProperties.Remove("SharePresenceBingSettings");
            }
            rootWeb.Update();
            rootWeb.AllowUnsafeUpdates = false;
        }

        internal void Remove(SPSite site)
        {
            SPWeb rootWeb = site.RootWeb;
            rootWeb.AllowUnsafeUpdates = true;
            if (rootWeb.AllProperties.ContainsKey("SharePresenceBingSettings"))
            {
                rootWeb.AllProperties.Remove("SharePresenceBingSettings");
            }
            rootWeb.Update();
            rootWeb.AllowUnsafeUpdates = false;
        }

        public BingSettings Save()//(SPSite site)
        {
            string value = new JavaScriptSerializer().Serialize(this);
            SPWeb rootWeb = SPContext.Current.Site.RootWeb;
            rootWeb.AllowUnsafeUpdates = true;
            if (!rootWeb.AllProperties.ContainsKey("SharePresenceBingSettings"))
            {
                rootWeb.AddProperty("SharePresenceBingSettings", value);
            }
            else
            {
                rootWeb.AllProperties["SharePresenceBingSettings"] = value;
            }

            rootWeb.Update();
            rootWeb.AllowUnsafeUpdates = false;
            return this;
        }

        public BingSettings Load()//(SPSite site)
        {
            SPWeb rootWeb = SPContext.Current.Site.RootWeb;

            if (rootWeb.AllProperties.ContainsKey("SharePresenceBingSettings"))
            {
                string value = rootWeb.AllProperties["SharePresenceBingSettings"] as string;
                if (!string.IsNullOrEmpty(value))
                {
                    try
                    {
                        BingSettings settings = new JavaScriptSerializer().Deserialize<BingSettings>(value);
                        if (settings != null)
                        {
                            return settings;
                        }
                    }
                    catch
                    {
                        return new BingSettings();
                    }
                }
            }

            return this;
        }
    }
}
