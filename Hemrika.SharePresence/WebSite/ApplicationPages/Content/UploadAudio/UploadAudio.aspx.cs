// <copyright file="UploadContent.cs" company="Hemrika">
// Copyright Hemrika. All rights reserved.
// </copyright>
// <author>MARKETING\Administrator</author>
// <date>2012-02-13 13:06:50Z</date>
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

    /// <summary>
    /// TODO: Add comment for UploadContent
    /// </summary>
    [SharePointPermission(SecurityAction.InheritanceDemand, ObjectModel = true)]
    public class UploadAudio : LayoutsPageBase
    {
        /// <summary>
        /// Initializes a new instance of the UploadContent class
        /// </summary>
        public UploadAudio()
        {
            this.RightsCheckMode = RightsCheckModes.OnPreInit;
        }

        /// <summary>
        /// Gets or sets the pointer to the TextInputBox on the page 
        /// </summary>
        protected InputFormTextBox TxtInput
        {
            get;
            set;
        }

        /// <summary>
        /// Gets or sets the pointer to the Ok button on the page
        /// </summary>
        protected Button BtnOk
        {
            get;
            set;
        }

        /// <summary>
        /// Gets or sets the pointer to the Cancel button on the page
        /// </summary>
        protected Button BtnCancel
        {
            get;
            set;
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

            this.BtnOk.Click += new EventHandler(this.BtnOk_Click);
            this.BtnCancel.Click += new EventHandler(this.BtnCancel_Click);
        }

        /// <summary>
        /// TODO: Add comment
        /// </summary>
        /// <param name="sender">Sender of the event</param>
        /// <param name="e">Arguments of the event</param>
        private void BtnCancel_Click(object sender, EventArgs e)
        {
            this.TxtInput.Text = "Cancel clicked";
        }

        /// <summary>
        /// TODO: Add comment
        /// </summary>
        /// <param name="sender">Sender of the event</param>
        /// <param name="e">Arguments of the event</param>
        private void BtnOk_Click(object sender, EventArgs e)
        {
            this.TxtInput.Text = "Ok clicked";
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
    }
}

