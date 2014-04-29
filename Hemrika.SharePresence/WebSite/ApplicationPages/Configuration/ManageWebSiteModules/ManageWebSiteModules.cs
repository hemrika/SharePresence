// <copyright file="ManageWebSiteModules.cs" company="Hemrika">
// Copyright Hemrika. All rights reserved.
// </copyright>
// <author>MARKETING\Administrator</author>
// <date>2011-11-02 16:38:01Z</date>
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
    using System.Diagnostics.CodeAnalysis;
    using Hemrika.SharePresence.Common.WebSiteController;
    using Microsoft.SharePoint.Administration;
    using Hemrika.SharePresence.Common.UI;
    using Hemrika.SharePresence.Common.Ribbon.Definitions;
    using Hemrika.SharePresence.Common.Ribbon.Libraries;
    using Hemrika.SharePresence.Common.Ribbon.Definitions.Controls;
    using System.Collections.Generic;

    /// <summary>
    /// TODO: Add comment for ManageWebSiteModules
    /// </summary>
    [SharePointPermission(SecurityAction.InheritanceDemand, ObjectModel = true)]
    public class ManageWebSiteModules : EnhancedLayoutsPage
    {
        const string RibbonPostbackId = "ManageModulePostbackEvent";
        string _event = "New";
        /// <summary>
        /// Initializes a new instance of the ManageWebSiteModules class
        /// </summary>
        public ManageWebSiteModules()
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
            if (this.Page.Request["__EVENTTARGET"] == RibbonPostbackId)
            {
                _event = this.Page.Request["__EVENTARGUMENT"];
            }

            if (_event == "Add")
            {
                Response.Redirect("/_layouts/Hemrika/ManageWebSiteModule.aspx?Source=/_layouts/Hemrika/ManageWebSiteModules.aspx",true);
            }

            // If AccessControl rules have not been created, they will need to be and therefore, AllowUnsafeUpdates must be set to true
            SPControl.GetContextWeb(Context).AllowUnsafeUpdates = true;

            foreach (IWebSiteControllerModule module in WebSiteControllerConfig.Modules)
            {
                TableRow row = new TableRow();
                TableCell cell = new TableCell();
                cell.CssClass = "ms-descriptionText";
                HyperLink link = new HyperLink();
                link.NavigateUrl = "~/_layouts/Hemrika/ManageWebSiteModule.aspx?guid=" + module.Id.ToString() + "&Source=/_layouts/Hemrika/ManageWebSiteModules.aspx";
                link.Text = module.GetType().FullName;
                cell.Controls.Add(link);
                row.Cells.Add(cell);
                this.entriesTable.Rows.Add(row);
            }

            SPControl.GetContextWeb(Context).AllowUnsafeUpdates = false;


        }

        /// <summary>
        /// Entries table
        /// </summary>
        [SuppressMessage("Microsoft.StyleCop.CSharp.MaintainabilityRules", "SA1401:FieldsMustBePrivate", Justification = "ASP.NET pattern")]
        protected Table entriesTable;


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
            var groups = new List<GroupDefinition>();
            var buttons = new List<ControlDefinition>();

            var add = new ButtonDefinition()
            {
                Id = "AddModuleButton",
                Title = "Add Modules",
                CommandEnableJavaScript = "true",
                CommandJavaScript = String.Format("__doPostBack('{0}','{1}');", RibbonPostbackId, "Add"),
                Image = ImageLibrary.GetStandardImage(0, 7)
            };

            buttons.Add(add);

            var edit = new ButtonDefinition()
            {
                Id = "EditModuleButton",
                Title = "Edit Modules",
                CommandEnableJavaScript = "false",
                CommandJavaScript = "alert('Edit');",
                Image = ImageLibrary.GetStandardImage(6, 0)
            };
            
            buttons.Add(edit);

            var delete = new ButtonDefinition()
            {
                Id = "DeleteModuleButton",
                Title = "Delete Modules",
                CommandEnableJavaScript = "false",
                CommandJavaScript = "alert('Delete');",
                Image = ImageLibrary.GetStandardImage(6, 0)
            };
            
            buttons.Add(delete);

            var group = new GroupDefinition()
           {
               Id = "ModulesGroup",
               Title = "Module Actions",
               Sequence = "10",
               Template = GroupTemplateLibrary.SimpleTemplate,
               Controls = buttons.ToArray()
           };
            
            groups.Add(group);

            if (groups.Count > 0)
            {
                return new TabDefinition()
                {
                    Id = "ModulesRibbon",
                    Title = "Modules",
                    Sequence = "110",                    
                    GroupTemplates = GroupTemplateLibrary.AllTemplates,
                    Groups = groups.ToArray()
                };
            }
            else
            {
                return null;
            }

            /*
            return new TabDefinition()
            {
                Id = "ModulesRibbon",
                Title = "Modules",
                Groups = new GroupDefinition[]
                {
                    new GroupDefinition()
                    {
                        Id = "ModulesGroup",
                        Title = "Module Actions",
                        Template = GroupTemplateLibrary.SimpleTemplate,
                        Controls = new ControlDefinition[]
                        {
                            new ButtonDefinition()
                            {
                                Id = "AddModuleButton",
                                Title = "Add Modules",
                                CommandEnableJavaScript = "true",
                                CommandJavaScript = String.Format("__doPostBack('{0}','{1}');", RibbonPostbackId, "Add"),
                                Image = ImageLibrary.GetStandardImage(0, 7)
                            },
                            new ButtonDefinition()
                            {
                                Id = "EditModuleButton",
                                Title = "Edit Modules",
                                CommandEnableJavaScript = "false",
                                CommandJavaScript = "alert('Edit');",
                                Image = ImageLibrary.GetStandardImage(6, 0)
                            },
                            new ButtonDefinition()
                            {
                                Id = "DeleteModuleButton",
                                Title = "Delete Modules",
                                CommandEnableJavaScript = "false",
                                CommandJavaScript = "alert('Delete');",
                                Image = ImageLibrary.GetStandardImage(6, 0)
                            },

                        }
                    }
                }

            };
            */
        }

        public override Type SetTypeForLicense()
        {
            return typeof(ManageWebSiteModules);
            //throw new NotImplementedException();
        }
    }
}

