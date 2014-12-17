// <copyright file="ContentRibbon.cs" company="Hemrika">
// Copyright Hemrika. All rights reserved.
// </copyright>
// <author>MARKETING\Administrator</author>
// <date>2012-03-22 15:41:28Z</date>
namespace Hemrika.SharePresence.WebSite
{
    using System;
    using System.Security.Permissions;
    using System.Text;
    using System.Web.UI.WebControls;
    using Microsoft.SharePoint;
    using Microsoft.SharePoint.Security;
    using Hemrika.SharePresence.Common.UI;
    using Hemrika.SharePresence.Common.Ribbon.Definitions;
    using Hemrika.SharePresence.Common.Ribbon.Libraries;
    using Hemrika.SharePresence.Common.Ribbon.Definitions.Controls;
    using Microsoft.SharePoint.WebPartPages;
    using System.Collections.Generic;
    using System.Xml;
    using System.IO;
    using System.Linq;
    using Microsoft.SharePoint.Utilities;
    using Hemrika.SharePresence.Common.Ribbon;
    using Hemrika.SharePresence.WebSite.PageParts;
    using Microsoft.SharePoint.WebControls;
    using System.Web.UI;
    using System.Web.UI.WebControls.WebParts;
    using System.Web;
using Hemrika.SharePresence.WebSite.ContentTypes;
using Hemrika.SharePresence.WebSite.Layout;
using Hemrika.SharePresence.Common;
using Hemrika.SharePresence.Common.ServiceLocation;
    using Hemrika.SharePresence.WebSite.Fields;
    using Hemrika.SharePresence.WebSite.FieldTypes;
    using Hemrika.SharePresence.WebSite.Page;
    using System.Web.UI.HtmlControls;

    /// <summary>
    /// TODO: Add comment for ContentRibbon
    /// </summary> 
    [AspNetHostingPermission(SecurityAction.LinkDemand, Level = AspNetHostingPermissionLevel.Minimal), SharePointPermission(SecurityAction.LinkDemand, ObjectModel = true)]
    [AspNetHostingPermission(SecurityAction.InheritanceDemand, Level = AspNetHostingPermissionLevel.Minimal)]
    [SharePointPermission(SecurityAction.InheritanceDemand, ObjectModel = true)]
    public class DesignRibbon : SPRibbonCommandHandler, IPostBackEventHandler//, ICallbackEventHandler
    {
        private IServiceLocator serviceLocator;
        private IPageLayoutRepository servicePageLayouts;
        string url;
        public DesignRibbon(SPPageStateControl psc)
            : base(psc)
        {
            serviceLocator = SharePointServiceLocator.GetCurrent();
            servicePageLayouts = serviceLocator.GetInstance<IPageLayoutRepository>();
        }

        private List<String> categories;
        private ICollection<GroupDefinition> groups;

        protected override void OnLoad(EventArgs e)
        {
            base.OnLoad(e);

            if (SPContext.Current != null)
            {
                GetSources();
            }

            var tabDefinition = GetTabDefinition();
            try
            {
                if (SPRibbon.GetCurrent(this.Page) == null)
                    return;
                if (tabDefinition != null && !this.DesignMode)
                    RibbonController.Current.AddRibbonTabToPage(tabDefinition, this.Page, false);
            }
            catch (Exception ex)
            {
                ex.ToString();
            }
        }

        private ICollection<GroupDefinition> Groups
        {
            get
            {
                if (this.groups == null)
                {
                    this.groups = new List<GroupDefinition>();
                }
                return this.groups;

            }
            set
            {
                this.groups = value;
            }
        }

        private ICollection<String> Categories
        {
            get
            {
                if (this.categories == null)
                {
                    this.categories = new List<String>();
                }
                return this.categories;
            }
        }

        private SPRibbon Ribbon
        {
            get
            {
                if (this.Page != null)
                {
                    return SPRibbon.GetCurrent(this.Page);
                }
                return null;
            }
        }


