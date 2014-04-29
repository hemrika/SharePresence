using System;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using Microsoft.SharePoint;
using System.Collections;
using Microsoft.SharePoint.Utilities;
using System.IO;
using System.Text;
using Hemrika.SharePresence.Common;
using Hemrika.SharePresence.Common.Configuration;
using Hemrika.SharePresence.WebSite.Layout;
using Hemrika.SharePresence.Common.ServiceLocation;
using System.Collections.Generic;
using Hemrika.SharePresence.WebSite.ContentTypes;
using Hemrika.SharePresence.WebSite.Fields;
using Hemrika.SharePresence.Common.UI;

namespace Hemrika.SharePresence.WebSite.ControlTemplates
{
    public partial class CreateWebPage : UserControl
    {
        private IServiceLocator serviceLocator;
        private IPageLayoutRepository servicePageLayouts;

        public CreateWebPage()
        {
            serviceLocator = SharePointServiceLocator.GetCurrent();
            servicePageLayouts = serviceLocator.GetInstance<IPageLayoutRepository>();
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!base.IsPostBack)
            {
                SPWeb web = SPContext.Current.Web;
                SPWeb rootWeb = SPContext.Current.Site.RootWeb;

                foreach (SPList list in web.Lists)
                {
                    if (list.BaseType == SPBaseType.DocumentLibrary)
                    {
                        SPDocumentLibrary docLib = (SPDocumentLibrary)list;

                        if (docLib.IsCatalog == false && !list.IsApplicationList && !list.IsSiteAssetsLibrary && ContainsWebSitePageTypes(list))
                        {
                            ListItem item = new ListItem(list.Title, list.ID.ToString());

                            if (!this.ddl_library.Items.Contains(item))
                            {
                                this.ddl_library.Items.Add(item);
                            }
                        }
                    }
                }


                //TODO ListId queryparam
                try
                {
                    this.ddl_library.SelectedValue = Request.Params["list"].ToString();
                }
                catch (Exception ex)
                {
                    ex.ToString();
                }

                SPContentTypeCollection availableContentTypes = rootWeb.AvailableContentTypes;


                foreach (SPContentType contenttype in availableContentTypes)
                {
                    if (!this.ddl_contentgroup.Items.Contains(new ListItem(contenttype.Group)))
                    {

                        this.ddl_contentgroup.Items.Add(contenttype.Group);
                    }
                }

                this.ddl_contentgroup.Items.Remove("_Hidden");
                this.ddl_contentgroup.SelectedValue = "Publishing Content Types";

                foreach (SPContentType contenttype in availableContentTypes)
                {
                    if (contenttype.Group == this.ddl_contentgroup.SelectedValue)
                    {
                        List<PageLayout> check = servicePageLayouts.GetPageLayouts(contenttype.Id);
                        if (check.Count > 0)
                        {
                            ListItem item = new ListItem(contenttype.Name, contenttype.Id.ToString());
                            this.ddl_contentname.Items.Add(item);
                        }
                    }
                }

                if (this.ddl_contentname.Items.Count > 0)
                {

                    this.ddl_contentname.SelectedIndex = 0; ;
                    SPContentTypeId contenttypeid = new SPContentTypeId(this.ddl_contentname.SelectedValue);

                    List<PageLayout> layouts = servicePageLayouts.GetPageLayouts(contenttypeid);
                    //layouts.AddRange(servicePageLayouts.GetPageLayouts(contenttypeid.Parent));

                    foreach (PageLayout layout in layouts)
                    {
                        ListItem item = new ListItem(layout.Title, layout.UniqueId.ToString());
                        if (!this.ddl_pagelayout.Items.Contains(item))
                        {
                            this.ddl_pagelayout.Items.Add(item);
                        }
                    }

                    if (this.ddl_pagelayout.Items.Count > 0)
                    {
                        PageLayout layout = servicePageLayouts.GetPageLayout(new Guid(this.ddl_pagelayout.Items[0].Value));
                        this.lbl_description.Text = layout.Description;
                    }
                }
            }
        }

        private bool ContainsWebSitePageTypes(SPList list)
        {
            bool found = false;

            SPContentTypeId pageTemplate = ContentTypeId.PageTemplate;
            
            foreach (SPContentType ct in list.ContentTypes)
            {
                if (ct.Id == pageTemplate || ct.Parent.Id == pageTemplate || ct.Id.IsChildOf(pageTemplate) || ct.Parent.Id.IsChildOf(pageTemplate))
                {
                    found = true;
                    break;
                }
            }
            return found;
        }

