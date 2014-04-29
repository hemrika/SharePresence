// -----------------------------------------------------------------------
// <copyright file="WebSiteMaster.cs" company="">
// TODO: Update copyright text.
// </copyright>
// -----------------------------------------------------------------------

namespace Hemrika.SharePresence.WebSite.Master
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Web.UI;
    using System.Web.UI.WebControls;
    using System.Web;
    using System.Web.UI.HtmlControls;
    using Microsoft.SharePoint;
    using Hemrika.SharePresence.WebSite.Page;
using Microsoft.SharePoint.WebControls;
    using Hemrika.SharePresence.Common.UI;
    using Microsoft.SharePoint.Utilities;

    /// <summary>
    /// TODO: Update summary.
    /// </summary>
    public class WebSiteMaster : EnhancedMasterPage
    {

        public WebSiteMaster()
        {
            //string controls = this.Controls.Count.ToString();
        }

        protected override void OnPreRender(EventArgs e)
        {
            try
            {
                if (HttpContext.Current.User.Identity.IsAuthenticated)
                {
                    CssRegistration.Register("/_layouts/Hemrika/WebSitePage/Hemrika.SharePresence.WebSite.css");
                }
                /*
                SiteMapNode current = SiteMap.CurrentNode;
                if (current != null) { this.Page.Title = current.Title; }
                */

                //<meta http-equiv="X-UA-Compatible" content="IE=8"/>
                //Editor Mode Compatible
                //HtmlMeta compatible = new HtmlMeta();
                //compatible.HttpEquiv = "X-UA-Compatible";

                //DisplayControlModes modes = WebSitePage.DetermineDisplayControlModes();
                //if (modes.displayMode == SPControlMode.Display)
                //{
                //    compatible.Content = "IE=100";
                //}
                //else
                //{
                //    compatible.Content = "IE=8";
                //}
                //Page.Header.Controls.Add(compatible);
            }
            catch (Exception)
            {
            }

            base.OnPreRender(e);
        }

        protected override void OnInit(EventArgs e)
        {
            base.OnInit(e);
        }

        protected override void OnLoad(EventArgs e)
        {
            try
            {
                if (!HttpContext.Current.User.Identity.IsAuthenticated)
                {
                    Response.Cache.SetCacheability(System.Web.HttpCacheability.Public);
                    Response.Headers.Add("Vary", "Accept-Encoding");
                }
            }
            catch (Exception ex)
            {
                ex.ToString();
            }

            try
            {
                HtmlGenericControl body = this.FindControl("body") as HtmlGenericControl;
                if (body != null)
                {
                    body.Attributes.Add("onload", "if (typeof(_spBodyOnLoadWrapper) != 'undefined') _spBodyOnLoadWrapper();");
                }
            }
            catch { };

            base.OnLoad(e);
        }

        public override Common.Ribbon.Definitions.TabDefinition GetTabDefinition()
        {
            return null;
        }
    }
}
