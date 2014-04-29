using System;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using Hemrika.SharePresence.Google.Analytics;

namespace Hemrika.SharePresence.WebSite.ControlTemplates
{
    public partial class GoogleSource : UserControl
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            GooglePageBase google = Page as GooglePageBase;

            DataQuery query = new DataQuery();
            query.Ids = "ga:"+google.Settings.Current.Id;
            query.Metrics = "ga:visits";
            query.Dimensions = "ga:date";
            query.Sort = "ga:medium==(none)";
            query.GAStartDate = DateTime.Now.AddMonths(-1).AddDays(-1).ToString("yyyy-MM-dd");
            query.GAEndDate = DateTime.Now.AddDays(-1).ToString("yyyy-MM-dd");

            if (google.Referringpage != null)
            {
                query.Filters = "ga:pagePath==" + google.Referringpage;
            }

            DataFeed actual = google.Analytics.Query(query);

            System.Data.DataTable dt = new System.Data.DataTable("Browser");
            dt.Columns.Add("Visits");
            dt.Columns.Add("Date", typeof(int));
            //int total = 0;

            foreach (DataEntry entry in actual.Entries)
            {
                dt.Rows.Add(new object[] { entry.Title.Text.Split(new char[] { '=' })[1].ToString(), int.Parse(entry.Metrics[0].Value) });
                //total += int.Parse(entry.Metrics[0].Value);
            }

            //Response.Write(total.ToString());

            this.pie_Source.GviEnableEvents = true;
            this.pie_Source.ChartData(dt);

        }
    }
}
