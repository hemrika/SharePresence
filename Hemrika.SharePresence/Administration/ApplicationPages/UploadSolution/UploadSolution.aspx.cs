// <copyright file="LicenseManager.cs" company="Hemrika">
// Copyright Hemrika. All rights reserved.
// </copyright>
// <author>MARKETING\Administrator</author>
// <date>2011-10-13 15:47:31Z</date>
namespace Hemrika.SharePresence.Administration
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
    using Microsoft.SharePoint.Administration;
    using Hemrika.SharePresence.Common.UI;
    using Hemrika.SharePresence.Common.Ribbon.Definitions;
    using Hemrika.SharePresence.Common.Ribbon.Libraries;
    using Hemrika.SharePresence.Common.Ribbon.Definitions.Controls;
    using System.Threading;

    /// <summary>
    /// TODO: Add comment for LicenseManager
    /// </summary>
    [SharePointPermission(SecurityAction.InheritanceDemand, ObjectModel = true)]
    public class UploadSolution : EnhancedLayoutsPage
    {
        /// <summary>
        /// Initializes a new instance of the LicenseManager class
        /// </summary>
        public UploadSolution()
        {
            this.RightsCheckMode = RightsCheckModes.OnPreInit;
        }

        /// <summary>
        /// Gets or sets the pointer to the TextInputBox on the page 
        /// </summary>
        protected FileUpload fup_wsp
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
        }

        private string strTempPath = Path.GetTempPath();

        /// <summary>
        /// TODO: Add comment
        /// </summary>
        /// <param name="sender">Sender of the event</param>
        /// <param name="e">Arguments of the event</param>
        private void BtnOk_Click(object sender, EventArgs e)
        {
            string strPackageFileName = Path.GetFileName(fup_wsp.PostedFile.FileName);
            string strPackageServerFileName = strTempPath + strPackageFileName;
            //lblErrorMessage.ForeColor = Color.Red;

            try
            {
                //Checking if the file selected...
                if (fup_wsp.FileName == "")
                {
                    //lblErrorMessage.Text = "Please select the solution file";
                    return;
                }

                //Checking if the package already existed in the Farm Solutions.
                if (SPFarm.Local.Solutions[strPackageFileName] != null)
                {
                    //lblErrorMessage.Text = "Please retract and remove the solution before uploading it";
                    return;
                }

                using (SPLongOperation operation = new SPLongOperation(this.Page))
                {
                    operation.LeadingHTML = "Uploading the solution package";
                    operation.TrailingHTML = "Adding " + strPackageFileName + " to the solution store..";
                    operation.Begin();

                    //Deleting the file if its already existed.
                    if (File.Exists(strPackageServerFileName))
                        File.Delete(strPackageServerFileName);

                    //Saving a copy of the Package.
                    fup_wsp.PostedFile.SaveAs(strPackageServerFileName);



                    //Adding the Package to the Store.
                    SPFarm.Local.Solutions.Add(strPackageServerFileName);

                    //Thread.Sleep(0x1770);
                    operation.End(SPContext.Current.Site.Url + "/_admin/operations.aspx");

                }

            }
            catch (Exception ex)
            {

                throw new SPException(ex.Message);
            }
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

        public override TabDefinition GetTabDefinition()
        {
            return null;
        }


        public override Type SetTypeForLicense()
        {
            return typeof(UploadSolution);
        }
    }
}

