// <copyright file="SiteMapNodeEvent.cs" company="SharePresence">
// Copyright SharePresence. All rights reserved.
// </copyright>
// <author>MARKETING\Administrator</author>
// <date>2012-09-17 13:30:40Z</date>
namespace Hemrika.SharePresence.WebSite
{
    using System;
    using System.Collections.Generic;
    using System.Security.Permissions;
    using System.Text;
    using Microsoft.SharePoint;
    using Microsoft.SharePoint.Security;
    using Hemrika.SharePresence.WebSite.Navigation;
    using Microsoft.SharePoint.Administration;

    /// <summary>
    /// TODO: Add comment for SiteMapNodeEvent
    /// </summary>
    public class SiteMapNodeEvent : SPItemEventReceiver
    {
        /// <summary>
        /// TODO: Add comment for event ItemAdded in SiteMapNodeEvent 
        /// </summary>
        /// <param name="properties">Contains list event properties</param>
        [SharePointPermission(SecurityAction.LinkDemand, ObjectModel = true)]
        public override void ItemAdded(SPItemEventProperties properties)
        {
            try
            {
                if (properties.List.BaseTemplate.ToString() == "20005")
                {
                    ConsistantLookup(properties);
                }
            }
            catch (Exception ex)
            {
                SPDiagnosticsService.Local.WriteTrace(0, new SPDiagnosticsCategory(ex.Source, TraceSeverity.High, EventSeverity.Error), TraceSeverity.High, ex.Message, ex.Data);
                //ex.ToString();
            }

            EventFiringEnabled = false;

            base.ItemAdded(properties);

            try
            {
                if (properties.List.BaseTemplate.ToString() == "20005")
                {
                    WebSiteProvider webSiteProvider = System.Web.SiteMap.Providers["WebSiteProvider"] as WebSiteProvider;
                    if (webSiteProvider != null)
                    {
                        webSiteProvider.ResetProvider();
                    }
                }
            }
            catch (Exception ex)
            {
                SPDiagnosticsService.Local.WriteTrace(0, new SPDiagnosticsCategory(ex.Source, TraceSeverity.High, EventSeverity.Error), TraceSeverity.High, ex.Message, ex.Data);
                //ex.ToString();
            }

            EventFiringEnabled = true;
        }

        /// <summary>
        /// TODO: Add comment for event ItemDeleted in SiteMapNodeEvent 
        /// </summary>
        /// <param name="properties">Contains list event properties</param>   
        [SharePointPermission(SecurityAction.LinkDemand, ObjectModel = true)]
        public override void ItemDeleted(SPItemEventProperties properties)
        {
            try
            {
                if (properties.List.BaseTemplate.ToString() == "20005")
                {
                    ConsistantLookup(properties);
                }
            }
            catch (Exception ex)
            {
                SPDiagnosticsService.Local.WriteTrace(0, new SPDiagnosticsCategory(ex.Source, TraceSeverity.High, EventSeverity.Error), TraceSeverity.High, ex.Message, ex.Data);
                //ex.ToString();
            }

            EventFiringEnabled = false;

            base.ItemDeleted(properties);

            try
            {
                if (properties.List.BaseTemplate.ToString() == "20005")
                {
                    WebSiteProvider webSiteProvider = System.Web.SiteMap.Providers["WebSiteProvider"] as WebSiteProvider;
                    webSiteProvider.ResetProvider();
                }

            }
            catch (Exception ex)
            {
                SPDiagnosticsService.Local.WriteTrace(0, new SPDiagnosticsCategory(ex.Source, TraceSeverity.High, EventSeverity.Error), TraceSeverity.High, ex.Message, ex.Data);
                //ex.ToString();
            }

            EventFiringEnabled = true;

        }

        /// <summary>
        /// TODO: Add comment for event ItemUpdated in SiteMapNodeEvent 
        /// </summary>
        /// <param name="properties">Contains list event properties</param>   
        [SharePointPermission(SecurityAction.LinkDemand, ObjectModel = true)]
        public override void ItemUpdated(SPItemEventProperties properties)
        {
            try
            {
                if (properties.List.BaseTemplate.ToString() == "20005")
                {
                    ConsistantLookup(properties);
                }
            }
            catch (Exception ex)
            {
                SPDiagnosticsService.Local.WriteTrace(0, new SPDiagnosticsCategory(ex.Source, TraceSeverity.High, EventSeverity.Error), TraceSeverity.High, ex.Message, ex.Data);
                //ex.ToString();
            }

            EventFiringEnabled = false;

            base.ItemUpdated(properties);

            try
            {
                if (properties.List.BaseTemplate.ToString() == "20005")
                {
                    WebSiteProvider webSiteProvider = System.Web.SiteMap.Providers["WebSiteProvider"] as WebSiteProvider;
                    webSiteProvider.ResetProvider();
                }

            }
            catch (Exception ex)
            {
                SPDiagnosticsService.Local.WriteTrace(0, new SPDiagnosticsCategory(ex.Source, TraceSeverity.High, EventSeverity.Error), TraceSeverity.High, ex.Message, ex.Data);
                //ex.ToString();
            }

            EventFiringEnabled = true;
        }

        private void ConsistantLookup(SPItemEventProperties properties)
        {
            SPFieldLookup lookupField = (SPFieldLookup)properties.List.Fields["Parent"];

            if (string.IsNullOrEmpty(lookupField.LookupList))
            {
                if (lookupField.LookupWebId != properties.Web.ID)
                {
                    lookupField.LookupWebId = properties.Web.ID;
                }
                if (lookupField.LookupList != properties.ListId.ToString())
                {
                    lookupField.LookupList = properties.List.ID.ToString();
                }
            }
            else
            {
                string schema = lookupField.SchemaXml;
                schema = ReplaceXmlAttributeValue(schema, "List", properties.List.ID.ToString());
                schema = ReplaceXmlAttributeValue(schema, "WebId", properties.Web.ID.ToString());
                lookupField.SchemaXml = schema;
            }
            lookupField.Update(true);

        }

        private static string ReplaceXmlAttributeValue(string xml, string attributeName, string value)
        {
            if (string.IsNullOrEmpty(xml))
            {
                throw new ArgumentNullException("xml");
            }

            if (string.IsNullOrEmpty(value))
            {
                throw new ArgumentNullException("value");
            }


            int indexOfAttributeName = xml.IndexOf(attributeName, StringComparison.CurrentCultureIgnoreCase);
            if (indexOfAttributeName == -1)
            {
                throw new ArgumentOutOfRangeException("attributeName", string.Format("Attribute {0} not found in source xml", attributeName));
            }

            int indexOfAttibuteValueBegin = xml.IndexOf('"', indexOfAttributeName);
            int indexOfAttributeValueEnd = xml.IndexOf('"', indexOfAttibuteValueBegin + 1);

            return xml.Substring(0, indexOfAttibuteValueBegin + 1) + value + xml.Substring(indexOfAttributeValueEnd);
        }

    }
}

