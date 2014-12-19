// -----------------------------------------------------------------------
// <copyright file="ErrorModule.cs" company="">
// TODO: Update copyright text.
// </copyright>
// -----------------------------------------------------------------------
//Hemrika.SharePresence.WebSite.Modules.ErrorModule.ErrorModule, Hemrika.SharePresence.WebSite, Version=1.0.0.0, Culture=neutral, PublicKeyToken=11e6604a27f32a11

namespace Hemrika.SharePresence.WebSite.Modules.ErrorModule
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
    using System.Diagnostics;

    /// <summary>
    /// TODO: Update summary.
    /// </summary>
    public class ErrorModule : IWebSiteControllerModule, IDisposable
    {
        /// <summary>
        /// The GUID to identify this module
        /// </summary>
        /// 
        //private const string ID = "17A3219B-049F-4056-9566-37590122BE8E";
        private Guid ID;
        private HttpApplication application;

        /// <summary>
        /// Initializes a new instance of the <see cref="ErrorModule"/> class.
        /// </summary>
        public ErrorModule()
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
            get { return "~/_controltemplates/15/Hemrika/Controls/ErrorModuleControl.ascx"; }
        }

        /// <summary>
        /// Attaches to appropriate request pipeline events
        /// </summary>
        /// <param name="pageControllerModule">The PageControllerModule whose event the module can attach to</param>
        public void Init(WebSiteControllerModule WebSiteControllerModule)
        {
            WebSiteControllerModule.OnAuthenticateRequest += new EventHandler(WebSiteControllerModule_OnAuthenticateRequest);
            WebSiteControllerModule.OnAuthorizeRequest += new EventHandler(WebSiteControllerModule_OnAuthorizeRequest);
            WebSiteControllerModule.OnError += new EventHandler(WebSiteControllerModule_OnError);
            WebSiteControllerModule.OnEndRequest += new EventHandler(WebSiteControllerModule_OnEndRequest);
            WebSiteControllerModule.OnBeginRequest += new EventHandler(WebSiteControllerModule_OnBeginRequest);
        }

        void WebSiteControllerModule_OnAuthorizeRequest(object sender, EventArgs e)
        {
            try
            {
                application = sender as HttpApplication;

                string absolutePath = application.Request.Url.AbsolutePath.ToLower();
                if (absolutePath.Contains(".dll") ||
                    absolutePath.Contains(".asmx") ||
                    absolutePath.Contains(".svc") ||
                    absolutePath.Contains("favicon.ico"))
                {
                    return;
                }

                if (application != null)
                {
                    if (application.Response.StatusCode == (int)HttpStatusCode.Unauthorized)
                    {
                        //string result = "Unauthorized";
                    }
                }
            }
            catch { return; };
        }

        void WebSiteControllerModule_OnAuthenticateRequest(object sender, EventArgs e)
        {
            try
            {
                application = sender as HttpApplication;

                string absolutePath = application.Request.Url.AbsolutePath.ToLower();
                if (absolutePath.Contains(".dll") ||
                    absolutePath.Contains(".asmx") ||
                    absolutePath.Contains(".svc") ||
                    absolutePath.Contains("favicon.ico"))
                {
                    return;
                }

                if (application != null)
                {
                    string AccessDeniedType = string.Empty;
                    string CatchAccessDeniedName = string.Empty;
                    WebSiteControllerRule rule = null;
                    bool act = false;
                    string page = "";

                    if (application.Context.Items.Contains("CatchAccessDeniedType"))
                    {
                        AccessDeniedType = application.Context.Items["CatchAccessDeniedType"].ToString();
                    }

                    if (application.Context.Items.Contains("CatchAccessDeniedName"))
                    {
                        CatchAccessDeniedName = application.Context.Items["CatchAccessDeniedName"].ToString();
                    }

                    try
                    {
                        if (!String.IsNullOrEmpty(AccessDeniedType) && !String.IsNullOrEmpty(CatchAccessDeniedName))
                        {
                            if ((application.Context.User == null) || (!application.Context.User.Identity.IsAuthenticated))
                            {
                                rule = WebSiteControllerConfig.GetRule("ErrorCode", "403");
                            }
                        }
                    }
                    catch (Exception ex) { ex.ToString(); };

                    if (rule != null && !rule.IsDisabled)// && IsError)
                    {
                        page = rule.Properties["ErrorPage"].ToString();
                        act = true;
                    }

                    if (act)
                    {
                        HandleError(page);
                    }
                    /*
                    if (!application.Request.IsAuthenticated)
                    {
                        application.Server.ClearError();
                        application.Response.StatusCode = (int)HttpStatusCode.Forbidden;
                        throw (new HttpException((int)HttpStatusCode.Forbidden, ""));    
                    }
                    */
                    /*
                    if (application.Response.StatusCode == (int)HttpStatusCode.Forbidden)
                    {
                        string result = "Forbidden";
                    }
                    */
                }

            }
            catch { return; };
        }

        void WebSiteControllerModule_OnBeginRequest(object sender, EventArgs e)
        {
            //Uri url = (sender as HttpApplication).Context.Request.Url;
            //Debug.WriteLine("Start Error:" + DateTime.Now + " : " + url.OriginalString);

            bool blocked = false;
            try
            {
                application = sender as HttpApplication;

                string absolutePath = application.Request.Url.AbsolutePath.ToLower();
                if (absolutePath.Contains(".dll") ||
                    absolutePath.Contains(".asmx") ||
                    absolutePath.Contains(".svc") ||
                    absolutePath.Contains("favicon.ico"))
                {
                    return;
                }

                if (application != null)
                {
                    string requestUrl = application.Request.Url.OriginalString;


                    if ((((((requestUrl.ToLower().IndexOf("/_layouts/") == -1) && (requestUrl.ToLower().IndexOf("/forms/") == -1)) && ((requestUrl.ToLower().IndexOf("/lists/") == -1) && (requestUrl.ToLower().IndexOf("/_controltemplates/") == -1))) && (requestUrl.ToLower().IndexOf("/_vti_bin/") == -1)) && (requestUrl.ToLower().IndexOf("/_wpresources/") == -1)))
                    {
                        SPSite site = new SPSite(application.Request.Url.OriginalString);

                        foreach (string ext in site.WebApplication.BlockedFileExtensions)
                        {
                            bool bindex = application.Request.Url.OriginalString.EndsWith(ext);
                            if (bindex)
                            {
                                blocked = true;
                                break;
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                ex.ToString();
            }

            if (blocked)
            {
                application.Server.ClearError();
                application.Response.StatusCode = (int)HttpStatusCode.Forbidden;
                throw (new HttpException((int)HttpStatusCode.Forbidden, ""));

                //application.Context.SkipAuthorization = true;
                //application.Context.Response.Redirect("~/" + page + "?aspxerrorpath=" + application.Request.Url.AbsolutePath, false);
                //application.Context.Response.Redirect("~/" + "Error/Forbidden.aspx", true);
            }
        }

        void WebSiteControllerModule_OnEndRequest(object sender, EventArgs e)
        {
            /*
            application = sender as HttpApplication;
            string absolutePath = application.Request.Url.AbsolutePath.ToLower();
            if (absolutePath.Contains(".dll") ||
                absolutePath.Contains(".asmx") ||
                absolutePath.Contains(".svc") ||
                absolutePath.Contains("favicon.ico"))
            {
                return;
            }

            ProcessStatus(sender, e, false);
            */
            //Uri url = (sender as HttpApplication).Context.Request.Url;
            //Debug.WriteLine("End Error:" + DateTime.Now + " : " + url.OriginalString);
        }

        void WebSiteControllerModule_OnError(object sender, EventArgs e)
        {
            application = sender as HttpApplication;
            string absolutePath = application.Request.Url.AbsolutePath.ToLower();
            if (absolutePath.Contains(".dll") ||
                absolutePath.Contains(".asmx") ||
                absolutePath.Contains(".svc") ||
                absolutePath.Contains("favicon.ico"))
            {
                return;
            }

            ProcessStatus(sender, e,true);
        }


        private void ProcessStatus(object sender, EventArgs e,bool IsError)
        {
            //ErrorModule module = new ErrorModule();

            application = sender as HttpApplication;
            bool act = false;
            string page = "";
            WebSiteControllerRule rule = null;

            /*
            if (application.Context.User == null || application.Context.User.Identity.IsAuthenticated)
            {
                return;
            }
            */

            string AccessDeniedType = string.Empty;
            string CatchAccessDeniedName = string.Empty;

            if (application.Context.Items.Contains("CatchAccessDeniedType"))
            {
                AccessDeniedType  = application.Context.Items["CatchAccessDeniedType"].ToString();
            }

            if (application.Context.Items.Contains("CatchAccessDeniedName"))
            {
                CatchAccessDeniedName = application.Context.Items["CatchAccessDeniedName"].ToString();
            }

            string statuscode = "200";

            if (!String.IsNullOrEmpty(AccessDeniedType) && !String.IsNullOrEmpty(CatchAccessDeniedName))
            {
                if ((application.Context.User == null) || (!application.Context.User.Identity.IsAuthenticated))
                {
                    rule = WebSiteControllerConfig.GetRule("ErrorCode", "403");
                }
            }
            else
            {
                statuscode = application.Response.StatusCode.ToString();
            }

            try
            {
                rule = WebSiteControllerConfig.GetRule("ErrorCode", statuscode);
            }
            catch (Exception ex) { ex.ToString(); };

            if (rule != null && !rule.IsDisabled)// && IsError)
            {
                page = rule.Properties["ErrorPage"].ToString();
                act = true;
            }


            if (act)
            {
                HandleError(page);
            }
        }

        private void HandleError(string page)
        {
            application.Server.ClearError();
            application.Response.StatusCode = (int)HttpStatusCode.OK;


            SPSite site = null;
            if (SPContext.Current != null)
            {
                site = SPContext.Current.Site;
                SPWeb web = site.OpenWeb(application.Request.Url.OriginalString);
                SPList list = null;
                if (web.Exists)
                {
                    list = web.Lists.TryGetList(application.Request.Url.OriginalString);
                }

                if (list == null || !web.Exists)
                {
                    //application.Context.RewritePath("~/" + page + "?aspxerrorpath=" + application.Request.Url.AbsolutePath, false);
                    application.Context.Response.Redirect("~/" + page + "?aspxerrorpath=" + application.Request.Url.AbsolutePath, false);
                    return;
                }
            }

            try
            {
                if (application.Context.Profile != null && application.Context.Profile.IsAnonymous)
                {
                    application.Context.SkipAuthorization = true;
                    application.Context.Response.Redirect("~/" + page + "?aspxerrorpath=" + application.Request.Url.AbsolutePath, false);
                }
                else
                {
                    //int index = application.Request.Url.OriginalString.IndexOf(".aspx");
                    //bool web = application.Request.Url.OriginalString.EndsWith("/");
                    //if (index != -1)
                    //{
                    application.Context.Response.Redirect("~/" + page + "?aspxerrorpath=" + application.Request.Url.AbsolutePath, false);
                    //}
                    //else
                    //{
                    //    application.Context.RewritePath("~/" + page);
                    //}
                }
            }
            catch (Exception ex)
            {
                ex.ToString();
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
                case "ErrorPage":
                    return "ErrorPage";
                case "ErrorCode":
                    return "ErrorCode";

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

        ~ErrorModule()
        {
            Dispose(false);
        }
    }
}
