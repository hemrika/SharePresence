using System;
using System.Security.Permissions;
using Microsoft.SharePoint;
using Microsoft.SharePoint.Security;
using Microsoft.SharePoint.WebControls;
using Hemrika.SharePresence.Common.UI;

namespace Hemrika.SharePresence.WebSite.MetaData
{
    [PermissionSetAttribute(SecurityAction.Demand, Name = "FullTrust")]
    [SharePointPermission(SecurityAction.LinkDemand, ObjectModel = true)]
    [SharePointPermission(SecurityAction.InheritanceDemand, ObjectModel = true)]
    public partial class MetaDataManage : EnhancedLayoutsPage
    {
        protected void Page_Load(object sender, EventArgs e)
        {
        }

        public override Common.Ribbon.Definitions.TabDefinition GetTabDefinition()
        {
            return null;
        }

        public override Type SetTypeForLicense()
        {
            return typeof(MetaDataManage);
        }
    }
}