        private WebPartAdder WebPartAdder
        {
            get
            {
                if (this.Page != null)
                {
                    return (this.Page.Items[typeof(WebPartAdder)] as WebPartAdder);
                }
                return null;
            }
        }

        private SPWebPartManager WebPartManager
        {
            get
            {
                if (this.Page != null)
                {
                    return (SPWebPartManager)System.Web.UI.WebControls.WebParts.WebPartManager.GetCurrentWebPartManager(this.Page);
                }
                return null;
            }
        }

        private void GetSources()
        {
        }

        //public override TabDefinition GetTabDefinition()
        public TabDefinition GetTabDefinition()
        {
            var groups = new List<GroupDefinition>();

            var listsettings = new List<ControlDefinition>();

            var listbutton = new ButtonDefinition()
            {
                Id = "PageLayoutList",
                Title = "PageLayouts",
                Image = ImageLibrary.GetStandardImage(13, 1),
                CommandJavaScript = "ShowPagelayoutList();",
                TemplateAlias = "c1"
            };

            listsettings.Add(listbutton);

            var settings = new GroupDefinition()
            {
                Id = "PageLayouts",
                Title = "Pagelayouts for this site",
                Sequence = "10",
                Template = GroupTemplateLibrary.SimpleTemplate,
                Controls = listsettings.ToArray()
            };

            groups.Add(settings);

            //int index = 0;
            //int sequence = 30;

            SPContentTypeId contentTypeId = (SPContentTypeId)SPContext.Current.ListItem[SPBuiltInFieldId.ContentTypeId];
            string contentType = SPContext.Current.ListItem[SPBuiltInFieldId.ContentType].ToString();
            //string id = SPContext.Current.ListItem[SPBuiltInFieldId.ContentTypeId].ToString();
            //SPContentTypeId contentTypeId = new SPContentTypeId(id);

            //foreach (string category in Categories)
            //{
            List<PageLayout> layouts = servicePageLayouts.GetPageLayouts().Where(n => n.ContenTypeId == contentTypeId).Select(n => n).ToList();
            layouts.AddRange(servicePageLayouts.GetPageLayouts().Where(n => n.ContenTypeId == contentTypeId.Parent).Select(n => n).ToList());
            layouts.AddRange(servicePageLayouts.GetPageLayouts().Where(n => n.ContenTypeId.Parent == contentTypeId).Select(n => n).ToList());

            if (layouts.Count > 0)
            {
                var controls = new List<ControlDefinition>();

                foreach (PageLayout item in layouts)
                {
                    
                    ImageDefinition image = new ImageDefinition();
                    image.Url16 = item.Icon;
                    image.Url32 = item.Icon;

                    ToolTipDefinition tooltip = new ToolTipDefinition();
                    tooltip.Description = item.Description.ToString();
                    tooltip.Title = item.Title;
                    tooltip.Image = item.Preview;
                    tooltip.Class = "tooltip_preview";

                    url = base.parentStateControl.ContextUri;


                    if (IsEditMode(this.Page))
                    {
                        url = WebPageStateControl.AddQueryStringParameter(WebPageStateControl.AddQueryStringParameter(url, "ControlMode", "Edit"), "DisplayMode", "Design");
                    }

                    url = WebPageStateControl.AddQueryStringParameter(url, "PageLayout", DateTime.Now.Ticks.ToString());

                    var button = new ButtonDefinition()
                    {
                        Id = item.Title.Replace(" ", String.Empty).Trim(),
                        Title = item.Title,
                        ToolTip = tooltip,
                        Image = image,
                        CommandEnableJavaScript = "document.forms[MSOWebPartPageFormName].MSOLayout_InDesignMode.value == 0",
                        CommandJavaScript = String.Format("ChangePageLayout('{0}','{1}','{2}')", this.ClientID, url, item.UniqueId),
                        TemplateAlias = "c1"
                    };

                    controls.Add(button);
                }

                var group = new GroupDefinition()
                {
                    Id = "PageLayouts",
                    Title = "Pagelayouts for ContentType " + contentType,
                    Sequence = "10",
                    Template = GroupTemplateLibrary.SimpleTemplate,
                    Controls = controls.ToArray()
                };

                groups.Add(group);

                //sequence += 10;
                //index += 1;
            }
            //}
            if (groups.Count > 0)
            {
                return new TabDefinition()
                {
                    Id = "Hemrika.SharePresence.Design",
                    Title = "Design",
                    Sequence = "120",
                    GroupTemplates = GroupTemplateLibrary.AllTemplates,
                    Groups = groups.ToArray()
                };
            }
            else
            {
                return null;
            }
        }

