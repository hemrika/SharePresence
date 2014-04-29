using System;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using Hemrika.SharePresence.WebSite.SiteMap;
using Microsoft.SharePoint;
using Hemrika.SharePresence.Common.UI;
using Hemrika.SharePresence.WebSite.Modules.SemanticModule;
using System.Web;
using Hemrika.SharePresence.Common.WebSiteController;
using System.Text.RegularExpressions;
using System.Collections.Generic;
using System.Text;
using Microsoft.SharePoint.Administration;
using Microsoft.SharePoint.Utilities;

namespace Hemrika.SharePresence.WebSite.ControlTemplates
{
    public partial class SemanticSettings : UserControl
    {
        string _List = string.Empty;
        string _ID = string.Empty;
        Guid _Guid = Guid.Empty;
        string _url = string.Empty;
        SPList _list = null;
        int _seconds = 0;

        protected override void OnInit(EventArgs e)
        {
            base.OnInit(e);
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            lbl_url.InnerText = "Semantic Url : " + SPContext.Current.Site.Url + "/";

            if (!String.IsNullOrEmpty(Request.QueryString["List"]))
            {
                _List = Request.QueryString["List"];
            }

            if (!String.IsNullOrEmpty(Request.QueryString["ID"]))
            {
                _ID = Request.QueryString["ID"];
            }

            if (!String.IsNullOrEmpty(_List) && !String.IsNullOrEmpty(_ID))
            {
                _Guid = new Guid(_List);
                _list = SPContext.Current.Web.Lists[_Guid];

                if (_list != null)
                {
                    _url = _list.GetItemById(int.Parse(_ID)).File.ServerRelativeUrl;
                }
         
            }

            gridSemantic.RowDataBound += new GridViewRowEventHandler(gridSemantic_RowDataBound);

            lbl_error.Visible = false;
            pnl_wait.Attributes["style"] = "display:none;";

            if (!IsPostBack)
            {

                SemanticModule module = new SemanticModule();


                Uri baseUri = new Uri(SPContext.Current.Web.Url, UriKind.RelativeOrAbsolute);
                Uri url = new Uri(baseUri, _url); ////WebSiteControllerModule.GetFullUrl(application.Context);

                try
                {
                    bool isControlled = false;
                    SPSite site = new SPSite(url.OriginalString);
                    CheckUrlOnZones(site, url, out url, out isControlled, module.RuleType);

                    //if (WebSiteControllerConfig.IsPageControlled(url, module.RuleType))
                    if (isControlled)
                    {
                        List<SemanticUrl> entries = new List<SemanticUrl>();

                        //SPSite site = new SPSite(url.OriginalString);

                        System.Collections.Generic.List<WebSiteControllerRule> Allrules = WebSiteControllerConfig.GetRulesForSiteCollection(new Uri(site.Url), module.RuleType);
                        List<WebSiteControllerRule> rules = new List<WebSiteControllerRule>();

                        foreach (WebSiteControllerRule arule in Allrules)
                        {
                            if (arule.RuleType == module.RuleType && arule.Properties.ContainsKey("OriginalUrl"))
                            {
                                string original = arule.Properties["OriginalUrl"].ToString().ToLower();
                                string _lurl = _url.ToString().ToLower();
                                if ((original == _lurl))// || (original.EndsWith(_lurl)))
                                {
                                    string org = arule.Url;
                                    if (org.EndsWith("/"))
                                    {
                                        org = org.TrimEnd(new char[1] { '/' });
                                    }
                                    //if ((org != SPContext.Current.Site.Url) && (org != SPContext.Current.Web.Url))
                                    //{
                                        SemanticUrl sem = new SemanticUrl();
                                        sem.OriginalUrl = arule.Properties["OriginalUrl"].ToString().ToLower();
                                        sem.Semantic = arule.Url;
                                        sem.Id = arule.Id;
                                        sem.Disabled = arule.IsDisabled;
                                        entries.Add(sem);
                                    //}
                                }
                            }
                        }

                        if (entries.Count > 0)
                        {
                            entries.Sort(SemanticUrl.UrlComparison);
                            gridSemantic.DataSource = entries;
                            gridSemantic.DataBind();
                            //Response.Write(rules.Count.ToString());
                        }
                    }
                }
                catch (Exception ex)
                {
                    ex.ToString();
                }
            }
            else
            {
                pnl_wait.Attributes["style"] = "";
            }
        }

