// -----------------------------------------------------------------------
// <copyright file="SemanticModule.cs" company="">
// TODO: Update copyright text.
// </copyright>
// -----------------------------------------------------------------------

namespace Hemrika.SharePresence.WebSite.Modules.SemanticModule
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using Hemrika.SharePresence.Common.WebSiteController;
    using System.Web;
    using System.Globalization;
    using Microsoft.SharePoint;
    using System.IO;
    using System.Text.RegularExpressions;
    using System.Web.UI.HtmlControls;
    using System.Web.Hosting;
    using System.Net;
    using Hemrika.SharePresence.WebSite.Adapters;
    using System.Web.UI;
    using Microsoft.SharePoint.Utilities;
    using Hemrika.SharePresence.WebSite.Modules.GateKeeper;
    using Microsoft.SharePoint.Administration;
    using System.Diagnostics;

    /// <summary>
    /// TODO: Update summary.
    /// </summary>
    public class SemanticModule : IWebSiteControllerModule, IDisposable
    {
        /// <summary>
        /// The GUID to identify this module
        /// </summary>
        //private const string ID = "386577D9-0777-4AD3-A90A-C240D8B0A49E";
        private Guid ID;
        private HttpApplication application;
        private string welcomepage;
        private bool islist = false;
        private bool isweb = false;
        private bool issubcollection = false;
        /// <summary>
        /// Initializes a new instance of the <see cref="SemanticModule"/> class.
        /// </summary>
        public SemanticModule()
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
            get { return "~/_controltemplates/Hemrika/Controls/SemanticModuleControl.ascx"; }
        }

        /// <summary>
        /// Attaches to appropriate request pipeline events
        /// </summary>
        /// <param name="pageControllerModule">The PageControllerModule whose event the module can attach to</param>
        public void Init(WebSiteControllerModule WebSiteControllerModule)
        {
            WebSiteControllerModule.OnBeginRequest += new EventHandler(WebSiteControllerModule_OnBeginRequest);
            WebSiteControllerModule.OnEndRequest += new EventHandler(WebSiteControllerModule_OnEndRequest);
        }

        void WebSiteControllerModule_OnEndRequest(object sender, EventArgs e)
        {
            //Uri url = (sender as HttpApplication).Context.Request.Url;
            //Debug.WriteLine("End Semantic:" + DateTime.Now + " : " + url.OriginalString);
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

            //Uri url = (sender as HttpApplication).Context.Request.Url;
            //Debug.WriteLine("Start Semantic:" + DateTime.Now + " : " + url.OriginalString);

            application = sender as HttpApplication;
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
                ProcessRequest(sender, e);
        }

        private void ProcessRequest(Object sender, EventArgs e)
        {

            //SemanticModule module = new SemanticModule();
            application = sender as HttpApplication;
            HttpContext context = application.Context;

            Uri url = WebSiteControllerModule.GetFullUrl(application.Context);

            if (url == null || url.AbsoluteUri.EndsWith("null"))
            {
                return;
            }
            /*
            else
            {
                if (url.DnsSafeHost == "clubcloud.kampong.nl")
                {
                    System.UriBuilder builder = new UriBuilder(url);
                    builder.Host = "kampong.clubcloud.nl";
                    string newurl = builder.ToString();

                    try
                    {
                        context.RewritePath(builder.ToString());
                    }
                    catch(Exception ex)
                    {
                        ex.ToString();
                        //context.Response.Redirect(builder.ToString()); 
                    }
                    
                }
            }
            */

            GateKeeperSettings gatekeeper = new GateKeeperSettings();
            gatekeeper = gatekeeper.Load();

            if (url.OriginalString.ToLower().Contains(gatekeeper.HoneyPotPath.ToLower()) && !string.IsNullOrEmpty(gatekeeper.HoneyPotPath))
            {
                return;
            }


            if (url.OriginalString.ToLower().Contains("_layouts") || url.OriginalString.ToLower().Contains("_login") || url.OriginalString.ToLower().Contains("_vti_bin") || url.OriginalString.ToLower().Contains("_catalogs") || url.OriginalString.ToLower().Contains("_windows") || url.OriginalString.ToLower().Contains("_forms") || url.OriginalString.ToLower().Contains("_trust"))
            {
                return;
            }


            try
            {
                bool handled = false;

                #region Controlled
                bool isControlled = false;

                using(SPSite site = new SPSite(url.OriginalString))
                {
                    CheckUrlOnZones(site, url, out url, out isControlled);
                }

                if (isControlled)
                {
                    using (SPSite site = new SPSite(url.OriginalString))
                    {

                        System.Collections.Generic.List<WebSiteControllerRule> rules = WebSiteControllerConfig.GetRulesForPage(site.WebApplication, url, this.RuleType);
                        WebSiteControllerRule rule = null;

                        foreach (WebSiteControllerRule arule in rules)
                        {
                            if (arule.RuleType == this.RuleType && arule.Properties.ContainsKey("OriginalUrl"))
                            {
                                if (arule.Url.ToLower() == url.ToString().ToLower() && !arule.IsDisabled)
                                {
                                    rule = arule;
                                    break;
                                }

                            }
                        }

                        if (rule != null)
                        {
                            //WebSiteControllerRule rule = rules[rules.Count - 1];
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


                            if (OriginalUrl.IndexOf(".aspx") >= 1)
                            {
                                if (CheckIfExists(OriginalUrl, url))
                                {
                                    application.Context.RewritePath("~/" + welcomepage);
                                }
                                else
                                {
                                    application.Server.ClearError();
                                    application.Response.StatusCode = (int)HttpStatusCode.NotFound;
                                    throw (new HttpException((int)HttpStatusCode.NotFound, ""));
                                }
                            }

                            //application.Context.RewritePath("~/" + OriginalUrl);
                            handled = true;
                        }
                        else
                        {
                            handled = false;
                        }
                    }
                }

                #endregion

                if (!handled)
                {
                    if (!application.Context.Request.Url.LocalPath.Contains("."))
                    {
                        //if (url.LocalPath.IndexOf(".aspx") >= 1)
                        //{
                        if (CheckIfExists(url.AbsolutePath, url))
                        {
                            if (islist)
                            {
                                //SPBasePermissions locked = (SPBasePermissions.ViewListItems | SPBasePermissions.Open | SPBasePermissions.ViewPages | SPBasePermissions.UseClientIntegration);
                                //SPBasePermissions permission = site.OpenWeb().AnonymousPermMask64;

                                //if ((locked != permission) || context.User.Identity.IsAuthenticated)
                                //{
                                application.Context.Response.Redirect("~/" + welcomepage, false);
                                //}
                                //else
                                //{
                                //    application.Server.ClearError();
                                //    application.Response.StatusCode = (int)HttpStatusCode.NotFound;
                                //    throw (new HttpException((int)HttpStatusCode.NotFound, ""));
                                //}
                            }
                            else
                            {
                                if (isweb)
                                {
                                    try
                                    {
                                        //application.Context.RewritePath("~/" + site.RootWeb.RootFolder.WelcomePage);

                                        //SPWeb cweb = SPContext.Current.Web;
                                        //SPFolder croot = cweb.RootFolder;
                                        //string cwelcome = croot.WelcomePage;
                                        //SPSecurity.RunWithElevatedPrivileges(delegate()
                                        //{
                                        using (SPSite site = new SPSite(url.OriginalString))
                                        {
                                            using (SPWeb web = site.OpenWeb())
                                            {

                                                SPFolder root = web.RootFolder;
                                                string welcomepage = string.Empty;
                                                if (root.Exists)
                                                {
                                                    welcomepage = root.WelcomePage;
                                                }
                                                else
                                                {
                                                    welcomepage = web.RootFolder.WelcomePage;
                                                }
                                                if (issubcollection)
                                                {
                                                    welcomepage = SPUtility.ConcatUrls(web.ServerRelativeUrl, welcomepage);
                                                }
                                                application.Context.RewritePath("~/" + welcomepage);
                                            }
                                        }
                                        //});
                                    }
                                    catch (Exception ex) 
                                    {
                                        SPDiagnosticsService.Local.WriteTrace(0, new SPDiagnosticsCategory(ex.Source, TraceSeverity.High, EventSeverity.Error), TraceSeverity.High, ex.Message, ex.Data);
                                        application.Context.RewritePath("~/" + welcomepage);
                                    }

                                }
                                else
                                {
                                    application.Context.RewritePath("~/" + welcomepage);
                                }
                            }
                        }
                        else
                        {
                            //TODO Catch SharePoint Designer requests.
                            if (SPContext.Current == null)
                            {
                                string useragent = application.Request.UserAgent;
                                if (useragent == "Microsoft Office Protocol Discovery")
                                {
                                    return;
                                }

                                application.Server.ClearError();
                                application.Response.StatusCode = (int)HttpStatusCode.NotFound;
                                throw (new HttpException((int)HttpStatusCode.NotFound, ""));
                                //return;
                            }
                            else
                            {
                                application.Server.ClearError();
                                application.Response.StatusCode = (int)HttpStatusCode.NotFound;
                                throw (new HttpException((int)HttpStatusCode.NotFound, ""));
                            }
                        }
                        //}
                    }
                    else
                    {
                        if (url.LocalPath.IndexOf(".aspx") >= 1)
                        {

                            if (!CheckIfExists(url.AbsolutePath, url))
                            {

                                application.Server.ClearError();
                                application.Response.StatusCode = (int)HttpStatusCode.NotFound;
                                throw (new HttpException((int)HttpStatusCode.NotFound, ""));
                            }
                            else
                            {
                                if ((url.AbsolutePath != welcomepage) && (url.AbsolutePath != "/" + welcomepage))
                                {
                                    application.Context.RewritePath("~/" + welcomepage, true);
                                }
                            }
                        }
                        /*
                        else
                        {
                            if (url.IsFile)
                            {
                                application.Server.ClearError();
                                application.Response.StatusCode = (int)HttpStatusCode.NotFound;
                                throw (new HttpException((int)HttpStatusCode.NotFound, ""));
                            }
                        }
                        */
                    }
                }
            }

            catch (HttpException ex)
            {
                throw (ex);
            }
            catch (Exception ex)
            {
                SPDiagnosticsService.Local.WriteTrace(0, new SPDiagnosticsCategory(ex.Source, TraceSeverity.High, EventSeverity.Error), TraceSeverity.High, ex.Message, ex.Data);
                application.Server.ClearError();
                application.Response.StatusCode = (int)HttpStatusCode.BadRequest;
                WebSiteControllerRule rule = WebSiteControllerConfig.GetRule("ErrorCode", "400");
                string page = rule.Properties["ErrorPage"].ToString();
                context.Response.Redirect("/" + page + "?aspxerrorpath=" + url, false);

            }
            finally
            {
            }
            //HttpContext context = application.Context;
            
            if (((context != null) && (context.Request != null)) && ((context.Request.Browser != null) && (context.Request.Browser.Adapters != null)))
            {
                
                string fullName = typeof(HtmlForm).FullName;
                
                if (!context.Request.Browser.Adapters.Contains(fullName))
                {
                    context.Request.Browser.Adapters[fullName] = typeof(WebSiteAdapter).AssemblyQualifiedName;
                }
                
                /*
                fullName = typeof(Page).FullName;
                if (!context.Request.Browser.Adapters.Contains(fullName))
                {
                    context.Request.Browser.Adapters[fullName] = typeof(WebSitePage).AssemblyQualifiedName;
                }
                */
            }

        }

        private void CheckUrlOnZones(SPSite site, Uri curl, out Uri url, out bool isControlled)
        {
            Uri zoneUri = curl;

            isControlled = WebSiteControllerConfig.IsPageControlled(site.WebApplication,zoneUri, this.RuleType);
            UriBuilder builder = new UriBuilder(curl);

            if (!isControlled)
            {
                try
                {
                    zoneUri = site.WebApplication.GetResponseUri(SPUrlZone.Default);
                    builder.Host = zoneUri.Host;
                    builder.Port = zoneUri.Port;
                    isControlled = WebSiteControllerConfig.IsPageControlled(site.WebApplication, builder.Uri, this.RuleType);
                    zoneUri = builder.Uri;
                }
                catch (Exception ex)
                {
                    ex.ToString();
                }
            }

            if (!isControlled)
            {
                try
                {
                    zoneUri = site.WebApplication.GetResponseUri(SPUrlZone.Internet);
                    builder.Host = zoneUri.Host;
                    builder.Port = zoneUri.Port;
                    isControlled = WebSiteControllerConfig.IsPageControlled(site.WebApplication, builder.Uri, this.RuleType);
                    zoneUri = builder.Uri;
                }
                catch (Exception ex)
                {
                    ex.ToString();
                }

            }

            if (!isControlled)
            {
                try
                {
                    zoneUri = site.WebApplication.GetResponseUri(SPUrlZone.Extranet);
                    builder.Host = zoneUri.Host;
                    builder.Port = zoneUri.Port;
                    isControlled = WebSiteControllerConfig.IsPageControlled(site.WebApplication, builder.Uri, this.RuleType);
                    zoneUri = builder.Uri;
                }
                catch (Exception ex)
                {
                    ex.ToString();
                }

            }

            if (!isControlled)
            {
                try
                {
                    zoneUri = site.WebApplication.GetResponseUri(SPUrlZone.Intranet);
                    builder.Host = zoneUri.Host;
                    builder.Port = zoneUri.Port;
                    isControlled = WebSiteControllerConfig.IsPageControlled(site.WebApplication, builder.Uri, this.RuleType);
                    zoneUri = builder.Uri;
                }
                catch (Exception ex)
                {
                    ex.ToString();
                }

            }

            if (!isControlled)
            {
                try
                {
                    zoneUri = site.WebApplication.GetResponseUri(SPUrlZone.Custom);
                    builder.Host = zoneUri.Host;
                    builder.Port = zoneUri.Port;
                    isControlled = WebSiteControllerConfig.IsPageControlled(site.WebApplication, builder.Uri, this.RuleType);
                    zoneUri = builder.Uri;
                }
                catch (Exception ex)
                {
                    ex.ToString();
                }
            }

            if (!isControlled)
            {
                Uri altZone = null;
                foreach (SPAlternateUrl altUrl in site.WebApplication.AlternateUrls)
                {

                    if (altUrl.UrlZone == site.Zone)
                    {
                        altZone = altUrl.Uri;
                        break;
                    }
                }

                if (altZone != null)
                {
                    builder.Host = zoneUri.Host;
                    builder.Port = zoneUri.Port;
                    isControlled = WebSiteControllerConfig.IsPageControlled(site.WebApplication, builder.Uri, this.RuleType);
                    zoneUri = builder.Uri;
                }
            }
            url = zoneUri;
        }

        private bool CheckIfExists(string uncheckedUrl, Uri url)
        {
            bool exits = false;

            SPSecurity.RunWithElevatedPrivileges(delegate()
            {
                using (SPSite site = new SPSite(url.OriginalString))
                {
                    SPWeb web = site.OpenWeb();
                    if (web.Exists)
                    {
                        SPFile file = null;
                        SPFolder folder = null;
                        SPList list = null;
                        welcomepage = uncheckedUrl;

                        if (web.ServerRelativeUrl == welcomepage)
                        {
                            exits = true;
                            isweb = true;
                        }

                        try
                        {
                            if (web.IsRootWeb)
                            { 
                                if (uncheckedUrl != "/")
                                {
                                    if (uncheckedUrl.StartsWith(web.ServerRelativeUrl) && web.ServerRelativeUrl != "/")
                                    {
                                        file = web.GetFile(web.RootFolder.WelcomePage);
                                        try
                                        {
                                            exits = file.Exists;
                                            issubcollection = true;
                                            isweb = true;
                                        }
                                        catch (Exception ex)
                                        {
                                            ex.ToString();
                                            exits = false;
                                            isweb = true;
                                            issubcollection = true;
                                        }

                                    }
                                    else
                                    {
                                        try
                                        {
                                            file = web.GetFile(uncheckedUrl);
                                            exits = file.Exists;
                                            isweb = !exits;
                                        }
                                        catch (Exception ex)
                                        {
                                            ex.ToString();
                                            exits = false;
                                            isweb = !exits;
                                        }
                                    }
                                }
                                else
                                {
                                    if (web.RootFolder.WelcomePage.Trim().Length > 0)
                                    {
                                        file = web.GetFile(web.RootFolder.WelcomePage);
                                    }
                                }
                            }
                            else
                            {
                                issubcollection = true;
                                string wurl = web.ServerRelativeUrl.TrimStart(new char[] { '/' });

                                wurl = uncheckedUrl.Replace(wurl, string.Empty);
                                wurl = wurl.TrimStart(new char[] { '/' });

                                if (!string.IsNullOrEmpty(wurl))
                                {
                                    file = web.GetFile(wurl);
                                }
                            }

                        }
                        catch (Exception ex) { ex.ToString(); exits = false; }

                        if ((file == null || !file.Exists) && exits == false )
                        {
                            if (url.AbsolutePath.Equals("/"))
                            {
                                folder = web.RootFolder;
                                welcomepage = folder.WelcomePage;
                                exits = true;
                            }
                            else
                            {
                                try
                                { 
                                    list = web.GetList(uncheckedUrl);
                                }
                                catch (Exception ex) { ex.ToString(); }

                                if (list != null)
                                {

                                    SPListItem item = web.GetListItem(uncheckedUrl);

                                    if (item != null && item.File != null && item.File.Exists)
                                    {
                                        exits = true;
                                        welcomepage = uncheckedUrl;
                                    }
                                    else
                                    {
                                        islist = true;
                                        if (item != null && item.ContentTypeId.IsChildOf(SPBuiltInContentTypeId.Folder))
                                        {
                                            islist = true;
                                            exits = true;
                                            //?RootFolder=%2FProducts%2FSubFolder
                                            folder = web.GetFolder(item.UniqueId);
                                            welcomepage = folder.DocumentLibrary.DefaultViewUrl + "?RootFolder=" + SPEncode.HtmlEncode(folder.Url);
                                        }
                                        else
                                        {

                                            if (uncheckedUrl.ToLowerInvariant().EndsWith(list.Title.ToLowerInvariant()) || uncheckedUrl.ToLowerInvariant().EndsWith(list.Title.ToLowerInvariant() + "/"))
                                            {
                                                exits = true;
                                                welcomepage = list.DefaultViewUrl;

                                            }
                                            else
                                            {
                                                if (uncheckedUrl.Contains("Forms"))
                                                {
                                                    exits = false;
                                                    foreach (SPView view in list.Views)
                                                    {
                                                        if ("/" + view.Url == uncheckedUrl)
                                                        {
                                                            exits = true;
                                                            break;
                                                        }
                                                    }

                                                    foreach (SPForm form in list.Forms)
                                                    {
                                                        if ("/" + form.Url == uncheckedUrl)
                                                        {
                                                            exits = true;
                                                            break;
                                                        }
                                                    }
                                                    if (exits)
                                                    {
                                                        welcomepage = url.PathAndQuery;
                                                    }
                                                }
                                                else
                                                {
                                                    exits = false;
                                                    welcomepage = uncheckedUrl;
                                                }

                                            }
                                        }
                                    }
                                }
                                else
                                {
                                    //TODO _layouts afhandelen
                                    if (uncheckedUrl.ToLower().Contains("_layouts") || uncheckedUrl.ToLower().Contains("_login") || url.OriginalString.ToLower().Contains("_windows") || url.OriginalString.ToLower().Contains("_forms") || url.OriginalString.ToLower().Contains("_trust"))
                                    {
                                        exits = true;
                                        welcomepage = url.PathAndQuery;
                                    }
                                    else
                                    {
                                        if (uncheckedUrl.Contains(".aspx") && uncheckedUrl.EndsWith("/"))
                                        {
                                            uncheckedUrl = uncheckedUrl + "/";
                                            exits = true;
                                            welcomepage = uncheckedUrl;
                                        }
                                    }
                                    /*
                                    try
                                    {
                                        folder = web.GetFolder(url.AbsoluteUri);// .RootFolder;
                                    }
                                    catch (Exception ex) { ex.ToString(); }

                                    if (folder != null)
                                    {
                                        try
                                        {
                                            file = folder.Files[url.AbsoluteUri];
                                        }
                                        catch (Exception ex) { ex.ToString(); }

                                        if (file != null && file.Exists)
                                        {
                                            welcomepage = file.Url;
                                            exits = true;
                                        }
                                        else
                                        {
                                            exits = false;
                                            if (web.Exists && list != null && !file.Exists)
                                            {
                                                exits = false;
                                            }
                                            
                                            //else
                                            //{
                                            //    welcomepage = uncheckedUrl;
                                            //    exits = true;
                                            //}
                                            
                                        }
                                    }
                                    else
                                    {
                                        welcomepage = uncheckedUrl;
                                        exits = false;
                                    }
                            */
                                    welcomepage.ToString();
                                }
                            }
                        }
                        else
                        {
                            if (file != null)
                            {
                                exits = true;
                                isweb = false;

                                if (issubcollection)
                                {
                                    welcomepage = file.ServerRelativeUrl;
                                }
                                else
                                {
                                    welcomepage = file.Url;
                                }
                            }
                        }
                    }
                    /*
                    if (!string.IsNullOrEmpty(welcomepage))
                    {
                        exits = true;
                        //welcomepage = "Default.aspx";
                    }
                    */
                }
            });

            return exits;
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
                            segments[segments.Length - 1] = "Pages/";
                            str = string.Join(string.Empty, segments) + str3 + ".aspx";
                        }
                    }
                }
            }
            return str;
        }


        public bool CanBeRemoved
        {
            get { return false; }
        }


        public bool AlwaysRun
        {
            get { return true; }
        }

        public void Dispose() { }
    }
}
