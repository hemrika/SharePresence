// <copyright file="ManagePageLayouts.cs" company="Hemrika">
// Copyright Hemrika. All rights reserved.
// </copyright>
// <author>MARKETING\Administrator</author>
// <date>2011-10-23 09:58:35Z</date>
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
    using Hemrika.SharePresence.Common;
    using Hemrika.SharePresence.Common.Configuration;
    using Hemrika.SharePresence.WebSite.Layout;
    using Hemrika.SharePresence.Common.ServiceLocation;
    using Hemrika.SharePresence.Common.UI;
    using System.ComponentModel;
    using Hemrika.SharePresence.Common.License;
    using Hemrika.SharePresence.SPLicense.LicenseFile;
    using Microsoft.SharePoint.Administration;

    /// <summary>
    /// TODO: Add comment for ManagePageLayouts
    /// </summary>
    [SharePointPermission(SecurityAction.InheritanceDemand, ObjectModel = true)]
    [LicenseProvider(typeof(SPLicenseProvider))]
    public class ManagePageLayouts : EnhancedLayoutsPage
    {
        /*
        private IServiceLocator serviceLocator;
        private IPageLayoutRepository servicepageLayouts;
        private IConfigManager configManager;
        private IPropertyBag bag;
        */

        /// <summary>
        /// Initializes a new instance of the ManagePageLayouts class
        /// </summary>
        public ManagePageLayouts()
        {
            this.RightsCheckMode = RightsCheckModes.OnPreInit;

            /*
            serviceLocator = SharePointServiceLocator.GetCurrent();
            servicepageLayouts = serviceLocator.GetInstance<IPageLayoutRepository>();

            configManager = serviceLocator.GetInstance<IConfigManager>();
            bag = configManager.GetPropertyBag(ConfigLevel.CurrentSPSite);
            */

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

        protected override void OnInit(EventArgs e)
        {
            base.OnInit(e);
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
            ((EnhancedLayoutsPage)Page).EndOperation(ModalDialogResult.Cancel);
        }

        /// <summary>
        /// TODO: Add comment
        /// </summary>
        /// <param name="sender">Sender of the event</param>
        /// <param name="e">Arguments of the event</param>
        private void BtnOk_Click(object sender, EventArgs e)
        {
            try
            {
                bool isLicensed = ((EnhancedLayoutsPage)this).IsLicensed;
                //this.TxtInput.Text += this._license.Issuer.FullName + Environment.NewLine;
                //this.TxtInput.Text += this._license.Product.Assembly+ Environment.NewLine;
                //this.TxtInput.Text += this._license.Product.FullName + Environment.NewLine;
                //this.TxtInput.Text += this._license.Product.Version + Environment.NewLine;

            }
            catch (Exception ex)
            {
                SPDiagnosticsService.Local.WriteTrace(0, new SPDiagnosticsCategory(ex.Source, TraceSeverity.High, EventSeverity.Error), TraceSeverity.High, ex.Message, ex.Data);
                //this.TxtInput.Text = error;
            }

            //((EnhancedLayoutsPage)Page).EndOperation(ModalDialogResult.OK);
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
            //throw new NotImplementedException();
        }

        public override Type SetTypeForLicense()
        {
            return typeof(ManagePageLayouts);
        }
    }
}

