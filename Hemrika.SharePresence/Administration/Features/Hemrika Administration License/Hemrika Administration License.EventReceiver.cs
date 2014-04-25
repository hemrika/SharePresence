using System;
using System.Runtime.InteropServices;
using System.Security.Permissions;
using Microsoft.SharePoint;
using Microsoft.SharePoint.Security;
using Microsoft.SharePoint.Administration;
using System.Globalization;

namespace Hemrika.SharePresence.Administration.Features.Hemrika_Administration_License
{
    /// <summary>
    /// This class handles events raised during feature activation, deactivation, installation, uninstallation, and upgrade.
    /// </summary>
    /// <remarks>
    /// The GUID attached to this class may be used during packaging and should not be modified.
    /// </remarks>

    [Guid("7c74c6cc-01c4-4b38-ab8e-5b4d5a94c563")]
    public class Hemrika_Administration_LicenseEventReceiver : SPFeatureReceiver
    {
        // Uncomment the method below to handle the event raised after a feature has been activated.

        public override void FeatureActivated(SPFeatureReceiverProperties properties)
        {
            SPWeb web = (SPWeb)properties.Feature.Parent;

            SPWebApplication webApp = web.Site.WebApplication;
            //SPWebApplication webApp = (SPWebApplication)properties.Feature.Parent;

            if (!webApp.IsAdministrationWebApplication)
            {
                Guid featureId = properties.Feature.DefinitionId;
                webApp.Features.Remove(featureId, true);
                webApp.Update();

                throw new SPException(string.Format(CultureInfo.InvariantCulture, "You can activate the feature with ID {0} on the Central Administration Web Application only!", featureId));
            }
            else
            {
                SPList list;
                
                Guid guid = web.Lists.Add("Hemrika License Files", "", SPListTemplateType.DocumentLibrary);
                list = web.Lists[guid];
                list.OnQuickLaunch = true;
                //list.Hidden = true;
                list.Update();
            }

        }


        // Uncomment the method below to handle the event raised before a feature is deactivated.

        //public override void FeatureDeactivating(SPFeatureReceiverProperties properties)
        //{
        //}


        // Uncomment the method below to handle the event raised after a feature has been installed.

        //public override void FeatureInstalled(SPFeatureReceiverProperties properties)
        //{
        //}


        // Uncomment the method below to handle the event raised before a feature is uninstalled.

        //public override void FeatureUninstalling(SPFeatureReceiverProperties properties)
        //{
        //}

        // Uncomment the method below to handle the event raised when a feature is upgrading.

        //public override void FeatureUpgrading(SPFeatureReceiverProperties properties, string upgradeActionName, System.Collections.Generic.IDictionary<string, string> parameters)
        //{
        //}
    }
}
