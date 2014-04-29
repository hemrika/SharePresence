// -----------------------------------------------------------------------
// <copyright file="LegacyControls.cs" company="">
// TODO: Update copyright text.
// </copyright>
// -----------------------------------------------------------------------

namespace Hemrika.SharePresence.WebSite.Controls
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Web.UI.WebControls;
    using System.Web.UI;
    using Microsoft.SharePoint;
    using Hemrika.SharePresence.WebSite.Page;
    using System.Web;
    using System.Security.Permissions;
    using Microsoft.SharePoint.Administration;

    /// <summary>
    /// TODO: Update summary.
    /// </summary>
    [PermissionSet(SecurityAction.Demand, Name = "FullTrust")]
    [ToolboxData("<{0}:LegacyControls runat=server></{0}:LegacyControls>")]
    public class LegacyControls : PlaceHolder
    {
        protected override void AddedControl(Control control, int index)
        {
            //base.AddedControl(control, index);

            if ((SPContext.Current.Item is SPListItem) && (WebSitePage.IsWebSitePage(SPContext.Current.Item as SPListItem)))
            {
                this.Controls.Clear();
                return;
            }

            SPListItem item = null;
            try
            {
                if (!HttpContext.Current.Request.Url.AbsolutePath.StartsWith("/_layouts/"))
                {
                    item = SPContext.Current.Web.GetListItem(HttpContext.Current.Request.Url.AbsolutePath);
                }
            }
            catch (Exception ex)
            {
                SPDiagnosticsService.Local.WriteTrace(0, new SPDiagnosticsCategory(ex.Source, TraceSeverity.High, EventSeverity.Error), TraceSeverity.High, ex.Message, ex.Data);
                item = null;
            }
            if ((item != null) && ((item is SPListItem) && WebSitePage.IsWebSitePage(item)))
            {
                this.Controls.Clear();
                return;
            }
        }

        protected override void Render(HtmlTextWriter writer)
        {
            //base.Render(writer);
        }

        protected override void RenderChildren(HtmlTextWriter writer)
        {
            //base.RenderChildren(writer);
        }

        public override void RenderControl(HtmlTextWriter writer)
        {
            //base.RenderControl(writer);
        }
    }
}
