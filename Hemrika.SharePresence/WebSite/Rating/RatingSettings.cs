using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Permissions;
using System.Text;
using Microsoft.SharePoint;
using System.Web.Script.Serialization;
using System.Web;

namespace Hemrika.SharePresence.WebSite.Rating
{
    [Serializable]
    public class RatingSettings
    {
        private int _minimum;

        public int Minimum
        {
            get { return _minimum; }
            set { _minimum = value; }
        }

        private int _maximum;

        public int Maximum
        {
            get { return _maximum; }
            set { _maximum = value; }
        }

        private float _step;

        public float Step
        {
            get { return _step; }
            set { _step = value; }
        }

        public void Remove()
        {
            SPWeb rootWeb = SPContext.Current.Site.RootWeb;
            rootWeb.AllowUnsafeUpdates = true;
            if (rootWeb.AllProperties.ContainsKey("SharePresenceRatingSettings"))
            {
                rootWeb.AllProperties.Remove("SharePresenceRatingSettings");
            }
            rootWeb.Update();
            rootWeb.AllowUnsafeUpdates = false;
        }

        public RatingSettings Save()
        {
            string value = new JavaScriptSerializer().Serialize(this);
            SPWeb rootWeb = SPContext.Current.Site.RootWeb;
            rootWeb.AllowUnsafeUpdates = true;
            if (!rootWeb.AllProperties.ContainsKey("SharePresenceRatingSettings"))
            {
                rootWeb.AddProperty("SharePresenceRatingSettings", value);
            }
            else
            {
                rootWeb.AllProperties["SharePresenceRatingSettings"] = value;
            }

            rootWeb.Update();
            rootWeb.AllowUnsafeUpdates = false;
            return this;

        }

        public RatingSettings Load()
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

            if (rootWeb.AllProperties.ContainsKey("SharePresenceRatingSettings"))
            {
                string value = rootWeb.AllProperties["SharePresenceRatingSettings"] as string;
                if (!string.IsNullOrEmpty(value))
                {
                    try
                    {
                        RatingSettings settings = new JavaScriptSerializer().Deserialize<RatingSettings>(value);
                        if (settings != null)
                        {
                            if (dispose) { rootWeb.Dispose(); };

                            return settings;
                        }
                    }
                    catch
                    {
                        return new RatingSettings();
                    }
                }
            }

            this.Maximum = 5;
            this.Minimum = 0;
            this.Step = 0.5f;
            return this;
        }

    }
}
