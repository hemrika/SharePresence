namespace Hemrika.SharePresence.WebSite.Page
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using Hemrika.SharePresence.Common.UI;
    using Microsoft.SharePoint.WebControls;
    using System.Web;
    using System.Web.UI.WebControls.WebParts;
    using Microsoft.SharePoint;
    using System.Security.Permissions;
    using System.Web.UI;
    using Microsoft.SharePoint.WebPartPages;
    using Hemrika.SharePresence.WebSite.ContentTypes;
    using Hemrika.SharePresence.WebSite.Page.Handlers;
    using System.Reflection;
    using Microsoft.SharePoint.Utilities;
    using System.Threading;
    using System.Globalization;
    using System.Web.UI.HtmlControls;
    using Hemrika.SharePresence.WebSite.Editor;
    using System.Collections;
    using System.Web.UI.WebControls;
    using Microsoft.SharePoint.Administration;

    [PermissionSet(SecurityAction.Demand, Name = "FullTrust")]
    public class WebSitePage : EnhancedWebPartPage
    {
        private string masterpage;
        private new string AppRelativeVirtualPath;
        protected WebPageStateControl wpsc;

        public WebSitePage()
        {
        }

        protected void Page_PreInit(object sender, EventArgs e)
        {
            AppRelativeVirtualPath = this.Page.Master.AppRelativeVirtualPath;
            masterpage = this.Page.MasterPageFile;
        }

        protected override void OnInit(EventArgs e)
        {
            if (SPContext.Current.FormContext.FormMode == SPControlMode.Edit)
            {
                base.OnInit(e);
            }
        }

        /*
        protected void Page_Load(object sender, EventArgs e)
        {
            try
            {
                HtmlGenericControl body = this.Master.FindControl("body") as HtmlGenericControl;
                if (body != null)
                {
                    body.Attributes.Add("onload", "if (typeof(_spBodyOnLoadWrapper) != 'undefined') _spBodyOnLoadWrapper();");
                }
            }
            catch { };
        }
        */

        protected override void OnLoad(EventArgs e)
        {
            base.OnLoad(e);
            //EnsureChildControls();
            SPRibbon current = SPRibbon.GetCurrent(this.Page);

            if ((current != null) && HttpContext.Current.User.Identity.IsAuthenticated)
            {
                if (wpsc == null)
                {
                    wpsc = new WebPageStateControl();
                    this.Page.Controls.Add(wpsc);
                }

                WebPageSetHomePageHandler homepage = new WebPageSetHomePageHandler(wpsc);
                this.Controls.Add(homepage);

                ContentRibbon content = new ContentRibbon(wpsc);
                this.Controls.Add(content);

                DesignRibbon design = new DesignRibbon(wpsc);
                this.Controls.Add(design);

                CssRegistration.Register("/_layouts/Hemrika/WebSitePage/Hemrika.SharePresence.WebSite.css");

                TransformDialog();
                var manager = new SPRibbonScriptManager();
                var methodInfo = typeof(SPRibbonScriptManager).GetMethod("RegisterInitializeFunction", BindingFlags.Instance | BindingFlags.NonPublic);
                methodInfo.Invoke(manager, new object[] { Page, "InitPageComponent", "/_layouts/Hemrika/WebSitePage/Hemrika.SharePresence.WebSite.Page.js", false, "Hemrika.SharePresence.WebSite.Page.PageComponent.initialize()" });

                current.CommandUIVisible = true;
                current.CheckForInitializationReadiness = true;
                current.MakeTabAvailable("Ribbon.Read", "WSSPageStateVisibilityContext");
                current.MakeTabAvailable("Ribbon.EditingTools", "WSSPageStateVisibilityContext");
                current.MakeTabAvailable("Ribbon.Hemrika.SharePresence.Page", "WSSPageStateVisibilityContext");
                current.MakeTabAvailable("Ribbon.Hemrika.SharePresence.Content", "WSSPageStateVisibilityContext");
                current.MakeTabAvailable("Ribbon.Hemrika.SharePresence.Design", "WSSPageStateVisibilityContext");
                current.MakeTabAvailable("Ribbon.Hemrika.SharePresence.SEO", "WSSPageStateVisibilityContext");
                current.MakeTabAvailable("Ribbon.Hemrika.SharePresence.Analytics", "WSSPageStateVisibilityContext");
                current.MakeTabAvailable("Ribbon.Hemrika.SharePresence.Settings", "WSSPageStateVisibilityContext");
                current.EnableVisibilityContext("WSSPageStateVisibilityContext");
                //current.DisableVisibilityContext("WSSWebPartPage");
                //Allow Browse button for visibility
                //current.TrimById(SPRibbon.ReadTabId);
                current.TrimById("Ribbon.WebPartPage", "WSSWebPartPage");
                current.TrimById("Ribbon.WikiPageTab", "WSSPageStateVisibilityContext");
                current.TrimById("Ribbon.EditingTools.CPEditTab.Layout.PageLayout", "WSSPageStateVisibilityContext");
                current.TrimById("Ribbon.WebPartInsert", "WSSPageStateVisibilityContext");
                current.TrimById("Ribbon.WebPartInsert.Tab", "WSSPageStateVisibilityContext");

                current.CommandUIVisible = true;
                //RegisterWebSitePage(this);

                //AlohaEditor(this);
            }
            else
            {
                EnsureChildControls();

                ScriptManager manager = ScriptManager.GetCurrent(Page);
                manager.LoadScriptsBeforeUI = false;
                manager.AllowCustomErrorsRedirect = true;
                manager.ScriptMode = ScriptMode.Release;
                //Remove WPSC script block
                this.Page.ClientScript.RegisterStartupScript(typeof(SPWebPartManager), "WPSCScriptBlock", string.Empty);

            }

            //BlogConnector();
        }

        private static void AlohaEditor(Page page)
        {
            if (SPContext.Current.FormContext.FormMode == SPControlMode.Edit)
            {
                CssRegistration.Register("/_layouts/Hemrika/Editor/css/aloha.css");
                CssRegistration.Register("http://code.jquery.com/ui/1.9.2/themes/base/jquery-ui.css");
                if (!page.ClientScript.IsClientScriptIncludeRegistered("HTML5Editor"))
                {
                    /*
                    HtmlGenericControl jqueryUi = new HtmlGenericControl("script");
                    jqueryUi.Attributes["type"] = "text/javascript";
                    jqueryUi.Attributes["src"] = "http://code.jquery.com/ui/1.9.2/jquery-ui.min.js";

                    page.Header.Controls.Add(jqueryUi);
                    */
                    
                    HtmlGenericControl require = new HtmlGenericControl("script");
                    require.Attributes["type"] = "text/javascript";
                    require.Attributes["src"] = "/_layouts/Hemrika/Editor/lib/require.js";
                    //require.Attributes["async"]
                    //require.Attributes["defer"]
                    page.Header.Controls.Add(require);
                    
                    HtmlGenericControl aloha = new HtmlGenericControl("script");
                    aloha.Attributes["type"] = "text/javascript";
                    aloha.Attributes["src"] = "/_layouts/Hemrika/Editor/lib/aloha.js";
                    EditorSettings settings = new EditorSettings();
                    settings = settings.Load();

                    StringBuilder plugins = new StringBuilder();
                    
                    foreach (DictionaryEntry entry in settings.Common)
                    {
                        bool used = bool.Parse(entry.Value.ToString());
                        if (used)
                        {
                            plugins.Append("common/" + entry.Key.ToString().ToLower() + ",");
                        }
                    }

                    foreach (DictionaryEntry entry in settings.Extra)
                    {
                        bool used = bool.Parse(entry.Value.ToString());
                        if (used)
                        {
                            plugins.Append("extra/" + entry.Key.ToString().ToLower() + ",");
                        }
                    }

                    if (plugins.Length > 0)
                    {
                        plugins = plugins.Remove(plugins.Length - 1, 1);

                        //aloha.Attributes["data-aloha-plugins"] = "common/ui,common/format,common/highlighteditables,common/link";
                        aloha.Attributes["data-aloha-plugins"] = plugins.ToString();// "common/ui,common/format,common/table,common/list,common/link,common/summery,common/highlighteditables,common/undo,common/contenthandler,common/paste,common/characterpicker,common/commands,common/block,common/horizontalruler,common/align,common/dom-to-xhtml,extra/formatlesspaste,extra/toc,extra/headerids,extra/listenforcer,extra/metaview,extra/linkbrowser,extra/cite,extra/htmlsource";
                        page.Header.Controls.Add(aloha);
                        //Page.ClientScript.RegisterClientScriptInclude(typeof(EnhancedLayoutsPage), "HTML5Editor", "");

                        HtmlGenericControl config = new HtmlGenericControl("script");
                        config.Attributes["type"] = "text/javascript";
                        config.Attributes["src"] = "/_layouts/Hemrika/Content/AlohaConfiguration.aspx";

                        page.Header.Controls.Add(config);

                        ScriptManager.RegisterOnSubmitStatement(page, page.GetType(), "HTML5Editor", "HTML5Editor();");
                    }
                }

                
            }
        }

        protected override void OnPreRenderComplete(EventArgs e)
        {
            base.OnPreRenderComplete(e);
        }

        private void TransformDialog()
        {
            if (IsDialogMode)
            {
                CssRegistration.Register("/_layouts/Hemrika/Content/css/metro/jquery.mobile.metro.theme.css");
                CssRegistration.Register("/_layouts/Hemrika/Content/css/jquery.mobile.fixedToolbar.polyfill.css");
                CssRegistration.Register("/_layouts/Hemrika/Content/css/mibiscroll.css");
                CssRegistration.Register("/_layouts/Hemrika/Content/css/progess-bar.css");
                CssRegistration.Register("/_layouts/Hemrika/Content/css/toggle-button.css");

                if (!Page.ClientScript.IsClientScriptIncludeRegistered("JQuery"))
                {
                    Page.ClientScript.RegisterClientScriptInclude(typeof(EnhancedLayoutsPage), "JQuery", "/_layouts/Hemrika/Content/js/jquery.js");
                }

                Page.ClientScript.RegisterClientScriptBlock(typeof(EnhancedLayoutsPage), "MobileInit", "$(document).bind(\"mobileinit\", function() {   $.extend(  $.mobile , {       ajaxFormsEnabled: true,autoInitializePage: true   }); }); ", true);

                if (!Page.ClientScript.IsClientScriptIncludeRegistered("Mobile"))
                {
                    Page.ClientScript.RegisterClientScriptInclude(typeof(EnhancedLayoutsPage), "Mobile", "/_layouts/Hemrika/Content/js/jquery.mobile.js");
                }

                if (!Page.ClientScript.IsClientScriptIncludeRegistered("Metro"))
                {
                    Page.ClientScript.RegisterClientScriptInclude(typeof(EnhancedLayoutsPage), "Mertro", "/_layouts/Hemrika/Content/css/metro/jquery.mobile.metro.theme.init.js");
                }

                if (!Page.ClientScript.IsClientScriptIncludeRegistered("GlobalCSS"))
                {
                    Page.ClientScript.RegisterClientScriptInclude(typeof(EnhancedLayoutsPage), "GlobalCSS", "/_layouts/Hemrika/Content/js/jquery.globalstylesheet.js");
                }

                if (!Page.ClientScript.IsClientScriptIncludeRegistered("Easing"))
                {
                    Page.ClientScript.RegisterClientScriptInclude(typeof(EnhancedLayoutsPage), "Easing", "/_layouts/Hemrika/Content/js/jquery.Easing.js");
                }

                if (!Page.ClientScript.IsClientScriptIncludeRegistered("MobiScroll"))
                {
                    Page.ClientScript.RegisterClientScriptInclude(typeof(EnhancedLayoutsPage), "MobiScroll", "/_layouts/Hemrika/Content/js/mobiscroll.min.js");
                }

                if (!Page.ClientScript.IsClientScriptIncludeRegistered("ProgessBar"))
                {
                    Page.ClientScript.RegisterClientScriptInclude(typeof(EnhancedLayoutsPage), "ProgessBar", "/_layouts/Hemrika/Content/js/progress-bar.js");
                }

                if (!Page.ClientScript.IsClientScriptIncludeRegistered("ToggleButton"))
                {
                    Page.ClientScript.RegisterClientScriptInclude(typeof(EnhancedLayoutsPage), "ToggleButton", "/_layouts/Hemrika/Content/js/toggle-button.js");
                }

                /*
                if (!IsPostBack)
                {
                    ScriptManager.RegisterStartupScript(Page, GetType(), "temp", "<script type='text/javascript'>$('div').trigger('create');</script>", false);
                }
                */
            }
        }

        private void AnalyticsConnector()
        {
            string _reportpath = @"~/_controltemplates/15/Hemrika/Analytics/GoogleConnector.ascx";

            try
            {
                Control report = Page.LoadControl(_reportpath);
                this.Controls.Add(report);
            }
            catch (Exception ex)
            {
                ex.ToString();
            }
        }

        private void BlogConnector()
        {
            string _reportpath = @"~/_controltemplates/15/Hemrika/Blog/BlogConnector.ascx";

            try
            {
                Control report = Page.LoadControl(_reportpath);
                this.Controls.Add(report);
            }
            catch (Exception ex)
            {
                ex.ToString();
            }
        }

        private static void RegisterWebSitePage(Page page)
        {
            if (((SPContext.Current != null) && !page.ClientScript.IsStartupScriptRegistered(typeof(ScriptLink), "websitepagecontextinfo")))
            {
                SPWeb web = SPContext.Current.Web;
                SPSite site = SPContext.Current.Site;
                StringBuilder builder = new StringBuilder();
                if (web != null)
                {
                    //builder.Append("var _fV4UI=");
                    //builder.Append((web.UIVersion > 3) ? "true;" : "false;");
                    builder.Append("var _WebSitePageContextInfo = {");
                    builder.AppendFormat("wspListId:\"{0}\"", SPContext.Current.ListId.ToString("B"));
                    builder.AppendFormat(",wspListUrl:\"{0}\"", SPContext.Current.List.RootFolder.Url.ToString());
                    builder.AppendFormat(",wspItemId:{0}", SPContext.Current.ListItem.ID.ToString());
                    builder.AppendFormat(",wspItemUId:\"{0}\"", SPContext.Current.ListItem.UniqueId.ToString("B"));
                    builder.AppendFormat(",wspUrl:\"{0}\"", SPContext.Current.ListItem.File.ServerRelativeUrl);
                    builder.AppendFormat(",wspHomePage:\"{0}\"", SPContext.Current.ContextPageInfo.IsWebWelcomePage);
                    if (SPContext.Current.File.RequiresCheckout && SPContext.Current.File.CheckedOutByUser != null && SPContext.Current.File.CheckedOutByUser.ID == SPContext.Current.Web.CurrentUser.ID)
                    {
                        builder.AppendFormat(",wspCheckedOut:\"{0}\"", SPFile.SPCheckOutType.Online);
                    }
                    else
                    {
                        if (!SPContext.Current.File.RequiresCheckout)
                        {
                            builder.AppendFormat(",wspCheckedOut:\"{0}\"", SPFile.SPCheckOutType.Online);
                        }
                        else
                        {
                            builder.AppendFormat(",wspCheckedOut:\"{0}\"", SPFile.SPCheckOutType.None);
                        }
                    }
                    
                    builder.AppendFormat(",wspFormMode:\"{0}\"", SPContext.Current.FormContext.FormMode.ToString());
                    builder.Append("};");
                }
                ScriptManager.RegisterClientScriptBlock(page, typeof(ScriptLink), "websitepagecontextinfo", builder.ToString(), true);
            }
        }

        protected override void OnLoadComplete(EventArgs e)
        {
            
            RegisterWebSitePage(this);
            AlohaEditor(this);
            base.OnLoadComplete(e);
            //EnsureChildControls();
        }
        
        internal static DisplayControlModes DetermineDisplayControlModes()
        {
            DisplayControlModes modes = new DisplayControlModes();
            modes.displayMode = SPControlMode.Display;
            modes.controlMode = SPControlMode.Display;
            if (HttpContext.Current.Request.Form["MSOLayout_InDesignMode"] == "1")
            {
                modes.displayMode = SPControlMode.Edit;
            }
            else
            {
                string str = HttpContext.Current.Request.QueryString["DisplayMode"];
                if (!string.IsNullOrEmpty(str) && (string.Compare(str, "DESIGN", StringComparison.OrdinalIgnoreCase) == 0))
                {
                    modes.displayMode = SPControlMode.Edit;
                }
            }
            if (GetPersonalizationScopeFromCurrentPage() != PersonalizationScope.User)
            {
                HttpRequest request = HttpContext.Current.Request;
                modes.controlMode = (request.Form.Get("MSOAuthoringConsole_FormContext") == "1") ? SPControlMode.Edit : SPControlMode.Display;
                if ((modes.controlMode == SPControlMode.Display) && (request.QueryString.Get("ControlMode") == "Edit"))
                {
                    modes.controlMode = SPControlMode.Edit;
                }
            }
            try
            {
                SPContext.Current.FormContext.FormMode = modes.controlMode;
            }
            catch { };
            return modes;
        }

        private static PersonalizationScope GetPersonalizationScopeFromCurrentPage()
        {
            switch (HttpContext.Current.Request.Form.Get("MSOWebPartPage_Shared"))
            {
                case "0":
                    return PersonalizationScope.User;

                case "1":
                    return PersonalizationScope.Shared;
            }
            return PersonalizationScope.Shared;
        }

        protected override void OnPreInit(EventArgs e)
        {
            base.OnPreInit(e);
            SetContextProperties();
            //SPContext current = SPContext.Current;
            if (SPContext.Current == null)
            {
                SPException exception = new SPException("invalid SharePoint Context.");
                throw exception;
            }

            /*
            SPWeb web = current.Web;
            this.MasterPageFile = web.CustomMasterUrl;
            */
        }

        internal static void SetContextProperties()
        {
            if (string.IsNullOrEmpty(HttpContext.Current.Items["ContextPropertiesSet"] as string))
            {
                HttpContext.Current.Items["ContextPropertiesSet"] = "true";
                SPContext.Current.UseDefaultCachePolicy = true;
            }
        }

        public static bool IsWebSitePage(SPListItem websitepage)
        {
            try
            {
                if (websitepage == null)
                {
                    return false;
                }

                if (websitepage.ContentTypeId == ContentTypeId.PageTemplate)
                {
                    return true;
                }

                if (websitepage.ContentTypeId.IsChildOf(ContentTypeId.PageTemplate))
                {
                    return true;
                }
            }
            catch (Exception ex)
            {
                SPDiagnosticsService.Local.WriteTrace(0, new SPDiagnosticsCategory(ex.Source, TraceSeverity.High, EventSeverity.Error), TraceSeverity.High, ex.Message, ex.Data);
                return false;
            }

            return false;
        }

        public static bool IsBlogPage(SPListItem websitepage)
        {
            try
            {
                if (websitepage.ContentTypeId == ContentTypeId.BlogPage)
                {
                    return true;
                }

                if (websitepage.ContentTypeId.IsChildOf(ContentTypeId.BlogPage))
                {
                    return true;
                }
            }
            catch (Exception ex)
            {
                SPDiagnosticsService.Local.WriteTrace(0, new SPDiagnosticsCategory(ex.Source, TraceSeverity.High, EventSeverity.Error), TraceSeverity.High, ex.Message, ex.Data);
                return false;
            }

            return false;
        }

        public static bool IsVideoPage(SPListItem websitepage)
        {
            try
            {
                if (websitepage.ContentTypeId == ContentTypeId.VideoPage)
                {
                    return true;
                }

                if (websitepage.ContentTypeId.IsChildOf(ContentTypeId.VideoPage))
                {
                    return true;
                }
            }
            catch (Exception ex)
            {
                SPDiagnosticsService.Local.WriteTrace(0, new SPDiagnosticsCategory(ex.Source, TraceSeverity.High, EventSeverity.Error), TraceSeverity.High, ex.Message, ex.Data);
                return false;
            }

            return false;
        }

        public static bool IsPressRelease(SPListItem websitepage)
        {
            try
            {
                if (websitepage.ContentTypeId == ContentTypeId.PressRelease)
                {
                    return true;
                }

                if (websitepage.ContentTypeId.IsChildOf(ContentTypeId.PressRelease))
                {
                    return true;
                }
            }
            catch (Exception ex)
            {
                SPDiagnosticsService.Local.WriteTrace(0, new SPDiagnosticsCategory(ex.Source, TraceSeverity.High, EventSeverity.Error), TraceSeverity.High, ex.Message, ex.Data);
                return false;
            }

            return false;
        }

        public static DateTime GetLastModified(SPListItem websitepage)
        {
            DateTime modified = DateTime.Now;
            try
            {
                if (IsWebSitePage(websitepage))
                {
                    SPFile file = null;
                    try
                    {
                        file = websitepage.File;
                        modified = file.TimeLastModified;
                    }
                    finally
                    {
                        file = null;
                    }
                }
            }
            catch (Exception ex)
            {
                SPDiagnosticsService.Local.WriteTrace(0, new SPDiagnosticsCategory(ex.Source, TraceSeverity.High, EventSeverity.Error), TraceSeverity.High, ex.Message, ex.Data);
                return DateTime.Now;
            }

            return modified;
        }

        public override Common.Ribbon.Definitions.TabDefinition GetTabDefinition()
        {
            return null;
        }
    }
}
