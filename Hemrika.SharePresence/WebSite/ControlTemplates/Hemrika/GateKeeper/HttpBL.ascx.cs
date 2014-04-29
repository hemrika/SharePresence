using System;
using System.Collections;
using System.Configuration;
using System.Data;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Xml;
using System.Linq;
using Hemrika.SharePresence.WebSite.Modules.GateKeeper;
using System.Collections.Generic;
using System.Globalization;
using Microsoft.SharePoint;

namespace Hemrika.SharePresence.WebSite.ControlTemplates
{
    public partial class HttpBL : System.Web.UI.UserControl
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            gridHttpBL.RowDataBound += new GridViewRowEventHandler(gridHttpBL_RowDataBound);

            if (!Page.IsPostBack)
            { LoadData(); }
        }


        void gridHttpBL_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            // Dynamically hide the ID field so it's in the client data but not displayed
            e.Row.Cells[0].Visible = false; // Hide entryID

            // Dynamically add confirmation to delete button
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                Button btn = (Button)e.Row.FindControl("btnDelete");
                if (btn != null) { btn.Attributes.Add("onClick", "return confirm('Are you sure you want to delete this entry?');"); }
            }
        }

        /// <summary>
        /// Binds to selected row and sends the id cell (0) to
        /// Utils.DeleteEntry for deletion based in the entryID
        /// </summary>
        protected void btnDelete_Click(object sender, EventArgs e)
        {
            Button btn = (Button)sender;
            GridViewRow grdRow = (GridViewRow)btn.Parent.Parent;

            int id = int.Parse(grdRow.Cells[0].Text);

            GateKeeperModule.DeleteGateKeeperItem(id);

            Response.Redirect(Request.RawUrl, true);
        }

        void LoadData()
        {
            List<httpBL> entries = new List<httpBL>();

            List<SPListItem> items = GateKeeperModule.GetGateKeeperItems(GateKeeperType.HTTP);

            foreach (SPListItem item in items)
            {
                try
                {

                    httpBL http = new httpBL();
                    http.ID = item.ID.ToString();
                    http.Date = DateTime.Parse(item["GateKeeper_Date"].ToString());
                    try
                    {
                        http.IPAddress = item["GateKeeper_IPAddress"].ToString();
                    }
                    catch { };
                    try
                    {
                        http.Referrer = item["GateKeeper_Referrer"].ToString();
                    }
                    catch { };
                    try
                    {
                        http.UserAgent = item["GateKeeper_Useragent"].ToString();
                    }
                    catch { };
                    try
                    {
                        http.LastActivity = item["GateKeeper_LastActivity"].ToString();
                    }
                    catch { };
                    try
                    {
                        http.ThreatLevel = item["GateKeeper_Threatlevel"].ToString();
                    }
                    catch { };
                    try
                    {
                        http.VisitorType = item["GateKeeper_Visitortype"].ToString();
                    }
                    catch { };
                    try
                    {
                        http.Comment = item["GateKeeper_Comment"].ToString();
                    }
                    catch { };

                    entries.Add(http);
                }
                catch (Exception ex)
                {
                    ex.ToString();
                }

            }

            gridHttpBL.DataSource = entries;
            gridHttpBL.DataBind();
        }
    }
}