// <copyright file="PublishingPageDesignControl.cs" company="Hemrika">
// Copyright Hemrika. All rights reserved.
// </copyright>
// <author>MARKETING\Administrator</author>
// <date>2012-02-15 11:56:32Z</date>
namespace Hemrika.SharePresence.WebSite.FieldTypes
{
    using System;
    using System.Globalization;
    using System.Runtime.InteropServices;
    using System.Text;
    using System.Web.UI;
    using System.Web.UI.WebControls;
    using Microsoft.SharePoint;
    using Microsoft.SharePoint.WebControls;
    using Hemrika.SharePresence.Common;
    using Hemrika.SharePresence.WebSite.Layout;
    using Hemrika.SharePresence.Common.Configuration;
    using Hemrika.SharePresence.Common.ServiceLocation;
    using System.Collections.Generic;

    /// <summary>
    /// Custom field type PublishingPageDesign
    /// DisplayName: Publishing Page Design
    /// Description: Publishing PageDesign Field
    /// </summary>
    [Guid("9ff9efea-847f-4c8d-9b3c-82a5692c7f67")]
    public class PublishingPageDesignControl : BaseFieldControl
    {
        protected DropDownList ddl_PublishingLayout;
        protected Label lbl_PublishingLayout;
        protected Label lbl_Description;

        private IServiceLocator serviceLocator;
        private IPageLayoutRepository servicePageLayouts;


        public PublishingPageDesignControl()
        {
            serviceLocator = SharePointServiceLocator.GetCurrent();
            servicePageLayouts = serviceLocator.GetInstance<IPageLayoutRepository>();

        }

        protected override string DefaultTemplateName
        {
            get
            {
                if (base.ControlMode == SPControlMode.Display)
                {
                    return this.DisplayTemplateName;
                }
                return "PublishingPageDesign";
            }
        }

        public override string DisplayTemplateName
        {
            get
            {
                return "PublishingPageDesignDisplay";
            }
            set
            {
                base.DisplayTemplateName = value;
            }
        }

        protected override void CreateChildControls()
        {
            PublishingPageDesignFieldValue itemFieldValue = null;// = (ContentTypeIdFieldValue)this.ItemFieldValue;
            base.CreateChildControls();
            SPWeb web = SPContext.Current.Web;

            if (base.ControlMode == SPControlMode.Display)
            {
                this.lbl_PublishingLayout = (Label)this.TemplateContainer.FindControl("lbl_PublishingLayout");
                itemFieldValue = (PublishingPageDesignFieldValue)this.ItemFieldValue;
                if (itemFieldValue != null)
                {
                    this.lbl_PublishingLayout.Text = itemFieldValue.Title;//[2];
                }
            }
            else
            {
                this.ddl_PublishingLayout = (DropDownList)this.TemplateContainer.FindControl("ddl_PublishingLayout");
                this.lbl_Description = (Label)this.TemplateContainer.FindControl("lbl_Description");

                this.PopulateControls("Publishing Content Types");//, layout);

                try
                {
                    itemFieldValue = (PublishingPageDesignFieldValue)this.ItemFieldValue;
                }
                catch (Exception ex)
                {
                    ex.ToString();
                }

                if (itemFieldValue != null && !itemFieldValue.IsEmpty)
                {
                    PageLayout layout = servicePageLayouts.GetPageLayout(new Guid(itemFieldValue[0]));
                    this.ddl_PublishingLayout.SelectedValue = itemFieldValue[0];
                }
                /*
                else
                {
                    this.PopulateControls(itemFieldValue.StoredGroup, itemFieldValue.Id);
                }
                */
                this.ddl_PublishingLayout.AutoPostBack = true;
                this.ddl_PublishingLayout.SelectedIndexChanged += new EventHandler(ddl_PublishingLayout_SelectedIndexChanged);
            }
        }

