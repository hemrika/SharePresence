// -----------------------------------------------------------------------
// <copyright file="WebPageSetHomePageHandler.cs" company="">
// TODO: Update copyright text.
// </copyright>
// -----------------------------------------------------------------------

namespace Hemrika.SharePresence.WebSite.Page.Handlers
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using Microsoft.SharePoint.WebControls;
    using System.Web.UI;
    using Microsoft.SharePoint;
    using Microsoft.SharePoint.Utilities;
    using Hemrika.SharePresence.Common.WebSiteController;
    using Hemrika.SharePresence.WebSite.Modules.SemanticModule;

    /// <summary>
    /// TODO: Update summary.
    /// </summary>
    public class WebPageSetHomePageHandler : SPRibbonCommandHandler, ICallbackEventHandler//, IPostBackEventHandler
    {
        public WebPageSetHomePageHandler(SPPageStateControl psc) : base(psc)
        {
        }

        protected override void OnLoad(EventArgs e)
        {
            base.OnLoad(e);
        }
        public string GetCallbackResult()
        {
            SPListItem listItem = SPContext.Current.ListItem;
            try
            {
                if ((listItem != null) && (listItem.File != null))
                {
                    SPFolder folder = SPContext.Current.Web.RootFolder;
                    List<WebSiteControllerRule> rules;
                    string url = SPContext.Current.Site.Url+"/";
                    WebSiteControllerRule homepage = null;

                    if (!SPContext.Current.Web.IsRootWeb)
                    {
                        url = url.TrimEnd(new char[1] { '/' });
                        url += SPContext.Current.Web.ServerRelativeUrl;
                    }

                    rules = WebSiteControllerConfig.GetRulesForPage(SPContext.Current.Site.WebApplication, new Uri(url));

                    foreach (WebSiteControllerRule rule in rules)
                    {
                        if (rule.Url == url)
                        {
                            homepage = rule;
                            break;
                        }
                        else
                        {
                            if (rule.Url == string.Empty)
                            {
                                try
                                {
                                    //if (string.IsNullOrEmpty(rule.ShortRuleType))
                                    //{
                                        rule.Delete();
                                    //}
                                }
                                catch (Exception ex)
                                {
                                    ex.ToString();
                                 
                                }
                            }
                        }
                    }

                    if (homepage != null)
                    {
                        CreateWorkItem(homepage, listItem.File.ServerRelativeUrl);
                    }
                    else
                    {
                        CreateWorkItem(listItem.File.ServerRelativeUrl);
                    }


                    folder.WelcomePage = listItem.File.Url;
                    folder.Update();
                }
            }
            catch (SPException exception)
            {
                base.SetGenericErrorMessage(exception);
            }

            try
            {
                SPContext.Current.Web.Title = SPContext.Current.ListItem.Title;
                SPContext.Current.Web.Update();
            }
            catch (SPException exception)
            {
                base.SetGenericErrorMessage(exception);
            }

            base.RefreshPageState();
            return base.BuildReturnValue("HomePage has been set");
        }

        private void CreateWorkItem(string url)
        {
            Guid siteId = SPContext.Current.Site.ID;
            Guid webId = SPContext.Current.Web.ID;

            bool disabled = false;
            WebSiteControllerPrincipalType principalType = WebSiteControllerPrincipalType.None;
            bool appliesToSSL = true;
            int sequence = 1;
            String pricipal = string.Empty;

            StringBuilder builder = new StringBuilder();
            builder.Append(SPContext.Current.Web.ServerRelativeUrl + ";");
            builder.Append(disabled.ToString() + ";");
            builder.Append(appliesToSSL.ToString() + ";");
            builder.Append(sequence.ToString() + ";");
            builder.Append(principalType.ToString() + ";");
            builder.Append(pricipal + ";");
            builder.Append("#");

            builder.Append(String.Format("{0}:{1};", "OriginalUrl", url));

            string full = builder.ToString();

            SemanticModule mod = new SemanticModule();
            IWebSiteControllerModule imod = null;// WebSiteControllerConfig.GetModule(web.Site.WebApplication, mod.RuleType);

            while (imod == null)
            {
                System.Threading.Thread.Sleep(1000);
                try
                {
                    imod = WebSiteControllerConfig.GetModule(SPContext.Current.Site.WebApplication, mod.RuleType);
                }
                catch (Exception ex)
                {
                    ex.ToString();
                }
            }


            int item = -1;

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
                        imod.Id,
                        Guid.Empty,
                        site.SystemAccount.ID,
                        null,
                        builder.ToString(),
                        Guid.Empty
                        );
                }
            });

            SPSecurity.RunWithElevatedPrivileges(delegate()
            {
                using (SPSite site = new SPSite(siteId))//, SPUserToken.SystemAccount))
                {
                    try
                    {

                        WebSiteControllerRuleWorkItem WebSiteControllerModuleJob = new WebSiteControllerRuleWorkItem(WebSiteControllerRuleWorkItem.WorkItemJobDisplayName + "HomePage", site.WebApplication);
                        SPOneTimeSchedule oneTimeSchedule = new SPOneTimeSchedule(DateTime.Now);

                        WebSiteControllerModuleJob.Schedule = oneTimeSchedule;
                        WebSiteControllerModuleJob.Update();
                    }
                    catch { };
                }
            });

        }

        private void CreateWorkItem(WebSiteControllerRule rule, string url)
        {
            Guid siteId = SPContext.Current.Site.ID;
            Guid webId = SPContext.Current.Web.ID;

            if (url.StartsWith("/"))
            {
                url = url.TrimStart('/');
            }

            StringBuilder builder = new StringBuilder();
            builder.Append(SPContext.Current.Web.ServerRelativeUrl + ";");
            builder.Append(rule.IsDisabled.ToString() + ";");
            builder.Append(rule.AppliesToSsl.ToString() + ";");
            builder.Append(rule.Sequence.ToString() + ";");
            builder.Append(rule.PrincipalType + ";");
            builder.Append(rule.Principal + ";");
            builder.Append("#");
            builder.Append(String.Format("{0}:{1};", "OriginalUrl", "/" + url));

            int item = 2;

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
                        rule.Id,
                        Guid.Empty,
                        site.SystemAccount.ID,
                        null,
                        builder.ToString(),
                        Guid.Empty
                        );
                }
            });

            SPSecurity.RunWithElevatedPrivileges(delegate()
            {
                using (SPSite site = new SPSite(siteId))//, SPUserToken.SystemAccount))
                {
                    try
                    {

                        WebSiteControllerRuleWorkItem WebSiteControllerModuleJob = new WebSiteControllerRuleWorkItem(WebSiteControllerRuleWorkItem.WorkItemJobDisplayName + "HomePage", site.WebApplication);
                        SPOneTimeSchedule oneTimeSchedule = new SPOneTimeSchedule(DateTime.Now);

                        WebSiteControllerModuleJob.Schedule = oneTimeSchedule;
                        WebSiteControllerModuleJob.Update();
                    }
                    catch { };
                }
            });

        }

        public void RaiseCallbackEvent(string eventArgument)
        {
        }

        public List<IRibbonCommand> Commands
        {
            get
            {
                List<IRibbonCommand> commands = new List<IRibbonCommand>();

                SPRibbonCommand command = new SPRibbonCommand(ClientSideCommandId, HandlerCommand,IsEnabledHandler);
                commands.Add(command);

                return commands;
            }
        }

        // Properties
        public override string ClientSideCommandId
        {
            get
            {
                return "PageStateGroupSetHomePage";
            }
        }

        public override string HandlerCommand
        {
            get
            {
                return (this.CallbackPreamble + this.Page.ClientScript.GetCallbackEventReference(this, "'" + this.ClientSideCommandId + "'", "SP.Ribbon.PageState.Handlers.GenericCompleteHandler", "SP.Ribbon.PageState.Handlers.GenericError", true));
                //return base.DefaultCallbackHandlerCommand;
            }
        }

        public override string IsEnabledHandler
        {
            get
            {
                return base.IsEnabledHandler;// "SP.Ribbon.PageState.Handlers.isCheckoutEnabled();";
            }
        }

        protected override string WaitForCallbackStringId
        {
            get
            {
                return "PageStateWaitScreenSettingHomePage";
            }
        }
    }
}
