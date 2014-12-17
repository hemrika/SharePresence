// <copyright file="WebSiteRootHandler.cs" company="Hemrika">
// Copyright Hemrika. All rights reserved.
// </copyright>
// <author>MARKETING\Administrator</author>
// <date>2012-01-18 16:26:14Z</date>
namespace Hemrika.SharePresence.Templates
{
    using System;
    using System.Collections.Generic;
    using System.Security.Permissions;
    using System.Text;
    using Microsoft.SharePoint;
    using Microsoft.SharePoint.Security;
    using Hemrika.SharePresence.Common.WebSiteController;
    using Microsoft.SharePoint.Administration;
    using Microsoft.SharePoint.WebPartPages;
    using System.IO;
    using Microsoft.SharePoint.Upgrade;
    using System.Security.Principal;
    using Microsoft.SharePoint.Administration.Claims;

    /// <summary>
    /// TODO: Add comment for WebSiteRootHandler
    /// </summary> 
    public class WebSiteRootHandler : SPWebEventReceiver
    {
        /// <summary>
        /// TODO: Add comment for event WebProvisioned in WebSiteRootHandler 
        /// </summary>
        /// <param name="properties">Contains list event properties</param>
        [SharePointPermission(SecurityAction.LinkDemand, ObjectModel = true)]
        public override void WebProvisioned(SPWebEventProperties properties)
        {
            //SPSite site = properties.Web.Site;
            //site.UserAccountDirectoryPath = "OU=US,OU=SharePoint,DC=Contoso,DC=com";

            try
            {
                /*
                while (!properties.Web.Provisioned)
                {
                    System.Threading.Thread.Sleep(3000);
                }
                */

                if (properties.Web.WebTemplate.ToLower() == "websiteroot")
                {
                    //Hemrika.SharePoint.WebSite.Modules.WebPageModule.WebPageModule, Hemrika.SharePoint.WebSite, Version=1.0.0.0, Culture=neutral, PublicKeyToken=3421bd1d946bda6c
                    CreateWorkItem(properties, "Hemrika.SharePoint.WebSite.Modules.WebPageModule.WebPageModule", "Hemrika.SharePoint.WebSite, Version=1.0.0.0, Culture=neutral, PublicKeyToken=3421bd1d946bda6c");
                    //Hemrika.SharePoint.WebSite.Modules.SemanticModule.SemanticModule, Hemrika.SharePoint.WebSite, Version=1.0.0.0, Culture=neutral, PublicKeyToken=3421bd1d946bda6c
                    CreateWorkItem(properties, "Hemrika.SharePoint.WebSite.Modules.SemanticModule.SemanticModule", "Hemrika.SharePoint.WebSite, Version=1.0.0.0, Culture=neutral, PublicKeyToken=3421bd1d946bda6c");
                    //Hemrika.SharePoint.WebSite.Modules.ErrorModule.ErrorModule, Hemrika.SharePoint.WebSite, Version=1.0.0.0, Culture=neutral, PublicKeyToken=3421bd1d946bda6c
                    CreateWorkItem(properties, "Hemrika.SharePoint.WebSite.Modules.ErrorModule.ErrorModule", "Hemrika.SharePoint.WebSite, Version=1.0.0.0, Culture=neutral, PublicKeyToken=3421bd1d946bda6c");
                }
            }
            catch (Exception ex)
            {
                SPDiagnosticsService.Local.WriteTrace(0, new SPDiagnosticsCategory(ex.Source, TraceSeverity.High, EventSeverity.Error), TraceSeverity.High, ex.Message, ex.Data);
                //ex.ToString();
            }

            try
            {
                if (properties.Web.WebTemplate.ToLower() == "websiteroot")
                {
                    properties.Web.AllowUnsafeUpdates = true;
                    SPFolder rootFolder = properties.Web.RootFolder;
                    //SPFile file = properties.Web.GetFile("Pages/Default.aspx");
                    rootFolder.WelcomePage = "Pages/default.aspx";
                    rootFolder.Update();
                    properties.Web.Update();
                    properties.Web.AllowUnsafeUpdates = false;
                }
            }
            catch (Exception ex)
            {
                SPDiagnosticsService.Local.WriteTrace(0, new SPDiagnosticsCategory(ex.Source, TraceSeverity.High, EventSeverity.Error), TraceSeverity.High, ex.Message, ex.Data);
                //ex.ToString();
            }

            try
            {
                if (properties.Web.WebTemplate.ToLower() == "websiteroot")
                {
                    if (properties.Web.IsRootWeb)
                    {
                        List<DropDownModesEx> dropDownModesThatCanUseResultPage = new List<DropDownModesEx> { DropDownModesEx.HideScopeDD, DropDownModesEx.HideScopeDD_DefaultContextual, DropDownModesEx.ShowDD, DropDownModesEx.ShowDD_DefaultContextual, DropDownModesEx.ShowDD_DefaultURL, DropDownModesEx.ShowDD_NoContextual, DropDownModesEx.ShowDD_NoContextual_DefaultURL };

                        properties.Web.AllowUnsafeUpdates = true;

                        //SRCH_ENH_FTR_URL
                        if (properties.Web.AllProperties.Contains("SRCH_ENH_FTR_URL"))
                        {
                            properties.Web.AllProperties["SRCH_ENH_FTR_URL"] = "/_layouts/searchresults.aspx";
                        }
                        else
                        {
                            properties.Web.AllProperties.Add("SRCH_ENH_FTR_URL", "/_layouts/searchresults.aspx");
                        }

                        //SRCH_TRAGET_RESULTS_PAGE
                        if (properties.Web.AllProperties.Contains("SRCH_TRAGET_RESULTS_PAGE"))
                        {
                            properties.Web.AllProperties["SRCH_TRAGET_RESULTS_PAGE"] = "/_layouts/searchresults.aspx";
                        }
                        else
                        {
                            properties.Web.AllProperties.Add("SRCH_TRAGET_RESULTS_PAGE", "/_layouts/searchresults.aspx");
                        }

                        /*
                        //SRCH_SITE_DROPDOWN_MODE
                        if (properties.Web.AllProperties.Contains("SRCH_SITE_DROPDOWN_MODE"))
                        {
                            properties.Web.AllProperties["SRCH_SITE_DROPDOWN_MODE"] = "";
                        }
                        else
                        {
                            properties.Web.AllProperties.Add("SRCH_SITE_DROPDOWN_MODE", "");
                        }
                        */
                        properties.Web.Update();
                        properties.Web.AllowUnsafeUpdates = false;
                    }
                }
            }
            catch (Exception ex)
            {
                SPDiagnosticsService.Local.WriteTrace(0, new SPDiagnosticsCategory(ex.Source, TraceSeverity.High, EventSeverity.Error), TraceSeverity.High, ex.Message, ex.Data);
                //ex.ToString();
            }

                try
                {
                    //SiteDesignerLibraries Feature
                    properties.Web.Features.Add(new Guid("63DE505E-E5AB-422d-909A-09609FD40CC8"), true);
                }
                catch (Exception ex)
                {
                    SPDiagnosticsService.Local.WriteTrace(0, new SPDiagnosticsCategory(ex.Source, TraceSeverity.High, EventSeverity.Error), TraceSeverity.High, ex.Message, ex.Data);
                    //ex.ToString();
                }

                try
                {
                    EnableFormsLockDown(properties.Web);
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

        private void CreateWorkItem(SPWebEventProperties properties, string fullClass, string fullAssembly)
        {
            Guid siteId = properties.SiteId;
            Guid webId = properties.WebId;
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

        }

        /// <summary>
        /// A site collection is being deleted
        /// </summary>
        public override void SiteDeleting(SPWebEventProperties properties)
        {
            if (properties.Web.WebTemplate.ToLower() == "websiteroot")
            {
                /*
                properties.Cancel = true;
                properties.ErrorMessage = "Deleting is not supported.";
                //base.SiteDeleting(properties);
                */
            }
        }

        /// <summary>
        /// A site is being deleted
        /// </summary>
        public override void WebDeleting(SPWebEventProperties properties)
        {
            if (properties.Web.WebTemplate.ToLower() == "websiteroot")
            {
                /*
                properties.Cancel = true;
                properties.ErrorMessage = "Deleting is not supported.";
                //base.WebDeleting(properties);
                */
            }
        }

    }
}
