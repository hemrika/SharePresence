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
    public partial class GoogleKeywords : UserControl
    {
        private GooglePageBase google;

        protected void Page_Load(object sender, EventArgs e)
        {
            google = Page as GooglePageBase;

            System.Data.DataTable dt = new System.Data.DataTable("Keywords");
            dt.Columns.Add("Keywords");
            dt.Columns.Add("Visitors", typeof(int));

            System.Data.DataTable wm = new System.Data.DataTable("Keywords");
            wm.Columns.Add("Name");
            wm.Columns.Add("Value");

            try
            {
                DataQuery query = new DataQuery();
                query.Ids = "ga:"+google.Settings.Current.Id;
                query.Dimensions = "ga:keyword";
                query.Metrics = "ga:visits";
                query.Sort = "-ga:visits";
                query.GAStartDate = DateTime.Now.AddMonths(-1).AddDays(-1).ToString("yyyy-MM-dd");
                query.GAEndDate = DateTime.Now.AddDays(-1).ToString("yyyy-MM-dd");

                if (google.Referringpage != null)
                {
                    query.Filters = "ga:pagePath==" + google.Referringpage;
                }

                /*
                if (google.Referringpage != null)
                {
                    query.Filters = "ga:keyword!=(not set),ga:pagePath==" + google.Referringpage;
                }
                else
                {
                    query.Filters = "ga:keyword!=(not set)";
                }
                */

                DataFeed actual = google.Analytics.Query(query);

                foreach (DataEntry entry in actual.Entries)
                {
                    dt.Rows.Add(new object[] { entry.Dimensions[0].Value, int.Parse(entry.Metrics[0].Value) });
                }

            }
            catch (Exception ex)
            {
                ex.ToString();
                //TextBox1.Text = ex.ToString();
            }

            try
            {
                /*
                MessagesQuery mquery = new MessagesQuery();

                MessagesFeed messages = google.Webmastertools.Query(mquery);

                TextBox1.Text += messages.Title.Text + "\n";
                TextBox1.Text += messages.TotalResults + "\n";
                TextBox1.Text += messages.Entries.Count + "\n";

                foreach (MessagesEntry entry in messages.Entries)
                {

                    TextBox1.Text += entry.Title.Text + "\n";
                    TextBox1.Text += entry.Subject.Value + "\n";
                    TextBox1.Text += entry.Body + "\n";
                    entry.Delete();
                }
                */

                using(GoogleWebmasterToolsSettings settings = google.Settings.Webmaster)
                {
                    KeywordsQuery query = new KeywordsQuery(KeywordsQuery.CreateCustomUri(settings.EncodedSiteId));
                    KeywordsFeed keywords = google.Webmastertools.Query(query);

                    /*
                    TextBox1.Text += keywords.Title.Text + "\n";
                    TextBox1.Text += keywords.TotalResults + "\n";
                    TextBox1.Text += keywords.Entries.Count + "\n";
                    */

                    foreach (KeywordsEntry entry in keywords.Entries)
                    {
                        wm.Rows.Add(new object[] { entry.Keyword.Name, entry.Keyword.Value });
                    }

                    /*
                    SitemapsQuery squery = new SitemapsQuery(SitemapsQuery.CreateCustomUri(settings.EncodedSiteId));
                    SitemapsFeed sitemaps = google.Webmastertools.Query(squery);

                    TextBox1.Text += sitemaps.Title.Text + "\n";
                    TextBox1.Text += sitemaps.TotalResults + "\n";
                    TextBox1.Text += sitemaps.Entries.Count + "\n";

                    foreach (SitemapsEntry entry in sitemaps.Entries)
                    {
                        TextBox1.Text += entry.Title.Text + "\n";
                        TextBox1.Text += entry.SitemapLastDownloaded + "\n";
                        TextBox1.Text += entry.SitemapType + "\n";
                        TextBox1.Text += entry.SitemapUrlCount + "\n";
                    }


                    CrawlIssuesQuery cquery = new CrawlIssuesQuery(CrawlIssuesQuery.CreateCustomUri(settings.EncodedSiteId));
                    CrawlIssuesFeed crawlissues = google.Webmastertools.Query(cquery);

                    TextBox1.Text += crawlissues.Title.Text + "\n";
                    TextBox1.Text += crawlissues.TotalResults + "\n";
                    TextBox1.Text += crawlissues.Entries.Count + "\n";

                    foreach (CrawlIssuesEntry entry in crawlissues.Entries)
                    {
                        
                        TextBox1.Text += entry.Title.Text + "\n";
                        TextBox1.Text += entry.IssueType + "\n";
                        TextBox1.Text += entry.IssueDetail + "\n";
                        //entry.Delete();
                    }
                    */
                }
                /*
                TextBox1.Text += google.Settings.Webmaster.EncodedSiteId;
                KeywordsQuery query = new KeywordsQuery(KeywordsQuery.CreateCustomUri(google.Settings.Webmaster.EncodedSiteId));
                KeywordsFeed keywords = google.Webmastertools.Query(query);

                TextBox1.Text += keywords.Title.Text + "\n";
                //TextBox1.Text += keywords.Subtitle.Text + "\n";
                TextBox1.Text += keywords.TotalResults + "\n";
                TextBox1.Text += keywords.Entries.Count + "\n";

                foreach (KeywordsEntry entry in keywords.Entries)
                {
                    TextBox1.Text += entry.Title + "\n";
                    TextBox1.Text += entry.Content.Content + "\n";
                    TextBox1.Text += entry.Content.Src + "\n";
                    TextBox1.Text += entry.Keyword.ChildNodes.Count + "\n";
                    TextBox1.Text += entry.Keyword.Name + "\n";
                    TextBox1.Text += entry.Keyword.Value + "\n";

                    //dt.Rows.Add(new object[] { entry.Keyword.Name, entry.Keyword.Value });
                }
                */
            }
            catch (Exception ex)
            {
                ex.ToString();
                //TextBox1.Text += ex.ToString();
            }

            this.tbl_Keywords.ChartData(dt);
            this.tbl_Webmaster.ChartData(wm);

        }

        protected override void Render(HtmlTextWriter writer)
        {
            base.Render(writer);
        }
    }
}