        [SharePointPermission(SecurityAction.Demand, ObjectModel = true)]
        public void RaisePostBackEvent(string eventArgument)
        {
            SPContext.Current.Web.AllowUnsafeUpdates = true;
            Guid uniqueId = Guid.Empty;

            try
            {
                string[] eventArg = eventArgument.Split(new string[1] { "#;" }, StringSplitOptions.None);

                SPWeb rootWeb = SPContext.Current.Site.RootWeb;
                SPListItem item = SPContext.Current.ListItem;

                if (!string.IsNullOrEmpty(eventArg[1]))
                {
                    PageLayout layout = servicePageLayouts.GetPageLayout(new Guid(eventArg[1]));

                    if (item != null && layout != null)
                    {
                        SPFile currentFile = item.File;
                        bool checkout = false;
                        if (currentFile != null && currentFile.Exists)
                        {
                            if (currentFile.CheckOutType == SPFile.SPCheckOutType.None && currentFile.RequiresCheckout)
                            {
                                currentFile.CheckOut();
                                checkout = true;
                            }
                        }

                        SPUser suser = null;
                        SPListItemVersion spversion = null;

                        SPListItemVersionCollection versions = item.Versions;
                        if (versions.Count > 0)
                        {
                            foreach (SPListItemVersion version in versions)
                            {
                                if(version.IsCurrentVersion)
                                {
                                    suser = version.CreatedBy.User;
                                    spversion = version;
                                    item = version.ListItem;
                                    break;
                                }
                            }
                        }
                        /*
                        else
                        {
                            listItem = file.ListItemAllFields;
                        }
                        */

                        using (SPSite site = new SPSite(SPContext.Current.Web.Url, suser.UserToken))
                        {
                            SPWeb spweb = site.OpenWeb();
                            SPFile spfile = spweb.GetFile(currentFile.UniqueId);
                            SPListItem spitem = spfile.Item.Versions.GetVersionFromLabel(spversion.VersionLabel).ListItem;


                            PublishingPageDesignFieldValue value = spitem[BuildFieldId.PublishingPageDesign] as PublishingPageDesignFieldValue;
                            value.Id = layout.UniqueId;
                            value.Title = layout.Title;
                            value.Url = layout.Url;

                            spitem[BuildFieldId.PublishingPageDesign] = value;

                            try
                            {
                                spitem.Fields[BuildFieldId.PublishingPageDesign].Update(true);
                            }
                            catch (Exception)
                            {
                            }

                            try
                            {

                                spitem.Update();
                            }
                            catch (Exception)
                            {
                            }

                            try
                            {
                                spitem.SystemUpdate(false);

                            }
                            catch (Exception)
                            {
                            }

                            try
                            {
                                spfile.Update();
                            }
                            catch (Exception)
                            {
                            }
                        }

                        uniqueId = layout.UniqueId;

                        //currentFile.Update();

                        if (checkout)
                        {
                            currentFile.CheckIn("Layout Changed", SPCheckinType.OverwriteCheckIn);

                        }
                    }
                }
            }
            catch (Exception ex)
            {
                ex.ToString();
                //result = ex;
            }
            SPContext.Current.Web.AllowUnsafeUpdates = false;

            url = base.parentStateControl.ContextUri;


            if (IsEditMode(this.Page))
            {
                url = WebPageStateControl.AddQueryStringParameter(WebPageStateControl.AddQueryStringParameter(url, "ControlMode", "Edit"), "DisplayMode", "Design");
            }

            url = WebPageStateControl.AddQueryStringParameter(url, "PageLayout", DateTime.Now.Ticks.ToString());

            base.parentStateControl.EnsureItemSavedIfEditMode(true);
            this.RefreshPageState();
            this.OnPageStateChanged();

            //base.ClearChildControlState();


            HtmlMeta refresh = new HtmlMeta();
            refresh.HttpEquiv = "refresh";
            refresh.Content = "0;URL=" + url + "";
            this.Page.Header.Controls.Add(refresh);
            this.Page.Header.Controls.Add(new LiteralControl(Environment.NewLine));

            /*
            if (IsEditMode(this.Page))
            {
                SPUtility.Redirect("~/" + url, SPRedirectFlags.Trusted,HttpContext.Current);
            }
            else
            {
                SPUtility.Redirect("~/" + base.parentStateControl.ContextUri, SPRedirectFlags.Trusted, HttpContext.Current);
            }
            */
        }

