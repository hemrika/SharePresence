using System;
using Microsoft.SharePoint;
using Microsoft.SharePoint.WebControls;
using Microsoft.SharePoint.Utilities;
using System.Web;

namespace Hemrika.SharePresence.WebSite
{
    public partial class GoogleAnalytics : GooglePageBase
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            string url = string.Empty;

            if (Request.QueryString.HasKeys())
            {
                if (!String.IsNullOrEmpty(Request.QueryString["Report"]))
                {
                    string report = Request.QueryString["Report"];

                    if (report == "GoogleAnalytics")
                    {
                        /*
                        string w = Settings.Current.WebPropertyId;
                        string p = Settings.Current.ProfileId;
                        string a = Settings.Current.AccountId;
                        url = @"https://www.google.com/analytics/web/";//#report/visitors-overview/a9934149w23790730p22156415/";
                        */
                    }

                    if (report == "CustomReport")
                    {
                        url = @"http://www.SharePresense.com/Reports";
                    }

                }
            }

            if (!String.IsNullOrEmpty(url))
            {
                SPUtility.Redirect(url, SPRedirectFlags.Trusted, HttpContext.Current);
            }
        }
    }
}
