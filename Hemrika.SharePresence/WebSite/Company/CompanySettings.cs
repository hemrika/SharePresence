using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Permissions;
using System.Text;
using Microsoft.SharePoint;
using System.Web.Script.Serialization;
using System.Web;

namespace Hemrika.SharePresence.WebSite.Company
{
    [Serializable]
    public class CompanySettings
    {
        private string _Name;

        public string Name
        {
            get { return _Name; }
            set { _Name = value; }
        }
        private string _Street;

        public string Street
        {
            get { return _Street; }
            set { _Street = value; }
        }
        private string _PostalCode;

        public string PostalCode
        {
            get { return _PostalCode; }
            set { _PostalCode = value; }
        }
        private string _City;

        public string City
        {
            get { return _City; }
            set { _City = value; }
        }
        private string _State;

        public string State
        {
            get { return _State; }
            set { _State = value; }
        }
        private string _Country;

        public string Country
        {
            get { return _Country; }
            set { _Country = value; }
        }
        private string _Email;

        public string Email
        {
            get { return _Email; }
            set { _Email = value; }
        }
        private string _Phone;

        public string Phone
        {
            get { return _Phone; }
            set { _Phone = value; }
        }
        private string _Twitter;

        public string Twitter
        {
            get { return _Twitter; }
            set { _Twitter = value; }
        }
        private string _FaceBook;

        public string FaceBook
        {
            get { return _FaceBook; }
            set { _FaceBook = value; }
        }
        private string _LinkedIn;

        public string LinkedIn
        {
            get { return _LinkedIn; }
            set { _LinkedIn = value; }
        }

        private string _Stock;

        public string Stock
        {
            get { return _Stock; }
            set { _Stock = value; }
        }

        public void Remove()
        {
            SPWeb rootWeb = SPContext.Current.Site.RootWeb;
            rootWeb.AllowUnsafeUpdates = true;
            if (rootWeb.AllProperties.ContainsKey("SharePresenceCompanySettings"))
            {
                rootWeb.AllProperties.Remove("SharePresenceCompanySettings");
            }
            rootWeb.Update();
            rootWeb.AllowUnsafeUpdates = false;
        }

        public CompanySettings Save()
        {
            string value = new JavaScriptSerializer().Serialize(this);
            SPWeb rootWeb = SPContext.Current.Site.RootWeb;
            rootWeb.AllowUnsafeUpdates = true;
            if (!rootWeb.AllProperties.ContainsKey("SharePresenceCompanySettings"))
            {
                rootWeb.AddProperty("SharePresenceCompanySettings", value);
            }
            else
            {
                rootWeb.AllProperties["SharePresenceCompanySettings"] = value;
            }

            rootWeb.Update();
            rootWeb.AllowUnsafeUpdates = false;
            return this;

        }

        public CompanySettings Load()
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

            if (rootWeb.AllProperties.ContainsKey("SharePresenceCompanySettings"))
            {
                string value = rootWeb.AllProperties["SharePresenceCompanySettings"] as string;
                if (!string.IsNullOrEmpty(value))
                {
                    try
                    {
                        CompanySettings settings = new JavaScriptSerializer().Deserialize<CompanySettings>(value);
                        if (settings != null)
                        {
                            if (dispose) { rootWeb.Dispose(); };

                            return settings;
                        }
                    }
                    catch
                    {
                        return new CompanySettings();
                    }
                }
            }

            return this;
        }

    }
}
