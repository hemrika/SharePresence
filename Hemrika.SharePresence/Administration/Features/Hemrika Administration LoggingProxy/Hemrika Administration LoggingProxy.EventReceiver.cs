// <copyright file="LoggingProxy.EventReceiver.cs" company="Hemrika">
// Copyright Hemrika. All rights reserved.
// </copyright>
// <author>MARKETING\Administrator</author>
// <date>2011-10-19 21:38:19Z</date>
namespace Hemrika.SharePresence.Administration
{
    using System;
    using System.Collections.Generic;
    using System.Security.Permissions;
    using System.Text;
    using Microsoft.SharePoint;
    using Microsoft.SharePoint.Security;
    using Microsoft.SharePoint.Administration;
    using Microsoft.SharePoint.UserCode;
    using Hemrika.SharePresence.Common.ProxyArgs;
    using Hemrika.SharePresence.Common.Logging;

    /// <summary>
    /// TODO: Add comment to LoggingProxyEventReceiver
    /// </summary>
    [SharePointPermission(SecurityAction.InheritanceDemand, ObjectModel = true)]
    public class LoggingProxyEventReceiver : SPFeatureReceiver
    {
        /// <summary>
        /// Registers the proxy operation for logging and tracing with the farm.
        /// </summary>
        /// <param name="properties">The properties provided to the feature receiver</param>
        public override void FeatureActivated(SPFeatureReceiverProperties properties)
        {
            SPUserCodeService userCodeService = SPUserCodeService.Local;

            SPProxyOperationType loggingOperation =
                new SPProxyOperationType(
                    ProxyOperationTypes.LoggingProxyAssemblyName, ProxyOperationTypes.LoggingOpTypeName);

            userCodeService.ProxyOperationTypes.Add(loggingOperation);

            SPProxyOperationType tracingOperation =
                new SPProxyOperationType(
                    ProxyOperationTypes.LoggingProxyAssemblyName, ProxyOperationTypes.TracingOpTypeName);

            userCodeService.ProxyOperationTypes.Add(tracingOperation);

            userCodeService.Update();
            DiagnosticsService.Register();
        }


        /// <summary>
        /// Removes the proxy operations for logging and tracing from the farm.
        /// </summary>
        /// <param name="properties">The properties provided</param>
        public override void FeatureDeactivating(SPFeatureReceiverProperties properties)
        {
            SPUserCodeService userCodeService = SPUserCodeService.Local;

            SPProxyOperationType loggingOperation =
                new SPProxyOperationType(
                    ProxyOperationTypes.LoggingProxyAssemblyName, ProxyOperationTypes.LoggingOpTypeName);

            userCodeService.ProxyOperationTypes.Remove(loggingOperation);

            SPProxyOperationType tracingOperation =
                new SPProxyOperationType(
                    ProxyOperationTypes.LoggingProxyAssemblyName, ProxyOperationTypes.TracingOpTypeName);

            userCodeService.ProxyOperationTypes.Remove(tracingOperation);

            userCodeService.Update();
            DiagnosticsService.Unregister();
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

