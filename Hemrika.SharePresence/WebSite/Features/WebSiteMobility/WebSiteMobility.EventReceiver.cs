// <copyright file="WebSiteMobility.EventReceiver.cs" company="SharePresence">
// Copyright SharePresence. All rights reserved.
// </copyright>
// <author>MARKETING\Administrator</author>
// <date>2013-03-11 16:12:01Z</date>
namespace Hemrika.SharePresence.WebSite
{
    using System;
    using System.Collections.Generic;
    using System.Security.Permissions;
    using System.Text;
    using Microsoft.SharePoint;
    using Microsoft.SharePoint.Security;
    using Microsoft.SharePoint.Administration;
    using Hemrika.SharePresence.Common.Extensions;

    /// <summary>
    /// TODO: Add comment to WebSiteMobilityEventReceiver
    /// </summary>
    [SharePointPermission(SecurityAction.InheritanceDemand, ObjectModel = true)]
    public class WebSiteMobilityEventReceiver : SPFeatureReceiver
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

            foreach (SPSite site in webApp.Sites)
            {
                try
                {
                    site.Features.Add(new Guid("63DE505E-E5AB-422d-909A-09609FD40CC8"), true);
                }
                catch (Exception ex)
                {
                    SPDiagnosticsService.Local.WriteTrace(0, new SPDiagnosticsCategory(ex.Source, TraceSeverity.High, EventSeverity.Error), TraceSeverity.High, ex.Message, ex.Data);
                    //ex.ToString();
                }

                try
                {
                    site.Features.Remove(new Guid("F41CC668-37E5-4743-B4A8-74D1DB3FD8A4"), true);
                }
                catch (Exception ex)
                {
                    SPDiagnosticsService.Local.WriteTrace(0, new SPDiagnosticsCategory(ex.Source, TraceSeverity.High, EventSeverity.Error), TraceSeverity.High, ex.Message, ex.Data);
                    //ex.ToString();
                }

                foreach (SPWeb web in site.AllWebs)
                {
                    try
                    {
                        web.Features.Add(new Guid("63DE505E-E5AB-422d-909A-09609FD40CC8"), true);
                        web.Update();
                    }
                    catch (Exception ex)
                    {
                        SPDiagnosticsService.Local.WriteTrace(0, new SPDiagnosticsCategory(ex.Source, TraceSeverity.High, EventSeverity.Error), TraceSeverity.High, ex.Message, ex.Data);
                        //ex.ToString();
                    }

                    try
                    {
                        web.Features.Remove(new Guid("F41CC668-37E5-4743-B4A8-74D1DB3FD8A4"), true);
                        web.Update();
                    }
                    catch (Exception ex)
                    {
                        SPDiagnosticsService.Local.WriteTrace(0, new SPDiagnosticsCategory(ex.Source, TraceSeverity.High, EventSeverity.Error), TraceSeverity.High, ex.Message, ex.Data);
                        //ex.ToString();
                    }
                }

                try
                {
                    //OverrideBrowserCaps(properties);
                }
                catch (Exception ex)
                {
                    SPDiagnosticsService.Local.WriteTrace(0, new SPDiagnosticsCategory(ex.Source, TraceSeverity.High, EventSeverity.Error), TraceSeverity.High, ex.Message, ex.Data);
                    //ex.ToString();
                }
            }
        }

        private void OverrideBrowserCaps(SPFeatureReceiverProperties properties)
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
                SPWebConfigModification mod = new SPWebConfigModification("add[@type='System.Web.Mobile.MobileCapabilities, System.Web.Mobile, Version=2.0.0.0, Culture=neutral, PublicKeyToken=b03f5f7f11d50a3a']", "configuration/system.web/browserCaps");
                mod.Owner = properties.Feature.DefinitionId.ToString();
                mod.Sequence = 0;
                mod.Type = SPWebConfigModification.SPWebConfigModificationType.EnsureChildNode;
                mod.Value = @"<result type=""System.Web.Mobile.MobileCapabilities, System.Web.Mobile, Version=2.0.0.0, Culture=neutral, PublicKeyToken=b03f5f7f11d50a3a"" /><filter>isMobileDevice=false</filter>";
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
        /// TODO: Add comment to describe the actions during feature deactivation
        /// </summary>
        /// <param name="properties">Properties of the feature</param>
        [SharePointPermission(SecurityAction.LinkDemand, ObjectModel = true)]
        public override void FeatureDeactivating(SPFeatureReceiverProperties properties)
        {
            FindWebApplication(properties);
            foreach (SPSite site in webApp.Sites)
            {

                try
                {
                    site.Features.Add(new Guid("63DE505E-E5AB-422d-909A-09609FD40CC8"), true);
                }
                catch (Exception ex)
                {
                    SPDiagnosticsService.Local.WriteTrace(0, new SPDiagnosticsCategory(ex.Source, TraceSeverity.High, EventSeverity.Error), TraceSeverity.High, ex.Message, ex.Data);
                    //ex.ToString();
                }

                try
                {
                    site.Features.Remove(new Guid("F41CC668-37E5-4743-B4A8-74D1DB3FD8A4"), true);
                }
                catch (Exception ex)
                {
                    SPDiagnosticsService.Local.WriteTrace(0, new SPDiagnosticsCategory(ex.Source, TraceSeverity.High, EventSeverity.Error), TraceSeverity.High, ex.Message, ex.Data);
                    //ex.ToString();
                }

                foreach (SPWeb web in site.AllWebs)
                {
                    try
                    {
                        web.Features.Remove(new Guid("63DE505E-E5AB-422d-909A-09609FD40CC8"), true);
                        web.Update();
                    }
                    catch (Exception ex)
                    {
                        SPDiagnosticsService.Local.WriteTrace(0, new SPDiagnosticsCategory(ex.Source, TraceSeverity.High, EventSeverity.Error), TraceSeverity.High, ex.Message, ex.Data);
                        //ex.ToString();
                    }

                    try
                    {
                        web.Features.Add(new Guid("F41CC668-37E5-4743-B4A8-74D1DB3FD8A4"), true);
                        web.Update();
                    }
                    catch (Exception ex)
                    {
                        SPDiagnosticsService.Local.WriteTrace(0, new SPDiagnosticsCategory(ex.Source, TraceSeverity.High, EventSeverity.Error), TraceSeverity.High, ex.Message, ex.Data);
                        //ex.ToString();
                    }

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

    }
}

