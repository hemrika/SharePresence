﻿// -----------------------------------------------------------------------
// <copyright file="ErrorModule.cs" company="">
// TODO: Update copyright text.
// </copyright>
// -----------------------------------------------------------------------

namespace Hemrika.SharePresence.WebSite.Modules.AccesModule
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

    /// <summary>
    /// TODO: Update summary.
    /// </summary>
    public class AccesModule : IWebSiteControllerModule, IDisposable
    {
        /// <summary>
        /// The GUID to identify this module
        /// </summary>
        //private const string ID = "FE6B2E3E-2979-4016-9848-C98322E50E55";
        private Guid ID;
        private HttpApplication application;

        /// <summary>
        /// Initializes a new instance of the <see cref="ErrorModule"/> class.
        /// </summary>
        public AccesModule()
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
            get { return "~/_controltemplates/15/Hemrika/Controls/AccesModuleControl.ascx"; }
        }

        /// <summary>
        /// Attaches to appropriate request pipeline events
        /// </summary>
        /// <param name="pageControllerModule">The PageControllerModule whose event the module can attach to</param>
        public void Init(WebSiteControllerModule WebSiteControllerModule)
        {
            //WebSiteControllerModule.OnBeginRequest += new EventHandler(WebSiteControllerModule_OnBeginRequest);
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

        private void WebSiteControllerModule_OnBeginRequest(object sender, EventArgs e)
        {
            AccesModule module = new AccesModule();
            application = sender as HttpApplication;
            Uri url = WebSiteControllerModule.GetFullUrl(application.Context);

            if (WebSiteControllerConfig.IsPageControlled(url, module.RuleType))
            {
                System.Collections.Generic.List<WebSiteControllerRule> rules = WebSiteControllerConfig.GetRulesForPage(SPContext.Current.Site.WebApplication, url, module.RuleType);
                WebSiteControllerRule rule = rules[rules.Count - 1];
                string OriginalUrl = rule.Properties["OriginalUrl"].ToString();

                string httpsAdjustedUrl = rule.Url;
                if (url.ToString().StartsWith("https:", StringComparison.InvariantCultureIgnoreCase))
                {
                    httpsAdjustedUrl = httpsAdjustedUrl.Replace("http", "https");
                }

                Match match = new Regex(httpsAdjustedUrl).Match(url.ToString());
                if (match.Groups.Count > 1)
                {
                    string[] matches = new string[match.Groups.Count - 1];
                    for (int i = 1; i < match.Groups.Count; i++)
                    {
                        matches[i - 1] = match.Groups[i].Value;
                    }

                    OriginalUrl = String.Format(OriginalUrl, matches);
                }

                application.Context.RewritePath(OriginalUrl);
            }
            else
            {
                String NewRule = this.Rewrite(url);
                //WebSiteControllerConfig.AddRule()
                //Forward against the new Rule
                //this.application.Context.RewritePath(NewRule);
            }
        }

        private string Rewrite(Uri uri)
        {
            string str = null;
            string requestUrl = uri.ToString().ToLower(CultureInfo.CurrentCulture);
            string str3 = uri.Segments[uri.Segments.Length - 1];
            if ((str3.IndexOf('.') == -1) && (((((requestUrl.IndexOf("/_layouts/") == -1) && (requestUrl.IndexOf("/forms/") == -1)) && ((requestUrl.IndexOf("/lists/") == -1) && (requestUrl.IndexOf("/_controltemplates/") == -1))) && (requestUrl.IndexOf("/_vti_bin/") == -1)) && (requestUrl.IndexOf("/_wpresources/") == -1)))
            {
                using (SPSite site = new SPSite(requestUrl))
                {
                    if (requestUrl.Length == site.Url.Length)
                    {
                        requestUrl = requestUrl + "/";
                    }
                    string str4 = requestUrl.Substring(site.Url.Length + 1);
                    using (SPWeb web = site.OpenWeb(uri.AbsolutePath))
                    {
                        try
                        {
                            bool flag = string.IsNullOrEmpty(web.Url);
                        }
                        catch (FileNotFoundException)
                        {
                            string[] segments = uri.Segments;
                            if (str3.EndsWith("/", StringComparison.CurrentCulture))
                            {
                                str3 = str3.Remove(str3.Length - 1);
                            }
                            segments[segments.Length - 1] = "SitePages/";
                            str = string.Join(string.Empty, segments) + str3 + ".aspx";
                        }
                    }
                }
            }
            return str;
        }

        /// <summary>
        /// 
        /// </summary>
        public bool CanBeRemoved
        {
            get { return false; }
        }

        /// <summary>
        /// 
        /// </summary>
        public bool AlwaysRun
        {
            get { return false; }
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

        ~AccesModule()
        {
            Dispose(false);
        }

    }
}
