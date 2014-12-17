using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.SharePoint.WebControls;
using Hemrika.SharePresence.Html5.WebControls;
using Hemrika.SharePresence.WebSite.FieldTypes;
using System.Collections;
using Microsoft.SharePoint;
using System.Web.UI.WebControls;
using System.Web.UI.HtmlControls;
using System.Web.UI;
using System.Text.RegularExpressions;

namespace Hemrika.SharePresence.WebSite.FieldTypes
{
    [ToolboxData("<{0}:HTML5ImageControl runat=server></{0}:HTML5ImageControl>")]
    public class HTML5ImageControl : BaseFieldControl
    {
        protected HiddenField html_image_hidden;
        protected Hemrika.SharePresence.Html5.WebControls.Image image;
        protected HyperLink upload;
        protected HTML5ImagePicker imagePicker;
        protected HTML5ImageField imageField;

        protected override void OnLoad(EventArgs e)
        {
            base.OnLoad(e);
        }

        protected override string DefaultTemplateName
        {
            get
            {
                if (base.ControlMode == SPControlMode.Display)
                {
                    return this.DisplayTemplateName;
                }
                return "HTML5Image";
            }
        }

        public override string DisplayTemplateName
        {
            get
            {
                return "HTML5ImageDisplay";
            }
            set
            {
                base.DisplayTemplateName = value;
            }
        }

        protected override void OnPreRender(EventArgs e)
        {
            base.OnPreRender(e);
        }

