using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
//using Microsoft.SharePoint;
using System.Web.Script.Serialization;

namespace Hemrika.SharePresence.Google
{
    [Serializable]
    public class GoogleAnalyticsSettings
    {
        /*
        private bool _Active;

        public bool Active
        {
            get { return _Active; }
            set { _Active = value; }
        }
        */
        /*
        private string _HostName;

        public string HostName
        {
            get { return _HostName; }
            set { _HostName = value; }
        }
        */
        /*
        private string _token;

        public string Token
        {
            get { return _token; }
            set { _token = value; }
        }
        */
        /*
        private string _tableId;

        public string TableId
        {
            get { return _tableId; }
            set { _tableId = value; }
        }

        private string _webPropertyId;

        public string WebPropertyId
        {
            get { return _webPropertyId; }
            set { _webPropertyId = value; }
        }

        private string _ProfileId;

        public string ProfileId
        {
            get { return _ProfileId; }
            set { _ProfileId = value; }
        }

        private string _AccountId;

        public string AccountId
        {
            get { return _AccountId; }
            set { _AccountId = value; }
        }

        private bool _ClientInfo;

        public bool ClientInfo
        {
            get { return _ClientInfo; }
            set { _ClientInfo = value; }
        }

        private bool _DetectFlash;

        public bool DetectFlash
        {
            get { return _DetectFlash; }
            set { _DetectFlash = value; }
        }

        private bool _DetectTitle;

        public bool DetectTitle
        {
            get { return _DetectTitle; }
            set { _DetectTitle = value; }
        }

        private bool _AllowLinker;

        public bool AllowLinker
        {
            get { return _AllowLinker; }
            set { _AllowLinker = value; }
        }

        private bool _AllowHash;

        public bool AllowHash
        {
            get { return _AllowHash; }
            set { _AllowHash = value; }
        }

        private bool _Authenticated;

        public bool Authenticated
        {
            get { return _Authenticated; }
            set { _Authenticated = value; }
        }
        private string _ProfileName;

        public string ProfileName
        {
            get { return _ProfileName; }
            set { _ProfileName = value; }
        }
        */
        /*
        public void Remove(SPSite site)
        {
            SPWeb rootWeb = site.RootWeb;
            rootWeb.AllowUnsafeUpdates = true;
            if (rootWeb.AllProperties.ContainsKey("SharePresenceGoogleAnalyticsSettings"))
            {
                rootWeb.AllProperties.Remove("SharePresenceGoogleAnalyticsSettings");
            }
            rootWeb.Update();
            rootWeb.AllowUnsafeUpdates = false;
        }

        public GoogleAnalyticsSettings Save(SPSite site)
        {
            string value = new JavaScriptSerializer().Serialize(this);
            SPWeb rootWeb = site.RootWeb;
            rootWeb.AllowUnsafeUpdates = true;
            rootWeb.AllProperties["SharePresenceGoogleAnalyticsSettings"] = value;
            rootWeb.Update();
            rootWeb.AllowUnsafeUpdates = false;
            return this;
        }

        public GoogleAnalyticsSettings Load(SPSite site)
        {
            SPWeb rootWeb = site.RootWeb;

            if (rootWeb.AllProperties.ContainsKey("SharePresenceGoogleAnalyticsSettings"))
            {
                string value = rootWeb.AllProperties["SharePresenceGoogleAnalyticsSettings"] as string;
                if (!string.IsNullOrEmpty(value))
                {
                    try
                    {
                        GoogleAnalyticsSettings settings = new JavaScriptSerializer().Deserialize<GoogleAnalyticsSettings>(value);
                        if (settings != null)
                        {
                            this.AccountId = settings.AccountId;
                            this.AllowHash = settings.AllowHash;
                            this.AllowLinker = settings.AllowLinker;
                            this.ClientInfo = settings.ClientInfo;
                            this.DetectFlash = settings.DetectFlash;
                            this.DetectTitle = settings.DetectTitle;
                            this.ProfileId = settings.ProfileId;
                        }
                    }
                    catch
                    {
                    }
                }
            }

            return this;
        }
        */



        public string Created { get; set; }

        public string Currency { get; set; }

        public string DefaultPage { get; set; }

        public bool ECommerceTracking { get; set; }

        public string Id { get; set; }

        public string InternalWebPropertyId { get; set; }

        public string Name { get; set; }

        public string Timezone { get; set; }

        public string WebsiteUrl { get; set; }

        public string AccountId { get; set; }

        public string WebPropertyId { get; set; }

        public bool Active { get; set; }

        public bool ClientInfo { get; set; }

        public bool DetectFlash { get; set; }

        public bool AllowLinker { get; set; }

        public bool DetectTitle { get; set; }

        public bool AllowHash { get; set; }

        public bool Authenticated { get; set; }
    }
}
