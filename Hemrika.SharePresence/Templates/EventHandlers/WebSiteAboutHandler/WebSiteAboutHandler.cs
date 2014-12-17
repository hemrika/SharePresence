// <copyright file="WebSiteAboutHandler.cs" company="Hemrika">
// Copyright Hemrika. All rights reserved.
// </copyright>
// <author>MARKETING\Administrator</author>
// <date>2012-01-17 12:50:01Z</date>
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
    using Microsoft.SharePoint.Utilities;

    /// <summary>
    /// TODO: Add comment for WebSiteAboutHandler
    /// </summary> 
    public class WebSiteAboutHandler : SPWebEventReceiver
    {
        /// <summary>
        /// TODO: Add comment for event WebProvisioned in WebSiteAboutHandler 
        /// </summary>
        /// <param name="properties">Contains list event properties</param>
        [SharePointPermission(SecurityAction.LinkDemand, ObjectModel = true)]
        public override void WebProvisioned(SPWebEventProperties properties)
        {
            try
            {
                if (properties.Web.WebTemplate.ToLower() == "websiteabout")
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

            SPUtility.ValidateFormDigest();

            try
            {
                SPList page = properties.Web.Lists["Pages"];
                //page.DraftVersionVisibility = DraftVisibilityType.Author | DraftVisibilityType.Approver;
                page.EnableVersioning = true;
                page.EnableMinorVersions = true;
                page.EnableThrottling = true;
                page.Update();
            }
            catch (Exception ex)
            {
                SPDiagnosticsService.Local.WriteTrace(0, new SPDiagnosticsCategory(ex.Source, TraceSeverity.High, EventSeverity.Error), TraceSeverity.High, ex.Message, ex.Data);
                //ex.ToString();
            }

            if (properties.Web.WebTemplate.ToLower() == "websiteabout")
            {
                CreateWorkItem(properties, "/Pages/default.aspx", "");
                CreateWorkItem(properties, "/Pages/People.aspx", "/People");
                CreateWorkItem(properties, "/Pages/About.aspx", "/About");
                CreateWorkItem(properties, "/Pages/Legal.aspx", "/Legal");
                CreateWorkItem(properties, "/Pages/History.aspx", "/History");
            }
        }

        private void CreateWorkItem(SPWebEventProperties properties, string pagename,string url)
        {
            Guid siteId = properties.SiteId;
            Guid webId = properties.WebId;

            bool disabled = false;
            WebSiteControllerPrincipalType principalType = WebSiteControllerPrincipalType.None;
            bool appliesToSSL = true;
            int sequence = 1;
            String pricipal = string.Empty;

            StringBuilder builder = new StringBuilder();
            builder.Append(properties.Web.ServerRelativeUrl + url + ";");
            builder.Append(disabled.ToString() + ";");
            builder.Append(appliesToSSL.ToString() + ";");
            builder.Append(sequence.ToString() + ";");
            builder.Append(principalType.ToString() + ";");
            builder.Append(pricipal + ";");
            builder.Append("#");

            builder.Append(String.Format("{0}:{1};", "OriginalUrl", properties.Web.ServerRelativeUrl + pagename));
         
            string full = builder.ToString();

            IWebSiteControllerModule imod = null;
            SPSite parentsite = new SPSite(properties.SiteId);

            while (imod == null)
            {
                System.Threading.Thread.Sleep(1000);
                try
                {
                    imod = WebSiteControllerConfig.GetModule(parentsite.WebApplication, "Hemrika.SharePoint.WebSite.Modules.SemanticModule.SemanticModule");
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
        }
    }
}

