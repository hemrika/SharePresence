// -----------------------------------------------------------------------
// <copyright file="ErrorModule.cs" company="">
// TODO: Update copyright text.
// </copyright>
// -----------------------------------------------------------------------
//Hemrika.SharePresence.WebSite.Modules.SemanticModule.SemanticModule
//Hemrika.SharePresence.WebSite.Modules.WebPageModule.WebPageModule, Hemrika.SharePresence.WebSite, Version=1.0.0.0, Culture=neutral, PublicKeyToken=3421bd1d946bda6c
namespace Hemrika.SharePresence.WebSite.Modules.WebPageModule
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
    using System.Threading;
    using System.Collections.Specialized;
    using System.Web.Hosting;
    using Microsoft.SharePoint.ApplicationRuntime;
    using System.Web.UI.WebControls.WebParts;
    using System.Web.UI.HtmlControls;
    using Hemrika.SharePresence.WebSite.Adapters;
    using Hemrika.SharePresence.Html5.WebControls.WebParts;
    using Microsoft.SharePoint.WebControls;
    using Hemrika.SharePresence.WebSite.MetaData;
    using Hemrika.SharePresence.WebSite.Modules.WebPageModule.Filters;
    using System.Xml.Linq;
    using Microsoft.SharePoint.Administration;
    using System.IO.Compression;
    using System.Net;
    using System.Diagnostics;

    /// <summary>
    /// TODO: Update summary.
    /// </summary>
    public class WebPageModule : IWebSiteControllerModule, IDisposable
    {
        /// <summary>
        /// The GUID to identify this module
        /// </summary>
        //private const string ID = "9F789E60-8881-4BBC-9FFA-22C85680E7E5";
        private Guid ID;
        private HttpApplication application;
        private const string KEY_CULTURE = "lcid";
        private static readonly Regex contentTypeFilter = new Regex("text/*|application.json", RegexOptions.Compiled | RegexOptions.Singleline | RegexOptions.IgnoreCase);

        #region "Class Variables"
        static bool wcfProviderInitialized = false;
        static object locker = new object();
        static Dictionary<string, string> mimeMappings;
        //static object s_lockObject = new object();

        #endregion

        /// <summary>
        /// Initializes a new instance of the <see cref="LanguageModule"/> class.
        /// </summary>
        public WebPageModule()
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
            get { return "~/_controltemplates/Hemrika/Controls/WebPageModuleControl.ascx"; }
        }

        /// <summary>
        /// Attaches to appropriate request pipeline events
        /// </summary>
        /// <param name="pageControllerModule">The PageControllerModule whose event the module can attach to</param>
        public void Init(WebSiteControllerModule WebSiteControllerModule)
        {
            WebSiteControllerModule.OnPreRequestHandlerExecute += new EventHandler(WebSiteControllerModule_OnPreRequestHandlerExecute);
            WebSiteControllerModule.OnBeginRequest += new EventHandler(WebSiteControllerModule_OnBeginRequest);
            WebSiteControllerModule.OnPreSendRequestHeaders += new EventHandler(WebSiteControllerModule_OnPreSendRequestHeaders);
            WebSiteControllerModule.OnEndRequest += new EventHandler(WebSiteControllerModule_OnEndRequest);
            WebSiteControllerModule.OnPostRequestHandlerExecute += new EventHandler(WebSiteControllerModule_OnPostRequestHandlerExecute);
            WebSiteControllerModule.OnPostReleaseRequestState += new EventHandler(WebSiteControllerModule_OnPostReleaseRequestState);
        }

        void WebSiteControllerModule_OnPostReleaseRequestState(object sender, EventArgs e)
        {
            /*
            HttpContext context = ((HttpApplication)sender).Context;
            string contentType = context.Response.ContentType;

            if (context.Request.HttpMethod == "GET" && contentTypeFilter.IsMatch(contentType) && context.Response.StatusCode == 200)
            {
                context.Response.Filter = new WebPageETagFilter(context);
            }
            */
        }

        void WebSiteControllerModule_OnPostRequestHandlerExecute(object sender, EventArgs e)
        {
            HttpContext context = ((HttpApplication)sender).Context;

            if (context.Request.HttpMethod == "GET" && context.Response.ContentType == "text/html" && !context.Request.IsAuthenticated)
            {
                //context.Response.Filter = new WebPageInlineScriptFilter(context);
                //context.Response.Filter = new WebPageInlineStyleFilter(context);

                if (context.Response.StatusCode == 200 && context.CurrentHandler != null)
                {
                    context.Response.Filter = new WebPageWhiteSpaceFilter(context);
                }
            }
        }

        void WebSiteControllerModule_OnPreSendRequestHeaders(object sender, EventArgs e)
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

                string mimeType = GetMimeType(application.Context);

                if (null != mimeType)
                {
                    application.Context.Response.ContentType = mimeType;
                    application.Context.Response.Headers["Content-Type"] = mimeType;
                }
            }
            catch (Exception ex)
            {
                SPDiagnosticsService.Local.WriteTrace(0, new SPDiagnosticsCategory(ex.Source, TraceSeverity.High, EventSeverity.Error), TraceSeverity.High, ex.Message, ex.Data);
            }

            //application.Context.Response.Headers["X-Powered-By"] = "SharePresence";
            //application.Context.Response.Headers["X-SharePointHealthScore"] = "100";

            HttpContext context = HttpContext.Current;

            if (context != null)
            {
                context.Response.Headers.Remove("Server");
                context.Response.Headers.Remove("X-AspNet-Version");
                context.Response.Headers.Remove("X-SharePointHealthScore");
                context.Response.Headers.Remove("SPRequestGuid");
                context.Response.Headers.Remove("X-Powered-By");
                //context.Response.Headers.Remove("MicrosoftSharePointTeamServices");

                if (context.Response.Headers.AllKeys.Contains("X-Powered-By"))
                {
                    context.Response.Headers.Set("X-Powered-By", "SharePresence");
                }
                else
                {
                    context.Response.Headers.Add("X-Powered-By", "SharePresence");
                }

                if (context.Response.Headers.AllKeys.Contains("X-MS-InvokeApp"))
                {
                    context.Response.Headers.Set("X-MS-InvokeApp", "1; RequireReadOnly");
                }
                else
                {
                    context.Response.Headers.Add("X-MS-InvokeApp", "1; RequireReadOnly");
                }

                if (SPContext.Current != null && SPContext.Current.File != null)
                {
                    try
                    {
                        DateTime tlm = SPContext.Current.File.TimeLastModified;
                        context.Response.Cache.SetLastModified(tlm);
                    }
                    catch { }

                }
                /*
                SPContext _current = SPContext.Current;
                if (context != null && _current == null)
                {
                    try
                    {
                        if (!context.Request.FilePath.Contains("/_"))
                        {
                            _current = SPContext.GetContext(context);
                        }
                    }
                    catch { };
                }

                try
                {
                    if (_current != null && _current.File != null)
                    {
                        context.Response.Cache.SetLastModified(_current.File.TimeLastModified);
                    }
                }
                catch { };
                if (_current != null)
                {
                    _current = null;
                }
                */
                context.Response.Cache.SetCacheability(HttpCacheability.Public);
                context.Response.Cache.SetMaxAge(TimeSpan.FromSeconds(3600));
                context.Response.Cache.SetExpires(DateTime.UtcNow.AddSeconds(3600));
            }
            /*
            foreach (string header in application.Context.Response.Headers)
            {
                string sheader = header + ":" + application.Context.Response.Headers[header].ToString();
                sheader.Trim();
            }
            */
        }

        void WebSiteControllerModule_OnEndRequest(object sender, EventArgs e)
        {
            //Uri url = (sender as HttpApplication).Context.Request.Url;
            //Debug.WriteLine("End WebPage:" + DateTime.Now + " : " + url.OriginalString);

            /*
            HttpApplication application = sender as HttpApplication;

            string absolutePath = application.Request.Url.AbsolutePath.ToLower();
            if (absolutePath.Contains(".dll") ||
                absolutePath.Contains(".asmx") ||
                absolutePath.Contains(".svc") ||
                absolutePath.Contains("favicon.ico"))
            {
                return;
            }
            */
        }

        static string GetMimeType(HttpContext context)
        {
            var ext = VirtualPathUtility.GetExtension(context.Request.FilePath.ToString());
            if (string.IsNullOrEmpty(ext)) return null;

            CreateMapping(context.ApplicationInstance);

            string mimeType = null;
            mimeMappings.TryGetValue(ext, out mimeType);
            
            if (mimeType == null)
            {
                ext = ext.Trim(new char[1]{'.'});
                mimeMappings.TryGetValue(ext, out mimeType);
            }

            return mimeType;

        }

        static void CreateMapping(HttpApplication app)
        {
            if (null == mimeMappings)
            {
                lock (locker)
                {
                    if (null == mimeMappings)
                    {
                        string path = app.Server.MapPath("~/web.config");
                        XDocument doc = XDocument.Load(path);

                        var s = from v in doc.Descendants("system.webServer").Descendants("staticContent").Descendants("mimeMap")
                                select new { mimeType = v.Attribute("mimeType").Value, fileExt = v.Attribute("fileExtension").Value };

                        mimeMappings = new Dictionary<string, string>();
                        foreach (var item in s)
                        {
                            mimeMappings.Add(item.fileExt.ToString(), item.mimeType.ToString());
                        }


                        if (SPContext.Current != null)
                        {
                            foreach (SPMimeMapping mapping in SPContext.Current.Site.WebApplication.MimeMappings)
                            {
                                if (!mimeMappings.ContainsKey(mapping.Extension))
                                {
                                    mimeMappings.Add(mapping.Extension, mapping.MimeType);
                                }
                            }
                        }

                    }
                }
            }
        }


        public void Dispose() { }

        void WebSiteControllerModule_OnPostAuthorizeRequest(object sender, EventArgs e)
        {
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


            /*
            if (!HttpContext.Current.User.Identity.IsAuthenticated)
            {
                string original = HttpContext.Current.Request.FilePath;

                if (original.ToLower().Contains(".aspx"))
                {
                    int version = GetPublicVersion(HttpContext.Current.Request.Url.ToString(), original);
                    string pathInfo = HttpContext.Current.Request.PathInfo;
                    string queryString = HttpContext.Current.Request.ServerVariables["QUERY_STRING"];
                    if (String.IsNullOrEmpty(queryString) && version > 0)
                    {
                        queryString = "PageVersion=" + version.ToString();
                    }
                    //HttpContext.Current.Request.ServerVariables.Set("QUERY_STRING", queryString);
                        HttpContext.Current.RewritePath(original, pathInfo, queryString, false);
                }
            }
            */
        }

        private int GetPublicVersion(string url, string filepath)
        {
            int versionId = 0;
            //SPSecurity.RunWithElevatedPrivileges(delegate()
            //{

                using (SPSite site = new SPSite(url))
                {
                    SPWeb web = site.OpenWeb();
                    SPFile file = web.GetFile(filepath);
                    SPFileVersionCollection versions = file.Versions;

                    foreach (SPFileVersion version in versions)
                    {
                        if (version.Level == SPFileLevel.Published)// && version.IsCurrentVersion)
                        {
                            if (versionId <= version.ID)
                            {
                                versionId = version.ID;
                            }
                            //break;
                        }
                    }

                }
            //});

            return versionId;
        }

        void WebSiteControllerModule_OnPreRequestHandlerExecute(object sender, EventArgs e)
        {
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

            HttpContext context = HttpContext.Current;
            /*
            bool IsA = false;
            if (context != null && context.User != null)
            {
                IsA = context.User.Identity.IsAuthenticated;
            }
            */

                SPContext _current = SPContext.Current;
                if (context != null && _current == null)
                {
                    try
                    {
                        if (!context.Request.FilePath.Contains("/_"))
                        {
                            _current = SPContext.GetContext(context);
                        }
                    }
                    catch { };
                }

                try
                {
                    if (_current != null)
                    {
                        bool IsAuthenticated = HttpContext.Current.User.Identity.IsAuthenticated;
                        SPControlMode mode = _current.FormContext.FormMode;
                        bool isMode = (mode == SPControlMode.Display || mode == SPControlMode.Invalid);

                        if (!IsAuthenticated && isMode)
                        {
                            bool isPage = Hemrika.SharePresence.WebSite.Page.WebSitePage.IsWebSitePage(_current.ListItem);
                            bool isContentType = contentTypeFilter.IsMatch(context.Response.ContentType);
                            bool isMethod = (context.Request.HttpMethod == "GET");

                            if (isMethod && isContentType && IsCompressionSupported(context))
                            {
                                //context.Items["filter"] = context.Response.Filter;
                                //WebPageProcessor.Compress(context);
                            }


                            //context.Response.Filter = new WebPageScriptFilter(context.Response);
                            //context.Response.Filter = new WebPageViewStateFilter(context.Response);
                            //context.Response.Filter = new WebPageWhiteSpaceFilter(context.Response);
                        }
                    }
            }
            catch (Exception ex)
            {
                ex.ToString();
            }
        }

        void WebSiteControllerModule_OnBeginRequest(object sender, EventArgs e)
        {
            //Uri url = (sender as HttpApplication).Context.Request.Url;
            //Debug.WriteLine("Start WebPage:" + DateTime.Now + " : " + url.OriginalString);

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

            //application.Response.Buffer = false;
            //application.Response.BufferOutput = false;

            if (!wcfProviderInitialized)
            {
                lock (locker)
                {
                    if (!wcfProviderInitialized)
                    {

                        HostingEnvironment.RegisterVirtualPathProvider(new WebSitePathProvider());

                        wcfProviderInitialized = true;
                    }
                }
            }

            /*
            HttpContext context = application.Context;

            SPContext _current = SPContext.Current;
            if (context != null && _current == null)
            {
                _current = SPContext.GetContext(context);
            }
            */

            /*
            DateTime clientContentTimestamp;
            DateTime.TryParse(HttpContext.Current.Request.Headers["If-Modified-Since"], out clientContentTimestamp);
            DateTime lastModified = Hemrika.SharePresence.WebSite.Page.WebSitePage.GetLastModified(_current.ListItem);
            if (lastModified < clientContentTimestamp)
            {
                HttpContext.Current.Response.StatusCode = (int)HttpStatusCode.NotModified;
                HttpContext.Current.Response.StatusDescription = "Not Modified";
                HttpContext.Current.Response.End();
            }
            */
            //CreateMapping(application);

            //if (context.Request.Path == "/favicon.ico") { return; }
            /*
            if (((context != null) && (context.Request != null)) && ((context.Request.Browser != null) && (context.Request.Browser.Adapters != null)))
            {

                string fullName = typeof(WebPartZone).FullName;

                if (!context.Request.Browser.Adapters.Contains(fullName))
                {
                    context.Request.Browser.Adapters[fullName] = typeof(WebPartZoneAdapter).AssemblyQualifiedName;
                }

                fullName = typeof(WebPart).FullName;
                if (!context.Request.Browser.Adapters.Contains(fullName))
                {
                    context.Request.Browser.Adapters[fullName] = typeof(WebPartAdapter).AssemblyQualifiedName;
                }

                fullName = typeof(InputFormTextBox).FullName;
                if (!context.Request.Browser.Adapters.Contains(fullName))
                {
                    context.Request.Browser.Adapters[fullName] = typeof(InputFormTextBoxAdapter).AssemblyQualifiedName;
                }

                fullName = typeof(RichTextField).FullName;
                if (!context.Request.Browser.Adapters.Contains(fullName))
                {
                    context.Request.Browser.Adapters[fullName] = typeof(RichTextFieldAdapter).AssemblyQualifiedName;
                }

                fullName = typeof(SPRibbonCommandHandler).FullName;

                if (!context.Request.Browser.Adapters.Contains(fullName))
                {
                    context.Request.Browser.Adapters[fullName] = typeof(SPRibbonCommandHandlerAdapter).AssemblyQualifiedName;
                }

                fullName = typeof(RobotsMetaTag).FullName;

                if (!context.Request.Browser.Adapters.Contains(fullName))
                {
                    context.Request.Browser.Adapters[fullName] = typeof(MetaDataControl).AssemblyQualifiedName;
                }

                fullName = typeof(HtmlGenericControl).FullName;

                if (!context.Request.Browser.Adapters.Contains(fullName))
                {
                    context.Request.Browser.Adapters[fullName] = typeof(MicroDataAdapter).AssemblyQualifiedName;
                }

                fullName = typeof(BaseFieldControl).FullName;

                if (!context.Request.Browser.Adapters.Contains(fullName))
                {
                    context.Request.Browser.Adapters[fullName] = typeof(BaseFieldControlAdapter).AssemblyQualifiedName;
                }
            }
            */
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

        /// <summary>
        /// Check if the browser support compression
        /// </summary>
        /// <param name="context">The HTTP context.</param>
        /// <returns>A value indicating whether or not the client supports compression.</returns>
        private static bool IsCompressionSupported(HttpContext context)
        {
            if (context.Request.Browser == null)
            {
                return false;
            }

            if (context.Request.Params["SERVER_PROTOCOL"] != null && context.Request.Params["SERVER_PROTOCOL"].Contains("1.1"))
            {
                return true;
            }

            return false;
        }
    }
}
