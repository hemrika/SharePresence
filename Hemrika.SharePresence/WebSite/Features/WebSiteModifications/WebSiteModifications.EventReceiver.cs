// <copyright file="WebSiteModifications.EventReceiver.cs" company="Hemrika">
// Copyright Hemrika. All rights reserved.
// </copyright>
// <author>MARKETING\Administrator</author>
// <date>2012-01-11 09:42:00Z</date>
namespace Hemrika.SharePresence.WebSite
{
    using System;
    using System.Collections.Generic;
    using System.Security.Permissions;
    using System.Text;
    using Microsoft.SharePoint;
    using Microsoft.SharePoint.Security;
    using Hemrika.SharePresence.Common.WebSiteController;
    using Microsoft.SharePoint.Administration;

    /// <summary>
    /// TODO: Add comment to WebSiteModificationsEventReceiver
    /// </summary>
    [SharePointPermission(SecurityAction.InheritanceDemand, ObjectModel = true)]
    public class WebSiteModificationsEventReceiver : SPFeatureReceiver
    {
        /// <summary>
        /// TODO: Add comment to describe the actions after feature activation
        /// </summary>
        /// <param name="properties">Properties of the feature</param>
        [SharePointPermission(SecurityAction.LinkDemand, ObjectModel = true)]
        public override void FeatureActivated(SPFeatureReceiverProperties properties)
        {
            RegisterModules(properties);
            UpdateUserProfile(properties);
        }

        private void UpdateUserProfile(SPFeatureReceiverProperties properties)
        {
            try
            {
                SPWeb web = properties.Feature.Parent as SPWeb;

                if (web.WebTemplate.ToLower() == "websiteroot")
                {
                    SPList users = web.Lists.TryGetList("User Information List");

                    if (users != null)
                    {
                        users.Fields.Add("Twitter Id", SPFieldType.Text, false);
                        users.Fields.Add("FaceBook Id", SPFieldType.Text, false);
                        users.Fields.Add("Google Id", SPFieldType.Text, false);
                        users.Fields.Add("LinkedIn Id", SPFieldType.Text, false);

                        users.Update(true);
                    }
                }
            }
            catch (Exception ex)
            {
                SPDiagnosticsService.Local.WriteTrace(0, new SPDiagnosticsCategory(ex.Source, TraceSeverity.High, EventSeverity.Error), TraceSeverity.High, ex.Message, ex.Data);
                //ex.ToString();
            }
        }

        private void RegisterModules(SPFeatureReceiverProperties properties)
        {
            try
            {
                SPWeb web = properties.Feature.Parent as SPWeb;

                if (web.WebTemplate.ToLower() == "websiteroot")
                {
                    //Hemrika.SharePresence.WebSite.Modules.WebPageModule.WebPageModule, Hemrika.SharePresence.WebSite, Version=1.0.0.0, Culture=neutral, PublicKeyToken=3421bd1d946bda6c
                    CreateWorkItem(web, "Hemrika.SharePresence.WebSite.Modules.WebPageModule.WebPageModule", "Hemrika.SharePresence.WebSite, Version=1.0.0.0, Culture=neutral, PublicKeyToken=3421bd1d946bda6c");
                    //Hemrika.SharePresence.WebSite.Modules.WebPageModule.WebPageModule, Hemrika.SharePresence.WebSite, Version=1.0.0.0, Culture=neutral, PublicKeyToken=3421bd1d946bda6c
                    //CreateWorkItem(web, "Hemrika.SharePresence.WebSite.Modules.LicenseModule.LicenseModule", "Hemrika.SharePresence.WebSite, Version=1.0.0.0, Culture=neutral, PublicKeyToken=3421bd1d946bda6c");
                    //Hemrika.SharePresence.WebSite.Modules.SemanticModule.SemanticModule, Hemrika.SharePresence.WebSite, Version=1.0.0.0, Culture=neutral, PublicKeyToken=3421bd1d946bda6c
                    CreateWorkItem(web, "Hemrika.SharePresence.WebSite.Modules.SemanticModule.SemanticModule", "Hemrika.SharePresence.WebSite, Version=1.0.0.0, Culture=neutral, PublicKeyToken=3421bd1d946bda6c");
                    //Hemrika.SharePresence.WebSite.Modules.Subscription.SubscriptioncModule, Hemrika.SharePresence.WebSite, Version=1.0.0.0, Culture=neutral, PublicKeyToken=3421bd1d946bda6c
                    //CreateWorkItem(web, "Hemrika.SharePresence.WebSite.Modules.Subscription.SubscriptionModule", "Hemrika.SharePresence.WebSite, Version=1.0.0.0, Culture=neutral, PublicKeyToken=3421bd1d946bda6c");
                    CreateWorkItem(web, "Hemrika.SharePresence.WebSite.Modules.MobileModule.MobileModule", "Hemrika.SharePresence.WebSite, Version=1.0.0.0, Culture=neutral, PublicKeyToken=3421bd1d946bda6c");
                }
            }
            catch (Exception ex)
            {
                SPDiagnosticsService.Local.WriteTrace(0, new SPDiagnosticsCategory(ex.Source, TraceSeverity.High, EventSeverity.Error), TraceSeverity.High, ex.Message, ex.Data);
                //ex.ToString();
            }
        }

        /// <summary>
        /// TODO: Add comment to describe the actions during feature deactivation
        /// </summary>
        /// <param name="properties">Properties of the feature</param>
        [SharePointPermission(SecurityAction.LinkDemand, ObjectModel = true)]
        public override void FeatureDeactivating(SPFeatureReceiverProperties properties)
        {
            ////TODO: place receiver code here or remove method
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
            UpdateUserProfile(properties);
            ////TODO: place receiver code here or remove method
        }

        private void CreateWorkItem(SPWeb web, string fullClass, string fullAssembly)
        {
            //SPSite tsite = properties.Feature.Parent as SPSite;

            Guid siteId = web.Site.ID;
            Guid webId = web.ID;
            string _modulename = fullClass;
            string _assembly = fullAssembly;


            SPSecurity.RunWithElevatedPrivileges(() =>
            {
                using (SPSite site = new SPSite(siteId))
                {
                    site.AddWorkItem(
                        Guid.NewGuid(),
                        DateTime.Now.ToUniversalTime(),
                        WebSiteControllerModuleWorkItem.WorkItemTypeId,
                        webId,
                        siteId,
                        1,
                        true,
                        Guid.Empty,
                        Guid.Empty,
                        site.SystemAccount.ID,
                        null,
                        _modulename + ";" + _assembly,
                        Guid.Empty
                        );
                }
            });

            try
            {

                WebSiteControllerModuleWorkItem WebSiteControllerModuleJob = new WebSiteControllerModuleWorkItem(WebSiteControllerModuleWorkItem.WorkItemJobDisplayName + _modulename, web.Site.WebApplication);
                SPOneTimeSchedule oneTimeSchedule = new SPOneTimeSchedule(DateTime.Now);

                WebSiteControllerModuleJob.Schedule = oneTimeSchedule;
                WebSiteControllerModuleJob.Update();
            }
            catch (Exception ex)
            {
                SPDiagnosticsService.Local.WriteTrace(0, new SPDiagnosticsCategory(ex.Source, TraceSeverity.High, EventSeverity.Error), TraceSeverity.High, ex.Message, ex.Data);
                //ex.ToString();
            }

        }

    }
}

