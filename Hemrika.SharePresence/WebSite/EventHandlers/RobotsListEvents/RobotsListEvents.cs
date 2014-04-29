using System;
using System.Security.Permissions;
using Microsoft.SharePoint;
using Microsoft.SharePoint.Security;
using Microsoft.SharePoint.Utilities;
using Microsoft.SharePoint.Workflow;

namespace Hemrika.SharePresence.WebSite.Robots
{
    /// <summary>
    /// List Item Events
    /// </summary>
    public class RobotsListEvents : SPItemEventReceiver
    {
        /// <summary>
        /// An item is being added.
        /// </summary>
        public override void ItemAdding(SPItemEventProperties properties)
        {
            try
            {
                if (properties.List.BaseTemplate.ToString() == "20003")
                {
                    string entry = properties.ListItem["Entry"].ToString();
                    string follow = properties.ListItem["Follow"].ToString();
                    SPWeb web = SPContext.Current.Site.OpenWeb(entry);
                    SPListItem item = web.GetListItem(entry);

                    if (item.Properties.ContainsKey("UsedInRobots"))
                    {
                        item.Properties["UsedInRobots"] = follow;
                    }
                    else
                    {
                        item.Properties.Add("UsedInRobots", follow);
                    }

                    item.Update();
                }
            }
            catch (Exception ex)
            {
                ex.ToString();
                //throw;
            }

            base.ItemAdding(properties);
        }

        /// <summary>
        /// An item is being updated.
        /// </summary>
        public override void ItemUpdating(SPItemEventProperties properties)
        {
            //TODO 
            /*
            try
            {

                string entry = properties.ListItem["Entry"].ToString();
                string follow = properties.ListItem["Follow"].ToString();

                SPWeb web = SPContext.Current.Site.OpenWeb(entry);
                SPListItem item = web.GetListItem(entry);
                if (item.Properties.ContainsKey("UsedInRobots"))
                {
                    item.Properties["UsedInRobots"] = follow;
                }
                else
                {
                    item.Properties.Add("UsedInRobots", follow);
                }

                item.Update();
            }
            catch (Exception ex)
            {
                ex.ToString();
                //throw;
            }
            */
            base.ItemUpdating(properties);
        }

        /// <summary>
        /// An item is being deleted.
        /// </summary>
        public override void ItemDeleting(SPItemEventProperties properties)
        {
            try
            {
                if (properties.List.BaseTemplate.ToString() == "20003")
                {
                    string entry = properties.ListItem["Entry"].ToString();
                    SPWeb web = SPContext.Current.Site.OpenWeb(entry);
                    SPListItem item = web.GetListItem(entry);
                    if (item.Properties.ContainsKey("UsedInRobots"))
                    {
                        item.Properties.Remove("UsedInRobots");
                    }

                    item.Update();
                }
            }
            catch (Exception ex)
            {
                ex.ToString();
                //throw;
            }

            base.ItemDeleting(properties);
        }

        /// <summary>
        /// An item was added.
        /// </summary>
        public override void ItemAdded(SPItemEventProperties properties)
        {
            base.ItemAdded(properties);
        }

        /// <summary>
        /// An item was updated.
        /// </summary>
        public override void ItemUpdated(SPItemEventProperties properties)
        {
            base.ItemUpdated(properties);
        }

        /// <summary>
        /// An item was deleted.
        /// </summary>
        public override void ItemDeleted(SPItemEventProperties properties)
        {
            base.ItemDeleted(properties);
        }


    }
}
