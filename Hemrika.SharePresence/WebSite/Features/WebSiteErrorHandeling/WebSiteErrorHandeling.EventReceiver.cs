// <copyright file="WebSiteErrorHandeling.EventReceiver.cs" company="Hemrika">
// Copyright Hemrika. All rights reserved.
// </copyright>
// <author>MARKETING\Administrator</author>
// <date>2012-01-21 09:02:01Z</date>
namespace Hemrika.SharePresence.WebSite
{
    using System;
    using System.Collections.Generic;
    using System.Security.Permissions;
    using System.Text;
    using Microsoft.SharePoint;
    using Microsoft.SharePoint.Security;
    using Hemrika.SharePresence.Common.WebSiteController;
using System.Net;
    using Hemrika.SharePresence.WebSite.Modules.ErrorModule;
    using Microsoft.SharePoint.Administration;

    /// <summary>
    /// TODO: Add comment to WebSiteErrorHandelingEventReceiver
    /// </summary>
    [SharePointPermission(SecurityAction.InheritanceDemand, ObjectModel = true)]
    public class WebSiteErrorHandelingEventReceiver : SPFeatureReceiver
    {
        /// <summary>
        /// TODO: Add comment to describe the actions after feature activation
        /// </summary>
        /// <param name="properties">Properties of the feature</param>
        [SharePointPermission(SecurityAction.LinkDemand, ObjectModel = true)]
        public override void FeatureActivated(SPFeatureReceiverProperties properties)
        {
            SPWeb web = properties.Feature.Parent as SPWeb;

            if (web.WebTemplate.ToLower() == "websiteroot")
            {
                //Hemrika.SharePresence.WebSite.Modules.ErrorModule.ErrorModule, Hemrika.SharePresence.WebSite, Version=1.0.0.0, Culture=neutral, PublicKeyToken=3421bd1d946bda6c
                CreateWorkItem(web, "Hemrika.SharePresence.WebSite.Modules.ErrorModule.ErrorModule", "Hemrika.SharePresence.WebSite, Version=1.0.0.0, Culture=neutral, PublicKeyToken=3421bd1d946bda6c");

                CreateErrorWorkItem(web, HttpStatusCode.BadRequest);
                CreateErrorWorkItem(web, HttpStatusCode.Conflict);
                CreateErrorWorkItem(web, HttpStatusCode.ExpectationFailed);
                CreateErrorWorkItem(web, HttpStatusCode.Forbidden);
                CreateErrorWorkItem(web, HttpStatusCode.Gone);
                CreateErrorWorkItem(web, HttpStatusCode.LengthRequired);
                CreateErrorWorkItem(web, HttpStatusCode.MethodNotAllowed);
                CreateErrorWorkItem(web, HttpStatusCode.NotAcceptable);
                CreateErrorWorkItem(web, HttpStatusCode.NotFound);
                CreateErrorWorkItem(web, HttpStatusCode.PaymentRequired);
                CreateErrorWorkItem(web, HttpStatusCode.PreconditionFailed);
                CreateErrorWorkItem(web, HttpStatusCode.ProxyAuthenticationRequired);
                CreateErrorWorkItem(web, HttpStatusCode.RequestedRangeNotSatisfiable);
                CreateErrorWorkItem(web, HttpStatusCode.RequestEntityTooLarge);
                CreateErrorWorkItem(web, HttpStatusCode.RequestTimeout);
                CreateErrorWorkItem(web, HttpStatusCode.RequestUriTooLong);
                CreateErrorWorkItem(web, HttpStatusCode.Unauthorized);
                CreateErrorWorkItem(web, HttpStatusCode.UnsupportedMediaType);

                try
                {
                    SPSecurity.CatchAccessDeniedException = true;
                }
                catch (Exception ex)
                {
                    SPDiagnosticsService.Local.WriteTrace(0, new SPDiagnosticsCategory(ex.Source, TraceSeverity.High, EventSeverity.Error), TraceSeverity.High, ex.Message, ex.Data);
                }

                /*
                SPSecurity.RunWithElevatedPrivileges(delegate()
                            {
                                try
                                {
                                    SPSecurity.CatchAccessDeniedException = true;

                                    SPWebApplication webApp = web.Site.WebApplication;
                                    if (webApp != null)
                                    {
                                        webApp.UpdateMappedPage(SPWebApplication.SPCustomPage.AccessDenied, "/_layouts/Error/" + HttpStatusCode.Unauthorized.ToString() + ".aspx;");
                                        //web.Site.WebApplication.UpdateMappedPage(SPWebApplication.SPCustomPage.Confirmation, "");
                                        webApp.UpdateMappedPage(SPWebApplication.SPCustomPage.Error, "/_layouts/Error/" + HttpStatusCode.BadRequest.ToString() + ".aspx;");
                                        webApp.UpdateMappedPage(SPWebApplication.SPCustomPage.Login, "/_layouts/Error/" + HttpStatusCode.Forbidden.ToString() + ".aspx;");
                                        //web.Site.WebApplication.UpdateMappedPage(SPWebApplication.SPCustomPage.RequestAccess, String.Format("{0}:{1};", "ErrorPage", "Error/" + HttpStatusCode.Forbidden.ToString() + ".aspx;"));
                                        //web.Site.WebApplication.UpdateMappedPage(SPWebApplication.SPCustomPage.Signout, "");
                                        webApp.UpdateMappedPage(SPWebApplication.SPCustomPage.WebDeleted, "/_layouts/Error/" + HttpStatusCode.Gone.ToString() + ".aspx;");

                                        webApp.Update(false);
                                    }
                                }
                                catch (Exception ex)
                                {
                                    ex.ToString();
                                }
                            });

                SPList specifiedList = web.GetList("Error");
                specifiedList.AnonymousPermMask64 = SPBasePermissions.ViewListItems;
                specifiedList.Update();
                */
            }
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

                WebSiteControllerModuleWorkItem WebSiteControllerModuleJob = new WebSiteControllerModuleWorkItem(WebSiteControllerModuleWorkItem.WorkItemJobDisplayName + "HomePage", web.Site.WebApplication);
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

        private void CreateErrorWorkItem(SPWeb web, HttpStatusCode code)
        {
            try
            {

                Guid siteId = web.Site.ID;
                Guid webId = web.ID;

                bool disabled = false;
                WebSiteControllerPrincipalType principalType = WebSiteControllerPrincipalType.None;
                bool appliesToSSL = true;
                int sequence = 1;
                String pricipal = string.Empty;

                StringBuilder builder = new StringBuilder();
                builder.Append("Error/" + code.ToString() + ".aspx;");
                builder.Append(disabled.ToString() + ";");
                builder.Append(appliesToSSL.ToString() + ";");
                builder.Append(sequence.ToString() + ";");
                builder.Append(principalType.ToString() + ";");
                builder.Append(pricipal + ";");
                builder.Append("#");

                builder.Append(String.Format("{0}:{1};", "ErrorPage", "Error/" + code.ToString() + ".aspx;"));
                builder.Append(String.Format("{0}:{1};", "ErrorCode", ((int)code).ToString()));


                string full = builder.ToString();

                ErrorModule mod = new ErrorModule();
                IWebSiteControllerModule imod = null;  //WebSiteControllerConfig.GetModule(web.Site.WebApplication, mod.RuleType);

                while (imod == null)
                {
                    System.Threading.Thread.Sleep(1000);
                    try
                    {
                        imod = WebSiteControllerConfig.GetModule(web.Site.WebApplication, mod.RuleType);
                    }
                    catch (Exception ex)
                    {
                        SPDiagnosticsService.Local.WriteTrace(0, new SPDiagnosticsCategory(ex.Source, TraceSeverity.High, EventSeverity.Error), TraceSeverity.High, ex.Message, ex.Data);
                        //ex.ToString();
                    }
                }

                //Guid itemGuid = new Guid("17A3219B-049F-4056-9566-37590122BE8E");
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

                try
                {

                    WebSiteControllerRuleWorkItem WebSiteControllerModuleJob = new WebSiteControllerRuleWorkItem(WebSiteControllerRuleWorkItem.WorkItemJobDisplayName + code.ToString(), web.Site.WebApplication);
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
            catch (Exception ex)
            {
                SPDiagnosticsService.Local.WriteTrace(0, new SPDiagnosticsCategory(ex.Source, TraceSeverity.High, EventSeverity.Error), TraceSeverity.High, ex.Message, ex.Data);
            }
        }

        /// <summary>
        /// TODO: Add comment to describe the actions during feature deactivation
        /// </summary>
        /// <param name="properties">Properties of the feature</param>
        [SharePointPermission(SecurityAction.LinkDemand, ObjectModel = true)]
        public override void FeatureDeactivating(SPFeatureReceiverProperties properties)
        {
            SPWeb web = properties.Feature.Parent as SPWeb;

            if (web.WebTemplate.ToLower() == "websiteroot")
            {

                SPSecurity.RunWithElevatedPrivileges(delegate()
                {
                    try
                    {
                        SPWebApplication webApp = web.Site.WebApplication;
                        if (webApp != null)
                        {
                            /*
                            webApp.UpdateMappedPage(SPWebApplication.SPCustomPage.None, null);
                            webApp.Update(true);
                            */
                        }
                    }
                    catch (Exception ex)
                    {
                        SPDiagnosticsService.Local.WriteTrace(0, new SPDiagnosticsCategory(ex.Source, TraceSeverity.High, EventSeverity.Error), TraceSeverity.High, ex.Message, ex.Data);
                        //ex.ToString();
                    }
                });
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
    }
}

