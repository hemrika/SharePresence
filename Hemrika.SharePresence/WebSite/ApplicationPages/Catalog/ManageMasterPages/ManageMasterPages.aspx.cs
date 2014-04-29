// <copyright file="ManageMasterPage.cs" company="Hemrika">
// Copyright Hemrika. All rights reserved.
// </copyright>
// <author>MARKETING\Administrator</author>
// <date>2011-10-23 08:37:43Z</date>
namespace Hemrika.SharePresence.WebSite
{
    using System;
    using System.IO;
    using System.Security.Permissions;
    using System.Text;
    using System.Web;
    using System.Web.UI;
    using System.Web.UI.HtmlControls;
    using System.Web.UI.WebControls;
    using System.Xml;
    using System.Xml.Serialization;
    using Microsoft.SharePoint;
    using Microsoft.SharePoint.Security;
    using Microsoft.SharePoint.Utilities;
    using Microsoft.SharePoint.WebControls;
    using Hemrika.SharePresence.WebSite.Master;
    using Hemrika.SharePresence.Common.ServiceLocation;
    using Hemrika.SharePresence.Common;
    using Hemrika.SharePresence.Common.Configuration;
    using Hemrika.SharePresence.Common.UI;
    using Hemrika.SharePresence.Common.Ribbon.Definitions;
    using Hemrika.SharePresence.Common.Ribbon.Libraries;
    using Hemrika.SharePresence.Common.Ribbon.Definitions.Controls;
    using System.Collections.Generic;

    [PermissionSetAttribute(SecurityAction.Demand, Name = "FullTrust")]
    [SharePointPermission(SecurityAction.LinkDemand, ObjectModel = true)]
    [SharePointPermission(SecurityAction.InheritanceDemand, ObjectModel = true)]
    public class ManageMasterPages : EnhancedLayoutsPage
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
            return typeof(ManageMasterPages);
        }
    }
}

