// <copyright file="WebSiteModule.cs" company="Hemrika">
// Copyright Hemrika. All rights reserved.
// </copyright>
// <author>SEO\Administrator</author>
// <date>2011-09-09 20:48:34Z</date>
namespace Hemrika.SharePresence.Common.WebSiteController
{
    using System;
    using System.Collections.Generic;
    using System.Text;
    using System.Web;
    using Microsoft.SharePoint;
using Microsoft.SharePoint.ApplicationRuntime;
    using System.Diagnostics;

    /// <summary>
    /// TODO: Add comment for WebSiteModule
    /// </summary>
    public class WebSiteControllerModule : IHttpModule
    {
        /// <summary>
        /// Tracks the number of modules loaded
        /// </summary>
        private static int moduleCount;

        #region Events

        /// <summary>
        /// Occurs when [on acquire request state].
        /// </summary>
        public event EventHandler OnAcquireRequestState;

        /// <summary>
        /// Occurs when [on authenticate request].
        /// </summary>
        public event EventHandler OnAuthenticateRequest;

        /// <summary>
        /// Occurs when [on authorize request].
        /// </summary>
        public event EventHandler OnAuthorizeRequest;

        /// <summary>
        /// Occurs when [on begin request].
        /// </summary>
        public event EventHandler OnBeginRequest;

        /// <summary>
        /// Occurs when [on end request].
        /// </summary>
        public event EventHandler OnEndRequest;

        /// <summary>
        /// Occurs when [on error].
        /// </summary>
        public event EventHandler OnError;

        /// <summary>
        /// Occurs when [on post acquire request state].
        /// </summary>
        public event EventHandler OnPostAcquireRequestState;

        /// <summary>
        /// Occurs when [on post authenticate request].
        /// </summary>
        public event EventHandler OnPostAuthenticateRequest;

        /// <summary>
        /// Occurs when [on post authorize request].
        /// </summary>
        public event EventHandler OnPostAuthorizeRequest;

        /// <summary>
        /// Occurs when [on post map request handler].
        /// </summary>
        public event EventHandler OnPostMapRequestHandler;

        /// <summary>
        /// Occurs when [on post release request state].
        /// </summary>
        public event EventHandler OnPostReleaseRequestState;

        /// <summary>
        /// Occurs when [on post request handler execute].
        /// </summary>
        public event EventHandler OnPostRequestHandlerExecute;

        /// <summary>
        /// Occurs when [on post resolve request cache].
        /// </summary>
        public event EventHandler OnPostResolveRequestCache;

        /// <summary>
        /// Occurs when [on post update request cache].
        /// </summary>
        public event EventHandler OnPostUpdateRequestCache;

        /// <summary>
        /// Occurs when [on pre request handler execute].
        /// </summary>
        public event EventHandler OnPreRequestHandlerExecute;

        /// <summary>
        /// Occurs when [on pre send request content].
        /// </summary>
        public event EventHandler OnPreSendRequestContent;

        /// <summary>
        /// Occurs when [on pre send request headers].
        /// </summary>
        public event EventHandler OnPreSendRequestHeaders;

        /// <summary>
        /// Occurs when [on release request state].
        /// </summary>
        public event EventHandler OnReleaseRequestState;

        /// <summary>
        /// Occurs when [on resolve request cache].
        /// </summary>
        public event EventHandler OnResolveRequestCache;

        /// <summary>
        /// Occurs when [on update request cache].
        /// </summary>
        public event EventHandler OnUpdateRequestCache;

        #endregion

        /// <summary>
        /// Gets the current user
        /// </summary>
        /// <param name="url">The URL being accessed</param>
        /// <returns>The current SPUser</returns>
        public static SPUser GetUser(string url)
        {
            try
            {
                using (SPSite site = new SPSite(url))
                {
                    using (SPWeb web = site.OpenWeb())
                    {
                        if (web.Exists)
                        {
                            return web.CurrentUser;
                        }
                        else
                        {
                            return null;
                        }
                    }
                }
            }
            catch
            {
                return null;
            }
        }

        /// <summary>
        /// Gets the full URL of the request.
        /// </summary>
        /// <param name="context">The request context.</param>
        /// <returns>A full URL of the request</returns>
        public static Uri GetFullUrl(HttpContext context)
        {
            string currUrl = context.Request.Url.ToString();
            string basePath = currUrl.Substring(0, currUrl.IndexOf(context.Request.Url.Host) + context.Request.Url.Host.Length);
            if (context.Request.Url.Port != 80 && context.Request.Url.Port != 443)
            {
                basePath += ":" + context.Request.Url.Port.ToString();
            }

            currUrl = context.Request.RawUrl;
            currUrl = currUrl.TrimEnd(new char[1] { '/' });
            return new Uri(basePath + currUrl);
        }

