// <copyright file="Hemrika_SharePresence_WebSiteNavigation.EventReceiver.cs" company="Hemrika">
// Copyright Hemrika. All rights reserved.
// </copyright>
// <author>MARKETING\Administrator</author>
// <date>2012-01-26 20:03:04Z</date>
namespace Hemrika.SharePresence.WebSite
{
    using System;
    using System.Collections.Generic;
    using System.Security.Permissions;
    using System.Text;
    using Microsoft.SharePoint;
    using Microsoft.SharePoint.Linq;
    using Microsoft.SharePoint.Security;
    using Microsoft.SharePoint.Administration;
    using Hemrika.SharePresence.Common.Extensions;


    /// <summary>
    /// TODO: Add comment to WebSiteNavigationEventReceiver
    /// </summary>
    [SharePointPermission(SecurityAction.InheritanceDemand, ObjectModel = true)]
    public class WebSiteNavigationEventReceiver : SPFeatureReceiver
    {
        private SPWebApplication webApp = null;

        private void FindWebApplication(SPFeatureReceiverProperties properties)
        {
            SPFeatureScope scope = properties.Feature.Definition.Scope;

            if (scope == SPFeatureScope.Web)
            {
                SPWeb web = properties.Feature.Parent as SPWeb;
                SPUserToken oSysToken = GetSysToken(web.Site.ID);
                using (SPSite centralSite = new SPSite(web.Site.ID, oSysToken))
                {

                    webApp = centralSite.WebApplication;
                }
            }


            if (scope == SPFeatureScope.Site)
            {
                SPSite site = properties.Feature.Parent as SPSite;

                SPUserToken oSysToken = GetSysToken(site.ID);
                using (SPSite centralSite = new SPSite(site.ID, oSysToken))
                {

                    webApp = centralSite.WebApplication;
                }
            }

            if (scope == SPFeatureScope.WebApplication)
            {
                webApp = properties.Feature.Parent as SPWebApplication;
            }
        }

        /// <summary>
        /// TODO: Add comment to describe the actions after feature activation
        /// </summary>
        /// <param name="properties">Properties of the feature</param>
        [SharePointPermission(SecurityAction.LinkDemand, ObjectModel = true)]
        public override void FeatureActivated(SPFeatureReceiverProperties properties)
        {
            FindWebApplication(properties);

            SPFeatureScope scope = properties.Feature.Definition.Scope;

            if (scope == SPFeatureScope.Web)
            {
                SPWeb web = properties.Feature.Parent as SPWeb;
                SPList list = web.GetList("SiteMap");

                try
                {
                    ConsistantLookup(web, list);
                }
                catch (Exception ex)
                {
                    SPDiagnosticsService.Local.WriteTrace(0, new SPDiagnosticsCategory(ex.Source, TraceSeverity.High, EventSeverity.Error), TraceSeverity.High, ex.Message, ex.Data);
                    //ex.ToString();
                }

                try
                {

                    Navigation.SitemapDataContext context = new Navigation.SitemapDataContext(web.Url);

                    Navigation.SiteMapNavigationEntry root = new Navigation.SiteMapNavigationEntry();
                    root.Title = "root";
                    context.SiteMap.InsertOnSubmit(root);

                    Navigation.SiteMapNavigationEntry home = new Navigation.SiteMapNavigationEntry();
                    home.Title = "Home";
                    home.URL = web.Url;
                    home.Parent = root;
                    home.Enabled = true;
                    context.SiteMap.InsertOnSubmit(home);

                    context.SubmitChanges(Microsoft.SharePoint.Linq.ConflictMode.ContinueOnConflict, true);

                    
                }
                catch (Exception ex)
                {
                    SPDiagnosticsService.Local.WriteTrace(0, new SPDiagnosticsCategory(ex.Source, TraceSeverity.High, EventSeverity.Error), TraceSeverity.High, ex.Message, ex.Data);
                    //ex.ToString();
                }
            }

            SPWebConfigModificationExtensions.WaitForOnetimeJobToFinish(webApp.Farm, "job-webconfig-modification", 180);

            Activate_WebSiteProvider(properties);

            try
            {
                webApp.Farm.Services.GetValue<SPWebService>().ApplyWebConfigModifications();
                SPWebConfigModificationExtensions.WaitForOnetimeJobToFinish(webApp.Farm, "job-webconfig-modification", 180);
                webApp.Update();
            }
            catch (Exception ex)
            {
                SPDiagnosticsService.Local.WriteTrace(0, new SPDiagnosticsCategory(ex.Source, TraceSeverity.High, EventSeverity.Error), TraceSeverity.High, ex.Message, ex.Data);
                //ex.ToString();
            }
        }

