// <copyright file="WebSiteControllerRule.cs" company="MMC Inc.">
//     Copyright (c) Matt Resnick. All rights reserved.
// </copyright>
namespace Hemrika.SharePresence.Common.WebSiteController
{
    using System;
    using System.Collections;
    using Microsoft.SharePoint.Administration;

    /// <summary>
    /// A class containing information about an action to be applied to a page
    /// </summary>
    public class WebSiteControllerRule : SPPersistedObject
    {
        #region Fields

        /// <summary>
        /// Site collection associated with this rule
        /// </summary>
        [Persisted]
        private string siteCollection;
        
        /// <summary>
        /// Page of this access control rule
        /// </summary>
        [Persisted]
        private string page;

        /// <summary>
        /// Type of rule
        /// </summary>
        [Persisted]
        private string ruleType;

        /// <summary>
        /// Principal this rule applies to
        /// </summary>
        [Persisted]
        private string principal;

        /// <summary>
        /// Type of principal this rule applies to
        /// </summary>
        [Persisted]
        private WebSiteControllerPrincipalType principalType;

        /// <summary>
        /// Determines if the rule should also be applied to urls that begin with https
        /// </summary>
        [Persisted]
        private bool appliesToSsl;

        /// <summary>
        /// Indicates if the rule should be applied or not
        /// </summary>
        [Persisted]
        private bool disabled;

        /// <summary>
        /// Sequence of the rule compared to other rules
        /// </summary>
        [Persisted]
        private int sequence;

        #endregion

