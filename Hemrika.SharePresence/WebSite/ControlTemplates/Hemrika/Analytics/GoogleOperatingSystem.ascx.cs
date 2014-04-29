using System;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using Hemrika.SharePresence.Google.Analytics;
using Hemrika.SharePresence.Google.WebmasterTools;
using Microsoft.SharePoint;
using System.Xml;
using System.Security.Policy;
using System.Text;
using Hemrika.SharePresence.Google.Client;
using System.Net;
using System.IO;
using System.Web;
using Hemrika.SharePresence.Google;

namespace Hemrika.SharePresence.WebSite.ControlTemplates
{
    public partial class GoogleOperatingSystem : UserControl
    {
        private GooglePageBase google;

        protected void Page_Load(object sender, EventArgs e)
        {
            google = Page as GooglePageBase;

            try
            {
                //dimensions=ga:operatingSystem,ga:operatingSystemVersion,ga:browser,ga:browserVersion
                //metrics=ga:visits
                DataQuery query = new DataQuery();
                query.Ids = "ga:"+google.Settings.Current.Id;;
                query.Dimensions = "ga:operatingSystem,ga:operatingSystemVersion";
                query.Metrics = "ga:visits";
                query.Sort = "-ga:visits";
                query.GAStartDate = DateTime.Now.AddMonths(-1).AddDays(-1).ToString("yyyy-MM-dd");
                query.GAEndDate = DateTime.Now.AddDays(-1).ToString("yyyy-MM-dd");

                if (google.Referringpage != null)
                {
                    query.Filters = "ga:pagePath==" + google.Referringpage;
                }

                DataFeed actual = google.Analytics.Query(query);

                System.Data.DataTable dt = new System.Data.DataTable("Operating Systems");
                dt.Columns.Add("Operating System");
                dt.Columns.Add("System Version");
                dt.Columns.Add("Visits", typeof(int));

                foreach (DataEntry entry in actual.Entries)
                {
                    int visits = int.Parse(entry.Metrics[0].Value);
                    string os = entry.Dimensions[0].Value.ToString();
                    string osVersion = entry.Dimensions[1].Value.ToString();
                    dt.Rows.Add(new object[] { os,osVersion,visits });
                }

                this.tbl_OS.ChartData(dt);
            }
            catch (Exception ex)
            {
                ex.ToString();
                //TextBox1.Text = ex.ToString();
            }
        }

        protected override void Render(HtmlTextWriter writer)
        {
            base.Render(writer);
        }
    }
}
