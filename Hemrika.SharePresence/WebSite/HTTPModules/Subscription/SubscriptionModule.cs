// -----------------------------------------------------------------------
// <copyright file="ErrorModule.cs" company="">
// TODO: Update copyright text.
// </copyright>
// -----------------------------------------------------------------------

namespace Hemrika.SharePresence.WebSite.Modules.Subscription
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
    using System.Web.SessionState;
    using System.Net;
    using Hemrika.SharePresence.WebSite.ContentTypes;

    /// <summary>
    /// TODO: Update summary.
    /// </summary>
    public class SubscriptionModule : IWebSiteControllerModule, IDisposable
    {
        /// <summary>
        /// The GUID to identify this module
        /// </summary>
        //private const string ID = "FE6B2E3E-2979-4016-9848-C98322E50E55";
        private Guid ID;
        private HttpApplication application;

        /// <summary>
        /// Initializes a new instance of the <see cref="SubscriptionModule"/> class.
        /// </summary>
        public SubscriptionModule()
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
            get { return "~/_controltemplates/Hemrika/Controls/SubscriptionModuleControl.ascx"; }
        }

        /// <summary>
        /// Attaches to appropriate request pipeline events
        /// </summary>
        /// <param name="pageControllerModule">The PageControllerModule whose event the module can attach to</param>
        public void Init(WebSiteControllerModule WebSiteControllerModule)
        {
            //WebSiteControllerModule.OnPostMapRequestHandler += new EventHandler(WebSiteControllerModule_OnPostMapRequestHandler);
            //WebSiteControllerModule.OnPostAcquireRequestState += new EventHandler(WebSiteControllerModule_OnPostAcquireRequestState);
            //WebSiteControllerModule.OnPreRequestHandlerExecute += new EventHandler(WebSiteControllerModule_OnPreRequestHandlerExecute);
        }

        void WebSiteControllerModule_OnPreRequestHandlerExecute(object sender, EventArgs e)
        {
            HttpContext context = ((HttpApplication)sender).Context;
            if (!IsSubscriptionItem(context.Request)) return;

            /*
            string page = "";
            ErrorModule.ErrorModule module = new ErrorModule.ErrorModule();
            WebSiteControllerRule rule = null;

            
            try
            {
                rule = WebSiteControllerConfig.GetRule("ErrorCode", application.Response.StatusCode.ToString());
            }
            catch (Exception ex) { ex.ToString(); };

            if (rule != null)
            {
                page = rule.Properties["ErrorPage"].ToString();
            }
            */

            application = sender as HttpApplication;
            application.Server.ClearError();
            application.Response.StatusCode = (int)HttpStatusCode.Unauthorized;
            throw (new HttpException((int)HttpStatusCode.Unauthorized, ""));
        }

        void WebSiteControllerModule_OnPostAcquireRequestState(object sender, EventArgs e)
        {
            HttpContext context = ((HttpApplication)sender).Context;
            SubscriptionHandler resourceHttpHandler = context.Handler as SubscriptionHandler;
            if (resourceHttpHandler != null)
                // set the original handler back
                context.Handler = resourceHttpHandler.OriginalHandler;
        }

        void WebSiteControllerModule_OnPostMapRequestHandler(object sender, EventArgs e)
        {
            HttpContext context = ((HttpApplication)sender).Context;
            //don't care about images
            if (!IsSubscriptionItem(context.Request)) return;
            // no need to replace the current handler
            if (context.Handler is IReadOnlySessionState || context.Handler is IRequiresSessionState)
                return;
            // swap the current handler
            context.Handler = new SubscriptionHandler(context.Handler);
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
                default:
                    return propertyName;
            }
        }

        public bool CanBeRemoved
        {
            get { return false; }
        }


        public bool AlwaysRun
        {
            get { return true; }
        }

        private static bool IsSubscriptionItem(HttpRequest request)
        {
            if (SPContext.Current != null && SPContext.Current.ListItem != null)
            {
                SPListItem item = SPContext.Current.ListItem;
                if (item.ContentTypeId == ContentTypeId.WebSitePage || ContentTypeId.WebSitePage.IsParentOf(item.ContentTypeId))
                {
                    //TODO Subscription Check.
                    //return true;
                }
            }

            return false;
        }

        private bool _disposed = false;

        protected virtual void Dispose(bool disposing)
        {
            if (!_disposed)
            {
                if (disposing)
                {
                    application.Dispose();
                    application = null;
                }
                _disposed = true;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        ~SubscriptionModule()
        {
            Dispose(false);
        }
    }
}
