// Copyright Hemrika. All rights reserved.
// </copyright>
// <author>SEO\Administrator</author>
// <date>2011-09-09 20:48:32Z</date>
namespace Hemrika.SharePresence.Common.WebSiteController
{
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using System.Collections.ObjectModel;
    using System.Text.RegularExpressions;
    using Microsoft.SharePoint;
    using Microsoft.SharePoint.Administration;
    using System.Web;
    using System.Runtime.InteropServices;

    /// <summary>
    /// A collection of WebSite Control rules and modules that serves as the root of the WebSiteControllerRulesCollection and WebSiteControllerModulesCollection
    /// </summary>
    public sealed class WebSiteControllerConfig : SPPersistedObject
    {
        /// <summary>
        /// The guid of the WebSiteControllerConfig in the config database
        /// </summary>
        internal static readonly Guid ID = new Guid("B6A41E80-4F05-4089-9C5C-6504335A0B66");

        /// <summary>
        /// The name used when persisting the WebSiteControllerConfig to the config database
        /// </summary>
        public const string OBJECTNAME = "WebSiteControllerConfig";

        private static SPWebApplication WebApp;
        /// <summary>
        /// Collection of WebSiteControllerRules
        /// </summary>
        private WebSiteControllerRulesCollection rules;

        /// <summary>
        /// Collection of Page controller modules
        /// </summary>
        private WebSiteControllerModulesCollection modules;

        /// <summary>
        /// Initializes a new instance of the <see cref="WebSiteControllerConfig"/> class.
        /// </summary>
        public WebSiteControllerConfig()
            : base(OBJECTNAME, SPFarm.Local, ID)
        {
            try
            {
                this.rules = new WebSiteControllerRulesCollection(this);
                this.modules = new WebSiteControllerModulesCollection(this);
            }
            catch (Exception ex)
            {
                SPDiagnosticsService.Local.WriteTrace(0, new SPDiagnosticsCategory(ex.Source, TraceSeverity.High, EventSeverity.Error), TraceSeverity.High, ex.Message, ex.Data);
                //ex.ToString();
            }
        }

        /*
        /// <summary>
        /// Initializes a new instance of the <see cref="WebSiteControllerConfig"/> class.
        /// </summary>
        /// <param name="persisted"></param>
        public WebSiteControllerConfig(SPWebApplication webApp)//
            : base(OBJECTNAME, webApp, ID)
        {
            try
            {
                this.rules = new WebSiteControllerRulesCollection(this);
                this.modules = new WebSiteControllerModulesCollection(this);
            }
            catch (Exception ex)
            {
                ex.ToString();
            }
        }
        */
        
        public WebSiteControllerConfig(SPPersistedObject persisted, Guid guid) : base(OBJECTNAME,persisted,guid)
        {
            try
            {
                this.rules = new WebSiteControllerRulesCollection(this);
                this.modules = new WebSiteControllerModulesCollection(this);
            }
            catch (Exception ex)
            {
                SPDiagnosticsService.Local.WriteTrace(0, new SPDiagnosticsCategory(ex.Source, TraceSeverity.High, EventSeverity.Error), TraceSeverity.High, ex.Message, ex.Data);
                //ex.ToString();
            }
        }

        protected override bool HasAdditionalUpdateAccess()
        {
            return true;
        }

        /// <summary>
        /// Gets the collection of modules.
        /// </summary>
        /// <value>The collection of modules.</value>
        public static Collection<IWebSiteControllerModule> Modules
        //public static Collection<WebSiteControllerModule> Modules
        {
            get
            {
                Collection<IWebSiteControllerModule> modules = new Collection<IWebSiteControllerModule>();
                //Collection<WebSiteControllerModule> modules = new Collection<WebSiteControllerModule>();
                try
                {
                    foreach (PersistedWebSiteControllerModule module in GetFromConfigDB().modules)
                    {
                        IWebSiteControllerModule imodule = GetModuleFromClassName(module.FullyQualifiedClassName);
                        imodule.Id = module.Id;
                        //modules.Add(GetModuleFromClassName(module.FullyQualifiedClassName));
                        modules.Add(imodule);
                    }
                }
                catch (Exception ex)
                {
                    SPDiagnosticsService.Local.WriteTrace(0, new SPDiagnosticsCategory(ex.Source, TraceSeverity.High, EventSeverity.Error), TraceSeverity.High, ex.Message, ex.Data);
                    //ex.ToString();
                }
                return modules;
            }
        }

