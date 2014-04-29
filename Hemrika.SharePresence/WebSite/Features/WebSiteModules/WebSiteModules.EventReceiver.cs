// <copyright file="WebSiteModules.EventReceiver.cs" company="Hemrika">
// Copyright Hemrika. All rights reserved.
// </copyright>
// <author>MARKETING\Administrator</author>
// <date>2011-11-02 12:54:25Z</date>
namespace Hemrika.SharePresence.WebSite
{
    using System;
    using System.Collections.Generic;
    using System.Security.Permissions;
    using System.Text;
    using Microsoft.SharePoint;
    using Microsoft.SharePoint.Security;
    using Microsoft.SharePoint.Administration;
    using Hemrika.SharePresence.Common.WebSiteController;
    using System.Web.Configuration;
    using Hemrika.SharePresence.Common;
    using Hemrika.SharePresence.Common.Extensions;
    using System.Threading;
    using Hemrika.SharePresence.WebSite.Bing;

    /// <summary>
    /// TODO: Add comment to WebSiteModulesEventReceiver
    /// </summary>
    [SharePointPermission(SecurityAction.InheritanceDemand, ObjectModel = true)]
    public class WebSiteModulesEventReceiver : SPFeatureReceiver
    {
        private SPWebApplication webApp = null;

        /// <summary>
        /// TODO: Add comment to describe the actions after feature activation
        /// </summary>
        /// <param name="properties">Properties of the feature</param>
        [SharePointPermission(SecurityAction.LinkDemand, ObjectModel = true)]
        public override void FeatureActivated(SPFeatureReceiverProperties properties)
        {
            FindWebApplication(properties);

            if (webApp != null)
            {
                CreateWebSiteConfig(properties);

                if (webApp != SPAdministrationWebApplication.Local)
                {
                    UpdateMimiMappings(properties);

                    ActivateModule_WebSiteModule(properties);

                    SPWebConfigModificationExtensions.WaitForOnetimeJobToFinish(webApp.Farm, "job-webconfig-modification", 180);

                    ActivateHttpModule_WebSiteModule(properties);

                    SPWebConfigModificationExtensions.WaitForOnetimeJobToFinish(webApp.Farm, "job-webconfig-modification", 180);

                    CreateJobs_WebSiteController(properties);

                    SPWebConfigModificationExtensions.WaitForOnetimeJobToFinish(webApp.Farm, "job-webconfig-modification", 180);

                    EnableAnonumous(properties);

                    SPWebConfigModificationExtensions.WaitForOnetimeJobToFinish(webApp.Farm, "job-webconfig-modification", 180);

                    //ActivateBrowserCaps(properties);

                    //SPWebConfigModificationExtensions.WaitForOnetimeJobToFinish(webApp.Farm, "job-webconfig-modification", 180);

                    /*
                    ActivateHttpHandler_Robots(properties);

                    SPWebConfigModificationExtensions.WaitForOnetimeJobToFinish(webApp.Farm, "job-webconfig-modification", 180);

                    ActivateHttpHandler_Bing(properties);

                    SPWebConfigModificationExtensions.WaitForOnetimeJobToFinish(webApp.Farm, "job-webconfig-modification", 180);

                    ActivateHttpHandler_SiteMap(properties);
                    
                    SPWebConfigModificationExtensions.WaitForOnetimeJobToFinish(webApp.Farm, "job-webconfig-modification", 180);

                    Activate_WebSiteProvider(properties);
                    */
                }
                else
                {
                    CreateDocIcons();
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

        private void UpdateMimiMappings(SPFeatureReceiverProperties properties)
        {
            try
            {
                if (webApp != null)
                {
                    SPMimeMappingCollection mappings = webApp.MimeMappings;
                    webApp.MimeMappings.Add("mp4", "Website.Video", "video/mp4");
                    webApp.MimeMappings.Add("ogg", "Website.Video", "video/ogg");
                    webApp.MimeMappings.Add("webm", "Website.Video", "video/webm");
                    webApp.MimeMappings.Add("flv", "Website.Video", "video/flv");
                    webApp.MimeMappings.Add("mpg", "Website.Video", "video/mpeg");
                    webApp.MimeMappings.Add("avi", "Website.Video", "video/x-msvideo");
                    webApp.MimeMappings.Add("mov", "Website.Video", "video/quicktime");
                    webApp.MimeMappings.Add("wmv", "Website.Video", "video/x-ms-wmv");
                    webApp.Update();
                }
            }
            catch (Exception ex)
            {
                SPDiagnosticsService.Local.WriteTrace(0, new SPDiagnosticsCategory(ex.Source, TraceSeverity.High, EventSeverity.Error), TraceSeverity.High, ex.Message, ex.Data);
                //ex.ToString();
            }

            try
            {
                Microsoft.Web.Administration.ServerManager serverManager = new Microsoft.Web.Administration.ServerManager();
            }
            catch (Exception)
            {

                throw;
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

            if (webApp != null)
            {

                if (webApp != SPAdministrationWebApplication.Local)
                {
                    SPWebConfigModificationExtensions.WaitForOnetimeJobToFinish(webApp.Farm, "job-webconfig-modification", 180);

                    DeactivateHttpModule_WebSiteModule(properties);

                    SPWebConfigModificationExtensions.WaitForOnetimeJobToFinish(webApp.Farm, "job-webconfig-modification", 180);

                    DeleteJob(webApp.JobDefinitions);

                    SPWebConfigModificationExtensions.WaitForOnetimeJobToFinish(webApp.Farm, "job-webconfig-modification", 180);

                    DeactivateBrowserCaps(properties);

                    SPWebConfigModificationExtensions.WaitForOnetimeJobToFinish(webApp.Farm, "job-webconfig-modification", 180);

                    /*
                    DeactivateHttpHandler_Robots(properties);

                    SPWebConfigModificationExtensions.WaitForOnetimeJobToFinish(webApp.Farm, "job-webconfig-modification", 180);

                    DeactivateHttpHandler_Bing(properties);

                    SPWebConfigModificationExtensions.WaitForOnetimeJobToFinish(webApp.Farm, "job-webconfig-modification", 180);

                    RemoveSettings_Bing(properties);

                    SPWebConfigModificationExtensions.WaitForOnetimeJobToFinish(webApp.Farm, "job-webconfig-modification", 180);

                    DeactivateHttpHandler_SiteMap(properties);

                    SPWebConfigModificationExtensions.WaitForOnetimeJobToFinish(webApp.Farm, "job-webconfig-modification", 180);

                    Deactivate_WebSiteProvider(properties);

                    SPWebConfigModificationExtensions.WaitForOnetimeJobToFinish(webApp.Farm, "job-webconfig-modification", 180);
                    */
                }
                else
                {
                    RemoveDocIcons();
                }

                SPWebConfigModificationExtensions.WaitForOnetimeJobToFinish(webApp.Farm, "job-webconfig-modification", 180);

                try
                {
                    SPWebConfigModificationExtensions.WaitForOnetimeJobToFinish(webApp.Farm, "job-webconfig-modification", 180);
                    webApp.Farm.Services.GetValue<SPWebService>().ApplyWebConfigModifications();
                    ////webApp.WebService.ApplyWebConfigModifications();
                    webApp.Update();
                }
                catch (Exception ex)
                {
                    SPDiagnosticsService.Local.WriteTrace(0, new SPDiagnosticsCategory(ex.Source, TraceSeverity.High, EventSeverity.Error), TraceSeverity.High, ex.Message, ex.Data);
                    //ex.ToString();
                }
            }
        }

        private void FindWebApplication(SPFeatureReceiverProperties properties)
        {
            SPFeatureScope scope = properties.Feature.Definition.Scope;

            if (scope == SPFeatureScope.Web)
            {
                SPWeb web = properties.Feature.Parent as SPWeb;
                webApp = web.Site.WebApplication;
            }


            if (scope == SPFeatureScope.Site)
            {
                SPSite site = properties.Feature.Parent as SPSite;
                webApp = site.WebApplication;
            }

            if (scope == SPFeatureScope.WebApplication)
            {
                webApp = properties.Feature.Parent as SPWebApplication;
            }
        }

        #region WebSiteController

        static object createlock = new object();

        private void CreateWebSiteConfig(SPFeatureReceiverProperties properties)
        {
            try
            {
                if (webApp != null)
                {

                    WebSiteControllerConfig config = null;
                    lock (createlock)
                    {

                        config = webApp.GetChild<WebSiteControllerConfig>(WebSiteControllerConfig.OBJECTNAME);
                        if (config == null)
                        {
                            WebSiteControllerConfig settings = new WebSiteControllerConfig(webApp, Guid.NewGuid());
                            settings.Update();
                        }
                    }

                    webApp.Update();
                }
            }
            catch (Exception ex)
            {
                SPDiagnosticsService.Local.WriteTrace(0, new SPDiagnosticsCategory(ex.Source, TraceSeverity.High, EventSeverity.Error), TraceSeverity.High, ex.Message, ex.Data);
                //ex.ToString();
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="properties"></param>
        private void Activate_WebSiteProvider(SPFeatureReceiverProperties properties)
        {
            if (webApp != null)
            {
                SPWebConfigModificationExtensions.WaitForOnetimeJobToFinish(webApp.Farm, "job-webconfig-modification", 180);

                SPWebConfigModification mod = new SPWebConfigModification("add[@type='Hemrika.SharePresence.WebSite.Navigation.WebSiteProvider, Hemrika.SharePresence.WebSite, Version=1.0.0.0, Culture=neutral, PublicKeyToken=11e6604a27f32a11']", "configuration/system.web/siteMap/providers");
                mod.Owner = properties.Feature.DefinitionId.ToString();
                mod.Sequence = 0;
                mod.Type = SPWebConfigModification.SPWebConfigModificationType.EnsureChildNode;
                mod.Value = @"<add name=""WebSiteProvider"" type=""Hemrika.SharePresence.WebSite.Navigation.WebSiteProvider, Hemrika.SharePresence.WebSite, Version=1.0.0.0, Culture=neutral, PublicKeyToken=11e6604a27f32a11"" />";
                if (!webApp.WebConfigModifications.Contains(mod))
                {
                    webApp.WebConfigModifications.Add(mod);
                }

                try
                {
                    SPWebConfigModificationExtensions.WaitForOnetimeJobToFinish(webApp.Farm, "job-webconfig-modification", 180);
                    webApp.Farm.Services.GetValue<SPWebService>().ApplyWebConfigModifications();
                    ////webApp.WebService.ApplyWebConfigModifications();
                    webApp.Update();
                }
                catch (Exception ex)
                {
                    SPDiagnosticsService.Local.WriteTrace(0, new SPDiagnosticsCategory(ex.Source, TraceSeverity.High, EventSeverity.Error), TraceSeverity.High, ex.Message, ex.Data);
                    //ex.ToString();
                }
            }
        }

        private void ActivateModule_WebSiteModule(SPFeatureReceiverProperties properties)
        {
            if (webApp != null)
            {
                SPWebConfigModification mod = new SPWebConfigModification("add[@type='Hemrika.SharePresence.Common.WebSiteController.WebSiteControllerModule, Hemrika.SharePresence.Common, Version=1.0.0.0, Culture=neutral, PublicKeyToken=11e6604a27f32a11']", "configuration/system.webServer/modules");
                mod.Owner = properties.Feature.DefinitionId.ToString();
                mod.Sequence = 0;
                mod.Type = SPWebConfigModification.SPWebConfigModificationType.EnsureChildNode;
                mod.Value = @"<add name=""WebSiteController"" type=""Hemrika.SharePresence.Common.WebSiteController.WebSiteControllerModule, Hemrika.SharePresence.Common, Version=1.0.0.0, Culture=neutral, PublicKeyToken=11e6604a27f32a11"" />";
                if (!webApp.WebConfigModifications.Contains(mod))
                {
                    webApp.WebConfigModifications.Add(mod);
                }


                try
                {
                    SPWebConfigModificationExtensions.WaitForOnetimeJobToFinish(webApp.Farm, "job-webconfig-modification", 180);
                    webApp.Farm.Services.GetValue<SPWebService>().ApplyWebConfigModifications();
                    ////webApp.WebService.ApplyWebConfigModifications();
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
        /// Registers the HTTP Module in web.config
        /// </summary>
        /// <param name="properties">Properties of the feature</param>
        private void ActivateHttpModule_WebSiteModule(SPFeatureReceiverProperties properties)
        {
            if (webApp != null)
            {
                SPWebConfigModificationExtensions.WaitForOnetimeJobToFinish(webApp.Farm, "job-webconfig-modification", 180);

                SPWebConfigModification mod = new SPWebConfigModification("add[@type='Hemrika.SharePresence.Common.WebSiteController.WebSiteControllerModule, Hemrika.SharePresence.Common, Version=1.0.0.0, Culture=neutral, PublicKeyToken=11e6604a27f32a11']", "configuration/system.web/httpModules");
                mod.Owner = properties.Feature.DefinitionId.ToString();
                mod.Sequence = 0;
                mod.Type = SPWebConfigModification.SPWebConfigModificationType.EnsureChildNode;
                mod.Value = @"<add name=""WebSiteController"" type=""Hemrika.SharePresence.Common.WebSiteController.WebSiteControllerModule, Hemrika.SharePresence.Common, Version=1.0.0.0, Culture=neutral, PublicKeyToken=11e6604a27f32a11"" />";
                if (!webApp.WebConfigModifications.Contains(mod))
                {
                    webApp.WebConfigModifications.Add(mod);
                }


                try
                {
                    SPWebConfigModificationExtensions.WaitForOnetimeJobToFinish(webApp.Farm, "job-webconfig-modification", 180);
                    webApp.Farm.Services.GetValue<SPWebService>().ApplyWebConfigModifications();
                    ////webApp.WebService.ApplyWebConfigModifications();
                    webApp.Update();
                }
                catch (Exception ex)
                {
                    SPDiagnosticsService.Local.WriteTrace(0, new SPDiagnosticsCategory(ex.Source, TraceSeverity.High, EventSeverity.Error), TraceSeverity.High, ex.Message, ex.Data);
                    //ex.ToString();
                }
            }
        }

        private void CreateJobs_WebSiteController(SPFeatureReceiverProperties properties)
        {
            if (webApp != null)
            {

                DeleteJob(webApp.JobDefinitions);
                try
                {

                    WebSiteControllerModuleWorkItem WebSiteControllerModuleJob = new WebSiteControllerModuleWorkItem(WebSiteControllerModuleWorkItem.WorkItemJobDisplayName, webApp);
                    SPMinuteSchedule moduleschedule = new SPMinuteSchedule();
                    moduleschedule.BeginSecond = 0;
                    moduleschedule.EndSecond = 59;
                    moduleschedule.Interval = 1;

                    WebSiteControllerModuleJob.Schedule = moduleschedule;
                    WebSiteControllerModuleJob.Update();
                }
                catch (Exception ex)
                {
                    SPDiagnosticsService.Local.WriteTrace(0, new SPDiagnosticsCategory(ex.Source, TraceSeverity.High, EventSeverity.Error), TraceSeverity.High, ex.Message, ex.Data);
                    //ex.ToString();
                }

                try
                {
                    WebSiteControllerRuleWorkItem WebSiteControllerRuleJob = new WebSiteControllerRuleWorkItem(WebSiteControllerRuleWorkItem.WorkItemJobDisplayName, webApp);
                    SPMinuteSchedule ruleschedule = new SPMinuteSchedule();
                    ruleschedule.BeginSecond = 0;
                    ruleschedule.EndSecond = 59;
                    ruleschedule.Interval = 1;

                    WebSiteControllerRuleJob.Schedule = ruleschedule;
                    WebSiteControllerRuleJob.Update();
                }
                catch (Exception ex)
                {
                    SPDiagnosticsService.Local.WriteTrace(0, new SPDiagnosticsCategory(ex.Source, TraceSeverity.High, EventSeverity.Error), TraceSeverity.High, ex.Message, ex.Data);
                    //ex.ToString();
                }
            }
        }

        private void DeleteJob(SPJobDefinitionCollection jobs)
        {
            if (webApp != null)
            {

                foreach (SPJobDefinition job in jobs)
                {
                    if (job.Id.Equals(WebSiteControllerModuleWorkItem.WorkItemTypeId))
                    {
                        try
                        {
                            job.Delete();
                        }
                        catch (Exception ex)
                        {
                            SPDiagnosticsService.Local.WriteTrace(0, new SPDiagnosticsCategory(ex.Source, TraceSeverity.High, EventSeverity.Error), TraceSeverity.High, ex.Message, ex.Data);
                            //ex.ToString();
                        }
                    }
                }
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="properties"></param>
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

                try
                {
                    SPWebConfigModificationExtensions.WaitForOnetimeJobToFinish(webApp.Farm, "job-webconfig-modification", 180);
                    webApp.Farm.Services.GetValue<SPWebService>().ApplyWebConfigModifications();
                    ////webApp.WebService.ApplyWebConfigModifications();
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
        /// Unregisters the HTTP Module in web.config
        /// </summary>
        /// <param name="properties">Properties of the feature</param>
        private void DeactivateHttpModule_WebSiteModule(SPFeatureReceiverProperties properties)
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

                try
                {
                    SPWebConfigModificationExtensions.WaitForOnetimeJobToFinish(webApp.Farm, "job-webconfig-modification", 180);
                    webApp.Farm.Services.GetValue<SPWebService>().ApplyWebConfigModifications();
                    ////webApp.WebService.ApplyWebConfigModifications();
                    webApp.Update();
                }
                catch (Exception ex)
                {
                    SPDiagnosticsService.Local.WriteTrace(0, new SPDiagnosticsCategory(ex.Source, TraceSeverity.High, EventSeverity.Error), TraceSeverity.High, ex.Message, ex.Data);
                    //ex.ToString();
                }
            }
        }

        #endregion

        #region DocIcons

        private void CreateDocIcons()
        {
            try
            {
                ToggleDocIcons(false);
                /*
                this.CreateJobs_DocIcons(DocIconType.ByProgID, "WebSite.WebSitePageMaster", "icmaster.gif", "Microsoft SharePoint Designer", "SharePoint.OpenDocuments", false);
                Thread.Sleep(1000);
                this.CreateJobs_DocIcons(DocIconType.ByProgID, "WebSite.WebSitePageLayout", "icdocset.gif", "Microsoft SharePoint Designer", "SharePoint.OpenDocuments", false);
                Thread.Sleep(1000);
                this.CreateJobs_DocIcons(DocIconType.ByProgID, "WebSite.WebSitePage", "icsmrtpg.gif", "Microsoft SharePoint Designer", "SharePoint.OpenDocuments", false);
                Thread.Sleep(1000);
                this.CreateJobs_DocIcons(DocIconType.ByProgID, "WebSite.Video", "icsmrtpg.gif", "Microsoft SharePoint Designer", "SharePoint.OpenDocuments", false);
                */
            }
            catch (Exception ex)
            {
                SPDiagnosticsService.Local.WriteTrace(0, new SPDiagnosticsCategory(ex.Source, TraceSeverity.High, EventSeverity.Error), TraceSeverity.High, ex.Message, ex.Data);
                //ex.ToString();
            }
        }

        private void RemoveDocIcons()
        {
            if (webApp != null)
            {

                try
                {
                    ToggleDocIcons(true);

                }
                catch (Exception ex)
                {
                    SPDiagnosticsService.Local.WriteTrace(0, new SPDiagnosticsCategory(ex.Source, TraceSeverity.High, EventSeverity.Error), TraceSeverity.High, ex.Message, ex.Data);
                    //ex.ToString();
                }
            }
        }

        private void ToggleDocIcons(bool remove)
        {
            this.CreateJobs_DocIcons(DocIconType.ByProgID, "WebSite.WebSitePageMaster", "icmaster.gif", "Microsoft SharePoint Designer", "SharePoint.OpenDocuments", remove);
            Thread.Sleep(1000);
            this.CreateJobs_DocIcons(DocIconType.ByProgID, "WebSite.WebSitePageLayout", "icdocset.gif", "Microsoft SharePoint Designer", "SharePoint.OpenDocuments", remove);
            Thread.Sleep(1000);
            this.CreateJobs_DocIcons(DocIconType.ByProgID, "WebSite.WebSitePage", "icsmrtpg.gif", "Microsoft SharePoint Designer", "SharePoint.OpenDocuments", remove);
            Thread.Sleep(1000);
            this.CreateJobs_DocIcons(DocIconType.ByProgID, "WebSite.Video", "icsmrtpg.gif", "Video Editor", "SharePoint.OpenDocuments", remove);
            Thread.Sleep(1000);
            this.CreateJobs_DocIcons(DocIconType.ByExtension, "3gp", "3gp.png", "3gp File", "SharePoint.OpenDocuments", remove);
            Thread.Sleep(1000);
            this.CreateJobs_DocIcons(DocIconType.ByExtension, "7z", "7z.png", "7z File", "SharePoint.OpenDocuments", remove);
            Thread.Sleep(1000);
            this.CreateJobs_DocIcons(DocIconType.ByExtension, "avi", "avi.png", "Avi File", "SharePoint.OpenDocuments", remove);
            Thread.Sleep(1000);
            this.CreateJobs_DocIcons(DocIconType.ByExtension, "css", "css.png", "StyleSheet File", "SharePoint.OpenDocuments", remove);
            Thread.Sleep(1000);
            this.CreateJobs_DocIcons(DocIconType.ByExtension, "fla", "fla.png", "Flash File", "SharePoint.OpenDocuments", remove);
            Thread.Sleep(1000);
            this.CreateJobs_DocIcons(DocIconType.ByExtension, "flv", "flv.png", "Flash Movie", "SharePoint.OpenDocuments", remove);
            Thread.Sleep(1000);
            this.CreateJobs_DocIcons(DocIconType.ByExtension, "gz", "gz.png", "gZip File", "SharePoint.OpenDocuments", remove);
            Thread.Sleep(1000);
            this.CreateJobs_DocIcons(DocIconType.ByExtension, "js", "js.png", "JavaScript File", "SharePoint.OpenDocuments", remove);
            Thread.Sleep(1000);
            this.CreateJobs_DocIcons(DocIconType.ByExtension, "mp3", "mp3.png", "MP3 Music", "SharePoint.OpenDocuments", remove);
            Thread.Sleep(1000);
            this.CreateJobs_DocIcons(DocIconType.ByExtension, "mp4", "mp4.png", "MP4 Movie", "SharePoint.OpenDocuments", remove);
            Thread.Sleep(1000);
            this.CreateJobs_DocIcons(DocIconType.ByExtension, "mpg", "mpg.png", "MPG Movie", "SharePoint.OpenDocuments", remove);
            Thread.Sleep(1000);
            this.CreateJobs_DocIcons(DocIconType.ByExtension, "ogg", "ogg.png", "OGG Audio", "SharePoint.OpenDocuments", remove);
            Thread.Sleep(1000);
            this.CreateJobs_DocIcons(DocIconType.ByExtension, "pdf", "pdf.png", "PDF File", "SharePoint.OpenDocuments", remove);
            Thread.Sleep(1000);
            this.CreateJobs_DocIcons(DocIconType.ByExtension, "tar", "tar.png", "TAR File", "SharePoint.OpenDocuments", remove);
            Thread.Sleep(1000);
            this.CreateJobs_DocIcons(DocIconType.ByExtension, "swf", "swf.png", "ShockWave Flash File", "SharePoint.OpenDocuments", remove);
            Thread.Sleep(1000);
            this.CreateJobs_DocIcons(DocIconType.ByExtension, "zip", "zip.png", "ZIP File", "SharePoint.OpenDocuments", remove);
            Thread.Sleep(1000);
            this.CreateJobs_DocIcons(DocIconType.ByExtension, "ogv", "ogg.png", "OGG Movie", "SharePoint.OpenDocuments", remove);
            Thread.Sleep(1000);
            this.CreateJobs_DocIcons(DocIconType.ByExtension, "webm", "ogg.png", "WebM Movie", "SharePoint.OpenDocuments", remove);

        }

        private void CreateJobs_DocIcons(DocIconType docType, string key, string value, string editText, string openControl, bool delete)
        {
            if (webApp != null)
            {

                string serviceName = "WSS_Administration";
                SPFarm farm = SPFarm.Local;

                WebSiteDocItemModifier docJob = null;

                foreach (SPService service in farm.Services)
                {
                    if (service.Name == serviceName)
                    {

                        docJob = new WebSiteDocItemModifier(service, docType, key, value, editText, openControl, delete);

                        SPJobDefinition def = service.GetJobDefinitionByName(docJob.Name);
                        if (def != null)
                        {
                            def.Delete();
                        }

                        break;
                    }
                }

                docJob.Schedule = new SPOneTimeSchedule(DateTime.Now);
                docJob.Title = string.Format("Modify {0} Icon for Mapping {1} in {2} section.", value, key, docType.ToString());
                docJob.Update();
                DateTime runtime;
                DateTime.TryParse(docJob.LastRunTime.ToString(), out runtime);
                while (runtime != null && (runtime == DateTime.MinValue || runtime == DateTime.MaxValue))
                {
                    DateTime.TryParse(docJob.LastRunTime.ToString(), out runtime);
                }
            }
        }

        #endregion

        #region EnableAnonumous

        private void EnableAnonumous(SPFeatureReceiverProperties properties)
        {
            try
            {
                if (webApp != null)
                {

                    SPUrlZone urlZone = SPUrlZone.Default;
                    //SPWebApplication specifiedWebApplication = specifiedSite.WebApplication;
                    SPIisSettings iisSettings = webApp.IisSettings[urlZone];
                    iisSettings.AuthenticationMode = AuthenticationMode.Windows;
                    iisSettings.AllowAnonymous = true;

                    try
                    {
                        SPWebConfigModificationExtensions.WaitForOnetimeJobToFinish(webApp.Farm, "job-webconfig-modification", 180);
                        webApp.Farm.Services.GetValue<SPWebService>().ApplyWebConfigModifications();
                        ////webApp.WebService.ApplyWebConfigModifications();
                        webApp.Update();
                    }
                    catch (Exception ex)
                    {
                        SPDiagnosticsService.Local.WriteTrace(0, new SPDiagnosticsCategory(ex.Source, TraceSeverity.High, EventSeverity.Error), TraceSeverity.High, ex.Message, ex.Data);
                        //ex.ToString();
                    }
                }
            }
            catch (Exception ex)
            {
                SPDiagnosticsService.Local.WriteTrace(0, new SPDiagnosticsCategory(ex.Source, TraceSeverity.High, EventSeverity.Error), TraceSeverity.High, ex.Message, ex.Data);
                //ex.ToString();
            }
        }

        #endregion

        #region Bing

        private void ActivateHttpHandler_Bing(SPFeatureReceiverProperties properties)
        {
            if (webApp != null)
            {
                SPWebConfigModificationExtensions.WaitForOnetimeJobToFinish(webApp.Farm, "job-webconfig-modification", 180);

                SPWebConfigModification mod = new SPWebConfigModification("add[@type='Hemrika.SharePresence.WebSite.Bing.BingHandler, Hemrika.SharePresence.WebSite, Version=1.0.0.0, Culture=neutral, PublicKeyToken=11e6604a27f32a11']", "configuration/system.webServer/handlers");
                mod.Owner = properties.Feature.DefinitionId.ToString();
                mod.Sequence = 0;
                mod.Type = SPWebConfigModification.SPWebConfigModificationType.EnsureChildNode;
                mod.Value = @"<add name=""BingHandler"" path=""BingSiteAuth.xml"" verb=""*"" type=""Hemrika.SharePresence.WebSite.Bing.BingHandler, Hemrika.SharePresence.WebSite, Version=1.0.0.0, Culture=neutral, PublicKeyToken=11e6604a27f32a11"" />";
                if (!webApp.WebConfigModifications.Contains(mod))
                {
                    webApp.WebConfigModifications.Add(mod);
                }


                try
                {
                    SPWebConfigModificationExtensions.WaitForOnetimeJobToFinish(webApp.Farm, "job-webconfig-modification", 180);
                    webApp.Farm.Services.GetValue<SPWebService>().ApplyWebConfigModifications();
                    ////webApp.WebService.ApplyWebConfigModifications();
                    webApp.Update();

                    webApp.BlockedFileExtensions.Remove("xml");
                    webApp.Update();
                }
                catch (Exception ex)
                {
                    SPDiagnosticsService.Local.WriteTrace(0, new SPDiagnosticsCategory(ex.Source, TraceSeverity.High, EventSeverity.Error), TraceSeverity.High, ex.Message, ex.Data);
                    //ex.ToString();
                }
            }
        }
        private void RemoveSettings_Bing(SPFeatureReceiverProperties properties)
        {
            if (webApp != null)
            {

                BingSettings settings = new BingSettings();
                foreach (SPSite site in webApp.Sites)
                {
                    try
                    {
                        settings.Remove(site);
                    }
                    catch (Exception ex)
                    {
                        SPDiagnosticsService.Local.WriteTrace(0, new SPDiagnosticsCategory(ex.Source, TraceSeverity.High, EventSeverity.Error), TraceSeverity.High, ex.Message, ex.Data);
                        //ex.ToString();
                    }
                }
            }
        }

        private void DeactivateHttpHandler_Bing(SPFeatureReceiverProperties properties)
        {
            if (webApp != null)
            {

                foreach (SPWebConfigModification mod in webApp.WebConfigModifications)
                {
                    if (mod.Owner == properties.Feature.DefinitionId.ToString())
                    {
                        try
                        {
                            webApp.WebConfigModifications.Remove(mod);
                            break;
                        }
                        catch (Exception ex)
                        {
                            SPDiagnosticsService.Local.WriteTrace(0, new SPDiagnosticsCategory(ex.Source, TraceSeverity.High, EventSeverity.Error), TraceSeverity.High, ex.Message, ex.Data);
                            //ex.ToString();
                        }
                    }
                }

                try
                {
                    SPWebConfigModificationExtensions.WaitForOnetimeJobToFinish(webApp.Farm, "job-webconfig-modification", 180);
                    webApp.Farm.Services.GetValue<SPWebService>().ApplyWebConfigModifications();
                    ////webApp.WebService.ApplyWebConfigModifications();
                    webApp.Update();
                }
                catch (Exception ex)
                {
                    SPDiagnosticsService.Local.WriteTrace(0, new SPDiagnosticsCategory(ex.Source, TraceSeverity.High, EventSeverity.Error), TraceSeverity.High, ex.Message, ex.Data);
                    //ex.ToString();
                }
            }
        }
        #endregion

        #region Robots

        private void ActivateHttpHandler_Robots(SPFeatureReceiverProperties properties)
        {

            if (webApp != null)
            {
                SPWebConfigModificationExtensions.WaitForOnetimeJobToFinish(webApp.Farm, "job-webconfig-modification", 180);

                SPWebConfigModification mod =
                    new SPWebConfigModification(
                        "add[@type='Hemrika.SharePresence.WebSite.Robots.RobotsHandler, Hemrika.SharePresence.WebSite, Version=1.0.0.0, Culture=neutral, PublicKeyToken=11e6604a27f32a11']",
                        "configuration/system.webServer/handlers");
                mod.Owner = properties.Feature.DefinitionId.ToString();
                mod.Sequence = 0;
                mod.Type = SPWebConfigModification.SPWebConfigModificationType.EnsureChildNode;
                mod.Value =
                    @"<add name=""RobotsHandler"" path=""robots.txt"" verb=""*"" type=""Hemrika.SharePresence.WebSite.Robots.RobotsHandler, Hemrika.SharePresence.WebSite, Version=1.0.0.0, Culture=neutral, PublicKeyToken=11e6604a27f32a11"" />";
                if (!webApp.WebConfigModifications.Contains(mod))
                {
                    webApp.WebConfigModifications.Add(mod);
                }


                try
                {
                    SPWebConfigModificationExtensions.WaitForOnetimeJobToFinish(webApp.Farm, "job-webconfig-modification", 180);
                    webApp.Farm.Services.GetValue<SPWebService>().ApplyWebConfigModifications();
                    ////webApp.WebService.ApplyWebConfigModifications();
                    webApp.Update();

                    webApp.BlockedFileExtensions.Remove("txt");
                    webApp.Update();
                }
                catch (Exception ex)
                {
                    SPDiagnosticsService.Local.WriteTrace(0, new SPDiagnosticsCategory(ex.Source, TraceSeverity.High, EventSeverity.Error), TraceSeverity.High, ex.Message, ex.Data);
                    //ex.ToString();
                }
            }
        }

        private void DeactivateHttpHandler_Robots(SPFeatureReceiverProperties properties)
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

                try
                {
                    SPWebConfigModificationExtensions.WaitForOnetimeJobToFinish(webApp.Farm, "job-webconfig-modification", 180);
                    webApp.Farm.Services.GetValue<SPWebService>().ApplyWebConfigModifications();
                    ////webApp.WebService.ApplyWebConfigModifications();
                    webApp.Update();
                }
                catch (Exception ex)
                {
                    SPDiagnosticsService.Local.WriteTrace(0, new SPDiagnosticsCategory(ex.Source, TraceSeverity.High, EventSeverity.Error), TraceSeverity.High, ex.Message, ex.Data);
                    //ex.ToString();
                }

                foreach (SPSite site in webApp.Sites)
                {
                    foreach (SPWeb allWeb in site.AllWebs)
                    {
                        SPWeb web = site.OpenWeb(allWeb.ID);
                        web.AllowUnsafeUpdates = true;
                        web.Update();
                        try
                        {
                            SPList robots = web.Lists["Robots"];
                            web.Lists.Delete(robots.ID);
                            web.Update();
                        }
                        catch (Exception ex)
                        {
                            SPDiagnosticsService.Local.WriteTrace(0, new SPDiagnosticsCategory(ex.Source, TraceSeverity.High, EventSeverity.Error), TraceSeverity.High, ex.Message, ex.Data);
                            //ex.ToString();
                        }
                        web.Update();
                        web.AllowUnsafeUpdates = false;
                    }
                }
            }
        }

        #endregion

        #region SiteMap

        private void ActivateHttpHandler_SiteMap(SPFeatureReceiverProperties properties)
        {

            if (webApp != null)
            {
                SPWebConfigModificationExtensions.WaitForOnetimeJobToFinish(webApp.Farm, "job-webconfig-modification", 180);

                SPWebConfigModification mod =
                    new SPWebConfigModification(
                        "add[@type='Hemrika.SharePresence.WebSite.SiteMap.SiteMapHandler, Hemrika.SharePresence.WebSite, Version=1.0.0.0, Culture=neutral, PublicKeyToken=11e6604a27f32a11']",
                        "configuration/system.webServer/handlers");
                mod.Owner = properties.Feature.DefinitionId.ToString();
                mod.Sequence = 0;
                mod.Type = SPWebConfigModification.SPWebConfigModificationType.EnsureChildNode;
                mod.Value =
                    @"<add name=""SiteMapHandler"" path=""*map.xml"" verb=""*"" type=""Hemrika.SharePresence.WebSite.SiteMap.SiteMapHandler, Hemrika.SharePresence.WebSite, Version=1.0.0.0, Culture=neutral, PublicKeyToken=11e6604a27f32a11"" />";
                if (!webApp.WebConfigModifications.Contains(mod))
                {
                    webApp.WebConfigModifications.Add(mod);
                }

                webApp.BlockedFileExtensions.Remove("xml");

                try
                {
                    SPWebConfigModificationExtensions.WaitForOnetimeJobToFinish(webApp.Farm, "job-webconfig-modification", 180);
                    webApp.Farm.Services.GetValue<SPWebService>().ApplyWebConfigModifications();
                    ////webApp.WebService.ApplyWebConfigModifications();
                    webApp.Update();
                }
                catch (Exception ex)
                {
                    SPDiagnosticsService.Local.WriteTrace(0, new SPDiagnosticsCategory(ex.Source, TraceSeverity.High, EventSeverity.Error), TraceSeverity.High, ex.Message, ex.Data);
                    //ex.ToString();
                }
            }
        }

        private void DeactivateHttpHandler_SiteMap(SPFeatureReceiverProperties properties)
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

                try
                {
                    SPWebConfigModificationExtensions.WaitForOnetimeJobToFinish(webApp.Farm, "job-webconfig-modification", 180);
                    webApp.Farm.Services.GetValue<SPWebService>().ApplyWebConfigModifications();
                    ////webApp.WebService.ApplyWebConfigModifications();
                    webApp.Update();
                }
                catch (Exception ex)
                {
                    SPDiagnosticsService.Local.WriteTrace(0, new SPDiagnosticsCategory(ex.Source, TraceSeverity.High, EventSeverity.Error), TraceSeverity.High, ex.Message, ex.Data);
                    //ex.ToString();
                }
            }
        }

        #endregion

        private void ActivateBrowserCaps(SPFeatureReceiverProperties properties)
        {
            if (webApp != null)
            {
                SPWebConfigModificationExtensions.WaitForOnetimeJobToFinish(webApp.Farm, "job-webconfig-modification", 180);
                /*
                <browserCaps>
                    <result type="System.Web.Mobile.MobileCapabilities, System.Web.Mobile, Version=2.0.0.0, Culture=neutral, PublicKeyToken=b03f5f7f11d50a3a" />
                    <filter>isMobileDevice=false</filter>
                </browserCaps>
                */

                SPWebConfigModification mod = new SPWebConfigModification("add[@type='System.Web.Mobile.MobileCapabilities, System.Web.Mobile, Version=2.0.0.0, Culture=neutral, PublicKeyToken=b03f5f7f11d50a3a']", "configuration/system.web");
                mod.Owner = properties.Feature.DefinitionId.ToString();
                mod.Sequence = 0;
                mod.Type = SPWebConfigModification.SPWebConfigModificationType.EnsureChildNode;
                mod.Value = @"<browserCaps><result type=""System.Web.Mobile.MobileCapabilities, System.Web.Mobile, Version=2.0.0.0, Culture=neutral, PublicKeyToken=b03f5f7f11d50a3a"" /><filter>isMobileDevice=false</filter></browserCaps>";
                if (!webApp.WebConfigModifications.Contains(mod))
                {
                    webApp.WebConfigModifications.Add(mod);
                }


                try
                {
                    SPWebConfigModificationExtensions.WaitForOnetimeJobToFinish(webApp.Farm, "job-webconfig-modification", 180);
                    webApp.Farm.Services.GetValue<SPWebService>().ApplyWebConfigModifications();
                    ////webApp.WebService.ApplyWebConfigModifications();
                    webApp.Update();
                }
                catch (Exception ex)
                {
                    SPDiagnosticsService.Local.WriteTrace(0, new SPDiagnosticsCategory(ex.Source, TraceSeverity.High, EventSeverity.Error), TraceSeverity.High, ex.Message, ex.Data);
                    //ex.ToString();
                }
            }
        }

        private void DeactivateBrowserCaps(SPFeatureReceiverProperties properties)
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

                try
                {
                    SPWebConfigModificationExtensions.WaitForOnetimeJobToFinish(webApp.Farm, "job-webconfig-modification", 180);
                    webApp.Farm.Services.GetValue<SPWebService>().ApplyWebConfigModifications();
                    ////webApp.WebService.ApplyWebConfigModifications();
                    webApp.Update();
                }
                catch (Exception ex)
                {
                    SPDiagnosticsService.Local.WriteTrace(0, new SPDiagnosticsCategory(ex.Source, TraceSeverity.High, EventSeverity.Error), TraceSeverity.High, ex.Message, ex.Data);
                    //ex.ToString();
                }
            }
        }
    }
}
