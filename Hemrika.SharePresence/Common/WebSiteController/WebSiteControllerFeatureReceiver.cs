// Copyright Hemrika. All rights reserved.
// </copyright>
// <author>SEO\Administrator</author>
// <date>2011-09-09 20:48:32Z</date>
namespace Hemrika.SharePresence.Common.WebSiteController
{
    using System.Security.Permissions;
    using Microsoft.SharePoint;
    using Microsoft.SharePoint.Administration;
    using Microsoft.SharePoint.Security;
    using Hemrika.SharePresence.Common.Extensions;
    using System;

    /// <summary>
    /// Feature Receiver class for WebSiteController Feature
    /// </summary>
    public sealed class WebSiteControllerFeatureReceiver : SPFeatureReceiver
    {
        /// <summary>
        /// Occurs when a Feature is deactivated.
        /// </summary>
        /// <param name="properties">An <see cref="T:Microsoft.SharePoint.SPFeatureReceiverProperties"></see> object that represents the properties of the event.</param>
        [SharePointPermission(SecurityAction.LinkDemand, ObjectModel = true)]
        public override void FeatureDeactivating(SPFeatureReceiverProperties properties)
        {
            SPWebApplication webApp = (SPWebApplication)properties.Feature.Parent;
            SPWebConfigModificationExtensions.WaitForOnetimeJobToFinish(webApp.Farm, "job-webconfig-modification", 180);
            webApp.WebConfigModifications.Remove(WebSiteControllerWebConfigManager.GetHttpModuleEntry());
            webApp.WebConfigModifications.Remove(WebSiteControllerWebConfigManager.GetModuleEntry());
            webApp.Farm.Services.GetValue<SPWebService>().ApplyWebConfigModifications();
            webApp.Update();
        }

        /// <summary>
        /// Occurs after a Feature is activated.
        /// </summary>
        /// <param name="properties">An <see cref="T:Microsoft.SharePoint.SPFeatureReceiverProperties"></see> object that represents the properties of the event.</param>
        [SharePointPermission(SecurityAction.LinkDemand, ObjectModel = true)]
        public override void FeatureActivated(SPFeatureReceiverProperties properties)
        {
            SPWebApplication webApp = (SPWebApplication)properties.Feature.Parent;

            SPWebConfigModificationExtensions.WaitForOnetimeJobToFinish(webApp.Farm, "job-webconfig-modification", 180);
            
            WebSiteControllerWebConfigManager.ClearWebConfigModifications(webApp);
            SPWebConfigModification mod = WebSiteControllerWebConfigManager.GetHttpModuleEntry();

            if (!webApp.WebConfigModifications.Contains(mod))
            {
                webApp.WebConfigModifications.Add(mod);
            }

            mod = WebSiteControllerWebConfigManager.GetModuleEntry();
            if (!webApp.WebConfigModifications.Contains(mod))
            {
                webApp.WebConfigModifications.Add(mod);
            }

            try
            {
            webApp.Farm.Services.GetValue<SPWebService>().ApplyWebConfigModifications();
            webApp.Update();
            }
            catch (Exception ex)
            {
                SPDiagnosticsService.Local.WriteTrace(0, new SPDiagnosticsCategory(ex.Source, TraceSeverity.High, EventSeverity.Error), TraceSeverity.High, ex.Message, ex.Data);
                //ex.ToString();
            }
        }

        /// <summary>
        /// Occurs after a Feature is installed.
        /// </summary>
        /// <param name="properties">An <see cref="T:Microsoft.SharePoint.SPFeatureReceiverProperties"></see> object that represents the properties of the event.</param>
        [SharePointPermission(SecurityAction.LinkDemand, ObjectModel = true)]
        public override void FeatureInstalled(SPFeatureReceiverProperties properties)
        {
        }

        /// <summary>
        /// Occurs when a Feature is uninstalled.
        /// </summary>
        /// <param name="properties">An <see cref="T:Microsoft.SharePoint.SPFeatureReceiverProperties"></see> object that represents the properties of the event.</param>
        [SharePointPermission(SecurityAction.LinkDemand, ObjectModel = true)]
        public override void FeatureUninstalling(SPFeatureReceiverProperties properties)
        {
            // WebSiteControllerConfig.DeleteFromConfigDB();
        }
    }
}
