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
    public partial class ProxyBL : System.Web.UI.UserControl
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            gridProxyBL.RowDataBound += new GridViewRowEventHandler(gridProxyBL_RowDataBound);

            if (!Page.IsPostBack)
            { LoadData(); }
        }

        void gridProxyBL_RowDataBound(object sender, GridViewRowEventArgs e)
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
                List<proxyBL> entries = new List<proxyBL>();

                List<SPListItem> items = GateKeeperModule.GetGateKeeperItems(GateKeeperType.Proxy);

                foreach (SPListItem item in items)
                {
                    try
                    {

                        proxyBL proxy = new proxyBL();
                        proxy.ID = item.ID.ToString();
                        proxy.Date = DateTime.Parse(item["GateKeeper_Date"].ToString());
                        try
                        {
                            proxy.IPAddress = item["GateKeeper_IPAddress"].ToString();
                        }
                        catch { };
                        try
                        {
                            proxy.Referrer = item["GateKeeper_Referrer"].ToString();
                        }
                        catch { };
                        try
                        {
                            proxy.UserAgent = item["GateKeeper_Useragent"].ToString();
                        }
                        catch { };
                        try
                        {
                            proxy.Comment = item["GateKeeper_Comment"].ToString();
                        }
                        catch { };

                        entries.Add(proxy);
                    }
                    catch (Exception ex)
                    {
                        ex.ToString();
                    }

                }

                gridProxyBL.DataSource = entries;
                gridProxyBL.DataBind();
            }
            catch (Exception)
            {
                return;
            }


        }
    }
}