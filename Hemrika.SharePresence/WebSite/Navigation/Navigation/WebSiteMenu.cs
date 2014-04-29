// -----------------------------------------------------------------------
// <copyright file="Menu.cs" company="">
// TODO: Update copyright text.
// </copyright>
// -----------------------------------------------------------------------

namespace Hemrika.SharePresence.WebSite.Navigation
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Web.UI.WebControls;
    using System.ComponentModel;
    using System.Web.UI;
    using Hemrika.SharePresence.Common.TemplateEngine;
    using Microsoft.SharePoint.Utilities;
    using Microsoft.SharePoint;
    using Hemrika.SharePresence.Common.WebSiteController;
    using Microsoft.SharePoint.Administration;
    using Hemrika.SharePresence.WebSite.Modules.SemanticModule;
    using Microsoft.SharePoint.WebControls;
    using System.Web;
    using System.Web.UI.HtmlControls;

    /// <summary>
    /// TODO: Update summary.
    /// </summary>
    [ToolboxData("<{0}:WebSiteMenu runat=server></{0}:WebSiteMenu>")]
    public class WebSiteMenu : HierarchicalDataBoundControl
    {
        public string ControlType { get; set; }
        public new string ControlStyle { get; set; }
        private string NodeXmlPath { get; set; }
        private string NodeSelector { get; set; }
        private bool IncludeContext { get; set; }
        private bool IncludeHidden { get; set; }
        private string IncludeNodes { get; set; }
        private string ExcludeNodes { get; set; }
        private string NodeManipulator { get; set; }

        [DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
        [PersistenceMode(PersistenceMode.InnerProperty)]
        public List<ClientOption> ClientOptions { get; set; }

        [DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
        [PersistenceMode(PersistenceMode.InnerProperty)]
        public List<TemplateArgument> TemplateArguments { get; set; }

        private string idPrefix = "nvgtn";
        private MenuBase menu;
        private WebSiteProvider provider;
        private WebSiteDataSource dataSource;

        public WebSiteMenu()
            : base()
        {
        }

        public override string ID
        {
            get
            {
                return base.ID;
            }
            set
            {
                base.ID = value;
            }
        }

        /// <summary>
        /// Gets the ClientID of this control. 
        /// </summary>
        public override string ClientID
        {
            [Microsoft.SharePoint.Security.SharePointPermission(System.Security.Permissions.SecurityAction.Demand, ObjectModel = true)]
            get
            {
                if (this.idPrefix == null)
                {
                    this.idPrefix = SPUtility.GetNewIdPrefix(this.Context);
                }
                string unique = this.idPrefix + "_" + SPContext.Current.Site.ID.ToString();
                return SPUtility.GetShortId(unique, this);
                //return SPUtility.GetShortId(this.idPrefix, this);
            }
        }
            
        protected override void OnPreRender(EventArgs e)
        {

            try
            {
                base.OnPreRender(e);

                dataSource = this.GetDataSource() as WebSiteDataSource;
                provider = (dataSource != null) ? dataSource.Provider : null;

                if (provider == null)
                {
                    return;
                }

                menu = MenuBase.Instantiate(ClientID,ControlType,ControlStyle);

                menu.ApplySettings(
                    new Settings
                    {
                        ControlType = ControlType,
                        ControlStyle = ControlStyle,
                        NodeXmlPath = NodeXmlPath,
                        NodeSelector = NodeSelector,
                        IncludeContext = IncludeContext,
                        IncludeHidden = IncludeHidden,
                        IncludeNodes = IncludeNodes,
                        ExcludeNodes = ExcludeNodes,
                        NodeManipulator = NodeManipulator,
                        ClientOptions = ClientOptions,
                        TemplateArguments = TemplateArguments
                    });

                Uri url = WebSiteControllerModule.GetFullUrl(Context);
                SPSite site = new SPSite(url.OriginalString);
                bool isControlled = false;
                CheckUrlOnZones(site, url, out url, out isControlled);
                List<WebSiteNode> nodes = new List<WebSiteNode>();

                if (isControlled)
                {
                    WebSiteNode cnode = provider.FindWebSiteNode(url.ToString());
                    if(cnode != null)
                    {
                        nodes.Add(cnode);
                    }
                }

                if (nodes.Count == 0)
                {
                    List<SemanticUrl> entries = CheckForManagedUrl(site, url);
                    foreach (SemanticUrl surl in entries)
                    {
                        WebSiteNode cnode = provider.FindWebSiteNode(surl.Semantic);
                        if (cnode != null)
                        {
                            nodes.Add(cnode);
                        }
                        
                    }
                    //urlnode = provider.FindWebSiteNode(Context.Request.Url.ToString());
                }

                WebSiteNode root = (WebSiteNode)provider.RootNode.Clone();

                if (provider.CurrentNode != null)
                {
                    root.FindById(provider.CurrentNode.Id).Selected = false;
                }
                else
                {
                    RemoveSelectedNodes(ref root);
                }
                
                if (nodes.Count > 0)// != null)
                {
                    foreach (WebSiteNode node in nodes)
                    {
                        root.FindById(node.Id).Selected = true;   
                    }

                    /*
                    if (nodes.Count == 1)
                    {
                        root.CurrentNode = nodes[0];
                    }
                    */
                }
                else
                {

                    root.Selected = true;
                }

                //provider.ResetProvider();
                menu.RootNode = root;// provider.RootNode;
                menu.PreRender(this.Page);
            }
            catch (Exception exc)
            {
                exc.ToString();
            }
        }

        private void RemoveSelectedNodes(ref WebSiteNode root)
        {
            List<WebSiteNode> selected = root.Children.Where(n => n.Selected == true).ToList<WebSiteNode>();
            foreach (WebSiteNode item in selected)
            {
                root.FindById(item.Id).Selected = false;
                /*
                if (item.HasChildren())
                {
                    RemoveSelectedNodes(ref item);
                }
                */
            }
        }

        private List<SemanticUrl> CheckForManagedUrl(SPSite site, Uri url)
        {
            List<SemanticUrl> entries = new List<SemanticUrl>();

            System.Collections.Generic.List<WebSiteControllerRule> Allrules = WebSiteControllerConfig.GetRulesForSiteCollection(new Uri(site.Url), typeof(SemanticModule).FullName);
            List<WebSiteControllerRule> rules = new List<WebSiteControllerRule>();

            foreach (WebSiteControllerRule arule in Allrules)
            {
                if (arule.RuleType == typeof(SemanticModule).FullName && arule.Properties.ContainsKey("OriginalUrl"))
                {
                    string original = arule.Properties["OriginalUrl"].ToString().ToLower();
                    string _lurl = url.AbsolutePath.ToString().ToLower();
                    if ((original == _lurl) || (original.EndsWith(_lurl)))
                    {
                        string org = arule.Url;
                        if (org.EndsWith("/"))
                        {
                            org = org.TrimEnd(new char[1] { '/' });
                        }
                        if ((org != SPContext.Current.Site.Url) && (org != SPContext.Current.Web.Url))
                        {
                            SemanticUrl sem = new SemanticUrl();
                            sem.OriginalUrl = arule.Properties["OriginalUrl"].ToString().ToLower();
                            sem.Semantic = arule.Url;
                            sem.Id = arule.Id;
                            sem.Disabled = arule.IsDisabled;
                            entries.Add(sem);
                        }
                    }
                }
            }
            entries.Sort(SemanticUrl.UrlComparison);
            return entries;
        }

        protected override void Render(HtmlTextWriter writer)
        {
            try
            {
                //this.RenderBeginTag(writer);
                //this.RenderContents(writer);
                

                //base.Render(writer);
                if (menu != null)
                {
                    menu.Render(writer);
                }
                else
                {
                    this.RenderBeginTag(writer);
                    this.RenderContents(writer);
                    this.RenderEndTag(writer);
                }
                /*
                if (HttpContext.Current.User.Identity.IsAuthenticated)
                {
                    System.Web.UI.HtmlControls.HtmlAnchor link = new HtmlAnchor();
                    //HtmlLink link = new HtmlLink();
                    link.Title = "Edit";
                    link.HRef = "javascript:(function () { var options = { url:'/Sitemap/AllItems.aspx?IsDlg=1', title: 'Modify Navigation', allowMaximize: true, showClose: true }; SP.UI.ModalDialog.showModalDialog(options); }) ();";
                    //link.HRef = "/Sitemap";
                    //link.InnerText = "Edit";
                    System.Web.UI.HtmlControls.HtmlImage image = new HtmlImage();
                    image.Alt = "Edit";
                    image.Src = "/_layouts/images/settingsIcon.png"; 
                    link.Controls.Add(image);
                    link.RenderControl(writer);
                }
                */
                //this.RenderEndTag(writer);
            }
            catch (Exception exc)
            {
                exc.ToString();
            }
        }

        private void CheckUrlOnZones(SPSite site, Uri curl, out Uri url, out bool isControlled)
        {
            Uri zoneUri = curl;

            isControlled = WebSiteControllerConfig.IsPageControlled(site.WebApplication, zoneUri, typeof(SemanticModule).FullName);
            UriBuilder builder = new UriBuilder(curl);

            if (!isControlled)
            {
                try
                {
                    zoneUri = site.WebApplication.GetResponseUri(SPUrlZone.Default);
                    builder.Host = zoneUri.Host;
                    builder.Port = zoneUri.Port;
                    isControlled = WebSiteControllerConfig.IsPageControlled(site.WebApplication, builder.Uri, typeof(SemanticModule).FullName);
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
                    isControlled = WebSiteControllerConfig.IsPageControlled(site.WebApplication, builder.Uri, typeof(SemanticModule).FullName);
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
                    isControlled = WebSiteControllerConfig.IsPageControlled(site.WebApplication, builder.Uri, typeof(SemanticModule).FullName);
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
                    isControlled = WebSiteControllerConfig.IsPageControlled(site.WebApplication, builder.Uri, typeof(SemanticModule).FullName);
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
                    isControlled = WebSiteControllerConfig.IsPageControlled(site.WebApplication, builder.Uri, typeof(SemanticModule).FullName);
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
                    isControlled = WebSiteControllerConfig.IsPageControlled(site.WebApplication, builder.Uri, typeof(SemanticModule).FullName);
                    zoneUri = builder.Uri;
                }
            }
            url = zoneUri;
        }

    }
}