        private void Activate_WebSiteProvider(SPFeatureReceiverProperties properties)
        {
            if (webApp != null)
            {
                SPWebConfigModification mod = new SPWebConfigModification("add[@type='Hemrika.SharePresence.WebSite.Navigation.WebSiteProvider, Hemrika.SharePresence.WebSite, Version=1.0.0.0, Culture=neutral, PublicKeyToken=11e6604a27f32a11']", "configuration/system.web/siteMap/providers");
                mod.Owner = properties.Feature.DefinitionId.ToString();
                mod.Sequence = 0;
                mod.Type = SPWebConfigModification.SPWebConfigModificationType.EnsureChildNode;
                mod.Value = @"<add name=""WebSiteProvider"" type=""Hemrika.SharePresence.WebSite.Navigation.WebSiteProvider, Hemrika.SharePresence.WebSite, Version=1.0.0.0, Culture=neutral, PublicKeyToken=11e6604a27f32a11"" />";
                if (!webApp.WebConfigModifications.Contains(mod))
                {
                    webApp.WebConfigModifications.Add(mod);
                }

                SPWebConfigModificationExtensions.WaitForOnetimeJobToFinish(webApp.Farm, "job-webconfig-modification", 180);

                try
                {
                    webApp.Farm.Services.GetValue<SPWebService>().ApplyWebConfigModifications();
                    SPWebConfigModificationExtensions.WaitForOnetimeJobToFinish(webApp.Farm, "job-webconfig-modification", 180);
                    webApp.Update();
                }
                catch (Exception ex)
                {
                    SPDiagnosticsService.Local.WriteTrace(0, new SPDiagnosticsCategory(ex.Source, TraceSeverity.High, EventSeverity.Error), TraceSeverity.High, ex.Message, ex.Data);
                    //ex.ToString();
                }
            }
        }

        /// <summary>
        /// TODO: Add comment to describe the actions during feature deactivation
        /// </summary>
        /// <param name="properties">Properties of the feature</param>
        [SharePointPermission(SecurityAction.LinkDemand, ObjectModel = true)]
        public override void FeatureDeactivating(SPFeatureReceiverProperties properties)
        {
            FindWebApplication(properties);

            Deactivate_WebSiteProvider(properties);

            SPWebConfigModificationExtensions.WaitForOnetimeJobToFinish(webApp.Farm, "job-webconfig-modification", 180);

            try
            {
                webApp.Farm.Services.GetValue<SPWebService>().ApplyWebConfigModifications();
                SPWebConfigModificationExtensions.WaitForOnetimeJobToFinish(webApp.Farm, "job-webconfig-modification", 180);
                webApp.Update();
            }
            catch (Exception ex)
            {
                SPDiagnosticsService.Local.WriteTrace(0, new SPDiagnosticsCategory(ex.Source, TraceSeverity.High, EventSeverity.Error), TraceSeverity.High, ex.Message, ex.Data);
                //ex.ToString();
            }
        }

