using System;
using System.Linq;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Collections.Generic;
using Microsoft.SharePoint;
using Hemrika.SharePresence.WebSite.MetaData;
using Hemrika.SharePresence.Common;
using Hemrika.SharePresence.Common.UI;

namespace Hemrika.SharePresence.WebSite.ControlTemplates
{
    public partial class ManageKeywordSettings : UserControl
    {
        //private IEnumerable<SPField> metafields;
        //private IEnumerable<SPContentType> publishingtypes;
        private KeywordSettings settings;

        protected override void OnInit(EventArgs e)
        {
            base.OnInit(e);
            settings = new KeywordSettings();
            settings = settings.Load(SPContext.Current.Site);
            btn_Save.Click += new EventHandler(btn_Save_Click);
        }

        void btn_Save_Click(object sender, EventArgs e)
        {
            settings = settings.Save(SPContext.Current.Site);

            var eventArgsJavaScript = String.Format("{{Message:'{0}',controlIDs:window.frameElement.dialogArgs}}", "The Properties have been updated.");

            ((EnhancedLayoutsPage)Page).EndOperation(ModalDialogResult.OK, eventArgsJavaScript);
            Context.Response.Flush();
            Context.Response.End();

        }

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
            }

            gridKeywords.RowDataBound += new GridViewRowEventHandler(gridKeywords_RowDataBound);

            //List<string> entries = new List<SemanticUrl>();
        }

        void gridKeywords_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            // Dynamically hide the ID field so it's in the client data but not displayed
            e.Row.Cells[0].Visible = false;

            // Dynamically add confirmation to delete button
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                Button btn = (Button)e.Row.FindControl("btnEdit");
                if (btn != null)
                {
                    btn.Attributes.Add("onClick", "return confirm('Are you sure you want to delete this entry?');");
                }
            }

        }

        protected void btnEdit_Click(object sender, EventArgs e)
        {
            //pnl_wait.Attributes["style"] = "";
            Button btn = (Button)sender;
            GridViewRow grdRow = (GridViewRow)btn.Parent.Parent;

            Guid id = new Guid(grdRow.Cells[0].Text);

            SPSite site = SPContext.Current.Site;

            //Response.Redirect(Request.RawUrl, true);
        }


    }
}
