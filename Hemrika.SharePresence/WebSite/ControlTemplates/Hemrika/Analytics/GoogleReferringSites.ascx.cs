using System;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using Microsoft.SharePoint;
using Hemrika.SharePresence.Google.Analytics;

namespace Hemrika.SharePresence.WebSite.ControlTemplates
{
    public partial class GoogleReferringSites : UserControl
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            GooglePageBase google = Page as GooglePageBase;

            DataQuery query = new DataQuery();
            query.Ids = "ga:"+google.Settings.Current.Id;;
            query.Metrics = "ga:pageviews,ga:timeOnSite,ga:exits";
            query.Dimensions = "ga:source";
            query.Sort = "-ga:pageviews";
            query.Filters = "ga:medium==referral";
            query.GAStartDate = DateTime.Now.AddMonths(-1).AddDays(-1).ToString("yyyy-MM-dd");
            query.GAEndDate = DateTime.Now.AddDays(-1).ToString("yyyy-MM-dd");

            if (google.Referringpage != null)
            {
                query.Filters = "ga:pagePath==" + google.Referringpage;
            }

            DataFeed actual = google.Analytics.Query(query);

            System.Data.DataTable dt = new System.Data.DataTable("ReferringSites");
            dt.Columns.Add("Page Views", typeof(int));
            dt.Columns.Add("Time On Site", typeof(int));
            dt.Columns.Add("Exits", typeof(int));
            dt.Columns.Add("Source");//, typeof(int));

            foreach (DataEntry entry in actual.Entries)
            {
                try
                {
                    int pageviews = int.Parse(entry.Metrics[0].Value);
                    float timeOnSite = float.Parse(entry.Metrics[1].Value);
                    int exits = int.Parse(entry.Metrics[2].Value);
                    string source = entry.Dimensions[0].Value.ToString();
                    dt.Rows.Add(new object[] { pageviews, timeOnSite, exits, source });
                }
                catch (Exception ex)
                {
                    Response.Write(ex.ToString());
                }
            }

            this.tbl_ReferringSites.ChartData(dt);
        }
    }
}
