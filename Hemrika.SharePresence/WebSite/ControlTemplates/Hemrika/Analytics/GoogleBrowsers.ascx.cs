using System;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using Hemrika.SharePresence.Google.Analytics;
using Hemrika.SharePresence.Google.Visualization;
using System.Web.UI.HtmlControls;

namespace Hemrika.SharePresence.WebSite.ControlTemplates
{
    public partial class GoogleBrowsers : UserControl
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            GooglePageBase google = Page as GooglePageBase;

            DataQuery query = new DataQuery();
            query.Ids = "ga:"+google.Settings.Current.Id;
            query.Metrics = "ga:pageviews";
            query.Dimensions = "ga:browser,ga:browserVersion";
            query.Sort = "ga:browser,ga:pageviews";
            query.GAStartDate = DateTime.Now.AddMonths(-1).AddDays(-1).ToString("yyyy-MM-dd");
            query.GAEndDate = DateTime.Now.AddDays(-1).ToString("yyyy-MM-dd");

            if (google.Referringpage != null)
            {
                query.Filters = "ga:pagePath==" + google.Referringpage;
            }

            DataFeed actual = google.Analytics.Query(query);

            System.Data.DataTable browsers = new System.Data.DataTable("Browser");
            browsers.Columns.Add("Browser");
            browsers.Columns.Add("Views", typeof(int));

            System.Data.DataTable versions = new System.Data.DataTable("BrowserVersion");
            versions.Columns.Add("Browser");
            versions.Columns.Add("Version");
            versions.Columns.Add("Views", typeof(int));

            System.Collections.SortedList values = new System.Collections.SortedList();
            foreach (DataEntry entry in actual.Entries)
            {
                string browser = entry.Dimensions[0].Value.ToString();
                string version = entry.Dimensions[1].Value.ToString();
                int visits = int.Parse(entry.Metrics[0].Value);

                if (values.ContainsKey(browser))
                {
                    int current = int.Parse(values[browser].ToString());
                    current += int.Parse(entry.Metrics[0].Value);
                    values[browser] = current;
                }
                else
                {
                    values.Add(browser, int.Parse(entry.Metrics[0].Value));
                }

                versions.Rows.Add(new object[] { browser,version, visits });
            }

            foreach (String key in values.Keys)
            {
                browsers.Rows.Add(new object[] { key, int.Parse(values[key].ToString()) });
            }

            this.pie_Browsers.GviEnableEvents = true;
            this.pie_Browsers.ChartData(browsers);

            this.tbl_Versions.ChartData(versions);
        }
    }
}
