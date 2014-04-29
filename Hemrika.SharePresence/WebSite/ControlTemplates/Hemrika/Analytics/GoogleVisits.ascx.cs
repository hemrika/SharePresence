using System;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using Microsoft.SharePoint;
using Hemrika.SharePresence.Google.Analytics;

namespace Hemrika.SharePresence.WebSite.ControlTemplates
{
    public partial class GoogleVisits : UserControl
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            GooglePageBase google = Page as GooglePageBase;

            DataQuery query = new DataQuery();
            query.Ids = "ga:"+google.Settings.Current.Id;
            query.Metrics = "ga:visits,ga:pageviews";
            query.Dimensions = "ga:date";
            query.Sort = "ga:date";
            query.GAStartDate = DateTime.Now.AddMonths(-1).AddDays(-1).ToString("yyyy-MM-dd");
            query.GAEndDate = DateTime.Now.AddDays(-1).ToString("yyyy-MM-dd");

            if (google.Referringpage != null)
            {
                query.Filters = "ga:pagePath==" + google.Referringpage;
            }

            DataFeed actual = google.Analytics.Query(query);

            System.Data.DataTable dt = new System.Data.DataTable("Visits / Page Views");
            dt.Columns.Add("Date");
            dt.Columns.Add("Visits", typeof(int));
            dt.Columns.Add("Page Views", typeof(int));
            dt.Columns.Add("Page Views per Visit", typeof(int));

            foreach (DataEntry entry in actual.Entries)
            {
                try
                {
                    string date = entry.Title.Text.Split(new char[] { '=' })[1].ToString();
                    DateTime datetime = new DateTime(int.Parse(date.Substring(0, 4)), int.Parse(date.Substring(4, 2)), int.Parse(date.Substring(6, 2)));
                    date = datetime.ToString("yyyy-MM-dd");
                    int visits = int.Parse(entry.Metrics[0].Value);
                    int views = int.Parse(entry.Metrics[1].Value);
                    int viewspervisit = 0;

                    try
                    {
                        viewspervisit = (views / visits);
                    }
                    catch (Exception ex)
                    {
                        ex.ToString();
                    }
                    // entry.Metrics[2].Value;

                    dt.Rows.Add(new object[] { date, visits, views, viewspervisit });
                }
                catch (Exception ex)
                {
                    Response.Write(ex.ToString());
                }
            }

            this.area_visits.GviEnableEvents = true;
            this.area_visits.ChartData(dt);
        }
    }
}
