// <copyright file="GateKeeperService.svc.cs" company="Hemrika">
// Copyright Hemrika. All rights reserved.
// </copyright>
// <author>MARKETING\Administrator</author>
// <date>2012-04-24 11:44:16Z</date>
namespace Hemrika.SharePresence.WebSite.GateKeeper
{
    using System.ServiceModel.Activation;
    using Microsoft.SharePoint.Client.Services;
    using Hemrika.SharePresence.WebSite.Modules.GateKeeper;
    using System.Collections.Generic;
    using System.Linq;
    using Microsoft.SharePoint;
    using Microsoft.SharePoint.Linq;
    using System.Web.Services;
    using System;
    using Hemrika.SharePresence.WebSite.ContentTypes;

    [BasicHttpBindingServiceMetadataExchangeEndpoint]
    [AspNetCompatibilityRequirements(RequirementsMode = AspNetCompatibilityRequirementsMode.Required)]
    [WebService(Namespace = "http://sharepresence.com/", Description = "GateKeeper Service", Name = "GateKeeperService")]
    public class GateKeeperService : IGateKeeperService
    {
        // Access the web service from URL http://<Servername>/_vti_bin/SharePresence/GateKeeperService.svc/mex
        private SPWeb web = null;
        private string url = null;

        public bool HasListing(GateKeeperType type, GateKeeperListing node, string value)
        {
            bool listed = false;

            try
            {
                url = SPContext.Current.Web.Url;
                OpenElevatedWeb();

                if (web != null)
                {
                    SPListItemCollection nodes = null;
                    string field = string.Empty;
                    SPList gatekeeper = web.Lists["GateKeeper"];

                    SPQuery query = new SPQuery();
                    query.Query = "<Where><And><Eq><FieldRef Name=\"GateKeeper_Type\" /><Value Type=\"Choice\">" + type.ToString() + "</Value></Eq><Eq><FieldRef Name=\"" + node.ToString() + "\" /><Value Type=\"Text\">" + value + "</Value></Eq></And></Where>";
                    query.ViewFields = "<FieldRef Name=\"GateKeeper_Type\" />";
                    nodes = gatekeeper.GetItems(query);

                    if (nodes.Count > 0)
                    {
                        listed = true;
                    }
                }
            }
            catch (System.Exception ex)
            {
                ex.ToString();
            }

            return listed;
        }

        public void RemoveListing(string id)
        {
            try
            {
                Guid itemId = Guid.Empty;

                try
                {
                    itemId = new Guid(id);
                }
                catch (Exception ex)
                {
                    ex.ToString();
                }

                if (itemId != Guid.Empty)
                {
                    url = SPContext.Current.Web.Url;
                    OpenElevatedWeb();

                    if (web != null)
                    {
                        web.AllowUnsafeUpdates = true;
                        SPList gatekeeper = web.Lists["GateKeeper"];
                        SPListItem item = gatekeeper.Items[itemId];
                        item.Delete();
                        gatekeeper.Update();
                        web.AllowUnsafeUpdates = false;
                    }
                }
            }
            catch (System.Exception ex)
            {
                ex.ToString();
            }
        }

        public void GateKeeper(GateKeeperType type, GateKeeperListing node, string value)
        {
            try
            {
                url = SPContext.Current.Web.Url;
                OpenElevatedWeb();

                if (web != null)
                {
                    web.AllowUnsafeUpdates = true;
                    SPList gatekeeper = web.Lists["GateKeeper"];
                    SPListItem http = gatekeeper.Items.Add();
                    http["GateKeeper_Date"] = DateTime.Now;
                    if (node == GateKeeperListing.GateKeeper_IPAddress)
                    {
                        http["GateKeeper_IPAddress"] = value;
                    }
                    if (node == GateKeeperListing.GateKeeper_Url)
                    {

                        http["GateKeeper_Url"] = value;
                    }
                    if (node == GateKeeperListing.GateKeeper_Useragent)
                    {

                        http["GateKeeper_Useragent"] = value;
                    }

                    http["GateKeeper_Type"] = type;
                    SPContentType itemtype = web.AvailableContentTypes[ContentTypeId.GateKeeper_Entry];
                    http[SPBuiltInFieldId.ContentTypeId] = itemtype.Id;
                    http[SPBuiltInFieldId.ContentType] = itemtype.Name;

                    http.Update();
                    web.AllowUnsafeUpdates = false;
                }

            }
            catch (System.Exception ex)
            {
                ex.ToString();
            }
        }

        public void HoneyPot(string UserHostAddress, string Referrer, string UserAgent)
        {
            try
            {
                url = SPContext.Current.Web.Url;
                OpenElevatedWeb();

                if (web != null)
                {
                    web.AllowUnsafeUpdates = true;
                    SPList gatekeeper = web.Lists["GateKeeper"];
                    SPListItem honeypot = gatekeeper.Items.Add();
                    honeypot["GateKeeper_Date"] = DateTime.Now;
                    honeypot["GateKeeper_IPAddress"] = UserHostAddress.ToString();
                    honeypot["GateKeeper_Referrer"] = String.IsNullOrEmpty(Referrer) ? string.Empty : Referrer;
                    honeypot["GateKeeper_Useragent"] = String.IsNullOrEmpty(UserAgent) ? string.Empty : UserAgent;
                    honeypot["GateKeeper_Type"] = GateKeeperType.HoneyPot;
                    SPContentType itemtype = web.AvailableContentTypes[ContentTypeId.GateKeeper_HoneyPot];
                    honeypot[SPBuiltInFieldId.ContentTypeId] = itemtype.Id;
                    honeypot[SPBuiltInFieldId.ContentType] = itemtype.Name;

                    honeypot.Update();
                    web.AllowUnsafeUpdates = false;
                }

            }
            catch (System.Exception ex)
            {
                ex.ToString();
            }
        }

