// <copyright file="WebSitePageEventHandler.cs" company="Hemrika">
// Copyright Hemrika. All rights reserved.
// </copyright>
// <author>MARKETING\Administrator</author>
// <date>2012-03-01 08:12:09Z</date>
namespace Hemrika.SharePresence.WebSite
{
    using System;
    using System.Collections.Generic;
    using System.Security.Permissions;
    using System.Text;
    using Microsoft.SharePoint;
    using Microsoft.SharePoint.Security;
    using Hemrika.SharePresence.Common.WebSiteController;
    using Hemrika.SharePresence.WebSite.Modules.SemanticModule;

    /// <summary>
    /// TODO: Add comment for WebSitePageEventHandler
    /// </summary>
    public class WebSitePageEventHandler : SPItemEventReceiver
    {
        /// <summary>
        /// TODO: Add comment for event ItemAdded in WebSitePageEventHandler 
        /// </summary>
        /// <param name="properties">Contains list event properties</param>
        [SharePointPermission(SecurityAction.LinkDemand, ObjectModel = true)]
        public override void ItemAdded(SPItemEventProperties properties)
        {
            ////Add code for event ItemAdded in WebSitePageEventHandler 

            /*
            SPListItem oItem = properties.ListItem;
            oItem["Body"] = "Body text maintained by the system.";
            oItem.Update();
            */
        }

        /// <summary>
        /// TODO: Add comment for event ItemDeleted in WebSitePageEventHandler 
        /// </summary>
        /// <param name="properties">Contains list event properties</param>   
        [SharePointPermission(SecurityAction.LinkDemand, ObjectModel = true)]
        public override void ItemDeleted(SPItemEventProperties properties)
        {
            if (properties.List.BaseTemplate.ToString() == "20002")
            {
                EventFiringEnabled = false;

                ////Add code for event ItemDeleted in WebSitePageEventHandler 

                EventFiringEnabled = true;
            }
        }

        /// <summary>
        /// TODO: Add comment for event ItemFileMoved in WebSitePageEventHandler 
        /// </summary>
        /// <param name="properties">Contains list event properties</param>   
        [SharePointPermission(SecurityAction.LinkDemand, ObjectModel = true)]
        public override void ItemFileMoved(SPItemEventProperties properties)
        {
            if (properties.List.BaseTemplate.ToString() == "20002")
            {
                EventFiringEnabled = false;

                ////Add code for event ItemFileMoved in WebSitePageEventHandler 

                EventFiringEnabled = true;
            }
        }

        /// <summary>
        /// TODO: Add comment for event ItemUpdated in WebSitePageEventHandler 
        /// </summary>
        /// <param name="properties">Contains list event properties</param>   
        [SharePointPermission(SecurityAction.LinkDemand, ObjectModel = true)]
        public override void ItemUpdated(SPItemEventProperties properties)
        {
            try
            {
                if (properties.List.BaseTemplate.ToString() == "20002")
                {
                    EventFiringEnabled = false;
                    SPItemEventDataCollection before = properties.BeforeProperties;
                    SPItemEventDataCollection after = properties.AfterProperties;
                    ////Add code for event ItemUpdated in WebSitePageEventHandler 
                    EventFiringEnabled = true;
                }
            }
            catch (Exception ex)
            {
                EventFiringEnabled = true;
                ex.ToString();
            }
        }

        private void CreateWorkItem(SPWeb web, string pagename, string url)
        {
            Guid siteId = web.Site.ID;
            Guid webId = web.ID;

            if (url.StartsWith("/"))
            {
                url = url.TrimStart('/');
            }

            bool disabled = false;
            WebSiteControllerPrincipalType principalType = WebSiteControllerPrincipalType.None;
            bool appliesToSSL = true;
            int sequence = 1;
            String pricipal = string.Empty;

            StringBuilder builder = new StringBuilder();
            builder.Append(url + ";");
            builder.Append(disabled.ToString() + ";");
            builder.Append(appliesToSSL.ToString() + ";");
            builder.Append(sequence.ToString() + ";");
            builder.Append(principalType.ToString() + ";");
            builder.Append(pricipal + ";");
            builder.Append("#");

            if (url.EndsWith("/"))
            {
                builder.Append(String.Format("{0}:{1};", "OriginalUrl", url + pagename + ".aspx"));
            }
            else
            {
                builder.Append(String.Format("{0}:{1};", "OriginalUrl", url + ".aspx"));
            }


            string full = builder.ToString();

            SemanticModule mod = new SemanticModule();
            IWebSiteControllerModule imod = null;

            while (imod == null)
            {
                try
                {
                    imod = WebSiteControllerConfig.GetModule(web.Site.WebApplication, mod.RuleType);
                }
                catch (Exception ex)
                {
                    ex.ToString();
                }
                System.Threading.Thread.Sleep(1000);
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

