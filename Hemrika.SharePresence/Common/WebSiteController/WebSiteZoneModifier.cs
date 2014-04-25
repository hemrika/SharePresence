// -----------------------------------------------------------------------
// <copyright file="WebSiteDocItemModifier.cs" company="">
// TODO: Update copyright text.
// </copyright>
// -----------------------------------------------------------------------

namespace Hemrika.SharePresence.Common.WebSiteController
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using Microsoft.SharePoint.Administration;
    using System.IO;
    using System.Xml.Linq;
    using Microsoft.SharePoint.Utilities;

    /// <summary>
    /// TODO: Update summary.
    /// </summary>
    public class WebSiteZoneModifier : SPJobDefinition
    {
        public WebSiteZoneModifier() : base() { }

        public WebSiteZoneModifier(SPWebApplication application, Guid featureID, bool delete)
            : base("Zone Modifier for " + application.Name, application, null, SPJobLockType.None)
        {
            SPFeatureDefinition def = application.Features[featureID].Definition;
            string szone = def.Properties[zoneKey].Value;
            this.Zone = (SPUrlZone)Enum.Parse(typeof(SPUrlZone), szone, true);
            this.Url = def.Properties[urlKey].Value;
            this._delete = delete;

        }

        public WebSiteZoneModifier(SPWebApplication application, SPUrlZone zone, string url, bool delete)
            : base("Zone Modifier for " + application.Name, application, null, SPJobLockType.None)
        {
            this._delete = delete;
            this.Zone = zone;
            this.Url = url;
        }

        public override void Execute(Guid targetInstanceId)
        {
            base.Execute(targetInstanceId);
            this.ChangeZone();
        }

        private readonly string deleteKey = "Zone_Delete";

        private bool _delete
        {
            get
            {
                if (this.Properties.ContainsKey(deleteKey))
                {
                    return Convert.ToBoolean(this.Properties[deleteKey]);
                }
                else
                {
                    return false;
                }
            }
            set
            {
                if (this.Properties.ContainsKey(deleteKey))
                {
                    this.Properties[deleteKey] = value.ToString();
                }
                else
                {
                    this.Properties.Add(deleteKey, value.ToString());
                }
            }
        }

        private readonly string zoneKey = "Zone_Name";

        private SPUrlZone Zone
        {
            get
            {
                if (this.Properties.ContainsKey(zoneKey))
                {
                    string szone = Convert.ToString(this.Properties[zoneKey]);
                    return (SPUrlZone)Enum.Parse(typeof(SPUrlZone), szone, true);
                }
                else
                {
                    return SPUrlZone.Custom;
                }
            }
            set
            {
                if (this.Properties.ContainsKey(zoneKey))
                {
                    this.Properties[zoneKey] = value.ToString();
                }
                else
                {
                    this.Properties.Add(zoneKey, value.ToString());
                }
            }
        }

        private readonly string urlKey = "Zone_Url";

        private string Url
        {
            get
            {
                if (this.Properties.ContainsKey(urlKey))
                {
                    return Convert.ToString(this.Properties[urlKey]);
                }
                else
                {
                    return string.Empty;
                }
            }
            set
            {
                if (this.Properties.ContainsKey(urlKey))
                {
                    this.Properties[urlKey] = value.ToString();
                }
                else
                {
                    this.Properties.Add(urlKey, value.ToString());
                }
            }
        }

        private void ChangeZone()
        {
            if (Zone != SPUrlZone.Internet)
            {
                SPAlternateUrlCollection alternateUrls = WebApplication.AlternateUrls;
                SPAlternateUrl alternateurl = new SPAlternateUrl(Url, Zone);
                Uri internetRoot = null;

                try
                {
                    internetRoot = WebApplication.GetResponseUri(Zone);
                }
                catch
                {
                    internetRoot = null;
                }
                    

                try
                {
                    alternateurl = alternateUrls[Url];
                    if (alternateurl == null || alternateurl.UrlZone != SPUrlZone.Internet)
                    {
                        alternateurl = null;
                    }
                }
                catch (Exception ex)
                {
                    alternateurl = null;
                    ex.ToString();
                }

                if (_delete)
                {


                    if (!string.IsNullOrEmpty(Url) && alternateurl != null)
                    {
                        try
                        {
                            alternateUrls.Delete(Url);
                        }
                        catch (Exception ex)
                        {
                            ex.ToString();
                        }
                        finally
                        {
                            alternateUrls.Update();
                        }
                    }
                }
                else
                {
                    try
                    {

                        //No Internet Url defined;
                        if (internetRoot == null)
                        {
                            if (!string.IsNullOrEmpty(Url))
                            {
                                SPAlternateUrl defaultInternet = new SPAlternateUrl(Url, Zone);
                                alternateUrls.SetResponseUrl(defaultInternet);
                            }
                        }
                        // Incoming Urls
                        else
                        {
                            SPAlternateUrl incomingUrl = new SPAlternateUrl(Url, Zone);
                            alternateUrls.Add(incomingUrl);
                        }
                    }
                    catch (Exception ex)
                    {
                        ex.ToString();
                    }
                    finally
                    {
                        alternateUrls.Update();
                    }
                }
            }
        }
    }
}