        private void CheckUrlOnZones(SPSite site, Uri curl, out Uri url, out bool isControlled, string RuleType)
        {
            Uri zoneUri = curl;

            isControlled = WebSiteControllerConfig.IsPageControlled(site.WebApplication, zoneUri, RuleType);
            UriBuilder builder = new UriBuilder(curl);

            if (!isControlled)
            {
                try
                {
                    zoneUri = site.WebApplication.GetResponseUri(SPUrlZone.Default);
                    builder.Host = zoneUri.Host;
                    builder.Port = zoneUri.Port;
                    isControlled = WebSiteControllerConfig.IsPageControlled(site.WebApplication, builder.Uri, RuleType);
                    zoneUri = builder.Uri;
                }
                catch (Exception ex)
                {
                    ex.ToString();
                }
            }

            if (!isControlled)
            {
                try
                {
                    zoneUri = site.WebApplication.GetResponseUri(SPUrlZone.Internet);
                    builder.Host = zoneUri.Host;
                    builder.Port = zoneUri.Port;
                    isControlled = WebSiteControllerConfig.IsPageControlled(site.WebApplication, builder.Uri, RuleType);
                    zoneUri = builder.Uri;
                }
                catch (Exception ex)
                {
                    ex.ToString();
                }

            }

            if (!isControlled)
            {
                try
                {
                    zoneUri = site.WebApplication.GetResponseUri(SPUrlZone.Extranet);
                    builder.Host = zoneUri.Host;
                    builder.Port = zoneUri.Port;
                    isControlled = WebSiteControllerConfig.IsPageControlled(site.WebApplication, builder.Uri, RuleType);
                    zoneUri = builder.Uri;
                }
                catch (Exception ex)
                {
                    ex.ToString();
                }

            }

            if (!isControlled)
            {
                try
                {
                    zoneUri = site.WebApplication.GetResponseUri(SPUrlZone.Intranet);
                    builder.Host = zoneUri.Host;
                    builder.Port = zoneUri.Port;
                    isControlled = WebSiteControllerConfig.IsPageControlled(site.WebApplication, builder.Uri, RuleType);
                    zoneUri = builder.Uri;
                }
                catch (Exception ex)
                {
                    ex.ToString();
                }

            }

            if (!isControlled)
            {
                try
                {
                    zoneUri = site.WebApplication.GetResponseUri(SPUrlZone.Custom);
                    builder.Host = zoneUri.Host;
                    builder.Port = zoneUri.Port;
                    isControlled = WebSiteControllerConfig.IsPageControlled(site.WebApplication, builder.Uri, RuleType);
                    zoneUri = builder.Uri;
                }
                catch (Exception ex)
                {
                    ex.ToString();
                }
            }

            if (!isControlled)
            {
                Uri altZone = null;
                foreach (SPAlternateUrl altUrl in site.WebApplication.AlternateUrls)
                {

                    if (altUrl.UrlZone == site.Zone)
                    {
                        altZone = altUrl.Uri;
                        break;
                    }
                }

                if (altZone != null)
                {
                    builder.Host = zoneUri.Host;
                    builder.Port = zoneUri.Port;
                    isControlled = WebSiteControllerConfig.IsPageControlled(site.WebApplication, builder.Uri, RuleType);
                    zoneUri = builder.Uri;
                }
            }
            url = zoneUri;
        }

        void gridSemantic_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            // Dynamically hide the ID field so it's in the client data but not displayed
            e.Row.Cells[0].Visible = false;
            e.Row.Cells[2].Visible = false;
            e.Row.Cells[3].Visible = false;

