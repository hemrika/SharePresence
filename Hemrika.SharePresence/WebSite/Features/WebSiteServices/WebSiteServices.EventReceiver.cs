// <copyright file="WebSiteServices.EventReceiver.cs" company="Hemrika">
// Copyright Hemrika. All rights reserved.
// </copyright>
// <author>MARKETING\Administrator</author>
// <date>2011-10-22 19:52:45Z</date>
namespace Hemrika.SharePresence.WebSite
{
    using System;
    using System.Collections.Generic;
    using System.Security.Permissions;
    using System.Text;
    using Microsoft.SharePoint;
    using Microsoft.SharePoint.Security;
    using Hemrika.SharePresence.WebSite.Layout;
    using Hemrika.SharePresence.Common.ServiceLocation;
    using Hemrika.SharePresence.Common;
    using Hemrika.SharePresence.Common.Configuration;
    using Hemrika.SharePresence.WebSite.Master;
    using Hemrika.SharePresence.Common.License.LicenseProxy;
    using Hemrika.SharePresence.Common.WebSiteController;
    using Microsoft.SharePoint.Administration;
    using Hemrika.SharePresence.Common.ProxyArgs;
    using Microsoft.SharePoint.UserCode;
    using Hemrika.SharePresence.WebSite.MetaData.Keywords;

    /// <summary>
    /// TODO: Add comment to WebSiteServicesEventReceiver
    /// </summary>
    [SharePointPermission(SecurityAction.InheritanceDemand, ObjectModel = true)]
    public class WebSiteServicesEventReceiver : SPFeatureReceiver
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
                IServiceLocator serviceLocator = SharePointServiceLocator.GetCurrent();
                IServiceLocatorConfig typeMappings = serviceLocator.GetInstance<IServiceLocatorConfig>();

                //SPSite site = properties.Feature.Parent as SPSite;
                SPWeb web = properties.Feature.Parent as SPWeb;
                typeMappings.Site = web.Site;
                typeMappings.RegisterTypeMapping<ILicenseRepository, LicenseRepository>();
                typeMappings.RegisterTypeMapping<IPageLayoutRepository, PageLayoutRepository>();
                typeMappings.RegisterTypeMapping<IMasterPageRepository, MasterPageRepository>();
                typeMappings.RegisterTypeMapping<IKeywordRepository, KeywordRepository>();
                web.Update();

            }
            catch (Exception ex)
            {
                SPDiagnosticsService.Local.WriteTrace(0, new SPDiagnosticsCategory(ex.Source, TraceSeverity.High, EventSeverity.Error), TraceSeverity.High, ex.Message, ex.Data);
                //ex.ToString();
            }

            try
            {
                SPWeb web = properties.Feature.Parent as SPWeb;

                if (web.WebTemplate.ToLower() == "websiteroot")
                {
                    //Hemrika.SharePresence.WebSite.Modules.WebPageModule.WebPageModule, Hemrika.SharePresence.WebSite, Version=1.0.0.0, Culture=neutral, PublicKeyToken=11e6604a27f32a11
                    CreateWorkItem(web, "Hemrika.SharePresence.WebSite.Modules.WebPageModule.WebPageModule", "Hemrika.SharePresence.WebSite, Version=1.0.0.0, Culture=neutral, PublicKeyToken=11e6604a27f32a11");
                    //Hemrika.SharePresence.WebSite.Modules.LicenseModule.LicenseModule, Hemrika.SharePresence.WebSite, Version=1.0.0.0, Culture=neutral, PublicKeyToken=11e6604a27f32a11
                    CreateWorkItem(web, "Hemrika.SharePresence.WebSite.Modules.LicenseModule.LicenseModule", "Hemrika.SharePresence.WebSite, Version=1.0.0.0, Culture=neutral, PublicKeyToken=11e6604a27f32a11");
                    //Hemrika.SharePresence.WebSite.Modules.SemanticModule.SemanticModule, Hemrika.SharePresence.WebSite, Version=1.0.0.0, Culture=neutral, PublicKeyToken=11e6604a27f32a11
                    CreateWorkItem(web, "Hemrika.SharePresence.WebSite.Modules.SemanticModule.SemanticModule", "Hemrika.SharePresence.WebSite, Version=1.0.0.0, Culture=neutral, PublicKeyToken=11e6604a27f32a11");
                }
            }
            catch (Exception ex)
            {
                SPDiagnosticsService.Local.WriteTrace(0, new SPDiagnosticsCategory(ex.Source, TraceSeverity.High, EventSeverity.Error), TraceSeverity.High, ex.Message, ex.Data);
                //ex.ToString();
            }

            /*
            IConfigManager configManager = serviceLocator.GetInstance<IConfigManager>();
            configManager.SetWeb(SPContext.Current.Site.RootWeb);
            IPropertyBag bag = configManager.GetPropertyBag(ConfigLevel.CurrentSPSite);
            */

            try
            {

            SPUserCodeService userCodeService = SPUserCodeService.Local;

            AddProxyOperation(userCodeService, ContainsKeyDataArgs.OperationAssemblyName, ContainsKeyDataArgs.OperationTypeName);
            AddProxyOperation(userCodeService, ReadConfigArgs.OperationAssemblyName, ReadConfigArgs.OperationTypeName);
            AddProxyOperation(userCodeService, ProxyInstalledArgs.OperationAssemblyName, ProxyInstalledArgs.OperationTypeName);
            userCodeService.Update();
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
            try
            {

                IServiceLocator serviceLocator = SharePointServiceLocator.GetCurrent();
                IServiceLocatorConfig typeMappings = serviceLocator.GetInstance<IServiceLocatorConfig>();
                typeMappings.Site = SPContext.Current.Site;
                typeMappings.RemoveTypeMappings<LicenseRepository>();
                typeMappings.RemoveTypeMappings<PageLayoutRepository>();
                typeMappings.RemoveTypeMappings<MasterPageRepository>();
            }
            catch (Exception ex)
            {
                ex.ToString();
            }

            try
            {
                SPUserCodeService userCodeService = SPUserCodeService.Local;

                RemoveProxyOperation(userCodeService, ContainsKeyDataArgs.OperationAssemblyName, ContainsKeyDataArgs.OperationTypeName);
                RemoveProxyOperation(userCodeService, ReadConfigArgs.OperationAssemblyName, ReadConfigArgs.OperationTypeName);
                RemoveProxyOperation(userCodeService, ProxyInstalledArgs.OperationAssemblyName, ProxyInstalledArgs.OperationTypeName);
                userCodeService.Update();
            }
            catch (Exception ex)
            {
                ex.ToString();
            }

        }

        #region Proxy
        
        private void AddProxyOperation(SPUserCodeService service, string assembly, string operation)
        {
            try
            {
            var proxyOp = new SPProxyOperationType(assembly, operation);
            service.ProxyOperationTypes.Add(proxyOp);

            }
            catch (Exception ex)
            {
                SPDiagnosticsService.Local.WriteTrace(0, new SPDiagnosticsCategory(ex.Source, TraceSeverity.High, EventSeverity.Error), TraceSeverity.High, ex.Message, ex.Data);
                //ex.ToString();
            }
        }

        private bool RemoveProxyOperation(SPUserCodeService service, string assembly, string operation)
        {
            var proxyOp = new SPProxyOperationType(assembly, operation);
            bool removed = service.ProxyOperationTypes.Remove(proxyOp);
            return removed;
        }

        #endregion

        #region WorkItem
        
        private void CreateWorkItem(SPWeb web, string fullClass, string fullAssembly)
        {
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

        #endregion

    }
}