        protected override void CreateChildControls()
        {
            this.DisableInputFieldLabel = true;
            base.ControlMode = SPContext.Current.FormContext.FormMode;
            base.CreateChildControls();

            imageField = (HTML5ImageField)ItemFieldValue;

            if (imageField == null)
            {
                imageField = new HTML5ImageField();
            }

            html_image_hidden = (HiddenField)this.TemplateContainer.FindControl("html_image_hidden");
            imagePicker = (HTML5ImagePicker)this.TemplateContainer.FindControl("ImagePicker");
            image = (Hemrika.SharePresence.Html5.WebControls.Image)this.TemplateContainer.FindControl("html_image");
            
            
            SPListItem item = null;
            SPFile file = null;
            //SPWeb web = null;

            if (!ChildControlsCreated)
            {

                if (image != null)
                {
                    imageField = (HTML5ImageField)Value;
                    if (imageField != null)
                    {
                        if (imageField.WebId != Guid.Empty && imageField.ItemId != Guid.Empty)
                        {
                            using (SPWeb web = SPContext.Current.Site.OpenWeb(imageField.WebId))
                            {
                                if (web.Exists)
                                {
                                    file = web.GetFile(imageField.ItemId);
                                    if (file.Exists)
                                    {
                                        item = file.Item;
                                        image.Src = "~/" + file.Url;
                                        image.Alt = Convert.ToString(item["ows_Description"]);

                                        if (string.IsNullOrEmpty(imageField.Title))
                                        {
                                            image.Title = imageField.Title;
                                        }

                                        if (string.IsNullOrEmpty(imageField.Style))
                                        {
                                            image.Style = imageField.Style;
                                        }
                                    }
                                    else
                                    {
                                        image.Visible = false;
                                    }
                                }
                                else
                                {
                                    image.Visible = false;
                                }
                            }
                        }
                        else
                        {
                            bool UseRoot = false;
                            using (SPWeb web = SPContext.Current.Site.OpenWeb(imageField.Src))
                            {
                                if (web.Exists)
                                {
                                    file = web.GetFile(imageField.Src);

                                    if (file.Exists)
                                    {
                                        item = file.Item;
                                        image.Src = "~/" + file.Url;
                                        image.Alt = Convert.ToString(item["ows_Description"]);

                                        if (string.IsNullOrEmpty(imageField.Title))
                                        {
                                            image.Title = imageField.Title;
                                        }

                                        if (string.IsNullOrEmpty(imageField.Style))
                                        {
                                            image.Style = imageField.Style;
                                        }
                                    }
                                    else
                                    {
                                        image.Visible = false;
                                    }
                                }
                                else
                                {
                                    UseRoot = true;
                                }
                            }

                            if (UseRoot)
                            {
                                SPWeb web = SPContext.Current.Site.RootWeb;

                                if (web.Exists)
                                {
                                    file = web.GetFile(imageField.Src);

                                    if (file.Exists)
                                    {
                                        item = file.Item;
                                        image.Src = "~/" + file.Url;
                                        image.Alt = Convert.ToString(item["ows_Description"]);

                                        if (string.IsNullOrEmpty(imageField.Title))
                                        {
                                            image.Title = imageField.Title;
                                        }

                                        if (string.IsNullOrEmpty(imageField.Style))
                                        {
                                            image.Style = imageField.Style;
                                        }
                                    }
                                    else
                                    {
                                        image.Visible = false;
                                    }
                                }
                            }                               
                        }
                    }
                }

                if (base.ControlMode == SPControlMode.Edit || base.ControlMode == SPControlMode.New)
                {
                    if (!Page.IsCallback)
                    {
                        HTML5ImagePropertyBag properties = new HTML5ImagePropertyBag();

                        if (item != null)
                        {
                            properties.ItemId = item.UniqueId;
                            properties.WebId = item.Web.ID;
                            properties.ListId = item.ParentList.ID;
                        }
                        else
                        {
                            properties.WebId = SPContext.Current.Web.ID;
                            properties.ListId = SPContext.Current.ListId;
                            properties.ItemId = SPContext.Current.ListItem.UniqueId;
                        }

                        if (imagePicker != null)
                        {
                            upload = (HyperLink)imagePicker.FindControl("upload");

                            if (upload != null)
                            {
                                upload.NavigateUrl = String.Format(upload.NavigateUrl, image.ClientID, imagePicker.ClientID, base.Web.ID);
                            }

                            //imagePicker.OnValueChangedClientScript = "UpdateImageAfterDialog('" + image.ClientID + "','" + imagePicker.ClientID + "');";
                            //imagePicker.AfterCallbackClientScript = "UpdateImageAfterDialog('" + image.ClientID + "','" + imagePicker.ClientID + "');";
                            imagePicker.OnValueChangedClientScript = "UpdateImageAfterDialog('" + html_image_hidden.ClientID + "','" + imagePicker.ClientID + "');";
                            imagePicker.AfterCallbackClientScript = "UpdateImageAfterDialog('" + html_image_hidden.ClientID + "','" + imagePicker.ClientID + "');";
                            
                            HTML5ImagePickerEntity entity = new HTML5ImagePickerEntity(item);

                            imagePicker.CustomProperty = properties.ToString();
                            imagePicker.Entities.Add(entity);
                            imagePicker.UpdateEntities(imagePicker.Entities);
                        }
                    }
                }
                ChildControlsCreated = true;
            }
        }

        public override object Value
        {
            get
            {
                this.EnsureChildControls();

                if (imagePicker != null)
                {
                    this.imagePicker.Entities.Count.ToString();

                    ArrayList resolvedEntities = this.imagePicker.ResolvedEntities;
                    if (resolvedEntities.Count == 0)
                        return this.ItemFieldValue;

                    if (resolvedEntities.Count == 1)
                    {
                        HTML5ImagePickerEntity imageEntity = (HTML5ImagePickerEntity)resolvedEntities[0];
                        return new HTML5ImageField(imageEntity.Src, imageEntity.Alt);
                    }
                    else
                    {
                        return new HTML5ImageField();
                    }
                    //else
                        //throw new IndexOutOfRangeException();
                }
                else
                {
                    return this.ItemFieldValue;
                }
            }

            set
            {
                this.EnsureChildControls();
                this.SetFieldControlValue(value);
            }
        }

