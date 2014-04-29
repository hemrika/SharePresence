using System;
using System.Collections.Generic;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Xml;
using System.Linq;
using Hemrika.SharePresence.WebSite.Modules.GateKeeper;
using System.Globalization;
using Microsoft.SharePoint;
using System.ServiceModel;

namespace Hemrika.SharePresence.WebSite.ControlTemplates
{
    public partial class IPWhiteList : UserControl
    {
        protected override void OnInit(EventArgs e)
        {
            base.OnInit(e);
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            gridIPAddress.RowDataBound += new GridViewRowEventHandler(gridIPAddress_RowDataBound);
            btnAdd.Click += new EventHandler(btnAdd_Click);

            if (!Page.IsPostBack)
            { LoadData(); }
        }

        void gridIPAddress_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            // Dynamically hide the ID field and date so it's in the client data but not displayed
            e.Row.Cells[0].Visible = false;
            e.Row.Cells[1].Visible = false;

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

        /// <summary>
        /// Adds new entry to the records list
        /// </summary>
        protected void btnAdd_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(txtIPAddress.Text)) { return; }

            string currUrl = Request.Url.ToString();
            string basePath = currUrl.Substring(0, currUrl.IndexOf(Request.Url.Host) + Request.Url.Host.Length);


            // Set up proxy.
            BasicHttpBinding binding = new BasicHttpBinding();
            binding.Security.Mode = BasicHttpSecurityMode.TransportCredentialOnly;
            binding.Security.Transport.ClientCredentialType = HttpClientCredentialType.Ntlm;
            EndpointAddress endpoint = new EndpointAddress(basePath + "/_vti_bin/SharePresence/GateKeeperService.svc");

            GateKeeperService.GateKeeperServiceClient service = new GateKeeperService.GateKeeperServiceClient(binding, endpoint);
            service.ClientCredentials.Windows.AllowedImpersonationLevel = System.Security.Principal.TokenImpersonationLevel.Impersonation;

            service.GateKeeper(GateKeeperService.GateKeeperType.White, GateKeeperService.GateKeeperListing.GateKeeper_IPAddress, txtIPAddress.Text);

            Response.Redirect(Request.RawUrl, true);
        }

        void LoadData()
        {
            List<ipAddress> entries = new List<ipAddress>();

            List<SPListItem> items = GateKeeperModule.GetGateKeeperItems(GateKeeperType.White, GateKeeperListing.GateKeeper_IPAddress);

            foreach (SPListItem item in items)
            {
                try
                {

                    ipAddress address = new ipAddress();
                    address.ID = item.ID.ToString();
                    address.Date = DateTime.Parse(item["GateKeeper_Date"].ToString());
                    try
                    {
                        address.IPAddress = item["GateKeeper_IPAddress"].ToString();
                    }
                    catch { };

                    try
                    {
                        address.Comment = item["GateKeeper_Comment"].ToString();
                    }
                    catch { };

                    entries.Add(address);
                }
                catch (Exception ex)
                {
                    ex.ToString();
                }

            }

            entries.Sort(ipAddress.IPComparison);
            gridIPAddress.DataSource = entries;
            gridIPAddress.DataBind();
        }
    }
}