        /// <summary>
        /// Gets the collection of WebSiteControllerRules
        /// </summary>
        /// <returns>An collection of WebSiteControllerRules</returns>
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1002:DoNotExposeGenericLists", Justification = "Need to sort, so using list")]
        public static List<WebSiteControllerRule> Rules
        {
            get
            {
                List<WebSiteControllerRule> rules = new List<WebSiteControllerRule>();
                try
                {
                    foreach (WebSiteControllerRule rule in GetFromConfigDB().rules)
                    {
                        rules.Add(rule);
                    }

                    rules.Sort(WebSiteControllerRule.CompareBySequence);
                }
                catch (Exception ex)
                {
                    SPDiagnosticsService.Local.WriteTrace(0, new SPDiagnosticsCategory(ex.Source, TraceSeverity.High, EventSeverity.Error), TraceSeverity.High, ex.Message, ex.Data);
                    //ex.ToString();
                }
                return rules;
            }
        }

        /// <summary>
        /// Adds the module.
        /// </summary>
        /// <param name="fullyQualifiedClassName">Name of the fully qualified class of the module being added.</param>
        public static void AddModule(string fullyQualifiedClassName)
        {
            WebSiteControllerConfig config = GetFromConfigDB();

            IWebSiteControllerModule module = GetModuleFromClassName(fullyQualifiedClassName);

            config.modules.Add(new PersistedWebSiteControllerModule(Guid.NewGuid(), module.RuleType, fullyQualifiedClassName, config));
        }

        public static void AddModule(IWebSiteControllerModule module, string fullyQualifiedClassName)
        {
                WebSiteControllerConfig config = GetFromConfigDB();

                PersistedWebSiteControllerModule mod = new PersistedWebSiteControllerModule(Guid.NewGuid(), module.RuleType, fullyQualifiedClassName, config);
                mod.Update();
                
                //config.modules.Add(new PersistedWebSiteControllerModule(module.Id, module.RuleType, fullyQualifiedClassName, config));
        }

        public static void AddModule(SPWebApplication Webapp, IWebSiteControllerModule module, string fullyQualifiedClassName)
        {
            try
            {
                WebApp = Webapp;
                WebSiteControllerConfig config = GetFromConfigDB();

                PersistedWebSiteControllerModule mod = new PersistedWebSiteControllerModule(Guid.NewGuid(), module.RuleType, fullyQualifiedClassName, config);
                mod.Update();
            }
            catch (Exception ex)
            {
                SPDiagnosticsService.Local.WriteTrace(0, new SPDiagnosticsCategory(ex.Source, TraceSeverity.High, EventSeverity.Error), TraceSeverity.High, ex.Message, ex.Data);
                //ex.ToString();
            }

            //config.modules.Add(new PersistedWebSiteControllerModule(module.Id, module.RuleType, fullyQualifiedClassName, config));
        }
        /// <summary>
        /// Gets the module.
        /// </summary>
        /// <param name="ruleType">Type of the rule.</param>
        /// <returns>A Page Controller module</returns>
        public static IWebSiteControllerModule GetModule(SPWebApplication Webapp, string ruleType)
        {
            WebApp = Webapp;
            foreach (IWebSiteControllerModule module in Modules)
            {
                if (module.RuleType.Equals(ruleType, StringComparison.OrdinalIgnoreCase))
                {
                    return module;
                }
            }

            return null;
        }

