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

    public class MetaDataSettings
    {
        private bool _autoKeywords;

        public bool AutoKeywords
        {
            get { return _autoKeywords; }
            set { _autoKeywords = value; }
        }

        private int _NumberOfkeywords;

        public int NumberOfkeywords
        {
            get { return _NumberOfkeywords; }
            set { _NumberOfkeywords = value; }
        }

        private const string _groupName = "MetaData Content Columns";

        public string GroupName
        {
            get { return _groupName; }
        }

        private bool _useMSNBotDMOZ;

        public bool UseMSNBotDMOZ
        {
            get { return _useMSNBotDMOZ; }
            set { _useMSNBotDMOZ = value; }
        }

        private bool _UseSiteLanguage;

        public bool UseSiteLanguage
        {
            get { return _UseSiteLanguage; }
            set { _UseSiteLanguage = value; }
        }

        private Authors _AuthorOverride;

        public Authors AuthorOverride
        {
            get { return _AuthorOverride; }
            set { _AuthorOverride = value; }
        }

        public Authors WebAuthorOverride
        {
            get { return _WebAuthorOverride; }
            set { _WebAuthorOverride = value; }
        }

        private Authors _WebAuthorOverride;

        public void Remove(SPSite site)
        {
            SPWeb rootWeb = site.RootWeb;
            rootWeb.AllowUnsafeUpdates = true;
            if (rootWeb.AllProperties.ContainsKey("SharePresenceMetaDataSettings"))
            {
                rootWeb.AllProperties.Remove("SharePresenceMetaDataSettings");
            }
            rootWeb.Update();
            rootWeb.AllowUnsafeUpdates = false;
        }


        public MetaDataSettings Save(SPSite site)
        {
            string value = new JavaScriptSerializer().Serialize(this);
            SPWeb rootWeb = site.RootWeb;
            rootWeb.AllowUnsafeUpdates = true;
            rootWeb.AllProperties["SharePresenceMetaDataSettings"] = value;
            rootWeb.Update();
            rootWeb.AllowUnsafeUpdates = false;
            return this;
        }

        public MetaDataSettings Load(SPSite site)
        {
            SPWeb rootWeb = site.RootWeb;

            this.UseMSNBotDMOZ = true;
            this.UseSiteLanguage = true;
            this.AutoKeywords = true;
            this.NumberOfkeywords = 30;
            this.AuthorOverride = Authors.NoOverride;
            this.WebAuthorOverride = Authors.NoOverride;

            if (rootWeb.AllProperties.ContainsKey("SharePresenceMetaDataSettings"))
            {
                string value = rootWeb.AllProperties["SharePresenceMetaDataSettings"] as string;
                if (!string.IsNullOrEmpty(value))
                {
                    try
                    {
                        MetaDataSettings settings = new JavaScriptSerializer().Deserialize<MetaDataSettings>(value);
                        if (settings != null)
                        {
                            this.AutoKeywords = settings.AutoKeywords;
                            this.NumberOfkeywords = settings.NumberOfkeywords;
                            this.UseSiteLanguage = settings.UseSiteLanguage;
                            this.AuthorOverride = settings.AuthorOverride;
                            this.WebAuthorOverride = settings.WebAuthorOverride;
                            this.UseMSNBotDMOZ = settings.UseMSNBotDMOZ;
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
