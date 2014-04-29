// <copyright file="WebSiteRobots.EventReceiver.cs" company="Hemrika">
// Copyright Hemrika. All rights reserved.
// </copyright>
// <author>MARKETING\Administrator</author>
// <date>2012-04-05 15:30:58Z</date>
namespace Hemrika.SharePresence.WebSite
{
    using System;
    using System.Collections.Generic;
    using System.Security.Permissions;
    using System.Text;
    using Microsoft.SharePoint;
    using Microsoft.SharePoint.Security;
    using Microsoft.SharePoint.Administration;
    using System.Runtime.InteropServices;
    using Hemrika.SharePresence.Common.Extensions;

    /// <summary>
    /// TODO: Add comment to WebSiteRobotsEventReceiver
    /// </summary>
    [SharePointPermission(SecurityAction.InheritanceDemand, ObjectModel = true)]
    public class WebSiteRobotsEventReceiver : SPFeatureReceiver
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
            ActivateHttpHandler_Robots(properties);
        }

        /// <summary>
        /// TODO: Add comment to describe the actions during feature deactivation
        /// </summary>
        /// <param name="properties">Properties of the feature</param>
        [SharePointPermission(SecurityAction.LinkDemand, ObjectModel = true)]
        public override void FeatureDeactivating(SPFeatureReceiverProperties properties)
        {
            FindWebApplication(properties);
            DeactivateHttpHandler_Robots(properties);
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

        #region Robots

        private void ActivateHttpHandler_Robots(SPFeatureReceiverProperties properties)
        {

            if (webApp != null)
            {
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
                    //webApp.WebService.ApplyWebConfigModifications();
                    webApp.Update();
                }
                catch (Exception ex)
                {
                    SPDiagnosticsService.Local.WriteTrace(0, new SPDiagnosticsCategory(ex.Source, TraceSeverity.High, EventSeverity.Error), TraceSeverity.High, ex.Message, ex.Data);
                    //ex.ToString();
                }

                try
                {
                    webApp.BlockedFileExtensions.Remove("txt");
                    webApp.Update();
                }
                catch (Exception ex)
                {
                    SPDiagnosticsService.Local.WriteTrace(0, new SPDiagnosticsCategory(ex.Source, TraceSeverity.High, EventSeverity.Error), TraceSeverity.High, ex.Message, ex.Data);
                    //ex.ToString();
                }
            }

            //webApp.UpdateMappedPage(SPWebApplication.SPCustomPage.AccessDenied, "/_layouts/CustomErrorPages/AccessDenied.aspx");
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
                    //webApp.WebService.ApplyWebConfigModifications();
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

    }
}