        /// <summary>
        /// Gets the module.
        /// </summary>
        /// <param name="id">The id of the module</param>
        /// <returns>A page controller module</returns>
        public static IWebSiteControllerModule GetModule(SPWebApplication Webapp, Guid id)
        {
            WebApp = Webapp;
            PersistedWebSiteControllerModule module = GetFromConfigDB().modules[id] as PersistedWebSiteControllerModule;
            if (module != null)
            {
                IWebSiteControllerModule imodule = GetModuleFromClassName(module.FullyQualifiedClassName);
                imodule.Id = id;
                return imodule;// GetModuleFromClassName(module.FullyQualifiedClassName);
            }
            return null;
        }

        /// <summary>
        /// Removes the module.
        /// </summary>
        /// <param name="id">The id of the module.</param>
        public static void RemoveModule(SPWebApplication Webapp, Guid id)
        {
            WebApp = Webapp;
            IWebSiteControllerModule module = GetModule(Webapp, id);
            GetFromConfigDB().modules.Remove(id);

            // Disable all rules for this module
            foreach (WebSiteControllerRule rule in Rules)
            {
                if (rule.RuleType.Equals(module.RuleType, StringComparison.OrdinalIgnoreCase))
                {
                    RemoveRule(rule);
                    AddRule(
                        Webapp,
                        rule.SiteCollection,
                        rule.Page,
                        rule.RuleType,
                        rule.Principal,
                        rule.PrincipalType,
                        true,
                        rule.AppliesToSsl,
                        rule.Sequence,
                        rule.Properties);
                }
            }
        }

        /// <summary>
        /// Adds a new WebSiteControllerRule.
        /// </summary>
        /// <param name="siteCollection">The site collection.</param>
        /// <param name="page">The page to which the rule applied</param>
        /// <param name="ruleType">Type of the rule.</param>
        /// <param name="principal">The principal.</param>
        /// <param name="principalType">Type of the principal.</param>
        /// <param name="disabled">if set to <c>true</c> the rule is disabled.</param>
        /// <param name="appliesToSsl">if set to <c>true</c> the rule applies to SSL.</param>
        /// <param name="sequence">The sequence of the rule compared to other rules.</param>
        /// <param name="properties">The properties.</param>
        /// <returns>The resulting WebSiteControllerRule</returns>
        public static WebSiteControllerRule AddRule(
                                    SPWebApplication Webapp,
                                    string siteCollection,
                                    string page,
                                    string ruleType,
                                    string principal,
                                    WebSiteControllerPrincipalType principalType,
                                    bool disabled,
                                    bool appliesToSsl,
                                    int sequence,
                                    Hashtable properties)
        {
            WebApp = Webapp;
            WebSiteControllerConfig config = GetFromConfigDB();
            WebSiteControllerRule rule = new WebSiteControllerRule(
                                        siteCollection,
                                        page,
                                        ruleType,
                                        principal,
                                        principalType,
                                        disabled,
                                        appliesToSsl,
                                        sequence,
                                        config);

            rule.Name = rule.Id.ToString();

            if (properties != null)
            {
                foreach (DictionaryEntry property in properties)
                {
                    rule.Properties.Add(property.Key, property.Value);
                }
            }

            if (!IsDuplicateRule(rule))
            {
                config.rules.Add(rule);
                config.Update();
            }
            else
            {
                throw new SPDuplicateObjectException(rule + " is the same as another rule in the WebSiteControllerRules collection");
            }

            return rule;
        }

        static object createlock = new object();
        //private SPWebApplication WebApp;
        //private Guid guid;

