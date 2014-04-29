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

    public class FaceBookSettings
    {
        private string _FaceBookAdmins;

        public string FaceBookAdmins
        {
            get { return _FaceBookAdmins; }
            set { _FaceBookAdmins = value; }
        }

        private string _FaceBookAppId;

        public string FaceBookAppId
        {
            get { return _FaceBookAppId; }
            set { _FaceBookAppId = value; }
        }

        private string _FaceBookProfileId;

        public string FaceBookProfileId
        {
            get { return _FaceBookProfileId; }
            set { _FaceBookProfileId = value; }
        }

        public void Remove(SPSite site)
        {
            SPWeb rootWeb = site.RootWeb;
            rootWeb.AllowUnsafeUpdates = true;
            if (rootWeb.AllProperties.ContainsKey("SharePresenceFaceBookSettings"))
            {
                rootWeb.AllProperties.Remove("SharePresenceFaceBookSettings");
            }
            rootWeb.Update();
            rootWeb.AllowUnsafeUpdates = false;
        }


        public FaceBookSettings Save(SPSite site)
        {
            string value = new JavaScriptSerializer().Serialize(this);
            SPWeb rootWeb = site.RootWeb;
            rootWeb.AllowUnsafeUpdates = true;
            rootWeb.AllProperties["SharePresenceFaceBookSettings"] = value;
            rootWeb.Update();
            rootWeb.AllowUnsafeUpdates = false;
            return this;
        }

        public FaceBookSettings Load(SPSite site)
        {
            SPWeb rootWeb = site.RootWeb;

            this.FaceBookAdmins = string.Empty;
            this.FaceBookAppId = string.Empty;
            this.FaceBookProfileId = string.Empty;

            if (rootWeb.AllProperties.ContainsKey("SharePresenceFaceBookSettings"))
            {
                string value = rootWeb.AllProperties["SharePresenceFaceBookSettings"] as string;
                if (!string.IsNullOrEmpty(value))
                {
                    try
                    {
                        FaceBookSettings settings = new JavaScriptSerializer().Deserialize<FaceBookSettings>(value);
                        if (settings != null)
                        {
                            this.FaceBookAdmins = settings.FaceBookAdmins;
                            this.FaceBookAppId = settings.FaceBookAppId;
                            this.FaceBookProfileId = settings.FaceBookProfileId;
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
