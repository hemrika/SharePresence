// Copyright Hemrika. All rights reserved.
// </copyright>
// <author>SEO\Administrator</author>
// <date>2011-09-09 20:48:32Z</date>
namespace Hemrika.SharePresence.Common.WebSiteController
{
    using Microsoft.SharePoint.Administration;

    /// <summary>
    /// A collection of Page Controller modules that can be persisted to the config database
    /// </summary>
    internal sealed class WebSiteControllerModulesCollection : SPPersistedChildCollection<PersistedWebSiteControllerModule>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="WebSiteControllerModulesCollection"/> class.
        /// </summary>
        /// <param name="config">The config.</param>
        internal WebSiteControllerModulesCollection(WebSiteControllerConfig config) :
            base(config)
        {
        }

    }
}