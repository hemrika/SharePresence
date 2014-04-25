// <copyright company="Hemrika">
// Copyright Hemrika. All rights reserved.
// </copyright>
// <author>SEO\Administrator</author>
// <date>2011-09-09 20:48:32Z</date>
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Hemrika.SharePresence.Common.WebSiteController
{
    /// <summary>
    /// 
    /// </summary>
    public interface IWebSiteControllerModule
    {
        
        /// <summary>
        /// Gets the id.
        /// </summary>
        /// <value>The id of the rule.</value>
        Guid Id
        {
            get;
            set;
        }
        

        /// <summary>
        /// Gets the type of the rule.
        /// </summary>
        /// <value>The type of the rule.</value>
        string RuleType
        {
            get;
        }

        /// <summary>
        /// Gets the short type of the rule for display purposes.
        /// </summary>
        /// <value>The short type of the rule for display purposes.</value>
        string ShortRuleType
        {
            get;
        }

        /// <summary>
        /// Gets the path to the control used to set the rule's properties.
        /// </summary>
        /// <value>The relative path to the control.</value>
        string Control
        {
            get;
        }

        /// <summary>
        /// Gets the value wther the Module can be removed via the user interface
        /// </summary>
        bool CanBeRemoved
        {
            get;
        }

        /// <summary>
        /// 
        /// </summary>
        bool AlwaysRun
        {
            get;
        }

        /// <summary>
        /// Gets the user-friendly name of a property.
        /// </summary>
        /// <param name="propertyName">Name of the property.</param>
        /// <returns>The friendly name of the property to be used for display purposes</returns>
        string GetFriendlyName(string propertyName);

        /// <summary>
        /// Attaches to appropriate request pipeline events
        /// </summary>
        /// <param name="WebSiteControllerModule">The page controller module.</param>
        void Init(WebSiteControllerModule WebSiteControllerModule);

    }
}
