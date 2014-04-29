using System;
using System.Globalization;
using System.Security.Permissions;
using System.Web.UI;
using Microsoft.SharePoint;
using Microsoft.SharePoint.Security;
using Microsoft.SharePoint.WebControls;
using System.Web.UI.WebControls;
using Hemrika.SharePresence.Common.UI;

namespace Hemrika.SharePresence.WebSite.Bing
{
    [PermissionSetAttribute(SecurityAction.Demand, Name = "FullTrust")]
    [SharePointPermission(SecurityAction.LinkDemand, ObjectModel = true)]
    [SharePointPermission(SecurityAction.InheritanceDemand, ObjectModel = true)]
    public partial class BingManage : EnhancedLayoutsPage
    {
        BingSettings settings = null;

        protected void Page_Load(object sender, EventArgs e)
        {
            if (settings == null)
            {
                settings = new BingSettings();
                settings = settings.Load();
            }

            btn_Save.Click += new EventHandler(btn_Save_Click);
            btn_Cancel.Click += new EventHandler(btn_Cancel_Click);
            

            if (!Page.IsPostBack)
            {
                tbx_BingUser.Text = settings.User;
                tbx_BingAPI.Text = settings.API;
            }
        }

        void btn_Cancel_Click(object sender, EventArgs e)
        {
            ((EnhancedLayoutsPage)Page).EndOperation(ModalDialogResult.Cancel);
        }

        void btn_Save_Click(object sender, EventArgs e)
        {
            settings.User = tbx_BingUser.Text;
            settings.API = tbx_BingAPI.Text;
            settings.Save();

            var eventArgsJavaScript = String.Format("{{Message:'{0}'}}", "The Properties have been updated.");

            ((EnhancedLayoutsPage)Page).EndOperation(ModalDialogResult.OK, eventArgsJavaScript);
            Context.Response.Flush();
            Context.Response.End();
        }

        /*
        protected override bool RequireSiteAdministrator
        {
            get
            {
                return true;
            }
        }
        */

        public override Common.Ribbon.Definitions.TabDefinition GetTabDefinition()
        {
           return null;
        }

        public override Type SetTypeForLicense()
        {
            return typeof(BingManage);
        }
    }
}
