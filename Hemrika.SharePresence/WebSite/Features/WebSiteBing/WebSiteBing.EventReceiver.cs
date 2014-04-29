// <copyright file="WebSiteBing.EventReceiver.cs" company="Hemrika">
// Copyright Hemrika. All rights reserved.
// </copyright>
// <author>MARKETING\Administrator</author>
// <date>2012-04-05 16:30:09Z</date>
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
    using Hemrika.SharePresence.WebSite.Bing;
    using Hemrika.SharePresence.Common.Extensions;

    /// <summary>
    /// TODO: Add comment to WebSiteBingEventReceiver
    /// </summary>
    [SharePointPermission(SecurityAction.InheritanceDemand, ObjectModel = true)]
    public class WebSiteBingEventReceiver : SPFeatureReceiver
    {
        private SPWebApplication webApp = null;

        public override void FeatureActivated(SPFeatureReceiverProperties properties)
        {
            FindWebApplication(properties);

            if (webApp != null)
            {
                SPSecurity.RunWithElevatedPrivileges(delegate()
                {
                    SPWebConfigModificationExtensions.WaitForOnetimeJobToFinish(webApp.Farm, "job-webconfig-modification", 180);
                    ActivateHttpHandler_Bing(properties);
                });
            }
        }

        public override void FeatureDeactivating(SPFeatureReceiverProperties properties)
        {
            FindWebApplication(properties);

            if (webApp != null)
            {
                SPSecurity.RunWithElevatedPrivileges(delegate()
                {
                    SPWebConfigModificationExtensions.WaitForOnetimeJobToFinish(webApp.Farm, "job-webconfig-modification", 180);
                    DeactivateHttpHandler_Bing(properties);
                });
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

        #region Bing

        private void ActivateHttpHandler_Bing(SPFeatureReceiverProperties properties)
        {
            if (webApp != null)
            {
                SPWebConfigModificationExtensions.WaitForOnetimeJobToFinish(webApp.Farm, "job-webconfig-modification", 180);

                SPWebConfigModification mod = new SPWebConfigModification("add[@type='Hemrika.SharePresence.WebSite.Bing.BingHandler, Hemrika.SharePresence.WebSite, Version=1.0.0.0, Culture=neutral, PublicKeyToken=3421bd1d946bda6c']", "configuration/system.webServer/handlers");
                mod.Owner = properties.Feature.DefinitionId.ToString();
                mod.Sequence = 0;
                mod.Type = SPWebConfigModification.SPWebConfigModificationType.EnsureChildNode;
                mod.Value = @"<add name=""BingHandler"" path=""BingSiteAuth.xml"" verb=""*"" type=""Hemrika.SharePresence.WebSite.Bing.BingHandler, Hemrika.SharePresence.WebSite, Version=1.0.0.0, Culture=neutral, PublicKeyToken=3421bd1d946bda6c"" />";
                webApp.WebConfigModifications.Add(mod);

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
                    settings.Remove(site);
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
            }
        }
        #endregion
    }
}

