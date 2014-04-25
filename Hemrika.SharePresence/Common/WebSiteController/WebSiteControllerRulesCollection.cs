// <copyright file="WebSiteControllerRulesCollection.cs" company="MMC Inc.">
//     Copyright (c) Matt Resnick. All rights reserved.
// </copyright>
namespace Hemrika.SharePresence.Common.WebSiteController
{
    using Microsoft.SharePoint.Administration;

    /// <summary>
    /// A collection of Page Control rules that can be persisted to the config database
    /// </summary>
    public sealed class WebSiteControllerRulesCollection : SPPersistedChildCollection<WebSiteControllerRule>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="WebSiteControllerRulesCollection"/> class.
        /// </summary>
        /// <param name="config">The config.</param>
        internal WebSiteControllerRulesCollection(WebSiteControllerConfig config) :
            base(config)
        {
        }
    }
}