using System;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using Microsoft.SharePoint;
using System.Text;
using Hemrika.SharePresence.Google;
using System.Web;
using System.Diagnostics;
using Hemrika.SharePresence.WebSite.Page;

namespace Hemrika.SharePresence.WebSite.ControlTemplates
{
    public partial class BlogConnector : UserControl
    {

        private bool _IsSystemPage;

        public bool IsSystemPage
        {
            get { return _IsSystemPage; }
            set { _IsSystemPage = value; }
        }


        protected override void OnInit(EventArgs e)
        {
            base.OnInit(e);
            IsSystemPage = true;
            try
            {

                if (Application != null && Request != null && !Request.Url.ToString().Contains("_layouts") && !Request.Url.ToString().Contains("Forms"))
                {
                    IsSystemPage = false;

                }
            }
            catch { };
        }

        protected string Discovery
        {
            get
            {
                StringBuilder meta = new StringBuilder();

                if (SPContext.Current.ListItem != null && WebSitePage.IsBlogPage(SPContext.Current.ListItem))
                {
                    SPWeb web = SPContext.Current.Web;

                    HttpContext.Current.Response.AddHeader("X-Pingback", string.Format("{0}/_layouts/pingback.axd", web.Url));

                    string webUrl = SPContext.Current.Web.Url;
                    // add links for autodiscovering blogsettings for blogging software
                    string rsdAutodiscoverUrl = webUrl + "/rsdAutoDiscovery.axd";
                    string wlwAutodiscoverUrl = webUrl + "/wlwAutoDiscovery.axd";
                    meta.AppendLine("<link rel=\"EditURI\" type=\"application/rsd+xml\" href=\"" + rsdAutodiscoverUrl + "\" />");
                    meta.AppendLine("<link rel=\"wlwmanifest\" type=\"application/wlwmanifest+xml\" href=\"" + wlwAutodiscoverUrl + "\" />");

                    //additionalHead.Text += "\r\n";
                }

                return meta.ToString();
            }
        }

        /*
        protected void Page_Load(object sender, EventArgs e)
        {
            if (SPContext.Current.ListItem != null )//&& WebSitePage.IsBlogPage(SPContext.Current.ListItem))
            {
                SPWeb web = SPContext.Current.Web;

                // add pingback url to response header
                HttpContext.Current.Response.AddHeader("X-Pingback", string.Format("{0}/_layouts/pingback.axd", web.Url));

                try
                {
                    Control oHead = Page.Header.FindControl("head");
                    if (oHead != null)
                    {
                        var additionalHead = new LiteralControl();

                        string webUrl = SPContext.Current.Web.Url;
                        // add links for autodiscovering blogsettings for blogging software
                        string rsdAutodiscoverUrl = webUrl + "/rsdAutoDiscovery.axd";
                        string wlwAutodiscoverUrl = webUrl + "/wlwAutoDiscovery.axd";
                        additionalHead.Text = "\r\n<link rel=\"EditURI\" type=\"application/rsd+xml\" href=\"" + rsdAutodiscoverUrl + "\" />";
                        additionalHead.Text += "\r\n<link rel=\"wlwmanifest\" type=\"application/wlwmanifest+xml\" href=\"" + wlwAutodiscoverUrl + "\" />";

                        additionalHead.Text += "\r\n";
                        oHead.Controls.Add(additionalHead);
                    }
                }
                catch (Exception ex)
                {
                    Debug.WriteLine(ex);
                }
            }
        }
        */
    }
}
