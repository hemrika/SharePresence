// -----------------------------------------------------------------------
// <copyright file="WebSiteControllerWorkitem.cs" company="">
// TODO: Update copyright text.
// </copyright>
// -----------------------------------------------------------------------

namespace Hemrika.SharePresence.Common.WebSiteController
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using Microsoft.SharePoint.Administration;
    using Microsoft.SharePoint;
    using System.Reflection;
    using System.Web;
    using System.Collections;

    public class WebSiteControllerRuleWorkItem : SPWorkItemJobDefinition
    {
        public static readonly string WorkItemJobDisplayName = "WebSite Controller Rule WorkItem";
        public static readonly Guid WorkItemTypeId = new Guid("{4CA50E9A-A671-4842-AAA8-522F37BCBDEF}");

        public override Guid WorkItemType()
        {
            return WorkItemTypeId;
        }

        public override int BatchFetchLimit
        {
            get
            {
                return 50;
            }
        }

        public override string DisplayName
        {
            get
            {
                return WorkItemJobDisplayName;
            }
        }

        public WebSiteControllerRuleWorkItem()
        {

        }

        public WebSiteControllerRuleWorkItem(string name, SPWebApplication webApplication)
            : base(name, webApplication)
        {

        }

        public override bool AutoDeleteWorkItemWhenException
        {
            get
            {
                return true;
            }
        }

        protected override bool ProcessWorkItems(SPContentDatabase contentDatabase, SPWorkItemCollection workItems, SPJobState jobState)
        {
            foreach (SPWorkItem workItem in workItems)
            {
                if (workItem != null)
                {
                    try
                    {

                        SPSite site = new SPSite(workItem.SiteId);
                        IWebSiteControllerModule module = null;
                        WebSiteControllerRule rule = null;
                        bool _deleted = false;
                        //int ItemID = workItem.ItemId;

                        if (workItem.ItemId > 0)
                        {
                            rule = WebSiteControllerConfig.GetRule(contentDatabase.WebApplication, workItem.ItemGuid);
                        }
                        else
                        {
                            module = WebSiteControllerConfig.GetModule(contentDatabase.WebApplication, workItem.ItemGuid);
                        }

                        if (rule != null)
                        {
                            WebSiteControllerConfig.RemoveRule(rule.Id);
                            _deleted = true;
                        }

                        if (workItem.ItemId < 0 || _deleted)
                        {
                            string[] data = workItem.TextPayload.Split(new char[] {'#'});
                            string parameterRule = data[0];
                            string parameterProperties = data[1];
                            string[] rules = parameterRule.Split(new char[] {';'});

                            string url = rules[0];
                            bool disabled = bool.Parse(rules[1]);
                            bool ssl = bool.Parse(rules[2]);
                            int sequence = int.Parse(rules[3]);

                            WebSiteControllerPrincipalType principalType = WebSiteControllerPrincipalType.None;

                            if (!String.IsNullOrEmpty(rules[4]))
                            {
                                principalType = (WebSiteControllerPrincipalType)Enum.Parse(typeof(WebSiteControllerPrincipalType), rules[4]);
                            }
                            string principal = rules[5];

                            string ruletype = string.Empty;
                            
                            if (module != null || String.IsNullOrEmpty(rule.RuleType))
                            {
                                ruletype = module.RuleType;
                            }
                            else if (rule != null && ruletype == string.Empty)
                            {
                                ruletype = rule.RuleType;
                            }

                            string[] properties = parameterProperties.Split(new char[] {';'},StringSplitOptions.RemoveEmptyEntries);
                            Hashtable props = new Hashtable();

                            foreach (string prop in properties)
                            {
                                try
                                {
                                    if (!string.IsNullOrEmpty(prop))
                                    {
                                        string[] keyval = prop.Split(new char[] { ':' });
                                        props.Add(keyval[0], keyval[1]);
                                    }
                                }
                                catch { };
                            }

                            if (_deleted && workItem.ItemId != 1)
                            {
                                WebSiteControllerConfig.AddRule(contentDatabase.WebApplication,
                                    site.Url + "/",
                                    url,
                                    rule.RuleType,
                                    rule.Principal,
                                    rule.PrincipalType,
                                    disabled,
                                    ssl,
                                    sequence,
                                    props
                                    );
                            }

                            if(workItem.ItemId == -1)
                            {
                                WebSiteControllerConfig.AddRule(contentDatabase.WebApplication,
                                    site.Url + "/",
                                    url,
                                    ruletype,
                                    principal,
                                    principalType,
                                    disabled,
                                    ssl,
                                    sequence,
                                    props
                                    );
                            }
                        }
                    }
                    catch (Exception ex)
                    {
                        ex.ToString();
                        //throw (ex);
                    }
                    finally
                    {
                        DeleteWorkItem(workItems, workItem);
                        //using (SPSite site = new SPSite(workItem.SiteId))
                        //{

                        //    using (SPWeb web = site.OpenWeb(workItem.WebId))
                        //    {
                        //        SPWorkItemCollection deletableCollection = workItems.SubCollection(site, web, (uint)workItem.i, (uint)itemIndex + 1);
                        //        deletableCollection.DeleteWorkItem(currentItem.Id);

                        //        //workItems.CompleteInProgressWorkItems(workItem.ParentId, workItem.Type, workItem.BatchId);
                        //        //workItems.SubCollection(site, web, 0, (uint)workItems.Count).DeleteWorkItem(workItem.Id);

                        //    }

                        //}

                        ////workItems.DeleteWorkItem(workItem.Id);
                    }

                }
            }
            return true;
        }

        protected void DeleteWorkItem(SPWorkItemCollection itemsToProcess, SPWorkItem currentItem)
        {
            int itemIndex = -1; int count = itemsToProcess.Count; for (int i = 0; i < count; i++)
            {
                if (itemsToProcess[i].Id == currentItem.Id)
                {
                    itemIndex = i;
                    break;
                }
            }
            if (itemIndex > -1)
            {
                try
                {
                using (SPSite site = new SPSite(currentItem.SiteId))
                {
                    using (SPWeb web = site.OpenWeb(currentItem.WebId))
                    {
                        SPWorkItemCollection deletableCollection = itemsToProcess.SubCollection(site, web, (uint)itemIndex, (uint)itemIndex + 1);
                        deletableCollection.DeleteWorkItem(currentItem.Id);
                    }
                }
                }
                catch (Exception ex)
                {
                    ex.ToString();
                }
            }
        }
    }
}