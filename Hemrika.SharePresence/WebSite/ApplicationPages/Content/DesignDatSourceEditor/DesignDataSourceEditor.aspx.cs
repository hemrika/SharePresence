// <copyright file="DesignDatSourceEditor.cs" company="SharePresence">
// Copyright SharePresence. All rights reserved.
// </copyright>
// <author>MARKETING\Administrator</author>
// <date>2012-09-21 21:39:17Z</date>
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
    using System.Collections.Generic;
    using Hemrika.SharePresence.WebSite.WebParts;
    using Hemrika.SharePresence.Common.TemplateEngine;

    /// <summary>
    /// TODO: Add comment for DesignDatSourceEditor
    /// </summary>
    [SharePointPermission(SecurityAction.InheritanceDemand, ObjectModel = true)]
    public class DesignDataSourceEditor : EnhancedLayoutsPage
    {
        private readonly HiddenField _hiddenSourceValue;
        /// <summary>
        /// Initializes a new instance of the DesignDatSourceEditor class
        /// </summary>
        public DesignDataSourceEditor()
        {
            this.RightsCheckMode = RightsCheckModes.OnPreInit;
            _hiddenSourceValue = new HiddenField { ID = "designdatasourceedit" };
        }

        protected override void CreateChildControls()
        {
            Form.Controls.Add(_hiddenSourceValue);
            base.CreateChildControls();
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

        public override Type SetTypeForLicense()
        {
            return typeof(DesignDataSourceEditor);
        }

        public override Common.Ribbon.Definitions.TabDefinition GetTabDefinition()
        {
            return null;
        }
    }
}

