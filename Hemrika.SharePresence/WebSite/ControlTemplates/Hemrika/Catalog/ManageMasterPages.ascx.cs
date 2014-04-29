using System;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using Microsoft.SharePoint;
using System.Collections;
using Microsoft.SharePoint.Utilities;
using System.IO;
using System.Text;
using Hemrika.SharePresence.Common;
using Hemrika.SharePresence.WebSite.ContentTypes;
using Hemrika.SharePresence.Common.UI;
using Hemrika.SharePresence.WebSite.Master;
using Hemrika.SharePresence.Common.ServiceLocation;
using System.Collections.Generic;

namespace Hemrika.SharePresence.WebSite.ControlTemplates
{
    public partial class ManageMasterPages : UserControl
    {
        private IServiceLocator serviceLocator;
        private IMasterPageRepository serviceMasterPages;
        private string cmurl;
        private string murl;
        private Guid original;
        private Guid current;

        public ManageMasterPages()
        {
            serviceLocator = SharePointServiceLocator.GetCurrent();
            serviceMasterPages = serviceLocator.GetInstance<IMasterPageRepository>();
            cmurl = SPContext.Current.Site.RootWeb.CustomMasterUrl;
            murl = SPContext.Current.Site.RootWeb.MasterUrl;
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            gridMasterPages.RowDataBound += new GridViewRowEventHandler(gridMasterPages_RowDataBound);
            string name = murl.Substring(murl.LastIndexOf('/'));
            name = name.Trim(new char[1] { '/' });

            if (!IsPostBack)
            {
                original = serviceMasterPages.GetMasterPage(name).UniqueId;
                current = serviceMasterPages.GetMasterPage(name).UniqueId;
                SetMasterPagesGrid();
            }
            else
            {
                current = serviceMasterPages.GetMasterPage(name).UniqueId;
            }

        }

        private void SetMasterPagesGrid()
        {
            try
            {
                List<Hemrika.SharePresence.WebSite.Master.MasterPage> masters = serviceMasterPages.GetMasterPages();

                //foreach (Hemrika.SharePresence.WebSite.Master.MasterPage master in masters)
                for (int i = 0; i < masters.Count; i++)
                {
                    Hemrika.SharePresence.WebSite.Master.MasterPage master = masters[i];
                    if (master.Name == "minimal.master" || master.Name == "default.master" || master.Name == "v4.master")
                    {
                        masters.Remove(master);
                    }
                }

                if (masters.Count > 0)
                {
                    gridMasterPages.DataSource = masters;
                    gridMasterPages.DataBind();
                }
            }
            catch (Exception ex)
            {
                ex.ToString();
            }
        }

        void gridMasterPages_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            e.Row.Cells[0].Visible = false;

            // Dynamically add confirmation to delete button
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                Guid id = ((Hemrika.SharePresence.WebSite.Master.MasterPage)e.Row.DataItem).UniqueId;
                Button btn = (Button)e.Row.FindControl("btnUse");
                if (btn != null)
                {
                    if (current != id)
                    {
                        btn.Attributes.Add("onClick", "return confirm('Are you sure you want to use this MasterPage?');");
                    }
                    else
                    {
                        btn.Enabled = false;
                        btn.Text = "Current";
                    }
                }
            }
        }

        /// <summary>
        /// Binds to selected row and sends the id cell (0) to
        /// Utils.DeleteEntry for deletion based in the entryID
        /// </summary>
        protected void btnUse_Click(object sender, EventArgs e)
        {
            Button btn = (Button)sender;
            GridViewRow grdRow = (GridViewRow)btn.Parent.Parent;
            Guid id = new Guid(grdRow.Cells[0].Text);

            Hemrika.SharePresence.WebSite.Master.MasterPage master = serviceMasterPages.GetMasterPage(id);
            //if (original != current)
            //{
                if (master != null && master.Url.Length > 0)
                {
                    try
                    {
                        SPWeb web = SPContext.Current.Site.RootWeb;
                        web.CustomMasterUrl = "/" + master.Url;
                        web.MasterUrl = "/" + master.Url;
                        web.Update();
                        current = master.UniqueId;
                    }
                    catch (Exception ex)
                    {
                        ex.ToString();
                    }
                    finally
                    {
                        SetMasterPagesGrid();
                    }
                }
            //}
        }

        protected void btn_Save_Click(object sender, EventArgs e)
        {
            ((EnhancedLayoutsPage)Page).PageToRedirectOnOK = "/_layouts/Settings.aspx";
            ((EnhancedLayoutsPage)Page).EndOperation(ModalDialogResult.OK, "/_layouts/Settings.aspx");
            
        }
    }
}
