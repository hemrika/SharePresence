using System;
using System.Runtime.InteropServices;
using System.Security.Permissions;
using Microsoft.SharePoint;
using Microsoft.SharePoint.Security;
using Microsoft.SharePoint.Administration;

namespace Hemrika.SharePresence.Administration.Features.Hemrika_Administration_Jobs
{
    /// <summary>
    /// This class handles events raised during feature activation, deactivation, installation, uninstallation, and upgrade.
    /// </summary>
    /// <remarks>
    /// The GUID attached to this class may be used during packaging and should not be modified.
    /// </remarks>

    [Guid("4eb488be-6823-4d84-b348-d3ec70915f65")]
    public class Hemrika_Administration_JobsEventReceiver : SPFeatureReceiver
    {
        // Uncomment the method below to handle the event raised after a feature has been activated.

        public override void FeatureActivated(SPFeatureReceiverProperties properties)
        {
            try
            {
                SPContext.Current.Web.AllowUnsafeUpdates = true;

                SPAdministrationWebApplication caWebApp = SPAdministrationWebApplication.Local;
                //Delete any existing jobs
                string jobName = "Check License";
                foreach (SPJobDefinition job in caWebApp.JobDefinitions)
                {
                    if (job.Name.ToUpper().Equals(jobName.ToUpper()))
                    {
                        job.Delete();
                    }
                }

                //Schedule new job
                SPMonthlySchedule schedule4 = new SPMonthlySchedule();
                schedule4.BeginDay = 1;
                schedule4.EndDay = 1;
                schedule4.BeginHour = 2;
                schedule4.EndHour = 6;
                LicenseChecker newJob4 = new LicenseChecker(jobName, caWebApp);
                newJob4.Schedule = schedule4;
                newJob4.Update();

            }
            catch (Exception ex)
            {
                SPDiagnosticsService.Local.WriteTrace(0, new SPDiagnosticsCategory(ex.Source, TraceSeverity.High, EventSeverity.Error), TraceSeverity.High, ex.Message, ex.Data);
            }

        }


        // Uncomment the method below to handle the event raised before a feature is deactivated.

        public override void FeatureDeactivating(SPFeatureReceiverProperties properties)
        {
            try
            {
                SPContext.Current.Web.AllowUnsafeUpdates = true;
                SPAdministrationWebApplication caWebApp = SPAdministrationWebApplication.Local;
                //Delete any existing jobs
                string jobName = "Check License";
                foreach (SPJobDefinition job in caWebApp.JobDefinitions)
                {
                    if (job.Name.ToUpper().Equals(jobName.ToUpper()))
                    {
                        job.Delete();
                    }
                }
            }
            catch (Exception ex)
            {
                SPDiagnosticsService.Local.WriteTrace(0, new SPDiagnosticsCategory(ex.Source, TraceSeverity.High, EventSeverity.Error), TraceSeverity.High, ex.Message, ex.Data);
            }
        }

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
