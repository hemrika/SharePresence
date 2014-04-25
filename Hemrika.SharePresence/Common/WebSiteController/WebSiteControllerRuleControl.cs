// <copyright file="WebSiteControllerRuleControl.cs" company="MMC Inc.">
//     Copyright (c) Matt Resnick. All rights reserved.
// </copyright>
namespace Hemrika.SharePresence.Common.WebSiteController
{
    using System;
    using System.Collections;
    using System.Web.UI;

    /// <summary>
    /// An abstract class that implementers of page control rules must follow
    /// to display the properties of the custom rule on the New Rule Page
    /// </summary>
    public abstract class WebSiteControllerRuleControl : UserControl
    {
        /// <summary>
        /// Gets the properties.
        /// </summary>
        /// <value>The properties.</value>
        public abstract Hashtable Properties 
        { 
            get; 
        }

        /// <summary>
        /// Specifies a boolean wether the other default controls should be hidden.
        /// ust diplay the specific Rule Controls.
        /// </summary>
        public abstract bool SimpleView
        {
            get;
        }

        /// <summary>
        /// Returns a default value for the controllled page value of the rule.
        /// This must return a value when SimpleVuew is set True;
        /// </summary>
        public abstract string DefaultUrl
        {
            get;
        }
    }
}
