// <copyright file="WebSiteDefaultSettings.EventReceiver.cs" company="Hemrika">
// Copyright Hemrika. All rights reserved.
// </copyright>
// <author>MARKETING\Administrator</author>
// <date>2012-01-20 12:25:16Z</date>
namespace Hemrika.SharePresence.WebSite
{
    using System;
    using System.Collections.Generic;
    using System.Security.Permissions;
    using System.Text;
    using Microsoft.SharePoint;
    using Microsoft.SharePoint.Security;
    using Hemrika.SharePresence.Common.WebSiteController;
    using Microsoft.SharePoint.Navigation;
    using Microsoft.SharePoint.Utilities;
    using Hemrika.SharePresence.WebSite.Modules.SemanticModule;
    using System.Collections.ObjectModel;
    using Microsoft.SharePoint.Administration;

    /// <summary>
    /// TODO: Add comment to WebSiteDefaultSettingsEventReceiver
    /// </summary>
    [SharePointPermission(SecurityAction.InheritanceDemand, ObjectModel = true)]
    public class WebSiteDefaultSettingsEventReceiver : SPFeatureReceiver
    {
        /// <summary>
        /// TODO: Add comment to describe the actions after feature activation
        /// </summary>
        /// <param name="properties">Properties of the feature</param>
        [SharePointPermission(SecurityAction.LinkDemand, ObjectModel = true)]
        public override void FeatureActivated(SPFeatureReceiverProperties properties)
        {
            try
            {
                SPWeb web = properties.Feature.Parent as SPWeb;

                try
                {

                    web.CustomMasterUrl = "/_catalogs/masterpage/v5.master";
                    web.MasterUrl = "/_catalogs/masterpage/v5.master";
                    web.AnonymousState = SPWeb.WebAnonymousState.On;
                    web.AnonymousPermMask64 = SPBasePermissions.Open | SPBasePermissions.ViewPages | SPBasePermissions.ViewListItems;
                    web.AllProperties["__PublishingFeatureActivated"] = true.ToString();

                    web.Update();
                }
                catch (Exception ex)
                {
                    SPDiagnosticsService.Local.WriteTrace(0, new SPDiagnosticsCategory(ex.Source, TraceSeverity.High, EventSeverity.Error), TraceSeverity.High, ex.Message, ex.Data);
                    //ex.ToString();
                }

                try
                {
                    CreateWorkItem(web, "/Pages/default.aspx", web.ServerRelativeUrl);
                }
                catch (Exception ex)
                {
                    SPDiagnosticsService.Local.WriteTrace(0, new SPDiagnosticsCategory(ex.Source, TraceSeverity.High, EventSeverity.Error), TraceSeverity.High, ex.Message, ex.Data);
                    //ex.ToString();
                }

                try
                {
                    SetAvailableSubWebs(web);
                    web.Update();
                }
                catch (Exception ex)
                {
                    SPDiagnosticsService.Local.WriteTrace(0, new SPDiagnosticsCategory(ex.Source, TraceSeverity.High, EventSeverity.Error), TraceSeverity.High, ex.Message, ex.Data);
                    //ex.ToString();
                }

                try
                {
                    //web.RootFolder.WelcomePage = "Pages/Default.aspx";
                    //web.Update();
                    //web.AllowUnsafeUpdates = true;
                    SPFolder rootFolder = web.RootFolder;
                    rootFolder.WelcomePage = "Pages/default.aspx";
                    rootFolder.Update();
                    web.Update();
                    //web.AllowUnsafeUpdates = false;
                }
                catch (Exception ex)
                {
                    SPDiagnosticsService.Local.WriteTrace(0, new SPDiagnosticsCategory(ex.Source, TraceSeverity.High, EventSeverity.Error), TraceSeverity.High, ex.Message, ex.Data);
                    //ex.ToString();
                }

                try
                {
                    EnableFormsLockDown(web);
                }
                catch (Exception ex)
                {
                    SPDiagnosticsService.Local.WriteTrace(0, new SPDiagnosticsCategory(ex.Source, TraceSeverity.High, EventSeverity.Error), TraceSeverity.High, ex.Message, ex.Data);
                    //ex.ToString();
                }

            }
            catch (Exception ex)
            {
                SPDiagnosticsService.Local.WriteTrace(0, new SPDiagnosticsCategory(ex.Source, TraceSeverity.High, EventSeverity.Error), TraceSeverity.High, ex.Message, ex.Data);
                //ex.ToString();
            }

        }