        /// <summary>
        /// Unregisters the HTTP Module in web.config
        /// </summary>
        /// <param name="properties">Properties of the feature</param>
        private void Deactivate_WebSiteProvider(SPFeatureReceiverProperties properties)
        {
            if (webApp != null)
            {

                foreach (SPWebConfigModification mod in webApp.WebConfigModifications)
                {
                    if (mod.Owner == properties.Feature.DefinitionId.ToString())
                    {
                        webApp.WebConfigModifications.Remove(mod);
                        break;
                    }
                }

                SPWebConfigModificationExtensions.WaitForOnetimeJobToFinish(webApp.Farm, "job-webconfig-modification", 180);

                try
                {
                    webApp.Farm.Services.GetValue<SPWebService>().ApplyWebConfigModifications();
                    SPWebConfigModificationExtensions.WaitForOnetimeJobToFinish(webApp.Farm, "job-webconfig-modification", 180);
                    webApp.Update();
                }
                catch (Exception ex)
                {
                    SPDiagnosticsService.Local.WriteTrace(0, new SPDiagnosticsCategory(ex.Source, TraceSeverity.High, EventSeverity.Error), TraceSeverity.High, ex.Message, ex.Data);
                    //ex.ToString();
                }
            }
        }

        /// <summary>
        /// TODO: Add comment to describe the actions after feature installation
        /// </summary>
        /// <param name="properties">Properties of the feature</param>
        [SharePointPermission(SecurityAction.LinkDemand, ObjectModel = true)]
        public override void FeatureInstalled(SPFeatureReceiverProperties properties)
        {
            ////TODO: place receiver code here or remove method
        }

        /// <summary>
        /// TODO: Add comment to describe the actions during feature uninstalling
        /// </summary>
        /// <param name="properties">Properties of the feature</param>
        [SharePointPermission(SecurityAction.LinkDemand, ObjectModel = true)]
        public override void FeatureUninstalling(SPFeatureReceiverProperties properties)
        {
            ////TODO: place receiver code here or remove method
        }

        /// <summary>
        /// TODO: Add comment to describe the actions during feature upgrading
        /// </summary>
        /// <param name="properties">Properties of the feature</param>
        /// <param name="upgradeActionName">The name of the custom upgrade action to execute. The value can be null if the override of this method implements only one upgrade action.</param>
        /// <param name="parameters">Parameter names and values for the custom action</param>
        [SharePointPermission(SecurityAction.LinkDemand, ObjectModel = true)]
        public override void FeatureUpgrading(SPFeatureReceiverProperties properties, string upgradeActionName, System.Collections.Generic.IDictionary<string, string> parameters)
        {
            ////TODO: place receiver code here or remove method
        }

        private static SPUserToken GetSysToken(Guid SPSiteID)
        {
            SPUserToken sysToken = new SPSite(SPSiteID).SystemAccount.UserToken;
            if (sysToken == null)
            {
                SPSecurity.RunWithElevatedPrivileges(
                delegate()
                {
                    using (SPSite site = new SPSite(SPSiteID))
                    {
                        sysToken = site.SystemAccount.UserToken;
                    }
                });
            }
            return sysToken;
        }

        private void ConsistantLookup(SPWeb web, SPList list)
        {
            try
            {

                SPFieldLookup lookupField = (SPFieldLookup)list.Fields["Parent"];

                if (string.IsNullOrEmpty(lookupField.LookupList))
                {
                    if (lookupField.LookupWebId != web.ID)
                    {
                        lookupField.LookupWebId = web.ID;
                    }
                    if (lookupField.LookupList != list.ID.ToString())
                    {
                        lookupField.LookupList = list.ID.ToString();
                    }
                }
                else
                {
                    string schema = lookupField.SchemaXml;
                    schema = ReplaceXmlAttributeValue(schema, "List", list.ID.ToString());
                    schema = ReplaceXmlAttributeValue(schema, "WebId", web.ID.ToString());
                    lookupField.SchemaXml = schema;
                }
                lookupField.Update(true);
            }
            catch (Exception ex)
            {
                SPDiagnosticsService.Local.WriteTrace(0, new SPDiagnosticsCategory(ex.Source, TraceSeverity.High, EventSeverity.Error), TraceSeverity.High, ex.Message, ex.Data);
                //ex.ToString();
            }
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

