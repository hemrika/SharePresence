using System;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using Hemrika.SharePresence.WebSite.SiteMap;
using Microsoft.SharePoint;
using Hemrika.SharePresence.Common.UI;
using Microsoft.SharePoint.WebControls;

namespace Hemrika.SharePresence.WebSite.ControlTemplates
{
    public partial class ManageNavigation : UserControl
    {
        private SiteMapSettings settings;

        protected override void OnInit(EventArgs e)
        {
            base.OnInit(e);
            settings = new SiteMapSettings();
            settings = settings.Load();
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
            }
        }

        protected override void CreateChildControls()
        {
            if (!Page.ClientScript.IsClientScriptIncludeRegistered("jQuery"))
            {
                Page.ClientScript.RegisterClientScriptInclude(typeof(ManageNavigation), "jQuery", "/_layouts/Hemrika/Content/js/jquery.min.js");
            }

            if (!Page.ClientScript.IsClientScriptIncludeRegistered("Navigation"))
            {
                Page.ClientScript.RegisterClientScriptInclude(typeof(ManageNavigation), "Navigation", "/_layouts/Hemrika/Content/js/Navigation.js");
            }

            CssRegistration.Register("/_layouts/Hemrika/Content/css/Navigation.css");

            if (!Page.ClientScript.IsClientScriptBlockRegistered("NavigationInit"))
            {
                Page.ClientScript.RegisterClientScriptBlock(typeof(ManageNavigation), "NavigationInit", "", true);
            }

            base.CreateChildControls();
        }

        protected void btn_Save_Click(object sender, EventArgs e)
        {
            var eventArgsJavaScript = String.Format("{{Message:'{0}',controlIDs:window.frameElement.dialogArgs}}", "The Properties have been updated.");

            ((EnhancedLayoutsPage)Page).EndOperation(ModalDialogResult.OK, eventArgsJavaScript);
            Context.Response.Flush();
            Context.Response.End();
        }
    }
}
