// <copyright file="WebSiteControllerWebConfigManager.cs" company="MMC Inc.">
//     Copyright (c) Matt Resnick. All rights reserved.
// </copyright>
namespace Hemrika.SharePresence.Common.WebSiteController
{
    using System;
    using System.Collections.ObjectModel;
    using System.Globalization;
    using Microsoft.SharePoint.Administration;
    using Hemrika.SharePresence.Common.Extensions;

    /// <summary>
    /// A class to manage the updates required to the web.config for this features
    /// </summary>
    public static class WebSiteControllerWebConfigManager
    {
        /// <summary>
        /// Name of owner of web.config entries
        /// </summary>
        private const string OWNERNAME = "Hemrika.SharePresence.Common.WebSiteController";

        /// <summary>
        /// Gets the the HttpModule web config item to add.
        /// </summary>
        /// <returns>An SPWebConfigModification to add to the web.config</returns>
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1024:UsePropertiesWhereAppropriate", Justification = "Most appropriate as method")]
        internal static SPWebConfigModification GetHttpModuleEntry()
        {
            SPWebConfigModification item = new SPWebConfigModification();
            item.Name = "add[@name=\"WebSiteController\"]";
            item.Value = @"<add name=""WebSiteController"" type=""Hemrika.SharePresence.Common.WebSiteController.WebSiteControllerModule, Hemrika.SharePresence.Common, Version=2.0.0.0, Culture=neutral, PublicKeyToken=0299ac4c0072d0df"" />";
            item.Type = SPWebConfigModification.SPWebConfigModificationType.EnsureChildNode;
            item.Path = "configuration/system.web/httpModules";
            item.Owner = OWNERNAME;
            item.Sequence = 0;
            return item;
        }

        /// <summary>
        /// Gets the the Module web config item to add.
        /// </summary>
        /// <returns>An SPWebConfigModification to add to the web.config</returns>
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1024:UsePropertiesWhereAppropriate", Justification = "Most appropriate as method")]
        internal static SPWebConfigModification GetModuleEntry()
        {
            SPWebConfigModification item = new SPWebConfigModification();
            item.Name = "add[@name=\"WebSiteController\"]";
            item.Value = @"<add name=""WebSiteController"" type=""Hemrika.SharePresence.Common.WebSiteController.WebSiteControllerModule, Hemrika.SharePresence.Common, Version=2.0.0.0, Culture=neutral, PublicKeyToken=0299ac4c0072d0df"" />";
            item.Type = SPWebConfigModification.SPWebConfigModificationType.EnsureChildNode;
            item.Path = "configuration/system.webServer/modules";
            item.Owner = OWNERNAME;
            item.Sequence = 0;
            return item;            
        }

        /// <summary>
        /// Clears the web config modifications.
        /// </summary>
        /// <param name="webApp">The web app where the config changes will be cleared</param>
        internal static void ClearWebConfigModifications(SPWebApplication webApp)
        {
            Collection<SPWebConfigModification> collection = webApp.WebConfigModifications;
            int startCount = collection.Count;

            // Remove any modifications that were originally created by the owner.
            for (int i = startCount - 1; i >= 0; i--)
            {
                SPWebConfigModification configMod = collection[i];
                if (configMod.Owner.Equals(OWNERNAME))
                {
                    collection.Remove(configMod);
                }
            }

            // Apply changes only if any items were removed.
            if (startCount > collection.Count)
            {
                SPWebConfigModificationExtensions.WaitForOnetimeJobToFinish(webApp.Farm, "job-webconfig-modification", 180);
                webApp.Farm.Services.GetValue<SPWebService>().ApplyWebConfigModifications();
                webApp.Update();
            }
        }
    }
}
