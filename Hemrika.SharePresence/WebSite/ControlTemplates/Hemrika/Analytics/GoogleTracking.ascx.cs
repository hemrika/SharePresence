using System;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using Microsoft.SharePoint;
using Hemrika.SharePresence.Google.Analytics;
using System.Data;
using Microsoft.SharePoint.Utilities;

namespace Hemrika.SharePresence.WebSite.ControlTemplates
{
    public partial class GoogleTracking : UserControl
    {
        private GooglePageBase google;
        private System.Data.DataTable previousDT = new System.Data.DataTable("Previous");
        private System.Data.DataTable nextDT = new System.Data.DataTable("Next");
        private SPListItem root;

        protected void Page_Load(object sender, EventArgs e)
        {
            google = Page as GooglePageBase;
            {
                if (google.Referringpage != null)
                {
                    previousDT.Columns.Add("Source");
                    previousDT.Columns.Add("Target");//, typeof(int));
                    previousDT.Columns.Add("Title");//, typeof(int));

                    nextDT.Columns.Add("Source");
                    nextDT.Columns.Add("Target");//, typeof(int));
                    nextDT.Columns.Add("Title");//, typeof(int));

                    try
                    {
                        previousDT.Rows.Add(new object[] { "(entrance)", "", "(entrance)" });
                        nextDT.Rows.Add(new object[] { "(entrance)", "", "(entrance)" });

                        SPSite site = SPContext.Current.Site;
                        SPWeb web = site.OpenWeb(google.Referringpage);
                        root = web.GetListItem(google.Referringpage);
                        //string serverRelativeListItemUrl = SPUrlUtility.CombineUrl(SPContext.Current.Site.RootWeb.ServerRelativeUrl,google.Referringpage);
                        //root = SPContext.Current.Site.RootWeb.GetListItem(serverRelativeListItemUrl);

                        //dt.Rows.Add(new object[] { root.Title, SPContext.Current.Site.PortalName, root.Title });
                        previousDT.Rows.Add(new object[] { root.Title, "(entrance)", root.Title });
                    }
                    catch (Exception ex)
                    {
                        Response.Write("Root" + ex.ToString());
                    }


                    GetNext();
                    this.org_tracking_next.ChartData(nextDT);

                    GetPrevious();
                    this.org_tracking_prev.ChartData(previousDT);

                    this.tbl_Tracking.ChartData(nextDT);
                }
            }
        }

        private void GetNext()
        {
            DataQuery query = new DataQuery();
            query.Ids = "ga:"+google.Settings.Current.Id;
            query.Dimensions = "ga:nextPagePath";
            query.Metrics = "ga:pageviews";
            query.GAStartDate = DateTime.Now.AddMonths(-1).AddDays(-1).ToString("yyyy-MM-dd");
            query.GAEndDate = DateTime.Now.AddDays(-1).ToString("yyyy-MM-dd");

            if (google.Referringpage != null)
            {
                //query.Filters = "ga:pagePath==" + google.Referringpage;
                query.Filters = "ga:previousPagePath==" + google.Referringpage;
            }

            DataFeed actual = google.Analytics.Query(query);

            foreach (DataEntry entry in actual.Entries)
            {
                try
                {
                    string title = entry.Title.Text;//.Split(new char[] { '=' })[1].ToString();

                    string next = entry.Dimensions[0].Value;
                    //string next = entry.Metrics[1].Value;//entry.Dimensions[1].Value;
                    //Response.Write("Next :" + next);

                    if (next != google.Referringpage && !next.Contains("_layouts") && !next.Contains("Forms") && next != "(entrance)")
                    {
                        try
                        {
                            SPSite site = SPContext.Current.Site;
                            SPWeb web = site.OpenWeb(next);
                            SPListItem item = web.GetListItem(next);

                            
                            nextDT.Rows.Add(new object[] { item.Title, root.Title, item.Title });
                        }
                        catch (Exception ex)
                        {
                            Response.Write("Next" + next +" : "+ ex.ToString());
                        }
                    }                    //Response.Write(title + ";" + next + "</br>");
                }
                catch (Exception ex)
                {
                    Response.Write(ex.ToString());
                }
            }
        }

        private void GetPrevious()
        {
            DataQuery query = new DataQuery();
            query.Ids = "ga:"+google.Settings.Current.Id;
            query.Dimensions = "ga:previousPagePath";//,ga:nextPagePath";
            query.Metrics = "ga:pageviews";
            query.GAStartDate = DateTime.Now.AddMonths(-1).AddDays(-1).ToString("yyyy-MM-dd");
            query.GAEndDate = DateTime.Now.AddDays(-1).ToString("yyyy-MM-dd");

            if (google.Referringpage != null)
            {
                //query.Filters = "ga:previousPagePath==" + google.Referringpage;
                query.Filters = "ga:pagePath==" + google.Referringpage;
            }

            DataFeed actual = google.Analytics.Query(query);

            //dt.Columns.Add("Page Views per Visit", typeof(int));

            foreach (DataEntry entry in actual.Entries)
            {
                try
                {
                    string title = entry.Title.Text;//.Split(new char[] { '=' })[1].ToString();

                    string previous = entry.Dimensions[0].Value;
                    //string next = entry.Metrics[1].Value;//entry.Dimensions[1].Value;
                    //Response.Write("Previous :" + previous);

                    if (previous != google.Referringpage && !previous.Contains("_layouts") && !previous.Contains("Forms") && previous != "(entrance)")
                    {
                        try
                        {
                            //Response.Write("Previous :" + previous);
                            SPSite site = SPContext.Current.Site;
                            SPWeb web = site.OpenWeb(previous);
                            SPListItem item = web.GetListItem(previous);
                            previousDT.Rows.Add(new object[] {item.Title, "(entrance)", item.Title });
                            previousDT.Rows.Add(new object[] { root.Title, item.Title, root.Title });
                        }
                        catch (Exception ex)
                        {
                            Response.Write("Next" + previous + " : " + ex.ToString());
                        }
                    }                    //Response.Write(title + ";" + previous + "</br>");
                }
                catch (Exception ex)
                {
                    Response.Write(ex.ToString());
                }
            }
        }
    }
}
