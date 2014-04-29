using System;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using Microsoft.SharePoint;
using Hemrika.SharePresence.Google.Analytics;

namespace Hemrika.SharePresence.WebSite.ControlTemplates
{
    public partial class GoogleTimeonSite : UserControl
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            GooglePageBase google = Page as GooglePageBase;

            DataQuery query = new DataQuery();
            query.Ids = "ga:"+google.Settings.Current.Id;;
            query.Metrics = "ga:visits,ga:timeOnSite";
            query.Dimensions = "ga:date";
            query.Sort = "ga:date";
            query.GAStartDate = DateTime.Now.AddMonths(-1).AddDays(-1).ToString("yyyy-MM-dd");
            query.GAEndDate = DateTime.Now.AddDays(-1).ToString("yyyy-MM-dd");

            if (google.Referringpage != null)
            {
                query.Filters = "ga:pagePath==" + google.Referringpage;
            }

            DataFeed actual = google.Analytics.Query(query);

            System.Data.DataTable dt = new System.Data.DataTable("Average Time by Date");
            dt.Columns.Add("Date");
            dt.Columns.Add("Average Time", typeof(float));
            dt.Columns.Add("Visits", typeof(int));

            System.Data.DataTable avg = new System.Data.DataTable("Average Time");
            avg.Columns.Add("Average");//, typeof(int));

            int visitstotal = 0;
            float time = float.Parse("0.0");

            foreach (DataEntry entry in actual.Entries)
            {
                try
                {
                    DateTime datetime = new DateTime(int.Parse(entry.Dimensions[0].Value.Substring(0, 4)), int.Parse(entry.Dimensions[0].Value.Substring(4, 2)), int.Parse(entry.Dimensions[0].Value.Substring(6, 2)));
                    string date = datetime.ToString("yyyy-MM-dd");

                    int visits = int.Parse(entry.Metrics[0].Value);
                    visitstotal += visits;
                    string timeonsite = entry.Metrics[1].Value.ToString();
                    time += float.Parse(timeonsite);
                    if (visits > 0)
                    {

                        dt.Rows.Add(new object[] { date, (float.Parse(timeonsite) / visits), visits });
                    }
                    else
                    {
                        dt.Rows.Add(new object[] { date, float.Parse("0.0"),visits});
                    }
                }
                catch (Exception ex)
                {
                    Response.Write(ex.ToString());
                }
            }

            avg.Rows.Add(new object[] {(time/visitstotal).ToString()+" seconds" });

            this.tbl_averageTime.ChartData(avg);
            this.area_averageTime.GviEnableEvents = true;
            this.area_averageTime.ChartData(dt);
        }
    }
}
