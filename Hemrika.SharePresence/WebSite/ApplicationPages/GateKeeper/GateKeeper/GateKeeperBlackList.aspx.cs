using System;
using System.Globalization;
using System.Security.Permissions;
using System.Web.UI;
using Microsoft.SharePoint;
using Microsoft.SharePoint.Security;
using Microsoft.SharePoint.WebControls;
using System.Web.UI.WebControls;
using Hemrika.SharePresence.Common.UI;

namespace Hemrika.SharePresence.WebSite.GateKeeper
{
    [PermissionSetAttribute(SecurityAction.Demand, Name = "FullTrust")]
    [SharePointPermission(SecurityAction.LinkDemand, ObjectModel = true)]
    [SharePointPermission(SecurityAction.InheritanceDemand, ObjectModel = true)]
    public partial class GateKeeperBlacklist : EnhancedLayoutsPage
    {
        protected void Page_Load(object sender, EventArgs e)
        {
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
            return typeof(GateKeeperBlacklist);
        }
    }
}