        private void EnableFormsLockDown(SPWeb web)
        {
            try
            {

                //SPWeb rootWeb = site.RootWeb;
                SPRoleDefinition guestRole = web.RoleDefinitions.GetByType(SPRoleType.Guest);
                guestRole.BasePermissions &= ~(SPBasePermissions.EmptyMask | SPBasePermissions.ViewFormPages);
                guestRole.BasePermissions &= ~SPBasePermissions.UseRemoteAPIs;
                guestRole.BasePermissions |= SPBasePermissions.ViewVersions;
                guestRole.Update();
                SPBasePermissions guest = guestRole.BasePermissions;

                web.AnonymousPermMask64 &= ~(SPBasePermissions.UseRemoteAPIs | SPBasePermissions.ViewFormPages);
                web.Update();
                web.AnonymousPermMask64 |= SPBasePermissions.ViewVersions;
                web.Update();
                SPBasePermissions permissions = web.AnonymousPermMask64;
            }
            catch (Exception ex)
            {
                SPDiagnosticsService.Local.WriteTrace(0, new SPDiagnosticsCategory(ex.Source, TraceSeverity.High, EventSeverity.Error), TraceSeverity.High, ex.Message, ex.Data);
            }
        }

        private void SetAvailableSubWebs(SPWeb web)
        {
            if (!web.AllProperties.ContainsKey("__WebTemplates"))
            {
                web.AllProperties.Add("__WebTemplates", "");
            }

            web.AllowAllWebTemplates();

            web.Update();

            SPWebTemplateCollection existingLanguageNeutralTemplatesCollection = web.GetAvailableCrossLanguageWebTemplates();
            SPWebTemplateCollection existingLanguageSpecificTemplatesCollection = web.GetAvailableWebTemplates(web.Language);

            Collection<SPWebTemplate> newLanguageNeutralTemplatesCollection = new Collection<SPWebTemplate>();
            Collection<SPWebTemplate> newLanguageSpecificTemplatesCollection = new Collection<SPWebTemplate>();

            foreach (SPWebTemplate existingTemplate in existingLanguageNeutralTemplatesCollection)
            {
                if (existingTemplate.DisplayCategory != null && existingTemplate.DisplayCategory.Contains("WebSite"))
                {
                    newLanguageNeutralTemplatesCollection.Add(existingTemplate);
                }
            }
            foreach (SPWebTemplate existingTemplate in existingLanguageSpecificTemplatesCollection)
            {

                if (existingTemplate.DisplayCategory != null && existingTemplate.DisplayCategory.Contains("WebSite"))
                {
                    newLanguageSpecificTemplatesCollection.Add(existingTemplate);
                }
            }

            if (newLanguageNeutralTemplatesCollection.Count != 0)
            {
                web.SetAvailableCrossLanguageWebTemplates(newLanguageNeutralTemplatesCollection);

            }

            if (newLanguageSpecificTemplatesCollection.Count != 0)
            {
                web.SetAvailableWebTemplates(newLanguageSpecificTemplatesCollection, web.Language);
            }

            web.Update();

        }

