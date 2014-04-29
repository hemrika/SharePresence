// <copyright file="ManageWebSiteModule.cs" company="Hemrika">
// Copyright Hemrika. All rights reserved.
// </copyright>
// <author>MARKETING\Administrator</author>
// <date>2011-11-02 22:01:05Z</date>
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
    using Hemrika.SharePresence.Common.UI;
    using Hemrika.SharePresence.Common.Ribbon.Definitions;
    using Hemrika.SharePresence.Common.Ribbon.Definitions.Controls;
    using Hemrika.SharePresence.Common.Ribbon.Libraries;
    using System.Reflection;
    using System.Globalization;
    using System.Collections;
    using Microsoft.SharePoint.Administration;

    /// <summary>
    /// TODO: Add comment for ManageWebSiteModule
    /// </summary>
    [SharePointPermission(SecurityAction.InheritanceDemand, ObjectModel = true)]
    public class ManageWebSiteModule : EnhancedLayoutsPage
    {
        const string RibbonPostbackId = "ManageModulePostbackEvent";
        private string _event = "New";
        private string _source = string.Empty;
        private Guid _guid = Guid.Empty;
        private IWebSiteControllerModule _module;
        private String _modulename;
        private String _assembly;
        private string _seconds = "60";
        /// <summary>
        /// Initializes a new instance of the ManageWebSiteModule class
        /// </summary>
        public ManageWebSiteModule()
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
        /// Module TextBox
        /// </summary>
        [SuppressMessage("Microsoft.StyleCop.CSharp.MaintainabilityRules", "SA1401:FieldsMustBePrivate", Justification = "ASP.NET pattern")]
        protected TextBox moduleTextBox;

                /// <summary>
        /// Assembly TextBox
        /// </summary>
        [SuppressMessage("Microsoft.StyleCop.CSharp.MaintainabilityRules", "SA1401:FieldsMustBePrivate", Justification = "ASP.NET pattern")]
        protected TextBox assemblyTextbox;

        /// <summary>
        /// Entries table
        /// </summary>
        [SuppressMessage("Microsoft.StyleCop.CSharp.MaintainabilityRules", "SA1401:FieldsMustBePrivate", Justification = "ASP.NET pattern")]
        protected Table entriesTable;

        /// <summary>
        /// Raises the <see cref="E:Load"/> event.
        /// </summary>
        /// <param name="e">The <see cref="System.EventArgs"/> instance containing the event data.</param>
        protected override void OnLoad(EventArgs e)
        {
            SPContext.Current.Web.AllowUnsafeUpdates = true;
            if (!String.IsNullOrEmpty(Request.QueryString["guid"]))
            {
                _guid = new Guid(Request.QueryString["guid"]);
            }

            if (!String.IsNullOrEmpty(Request.QueryString["Source"]))
            {
                _source = Request.QueryString["Source"];
            }

            if (this.Page.Request["__EVENTTARGET"] == RibbonPostbackId)
            {
                _event = this.Page.Request["__EVENTARGUMENT"];
            }

            _modulename = this.moduleTextBox.Text;
            _assembly = this.assemblyTextbox.Text;

            if (_event == "GoModules")
            {
                GoBack();
            }

            if (_guid != Guid.Empty)
            {
                _module = WebSiteControllerConfig.GetModule(SPContext.Current.Site.WebApplication, _guid);

                if (!IsPostBack)
                {
                    this.moduleTextBox.Text = _module.GetType().FullName;
                    this.moduleTextBox.Enabled = false;
                    this.assemblyTextbox.Text = _module.GetType().AssemblyQualifiedName;
                    this.assemblyTextbox.Enabled = false;

                    this.LoadRules();
                }

                if (_event == "Delete")
                {
                    DeleteModule();
                }

                if (_event == "Rules")
                {
                    CreateRule();
                }
            }
            else
            {
                if (_event == "Save")
                {
                    SaveModule();
                }
            }
            SPContext.Current.Web.AllowUnsafeUpdates = false;
        }

        private void CreateRule()
        {
            Response.Redirect("/_layouts/Hemrika/ManageWebSiteModuleRule.aspx?ruletype=" + _module.RuleType + "&Source=/_layouts/Hemrika/ManageWebSiteModule.aspx?guid=" + _module.Id.ToString());
        }

        private void GoBack()
        {
            if (String.IsNullOrEmpty(_source))
            {
                Response.Redirect(_source,true);
            }
            else
            {
                Response.Redirect("/_layouts/Hemrika/ManageWebSiteModules.aspx",true);
            }
        }

        private void DeleteModule()
        {
            try
            {
                if (_module.CanBeRemoved)
                {
                    this.CreateWorkItem();
                    AddSharePointNotification(this.Page, "Deleting Module, deletion will occur in " + _seconds + " seconds.");
                }
            }
            catch (Exception ex)
            {
                SPDiagnosticsService.Local.WriteTrace(0, new SPDiagnosticsCategory(ex.Source, TraceSeverity.High, EventSeverity.Error), TraceSeverity.High, ex.Message, ex.Data);
                AddSharePointNotification(this.Page, "Deleting of Module has failed.");
            }
        }

        private void SaveModule()
        {
            try
            {
                if (_module != null)
                {
                    if (_module.CanBeRemoved)
                    {
                        this.CreateWorkItem();
                    }
                }
                else if (_module == null)
                {
                    this.CreateWorkItem();
                }

                AddSharePointNotification(this.Page, "Saving Module, update will occur in " + _seconds + " seconds.");
            }
            catch (Exception ex)
            {
                SPDiagnosticsService.Local.WriteTrace(0, new SPDiagnosticsCategory(ex.Source, TraceSeverity.High, EventSeverity.Error), TraceSeverity.High, ex.Message, ex.Data);
                AddSharePointNotification(this.Page, "Saving of Module has failed.");
            }
        }

        private void LoadRules()
        {
            string url = Site.Url;

            if (!url.EndsWith("/", StringComparison.OrdinalIgnoreCase))
            {
                url += "/";
            }

            TableRow headerRow = this.entriesTable.Rows[0];
            this.entriesTable.Rows.Clear();
            this.entriesTable.Rows.Add(headerRow);

            foreach (WebSiteControllerRule rule in WebSiteControllerConfig.GetRulesForSiteCollection(new Uri(url), _module.RuleType))
            {
                TableRow row = new TableRow();
                TableCell cell = new TableCell();
                cell.CssClass = "ms-descriptionText";
                cell.Text = rule.ShortRuleType;
                row.Cells.Add(cell);
                cell = new TableCell();
                cell.CssClass = "ms-descriptionText";
                HyperLink link = new HyperLink();
                link.NavigateUrl = "/_layouts/Hemrika/ManageWebSiteModuleRule.aspx?guid=" + rule.Id + "&ruletype=" + rule.RuleType + "&Source=/_layouts/Hemrika/ManageWebSiteModules.aspx?guid="+_module.Id.ToString();
                link.Text = rule.Url.ToString();
                cell.Controls.Add(link);
                row.Cells.Add(cell);
                cell = new TableCell();
                cell.CssClass = "ms-descriptionText";
                cell.Text = String.IsNullOrEmpty(rule.Principal) ? "&nbsp" : rule.Principal;
                row.Cells.Add(cell);
                cell = new TableCell();
                cell.CssClass = "ms-descriptionText";
                cell.Style.Add("text-align", "center");
                cell.Text = rule.PrincipalType.ToString();
                row.Cells.Add(cell);
                cell = new TableCell();
                CheckBox cb = new CheckBox();
                cb.Enabled = false;
                cb.Checked = rule.IsDisabled;
                cell.Controls.Add(cb);
                cell.HorizontalAlign = HorizontalAlign.Center;
                row.Cells.Add(cell);
                cell = new TableCell();
                cell.HorizontalAlign = HorizontalAlign.Center;
                cb = new CheckBox();
                cb.Enabled = false;
                cb.Checked = rule.AppliesToSsl;
                cell.Controls.Add(cb);
                row.Cells.Add(cell);
                cell = new TableCell();
                cell.CssClass = "ms-descriptionText";
                cell.Style.Add("text-align", "center");
                cell.Text = rule.Sequence.ToString(CultureInfo.InvariantCulture);
                row.Cells.Add(cell);
                cell = new TableCell();
                Literal literal = new Literal();
                if (rule.Properties.Count > 0)
                {
                    cell.CssClass = "ms-descriptionText";
                    StringBuilder sb = new StringBuilder();
                    foreach (DictionaryEntry property in rule.Properties)
                    {
                            sb.Append(_module.GetFriendlyName(property.Key.ToString()) + ": " + property.Value.ToString() + "<br/>");
                    }

                    literal.Text = sb.ToString();
                }
                else
                {
                    literal.Text = "None";
                }

                cell.Controls.Add(literal);
                row.Cells.Add(cell);
                this.entriesTable.Rows.Add(row);
            }
        }

        private void CreateWorkItem()
        {
            Guid siteId = SPContext.Current.Site.ID;
            Guid webId = SPContext.Current.Web.ID;

            SPSecurity.RunWithElevatedPrivileges(() =>
            {
                using (SPSite site = new SPSite(siteId))
                {
                    site.AddWorkItem(
                        Guid.NewGuid(),
                        DateTime.Now.ToUniversalTime(),
                        WebSiteControllerModuleWorkItem.WorkItemTypeId,
                        webId,
                        siteId,
                        1,
                        true,
                        _guid,
                        Guid.Empty,
                        site.SystemAccount.ID,
                        null,
                        _modulename + ";" + _assembly,
                        Guid.Empty
                        );

                    SPJobDefinitionCollection jobs = site.WebApplication.JobDefinitions;

                    foreach (SPJobDefinition job in jobs)
                    {
                        if (job.Name == WebSiteControllerModuleWorkItem.WorkItemJobDisplayName)
                        {
                            DateTime next = job.Schedule.NextOccurrence(job.LastRunTime);
                            _seconds = next.Second.ToString();

                            break;
                        }
                    }

                    //SPJobDefinition job = site.WebApplication.JobDefinitions[WebSiteControllerModuleWorkItem.WorkItemJobDisplayName];
                    //job.RunNow();

                }
            });
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

            GroupDefinition[] group = new GroupDefinition[3];

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
            Modulelayouts.Id = "ModulesGroup";
            Modulelayouts.Title = "Manage Module";
            Modulelayouts.Sequence = "20";
            Modulelayouts.Template = GroupTemplateLibrary.SimpleTemplate;

            ControlDefinition[] Modulescontrols = new ControlDefinition[2];

            ButtonDefinition savebutton = new ButtonDefinition();
            savebutton.Id = "SaveModules";
            savebutton.TemplateAlias = "o1";
            savebutton.Title = "Save Module";
            //button.Description = master.Description;
            savebutton.CommandEnableJavaScript = "true";
            savebutton.CommandJavaScript = String.Format("__doPostBack('{0}','{1}');", RibbonPostbackId, "Save");
            savebutton.Image = ImageLibrary.GetStandardImage(8, 4);
            Modulescontrols[0] = savebutton;

            ButtonDefinition deletebutton = new ButtonDefinition();
            deletebutton.Id = "DeleteModules";
            deletebutton.TemplateAlias = "o1";
            deletebutton.Title = "Delete Module";
            //button.Description = master.Description;
            deletebutton.CommandEnableJavaScript = Guid.Empty.Equals(_guid) ? Boolean.FalseString : Boolean.TrueString;
            deletebutton.CommandJavaScript = String.Format("__doPostBack('{0}','{1}');", RibbonPostbackId, "Delete");
            deletebutton.Image = ImageLibrary.GetStandardImage(4, 4);
            Modulescontrols[1] = deletebutton;

            Modulelayouts.Controls = Modulescontrols;
            group[1] = Modulelayouts;

            GroupDefinition Rulelayouts = new GroupDefinition();
            Rulelayouts.Id = "RulesGroup";
            Rulelayouts.Title = "Manage Rules";
            Rulelayouts.Sequence = "30";
            Rulelayouts.Template = GroupTemplateLibrary.SimpleTemplate;

            ControlDefinition[] Rulecontrols = new ControlDefinition[1];

            ButtonDefinition Rulebutton = new ButtonDefinition();
            Rulebutton.Id = "ManageRule";
            Rulebutton.TemplateAlias = "o1";
            Rulebutton.Title = "Manage Rules";
            //button.Description = master.Description;
            Rulebutton.CommandEnableJavaScript = Guid.Empty.Equals(_guid) ? Boolean.FalseString : Boolean.TrueString;
            Rulebutton.CommandJavaScript = String.Format("__doPostBack('{0}','{1}');", RibbonPostbackId, "Rules");
            Rulebutton.Image = ImageLibrary.GetStandardImage(0, 7);
            Rulecontrols[0] = Rulebutton;

            Rulelayouts.Controls = Rulecontrols;
            group[2] = Rulelayouts;
            tab.Groups = group;

            return tab;
        }

        public override Type SetTypeForLicense()
        {
            return typeof(ManageWebSiteModule);
            //throw new NotImplementedException();
        }
    }
}