        public void HTTP(string UserHostAddress, string LastActivity, string Referrer, string ThreatLevel, string UserAgent, string VisitorType)
        {
            try
            {
                url = SPContext.Current.Web.Url;
                OpenElevatedWeb();

                if (web != null)
                {
                    web.AllowUnsafeUpdates = true;
                    SPList gatekeeper = web.Lists["GateKeeper"];
                    SPListItem http = gatekeeper.Items.Add();
                    http["GateKeeper_Date"] = DateTime.Now;
                    http["GateKeeper_IPAddress"] = UserHostAddress.ToString();
                    http["GateKeeper_Referrer"] = String.IsNullOrEmpty(Referrer) ? string.Empty : Referrer;
                    http["GateKeeper_Useragent"] = String.IsNullOrEmpty(UserAgent) ? string.Empty : UserAgent;
                    http["GateKeeper_Threatlevel"] = String.IsNullOrEmpty(ThreatLevel) ? string.Empty : ThreatLevel;
                    http["GateKeeper_Visitortype"] = String.IsNullOrEmpty(VisitorType) ? string.Empty : VisitorType;
                    http["GateKeeper_LastActivity"] = String.IsNullOrEmpty(LastActivity) ? string.Empty : LastActivity;
                    http["GateKeeper_Type"] = GateKeeperType.HTTP;
                    SPContentType itemtype = web.AvailableContentTypes[ContentTypeId.GateKeeper_HTTP];
                    http[SPBuiltInFieldId.ContentTypeId] = itemtype.Id;
                    http[SPBuiltInFieldId.ContentType] = itemtype.Name;

                    http.Update();
                    web.AllowUnsafeUpdates = false;
                }

            }
            catch (System.Exception ex)
            {
                ex.ToString();
            }
        }

        public void Drone(string UserHostAddress, string Referrer, string UserAgent)
        {
            try
            {
                url = SPContext.Current.Web.Url;
                OpenElevatedWeb();

                if (web != null)
                {
                    web.AllowUnsafeUpdates = true;
                    SPList gatekeeper = web.Lists["GateKeeper"];
                    SPListItem drone = gatekeeper.Items.Add();
                    drone["GateKeeper_Date"] = DateTime.Now;
                    drone["GateKeeper_IPAddress"] = UserHostAddress.ToString();
                    drone["GateKeeper_Referrer"] = String.IsNullOrEmpty(Referrer) ? string.Empty : Referrer;
                    drone["GateKeeper_Useragent"] = String.IsNullOrEmpty(UserAgent) ? string.Empty : UserAgent;
                    drone["GateKeeper_Type"] = GateKeeperType.Drone;
                    SPContentType itemtype = web.AvailableContentTypes[ContentTypeId.GateKeeper_Drone];
                    drone[SPBuiltInFieldId.ContentTypeId] = itemtype.Id;
                    drone[SPBuiltInFieldId.ContentType] = itemtype.Name;

                    drone.Update();
                    web.AllowUnsafeUpdates = false;
                }

            }
            catch (System.Exception ex)
            {
                ex.ToString();
            }
        }

        public void Proxy(string UserHostAddress, string Referrer, string UserAgent)
        {
            try
            {
                url = SPContext.Current.Web.Url;
                OpenElevatedWeb();

                if (web != null)
                {
                    web.AllowUnsafeUpdates = true;
                    SPList gatekeeper = web.Lists["GateKeeper"];
                    SPListItem proxy = gatekeeper.Items.Add();
                    proxy["GateKeeper_Date"] = DateTime.Now;
                    proxy["GateKeeper_IPAddress"] = UserHostAddress.ToString();
                    proxy["GateKeeper_Referrer"] = String.IsNullOrEmpty(Referrer) ? string.Empty : Referrer;
                    proxy["GateKeeper_Useragent"] = String.IsNullOrEmpty(UserAgent) ? string.Empty : UserAgent;
                    proxy["GateKeeper_Type"] = GateKeeperType.Proxy;
                    SPContentType itemtype = web.AvailableContentTypes[ContentTypeId.GateKeeper_Proxy];
                    proxy[SPBuiltInFieldId.ContentTypeId] = itemtype.Id;
                    proxy[SPBuiltInFieldId.ContentType] = itemtype.Name;

                    proxy.Update();
                    web.AllowUnsafeUpdates = false;
                }

            }
            catch (System.Exception ex)
            {
                ex.ToString();
            }
        }

        private void OpenElevatedWeb()
        {
            SPSecurity.RunWithElevatedPrivileges(delegate()
            {
                using (SPSite site = new SPSite(url, SPUserToken.SystemAccount))
                {
                    site.AllowUnsafeUpdates = true;
                    web = site.OpenWeb();
                }
            });
        }
    }
}

