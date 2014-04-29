// -----------------------------------------------------------------------
// <copyright file="MetaDataSettings.cs" company="">
// TODO: Update copyright text.
// </copyright>
// -----------------------------------------------------------------------

namespace Hemrika.SharePresence.WebSite.MetaData
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Web.Script.Serialization;
    using Microsoft.SharePoint;

    public class TwitterSettings
    {

        private Cards _TwitterCard;

        public Cards TwitterCard
        {
            get { return _TwitterCard; }
            set { _TwitterCard = value; }
        }

        private string _TwitterSite;

        public string TwitterSite
        {
            get { return _TwitterSite; }
            set { _TwitterSite = value;}
        }

        private string _TwitterSiteId;

        public string TwitterSiteId
        {
            get { return _TwitterSiteId; }
            set { _TwitterSiteId = value; }
        }

        private string _TwitterCreator;

        public string TwitterCreator
        {
            get { return _TwitterCreator; }
            set { _TwitterCreator = value; }
        }

        private string _TwitterCreatorId;

        public string TwitterCreatorId
        {
            get { return _TwitterCreatorId; }
            set { _TwitterCreatorId = value; }
        }

        public void Remove(SPSite site)
        {
            SPWeb rootWeb = site.RootWeb;
            rootWeb.AllowUnsafeUpdates = true;
            if (rootWeb.AllProperties.ContainsKey("SharePresenceTwitterSettings"))
            {
                rootWeb.AllProperties.Remove("SharePresenceTwitterSettings");
            }
            rootWeb.Update();
            rootWeb.AllowUnsafeUpdates = false;
        }


        public TwitterSettings Save(SPSite site)
        {
            string value = new JavaScriptSerializer().Serialize(this);
            SPWeb rootWeb = site.RootWeb;
            rootWeb.AllowUnsafeUpdates = true;
            rootWeb.AllProperties["SharePresenceTwitterSettings"] = value;
            rootWeb.Update();
            rootWeb.AllowUnsafeUpdates = false;
            return this;
        }

        public TwitterSettings Load(SPSite site)
        {
            SPWeb rootWeb = site.RootWeb;

            this.TwitterCard = Cards.summery;
            this.TwitterCreator = string.Empty;
            this.TwitterCreatorId = string.Empty;
            this.TwitterSite = string.Empty;
            this.TwitterSiteId = string.Empty;

            if (rootWeb.AllProperties.ContainsKey("SharePresenceTwitterSettings"))
            {
                string value = rootWeb.AllProperties["SharePresenceTwitterSettings"] as string;
                if (!string.IsNullOrEmpty(value))
                {
                    try
                    {
                        TwitterSettings settings = new JavaScriptSerializer().Deserialize<TwitterSettings>(value);
                        if (settings != null)
                        {
                            this.TwitterCard = settings.TwitterCard;
                            this.TwitterCreator = settings.TwitterCreator;
                            this.TwitterCreatorId = settings.TwitterCreatorId;
                            this.TwitterSite = settings.TwitterSite;
                            this.TwitterSiteId = settings.TwitterSiteId;
                        }
                    }
                    catch
                    {
                    }
                }
            }

            return this;
        }
    }
}
