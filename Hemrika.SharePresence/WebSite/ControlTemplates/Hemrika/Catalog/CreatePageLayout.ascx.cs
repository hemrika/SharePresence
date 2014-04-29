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
using Hemrika.SharePresence.WebSite.ContentTypes;
using Hemrika.SharePresence.Common.UI;

namespace Hemrika.SharePresence.WebSite.ControlTemplates
{
    public partial class CreatePageLayout : UserControl
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!base.IsPostBack)
            {
                SPWeb web = SPContext.Current.Web;
                SPWeb rootWeb = SPContext.Current.Site.RootWeb;
                SPContentTypeCollection availableContentTypes = rootWeb.AvailableContentTypes;
                SPList catalog = rootWeb.Site.GetCatalog(SPListTemplateType.MasterPageCatalog);

                foreach (SPContentType contenttype in catalog.ContentTypes)
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
                        if (contenttype.Id.IsChildOf(ContentTypeId.PageTemplate) && contenttype.Id != ContentTypeId.PageTemplate && contenttype.Id != ContentTypeId.PageLayout)
                        {
                            ListItem item = new ListItem(contenttype.Name, contenttype.Id.ToString());
                            this.ddl_contentname.Items.Add(item);
                        }
                    }
                }

                if (this.ddl_contentname.Items.Count > 0)
                {
                    this.ddl_contentname.SelectedIndex = 0;
                    SPContentTypeId contenttypeid = new SPContentTypeId(this.ddl_contentname.SelectedValue);

                    SPContentType type = rootWeb.AvailableContentTypes[contenttypeid];

                    if (type != null)
                    {
                        this.lbl_description.Text = type.Description;
                    }
                    this.Btn_Save.Enabled = true;
                }
                else
                {
                    this.Btn_Save.Enabled = false;
                    this.lbl_description.Text = "No content template available.";
                }
            }
        }

        protected void Btn_Save_Click(object sender, EventArgs e)
        {
            SPWeb rootWeb = SPContext.Current.Site.RootWeb;
            SPList currentList = SPContext.Current.Site.GetCatalog(SPListTemplateType.MasterPageCatalog);//rootWeb.GetList("_catalogs/masterpage");

            if (currentList != null)
            {
                SPFolder rootFolder = currentList.RootFolder;
                MemoryStream stream = new MemoryStream();
                StreamWriter writer = new StreamWriter(stream);
                string template = this.GetTemplate();
                writer.Write(template);
                writer.Flush();
                if (!string.IsNullOrEmpty(this.tbx_pagename.Text))
                {
                    rootFolder.Files.Add(this.tbx_pagename.Text + ".aspx", stream, true);
                    SPFile currentFile = rootFolder.Files[this.tbx_pagename.Text + ".aspx"];

                    if (currentFile != null && currentFile.Exists)
                    {
                        currentFile.Properties["vti_progid"] = "WebSite.WebSitePage";
                        currentFile.Properties["HTML_x0020_File_x0020_Type"] = "WebSite.WebSitePage";
                        currentFile.Update();
                    }


                    SPListItem item = currentFile.ListItemAllFields;
                    SPContentType itemtype = rootWeb.AvailableContentTypes[ContentTypeId.PageLayout];

                    if (currentList.ContentTypes[ContentTypeId.PageLayout] == null)
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

                    item["ContentTypeId"] = itemtype.Id;
                    item["ContentType"] = itemtype.Name;
                    item.Update();
                    item.SystemUpdate();


                    item["Title"] = this.tbx_title.Text;
                    item["Description"] = this.tbx_description.Text;
                    SPFieldMultiColumnValue value2 = new SPFieldMultiColumnValue(3);
                    value2[0] = this.ddl_contentgroup.SelectedValue;
                    value2[1] = this.ddl_contentname.SelectedValue;
                    value2[2] = this.ddl_contentname.SelectedItem.Text;
                    item["Publishing ContentType"] = value2;

                    SPFieldMultiColumnValue value3 = new SPFieldMultiColumnValue(3);
                    value3[0] = item.UniqueId.ToString();
                    value3[1] = item.Title;
                    value3[2] = item.File.ServerRelativeUrl;
                    item["Publishing Page Design"] = value3;

                    item.Properties["vti_progid"] = "WebSite.WebSitePageLayout";
                    item.Properties["HTML_x0020_File_x0020_Type"] = "WebSite.WebSitePageLayout";
                    item.Update();

                    ((EnhancedLayoutsPage)Page).EndOperation(ModalDialogResult.OK, item.File.ServerRelativeUrl);
                    //SPUtility.Redirect(string.Empty, SPRedirectFlags.UseSource, this.Context);
                }
            }
        }

        private string GetTemplate()
        {
            StringBuilder builder = new StringBuilder();
            builder.AppendLine("<%@ Page language=\"C#\" MasterPageFile=\"~masterurl/custom.master\"   Inherits=\"Hemrika.SharePresence.WebSite.Page.WebSitePage,Hemrika.SharePresence.WebSite,Version=1.0.0.0,Culture=neutral,PublicKeyToken=3421bd1d946bda6c\" meta:progid=\"WebSite.WebSitePageLayout\" meta:webpartpageexpansion=\"full\"  %>");
            builder.AppendLine("<%@ Assembly Name=\"Microsoft.Web.CommandUI, Version=15.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c\" %>");
            builder.AppendLine("<%@ Import Namespace=\"Microsoft.SharePoint\" %>");
            builder.AppendLine("<%@ Import Namespace=\"Hemrika.SharePresence.Common\" %>");
            builder.AppendLine("<%@ Import Namespace=\"Hemrika.SharePresence.WebSite\" %>");
            builder.AppendLine("<%@ Import Namespace=\"Hemrika.SharePresence.WebSite.FieldTypes\" %>");
            builder.AppendLine("<%@ Register Tagprefix=\"SharePoint\" Namespace=\"Microsoft.SharePoint.WebControls\" Assembly=\"Microsoft.SharePoint, Version=15.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c\" %>");
            builder.AppendLine("<%@ Register Tagprefix=\"Utilities\" Namespace=\"Microsoft.SharePoint.Utilities\" Assembly=\"Microsoft.SharePoint, Version=15.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c\" %>");
            builder.AppendLine("<%@ Register Tagprefix=\"WebPartPages\" Namespace=\"Microsoft.SharePoint.WebPartPages\" Assembly=\"Microsoft.SharePoint, Version=15.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c\" %>");
            builder.AppendLine("<%@ Register Tagprefix=\"SharePresence\" Namespace=\"Hemrika.SharePresence.WebSite.FieldTypes\" Assembly=\"Hemrika.SharePresence.WebSite,Version=1.0.0.0,Culture=neutral,PublicKeyToken=3421bd1d946bda6c\" %>");
            builder.AppendLine("<%@ Register Tagprefix=\"HTML5\" Namespace=\"Hemrika.SharePresence.Html5.WebControls\" Assembly=\"Hemrika.SharePresence.HTML5,Version=1.0.0.0,Culture=neutral,PublicKeyToken=3421bd1d946bda6c\" %>");
            builder.AppendLine("<asp:Content ContentPlaceholderID=\"PlaceHolderPageTitle\" runat=\"server\"><SharePoint:FieldValue id=\"PageTitle\" FieldName=\"Title\" runat=\"server\"/></asp:Content>");
            builder.AppendLine("<asp:Content ContentPlaceholderID=\"PlaceHolderMain\" runat=\"server\">");
            builder.AppendLine("<!-- Place your custom field within this placeholder -->");
            builder.AppendLine("<WebPartPages:WebPartZone runat=\"server\" FrameType=\"TitleBarOnly\" ID=\"Top\" Title=\"loc:Top\" ><ZoneTemplate></ZoneTemplate></WebPartPages:webpartzone>");
            builder.AppendLine("<WebPartPages:WebPartZone runat=\"server\" FrameType=\"TitleBarOnly\" ID=\"Bottom\" Title=\"loc:Bottom\" ><ZoneTemplate></ZoneTemplate></WebPartPages:webpartzone>");
            builder.AppendLine("</asp:Content>");
            return builder.ToString();
        }

        protected void ddl_contentgroup_SelectedIndexChanged(object sender, EventArgs e)
        {
            this.ddl_contentname.Items.Clear();
            SPWeb rootWeb = SPContext.Current.Site.RootWeb;
            SPContentTypeCollection availableContentTypes = rootWeb.AvailableContentTypes;

            foreach (SPContentType contenttype in availableContentTypes)
            {
                if (contenttype.Group == this.ddl_contentgroup.SelectedValue)
                {
                    if (contenttype.Id.IsChildOf(ContentTypeId.PageTemplate) && contenttype.Id != ContentTypeId.PageTemplate && contenttype.Id != ContentTypeId.PageLayout)
                    {
                        ListItem item = new ListItem(contenttype.Name, contenttype.Id.ToString());
                        this.ddl_contentname.Items.Add(item);
                    }
                }
            }

            if (this.ddl_contentname.Items.Count > 0)
            {
                this.ddl_contentname.SelectedIndex = 0;
                SPContentTypeId contenttypeid = new SPContentTypeId(this.ddl_contentname.SelectedValue);

                SPContentType type = rootWeb.AvailableContentTypes[contenttypeid];

                if (type != null)
                {
                    this.lbl_description.Text = type.Description;
                }
                this.Btn_Save.Enabled = true;
            }
            else
            {
                this.lbl_description.Text = "No content template available.";
                this.Btn_Save.Enabled = false;
            }

        }

        protected void ddl_contentname_SelectedIndexChanged(object sender, EventArgs e)
        {
            SPContentTypeId id = new SPContentTypeId(this.ddl_contentname.SelectedValue);
            SPWeb rootWeb = SPContext.Current.Site.RootWeb;

            SPContentType type = rootWeb.AvailableContentTypes[id];

            if (type != null)
            {
                this.lbl_description.Text = type.Description;
            }
            this.Btn_Save.Enabled = true;
        }

        protected void btn_Cancel_Click(object sender, EventArgs e)
        {
            ((EnhancedLayoutsPage)Page).EndOperation(ModalDialogResult.Cancel,SPContext.Current.Site.Url);
            //SPUtility.Redirect(string.Empty, SPRedirectFlags.UseSource, this.Context);
        }
    }
}
