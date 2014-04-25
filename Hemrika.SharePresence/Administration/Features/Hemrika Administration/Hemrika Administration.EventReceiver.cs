using System;
using System.Runtime.InteropServices;
using System.Security.Permissions;
using Microsoft.SharePoint;
using Microsoft.SharePoint.Security;
using Microsoft.SharePoint.Administration;
using System.Globalization;

namespace Hemrika.SharePresence.Administration.Features.Hemrika_Administration
{
    /// <summary>
    /// This class handles events raised during feature activation, deactivation, installation, uninstallation, and upgrade.
    /// </summary>
    /// <remarks>
    /// The GUID attached to this class may be used during packaging and should not be modified.
    /// </remarks>

    [Guid("e7924f9d-2d64-4583-8e53-4ce34be547fd")]
    public class Hemrika_AdministrationEventReceiver : SPFeatureReceiver
    {
        // Uncomment the method below to handle the event raised after a feature has been activated.

        public override void FeatureActivated(SPFeatureReceiverProperties properties)
        {
            SPWebApplication webApp = (SPWebApplication)properties.Feature.Parent;

            if (!webApp.IsAdministrationWebApplication)
            {
                Guid featureId = properties.Feature.DefinitionId;
                webApp.Features.Remove(featureId, true);
                webApp.Update();

                throw new SPException(string.Format(CultureInfo.InvariantCulture, "You can activate the feature with ID {0} on the Central Administration Web Application only!", featureId));
            }
            /*
            else
            {
                SPList list;
                SPWeb caWeb = webApp.Sites[0].OpenWeb();
                Guid guid = caWeb.Lists.Add("Hemrika License Files","", SPListTemplateType.DocumentLibrary);
                list = caWeb.Lists[guid];
                list.OnQuickLaunch = true;
                //list.Hidden = true;
                list.Update();
            }
            */
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
