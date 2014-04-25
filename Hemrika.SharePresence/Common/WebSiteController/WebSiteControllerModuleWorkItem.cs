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

    public class WebSiteControllerModuleWorkItem : SPWorkItemJobDefinition
    {
        public static readonly string WorkItemJobDisplayName = "WebSite Controller Module WorkItem";
        public static readonly Guid WorkItemTypeId = new Guid("{F0A3610F-7B95-4DFF-A79F-EA90CF9F2721}");

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

        public WebSiteControllerModuleWorkItem()
        {

        }

        public WebSiteControllerModuleWorkItem(string name, SPWebApplication webApplication)
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
                    if (workItem.ItemGuid != Guid.Empty)
                    {
                        WebSiteControllerConfig.RemoveModule(contentDatabase.WebApplication, workItem.ItemGuid);
                    }

                    if (!String.IsNullOrEmpty(workItem.TextPayload))
                    {
                        try
                        {
                            string[] param = workItem.TextPayload.Split(new char[] { ';' });

                            Assembly assembly = System.Reflection.Assembly.Load(param[1]);
                            Type moduletype = null;
                            Type[] types = assembly.GetTypes();

                            foreach (Type type in types)
                            {
                                string name = type.FullName;
                                if (name.Equals(param[0]))
                                {
                                    moduletype = type;
                                    break;
                                }
                            }

                            if (moduletype != null)
                            {
                                SPWebApplication WebApp = contentDatabase.WebApplication;
                                //WebSiteControllerConfig config = WebApp.GetChild<WebSiteControllerConfig>(WebSiteControllerConfig.OBJECTNAME);
                                IWebSiteControllerModule module = (IWebSiteControllerModule)Activator.CreateInstance(moduletype);
                                WebSiteControllerConfig.AddModule(WebApp, module, param[0] + "," + param[1]);
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
                            /*
                            using (SPSite site = new SPSite(workItem.SiteId))
                            {

                                using (SPWeb web = site.OpenWeb(workItem.WebId))
                                {
                                    workItems.CompleteInProgressWorkItems(workItem.ParentId, workItem.Type, workItem.BatchId);
                                    //workItems.SubCollection(site, web, 0, (uint)workItems.Count).DeleteWorkItem(workItem.Id);

                                }

                            }
                            */
                        }

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
                using (SPSite site = new SPSite(currentItem.SiteId))
                {
                    using (SPWeb web = site.OpenWeb(currentItem.WebId))
                    {
                        SPWorkItemCollection deletableCollection = itemsToProcess.SubCollection(site, web, (uint)itemIndex, (uint)itemIndex + 1);
                        deletableCollection.DeleteWorkItem(currentItem.Id);
                    }
                }
            }
        }

    }
}