        private void SetFieldControlValue(object value)
        {
            ArrayList list = new ArrayList();
            HTML5ImageField imageField = null;

            if (imagePicker != null)
            {
                if (this.ControlMode == SPControlMode.New && imagePicker.Entities.Count == 0)
                {
                    imageField = new HTML5ImageField();

                    HTML5ImagePickerEntity defaultEntity = this.imagePicker.ValidateEntity(new HTML5ImagePickerEntity() { Key = imageField.Src, DisplayText = imageField.Alt });
                    if (defaultEntity != null)
                        list.Add(defaultEntity);
                    imagePicker.Entities.Clear();
                    imagePicker.UpdateEntities(list);
                }

                if (value != null)
                {
                    ArrayList entities = new ArrayList();
                    HTML5ImageField field = (HTML5ImageField)value;
                    HTML5ImagePickerEntity entity = new HTML5ImagePickerEntity(field);
                    entities.Add(entity);
                    imagePicker.UpdateEntities(entities);
                }
            }
        }

        public override void UpdateFieldValueInItem()
        {

            EnsureChildControls();
            string style = string.Empty;
            string title = string.Empty;

            if (html_image_hidden != null)
            {
                string value = html_image_hidden.Value;

                Regex rimage = new Regex(@"<img\s[^>]*\s[^>]*>", RegexOptions.Compiled);
                string simage = rimage.Match(value).Value;
                if (!string.IsNullOrEmpty(simage))
                {
                    Regex rstyle = new Regex("style=\"([^\"]*)\"", RegexOptions.Compiled);
                    Regex rtitle = new Regex("title=\"([^\"]*)\"", RegexOptions.Compiled);
                    string sstyle = rstyle.Match(simage).Value;
                    title = rtitle.Match(value).Value;

                    if (!string.IsNullOrEmpty(sstyle))
                    {
                        StringBuilder builder = new StringBuilder();
                        Regex rheight = new Regex("height:([^;]*);", RegexOptions.Compiled);
                        Regex rwidth = new Regex("width:([^;]*);", RegexOptions.Compiled);
                        Regex rpadding = new Regex("padding:([^;]*);", RegexOptions.Compiled);
                        Regex rfloat = new Regex("float:([^;]*);", RegexOptions.Compiled);
                        builder.Append(rheight.Match(sstyle).Value);
                        builder.Append(rwidth.Match(sstyle).Value);
                        builder.Append(rpadding.Match(sstyle).Value);
                        builder.Append(rfloat.Match(sstyle).Value);
                        if (builder.Length > 0)
                        {
                            style = builder.ToString();
                        }
                    }
                }
            }

            if (imagePicker.Enabled)
            {
                imagePicker.Validate();
                if (imagePicker.Entities.Count > 0)
                {
                    PickerEntity entity = null;
                    if (imagePicker.ResolvedEntities.Count == 1)
                    {
                        entity = (PickerEntity)imagePicker.ResolvedEntities[0];
                    }
                    else
                    {
                        entity = (PickerEntity)imagePicker.Entities[0];
                    }

                    HTML5ImagePickerEntity imageEntity = new HTML5ImagePickerEntity(entity);

                    HTML5ImageField itemFieldValue = (HTML5ImageField)this.ItemFieldValue;

                    if (imageEntity.IsResolved)
                    {
                        if (itemFieldValue == null)
                        {
                            itemFieldValue = new HTML5ImageField();
                        }
                        itemFieldValue.Alt = imageEntity.Alt;
                        itemFieldValue.Src = imageEntity.Src;
                        itemFieldValue.ItemId = imageEntity.ItemId;
                        itemFieldValue.WebId = imageEntity.WebId;

                        if (!string.IsNullOrEmpty(title))
                        {
                            itemFieldValue.Title = title;
                        }

                        if (string.IsNullOrEmpty(style))
                        {
                            itemFieldValue.Style = style;
                        }

                        this.ItemFieldValue = itemFieldValue;

                        using (SPWeb web = SPContext.Current.Site.OpenWeb(itemFieldValue.WebId))
                        {
                            SPFile file = web.GetFile(itemFieldValue.ItemId);
                            SPListItem item = file.Item;

                            image.Src = "~/" + file.Url;
                            image.Alt = Convert.ToString(item["ows_Description"]);

                            if (!string.IsNullOrEmpty(title))
                            {
                                image.Title = title;
                            }

                            if (!string.IsNullOrEmpty(style))
                            {
                                image.Style = style;
                            }
                        }
                    }
                }
            }
        }
    }
}