        protected void ddl_PublishingLayout_SelectedIndexChanged(object sender, EventArgs e)
        {
            /*
            SPContentTypeId id = new SPContentTypeId(this.ddl_PublishingLayout.SelectedValue);
            SPWeb rootWeb = SPContext.Current.Site.RootWeb;
            //SPList list = rootWeb.GetList("_catalogs/masterpage");
            //SPContentTypeCollection availableContentTypes = list.ContentTypes;
            SPContentTypeCollection availableContentTypes = rootWeb.AvailableContentTypes;

            SPContentType type = availableContentTypes[id];
            this.lbl_Description.Text = type.Description;
            */
            //SPWeb rootWeb = SPContext.Current.Site.RootWeb;
            //SPContentTypeCollection availableContentTypes = rootWeb.AvailableContentTypes;

            PageLayout layout = servicePageLayouts.GetPageLayout(new Guid(this.ddl_PublishingLayout.SelectedValue));
            //SPContentType type = availableContentTypes[new SPContentTypeId(this.ddl_PublishingLayout.SelectedValue)];
            this.lbl_Description.Text = layout.Description;

        }


        private void PopulateControls(string Group)//, PageLayout pageLayout)
        {
            SPWeb rootWeb = SPContext.Current.Site.RootWeb;
            SPContentTypeCollection availableContentTypes = rootWeb.AvailableContentTypes;

            foreach (SPContentType contenttype in availableContentTypes)
            {
                if (contenttype.Group == Group)//"Publishing Content Types")
                {
                    //if (contenttype.Id == SPContext.Current.ListItem.ContentTypeId)
                    //{
                    List<PageLayout> layouts = servicePageLayouts.GetPageLayouts(contenttype.Id);
                    foreach (PageLayout layout in layouts)
                    {
                        ListItem item = new ListItem(layout.Title, layout.UniqueId.ToString());
                        if (!this.ddl_PublishingLayout.Items.Contains(item))
                        {
                            this.ddl_PublishingLayout.Items.Add(item);
                        }
                    }
                    //}
                }
            }

            /*
            if (this.ddl_PublishingLayout.Items.Count > 0)
            {
                if (SPContentTypeId.Empty != contentTypeId)
                {
                    SPContentType current = availableContentTypes[contentTypeId];
                    this.ddl_PublishingLayout.SelectedValue = current.Id.ToString();
                    this.lbl_Description.Text = current.Description;
                }
                else
                {
                    SPContentType current = availableContentTypes[new SPContentTypeId(this.ddl_PublishingLayout.Items[0].Value)];
                    this.lbl_Description.Text = current.Description;
                }
            */
            /*
            //this.ddl_contentname.SelectedValue = Hemrika.SharePresence.WebSite.ContentTypes.ContentTypeId.PageLayout.ToString();
            SPContentType type = availableContentTypes[new SPContentTypeId(this.ddl_PublishingLayout.Items[0].Value)];
            this.lbl_Description.Text = type.Description;
            */
            //}

        }

        public override object Value
        {
            get
            {
                this.EnsureChildControls();
                PublishingPageDesignFieldValue value = new PublishingPageDesignFieldValue();
                if (this.ddl_PublishingLayout != null && this.ddl_PublishingLayout.SelectedValue != String.Empty)
                {
                    PageLayout layout = servicePageLayouts.GetPageLayout(new Guid(this.ddl_PublishingLayout.SelectedValue));
                    value[0] = layout.UniqueId.ToString();
                    value[1] = layout.Title;
                    value[2] = layout.Url;
                }

                return value;
            }
            set
            {
                this.EnsureChildControls();
                //SPWeb rootWeb = SPContext.Current.Site.RootWeb;
                //SPContentTypeCollection availableContentTypes = rootWeb.AvailableContentTypes;
                SPFieldMultiColumnValue itemFieldValue = (SPFieldMultiColumnValue)this.ItemFieldValue;
                PageLayout layout = servicePageLayouts.GetPageLayout(new Guid(itemFieldValue[0]));
                this.PopulateControls(layout.ContentTypeGroup);//,layout);
                this.ddl_PublishingLayout.SelectedValue = itemFieldValue[0];
                //SPContentType type = availableContentTypes[new SPContentTypeId(itemFieldValue[1])];
                this.lbl_Description.Text = layout.Description;
                base.UpdateFieldValueInItem();

            }
        }
    }
}

