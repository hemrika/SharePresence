// -----------------------------------------------------------------------
// <copyright file="EnhancedWebPartPage.cs" company="Microsoft">
// TODO: Update copyright text.
// </copyright>
// -----------------------------------------------------------------------

namespace Hemrika.SharePresence.Common.UI
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using Microsoft.SharePoint.WebPartPages;
    using Hemrika.SharePresence.Common.Ribbon.Definitions;
    using Hemrika.SharePresence.Common.Ribbon;
    using Microsoft.SharePoint;
    using Microsoft.SharePoint.WebControls;
    using Microsoft.SharePoint.Administration;
    using System.Web;
    using Microsoft.SharePoint.Utilities;
using System.Web.UI;
using System.IO;
    using System.IO.Compression;
    using System.Web.Caching;

    /// <summary>
    /// TODO: Update summary.
    /// </summary>
    public abstract class EnhancedWebPartPage : WebPartPage
    {
        public EnhancedWebPartPage()
        {

        }

        /// <summary>
        /// Gets a value indicating whether the page is processed asynchronously.
        /// </summary>
        public new bool IsAsync
        {
            get
            { return true; }
        }

        protected override object LoadPageStateFromPersistenceMedium()
        {
            /*
            string viewState = Request.Form["__VSTATE"];
            byte[] bytes = Convert.FromBase64String(viewState);
            bytes = Decompress(bytes);
            LosFormatter formatter = new LosFormatter();
            return formatter.Deserialize(Convert.ToBase64String(bytes));
            */
            /*
            try
            {
                if (Page.Session != null && Page.Session.Mode != System.Web.SessionState.SessionStateMode.Off)
                {
                    string VSKey = Request.Form["__VIEWSTATE_KEY"];
                    return Cache[VSKey];
                }
                else
                {
                    return base.LoadPageStateFromPersistenceMedium();
                }
            }
            catch
            {
                return base.LoadPageStateFromPersistenceMedium();
            }
            */

            return base.LoadPageStateFromPersistenceMedium();
        }

        protected override void SavePageStateToPersistenceMedium(object viewState)
        {
            /*
            LosFormatter formatter = new LosFormatter();
            StringWriter writer = new StringWriter();
            formatter.Serialize(writer, viewState);
            string viewStateString = writer.ToString();
            byte[] bytes = Convert.FromBase64String(viewStateString);
            bytes = Compress(bytes);
            ClientScript.RegisterHiddenField("__VSTATE", Convert.ToBase64String(bytes));
            */
            /*
            try
            {
                if (Page.Session != null && Page.Session.Mode != System.Web.SessionState.SessionStateMode.Off)
                {
                    string VSKey = "VIEWSTATE_" + base.Session.SessionID + "_" +
                    Request.RawUrl + "_" + DateTime.Now.Ticks.ToString();
                    Cache.Add(VSKey, viewState, null, DateTime.Now.AddMinutes(Session.Timeout), Cache.NoSlidingExpiration, CacheItemPriority.High, null);
                    ClientScript.RegisterHiddenField("__VIEWSTATE_KEY", VSKey);
                }
                else
                {
                    base.SavePageStateToPersistenceMedium(viewState);
                }
            }
            catch
            {
                base.SavePageStateToPersistenceMedium(viewState);
            }
            */
            base.SavePageStateToPersistenceMedium(viewState);
        }

        public static byte[] Compress(byte[] data)
        {

            MemoryStream output = new MemoryStream();

            GZipStream gzip = new GZipStream(output,

                              CompressionMode.Compress, true);

            gzip.Write(data, 0, data.Length);

            gzip.Close();

            return output.ToArray();

        }



        public static byte[] Decompress(byte[] data)
        {

            MemoryStream input = new MemoryStream();

            input.Write(data, 0, data.Length);

            input.Position = 0;

            GZipStream gzip = new GZipStream(input,

                              CompressionMode.Decompress, true);

            MemoryStream output = new MemoryStream();

            byte[] buff = new byte[64];

            int read = -1;

            read = gzip.Read(buff, 0, buff.Length);

            while (read > 0)
            {

                output.Write(buff, 0, read);

                read = gzip.Read(buff, 0, buff.Length);

            }

            gzip.Close();

            return output.ToArray();

        }

        protected override System.Web.UI.PageStatePersister PageStatePersister
        {
            get
            {
                return base.PageStatePersister;
            }
        }


        protected override void OnInit(EventArgs e)
        {
            base.OnInit(e);
        }

        /*
        private void FormOnLoad(object sender, EventArgs e)
        {
            if (HttpContext.Current != null)
            {
                SPWeb contextWeb = SPControl.GetContextWeb(HttpContext.Current);
                if (contextWeb != null)
                {
                    SPWebPartManager.RegisterOWSScript(this, contextWeb);
                    if (this.Page.Items["FormDigestRegistered"] == null)
                    {
                        //string bstrUrl = SPGlobal.GetVTIRequestUrl(this.Context.Request, null).ToString();
                        //SPStringCallback pFormCallback = new SPStringCallback();
                        //contextWeb.Request.RenderFormDigest(bstrUrl, pFormCallback);
                        //base.ClientScript.RegisterHiddenField("__REQUESTDIGEST", SPHttpUtility.NoEncode(pFormCallback.StringResult));
                        //FormDigest.RegisterDigestUpdateClientScriptBlockIfNeeded(this);
                        //this.Page.Items["FormDigestRegistered"] = true;
                    }
                }
            }
        }
        */
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

        protected internal bool IsDialogMode
        {
            get
            {
                return (this.Context.Request.QueryString["IsDlg"] != null);
            }
        }
    }
}
