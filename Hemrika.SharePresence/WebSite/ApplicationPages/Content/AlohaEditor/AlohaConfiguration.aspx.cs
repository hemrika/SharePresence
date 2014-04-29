// <copyright file="CreateWebPage.cs" company="Hemrika">
// Copyright Hemrika. All rights reserved.
// </copyright>
// <author>MARKETING\Administrator</author>
// <date>2011-10-22 19:23:00Z</date>
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
    using Hemrika.SharePresence.WebSite.Layout;
    using Hemrika.SharePresence.Common.ServiceLocation;
    using Hemrika.SharePresence.Common.UI;

    /// <summary>
    /// TODO: Add comment for CreateWebPage
    /// </summary>
    [SharePointPermission(SecurityAction.InheritanceDemand, ObjectModel = true)]
    public class AlohaConfiguration : UnsecuredLayoutsPageBase
    {
        /// <summary>
        /// Initializes a new instance of the CreateWebPage class
        /// </summary>
        public AlohaConfiguration()
        {
        }

        /// <summary>
        /// Sets the inital values of controls
        /// </summary>
        /// <param name="e">Event arguments</param>
        protected override void OnLoad(EventArgs e)
        {
            base.OnLoad(e);
            this.Response.ContentType = "application/x-javascript";
        }
    }
}