        /// <summary>
        /// Creates the WebSiteControllerRules in config DB.
        /// </summary>
        public static void CreateInConfigDB()
        {
            if (!Exists())
            {
                if (HttpContext.Current != null)
                {
                    try
                    {
                        SPSite site = new SPSite(HttpContext.Current.Request.Url.OriginalString);
                        WebApp = site.WebApplication;
                    }
                    catch { };
                }

                if (WebApp != null && WebApp != SPAdministrationWebApplication.Local)
                {
                    lock (createlock)
                    {

                        WebSiteControllerConfig settings = new WebSiteControllerConfig(WebApp,Guid.NewGuid());
                        settings.Update();
                    }
                }

                /*
                else
                {
                    lock (createlock)
                    {

                        WebSiteControllerConfig settings = new WebSiteControllerConfig();
                        settings.Update();
                    }
                }
                */
            }
        }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1011:ConsiderPassingBaseTypesAsParameters", Justification = "Overloaded")]

        /// <summary>
        /// Determines whether the WebSiteControllerRules contains a rule
        /// </summary>
        /// <param name="rule">The id of the rule</param>
        /// <returns>
        ///      <c>true</c> if the specified rules exists; otherwise, <c>false</c>.
        /// </returns>
        public static bool ContainsRule(WebSiteControllerRule rule)
        {
            return ContainsRule(rule.Id.ToString());
        }

        /// <summary>
        /// Determines whether the WebSiteControllerRules contains a rule
        /// </summary>
        /// <param name="id">The id of the rule</param>
        /// <returns>         
        ///     <c>true</c> if the specified rules exists; otherwise, <c>false</c>.
        /// </returns>
        public static bool ContainsRule(string id)
        {
            Guid guid = new Guid(id);
            foreach (WebSiteControllerRule rule in GetFromConfigDB().rules)
            {
                if (rule.Id.Equals(guid))
                {
                    return true;
                }
            }

            return false;
        }

        /// <summary>
        /// Checks if the WebSiteControllerRules exist
        /// </summary>
        /// <returns>true if they exist; otherwise false</returns>
        public static bool Exists()
        {

            if (WebApp ==  null && HttpContext.Current != null)// && HttpContext.Current.Request != null)
            {
                HttpRequest request = HttpContext.Current.Request;

                string url = HttpContext.Current.Request.Url.OriginalString;
                //try
                //{
                if (SPSite.Exists(HttpContext.Current.Request.Url))
                {
                    SPSite site = new SPSite(HttpContext.Current.Request.Url.OriginalString);
                    WebApp = site.WebApplication;
                }
                //}
                //catch { return false; };
            }

            if (WebApp != null)
            {
                if (WebApp != SPAdministrationWebApplication.Local)
                {
                    WebSiteControllerConfig config = WebApp.GetChild<WebSiteControllerConfig>(OBJECTNAME);
                    return (config != null);
                }
                /*
                else
                {

                    return SPFarm.Local.GetObject(ID) != null;
                }
                */
            }

            return false;
            /*
            try
            {
                return SPContext.Current.Site.WebApplication.GetChild<WebSiteControllerConfig>(OBJECTNAME) != null;
            }
            catch (Exception ex)
            {
                //WebSiteControllerConfig config = SPContext.Current.Site.WebApplication.GetChild<WebSiteControllerConfig>();
                //config.Delete();
            }
            return false;            
            */
        }

        /// <summary>
        /// Removes the rule.
        /// </summary>
        /// <param name="rule">The rule.</param>
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1011:ConsiderPassingBaseTypesAsParameters", Justification = "Overloaded")]

        /// <summary>
        /// Removes the rule.
        /// </summary>
        /// <param name="rule">The rule to remove</param>
        public static void RemoveRule(WebSiteControllerRule rule)
        {
            RemoveRule(rule.Id.ToString());
        }

        /// <summary>
        /// Removes the rule.
        /// </summary>
        /// <param name="id">The id of the rule to remove</param>
        public static void RemoveRule(string id)
        {
            WebSiteControllerConfig rules = GetFromConfigDB();
            rules.rules.Remove(new Guid(id));
            rules.Update();
        }

