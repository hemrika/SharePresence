// <copyright file="WebSitePage.EventReceiver.cs" company="Hemrika">
// Copyright Hemrika. All rights reserved.
// </copyright>
// <author>MARKETING\Administrator</author>
// <date>2012-02-14 21:22:25Z</date>
namespace Hemrika.SharePresence.WebSite
{
    using System;
    using System.Collections.Generic;
    using System.Security.Permissions;
    using System.Text;
    using Microsoft.SharePoint;
    using Microsoft.SharePoint.Security;
    using Microsoft.SharePoint.Administration;

    /// <summary>
    /// TODO: Add comment for WebSitePageEventReceivers
    /// </summary> 
    [SharePointPermission(SecurityAction.LinkDemand, ObjectModel = true)]
    [SharePointPermission(SecurityAction.InheritanceDemand, ObjectModel = true)]
    public class WebSitePageEventReceivers : SPItemEventReceiver
    {

        /// <summary>
        /// TODO: Add comment for event ItemAdded in WebSitePageEventReceivers
        /// </summary>
        /// <param name="properties">Contains event properties</param>
        public override void ItemAdded(SPItemEventProperties properties)
        {
            base.ItemAdded(properties);
            string title = properties.ListItem.Title.ToString();
            /*
            DisableEventFiring();
            SPListItem oItem = properties.ListItem;
            oItem["Body"] = "Body text maintained by the system.";
            oItem.Update();
            EnableEventFiring();
            */
        }

        public override void ItemUpdated(SPItemEventProperties properties)
        {
            base.ItemUpdated(properties);
        }

        public override void ItemDeleted(SPItemEventProperties properties)
        {
            base.ItemDeleted(properties);
        }

        public override void ItemFileMoved(SPItemEventProperties properties)
        {
            base.ItemFileMoved(properties);
        }
    }
}
