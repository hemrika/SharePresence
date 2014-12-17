// <copyright file="WebSiteSearchHandler.cs" company="SharePresence">
// Copyright SharePresence. All rights reserved.
// </copyright>
// <author>MARKETING\Administrator</author>
// <date>2013-04-04 10:26:35Z</date>
namespace Hemrika.SharePresence.Templates
{
    using System;
    using System.Collections.Generic;
    using System.Security.Permissions;
    using System.Text;
    using Microsoft.SharePoint;
    using Microsoft.SharePoint.Security;
    using Microsoft.SharePoint.Administration;
    using Microsoft.SharePoint.WebControls;
    using Microsoft.SharePoint.Utilities;

    /// <summary>
    /// TODO: Add comment for WebSiteSearchHandler
    /// </summary> 
    public class WebSiteSearchHandler : SPWebEventReceiver
    {
        /// <summary>
        /// TODO: Add comment for event SiteDeleted in WebSiteSearchHandler 
        /// </summary>
        /// <param name="properties">Contains list event properties</param>
        [SharePointPermission(SecurityAction.LinkDemand, ObjectModel = true)]
        public override void SiteDeleted(SPWebEventProperties properties)
        {
            ////Add code for event SiteDeleted in WebSiteSearchHandler
        }

        /// <summary>
        /// TODO: Add comment for event WebDeleted in WebSiteSearchHandler 
        /// </summary>
        /// <param name="properties">Contains list event properties</param>
        [SharePointPermission(SecurityAction.LinkDemand, ObjectModel = true)]
        public override void WebDeleted(SPWebEventProperties properties)
        {
            try
            {
                if (properties.Web.WebTemplate.ToLower() == "websitesearch")
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

        }

        /// <summary>
        /// TODO: Add comment for event WebDeleting in WebSiteSearchHandler 
        /// </summary>
        /// <param name="properties">Contains list event properties</param>
        [SharePointPermission(SecurityAction.LinkDemand, ObjectModel = true)]
        public override void WebDeleting(SPWebEventProperties properties)
        {
            ////Add code for event WebDeleting in WebSiteSearchHandler

            /*
            properties.Cancel = true;
            properties.ErrorMessage = "Deleting is not supported.";
            */
        }


        /// <summary>
        /// TODO: Add comment for event WebAdding in WebSiteSearchHandler 
        /// </summary>
        /// <param name="properties">Contains list event properties</param>
        [SharePointPermission(SecurityAction.LinkDemand, ObjectModel = true)]
        public override void WebAdding(SPWebEventProperties properties)
        {
            ////Add code for event WebAdding in WebSiteSearchHandler
        }

        /// <summary>
        /// TODO: Add comment for event WebProvisioned in WebSiteSearchHandler 
        /// </summary>
        /// <param name="properties">Contains list event properties</param>
        [SharePointPermission(SecurityAction.LinkDemand, ObjectModel = true)]
        public override void WebProvisioned(SPWebEventProperties properties)
        {
            try
            {
                if (properties.Web.WebTemplate.ToLower() == "websitesearch")
                {
                    if (!properties.Web.IsRootWeb)
                    {
                        List<DropDownModesEx> dropDownModesThatCanUseResultPage = new List<DropDownModesEx> { DropDownModesEx.HideScopeDD, DropDownModesEx.HideScopeDD_DefaultContextual, DropDownModesEx.ShowDD, DropDownModesEx.ShowDD_DefaultContextual, DropDownModesEx.ShowDD_DefaultURL, DropDownModesEx.ShowDD_NoContextual, DropDownModesEx.ShowDD_NoContextual_DefaultURL };

                        properties.Web.AllowUnsafeUpdates = true;

                        //SRCH_ENH_FTR_URL
                        if (properties.Web.AllProperties.Contains("SRCH_ENH_FTR_URL"))
                        {
                            properties.Web.AllProperties["SRCH_ENH_FTR_URL"] = "/Page/Search.aspx";
                        }
                        else
                        {
                            properties.Web.AllProperties.Add("SRCH_ENH_FTR_URL", "/Page/Search.aspx");
                        }

                        //SRCH_TRAGET_RESULTS_PAGE
                        if (properties.Web.AllProperties.Contains("SRCH_TRAGET_RESULTS_PAGE"))
                        {
                            properties.Web.AllProperties["SRCH_TRAGET_RESULTS_PAGE"] = "/Page/Search.aspx";
                        }
                        else
                        {
                            properties.Web.AllProperties.Add("SRCH_TRAGET_RESULTS_PAGE", "/Page/Search.aspx");
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
                if (properties.Web.WebTemplate.ToLower() == "websitesearch")
                {

                    //web.RootFolder.WelcomePage = "Pages/Default.aspx";
                    //web.Update();
                    //web.AllowUnsafeUpdates = true;
                    SPFolder rootFolder = properties.Web.RootFolder;
                    rootFolder.WelcomePage = "Pages/Search.aspx";
                    rootFolder.Update();
                    properties.Web.Update();
                    //web.AllowUnsafeUpdates = false;
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
        }
    }
}
