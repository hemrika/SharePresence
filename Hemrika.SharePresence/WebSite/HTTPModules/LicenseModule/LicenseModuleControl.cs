// -----------------------------------------------------------------------
// <copyright file="SematicModuleControl.cs" company="">
// TODO: Update copyright text.
// </copyright>
// -----------------------------------------------------------------------

namespace Hemrika.SharePresence.WebSite.Modules.LicenseModule
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
    using Microsoft.SharePoint.Administration;

    /// <summary>
    /// TODO: Update summary.
    /// </summary>
    public class LicenseModuleControl : WebSiteControllerRuleControl
    {
        /// <summary>
        /// Forward To Page TextBox
        /// </summary>
        [SuppressMessage("Microsoft.StyleCop.CSharp.MaintainabilityRules", "SA1401:FieldsMustBePrivate", Justification = "ASP.NET pattern")]
        protected TextBox tbx_LicenseDomain;

        /// <summary>
        /// Error Code TextBox
        /// </summary>
        [SuppressMessage("Microsoft.StyleCop.CSharp.MaintainabilityRules", "SA1401:FieldsMustBePrivate", Justification = "ASP.NET pattern")]
        protected TextBox txb_LicenseFile;

        /// <summary>
        /// Gets the properties for this rule.
        /// </summary>
        /// <value>The properties for this rule.</value>
        public override Hashtable Properties
        {
            get
            {
                Hashtable properties = new Hashtable();
                properties.Add("LicenseDomain", this.tbx_LicenseDomain.Text);
                properties.Add("LicenseFile", this.txb_LicenseFile.Text);
                return properties;
            }
        }

        /// <summary>
        /// Raises the <see cref="E:Load"/> event.
        /// </summary>
        /// <param name="e">The <see cref="System.EventArgs"/> instance containing the event data.</param>
        protected override void OnLoad(EventArgs e)
        {
            SPContext.Current.Web.AllowUnsafeUpdates = true;
            if (!String.IsNullOrEmpty(Request.QueryString["guid"]) && !IsPostBack)
            {
                WebSiteControllerRule rule = WebSiteControllerConfig.GetRule(this.Page.Request.QueryString["guid"]);
                this.tbx_LicenseDomain.Text = rule.Properties["LicenseDomain"].ToString();
                this.txb_LicenseFile.Text = rule.Properties["LicenseFile"].ToString();
            }
            //TODO Button Dialog Update
            else
            {
                try
                {
                    LicenseModule module = new LicenseModule();
                    Uri url = new Uri(this.Page.Request.QueryString["Source"].ToString());

                    if (WebSiteControllerConfig.IsPageControlled( url, module.RuleType))
                    {
                        System.Collections.Generic.List<WebSiteControllerRule> rules = WebSiteControllerConfig.GetRulesForPage(SPAdministrationWebApplication.Local, url, module.RuleType);
                        WebSiteControllerRule rule = rules[rules.Count - 1];
                        this.tbx_LicenseDomain.Text = rule.Properties["LicenseDomain"].ToString();
                        this.txb_LicenseFile.Text = rule.Properties["LicenseFile"].ToString();
                    }
                }
                catch (Exception ex)
                {
                    ex.ToString();
                }
            }
        }

        public override bool SimpleView
        {
            get { return true; }
        }

        public override string DefaultUrl
        {
            get
            {
                if (tbx_LicenseDomain != null && !string.IsNullOrEmpty(tbx_LicenseDomain.Text))
                {
                    return tbx_LicenseDomain.Text;
                }

                return "/";
            }
        }
    }
}
