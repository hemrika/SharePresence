using System;
using System.Collections;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Globalization;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Xml;
using System.Linq;
using Hemrika.SharePresence.WebSite.Modules.GateKeeper;
using Microsoft.SharePoint;

namespace Hemrika.SharePresence.WebSite.ControlTemplates
{
    public partial class Honeypot : System.Web.UI.UserControl
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            gridHoneyPot.RowDataBound += new GridViewRowEventHandler(gridHoneyPot_RowDataBound);

            if (!Page.IsPostBack)
            { LoadData(); }
        }

        void gridHoneyPot_RowDataBound(object sender, GridViewRowEventArgs e)
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
            List<honeyPot> entries = new List<honeyPot>();

            List<SPListItem> items = GateKeeperModule.GetGateKeeperItems(GateKeeperType.HoneyPot);

            foreach (SPListItem item in items)
            {
                try
                {

                    honeyPot honeypot = new honeyPot();
                    honeypot.ID = item.ID.ToString();
                    honeypot.Date = DateTime.Parse(item["GateKeeper_Date"].ToString());

                    try
                    {
                        honeypot.IPAddress = item["GateKeeper_IPAddress"].ToString();
                    }
                    catch { };
                    try
                    {
                        honeypot.Referrer = item["GateKeeper_Referrer"].ToString();
                    }
                    catch { };

                    try
                    {
                        honeypot.UserAgent = item["GateKeeper_Useragent"].ToString();
                    }
                    catch { };

                    try
                    {
                        honeypot.Comment = item["GateKeeper_Comment"].ToString();
                    }
                    catch { };

                    entries.Add(honeypot);
                }
                catch (Exception ex)
                {
                    ex.ToString();
                }

            }

            gridHoneyPot.DataSource = entries;
            gridHoneyPot.DataBind();
        }




    }
}