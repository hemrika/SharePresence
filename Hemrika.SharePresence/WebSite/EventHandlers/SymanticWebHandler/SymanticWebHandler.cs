// <copyright file="SymanticWebHandler.cs" company="Hemrika">
// Copyright Hemrika. All rights reserved.
// </copyright>
// <author>MARKETING\Administrator</author>
// <date>2012-01-13 12:35:25Z</date>
namespace Hemrika.SharePresence.WebSite
{
    using System;
    using System.Collections.Generic;
    using System.Security.Permissions;
    using System.Text;
    using Microsoft.SharePoint;
    using Microsoft.SharePoint.Security;
    using Hemrika.SharePresence.Common.WebSiteController;
    using Microsoft.SharePoint.Administration;
    using System.Collections.ObjectModel;

    /// <summary>
    /// TODO: Add comment for SymanticWebHandler
    /// </summary> 
    public class SymanticWebHandler : SPWebEventReceiver
    {
        /// <summary>
        /// TODO: Add comment for event WebAdding in SymanticWebHandler 
        /// </summary>
        /// <param name="properties">Contains list event properties</param>
        [SharePointPermission(SecurityAction.LinkDemand, ObjectModel = true)]
        public override void WebAdding(SPWebEventProperties properties)
        {
            
            ////Add code for event WebAdding in SymanticWebHandler
        }

        /// <summary>
        /// TODO: Add comment for event WebProvisioned in SymanticWebHandler 
        /// </summary>
        /// <param name="properties">Contains list event properties</param>
        [SharePointPermission(SecurityAction.LinkDemand, ObjectModel = true)]
        public override void WebProvisioned(SPWebEventProperties properties)
        {
            //this.CreateWorkItem(properties);

            this.SetAvailableSubWebs(properties);
            ////Add code for event WebProvisioned in SymanticWebHandler
        }

        private void SetAvailableSubWebs(SPWebEventProperties properties)
        {
            SPWeb web = properties.Web;

            if (!web.AllProperties.ContainsKey("__WebTemplates"))
            {
                web.AllProperties.Add("__WebTemplates", "");
            }

            web.AllowAllWebTemplates();

            web.Update();

            SPWebTemplateCollection existingLanguageNeutralTemplatesCollection = web.GetAvailableCrossLanguageWebTemplates();
            SPWebTemplateCollection existingLanguageSpecificTemplatesCollection = web.GetAvailableWebTemplates(web.Language);

            Collection<SPWebTemplate> newLanguageNeutralTemplatesCollection = new Collection<SPWebTemplate>();
            Collection<SPWebTemplate> newLanguageSpecificTemplatesCollection = new Collection<SPWebTemplate>();

            foreach (SPWebTemplate existingTemplate in existingLanguageNeutralTemplatesCollection)
            {
                if (existingTemplate.DisplayCategory.Contains("WebSite"))
                {
                    newLanguageNeutralTemplatesCollection.Add(existingTemplate);
                }
            }
            foreach (SPWebTemplate existingTemplate in existingLanguageSpecificTemplatesCollection)
            {

                if (existingTemplate.DisplayCategory.Contains("WebSite"))
                {
                    newLanguageSpecificTemplatesCollection.Add(existingTemplate);
                }
            }

            if (newLanguageNeutralTemplatesCollection.Count != 0)
            {
                web.SetAvailableCrossLanguageWebTemplates(newLanguageNeutralTemplatesCollection);

            }

            if (newLanguageSpecificTemplatesCollection.Count != 0)
            {
                web.SetAvailableWebTemplates(newLanguageSpecificTemplatesCollection, web.Language);
            }

            web.Update();
        }
     
        private void CreateWorkItem(SPWebEventProperties properties)
        {
            Guid siteId = properties.SiteId;
            Guid webId = properties.WebId;
            string url = properties.ServerRelativeUrl;

            if (url.StartsWith("/"))
            {
                url = url.Remove(0, 1);
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

            builder.Append(String.Format("{0}:{1};", "OriginalUrl", url+"/default.aspx"));

            string full = builder.ToString();

            Guid itemGuid = new Guid("386577D9-0777-4AD3-A90A-C240D8B0A49E");
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
                        itemGuid,
                        Guid.Empty,
                        site.SystemAccount.ID,
                        null,
                        builder.ToString(),
                        Guid.Empty
                        );
                }
            });

            SPJobDefinitionCollection jobs = properties.Web.Site.WebApplication.JobDefinitions;
            int _seconds = 0;
            foreach (SPJobDefinition job in jobs)
            {
                if (job.Name == WebSiteControllerRuleWorkItem.WorkItemJobDisplayName)
                {
                    DateTime next = job.Schedule.NextOccurrence(job.LastRunTime);
                    _seconds = next.Millisecond;
                    break;
                }
            }

            //System.Threading.Thread.Sleep(_seconds);

        }

    }
}