            // Dynamically add confirmation to delete button
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                Button btn = (Button)e.Row.FindControl("btnDelete");
                if (btn != null)
                {
                    btn.Attributes.Add("onClick", "return confirm('Are you sure you want to delete this entry?');");
                }
            }
        }

        /// <summary>
        /// Binds to selected row and sends the id cell (0) to
        /// Utils.DeleteEntry for deletion based in the entryID
        /// </summary>
        protected void btnDelete_Click(object sender, EventArgs e)
        {
            pnl_wait.Attributes["style"] = "";
            Button btn = (Button)sender;
            GridViewRow grdRow = (GridViewRow)btn.Parent.Parent;

            Guid id = new Guid(grdRow.Cells[0].Text);

            SPSite site = SPContext.Current.Site;

            SPSecurity.RunWithElevatedPrivileges(() =>
            {

                try
                {
                    Guid siteId = site.ID;
                    Guid webId = SPContext.Current.Web.ID;
                    int itemId = SPContext.Current.ItemId;

                    WebSiteControllerRule rule = WebSiteControllerConfig.GetRule(site.WebApplication, id);

                    using (SPSite wsite = new SPSite(siteId))
                    {
                        site.AddWorkItem(
                            Guid.NewGuid(),
                            DateTime.Now.ToUniversalTime(),
                            WebSiteControllerRuleWorkItem.WorkItemTypeId,
                            webId,
                            siteId,
                            itemId,
                            true,
                            rule.Id,
                            Guid.Empty,
                            site.SystemAccount.ID,
                            null,
                            string.Empty,
                            Guid.Empty
                            );
                    }

                    /*
                    rule.Delete();
                    rule.Unprovision();
                    rule.Uncache();
                    */
                }
                catch (Exception ex)
                {
                    SPDiagnosticsService.Local.WriteTrace(0, new SPDiagnosticsCategory(ex.Source, TraceSeverity.High, EventSeverity.Error), TraceSeverity.High, ex.Message, ex.Data);
                    //ex.ToString();
                }
            });

            Response.Redirect(Request.RawUrl, true);
        }

        protected void btn_Save_Click(object sender, EventArgs e)
        {
            pnl_wait.Attributes["style"] = "";
            var eventArgsJavaScript = String.Format("{{Message:'{0}',controlIDs:window.frameElement.dialogArgs}}", "The Properties have been updated.");

            ((EnhancedLayoutsPage)Page).EndOperation(ModalDialogResult.OK, eventArgsJavaScript);
            Context.Response.Flush();
            Context.Response.End();
        }

        protected void GridView1_RowEditing(object sender, GridViewEditEventArgs e)
        {
        }

        protected void GridView1_RowDeleting(object sender, GridViewDeleteEventArgs e)
        {
        }

        protected void GridView1_RowUpdated(object sender, GridViewUpdatedEventArgs e)
        {
        }

        protected void btn_Add_Click(object sender, EventArgs e)
        {
            pnl_wait.Attributes["style"] = "";
            SemanticModule mod = new SemanticModule();

            Uri baseUri = new Uri(SPContext.Current.Site.Url, UriKind.RelativeOrAbsolute);
            Uri url = new Uri(baseUri, tbx_url.Text); ////WebSiteControllerModule.GetFullUrl(application.Context);
            
            if (!WebSiteControllerConfig.HasRule(SPContext.Current.Site.WebApplication, url, mod.RuleType))
            {
                CreateWorkItem(tbx_url.Text);
                System.Threading.Thread.Sleep((_seconds * 1000));
                SPUtility.Redirect(Request.RawUrl, (SPRedirectFlags.DoNotEncodeUrl | SPRedirectFlags.Trusted), HttpContext.Current);
            }
            else 
            {
                WebSiteControllerRule rule = WebSiteControllerConfig.GetRule(SPContext.Current.Site.WebApplication, url, mod.RuleType);
                lbl_error.Visible = true;

                if (rule != null && rule.Properties.ContainsKey("OriginalUrl"))
                {
                    lbl_error.InnerHtml = "This url is alreay in use by <a target=\"_blank\" href=\"" + rule.Properties["OriginalUrl"].ToString()+"\" >"+rule.Properties["OriginalUrl"].ToString()+"</a>";
                }
            }
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
            builder.Append(url + ";");
            builder.Append(disabled.ToString() + ";");
            builder.Append(appliesToSSL.ToString() + ";");
            builder.Append(sequence.ToString() + ";");
            builder.Append(principalType.ToString() + ";");
            builder.Append(pricipal + ";");
            builder.Append("#");

            builder.Append(String.Format("{0}:{1};", "OriginalUrl", _url));

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
                    SPDiagnosticsService.Local.WriteTrace(0, new SPDiagnosticsCategory(ex.Source, TraceSeverity.High, EventSeverity.Error), TraceSeverity.High, ex.Message, ex.Data);
                    //ex.ToString();
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

                SPJobDefinitionCollection jobs = SPContext.Current.Site.WebApplication.JobDefinitions;

                foreach (SPJobDefinition job in jobs)
                {
                    if (job.Name == WebSiteControllerRuleWorkItem.WorkItemJobDisplayName)
                    {
                        try
                        {
                            DateTime next = job.Schedule.NextOccurrence(job.LastRunTime);
                            _seconds = next.Second;
                            break;
                        }
                        catch (Exception ex)
                        {
                            SPDiagnosticsService.Local.WriteTrace(0, new SPDiagnosticsCategory(ex.Source, TraceSeverity.High, EventSeverity.Error), TraceSeverity.High, ex.Message, ex.Data);
                            //ex.ToString();
                        }
                    }
                }

            });
        }

        protected void btn_Cancel_Click(object sender, EventArgs e)
        {
            pnl_wait.Attributes["style"] = "";
            tbx_url.Text = string.Empty;
            lbl_error.Visible = false;
            SPUtility.Redirect(Request.RawUrl, (SPRedirectFlags.DoNotEncodeUrl | SPRedirectFlags.Trusted), HttpContext.Current);
        }
    }
}