        protected void Btn_Save_Click(object sender, EventArgs e)
        {
            SPWeb rootWeb = SPContext.Current.Site.RootWeb;
            SPWeb currentWeb = SPContext.Current.Web;
            SPList currentList = currentWeb.Lists[new Guid(this.ddl_library.SelectedValue)];

            if (currentList != null)
            {
                currentWeb.AllowUnsafeUpdates = true;
                SPFolder rootFolder = currentList.RootFolder;
                MemoryStream stream = new MemoryStream();
                StreamWriter writer = new StreamWriter(stream);
                string template = this.GetTemplate();
                writer.Write(template);
                writer.Flush();

                SPListItem item = null;

                string filename = rootFolder.Url + "/" + this.tbx_pagename.Text + ".aspx";
                SPFile currentFile = currentWeb.GetFile(filename);

                if (currentFile != null && currentFile.Exists)
                {
                    if (currentFile.CheckOutType == SPFile.SPCheckOutType.None && currentFile.RequiresCheckout)
                    {
                        currentFile.CheckOut();
                    }
                }

                try
                {
                    currentFile = rootFolder.Files.Add(this.tbx_pagename.Text + ".aspx", stream, true, "System Creation", false);
                }
                catch (Exception ex)
                {
                    ex.ToString();
                }

                if (currentFile != null && currentFile.Exists)
                {
                    currentFile.Properties["vti_progid"] = "WebSite.WebSitePage";
                    currentFile.Properties["HTML_x0020_File_x0020_Type"] = "WebSite.WebSitePage";
                    currentFile.Update();
                    //item = currentFile.ListItemAllFields;
                    item = currentFile.Item;
                    item.Update();
                }

                SPContentTypeId contenttypeid = new SPContentTypeId(this.ddl_contentname.SelectedValue);
                SPContentType itemtype = rootWeb.AvailableContentTypes[contenttypeid];
                if (currentList.ContentTypes[contenttypeid] == null)
                {
                    try
                    {
                        currentList.ContentTypes.Add(itemtype);
                        currentList.Update();
                    }
                    catch (Exception ex)
                    {
                        ex.ToString();
                    }
                }

                item = currentFile.Item;

                if (item != null)
                {
                    item[SPBuiltInFieldId.ContentTypeId] = itemtype.Id;
                    item[SPBuiltInFieldId.ContentType] = itemtype.Name;
                    item.Update();


                    item[SPBuiltInFieldId.Title] = this.tbx_title.Text;
                    item[SPBuiltInFieldId.Comments] = this.tbx_description.Text;

                    PageLayout layout = servicePageLayouts.GetPageLayout(new Guid(this.ddl_pagelayout.SelectedValue));
                    SPFieldMultiColumnValue value = new SPFieldMultiColumnValue(3);
                    value[0] = layout.UniqueId.ToString();
                    value[1] = layout.Title;
                    value[2] = layout.Url;

                    item[BuildFieldId.PublishingPageDesign] = value;
                    //item["vti_progid"] = "WebSite.WebSitePage";
                    //item["HTML_x0020_File_x0020_Type"] = "WebSite.WebSitePage";
                    /*
                    if (item.Properties != null && item.Properties.ContainsKey("vti_progid"))
                    {
                        item.Properties["vti_progid"] = "WebSite.WebSitePage";
                    }
                    else
                    {
                        if (item.Properties != null)
                        {
                            item.Properties.Add("vti_progid", "WebSite.WebSitePage");
                        }
                    }

                    if (item.Properties != null && item.Properties.ContainsKey("HTML_x0020_File_x0020_Type"))
                    {
                        item.Properties["HTML_x0020_File_x0020_Type"] = "WebSite.WebSitePage";
                    }
                    else
                    {
                        if (item.Properties != null)
                        {
                            item.Properties.Add("HTML_x0020_File_x0020_Type", "WebSite.WebSitePage");
                        }
                    }
                    */
                    //item.Properties["HTML_x0020_File_x0020_Type"] = "WebSite.WebSitePage";
                    try
                    {
                        item.Update();
                    }
                    catch (Exception ex)
                    {
                        ex.ToString();
                    }
                    finally
                    {
                        item.SystemUpdate(false);
                    }

                }

                if (currentFile != null && currentFile.Exists)
                {
                    currentFile.Update();
                    if (currentFile.CheckOutType != SPFile.SPCheckOutType.None)
                    {
                        currentFile.CheckIn("System Creation Checkin", SPCheckinType.MajorCheckIn);
                    }
                }
                ((EnhancedLayoutsPage)Page).PageToRedirectOnOK = item.File.ServerRelativeUrl;
                ((EnhancedLayoutsPage)Page).EndOperation(ModalDialogResult.OK, item.File.ServerRelativeUrl);
            }
        }

