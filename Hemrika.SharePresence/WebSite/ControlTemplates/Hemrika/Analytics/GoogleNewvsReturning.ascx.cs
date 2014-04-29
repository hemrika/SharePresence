using System;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using Microsoft.SharePoint;
using Hemrika.SharePresence.Google.Analytics;
using System.Collections;

namespace Hemrika.SharePresence.WebSite.ControlTemplates
{
    public partial class GoogleNewvsReturning : UserControl
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            GooglePageBase google = Page as GooglePageBase;

            DataQuery query = new DataQuery();
            query.Ids = "ga:"+google.Settings.Current.Id;
            query.Metrics = "ga:visits";
            query.Dimensions = "ga:visitorType,ga:date";
            query.Sort = "ga:date";
            query.GAStartDate = DateTime.Now.AddMonths(-1).AddDays(-1).ToString("yyyy-MM-dd");
            query.GAEndDate = DateTime.Now.AddDays(-1).ToString("yyyy-MM-dd");

            if (google.Referringpage != null)
            {
                query.Filters = "ga:pagePath==" + google.Referringpage;
            }

            DataFeed actual = google.Analytics.Query(query);

            System.Data.DataTable nvsr = new System.Data.DataTable("New vs Returning");
            nvsr.Columns.Add("visitorType", typeof(string));
            nvsr.Columns.Add("Visits", typeof(int));

            System.Collections.SortedList visitors = new System.Collections.SortedList();

            System.Data.DataTable nandr = new System.Data.DataTable("New and Returning");
            nandr.Columns.Add("Date");
            nandr.Columns.Add("New Visitor", typeof(int));
            nandr.Columns.Add("Returning Visitor", typeof(int));

            System.Collections.SortedList newvisitors = new System.Collections.SortedList();
            System.Collections.SortedList returningvisitors = new System.Collections.SortedList();

            foreach (DataEntry entry in actual.Entries)
            {
                try
                {
                    int visits = int.Parse(entry.Metrics[0].Value);
                    string visitorType = entry.Dimensions[0].Value.ToString();
                    DateTime datetime = new DateTime(int.Parse(entry.Dimensions[1].Value.Substring(0, 4)), int.Parse(entry.Dimensions[1].Value.Substring(4, 2)), int.Parse(entry.Dimensions[1].Value.Substring(6, 2)));
                    string date = datetime.ToString("yyyy-MM-dd");
                    //dt.Rows.Add(new object[] { visitorType,visits });

                    if (visitorType.StartsWith("New"))
                    {
                        if (newvisitors.ContainsKey(date))
                        {
                            int current = int.Parse(newvisitors[date].ToString());
                            current += visits;
                            newvisitors[date] = current;
                        }
                        else
                        {
                            newvisitors.Add(date, visits);
                        }
                    }
                    else
                    {
                        if (returningvisitors.ContainsKey(date))
                        {
                            int current = int.Parse(returningvisitors[date].ToString());
                            current += visits;
                            returningvisitors[date] = current;
                        }
                        else
                        {
                            returningvisitors.Add(date, visits);
                        }
                    }

                    if (visitors.ContainsKey(visitorType))
                    {
                        int current = int.Parse(visitors[visitorType].ToString());
                        current += visits;
                        visitors[visitorType] = current;
                    }
                    else
                    {
                        visitors.Add(visitorType, visits);
                    }

                }
                catch (Exception ex)
                {
                    Response.Write(ex.ToString());
                }
            }

            foreach (String key in visitors.Keys)
            {
                nvsr.Rows.Add(new object[] { key, int.Parse(visitors[key].ToString()) });
            }

            ArrayList keys = new ArrayList();
            keys.AddRange(newvisitors.Keys);
            keys.AddRange(returningvisitors.Keys);

            foreach (object key in keys)
            {
                int _newvisitors = 0;
                int _returningvisitors = 0;

                if (newvisitors.ContainsKey(key))
                {
                    _newvisitors = int.Parse(newvisitors[key].ToString());
                }
                if (returningvisitors.ContainsKey(key))
                {
                    _returningvisitors = int.Parse(returningvisitors[key].ToString());
                }

                nandr.Rows.Add(new object[] { key, _newvisitors, _returningvisitors });
            }

            this.pie_newvsreturning.GviEnableEvents = true;
            this.pie_newvsreturning.ChartData(nvsr);

            this.area_newvsreturning.GviEnableEvents = true;
            this.area_newvsreturning.ChartData(nandr);

        }
    }
}
