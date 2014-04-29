// -----------------------------------------------------------------------
// <copyright file="SematicModuleControl.cs" company="">
// TODO: Update copyright text.
// </copyright>
// -----------------------------------------------------------------------

namespace Hemrika.SharePresence.WebSite.Modules.WebPageModule
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using Hemrika.SharePresence.Common.WebSiteController;
    using System.Diagnostics.CodeAnalysis;
    using System.Collections;
    using System.Web.UI.WebControls;
    using Microsoft.SharePoint;
    using System.Web;

    /// <summary>
    /// TODO: Update summary.
    /// </summary>
    public class WebPageModuleControl : WebSiteControllerRuleControl
    {

        /// <summary>
        /// Gets the properties for this rule.
        /// </summary>
        /// <value>The properties for this rule.</value>
        public override Hashtable Properties
        {
            get
            {
                Hashtable properties = new Hashtable();
                //properties.Add("Language", this.ddl_language.SelectedItem.Value);
                //properties.Add("SemanticUrl", this.SemanticUrlTextBox.Text);
                return properties;
            }
        }

        /// <summary>
        /// Raises the <see cref="E:Load"/> event.
        /// </summary>
        /// <param name="e">The <see cref="System.EventArgs"/> instance containing the event data.</param>
        protected override void OnLoad(EventArgs e)
        {
            /*
            SPContext.Current.Web.AllowUnsafeUpdates = true;
            if (!String.IsNullOrEmpty(Request.QueryString["guid"]) && !IsPostBack)
            {
                WebSiteControllerRule rule = WebSiteControllerConfig.GetRule(this.Page.Request.QueryString["guid"]);
                //this.ddl_language.SelectedValue = rule.Properties["Language"].ToString();
                //this.SemanticUrlTextBox.Text = rule.Properties["SemanticUrl"].ToString();
            }
            //TODO Button Dialog Update
            else
            {
                try
                {
                    WebPageModule module = new WebPageModule();
                    Uri url = new Uri(this.Page.Request.QueryString["Source"].ToString());

                    if (WebSiteControllerConfig.IsPageControlled( url, module.RuleType))
                    {
                        System.Collections.Generic.List<WebSiteControllerRule> rules = WebSiteControllerConfig.GetRulesForPage(SPContext.Current.Site.WebApplication, url, module.RuleType);
                        WebSiteControllerRule rule = rules[rules.Count - 1];
                        //this.ddl_language.SelectedValue = rule.Properties["Language"].ToString();
                        //this.SemanticUrlTextBox.Text = rule.Properties["SemanticUrl"].ToString();
                    }
                }
                catch (Exception ex)
                {
                    ex.ToString();
                }
            }
            SPContext.Current.Web.AllowUnsafeUpdates = false;
            -*/
        }

        public override bool SimpleView
        {
            get { return true; }
        }

        public override string DefaultUrl
        {
            get { return ""; }
        }
    }
}