        private static bool IsEditMode(System.Web.UI.Page page)
        {
            if (page == null)
            {
                return false;
            }

            SPWebPartManager manager = System.Web.UI.WebControls.WebParts.WebPartManager.GetCurrentWebPartManager(page) as SPWebPartManager;
            return (manager != null && manager.GetDisplayMode().AllowPageDesign);
        }

        public void OnPageStateChanged()
        {
            HttpContext.Current.Items["currentAuthoringStates"] = null;

            HttpContext current = HttpContext.Current;

            if (current != null)
            {
                SPContext.Current.ResetItem();

                if (current.Items.Contains("CachedObjectWrapper_checkedOutVersion"))
                {
                    current.Items.Remove("CachedObjectWrapper_checkedOutVersion");
                }
                if (current.Items.Contains("currentVisibilityStates"))
                {
                    current.Items.Remove("currentVisibilityStates");
                }
            }
        }

        // Properties
        public override string ClientSideCommandId
        {
            get
            {
                return "ChangePageLayout";
            }
        }

        public override string HandlerCommand
        {
            get
            {
                
                StringBuilder builder = new StringBuilder();
                /*
                builder.Append("SP.Ribbon.PageState.PageStateHandler.ignoreNextUnload = false;");
                builder.Append("SP.Ribbon.PageState.PageStateHandler.EnableSaveBeforeNavigate(false);");
                builder.Append("SP.Utilities.HttpUtility.navigateTo('");
                url = base.parentStateControl.ContextUri;

                if (IsEditMode(this.Page))
                {
                    url = WebPageStateControl.AddQueryStringParameter(WebPageStateControl.AddQueryStringParameter(url, "ControlMode", "Edit"), "DisplayMode", "Design");
                }

                url = WebPageStateControl.AddQueryStringParameter(url, "PageLayout", DateTime.Now.Ticks.ToString());

                builder.Append(this.url);
                builder.Append("');");
                builder.Append(this.Page.ClientScript.GetCallbackEventReference(this, "'" + this.ClientSideCommandId + "'", "SP.Ribbon.PageState.PageStateHandler.PageStateGroupDontSaveAndStop", "SP.Ribbon.PageState.PageStateGroupDontSaveAndStop", true));
                */
                builder.Append(base.DefaultPostbackHandlerCommand);
                return builder.ToString();
            }
        }

        public override string IsEnabledHandler
        {
            get
            {
                return "!SP.Ribbon.PageState.Handlers.isCheckinEnabled();";
            }
        }

        /*
        protected override string WaitForCallbackStringId
        {
            get
            {
                return "ChangePageLayoutCallBack";
            }
        }
        */

        /*
        private void BaseRaisePostBackEvent(string eventArgument)
        {
            base.RaisePostBackEvent(eventArgument);
        }
        */
    }
}

