// <copyright file="VideoPage.EventReceiver.cs" company="Hemrika">
// Copyright Hemrika. All rights reserved.
// </copyright>
// <author>MARKETING\Administrator</author>
// <date>2012-05-09 20:41:56Z</date>
namespace Hemrika.SharePresence.WebSite
{
    using System;
    using System.Collections.Generic;
    using System.Security.Permissions;
    using System.Text;
    using Microsoft.SharePoint;
    using Microsoft.SharePoint.Security;

    /// <summary>
    /// TODO: Add comment for VideoPageEventReceivers
    /// </summary> 
    [SharePointPermission(SecurityAction.LinkDemand, ObjectModel = true)]
    [SharePointPermission(SecurityAction.InheritanceDemand, ObjectModel = true)]
    public class VideoPageEventReceivers : SPItemEventReceiver
    {

        /// <summary>
        /// TODO: Add comment for event ItemAdded in VideoPageEventReceivers
        /// </summary>
        /// <param name="properties">Contains event properties</param>
        public override void ItemAdded(SPItemEventProperties properties)
        {
            /*
            DisableEventFiring();
            SPListItem oItem = properties.ListItem;
            oItem["Body"] = "Body text maintained by the system.";
            oItem.Update();
            EnableEventFiring();
            */
        }
        /// <summary>
        /// TODO: Add comment for event ItemUpdated in VideoPageEventReceivers
        /// </summary>
        /// <param name="properties">Contains event properties</param>
        public override void ItemUpdated(SPItemEventProperties properties)
        {
            /*
            DisableEventFiring();
            SPListItem oItem = properties.ListItem;
            oItem["Body"] = "Body text maintained by the system.";
            oItem.Update();
            EnableEventFiring();
            */
        }
        /// <summary>
        /// TODO: Add comment for event ItemDeleted in VideoPageEventReceivers
        /// </summary>
        /// <param name="properties">Contains event properties</param>
        public override void ItemDeleted(SPItemEventProperties properties)
        {
            /*
            properties.Cancel = true;
            properties.ErrorMessage = "Deleting is not supported.";
            */
        }
    }
}
