using Hemrika.SharePresence.WebSite.HTTPModules;
// -----------------------------------------------------------------------
// <copyright file="ErrorModule.cs" company="">
// TODO: Update copyright text.
// </copyright>
// -----------------------------------------------------------------------

namespace Hemrika.SharePresence.WebSite.Modules.MediaModule
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using Hemrika.SharePresence.Common.WebSiteController;
    using System.Web;
    using System.Text.RegularExpressions;
    using System.Globalization;
    using Microsoft.SharePoint;
    using System.IO;
    using Hemrika.SharePresence.WebSite.Modules.MediaModule.Filters;

    /// <summary>
    /// TODO: Update summary.
    /// </summary>
    public class MediaModule : IWebSiteControllerModule, IDisposable
    {
        /// <summary>
        /// The GUID to identify this module
        /// </summary>
        //private const string ID = "FE6B2E3E-2979-4016-9848-C98322E50E55";
        private Guid ID;
        //private HttpApplication application;

        /// <summary>
        /// Initializes a new instance of the <see cref="ErrorModule"/> class.
        /// </summary>
        public MediaModule()
        {
        }

        /// <summary>
        /// Gets the type of the rule.
        /// </summary>
        /// <value>The type of the rule.</value>
        public string RuleType
        {
            get { return this.GetType().FullName; }
        }

        /// <summary>
        /// Gets the short type of the rule for display purposes.
        /// </summary>
        /// <value>The short type of the rule for display purposes.</value>
        public string ShortRuleType
        {
            get { return this.GetType().Name; }
        }

        /// <summary>
        /// Gets the id associated with this module.
        /// </summary>
        /// <value>The id associated with this module.</value>
        public Guid Id
        {
            get { return ID; }
            set { ID = value; }
        }

        /// <summary>
        /// Gets the virutal path to the control used to update properties for this rule type.
        /// </summary>
        /// <value>The control.</value>
        public string Control
        {
            get { return "~/_controltemplates/Hemrika/Controls/MediaModuleControl.ascx"; }
        }

        /// <summary>
        /// Attaches to appropriate request pipeline events
        /// </summary>
        /// <param name="pageControllerModule">The PageControllerModule whose event the module can attach to</param>
        public void Init(WebSiteControllerModule WebSiteControllerModule)
        {
            WebSiteControllerModule.OnPreSendRequestContent += new EventHandler(WebSiteControllerModule_OnPreSendRequestContent);
        }

        void WebSiteControllerModule_OnPreSendRequestContent(object sender, EventArgs e)
        {
            HttpContext context = ((HttpApplication)sender).Context;

            if (context.Request.HttpMethod == "GET" && context.Response.ContentType == "text/html" && !context.Request.IsAuthenticated)
            {
                //context.Response.Filter = new WebPageInlineScriptFilter(context);
                //context.Response.Filter = new WebPageInlineStyleFilter(context);

                if (context.Response.StatusCode == 200 && context.CurrentHandler != null)
                {
                    context.Response.Filter = new MediaFilter(context);
                }
            }
        }

        /// <summary>
        /// Gets the user-friendly name of a property.
        /// </summary>
        /// <param name="propertyName">Name of the property.</param>
        /// <returns>
        /// The friendly name of the property to be used for display purposes
        /// </returns>
        public string GetFriendlyName(string propertyName)
        {
            switch (propertyName)
            {
                case "OriginalUrl":
                    return "Original Url";
                default:
                    return propertyName;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        public bool CanBeRemoved
        {
            get { return true; }
        }

        /// <summary>
        /// 
        /// </summary>
        public bool AlwaysRun
        {
            get { return true; }
        }

        /// <summary>
        /// 
        /// </summary>
        public void Dispose() { }
    }
}
