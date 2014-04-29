// <copyright file="CreatePageLayout.cs" company="Hemrika">
// Copyright Hemrika. All rights reserved.
// </copyright>
// <author>MARKETING\Administrator</author>
// <date>2012-01-11 15:07:29Z</date>
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
    using Hemrika.SharePresence.Common.UI;

    /// <summary>
    /// TODO: Add comment for CreatePageLayout
    /// </summary>
    [SharePointPermission(SecurityAction.InheritanceDemand, ObjectModel = true)]
    public class CreatePageLayout : EnhancedLayoutsPage
    {
        /// <summary>
        /// Initializes a new instance of the CreatePageLayout class
        /// </summary>
        public CreatePageLayout()
        {
            this.RightsCheckMode = RightsCheckModes.OnPreInit;
        }


        /// <summary>
        /// Defines which rights are required
        /// </summary>
        protected override SPBasePermissions RightsRequired
        {
            get
            {
                return base.RightsRequired | SPBasePermissions.BrowseUserInfo | SPBasePermissions.ManageLists;
            }
        }


        /// <summary>
        /// Sets the inital values of controls
        /// </summary>
        /// <param name="e">Event arguments</param>
        protected override void OnLoad(EventArgs e)
        {
            SPSite siteCollection = this.Site;
            SPWeb site = this.Web;

            //Control _control = this.LoadControl(_ascxPath);
            //ControlCollection controls = this.Controls;

            //foreach (Control control in controls)
            //{
            //    Page.Response.Write(control.ID.ToString()+";");
            //}

            ////Control main = this.Page.FindControl("Main");

            ////main.Controls.Add(_control);

        }


        /*
        Use this code to perform own security checks
        protected virtual void CheckCustomRights()
        {

          bool userCheckedForCustomLogic = false;
          //write here a custom logic to check if user has enough rights to access application page
          //if yes, set userCheckedForCustomLogic to true;

          if (!userCheckedForCustomLogic)
          {
            SPUtility.HandleAccessDenied(new UnauthorizedAccessException());
          }
        } 

        protected override void OnLoad(EventArgs e)
        {
          this.CheckCustomRights();   
        }
        */

        public override Common.Ribbon.Definitions.TabDefinition GetTabDefinition()
        {
            return null;
        }

        public override Type SetTypeForLicense()
        {
            return typeof(CreatePageLayout);
        }
    }
}

