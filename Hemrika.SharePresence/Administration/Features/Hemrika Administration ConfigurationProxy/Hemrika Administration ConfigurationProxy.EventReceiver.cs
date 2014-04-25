// <copyright file="ConfigurationProxy.EventReceiver.cs" company="Hemrika">
// Copyright Hemrika. All rights reserved.
// </copyright>
// <author>MARKETING\Administrator</author>
// <date>2011-10-19 21:15:31Z</date>
namespace Hemrika.SharePresence.Administration
{
    using System;
    using System.Collections.Generic;
    using System.Security.Permissions;
    using System.Text;
    using Microsoft.SharePoint;
    using Microsoft.SharePoint.Security;
    using Microsoft.SharePoint.Administration;
    using Hemrika.SharePresence.Common.ProxyArgs;
    using Microsoft.SharePoint.UserCode;

    /// <summary>
    /// TODO: Add comment to ConfigurationProxyEventReceiver
    /// </summary>
    [SharePointPermission(SecurityAction.InheritanceDemand, ObjectModel = true)]
    public class ConfigurationProxyEventReceiver : SPFeatureReceiver
    {
        // Uncomment the method below to handle the event raised after a feature has been activated.
        /// <summary>
        /// Registers the proxy operations with the farm.
        /// </summary>
        /// <param name="properties">the properties provided by the feature receiver</param>
        public override void FeatureActivated(SPFeatureReceiverProperties properties)
        {
            SPUserCodeService userCodeService = SPUserCodeService.Local;

            AddProxyOperation(userCodeService, ContainsKeyDataArgs.OperationAssemblyName, ContainsKeyDataArgs.OperationTypeName);
            AddProxyOperation(userCodeService, ReadConfigArgs.OperationAssemblyName, ReadConfigArgs.OperationTypeName);
            AddProxyOperation(userCodeService, ProxyInstalledArgs.OperationAssemblyName, ProxyInstalledArgs.OperationTypeName);
            userCodeService.Update();
        }

        private void AddProxyOperation(SPUserCodeService service, string assembly, string operation)
        {
            var proxyOp = new SPProxyOperationType(assembly, operation);
            service.ProxyOperationTypes.Add(proxyOp);
        }


        /// <summary>
        /// Removes the proxy operations from the farm for configuration proxies.
        /// </summary>
        /// <param name="properties">The properties provided on deactivation</param>
        public override void FeatureDeactivating(SPFeatureReceiverProperties properties)
        {
            SPUserCodeService userCodeService = SPUserCodeService.Local;

            RemoveProxyOperation(userCodeService, ContainsKeyDataArgs.OperationAssemblyName, ContainsKeyDataArgs.OperationTypeName);
            RemoveProxyOperation(userCodeService, ReadConfigArgs.OperationAssemblyName, ReadConfigArgs.OperationTypeName);
            RemoveProxyOperation(userCodeService, ProxyInstalledArgs.OperationAssemblyName, ProxyInstalledArgs.OperationTypeName);
            userCodeService.Update();
        }

        private bool RemoveProxyOperation(SPUserCodeService service, string assembly, string operation)
        {
            var proxyOp = new SPProxyOperationType(assembly, operation);
            bool removed = service.ProxyOperationTypes.Remove(proxyOp);
            return removed;
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

