// <copyright file="PublishingMasterTemplate_ContentTypeName.EventReceiver.cs" company="Hemrika">
// Copyright Hemrika. All rights reserved.
// </copyright>
// <author>MARKETING\Administrator</author>
// <date>2012-04-27 12:57:15Z</date>
namespace Hemrika.SharePresence.WebSite
{
    using System;
    using System.Collections.Generic;
    using System.Security.Permissions;
    using System.Text;
    using Microsoft.SharePoint;
    using Microsoft.SharePoint.Security;

    /// <summary>
    /// TODO: Add comment for PublishingMasterTemplate_ContentTypeNameEventReceivers
    /// </summary> 
    [SharePointPermission(SecurityAction.LinkDemand, ObjectModel = true)]
    [SharePointPermission(SecurityAction.InheritanceDemand, ObjectModel = true)]
    public class PublishingMasterTemplateEventReceivers : SPItemEventReceiver
    {

        /// <summary>
        /// TODO: Add comment for event ItemAdded in PublishingMasterTemplate_ContentTypeNameEventReceivers
        /// </summary>
        /// <param name="properties">Contains event properties</param>
        public override void ItemAdded(SPItemEventProperties properties)
        {
            base.ItemAdded(properties);
            /*
            DisableEventFiring();
            SPListItem oItem = properties.ListItem;
            oItem["Body"] = "Body text maintained by the system.";
            oItem.Update();
            EnableEventFiring();
            */
        }
        /// <summary>
        /// TODO: Add comment for event ItemUpdated in PublishingMasterTemplate_ContentTypeNameEventReceivers
        /// </summary>
        /// <param name="properties">Contains event properties</param>
        public override void ItemUpdated(SPItemEventProperties properties)
        {
            base.ItemUpdated(properties);
            /*
            DisableEventFiring();
            SPListItem oItem = properties.ListItem;
            oItem["Body"] = "Body text maintained by the system.";
            oItem.Update();
            EnableEventFiring();
            */
        }
        /// <summary>
        /// TODO: Add comment for event ItemDeleted in PublishingMasterTemplate_ContentTypeNameEventReceivers
        /// </summary>
        /// <param name="properties">Contains event properties</param>
        public override void ItemDeleted(SPItemEventProperties properties)
        {
            base.ItemDeleted(properties);
            /*
            properties.Cancel = true;
            properties.ErrorMessage = "Deleting is not supported.";
            */
        }
    }
}
