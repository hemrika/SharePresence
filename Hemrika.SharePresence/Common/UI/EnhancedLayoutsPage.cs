using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.SharePoint.WebControls;
using Hemrika.SharePresence.Common.Ribbon.Definitions;
using Hemrika.SharePresence.Common.Ribbon;
using System.ComponentModel;
using System.Reflection;
using Hemrika.SharePresence.Common.License;
using System.Web.UI;
using Microsoft.SharePoint.Utilities;
using System.Globalization;
using Microsoft.SharePoint;
using Microsoft.SharePoint.Administration;
using Hemrika.SharePresence.SPLicense.LicenseFile;
using System.Web.UI.WebControls;

namespace Hemrika.SharePresence.Common.UI
{
    /// <summary>
    /// <para>
    /// This base class simplifies creation of Application Page with custom ribbon tab.
    /// </para>
    /// <para>
    /// You need to inherit from this class rather when from LayoutsPageBase, to use the functionality.
    /// Also, you should override the GetTabDefinition method and provide ribbon tab definition, using <see cref="TabDefinition"/> class.
    /// </para>
    /// </summary>
    public abstract class EnhancedLayoutsPage : LayoutsPageBase
    {
        internal static SPLicenseFile _license;
        internal String error = String.Empty;
        internal Type _licenseType;

        public EnhancedLayoutsPage()
        {
            
            try
            {
                if (_license == null)
                {
                    _licenseType = SetTypeForLicense();
                    LicenseManager.Validate(_licenseType);
                    System.ComponentModel.License license;
                    LicenseManager.IsValid(_licenseType, this, out license);
                    _license = (SPLicenseFile)license;
                }
            }
            catch (Exception ex)
            {
                error = ex.ToString();
            }
        }
        
        #region License
        
