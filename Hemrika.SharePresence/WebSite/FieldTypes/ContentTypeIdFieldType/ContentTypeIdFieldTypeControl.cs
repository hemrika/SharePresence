// <copyright file="ContentTypeIdFieldTypeControl.cs" company="Hemrika">
// Copyright Hemrika. All rights reserved.
// </copyright>
// <author>MARKETING\Administrator</author>
// <date>2012-01-11 16:41:32Z</date>
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
    using System.Collections;

    /// <summary>
    /// Custom field type ContentTypeIdFieldType
    /// DisplayName: ContentTypeIdFieldType
    /// Description: ContentTypeIdFieldType
    /// </summary>
    [Guid("97893b7e-4d62-4bed-bf60-0f12b72fad26")]
    public class ContentTypeIdFieldTypeControl : BaseFieldControl
    {
        protected DropDownList ddl_contentgroup;
        protected DropDownList ddl_contentname;
        protected Label lbl_ContentType;
        protected Label lbl_Description;

        protected override string DefaultTemplateName
        {
            get
            {
                if (base.ControlMode == SPControlMode.Display)
                {
                    return this.DisplayTemplateName;
                }
                return "ContentTypeIdField";
            }
        }

        public override string DisplayTemplateName
        {
            get
            {
                return "ContentTypeIdDisplay";
            }
            set
            {
                base.DisplayTemplateName = value;
            }
        }
        protected override void CreateChildControls()
        {
            ContentTypeIdFieldValue itemFieldValue;// = (ContentTypeIdFieldValue)this.ItemFieldValue;
            base.CreateChildControls();
            SPWeb web = SPContext.Current.Web;

            if (base.ControlMode == SPControlMode.Display)
            {
                this.lbl_ContentType = (Label)this.TemplateContainer.FindControl("lbl_ContentType");
                itemFieldValue = (ContentTypeIdFieldValue)this.ItemFieldValue;
                if (itemFieldValue != null)
                {
                    this.lbl_ContentType.Text = itemFieldValue.StoredName;//[2];
                }
            }
            else
            {
                this.ddl_contentname = (DropDownList)this.TemplateContainer.FindControl("ddl_contentname");
                this.ddl_contentgroup = (DropDownList)this.TemplateContainer.FindControl("ddl_contentgroup");
                this.lbl_Description = (Label)this.TemplateContainer.FindControl("lbl_Description");

                itemFieldValue = (ContentTypeIdFieldValue)this.ItemFieldValue;

                if (itemFieldValue == null || !String.IsNullOrEmpty(itemFieldValue.StoredGroup))
                {
                    this.PopulateControls("Publishing Content Types",SPContentTypeId.Empty);
                }
                else
                {
                    this.PopulateControls(itemFieldValue.StoredGroup,itemFieldValue.Id);
                }

                this.ddl_contentgroup.AutoPostBack = true;
                this.ddl_contentgroup.SelectedIndexChanged += new EventHandler(ddl_contentgroup_SelectedIndexChanged);
                this.ddl_contentname.AutoPostBack = true;
                this.ddl_contentname.SelectedIndexChanged += new EventHandler(ddl_contentname_SelectedIndexChanged);
            }
        }

        private void PopulateControls(string contentTypeGroup, SPContentTypeId contentTypeId)
        {
            if (string.IsNullOrEmpty(contentTypeGroup))
            {
                contentTypeGroup = "Publishing Content Types";
            }

            SPWeb rootWeb = SPContext.Current.Site.RootWeb;
            //SPList list = rootWeb.GetList("_catalogs/masterpage");
            //SPContentTypeCollection availableContentTypes = list.ContentTypes;
            SPContentTypeCollection availableContentTypes = rootWeb.AvailableContentTypes;

            foreach (SPContentType contenttype in availableContentTypes)
            {
                if (!this.ddl_contentgroup.Items.Contains(new ListItem(contenttype.Group)))
                {
                    this.ddl_contentgroup.Items.Add(contenttype.Group);
                }
            }

            this.ddl_contentgroup.Items.Remove("_Hidden");
            this.ddl_contentgroup.SelectedValue = contentTypeGroup;

            foreach (SPContentType contenttype in availableContentTypes)
            {
                if (contenttype.Group == this.ddl_contentgroup.SelectedValue)
                {
                    ListItem item = new ListItem(contenttype.Name, contenttype.Id.ToString());
                    if (!this.ddl_contentname.Items.Contains(item))
                    {
                        this.ddl_contentname.Items.Add(item);
                    }
                }
            }
        
            if (this.ddl_contentname.Items.Count > 0)
            {
                if (SPContentTypeId.Empty != contentTypeId)
                {
                    SPContentType current = availableContentTypes[contentTypeId];

                    if (current != null)
                    {
                        this.ddl_contentname.SelectedValue = current.Id.ToString();
                        this.lbl_Description.Text = current.Description;
                    }
                }
                else
                {
                    SPContentType current = availableContentTypes[new SPContentTypeId(this.ddl_contentname.Items[0].Value)];
                    this.lbl_Description.Text = current.Description;
                }
            }

        }

        protected void ddl_contentgroup_SelectedIndexChanged(object sender, EventArgs e)
        {
            this.ddl_contentname.Items.Clear();
            SPWeb rootWeb = SPContext.Current.Site.RootWeb;
            //SPList list = rootWeb.GetList("_catalogs/masterpage");
            //SPContentTypeCollection availableContentTypes = list.ContentTypes;
            SPContentTypeCollection availableContentTypes = rootWeb.AvailableContentTypes;

            foreach (SPContentType contenttype in availableContentTypes)
            {
                if (contenttype.Group == this.ddl_contentgroup.SelectedValue)
                {
                    this.ddl_contentname.Items.Add(new ListItem(contenttype.Name, contenttype.Id.ToString()));
                }
            }
            
            if (this.ddl_contentname.Items.Count > 0)
            {
                SPContentType current = availableContentTypes[new SPContentTypeId(this.ddl_contentname.Items[0].Value)];
                this.lbl_Description.Text = current.Description;
            }
        }

        protected void ddl_contentname_SelectedIndexChanged(object sender, EventArgs e)
        {
            SPWeb rootWeb = SPContext.Current.Site.RootWeb;
            //SPList list = rootWeb.GetList("_catalogs/masterpage");
            //SPContentTypeCollection availableContentTypes = list.ContentTypes;
            SPContentTypeCollection availableContentTypes = rootWeb.AvailableContentTypes;

            SPContentType type = availableContentTypes[new SPContentTypeId(this.ddl_contentname.SelectedValue)];
            this.lbl_Description.Text = type.Description;
        }

        public override object Value
        {
            get
            {
                this.EnsureChildControls();
                ContentTypeIdFieldValue value = new ContentTypeIdFieldValue();
                value[0] = this.ddl_contentgroup.SelectedValue;
                value[1] = this.ddl_contentname.SelectedValue;
                value[2] = this.ddl_contentname.SelectedItem.Text;
                return value;

                /*
                SPFieldMultiColumnValue value2 = new SPFieldMultiColumnValue(3);
                value2[0] = this.ddl_contentgroup.SelectedValue;
                value2[1] = this.ddl_contentname.SelectedValue;
                value2[2] = this.ddl_contentname.SelectedItem.Text;
                return value2;
                */
            }
            set
            {
                this.EnsureChildControls();
                SPWeb rootWeb = SPContext.Current.Site.RootWeb;
                //SPList list = rootWeb.GetList("_catalogs/masterpage");
                //SPContentTypeCollection availableContentTypes = list.ContentTypes;
                SPContentTypeCollection availableContentTypes = rootWeb.AvailableContentTypes;
                SPFieldMultiColumnValue itemFieldValue = (SPFieldMultiColumnValue)this.ItemFieldValue;
                this.PopulateControls(itemFieldValue[0],new SPContentTypeId(itemFieldValue[1]));
                this.ddl_contentgroup.SelectedValue = itemFieldValue[0];
                this.ddl_contentname.SelectedValue = itemFieldValue[1];
                SPContentType type = availableContentTypes[new SPContentTypeId(itemFieldValue[1])];
                if (type != null)
                {
                    this.lbl_Description.Text = type.Description;
                }

                base.UpdateFieldValueInItem();
            }
        }

        
    }
}

