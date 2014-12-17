// -----------------------------------------------------------------------
// <copyright file="ErrorModule.cs" company="">
// TODO: Update copyright text.
// </copyright>
// -----------------------------------------------------------------------
//Hemrika.SharePresence.WebSite.Modules.ErrorModule.ErrorModule, Hemrika.SharePresence.WebSite, Version=1.0.0.0, Culture=neutral, PublicKeyToken=11e6604a27f32a11

namespace Hemrika.SharePresence.WebSite.Modules.MobileModule
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
    using System.Net;
    using System.Collections.Specialized;
    using System.Diagnostics;

    /// <summary>
    /// TODO: Update summary.
    /// </summary>
    public class MobileModule : IWebSiteControllerModule, IDisposable
    {
        /// <summary>
        /// The GUID to identify this module
        /// </summary>
        /// 
        //private const string ID = "17A3219B-049F-4056-9566-37590122BE8E";
        private Guid ID;
        //private HttpApplication application;
        private HttpBrowserCapabilities browserCapabilities;

        /// <summary>
        /// Initializes a new instance of the <see cref="ErrorModule"/> class.
        /// </summary>
        public MobileModule()
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
            get { return "~/_controltemplates/Hemrika/Controls/MobileModuleControl.ascx"; }
        }

        /// <summary>
        /// Attaches to appropriate request pipeline events
        /// </summary>
        /// <param name="pageControllerModule">The WebSiteControllerModule whose event the module can attach to</param>
        public void Init(WebSiteControllerModule WebSiteControllerModule)
        {
            WebSiteControllerModule.OnAcquireRequestState += new EventHandler(WebSiteControllerModule_OnAcquireRequestState);
            WebSiteControllerModule.OnEndRequest += new EventHandler(WebSiteControllerModule_OnEndRequest);
            WebSiteControllerModule.OnPreRequestHandlerExecute += new EventHandler(WebSiteControllerModule_OnPreRequestHandlerExecute);
            WebSiteControllerModule.OnBeginRequest += new EventHandler(WebSiteControllerModule_OnBeginRequest);
            WebSiteControllerModule.OnAuthenticateRequest += new EventHandler(WebSiteControllerModule_OnAuthenticateRequest);
        }

        void WebSiteControllerModule_OnEndRequest(object sender, EventArgs e)
        {
            //Uri url = (sender as HttpApplication).Context.Request.Url;
            //Debug.WriteLine("End Mobile:" + DateTime.Now + " : " + url.OriginalString);
        }

        void WebSiteControllerModule_OnPreRequestHandlerExecute(object sender, EventArgs e)
        {
            //Uri url = (sender as HttpApplication).Context.Request.Url;
            //Debug.WriteLine("Begin PreRequestHandler:" + DateTime.Now + " : " + url.OriginalString);

            HttpApplication application = sender as HttpApplication;

            string absolutePath = application.Request.Url.AbsolutePath.ToLower();
            if (absolutePath.Contains(".dll") ||
                absolutePath.Contains(".asmx") ||
                absolutePath.Contains(".svc") ||
                absolutePath.Contains(".axd") ||
                absolutePath.Contains(".ashx") ||
                absolutePath.Contains("favicon.ico"))
            {
                return;
            }

            application.Request.Browser = new MobileBrowserCapabilities(browserCapabilities);

            //Debug.WriteLine("End PreRequestHandler:" + DateTime.Now + " : " + url.OriginalString);
        }

        void WebSiteControllerModule_OnAcquireRequestState(object sender, EventArgs e)
        {
            //Uri url = (sender as HttpApplication).Context.Request.Url;
            //Debug.WriteLine("Begin AcquireRequest:" + DateTime.Now + " : " + url.OriginalString);

            HttpApplication application = sender as HttpApplication;

            string absolutePath = application.Request.Url.AbsolutePath.ToLower();
            if (absolutePath.Contains(".dll") ||
                absolutePath.Contains(".asmx") ||
                absolutePath.Contains(".svc") ||
                absolutePath.Contains("favicon.ico"))
            {
                return;
            }

            application.Request.Browser = new MobileBrowserCapabilities(browserCapabilities);

            //Debug.WriteLine("End AcquireRequest:" + DateTime.Now + " : " + url.OriginalString);

        }

        void WebSiteControllerModule_OnAuthenticateRequest(object sender, EventArgs e)
        {
            //Uri url = (sender as HttpApplication).Context.Request.Url;
            //Debug.WriteLine("End AuthenticateRequest:" + DateTime.Now + " : " + url.OriginalString);
            HttpApplication application = sender as HttpApplication;

            string absolutePath = application.Request.Url.AbsolutePath.ToLower();
            if (absolutePath.Contains(".dll") ||
                absolutePath.Contains(".asmx") ||
                absolutePath.Contains(".svc") ||
                absolutePath.Contains(".axd") ||
                absolutePath.Contains(".ashx") ||
                absolutePath.Contains("favicon.ico"))
            {
                return;
            }

            application.Request.Browser = new MobileBrowserCapabilities(browserCapabilities);
            //Debug.WriteLine("End AuthenticateRequest:" + DateTime.Now + " : " + url.OriginalString);
        }

        void WebSiteControllerModule_OnBeginRequest(object sender, EventArgs e)
        {
            //Uri url = (sender as HttpApplication).Context.Request.Url;
            //Debug.WriteLine("Start OnBeginRequest:" + DateTime.Now + " : " + url.OriginalString);

            HttpApplication application = sender as HttpApplication;

            string absolutePath = application.Request.Url.AbsolutePath.ToLower();
            if (absolutePath.Contains(".dll") ||
                absolutePath.Contains(".asmx") ||
                absolutePath.Contains(".svc") ||
                absolutePath.Contains(".axd") ||
                absolutePath.Contains(".ashx") ||
                absolutePath.Contains("favicon.ico"))
            {
                return;
            }

            browserCapabilities = application.Request.Browser;
            application.Request.Browser = new MobileBrowserCapabilities(browserCapabilities);

            //Debug.WriteLine("End OnBeginRequest:" + DateTime.Now + " : " + url.OriginalString);
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
                case "Mobile":
                    return "Mobile";
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

        private bool _disposed = false;

        protected virtual void Dispose(bool disposing)
        {
            if (!_disposed)
            {
                if (disposing)
                {
                    browserCapabilities = null;
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

        ~MobileModule()
        {
            Dispose(false);
        }
    }
}
