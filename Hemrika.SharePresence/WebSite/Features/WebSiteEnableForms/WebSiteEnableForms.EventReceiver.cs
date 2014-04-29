// <copyright file="WebSiteEnableForms.EventReceiver.cs" company="Hemrika">
// Copyright Hemrika. All rights reserved.
// </copyright>
// <author>MARKETING\Administrator</author>
// <date>2012-02-13 10:19:15Z</date>
namespace Hemrika.SharePresence.WebSite
{
    using System;
    using System.Collections.Generic;
    using System.Security.Permissions;
    using System.Text;
    using Microsoft.SharePoint;
    using Microsoft.SharePoint.Security;
    using Microsoft.SharePoint.Administration;
    using System.Net;

    /// <summary>
    /// TODO: Add comment to WebSiteEnableFormsEventReceiver
    /// </summary>
    [SharePointPermission(SecurityAction.InheritanceDemand, ObjectModel = true)]
    public class WebSiteEnableFormsEventReceiver : SPFeatureReceiver
    {
        /// <summary>
        /// TODO: Add comment to describe the actions after feature activation
        /// </summary>
        /// <param name="properties">Properties of the feature</param>
        [SharePointPermission(SecurityAction.LinkDemand, ObjectModel = true)]
        public override void FeatureActivated(SPFeatureReceiverProperties properties)
        {
            if (properties.Feature.Parent is SPWeb)
            {
                try
                {
                    SPWeb subWeb = (SPWeb)properties.Feature.Parent;
                    if (!subWeb.IsRootWeb && !subWeb.HasUniqueRoleDefinitions)
                    {
                        subWeb.RoleDefinitions.BreakInheritance(true, false);
                    }

                    //subWeb.BreakRoleInheritance(true);
                    SPRoleDefinition guestRole = subWeb.RoleDefinitions.GetByType(SPRoleType.Guest);
                    guestRole.BasePermissions |= SPBasePermissions.EmptyMask | Microsoft.SharePoint.SPBasePermissions.ViewFormPages;// | Microsoft.SharePoint.SPBasePermissions.Open | Microsoft.SharePoint.SPBasePermissions.BrowseUserInfo | Microsoft.SharePoint.SPBasePermissions.UseClientIntegration | Microsoft.SharePoint.SPBasePermissions.UseRemoteAPIs;
                    guestRole.BasePermissions |= SPBasePermissions.UseRemoteAPIs;

                    //guestRole.BasePermissions = Microsoft.SharePoint.SPBasePermissions.ViewFormPages | Microsoft.SharePoint.SPBasePermissions.Open | Microsoft.SharePoint.SPBasePermissions.BrowseUserInfo | Microsoft.SharePoint.SPBasePermissions.UseClientIntegration | Microsoft.SharePoint.SPBasePermissions.UseRemoteAPIs ;
                    guestRole.Update();
                    //subWeb.AnonymousPermMask64 = Microsoft.SharePoint.SPBasePermissions.ViewListItems | Microsoft.SharePoint.SPBasePermissions.ViewFormPages | Microsoft.SharePoint.SPBasePermissions.Open | Microsoft.SharePoint.SPBasePermissions.ViewPages | Microsoft.SharePoint.SPBasePermissions.UseClientIntegration;
                    subWeb.Update();

                }
                catch (Exception ex)
                {
                    SPDiagnosticsService.Local.WriteTrace(0, new SPDiagnosticsCategory(ex.Source, TraceSeverity.High, EventSeverity.Error), TraceSeverity.High, ex.Message, ex.Data);
                    //throw;
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
            if (properties.Feature.Parent is SPWeb)
            {
                try
                {
                    SPWeb web = (SPWeb)properties.Feature.Parent;
                    SPRoleDefinition guestRole = web.RoleDefinitions.GetByType(SPRoleType.Guest);
                    guestRole.BasePermissions &= ~(SPBasePermissions.EmptyMask | SPBasePermissions.ViewFormPages);
                    guestRole.BasePermissions &= ~SPBasePermissions.UseRemoteAPIs;
                    guestRole.BasePermissions |= SPBasePermissions.ViewVersions;
                    guestRole.Update();
                    web.AnonymousPermMask64 &= ~(SPBasePermissions.UseRemoteAPIs | SPBasePermissions.ViewFormPages);// | SPBasePermissions.ViewVersions);
                    web.AnonymousPermMask64 |= SPBasePermissions.ViewVersions;
                    web.Update();
                }
                catch (Exception ex)
                {
                    SPDiagnosticsService.Local.WriteTrace(0, new SPDiagnosticsCategory(ex.Source, TraceSeverity.High, EventSeverity.Error), TraceSeverity.High, ex.Message, ex.Data);
                    //throw;
                }
            }

            //SPSecurity.RunWithElevatedPrivileges(() =>
            //{
                try
                {

                    SPSecurity.CatchAccessDeniedException = true;

                    /*
                    Guid id = ((SPWeb)properties.Feature.Parent).Site.ID;
                    SPSite site = new SPSite(id);
                    SPWebApplication webApp = site.WebApplication;

                    if (webApp != null)
                    { 
                        webApp.UpdateMappedPage(SPWebApplication.SPCustomPage.AccessDenied, "Error/" + HttpStatusCode.Unauthorized.ToString() + ".aspx;");
                        //web.Site.WebApplication.UpdateMappedPage(SPWebApplication.SPCustomPage.Confirmation, "");
                        webApp.UpdateMappedPage(SPWebApplication.SPCustomPage.Error, "Error/" + HttpStatusCode.BadRequest.ToString() + ".aspx;");
                        webApp.UpdateMappedPage(SPWebApplication.SPCustomPage.Login, "Error/" + HttpStatusCode.Forbidden.ToString() + ".aspx;");
                        //web.Site.WebApplication.UpdateMappedPage(SPWebApplication.SPCustomPage.RequestAccess, String.Format("{0}:{1};", "ErrorPage", "Error/" + HttpStatusCode.Forbidden.ToString() + ".aspx;"));
                        //web.Site.WebApplication.UpdateMappedPage(SPWebApplication.SPCustomPage.Signout, "");
                        webApp.UpdateMappedPage(SPWebApplication.SPCustomPage.WebDeleted, "Error/" + HttpStatusCode.Gone.ToString() + ".aspx;");

                        webApp.Update(false);
                    }
                    */
                }
                catch (Exception ex)
                {
                    SPDiagnosticsService.Local.WriteTrace(0, new SPDiagnosticsCategory(ex.Source, TraceSeverity.High, EventSeverity.Error), TraceSeverity.High, ex.Message, ex.Data);
                }
            //});
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

