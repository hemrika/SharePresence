// <copyright file="ManageWebSiteModuleRule.cs" company="Hemrika">
// Copyright Hemrika. All rights reserved.
// </copyright>
// <author>MARKETING\Administrator</author>
// <date>2011-11-05 13:34:57Z</date>
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
    using Hemrika.SharePresence.Common.Ribbon.Definitions;
    using Hemrika.SharePresence.Common.Ribbon.Definitions.Controls;
    using Hemrika.SharePresence.Common.Ribbon.Libraries;
    using Hemrika.SharePresence.Common.WebSiteController;
    using System.Diagnostics.CodeAnalysis;
    using System.Globalization;
    using System.Collections;
    using Microsoft.SharePoint.Administration;

    /// <summary>
    /// TODO: Add comment for ManageWebSiteModuleRule
    /// </summary>
    [SharePointPermission(SecurityAction.InheritanceDemand, ObjectModel = true)]
    public class ManageWebSiteModuleRule : EnhancedLayoutsPage
    {
        const string RibbonPostbackId = "ManageModulePostbackEvent";
        private string _event = "New";
        private string _source = string.Empty;
        private Guid _guid = Guid.Empty;
        private string _ruletype = String.Empty;
        private IWebSiteControllerModule _module;
        private WebSiteControllerRule _rule;
        private string _seconds = "60";
        private bool _SimpleView = false;
        /// <summary>
        /// Initializes a new instance of the ManageWebSiteModuleRule class
        /// </summary>
        public ManageWebSiteModuleRule()
        {
            this.RightsCheckMode = RightsCheckModes.OnPreInit;
        }

        /// <summary>
        /// Web app url label
        /// </summary>
        [SuppressMessage("Microsoft.StyleCop.CSharp.MaintainabilityRules", "SA1401:FieldsMustBePrivate", Justification = "ASP.NET pattern")]
        protected Label webAppUrlLabel;

        /// <summary>
        /// Page TextBox
        /// </summary>
        [SuppressMessage("Microsoft.StyleCop.CSharp.MaintainabilityRules", "SA1401:FieldsMustBePrivate", Justification = "ASP.NET pattern")]
        protected TextBox pageTextBox;

        /// <summary>
        /// Principal Type section
        /// </summary>
        [SuppressMessage("Microsoft.StyleCop.CSharp.MaintainabilityRules", "SA1401:FieldsMustBePrivate", Justification = "ASP.NET pattern")]
        protected InputFormSection principalTypeSection;

        /// <summary>
        /// Principal Type Radio Buttons
        /// </summary>
        [SuppressMessage("Microsoft.StyleCop.CSharp.MaintainabilityRules", "SA1401:FieldsMustBePrivate", Justification = "ASP.NET pattern")]
        protected RadioButtonList principalTypeList;

        /// <summary>
        /// Principal TextBox
        /// </summary>
        [SuppressMessage("Microsoft.StyleCop.CSharp.MaintainabilityRules", "SA1401:FieldsMustBePrivate", Justification = "ASP.NET pattern")]
        protected TextBox principalTextBox;

        /// <summary>
        /// Is Disabled CheckBox
        /// </summary>
        [SuppressMessage("Microsoft.StyleCop.CSharp.MaintainabilityRules", "SA1401:FieldsMustBePrivate", Justification = "ASP.NET pattern")]
        protected CheckBox disabledCheckBox;

        /// <summary>
        /// Applies to SSL CheckBox
        /// </summary>
        [SuppressMessage("Microsoft.StyleCop.CSharp.MaintainabilityRules", "SA1401:FieldsMustBePrivate", Justification = "ASP.NET pattern")]
        protected CheckBox appliesToSslCheckBox;

        /// <summary>
        /// Sequence number text box
        /// </summary>
        [SuppressMessage("Microsoft.StyleCop.CSharp.MaintainabilityRules", "SA1401:FieldsMustBePrivate", Justification = "ASP.NET pattern")]
        protected TextBox sequenceTextBox;

        /// <summary>
        /// Properties section
        /// </summary>
        [SuppressMessage("Microsoft.StyleCop.CSharp.MaintainabilityRules", "SA1401:FieldsMustBePrivate", Justification = "ASP.NET pattern")]
        protected InputFormSection propertiesSection;

        /// <summary>
        /// Properties place holder
        /// </summary>
        [SuppressMessage("Microsoft.StyleCop.CSharp.MaintainabilityRules", "SA1401:FieldsMustBePrivate", Justification = "ASP.NET pattern")]
        protected PlaceHolder propertiesPlaceholder;

                /// <summary>
        /// Default place holder
        /// </summary>
        [SuppressMessage("Microsoft.StyleCop.CSharp.MaintainabilityRules", "SA1401:FieldsMustBePrivate", Justification = "ASP.NET pattern")]
        protected PlaceHolder defaultPlaceHolder;
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
            this.webAppUrlLabel.Text = this.Site.Url + "/";

            if (_guid != Guid.Empty)
            {
                if (!IsPostBack)
                {
                    this._rule = WebSiteControllerConfig.GetRule(SPContext.Current.Site.WebApplication, _guid);

                    this.pageTextBox.Text = this._rule.Page;
                    this.disabledCheckBox.Checked = this._rule.IsDisabled;
                    this.appliesToSslCheckBox.Checked = this._rule.AppliesToSsl;
                    this.sequenceTextBox.Text = this._rule.Sequence.ToString(CultureInfo.CurrentCulture);
                    this.principalTextBox.Text = this._rule.Principal;
                    if (!String.IsNullOrEmpty(this._rule.Principal))
                    {
                        this.principalTypeSection.Visible = true;
                        if (this._rule.PrincipalType == WebSiteControllerPrincipalType.User)
                        {
                            this.principalTypeList.SelectedValue = "User";
                        }
                        else
                        {
                            this.principalTypeList.SelectedValue = "Group";
                        }
                    }

                    //this.ruleTypeLabel.Text = this.rule.ShortRuleType;
                }

            }
            else
            {
                //this.pageTextBox.Text = this._rule.Page.Replace(this.Site.Url + "/", String.Empty);
                this.appliesToSslCheckBox.Checked = true;
                this.sequenceTextBox.Text = WebSiteControllerConfig.Rules.Count.ToString(CultureInfo.CurrentCulture);
                //this.ruleTypeLabel.Text = _ruletype;
            }
            /*
            if (_SimpleView)
            {
                ControlCollection controls = this.propertiesPlaceholder.Controls;
                foreach (Control ctrl in controls)
                {
                    ctrl.Visible = false;
                }
            }
            */
            if (_event == "Save")
            {
                SaveRule();
            }

            if (_event == "Delete")
            {
                DeleteRule();
            }

        }

        private void GoBack()
        {
            AddSharePointNotification(this.Page, "Previous Page");
            Response.Redirect(_source);
        }

        private void SaveRule()
        {
            CreateWorkItem();
            AddSharePointNotification(this.Page, "Saving Rule, update will occur in "+ _seconds+" seconds.");
        }

        private void DeleteRule()
        {
            CreateWorkItem();
            AddSharePointNotification(this.Page, "Deleting Rule, deletion will occur in " + _seconds+ " seconds.");
        }

        private void CreateWorkItem()
        {
            Guid siteId = SPContext.Current.Site.ID;
            Guid webId = SPContext.Current.Web.ID;

            WebSiteControllerRuleControl control = (WebSiteControllerRuleControl)FindControlRecursive(this.Master, "moduleControl");

            string url = this.pageTextBox.Text;
            if (_SimpleView)
            {
                url = control.DefaultUrl;
            }

            bool disabled = this.disabledCheckBox.Checked;
            bool appliesToSSL = this.appliesToSslCheckBox.Checked;
            int sequence = Convert.ToInt32(this.sequenceTextBox.Text, CultureInfo.CurrentCulture);
            WebSiteControllerPrincipalType principalType = WebSiteControllerPrincipalType.None;
            String pricipal = this.principalTextBox.Text;

            if (!String.IsNullOrEmpty(this.principalTextBox.Text))
            {
                principalType = (WebSiteControllerPrincipalType)Enum.Parse(typeof(WebSiteControllerPrincipalType), this.principalTypeList.SelectedValue);
            }

            Hashtable properties = null;
            if (control != null)
            {
                properties = control.Properties;
            }

            StringBuilder builder = new StringBuilder();
            builder.Append(url+";");
            builder.Append(disabled.ToString()+";");
            builder.Append(appliesToSSL.ToString()+";");
            builder.Append(sequence.ToString()+";");
            builder.Append(principalType.ToString() + ";");
            builder.Append(pricipal + ";");
            builder.Append("#");

            foreach(DictionaryEntry prop in properties)
            {
            builder.Append(String.Format("{0}:{1};",prop.Key,prop.Value));
            }

            Guid itemGuid = _guid;
            int item = 0;

            if (itemGuid.Equals(Guid.Empty))
            {
                itemGuid = _module.Id;
                item = -1;
            }
            else
            {
                item = 2;
            }

            if (_event == "Delete")
            {
                item = 1;
            }

            SPSecurity.RunWithElevatedPrivileges(() =>
            {
                using (SPSite site = new SPSite(siteId))
                {
                    site.AddWorkItem(
                        Guid.NewGuid(),
                        DateTime.Now.ToUniversalTime(),
                        WebSiteControllerRuleWorkItem.WorkItemTypeId,
                        webId,
                        siteId,
                        item,
                        true,
                        itemGuid,
                        Guid.Empty,
                        site.SystemAccount.ID,
                        null,
                        builder.ToString(),
                        Guid.Empty
                        );

                    SPJobDefinitionCollection jobs = site.WebApplication.JobDefinitions;

                    foreach (SPJobDefinition job in jobs)
                    {
                        if (job.Name == WebSiteControllerRuleWorkItem.WorkItemJobDisplayName)
                        {
                            DateTime next = job.Schedule.NextOccurrence(job.LastRunTime);
                            _seconds = next.Second.ToString();
                            break;
                        }
                    }

                    //SPJobDefinition job = site.WebApplication.JobDefinitions[WebSiteControllerRuleWorkItem.WorkItemJobDisplayName];
                    //job.RunNow();
                }
            });
        }

        /// <summary>
        /// Raises the <see cref="E:System.Web.UI.Control.Init"/> event to initialize the page.
        /// </summary>
        /// <param name="e">An <see cref="T:System.EventArgs"/> that contains the event data.</param>
        protected override void OnInit(EventArgs e)
        {
            SPContext.Current.Web.AllowUnsafeUpdates = true;

            if (!String.IsNullOrEmpty(Request.QueryString["guid"]))
            {
                _guid = new Guid(Request.QueryString["guid"]);
            }

            if (!String.IsNullOrEmpty(Request.QueryString["ruletype"]))
            {
                _ruletype = Request.QueryString["ruletype"];
            }


            if (!String.IsNullOrEmpty(Request.QueryString["Source"]))
            {
                _source = Request.QueryString["Source"];
            }

            if (this.Page.Request["__EVENTTARGET"] == RibbonPostbackId)
            {
                _event = this.Page.Request["__EVENTARGUMENT"];
            }

            if (_event == "GoModules")
            {
                GoBack();
            }

            if (!string.IsNullOrEmpty(_ruletype))
            {
                _module = WebSiteControllerConfig.GetModule(SPContext.Current.Site.WebApplication, _ruletype);
            }

            try
            {
                if (_module != null)
                {
                    if (!String.IsNullOrEmpty(_module.Control))
                    {
                        WebSiteControllerRuleControl control = (WebSiteControllerRuleControl)Page.LoadControl(_module.Control);
                        control.ID = "moduleControl";
                        if (control.SimpleView)
                        {
                            _SimpleView = control.SimpleView;
                            
                            ControlCollection controls = this.defaultPlaceHolder.Controls;
                            foreach (Control ctrl in controls)
                            {
                                ctrl.Visible = false;
                            }
                        }

                        this.propertiesPlaceholder.Controls.Add(control);
                    }
                }
            }
            catch (Exception ex)
            {
                ex.ToString();
            }

            SPContext.Current.Web.AllowUnsafeUpdates = false;
        }

        /// <summary>
        /// Finds the control recursive.
        /// </summary>
        /// <param name="root">The root of the search.</param>
        /// <param name="id">The id being searched for.</param>
        /// <returns>The control on the page</returns>
        private static Control FindControlRecursive(Control root, string id)
        {
            if (root.ID == id)
            {
                return root;
            }

            foreach (Control ctl in root.Controls)
            {
                Control foundCtl = FindControlRecursive(ctl, id);
                if (foundCtl != null)
                {
                    return foundCtl;
                }
            }

            return null;
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
            TabDefinition tab = new TabDefinition();
            tab.Id = "ModulesRibbon";
            tab.Title = "Modules";
            tab.Sequence = "110";
            tab.GroupTemplates = GroupTemplateLibrary.AllTemplates;

            GroupDefinition[] group = new GroupDefinition[2];

            GroupDefinition Navigationlayouts = new GroupDefinition();
            Navigationlayouts.Id = "ActionsGroup";
            Navigationlayouts.Title = "Navigation";
            Navigationlayouts.Sequence = "10";
            Navigationlayouts.Template = GroupTemplateLibrary.SimpleTemplate;

            ControlDefinition[] Navigationcontrols = new ControlDefinition[1];

            ButtonDefinition backbutton = new ButtonDefinition();
            backbutton.Id = "GoBackModules";
            backbutton.TemplateAlias = "o1";
            backbutton.Title = "Go back";
            //button.Description = master.Description;
            backbutton.CommandEnableJavaScript = "true";
            backbutton.CommandJavaScript = String.Format("__doPostBack('{0}','{1}');", RibbonPostbackId, "GoModules");
            backbutton.Image = ImageLibrary.GetStandardImage(3, 13);
            Navigationcontrols[0] = backbutton;

            Navigationlayouts.Controls = Navigationcontrols;
            group[0] = Navigationlayouts;


            GroupDefinition Modulelayouts = new GroupDefinition();
            Modulelayouts.Id = "RulesGroup";
            Modulelayouts.Title = "Manage Rules";
            Modulelayouts.Sequence  = "20";
            Modulelayouts.Template = GroupTemplateLibrary.SimpleTemplate;

            ControlDefinition[] Modulescontrols = new ControlDefinition[2];

            ButtonDefinition savebutton = new ButtonDefinition();
            savebutton.Id = "SaveRule";
            savebutton.TemplateAlias = "o1";
            savebutton.Title = "Save Rule";
            //button.Description = master.Description;
            savebutton.CommandEnableJavaScript = "true";
            savebutton.CommandJavaScript = String.Format("__doPostBack('{0}','{1}');", RibbonPostbackId, "Save");
            savebutton.Image = ImageLibrary.GetStandardImage(8, 4);
            Modulescontrols[0] = savebutton;

            ButtonDefinition deletebutton = new ButtonDefinition();
            deletebutton.Id = "DeleteRule";
            deletebutton.TemplateAlias = "o1";
            deletebutton.Title = "Delete Rule";
            //button.Description = master.Description;
            deletebutton.CommandEnableJavaScript = Guid.Empty.Equals(_guid) ? Boolean.FalseString : Boolean.TrueString;
            deletebutton.CommandJavaScript = String.Format("__doPostBack('{0}','{1}');", RibbonPostbackId, "Delete");
            deletebutton.Image = ImageLibrary.GetStandardImage(4, 4);
            Modulescontrols[1] = deletebutton;

            Modulelayouts.Controls = Modulescontrols;
            group[1] = Modulelayouts;

            tab.Groups = group;

            return tab;
        }

        public override Type SetTypeForLicense()
        {
            return typeof(ManageWebSiteModuleRule);
            //throw new NotImplementedException();
        }
    }
}

