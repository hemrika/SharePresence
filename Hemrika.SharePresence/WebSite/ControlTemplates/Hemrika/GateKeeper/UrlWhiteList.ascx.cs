﻿using System;
using System.Collections.Generic;
using System.Globalization;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Xml;
using System.Linq;
using Hemrika.SharePresence.WebSite.Modules.GateKeeper;
using Microsoft.SharePoint;
using System.ServiceModel;

namespace Hemrika.SharePresence.WebSite.ControlTemplates
{
    public partial class UrlWhiteList : System.Web.UI.UserControl
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            gridUrl.RowDataBound += new GridViewRowEventHandler(gridUserAgent_RowDataBound);
            btnAdd.Click += new EventHandler(btnAdd_Click);

            if (!Page.IsPostBack)
            { LoadData(); }
        }

        void gridUserAgent_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            // Dynamically hide the ID field so it's in the client data but not displayed
            e.Row.Cells[0].Visible = false;
            e.Row.Cells[1].Visible = false;

            // Dynamically add confirmation to delete button
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                Button btn = (Button)e.Row.FindControl("btnDelete");
                if (btn != null)
                    btn.Attributes.Add("onClick", "return confirm('Are you sure you want to delete this entry?');");
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

        protected void btnAdd_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(txtUrl.Text)) { return; }

            string currUrl = Request.Url.ToString();
            string basePath = currUrl.Substring(0, currUrl.IndexOf(Request.Url.Host) + Request.Url.Host.Length);


            // Set up proxy.
            BasicHttpBinding binding = new BasicHttpBinding();
            binding.Security.Mode = BasicHttpSecurityMode.TransportCredentialOnly;
            binding.Security.Transport.ClientCredentialType = HttpClientCredentialType.Ntlm;
            EndpointAddress endpoint = new EndpointAddress(basePath + "/_vti_bin/SharePresence/GateKeeperService.svc");

            GateKeeperService.GateKeeperServiceClient service = new GateKeeperService.GateKeeperServiceClient(binding, endpoint);
            service.ClientCredentials.Windows.AllowedImpersonationLevel = System.Security.Principal.TokenImpersonationLevel.Impersonation;

            service.GateKeeper(GateKeeperService.GateKeeperType.White, GateKeeperService.GateKeeperListing.GateKeeper_Url, txtUrl.Text);

            Response.Redirect(Request.RawUrl, true);
        }

        void LoadData()
        {
            List<SafeUrl> entries = new List<SafeUrl>();

            List<SPListItem> items = GateKeeperModule.GetGateKeeperItems(GateKeeperType.White, GateKeeperListing.GateKeeper_Url);

            foreach (SPListItem item in items)
            {
                try
                {

                    SafeUrl safeurl = new SafeUrl();
                    safeurl.ID = item.ID.ToString();
                    safeurl.Date = DateTime.Parse(item["GateKeeper_Date"].ToString());
                    try
                    {
                        safeurl.Url = item["GateKeeper_Url"].ToString();
                    }
                    catch { };
                    try
                    {
                        safeurl.Comment = item["GateKeeper_Comment"].ToString();
                    }
                    catch { };

                    entries.Add(safeurl);
                }
                catch (Exception ex)
                {
                    ex.ToString();
                }

            }

            entries.Sort(SafeUrl.UrlComparison);
            gridUrl.DataSource = entries;
            gridUrl.DataBind();
        }
    }
}