        #region IHttpModule Members

        /// <summary>
        /// Disposes of the resources (other than memory) used by the module that implements <see cref="T:System.Web.IHttpModule"/>.
        /// </summary>
        public void Dispose()
        {
            this.ClearEvents();

            //WebSiteControllerConfig.Modules.Clear();
            foreach (IDisposable module in WebSiteControllerConfig.Modules)
            {

                module.Dispose();
            }

            moduleCount = WebSiteControllerConfig.Modules.Count;

        }

        /// <summary>
        /// Initializes a module and prepares it to handle requests.
        /// </summary>
        /// <param name="application">An <see cref="T:System.Web.HttpApplication"/> that provides access to the methods, properties, and events common to all application objects within an ASP.NET application</param>
        public void Init(HttpApplication application)
        {
            SPHttpApplication context = application as SPHttpApplication;
            if (context != null)
            {
                context.AcquireRequestState += new EventHandler(this.Context_AcquireRequestState);
                context.AuthenticateRequest += new EventHandler(this.Context_AuthenticateRequest);
                context.AuthorizeRequest += new EventHandler(this.Context_AuthorizeRequest);
                context.BeginRequest += new EventHandler(this.Context_BeginRequest);
                context.EndRequest += new EventHandler(this.Context_EndRequest);
                context.Error += new EventHandler(this.Context_Error);
                context.PostAcquireRequestState += new EventHandler(this.Context_PostAcquireRequestState);
                context.PostAuthenticateRequest += new EventHandler(this.Context_PostAuthenticateRequest);
                context.PostAuthorizeRequest += new EventHandler(this.Context_PostAuthorizeRequest);
                context.PostMapRequestHandler += new EventHandler(this.Context_PostMapRequestHandler);
                context.PostReleaseRequestState += new EventHandler(this.Context_PostReleaseRequestState);
                context.PostRequestHandlerExecute += new EventHandler(this.Context_PostRequestHandlerExecute);
                context.PostResolveRequestCache += new EventHandler(this.Context_PostResolveRequestCache);
                context.PostUpdateRequestCache += new EventHandler(this.Context_PostUpdateRequestCache);
                context.PreRequestHandlerExecute += new EventHandler(this.Context_PreRequestHandlerExecute);
                context.PreSendRequestContent += new EventHandler(this.Context_PreSendRequestContent);
                context.PreSendRequestHeaders += new EventHandler(this.Context_PreSendRequestHeaders);
                context.ReleaseRequestState += new EventHandler(this.Context_ReleaseRequestState);
                context.ResolveRequestCache += new EventHandler(this.Context_ResolveRequestCache);
                context.UpdateRequestCache += new EventHandler(this.Context_UpdateRequestCache);

                this.LoadModules();
            }
        }

        /// <summary>
        /// Loads the modules.
        /// </summary>
        public void LoadModules()
        {
            this.ClearEvents();

            //WebSiteControllerConfig.Modules.Clear();
            foreach (IWebSiteControllerModule module in WebSiteControllerConfig.Modules)
            {

                module.Init(this);
            }

            moduleCount = WebSiteControllerConfig.Modules.Count;
        }

        /// <summary>
        /// Dispatches the events.
        /// </summary>
        /// <param name="app">The HttpApplication.</param>
        /// <returns>true if there are pages associated with this event and event should be dispatched; otherwise false</returns>
        private static bool DispatchEvents(HttpApplication app)
        {
            /*
            Uri url = GetFullUrl(app.Context);
            SPSite site = new SPSite(url.OriginalString);
            return WebSiteControllerConfig.GetRulesForPage(site.WebApplication, url).Count > 0;
            */

            return true;
        }

