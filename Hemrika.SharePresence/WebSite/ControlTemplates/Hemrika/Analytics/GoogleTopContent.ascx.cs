using System;
using System.Data;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using Hemrika.SharePresence.Google.Analytics;

namespace Hemrika.SharePresence.WebSite.ControlTemplates
{
    public partial class GoogleTopContent : UserControl
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            GooglePageBase google = Page as GooglePageBase;

            DataQuery query = new DataQuery();
            query.Ids = "ga:"+google.Settings.Current.Id;
            query.Metrics = "ga:pageviews";
            query.Dimensions = "ga:pagePath,ga:pageTitle";
            query.Sort = "-ga:pageviews";
            query.NumberToRetrieve = 10;
            query.GAStartDate = DateTime.Now.AddDays(-14).ToString("yyyy-MM-dd");
            query.GAEndDate = DateTime.Now.AddDays(-2).ToString("yyyy-MM-dd");

            DataFeed actual = google.Analytics.Query(query);

            System.Data.DataTable dt = new System.Data.DataTable("Browser");
            dt.Columns.Add("Path");
            dt.Columns.Add("Title");
            dt.Columns.Add("Views", typeof(int));

            foreach (DataEntry entry in actual.Entries)
            {
                dt.Rows.Add(new object[] { entry.Dimensions[0].Value, entry.Dimensions[1].Value, int.Parse(entry.Metrics[0].Value) });
            }

            foreach (DataRow row in dt.Rows)
            {
                //Response.Write(row["Title"].ToString() + " : " + row["Views"].ToString());
            }
            //Response.Write(dt.Rows.Count.ToString());
        }
    }
}