        /// <summary>
        /// Removes the rule.
        /// </summary>
        /// <param name="id">The id of the rule to remove</param>
        public static void RemoveRule(Guid id)
        {
            WebSiteControllerConfig rules = GetFromConfigDB();
            rules.rules.Remove(id);
            rules.Update();
        }
        /// <summary>
        /// Deletes all WebSiteControllerRules from the config db
        /// </summary>
        public static void DeleteFromConfigDB()
        {
            GetFromConfigDB().Delete();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        public static WebSiteControllerRule GetRule(string name)
        {
            return GetFromConfigDB().rules[name];
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="propertie"></param>
        /// <param name="name"></param>
        /// <returns></returns>
        public static WebSiteControllerRule GetRule(string propertie,string name)
        {
            WebSiteControllerRule _rule = null;
            int code = 100;
            bool NaN = int.TryParse(name,out code);
            if (!NaN || code > 299)
            {
                WebSiteControllerRulesCollection rules = GetFromConfigDB().rules;
                foreach (WebSiteControllerRule rule in rules)
                {
                    try
                    {
                        if (rule.Properties.ContainsValue(name))
                        {
                            if (rule.Properties[propertie].Equals(name))
                            {
                                _rule = rule;
                                break;
                            }
                        }
                    }
                    catch (Exception ex)
                    {
                        ex.ToString();
                    }
                }
            }
            return _rule;
            //return GetFromConfigDB().rules[name];
        }

        public static WebSiteControllerRule GetRule(SPWebApplication Webapp, Guid guid)
        {
            WebApp = Webapp;
            return GetFromConfigDB().rules[guid];
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="Webapp"></param>
        /// <param name="url"></param>
        /// <param name="ruleType"></param>
        /// <returns></returns>
        public static bool HasRule(SPWebApplication Webapp, Uri url, string ruleType)
        {
            WebSiteControllerRulesCollection rules = GetFromConfigDB().rules;
            foreach (WebSiteControllerRule rule in rules)
            {
                try
                {
                    if (rule.RuleType == ruleType && rule.Url == url.ToString())
                    {
                        return true;
                    }
                }
                catch (Exception ex)
                {
                    SPDiagnosticsService.Local.WriteTrace(0, new SPDiagnosticsCategory(ex.Source, TraceSeverity.High, EventSeverity.Error), TraceSeverity.High, ex.Message, ex.Data);
                    //ex.ToString();
                }
            }

            return false;
            //return GetFromConfigDB().rules[name];
        }

        /// <summary>
        /// Gets the rule.
        /// </summary>
        /// <param name="id">The id of the rule</param>
        /// <returns>The WebSiteControllerRule</returns>
        public static WebSiteControllerRule GetRule(SPWebApplication Webapp, Uri url, string ruleType)
        {
            WebSiteControllerRule darule = null;
            WebSiteControllerRulesCollection rules = GetFromConfigDB().rules;
            foreach (WebSiteControllerRule rule in rules)
            {
                try
                {
                    if (rule.RuleType == ruleType && rule.Url == url.ToString())
                    {
                        darule = rule;
                    }
                }
                catch (Exception ex)
                {
                    SPDiagnosticsService.Local.WriteTrace(0, new SPDiagnosticsCategory(ex.Source, TraceSeverity.High, EventSeverity.Error), TraceSeverity.High, ex.Message, ex.Data);
                    //ex.ToString();
                }
            }

            return darule;
            //return GetFromConfigDB().rules[name];
        }

        /// <summary>
        /// Determines whether the specified page has associated Page Control rules of the given type.
        /// </summary>
        /// <param name="url">The url to the page</param>
        /// <param name="ruleType">Type of the rule.</param>
        /// <param name="user">The current user.</param>
        /// <returns>
        ///      <c>true</c> if the page specified by the url has rules otherwise, <c>false</c>.
        /// </returns>
        public static bool IsPageControlled(Uri url, string ruleType, SPUser user)
        {
            bool found = false;
            WebSiteControllerRulesCollection rules = GetFromConfigDB().rules;
            foreach (WebSiteControllerRule rule in rules)
            {
                if (IsSinglePageControlled(rule, url, ruleType, user))
                {
                    found = true;
                    break;
                }
            }

            return found;
        }

        /// <summary>
        /// Determines whether the specified page has any associated Page Control rules.
        /// </summary>
        /// <param name="url">The url to the page</param>
        /// <param name="ruleType">Type of the rule.</param>
        /// <returns>
        ///      <c>true</c> if the page specified by the url has rules otherwise, <c>false</c>.
        /// </returns>
        public static bool IsPageControlled(Uri url, string ruleType)
        {
            return IsPageControlled(url, ruleType, WebSiteControllerModule.GetUser(url.ToString()));
        }

        /// <summary>
        /// Determines whether the specified page has any associated Page Control rules.
        /// </summary>
        /// <param name="url">The url to the page</param>
        /// <param name="ruleType">Type of the rule.</param>
        /// <returns>
        ///      <c>true</c> if the page specified by the url has rules otherwise, <c>false</c>.
        /// </returns>
        public static bool IsPageControlled(SPWebApplication Webapp, Uri url, string ruleType)
        {
            WebApp = Webapp;
            return IsPageControlled(url, ruleType, WebSiteControllerModule.GetUser(url.ToString()));
        }

        /// <summary>
        /// Gets the rules for site collection.
        /// </summary>
        /// <param name="url">The URL of the site collection.</param>
        /// <param name="ruleType">Type of the rule</param>
        /// <returns>Coillection of rules for this site collection and rule type</returns>
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1002:DoNotExposeGenericLists", Justification = "Need to sort, so using list")]
        public static List<WebSiteControllerRule> GetRulesForSiteCollection(Uri url, string ruleType)
        {
            WebSiteControllerRulesCollection rules = GetFromConfigDB().rules;
            List<WebSiteControllerRule> list = new List<WebSiteControllerRule>();
            foreach (WebSiteControllerRule rule in rules)
            {
                if (rule.SiteCollection.Equals(url.ToString(), StringComparison.OrdinalIgnoreCase) &&
                    (String.IsNullOrEmpty(ruleType) || rule.RuleType.Equals(ruleType, StringComparison.OrdinalIgnoreCase)))
                {
                    list.Add(rule);
                }
            }

            list.Sort(WebSiteControllerRule.CompareBySequence);
            return list;
        }

        /// <summary>
        /// Gets the rules for site collection.
        /// </summary>
        /// <param name="url">The URL of the site collection</param>
        /// <returns>A collection of rules of all types for this site collection</returns>
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1002:DoNotExposeGenericLists", Justification = "Need to sort, so using list")]
        public static List<WebSiteControllerRule> GetRulesForSiteCollection(Uri url)
        {
            return GetRulesForSiteCollection(url, String.Empty);
        }

        /// <summary>
        /// Gets the page control rules associated with this url of the specified type.
        /// </summary>
        /// <param name="url">The URL for which to get the rules</param>
        /// <param name="ruleType">Type of the rule.</param>
        /// <param name="user">The current user.</param>
        /// <returns>
        /// A collection of WebSiteControllerRule object
        /// </returns>
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1002:DoNotExposeGenericLists", Justification = "Need to sort, so using list")]
        public static List<WebSiteControllerRule> GetRulesForPage(SPWebApplication Webapp, Uri url, string ruleType, SPUser user)
        {
            WebApp = Webapp;
            List<WebSiteControllerRule> list = new List<WebSiteControllerRule>();
            if (url == null)
            {
                return list;
            }

            WebSiteControllerModulesCollection modules = GetFromConfigDB().modules;

            foreach (PersistedWebSiteControllerModule module in modules)
            {
                IWebSiteControllerModule imodule = GetModule(Webapp, module.Id);
                if (imodule.AlwaysRun)
                {
                    try
                    {

                        WebSiteControllerRule _rule = GetRule(imodule.RuleType);
                        if (_rule == null)
                        {
                            _rule = new WebSiteControllerRule();

                        }
                        //WebSiteControllerRule temp = new WebSiteControllerRule(SPContext.Current.Site.Url, url.ToString(), ruleType, String.Empty, WebSiteControllerPrincipalType.None, false, true, 0, _rule.Parent);
                        list.Add(_rule);
                    }
                    catch (Exception ex)
                    {
                        SPDiagnosticsService.Local.WriteTrace(0, new SPDiagnosticsCategory(ex.Source, TraceSeverity.High, EventSeverity.Error), TraceSeverity.High, ex.Message, ex.Data);
                        //ex.ToString();
                    }
                }
            }

            WebSiteControllerRulesCollection rules = GetFromConfigDB().rules;
            foreach (WebSiteControllerRule rule in rules)
            {
                if (IsSinglePageControlled(rule, url, ruleType, user))
                {
                    list.Add(rule);
                }
            }
            
            list.Sort(WebSiteControllerRule.CompareBySequence);
            return list;
        }

        /// <summary>
        /// Gets the rules for a specific URL.
        /// </summary>
        /// <param name="url">The URL being checked.</param>
        /// <param name="ruleType">Type of the rules to retrieve.</param>
        /// <returns>A list of <see cref="WebSiteControllerRule"/>s that apply</returns>
        public static List<WebSiteControllerRule> GetRulesForPage(SPWebApplication Webapp, Uri url, string ruleType)
        {
            return GetRulesForPage(Webapp,url, ruleType, WebSiteControllerModule.GetUser(url.ToString()));
        }

        /// <summary>
        /// Gets the page control rules associated with this url of the specified type.
        /// </summary>
        /// <param name="url">The URL for which to get the rules</param>
        /// <returns>A collection of WebSiteControllerRule object</returns>
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1002:DoNotExposeGenericLists", Justification = "Need to sort, so using list")]
        public static List<WebSiteControllerRule> GetRulesForPage(SPWebApplication Webapp, Uri url)
        {
            return GetRulesForPage(WebApp,url, String.Empty, WebSiteControllerModule.GetUser(url.ToString()));
        }

        /// <summary>
        /// Determines whether the rule already exists in the rule collection.
        /// </summary>
        /// <param name="rule">The rule being checked</param>
        /// <returns>
        ///      <c>true</c> if the rule is a duplicate; otherwise, <c>false</c>.
        /// </returns>
        public static bool IsDuplicateRule(WebSiteControllerRule rule)
        {
            foreach (WebSiteControllerRule ruleToCheck in
                     GetRulesForSiteCollection(new Uri(rule.SiteCollection), rule.RuleType))
            {
                if (ruleToCheck.Equals(rule))
                {
                    return true;
                }
            }

            return false;
        }

        /// <summary>
        /// Does the entry apply to user.
        /// </summary>
        /// <param name="rule">The rule to check</param>
        /// <param name="user">The current user.</param>
        /// <returns>
        ///      <c>true</c> if the entry applies to the user; otherwise <c>false</c>
        /// </returns>
        private static bool DoesRuleApplyToUser(WebSiteControllerRule rule, SPUser user)
        {
            if (rule.PrincipalType == WebSiteControllerPrincipalType.None)
            {
                return true;
            }

            if (rule.PrincipalType == WebSiteControllerPrincipalType.User)
            {
                if (user != null &&
                    user.LoginName.Equals(rule.Principal, StringComparison.OrdinalIgnoreCase))
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }
            else if (rule.PrincipalType == WebSiteControllerPrincipalType.Group && user != null)
            {
                foreach (SPGroup group in user.Groups)
                {
                    if (group.Name.Equals(rule.Principal, StringComparison.OrdinalIgnoreCase))
                    {
                        return true;
                    }
                }
            }

            return false;
        }

        /// <summary>
        /// Gets the name of the module from class.
        /// </summary>
        /// <param name="fullyQualifiedClassName">Name of the class.</param>
        /// <returns>A page controller module</returns>
        private static IWebSiteControllerModule GetModuleFromClassName(string fullyQualifiedClassName)
        {
            // Only want to use the namespace qualified type, not the full type, which is assembly dependent
            // This makes upgrades difficult is versioning is done on the dll
            // Type moduleType = Type.GetType(Type.GetType(className).FullName);
            return (IWebSiteControllerModule)Activator.CreateInstance(Type.GetType(fullyQualifiedClassName));
        }

        /// <summary>
        /// Gets the rules from config DB.
        /// </summary>
        /// <returns>The WebSiteControllerRules</returns>
        private static WebSiteControllerConfig GetFromConfigDB()
        {
            if (!Exists())
            {
                CreateInConfigDB();
            }

            if (WebApp == null && HttpContext.Current != null)
            {
                SPSite site = new SPSite(HttpContext.Current.Request.Url.OriginalString);
                WebApp = site.WebApplication;
            }

            if (WebApp != null && WebApp != SPAdministrationWebApplication.Local)
            {
                return WebApp.GetChild<WebSiteControllerConfig>(OBJECTNAME);
            }
            else
            {
                return null;
                //return SPFarm.Local.GetObject(ID) as WebSiteControllerConfig;
            }            
        }

        /// <summary>
        /// Determines whether a specfic rule applies to a page.
        /// </summary>
        /// <param name="rule">The rule being checked.</param>
        /// <param name="url">The page URL.</param>
        /// <param name="ruleType">Type of the rule.</param>
        /// <param name="user">The current user.</param>
        /// <returns>
        ///      <c>true</c> if the page has a rule associated with it of the rule type; otherwise, <c>false</c>.
        /// </returns>
        private static bool IsSinglePageControlled(WebSiteControllerRule rule, Uri url, string ruleType, SPUser user)
        {
            if (rule == null)
            {
                return false;
            }

            if (rule.IsDisabled)
            {
                return false;
            }

            if (String.IsNullOrEmpty(ruleType) || rule.RuleType.Equals(ruleType, StringComparison.InvariantCultureIgnoreCase))
            {
                Regex regEx = new Regex("^" + rule.Url.ToUpperInvariant());
                /*
                if (rule.Properties.ContainsKey("OriginalUrl"))
                {
                    string OriginalUrl = rule.Properties["OriginalUrl"].ToString();
                    regEx = new Regex("^" + OriginalUrl.ToUpperInvariant());

                }
                else
                {
                    regEx = new Regex("^" + rule.Url.ToUpperInvariant());
                }
                */
                // Put anchor at beginning
                //Regex regEx = new Regex(rule.Url.ToUpperInvariant());
                bool ruleAppliesToUser = DoesRuleApplyToUser(rule, user);

                if (regEx != null)
                {
                    Match m = regEx.Match(url.ToString().ToUpperInvariant());

                    while (m.Success)
                    {
                        //do things with your matching text  

                        if (rule.Url.ToUpperInvariant() == m.Value.ToUpperInvariant())
                        {
                            return true;
                        }
                        m = m.NextMatch();
                    }
                    /*
                    if (regEx.IsMatch(url.ToString().ToUpperInvariant()) && ruleAppliesToUser)
                    {
                        return true;
                    }
                    */

                    if (rule.AppliesToSsl && url.ToString().StartsWith("https://", StringComparison.InvariantCultureIgnoreCase))
                    {
                        if (regEx.IsMatch(url.ToString().Replace("https://", "http://").ToUpperInvariant()) && ruleAppliesToUser)
                        {
                            return true;
                        }
                    }
                }
            }

            return false;
        }

        private static SPUserToken GetSysToken(Guid SPSiteID)
        {
            SPUserToken sysToken = new SPSite(SPSiteID).SystemAccount.UserToken;
            if (sysToken == null)
            {
                SPSecurity.RunWithElevatedPrivileges(
                delegate()
                {
                    using (SPSite site = new SPSite(SPSiteID))
                    {
                        sysToken = site.SystemAccount.UserToken;
                    }
                });
            }
            return sysToken;
        }

    }
}