        /// <summary>
        /// Clears the events.
        /// </summary>
        private void ClearEvents()
        {
            this.OnAcquireRequestState = null;
            this.OnAuthenticateRequest = null;
            this.OnBeginRequest = null;
            this.OnEndRequest = null;
            this.OnError = null;
            this.OnPostAcquireRequestState = null;
            this.OnPostAuthenticateRequest = null;
            this.OnPostAuthorizeRequest = null;
            this.OnPostMapRequestHandler = null;
            this.OnPostReleaseRequestState = null;
            this.OnPostRequestHandlerExecute = null;
            this.OnPostResolveRequestCache = null;
            this.OnPostUpdateRequestCache = null;
            this.OnPreRequestHandlerExecute = null;
            this.OnPreSendRequestContent = null;
            this.OnPreSendRequestHeaders = null;
            this.OnReleaseRequestState = null;
            this.OnResolveRequestCache = null;
            this.OnUpdateRequestCache = null;
        }

        /// <summary>
        /// Handles the UpdateRequestCache event of the context control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="System.EventArgs"/> instance containing the event data.</param>
        private void Context_UpdateRequestCache(object sender, EventArgs e)
        {
            if (this.OnUpdateRequestCache != null && DispatchEvents(sender as HttpApplication))
            {
                this.OnUpdateRequestCache(sender, e);
            }
        }

        /// <summary>
        /// Handles the ResolveRequestCache event of the context control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="System.EventArgs"/> instance containing the event data.</param>
        private void Context_ResolveRequestCache(object sender, EventArgs e)
        {
            if (this.OnResolveRequestCache != null && DispatchEvents(sender as HttpApplication))
            {
                this.OnResolveRequestCache(sender, e);
            }
        }

        /// <summary>
        /// Handles the ReleaseRequestState event of the context control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="System.EventArgs"/> instance containing the event data.</param>
        private void Context_ReleaseRequestState(object sender, EventArgs e)
        {
            if (this.OnReleaseRequestState != null && DispatchEvents(sender as HttpApplication))
            {
                this.OnReleaseRequestState(sender, e);
            }
        }

        /// <summary>
        /// Handles the PreRequestHandlerExecute event of the context control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="System.EventArgs"/> instance containing the event data.</param>
        private void Context_PreRequestHandlerExecute(object sender, EventArgs e)
        {
            if (this.OnPreRequestHandlerExecute != null && DispatchEvents(sender as HttpApplication))
            {
                this.OnPreRequestHandlerExecute(sender, e);
            }
        }

        /// <summary>
        /// Handles the PostReleaseRequestState event of the context control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="System.EventArgs"/> instance containing the event data.</param>
        private void Context_PostReleaseRequestState(object sender, EventArgs e)
        {
            if (this.OnPostReleaseRequestState != null && DispatchEvents(sender as HttpApplication))
            {
                this.OnPostReleaseRequestState(sender, e);
            }
        }

        /// <summary>
        /// Handles the PreSendRequestHeaders event of the context control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="System.EventArgs"/> instance containing the event data.</param>
        private void Context_PreSendRequestHeaders(object sender, EventArgs e)
        {
            if (this.OnPreSendRequestHeaders != null && DispatchEvents(sender as HttpApplication))
            {
                this.OnPreSendRequestHeaders(sender, e);
            }
        }

        /// <summary>
        /// Handles the PreSendRequestContent event of the context control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="System.EventArgs"/> instance containing the event data.</param>
        private void Context_PreSendRequestContent(object sender, EventArgs e)
        {
            if (this.OnPreSendRequestContent != null && DispatchEvents(sender as HttpApplication))
            {
                this.OnPreSendRequestContent(sender, e);
            }
        }

        /// <summary>
        /// Handles the PostUpdateRequestCache event of the context control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="System.EventArgs"/> instance containing the event data.</param>
        private void Context_PostUpdateRequestCache(object sender, EventArgs e)
        {
            if (this.OnPostUpdateRequestCache != null && DispatchEvents(sender as HttpApplication))
            {
                this.OnPostUpdateRequestCache(sender, e);
            }
        }

        /// <summary>
        /// Handles the PostResolveRequestCache event of the context control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="System.EventArgs"/> instance containing the event data.</param>
        private void Context_PostResolveRequestCache(object sender, EventArgs e)
        {
            if (this.OnPostResolveRequestCache != null && DispatchEvents(sender as HttpApplication))
            {
                this.OnPostResolveRequestCache(sender, e);
            }
        }

        /// <summary>
        /// Handles the PostRequestHandlerExecute event of the context control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="System.EventArgs"/> instance containing the event data.</param>
        private void Context_PostRequestHandlerExecute(object sender, EventArgs e)
        {
            if (this.OnPostRequestHandlerExecute != null && DispatchEvents(sender as HttpApplication))
            {
                this.OnPostRequestHandlerExecute(sender, e);
            }
        }