        public bool IsLicensed
        {
            get
            {
                return _license.Product.IsLicensed;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public abstract Type SetTypeForLicense();

        #endregion

        protected override void OnPreInit(EventArgs e)
        {
            base.OnPreInit(e);
            /*
            ScriptManager manager = ScriptManager.GetCurrent(Page);//.Dispose();
            if (manager != null)
            {
                manager.ScriptMode = ScriptMode.Release;
                manager.EnablePageMethods = false;
                manager.EnablePartialRendering = false;
            }
            */
        }
        /// <summary>
        /// 
        /// </summary>
        protected override void CreateChildControls()
        {
            /*
            if (IsDialogMode)
            {
                ScriptManager.GetCurrent(Page).Dispose();
            }
            */
            
            //if (IsDialogMode || IsPopUI)
            //{
                CssRegistration.Register("/_layouts/Hemrika/WebSitePage/Hemrika.SharePoint.WebSite.css");
                CssRegistration.Register("/_layouts/Hemrika/Content/css/metro/jquery.mobile.metro.theme.css");
                CssRegistration.Register("/_layouts/Hemrika/Content/css/jquery.mobile.fixedToolbar.polyfill.css");
                CssRegistration.Register("/_layouts/Hemrika/Content/css/mibiscroll.css");
                CssRegistration.Register("/_layouts/Hemrika/Content/css/progess-bar.css");
                CssRegistration.Register("/_layouts/Hemrika/Content/css/toggle-button.css");

                if (!Page.ClientScript.IsClientScriptIncludeRegistered("JQuery"))
                {
                    Page.ClientScript.RegisterClientScriptInclude(typeof(EnhancedLayoutsPage), "JQuery", "/_layouts/Hemrika/Content/js/jquery.js");
                }

                Page.ClientScript.RegisterClientScriptBlock(typeof(EnhancedLayoutsPage), "MobileInit", "$(document).bind(\"mobileinit\", function() {   $.extend(  $.mobile , {       ajaxFormsEnabled: false, autoInitializePage: true   }); }); ", true);

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
            //}

            /*
            if (IsDialogMode)
            {
                ScriptManager.GetCurrent(Page).Dispose();
            }
            */

            base.CreateChildControls();

            /*
            EnsureChildControls();
            if (_license != null && !_license.Product.IsLicensed)
            {

                foreach (Control control in Controls)
                {
                    DisableEvent(control);
                }
            }
            */
        }

        #region Mobilefy controls

        private void DisableEvent(Control current)
        {
            foreach (Control control in current.Controls)
            {
                if (control.GetType() == typeof(Button) || control.GetType() == typeof(ImageButton))
                {
                    if (control.ID != "btn_Cancel")
                    {
                        if (IsPostBack)
                        {

                            if (Context.Session != null && Session["update" + control.ID].ToString() != ViewState["update" + control.ID].ToString())
                            {
                                RemoveClickEvent((Button)control);
                            }
                            else
                            {
                                RemoveClickEvent((Button)control);
                                ((Button)control).Click += new EventHandler(Button_Disable);
                            }
                        }
                        else
                        {
                            if (this.Context.Session != null)
                            {
                                Session["update" + control.ID] = Server.UrlEncode(System.DateTime.Now.ToString());
                            }
                            else
                            {
                                RemoveClickEvent((Button)control);
                                ((Button)control).Click += new EventHandler(Button_Disable);
                            }
                        }
                    }
                }
                DisableEvent(control);
            }
        }

        protected void RemoveClickEvent(Button b)
        {
            System.Reflection.FieldInfo f1 = typeof(Button).GetField("EventClick", System.Reflection.BindingFlags.Static | System.Reflection.BindingFlags.NonPublic);
            object obj = f1.GetValue(b);
            System.Reflection.PropertyInfo pi = typeof(Button).GetProperty("Events", System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance);
            System.ComponentModel.EventHandlerList list = (System.ComponentModel.EventHandlerList)pi.GetValue(b, null);
            list.RemoveHandler(obj, list[obj]);
        }

        protected void Button_Disable(object sender, EventArgs e)
        {
            Button b = (Button)sender;
            if (this.Context.Session != null)
            {
                Session["update" + b.ID] = Server.UrlEncode(System.DateTime.Now.ToString());
            }
        }
        #endregion


        #region Dialog Response
        
        /// <summary>
        /// URL of the page to redirect to when not in Dialog mode.
        /// </summary>
        public string PageToRedirectOnOK { get; set; }
        /// <summary>
        /// Returns true if the Application Page is displayed in Modal Dialog.
        /// </summary>
        protected bool IsPopUI
        {
            get
            {
                return !String.IsNullOrEmpty(base.Request.QueryString["IsDlg"]);
            }
        }

        /// <summary>
        /// Call after completing custom logic in the Application Page.
        /// Returns the OK response.
        /// </summary>
        public void EndOperation()
        {
            EndOperation(ModalDialogResult.OK);
        }

        /// <summary>
        /// Call after completing custom logic in the Application Page.
        /// </summary>
        /// <param name="result">Result code to pass to the output. Available results: -1 = invalid; 0 = cancel; 1 = OK</param>
        public void EndOperation(ModalDialogResult result)
        {
            EndOperation(result, PageToRedirectOnOK);
        }

        /// <summary>
        /// Call after completing custom logic in the Application Page.
        /// </summary>
        /// <param name="result">Result code to pass to the output. Available results: -1 = invalid; 0 = cancel; 1 = OK</param>
        /// <param name="returnValue">Value to pass to the callback method defined when opening the Modal Dialog.</param>
        public void EndOperation(ModalDialogResult result, string returnValue)
        {
                if (IsPopUI)
                {
                    if (string.IsNullOrEmpty(PageToRedirectOnOK))
                    {
                        Page.Response.Clear();
                        Page.Response.Write(String.Format(CultureInfo.InvariantCulture, "<script type=\"text/javascript\">window.frameElement.commonModalDialogClose({0}, {1});</script>",
                            new object[] { 
                        (int)result, 
                        String.IsNullOrEmpty(returnValue) ? "null" :  returnValue
                    }));
                        Page.Response.End();

                    }
                    else
                    {
                        ScriptManager.RegisterStartupScript((Page)this, typeof(EnhancedLayoutsPage), "RedirectToCreatedPage", "window.frameElement.navigateParent('" + SPHttpUtility.EcmaScriptStringLiteralEncode(SPHttpUtility.UrlPathEncode(PageToRedirectOnOK, false)) + "');", true);
                    }
                }
                else
                {
                    RedirectOnOK();
                }
        }

        /// <summary>
        /// Redirects to the URL specified in the PageToRedirectOnOK property.
        /// </summary>
        private void RedirectOnOK()
        {
            SPUtility.Redirect(PageToRedirectOnOK ?? SPContext.Current.Web.Url, SPRedirectFlags.UseSource, Context);
        }

        #endregion

        #region Page Notification
        
        public void AddSharePointNotification(Page page, string text)
        {
            //build up javascript to inject at the tail end of the page 
            StringBuilder stringBuilder = new StringBuilder();

            stringBuilder.AppendLine("<script>");

            //First wait until the SP.js is loaded, otherwise the notification doesn’t work 
            //gets an null reference exception 
            stringBuilder.AppendLine("ExecuteOrDelayUntilScriptLoaded(ShowNotification, \"sp.js\");");

            stringBuilder.AppendLine("function ShowNotification()");
            stringBuilder.AppendLine("{");
            stringBuilder.AppendLine(string.Format("SP.UI.Notify.addNotification(\"{0}\");", text));
            stringBuilder.AppendLine("}");

            stringBuilder.AppendLine("</script>");

            //add to the page 
            page.Controls.Add(new LiteralControl(stringBuilder.ToString()));

        }

        #endregion

        #region Tabs

        /// <summary>
        /// Provide ribbon tab definition.
        /// </summary>
        /// <returns>
        /// If you return null here, tab will not be shown.
        /// Overwise, the ribbon tab is created and activated when the page is displayed.
        /// </returns>
        public abstract TabDefinition GetTabDefinition();

        public virtual bool DisplayTab
        {
            get
            {
                return true;
            }

        }

        /// <summary>
        /// Adding ribbon tab to page here
        /// </summary>
        protected override void OnPreRender(EventArgs e)
        {
            base.OnPreRender(e);

            if (!DisplayTab)
                return;

            var tabDefinition = GetTabDefinition();
            try
            {
                if (SPRibbon.GetCurrent(this.Page) == null)
                    return;
                if (tabDefinition != null && !this.DesignMode)
                    RibbonController.Current.AddRibbonTabToPage(tabDefinition, this.Page, false);
            }
            catch (Exception ex)
            {
                SPDiagnosticsService diagSvc = SPDiagnosticsService.Local;
                diagSvc.WriteTrace(0, new SPDiagnosticsCategory("Ribbon", TraceSeverity.Monitorable, EventSeverity.Error),
                                        TraceSeverity.Monitorable,
                                        "Error occured: " + ex.Message + "\nStackTrace: " + ex.StackTrace);
            }
        }
        #endregion
    }
}