        private void CreateWorkItem(SPWeb web, string pagename, string url)
        {
            Guid siteId = web.Site.ID;
            Guid webId = web.ID;
            //string url = properties.ServerRelativeUrl;
            /*
            if (url.StartsWith("/"))
            {
                url = url.TrimStart('/');
            }
            */

            bool disabled = false;
            WebSiteControllerPrincipalType principalType = WebSiteControllerPrincipalType.None;
            bool appliesToSSL = true;
            int sequence = 1;
            String pricipal = string.Empty;

            StringBuilder builder = new StringBuilder();
            builder.Append(url + ";");
            builder.Append(disabled.ToString() + ";");
            builder.Append(appliesToSSL.ToString() + ";");
            builder.Append(sequence.ToString() + ";");
            builder.Append(principalType.ToString() + ";");
            builder.Append(pricipal + ";");
            builder.Append("#");

            //builder.Append(String.Format("{0}:{1};", "OriginalUrl", url));
            //string full = builder.ToString();

            if (url.EndsWith("/"))
            {
                if (!pagename.EndsWith(".aspx"))
                {
                    builder.Append(String.Format("{0}:{1};", "OriginalUrl", pagename + ".aspx"));
                }
                else
                {
                    builder.Append(String.Format("{0}:{1};", "OriginalUrl", pagename));
                }
            }
            else
            {
                if (!url.EndsWith(".aspx"))
                {
                    builder.Append(String.Format("{0}:{1};", "OriginalUrl", url + pagename + ".aspx"));
                }
                else
                {
                    builder.Append(String.Format("{0}:{1};", "OriginalUrl", url + pagename));
                }
            }

            string full = builder.ToString();

            //Guid itemGuid = new Guid("386577D9-0777-4AD3-A90A-C240D8B0A49E");
            SemanticModule mod = new SemanticModule();
            IWebSiteControllerModule imod = null;// WebSiteControllerConfig.GetModule(web.Site.WebApplication, mod.RuleType);

            while (imod == null)
            {
                System.Threading.Thread.Sleep(1000);
                try
                {
                    imod = WebSiteControllerConfig.GetModule(web.Site.WebApplication, mod.RuleType);
                }
                catch (Exception ex)
                {
                    ex.ToString();
                }
            }


            int item = -1;

            SPSecurity.RunWithElevatedPrivileges(() =>
            {
                using (SPSite site = new SPSite(siteId))
                {
                    site.AddWorkItem(
                        Guid.NewGuid(),
                        DateTime.Now.ToUniversalTime(),
                        WebSiteControllerRuleWorkItem.WorkItemTypeId,
                        webId,
                        siteId,
                        item,
                        true,
                        imod.Id,
                        Guid.Empty,
                        site.SystemAccount.ID,
                        null,
                        builder.ToString(),
                        Guid.Empty
                        );
                }
            });

            SPSecurity.RunWithElevatedPrivileges(delegate()
            {
                using (SPSite site = new SPSite(siteId, SPUserToken.SystemAccount))
                {
                    try
                    {

                        WebSiteControllerRuleWorkItem WebSiteControllerModuleJob = new WebSiteControllerRuleWorkItem(WebSiteControllerRuleWorkItem.WorkItemJobDisplayName + "HomePage", site.WebApplication);
                        SPOneTimeSchedule oneTimeSchedule = new SPOneTimeSchedule(DateTime.Now);

                        WebSiteControllerModuleJob.Schedule = oneTimeSchedule;
                        WebSiteControllerModuleJob.Update();
                    }
                    catch { };
                }

                /*
                if (SPContext.Current != null)
                {
                    SPJobDefinitionCollection jobs = SPContext.Current.Site.WebApplication.JobDefinitions;

                    int _seconds = 0;

                    foreach (SPJobDefinition job in jobs)
                    {
                        if (job.Name == WebSiteControllerRuleWorkItem.WorkItemJobDisplayName)
                        {
                            DateTime next = job.Schedule.NextOccurrence(job.LastRunTime);
                            _seconds = next.Second;
                            break;
                        }
                    }
                }
                */
            });

            /*
            SPSecurity.RunWithElevatedPrivileges(() =>
            {
                using (SPSite site = new SPSite(siteId))
                {
                    site.AddWorkItem(
                        Guid.NewGuid(),
                        DateTime.Now.ToUniversalTime(),
                        WebSiteControllerRuleWorkItem.WorkItemTypeId,
                        webId,
                        siteId,
                        item,
                        true,
                        imod.Id,
                        Guid.Empty,
                        site.SystemAccount.ID,
                        null,
                        builder.ToString(),
                        Guid.Empty
                        );

                    try
                    {

                        WebSiteControllerRuleWorkItem WebSiteControllerModuleJob = new WebSiteControllerRuleWorkItem(WebSiteControllerRuleWorkItem.WorkItemJobDisplayName + "HomePage", site.WebApplication);
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

                SPJobDefinitionCollection jobs = SPContext.Current.Site.WebApplication.JobDefinitions;

                int _seconds = 0;

                foreach (SPJobDefinition job in jobs)
                {
                    if (job.Name == WebSiteControllerRuleWorkItem.WorkItemJobDisplayName)
                    {
                        DateTime next = job.Schedule.NextOccurrence(job.LastRunTime);
                        _seconds = next.Second;
                        break;
                    }
                }
            });
            */
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
            ////TODO: place receiver code here or remove method
        }
    }
}