        /// <summary>
        /// Handles the PostMapRequestHandler event of the context control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="System.EventArgs"/> instance containing the event data.</param>
        private void Context_PostMapRequestHandler(object sender, EventArgs e)
        {
            if (this.OnPostMapRequestHandler != null && DispatchEvents(sender as HttpApplication))
            {
                this.OnPostMapRequestHandler(sender, e);
            }
        }

        /// <summary>
        /// Handles the PostAuthorizeRequest event of the context control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="System.EventArgs"/> instance containing the event data.</param>
        private void Context_PostAuthorizeRequest(object sender, EventArgs e)
        {
            if (this.OnPostAuthorizeRequest != null && DispatchEvents(sender as HttpApplication))
            {
                this.OnPostAuthorizeRequest(sender, e);
            }
        }

        /// <summary>
        /// Handles the PostAuthenticateRequest event of the context control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="System.EventArgs"/> instance containing the event data.</param>
        private void Context_PostAuthenticateRequest(object sender, EventArgs e)
        {
            if (this.OnPostAuthenticateRequest != null && DispatchEvents(sender as HttpApplication))
            {
                this.OnPostAuthenticateRequest(sender, e);
            }
        }

        /// <summary>
        /// Handles the PostAcquireRequestState event of the context control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="System.EventArgs"/> instance containing the event data.</param>
        private void Context_PostAcquireRequestState(object sender, EventArgs e)
        {
            if (this.OnPostAcquireRequestState != null && DispatchEvents(sender as HttpApplication))
            {
                this.OnPostAcquireRequestState(sender, e);
            }
        }

        /// <summary>
        /// Handles the Error event of the context control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="System.EventArgs"/> instance containing the event data.</param>
        private void Context_Error(object sender, EventArgs e)
        {
            if (this.OnError != null && DispatchEvents(sender as HttpApplication))
            {
                this.OnError(sender, e);
            }
        }

        /// <summary>
        /// Handles the EndRequest event of the context control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="System.EventArgs"/> instance containing the event data.</param>
        private void Context_EndRequest(object sender, EventArgs e)
        {
            if (this.OnEndRequest != null && DispatchEvents(sender as HttpApplication))
            {
                this.OnEndRequest(sender, e);
            }
            
            //Uri url = GetFullUrl((sender as HttpApplication).Context);
            //Debug.WriteLine("End:" + DateTime.Now + " : " + url.OriginalString);
        }

        /// <summary>
        /// Handles the BeginRequest event of the context control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="System.EventArgs"/> instance containing the event data.</param>
        private void Context_BeginRequest(object sender, EventArgs e)
        {
            //Uri url = GetFullUrl((sender as HttpApplication).Context);
            //Debug.WriteLine("Start:" + DateTime.Now + " : " + url.OriginalString);
            
            // Check that the modules have not changed and if they have, reload them
            // This is the first event in the event chain and guaranteed to be raised, so 
            // safe to check for this here
            if (moduleCount != WebSiteControllerConfig.Modules.Count)
            {
                this.LoadModules();
            }

            if (this.OnBeginRequest != null && DispatchEvents(sender as HttpApplication))
            {
                this.OnBeginRequest(sender, e);
            }
        }

        /// <summary>
        /// Handles the AuthorizeRequest event of the context control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="System.EventArgs"/> instance containing the event data.</param>
        private void Context_AuthorizeRequest(object sender, EventArgs e)
        {
            if (this.OnAuthorizeRequest != null && DispatchEvents(sender as HttpApplication))
            {
                this.OnAuthorizeRequest(sender, e);
            }
        }

        /// <summary>
        /// Handles the AuthenticateRequest event of the context control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="System.EventArgs"/> instance containing the event data.</param>
        private void Context_AuthenticateRequest(object sender, EventArgs e)
        {
            if (this.OnAuthenticateRequest != null && DispatchEvents(sender as HttpApplication))
            {
                this.OnAuthenticateRequest(sender, e);
            }
        }

        /// <summary>
        /// Handles the AcquireRequestState event of the context control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="System.EventArgs"/> instance containing the event data.</param>
        private void Context_AcquireRequestState(object sender, EventArgs e)
        {
            if (this.OnAcquireRequestState != null && DispatchEvents(sender as HttpApplication))
            {
                this.OnAcquireRequestState(sender, e);
            }
        }

        #endregion
    }
}