        private string GetTemplate()
        {

            StringBuilder builder = new StringBuilder();

            builder.AppendLine("<%@ Page language=\"C#\" MasterPageFile=\"~masterurl/default.master\"   Inherits=\" Hemrika.SharePresence.WebSite.Page.WebSitePage,Hemrika.SharePresence.WebSite,Version=1.0.0.0,Culture=neutral,PublicKeyToken=11e6604a27f32a11\" meta:progid=\"WebSite.WebSitePage\" meta:webpartpageexpansion=\"full\"  %>");
            builder.AppendLine("<%@ Assembly Name=\"Microsoft.Web.CommandUI, Version=15.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c\" %>");
            builder.AppendLine("<%@ Register Tagprefix=\"SharePoint\" Namespace=\"Microsoft.SharePoint.WebControls\" Assembly=\"Microsoft.SharePoint, Version=15.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c\" %>");
            builder.AppendLine("<%@ Register Tagprefix=\"Utilities\" Namespace=\"Microsoft.SharePoint.Utilities\" Assembly=\"Microsoft.SharePoint, Version=15.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c\" %>");
            builder.AppendLine("<%@ Register Tagprefix=\"WebPartPages\" Namespace=\"Microsoft.SharePoint.WebPartPages\" Assembly=\"Microsoft.SharePoint, Version=15.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c\" %>");
            builder.AppendLine("<%@ Import Namespace=\"Microsoft.SharePoint\" %>");
            builder.AppendLine("<%@ Import Namespace=\"Hemrika.SharePresence.Common\" %>");
            builder.AppendLine("<%@ Import Namespace=\"Hemrika.SharePresence.WebSite\" %>");
            return builder.ToString();
        }

        protected void ddl_contentgroup_SelectedIndexChanged(object sender, EventArgs e)
        {
            this.ddl_contentname.Items.Clear();
            SPWeb rootWeb = SPContext.Current.Site.RootWeb;
            //SPList list = rootWeb.GetList("_catalogs/masterpage");
            SPContentTypeCollection availableContentTypes = rootWeb.AvailableContentTypes;

            foreach (SPContentType contenttype in availableContentTypes)
            {
                if (contenttype.Group == this.ddl_contentgroup.SelectedValue)
                {
                    List<PageLayout> check = servicePageLayouts.GetPageLayouts(contenttype.Id);
                    if (check.Count > 0)
                    {
                        ListItem item = new ListItem(contenttype.Name, contenttype.Id.ToString());
                        this.ddl_contentname.Items.Add(item);
                    }
                }
            }

            this.ddl_pagelayout.Items.Clear();
            if (this.ddl_contentname.Items.Count > 0)
            {
                this.ddl_contentname.SelectedIndex = 0; ;
                SPContentTypeId contenttypeid = new SPContentTypeId(this.ddl_contentname.SelectedValue);
                
                List<PageLayout> layouts = servicePageLayouts.GetPageLayouts(contenttypeid);

                foreach (PageLayout layout in layouts)
                {
                    ListItem item = new ListItem(layout.Title, layout.UniqueId.ToString());
                    if (!this.ddl_pagelayout.Items.Contains(item))
                    {
                        this.ddl_pagelayout.Items.Add(item);
                    }
                }

                if (this.ddl_contentname.Items.Count > 0)
                {
                    this.ddl_pagelayout.SelectedIndex = 0;
                    PageLayout layout = servicePageLayouts.GetPageLayout(new Guid(this.ddl_pagelayout.SelectedValue));
                    this.lbl_description.Text = layout.Description;
                }
            }
        }

        protected void ddl_contentname_SelectedIndexChanged(object sender, EventArgs e)
        {
            this.ddl_pagelayout.Items.Clear();
            SPContentTypeId contenttypeid = new SPContentTypeId(this.ddl_contentname.SelectedValue);

            List<PageLayout> layouts = servicePageLayouts.GetPageLayouts(contenttypeid);

            foreach (PageLayout layout in layouts)
            {
                ListItem item = new ListItem(layout.Title, layout.UniqueId.ToString());
                if (!this.ddl_pagelayout.Items.Contains(item))
                {
                    this.ddl_pagelayout.Items.Add(item);
                }
            }

            if (this.ddl_pagelayout.Items.Count > 0)
            {
                this.ddl_pagelayout.SelectedIndex = 0;
                PageLayout layout = servicePageLayouts.GetPageLayout(new Guid(this.ddl_pagelayout.SelectedValue));
                this.lbl_description.Text = layout.Description;
            }
        }

        protected void btn_Cancel_Click(object sender, EventArgs e)
        {
            SPUtility.Redirect(string.Empty, SPRedirectFlags.UseSource, this.Context);
        }

        protected void ddl_library_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        protected void ddl_pagelayout_SelectedIndexChanged(object sender, EventArgs e)
        {
            PageLayout layout = servicePageLayouts.GetPageLayout(new Guid(this.ddl_pagelayout.SelectedValue));
            this.lbl_description.Text = layout.Description;

        }
    }
}