// <copyright file="ComponentManager.cs" company="Hemrika">
// Copyright Hemrika. All rights reserved.
// </copyright>
// <author>MARKETING\Administrator</author>
// <date>2011-10-13 15:53:11Z</date>
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
    using Hemrika.SharePresence.Common.Ribbon.Definitions;
    using Hemrika.SharePresence.Common.Ribbon.Libraries;
    using Hemrika.SharePresence.Common.Ribbon.Definitions.Controls;
    using Hemrika.SharePresence.Common.UI;

    /// <summary>
    /// TODO: Add comment for ComponentManager
    /// </summary>
    [SharePointPermission(SecurityAction.InheritanceDemand, ObjectModel = true)]
    public class ComponentManager : EnhancedLayoutsPage
    {
        /// <summary>
        /// Initializes a new instance of the ComponentManager class
        /// </summary>
        public ComponentManager()
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
        public override TabDefinition GetTabDefinition()
        {
            return new TabDefinition()
            {
                Id = "TestRibbon",
                Title = "Test",
                Groups = new GroupDefinition[]
                {
                    new GroupDefinition()
                    {
                        Id = "TestGroup",
                        Title = "Test group",
                        Template = GroupTemplateLibrary.SimpleTemplate,
                        Controls = new ControlDefinition[]
                        {
                            new ButtonDefinition()
                            {
                                Id = "TestButton",
                                Title = "Test button",
                                CommandJavaScript = "alert('test!');",
                                Image = ImageLibrary.GetStandardImage(6, 0)
                            }
                        }
                    }
                }

            };
        }



        public override Type SetTypeForLicense()
        {
            return typeof(ComponentManager);
        }
    }
}