        /// <summary>
        /// Initializes a new instance of the <see cref="WebSiteControllerRule"/> class.
        /// </summary>
        /// <param name="site">The site collection.</param>
        /// <param name="page">The page to which this rule applies.  It cannot be null</param>
        /// <param name="ruleType">Type of the rule.</param>
        /// <param name="principal">The principal.</param>
        /// <param name="principalType">Type of the principal.</param>
        /// <param name="disabled">if set to <c>true</c> the rule is disabled.</param>
        /// <param name="appliesToSsl">if set to <c>true</c> the rule applies to SSL.</param>
        /// <param name="sequence">The sequence of the rule compared to other rules.</param>
        /// <param name="parent">Parent object of the WebSiteControllerRule</param>
        public WebSiteControllerRule(
            string site,
            string page,
            string ruleType,
            string principal,
            WebSiteControllerPrincipalType principalType,
            bool disabled,
            bool appliesToSsl,
            int sequence,
            SPPersistedObject parent) :
            base(String.Empty, parent)
        {
            if (String.IsNullOrEmpty(site))
            {
                throw new ArgumentNullException("siteCollection");
            } 
            
            if (String.IsNullOrEmpty(page))
            {
                throw new ArgumentNullException("page");
            }

            this.SiteCollection = site;
                
            // Trim the slash if the user put one in.  Slash is on the site collection
            if (page.StartsWith("/", StringComparison.Ordinal))
            {
                page = page.TrimStart('/');
            }

            this.page = page;
            this.ruleType = ruleType;
            this.principal = principal;
            this.principalType = principalType;
            this.appliesToSsl = appliesToSsl;
            this.disabled = disabled;
            this.sequence = sequence;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="WebSiteControllerRule"/> class.
        /// </summary>
        public WebSiteControllerRule() :
            base()
        {
        }

        #region Properties

        /// <summary>
        /// Gets the full url of the page to which this to which this rule applies.
        /// </summary>
        /// <value>The url of the page</value>
        public string SiteCollection
        {
            get { return this.siteCollection; }
            set {this.siteCollection = value; }
        }

        /// <summary>
        /// Gets the full url of the page to which this to which this rule applies.
        /// </summary>
        /// <value>The url of the page</value>
        public string Page
        {
            get { return this.page; }
        }

        /// <summary>
        /// Gets the full URL of the page to which this rule applies
        /// </summary>
        /// <value>The full URL of the page.</value>
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1056:UriPropertiesShouldNotBeStrings", Justification = "More appropriate as string")]
        public string Url
        {
            get
            {
                try
                {
                    if (!string.IsNullOrEmpty(this.siteCollection))
                    {
                        if (this.SiteCollection.EndsWith("/", StringComparison.OrdinalIgnoreCase))
                        {
                            return this.siteCollection + this.page;
                        }
                        else
                        {
                            return this.siteCollection + "/" + this.page;
                        }
                    }
                    else
                    {
                        return string.Empty;
                    }
                }
                catch (Exception)
                {
                    return String.Empty;
                }
            }
        }

        /// <summary>
        /// Gets the type of the rule.
        /// </summary>
        /// <value>The type of the rule.</value>
        public string RuleType
        {
            get
            {
                return this.ruleType;
            }
        }

        /// <summary>
        /// Gets the short type of the rule for display purposes.
        /// </summary>
        /// <value>The short type of the rule for display purposes.</value>
        public string ShortRuleType
        {
            get
            {
                try
                {

                    IWebSiteControllerModule module = WebSiteControllerConfig.GetModule((SPWebApplication)Parent.Parent, this.ruleType);
                    if (module != null)
                    {
                        return module.ShortRuleType;
                    }
                    else
                    {
                        return this.RuleType;
                    }
                }
                catch
                {
                    return string.Empty;
                }
            }
        }

        /// <summary>
        /// Gets the principal for the rule.
        /// </summary>
        /// <value>The principal for the rule.</value>
        public string Principal
        {
            get { return this.principal; }
        }

        /// <summary>
        /// Gets the type of the principal.
        /// </summary>
        /// <value>The type of the principal.</value>
        public WebSiteControllerPrincipalType PrincipalType
        {
            get { return this.principalType; }
        }

        /// <summary>
        /// Gets a value indicating whether the rule should also be applied to urls that begin with https
        /// </summary>
        /// <value>true if the rule applies to urls beginning with https; otherwise false</value>
        public bool AppliesToSsl
        {
            get
            {
                return this.appliesToSsl;
            }
        }

        /// <summary>
        /// Gets a value indicating whether the rule should be applied
        /// </summary>
        /// <value>true if the rule is not to be applied; otherwise false</value>
        public bool IsDisabled
        {
            get
            {
                return this.disabled;
            }
        }

        /// <summary>
        /// Gets the sequence.
        /// </summary>
        /// <value>The sequence.</value>
        public int Sequence
        {
            get { return this.sequence; }
        }

        #endregion

        /// <summary>
        /// Compares the by sequence.
        /// </summary>
        /// <param name="lhs">The left hand side.</param>
        /// <param name="rhs">The right hand side.</param>
        /// <returns>0 if the rules have the same sequence number; 1 if lhs greater than rhs; and -1 if lhs less than rhs</returns>
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Naming", "CA1704:IdentifiersShouldBeSpelledCorrectly", MessageId = "rhs", Justification = "Not hungarian notation and lhs and rhs are a common convention")]
        public static int CompareBySequence(WebSiteControllerRule lhs, WebSiteControllerRule rhs)
        {
            if (lhs.Sequence == rhs.Sequence)
            {
                return 0;
            }
            else if (lhs.Sequence > rhs.Sequence)
            {
                return 1;
            }
            else
            {
                return -1;
            }
        }

        /// <summary>
        /// Checks if the passed in rule is equivalent to this instance.
        /// </summary>
        /// <param name="rule">The rule to compare.</param>
        /// <returns>true if the rules have the same Url, Principal, PrincipalType and RuleType; otherwise false</returns>
        public bool Equals(WebSiteControllerRule rule)
        {
            if (rule == null)
            {
                return false;
            }

            bool baseFieldsEqual = 
                this.RuleType.Equals(rule.RuleType) &&
                this.Url.Equals(rule.Url) &&
                this.Principal.Equals(rule.Principal) &&
                this.PrincipalType.Equals(rule.PrincipalType);

            if (baseFieldsEqual)
            {
                foreach (DictionaryEntry property in rule.Properties)
                {
                    if (!this.Properties[property.Key].Equals(property.Value))
                    {
                        return false;
                    }
                }

                return true;
            }
            else
            {
                return false;
            }                    
        }

        /// <summary>
        /// Gets a string representation of the page control rule
        /// </summary>
        /// <param name="withGuid">if set to <c>true</c> [with GUID].</param>
        /// <returns>String representation of the access control rule</returns>
        public string ToString(bool withGuid)
        {
            System.Text.StringBuilder sb = new System.Text.StringBuilder();
            if (withGuid)
            {
                sb.Append("guid=\"" + this.Id.ToString() + "\" ");
            }

            sb.Append("url=\"" + this.Url + "\" ");
            sb.Append("ruletype=\"" + this.RuleType + "\" ");
            sb.Append("principal=\"" + this.Principal + "\" ");
            sb.Append("principal=\"" + this.PrincipalType.ToString() + "\" ");
            return sb.ToString();
        }

        /// <summary>
        /// Gets a string representation of the access control rule
        /// </summary>
        /// <returns>
        /// A string representation of the access control rule, with the GUID
        /// </returns>
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Security", "CA2123:OverrideLinkDemandsShouldBeIdenticalToBase", Justification = "Needed to suppress another warning")]
        public override string ToString()
        {
            return this.ToString(true);
        }

        protected override bool HasAdditionalUpdateAccess()
        {
            return true;
        }
    }
}
