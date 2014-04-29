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
using Hemrika.SharePresence.WebSite.Modules.GateKeeper;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using Microsoft.SharePoint.Linq;
using Microsoft.SharePoint;

namespace Hemrika.SharePresence.WebSite.ControlTemplates
{

    public partial class DroneBL : System.Web.UI.UserControl
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            gridDroneBL.RowDataBound += new GridViewRowEventHandler(gridDroneBL_RowDataBound);

            if (!Page.IsPostBack)
            { LoadData(); }
        }


        void gridDroneBL_RowDataBound(object sender, GridViewRowEventArgs e)
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
            try
            {
                List<droneBL> entries = new List<droneBL>();

                List<SPListItem> items = GateKeeperModule.GetGateKeeperItems(GateKeeperType.Drone);

                foreach (SPListItem item in items)
                {
                    try
                    {

                        droneBL drone = new droneBL();
                        drone.ID = item.ID.ToString();
                        drone.Date = DateTime.Parse(item["GateKeeper_Date"].ToString());

                        try
                        {
                            drone.IPAddress = item["GateKeeper_IPAddress"].ToString();
                        }
                        catch { };
                        try
                        {
                            drone.Referrer = item["GateKeeper_Referrer"].ToString();
                        }
                        catch { };
                        try
                        {
                            drone.UserAgent = item["GateKeeper_Useragent"].ToString();
                        }
                        catch { };
                        try
                        {
                            drone.Comment = item["GateKeeper_Comment"].ToString();
                        }
                        catch { };

                        entries.Add(drone);
                    }
                    catch (Exception ex)
                    {
                        ex.ToString();
                    }

                }

                gridDroneBL.DataSource = entries;
                gridDroneBL.DataBind();
            }
            catch (Exception)
            {
                return;
            }
        }
    }
}