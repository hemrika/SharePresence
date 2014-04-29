using System;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using Microsoft.SharePoint;
using Hemrika.SharePresence.Google.Analytics;

namespace Hemrika.SharePresence.WebSite.ControlTemplates
{
    public partial class GoogleBounceExit : UserControl
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            GooglePageBase google = Page as GooglePageBase;

            DataQuery query = new DataQuery();
            query.Ids = "ga:"+google.Settings.Current.Id;
            query.Metrics = "ga:pageviews,ga:entrances,ga:bounces,ga:exits";
            query.Dimensions = "ga:date";
            query.Sort = "ga:date";
            query.GAStartDate = DateTime.Now.AddMonths(-1).AddDays(-1).ToString("yyyy-MM-dd");
            query.GAEndDate = DateTime.Now.AddDays(-1).ToString("yyyy-MM-dd");

            if (google.Referringpage != null)
            {
                query.Filters = "ga:pagePath==" + google.Referringpage;
            }

            DataFeed actual = google.Analytics.Query(query);

            System.Data.DataTable dt = new System.Data.DataTable("Entrances / Bounces / Exits");
            dt.Columns.Add("Date");
            dt.Columns.Add("Entrance", typeof(int));
            dt.Columns.Add("Bounce", typeof(int));
            dt.Columns.Add("Exit", typeof(int));

            foreach (DataEntry entry in actual.Entries)
            {
                try
                {
                    string date = entry.Title.Text.Split(new char[] { '=' })[1].ToString();
                    DateTime datetime = new DateTime(int.Parse(date.Substring(0, 4)), int.Parse(date.Substring(4, 2)), int.Parse(date.Substring(6, 2)));
                    date = datetime.ToString("yyyy-MM-dd");
                    int views = int.Parse(entry.Metrics[0].Value);
                    int entrance = int.Parse(entry.Metrics[1].Value);
                    int bounce = int.Parse(entry.Metrics[2].Value);
                    int exit = int.Parse(entry.Metrics[3].Value);

                    int bouncerate = 0;

                    try
                    {
                        bouncerate = (bounce / entrance) * 100;
                    }
                    catch (Exception ex)
                    {
                        ex.ToString();
                    }

                    //int exitrate = 0;

                    try
                    {
                        exit = (exit / views);
                    }
                    catch (Exception ex)
                    {
                        ex.ToString();
                    }

                    dt.Rows.Add(new object[] { date, entrance, bounce, exit });
                }
                catch (Exception ex)
                {
                    Response.Write(ex.ToString());
                }
            }

            this.area_bounce.GviEnableEvents = true;
            this.area_bounce.ChartData(dt);
        }
    }
}
