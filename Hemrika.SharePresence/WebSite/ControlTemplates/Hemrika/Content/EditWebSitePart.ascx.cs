using System;
using System.Linq;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using Hemrika.SharePresence.Common.UI;
using Microsoft.SharePoint;
using Microsoft.SharePoint.WebPartPages;
using Hemrika.SharePresence.WebSite.WebParts;
using System.Reflection;
using Hemrika.SharePresence.Common;
using Hemrika.SharePresence.Common.ServiceLocation;
using System.Collections.Generic;
using System.ComponentModel;
using System.Web.UI.HtmlControls;

namespace Hemrika.SharePresence.WebSite.ControlTemplates
{
    public partial class EditWebSitePart : UserControl
    {
        #region Constants

        /// <summary>
        /// The query string name for the page url.
        /// </summary>
        private const string WEBIDQSKEYNAME = "webId";

        /// <summary>
        /// The query string name for the webpart id.
        /// </summary>
        private const string ITEMIDQSKEYNAME = "itemId";

        /// <summary>
        /// The query string name for the webpart id.
        /// </summary>
        private const string WEBPARTIDQSKEYNAME = "webPartId";

        #endregion

        private SPWeb web = null;
        private SPFile file = null;
        private SPListItem item = null;
        private WebSitePart webpart = null;

        protected void Page_Load(object sender, EventArgs e)
        {
            this.EnableViewState = true;

            if (CheckQueryStringValues())
            {

                web = SPContext.Current.Site.OpenWeb(new Guid(this.Page.Request.QueryString[WEBIDQSKEYNAME]));
                file = web.GetFile(new Guid(this.Page.Request.QueryString[ITEMIDQSKEYNAME]));
                item = file.Item;

                WebPartServiceInit();
                FindWebPart();

                //Only set the values if not postback or callback
                if (Page.IsPostBack && Page.IsCallback)
                {
                    
                    ViewState.Clear();
                    SetValues();
                }
                else
                {
                    CreateChildControls();
                    GetValues();
                }
            }
        }

        private void WebPartServiceInit()
        {
            WebPartServiceLocator.Current.Initialize(GetCompositionAssemblies().Concat(new Assembly[]
                    {
                        Assembly.GetExecutingAssembly(),
                        Assembly.GetCallingAssembly()
                    })
                    );
        }

        protected virtual IEnumerable<Assembly> GetCompositionAssemblies()
        {
            return new List<Assembly>();
        }

        protected override void CreateChildControls()
        {
            if (webpart != null)
            {
                PropertyInfo[] properties = webpart.GetType().GetProperties();

                foreach (PropertyInfo propertyInfo in properties)
                {
                    Type propertyType = propertyInfo.PropertyType;
                    object[] attributes = propertyInfo.GetCustomAttributes(false);

                    IDisplayableClass displayClass = WebPartServiceLocator.Current.DisplayableClasses.FirstOrDefault(c => c.IsAppliable(propertyType));

                    if (displayClass != null)
                    {

                        CategoryAttribute categoryAttribute = (CategoryAttribute)propertyInfo.GetCustomAttributes(typeof(CategoryAttribute), false).FirstOrDefault();

                        if (categoryAttribute != null)
                        {
                            HtmlGenericControl fieldset = null;
                            //HtmlGenericControl ol = null;

                            Control previous = section.FindControl("fieldset_" + categoryAttribute.Category);
                            if (previous == null)
                            {

                                fieldset = new HtmlGenericControl("fieldset");
                                fieldset.ID = "fieldset_" + categoryAttribute.Category;
                                HtmlGenericControl legend = new HtmlGenericControl("legend");
                                legend.ID = "legend_" + categoryAttribute.Category;
                                legend.InnerText = categoryAttribute.Category;
                                fieldset.Controls.Add(legend);

                                //ol = new HtmlGenericControl("ol");
                                //ol.ID = "ol_" + categoryAttribute.Category;
                                //fieldset.Controls.Add(ol);

                                section.Controls.Add(fieldset);
                            }
                            else
                            {
                                fieldset = (HtmlGenericControl)previous;
                                //ol = (HtmlGenericControl)fieldset.FindControl("ol_" + categoryAttribute.Category);
                            }


                            //HtmlGenericControl li = new HtmlGenericControl("li");
                            //li.ID = "li_" + propertyInfo.Name.ToString();
                            //ol.Controls.Add(li);

                            DisplayNameAttribute displayNameAttribute = attributes.OfType<DisplayNameAttribute>().FirstOrDefault();

                            if (displayNameAttribute != null)
                            {
                                if (displayClass.GetType() != typeof(DisplayableBoolean))
                                {
                                    Label label = new Label();
                                    label.Attributes.Add("for", displayNameAttribute.DisplayName);
                                    DescriptionAttribute descriptionAttribute = attributes.OfType<DescriptionAttribute>().FirstOrDefault();
                                    if (descriptionAttribute != null)
                                    {
                                        label.Attributes.Add("title", descriptionAttribute.Description);
                                    }

                                    label.Text = displayNameAttribute.DisplayName;
                                    //li.Controls.Add(label);
                                    fieldset.Controls.Add(label);
                                }

                                Control control = displayClass.CreateControl();

                                if (displayClass.GetType() == typeof(DisplayableBoolean))
                                {
                                    WebControl checkbox = (WebControl)control;
                                    checkbox.Attributes.Add("Text", displayNameAttribute.DisplayName);
                                    checkbox.Attributes.Add("TextAlign", "Right");
                                    fieldset.Controls.Add(checkbox);
                                }
                                else
                                {
                                    control.ID = "control_" + propertyInfo.Name.ToString();
                                    fieldset.Controls.Add(control);
                                }
                            }
                            else
                            {
                                section.Controls.Remove(fieldset);
                            }
                        }
                    }
                }
            }

            base.CreateChildControls();
        }

        private void FindWebPart()
        {
            using (SPLimitedWebPartManager wpm = file.GetLimitedWebPartManager(PersonalizationScope.Shared))
            {
                foreach (System.Web.UI.WebControls.WebParts.WebPart wpm_webpart in wpm.WebParts)
                {
                    if (this.Page.Request.QueryString[WEBPARTIDQSKEYNAME] == wpm_webpart.ID)
                    {
                        webpart = wpm_webpart as WebSitePart;
                        break;
                    }
                }

            }
        }

        private void SetValues()
        {
            try
            {
                if (webpart != null)
                {
                    PropertyInfo[] properties = webpart.GetType().GetProperties();

                    foreach (PropertyInfo propertyInfo in properties)
                    {
                        Type propertyType = propertyInfo.PropertyType;
                        object[] attributes = propertyInfo.GetCustomAttributes(false);
                        IDisplayableClass displayClass = WebPartServiceLocator.Current.DisplayableClasses.FirstOrDefault(c => c.IsAppliable(propertyType));

                        if (displayClass != null)
                        {
                            try
                            {
                                object propertyValue = null;
                                try
                                {
                                    propertyValue = propertyInfo.GetValue(webpart, null);
                                }
                                catch 
                                {
                                    propertyValue = string.Empty;
                                }

                                foreach (Control ctrl in this.Controls)
                                {
                                    string id = ctrl.ID;
                                    string clientId = ctrl.ClientID;
                                }

                                Control control = this.FindControl("control_" + propertyInfo.Name.ToString());
                                if (control != null)
                                {
                                    if (propertyInfo != null || string.IsNullOrEmpty(propertyValue.ToString()))
                                    {
                                        displayClass.SetControlValue(control, propertyValue);
                                        this.ViewState.Add(control.ID, propertyValue);
                                    }
                                    else
                                    {
                                        DefaultValueAttribute defaultValueAttribute = attributes.OfType<DefaultValueAttribute>().FirstOrDefault();
                                        if (defaultValueAttribute != null)
                                        {
                                            displayClass.SetControlValue(control, defaultValueAttribute.Value);
                                            this.ViewState.Add(control.ID, propertyValue);
                                        }
                                        else
                                        {
                                            Control li = this.FindControl("li_" + propertyInfo.Name.ToString());
                                            if (li != null)
                                            {
                                                li.Parent.Controls.Remove(li);
                                            }
                                        }
                                    }
                                }
                            }
                            catch (Exception)
                            {
                                Control li = this.FindControl("li_" + propertyInfo.Name.ToString());
                                if (li != null)
                                {
                                    li.Parent.Controls.Remove(li);
                                }
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                //ol.Controls.Remove(li);
                ex.ToString();
            }
        }

        private void GetValues()
        {
            try
            {
                if (webpart != null)
                {
                    PropertyInfo[] properties = webpart.GetType().GetProperties();

                    foreach (PropertyInfo propertyInfo in properties)
                    {
                        Type propertyType = propertyInfo.PropertyType;
                        object[] attributes = propertyInfo.GetCustomAttributes(false);
                        IDisplayableClass displayClass = WebPartServiceLocator.Current.DisplayableClasses.FirstOrDefault(c => c.IsAppliable(propertyType));

                        if (displayClass != null)
                        {
                            try
                            {
                                Control control = this.FindControl("control_" + propertyInfo.Name.ToString());

                                if (control != null)
                                {
                                    object propertyValue = this.ViewState[control.ID];

                                    if (propertyInfo != null || string.IsNullOrEmpty(propertyValue.ToString()))
                                    {
                                        displayClass.SetControlValue(control, propertyValue);
                                        //this.ViewState.Add(control.ID, propertyValue);
                                    }
                                    else
                                    {
                                        DefaultValueAttribute defaultValueAttribute = attributes.OfType<DefaultValueAttribute>().FirstOrDefault();
                                        if (defaultValueAttribute != null)
                                        {
                                            displayClass.SetControlValue(control, defaultValueAttribute.Value);
                                            //this.ViewState.Add(control.ID, propertyValue);
                                        }
                                        else
                                        {
                                            Control li = this.FindControl("li_" + propertyInfo.Name.ToString());
                                            if (li != null)
                                            {
                                                li.Parent.Controls.Remove(li);
                                            }
                                        }
                                    }
                                }
                            }
                            catch (Exception)
                            {
                                Control li = this.FindControl("li_" + propertyInfo.Name.ToString());
                                if (li != null)
                                {
                                    li.Parent.Controls.Remove(li);
                                }
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                //ol.Controls.Remove(li);
                ex.ToString();
            }
        }

        private bool CheckQueryStringValues()
        {
            //Check that the page url was supplied
            if ((this.Page.Request.QueryString[WEBIDQSKEYNAME] == null) && (String.IsNullOrEmpty(this.Page.Request.QueryString[WEBIDQSKEYNAME])))
            {
                return false;
            }

            //Check that the item id was supplied
            if ((this.Page.Request.QueryString[ITEMIDQSKEYNAME] == null) && (String.IsNullOrEmpty(this.Page.Request.QueryString[ITEMIDQSKEYNAME])))
            {
                return false;
            }

            //Check that the webpart id was supplied
            if ((this.Page.Request.QueryString[WEBPARTIDQSKEYNAME] == null) && (String.IsNullOrEmpty(this.Page.Request.QueryString[WEBPARTIDQSKEYNAME])))
            {
                return false;
            }

            return true;
        }

        protected void Btn_Save_Click(object sender, EventArgs e)
        {
            ApplyChanges();
            var eventArgsJavaScript = String.Format("{{Message:'{0}',controlIDs:window.frameElement.dialogArgs}}", "The Properties have been updated.");

            ((EnhancedLayoutsPage)Page).EndOperation(ModalDialogResult.OK, eventArgsJavaScript);
            Context.Response.Flush();
            Context.Response.End();
        }

        protected void Btn_Cancel_Click(object sender, EventArgs e)
        {
            ((EnhancedLayoutsPage)Page).EndOperation(ModalDialogResult.Cancel);
        }

        private bool ApplyChanges()
        {
            using (SPLimitedWebPartManager wpm = file.GetLimitedWebPartManager(PersonalizationScope.Shared))
            {

                foreach (System.Web.UI.WebControls.WebParts.WebPart wpm_webpart in wpm.WebParts)
                {
                    if (this.Page.Request.QueryString[WEBPARTIDQSKEYNAME] == wpm_webpart.ID)
                    {
                        webpart = wpm_webpart as WebSitePart;
                        break;
                    }
                }

                if (webpart != null)
                {
                    wpm.Web.AllowUnsafeUpdates = true;


                    PropertyInfo[] properties = webpart.GetType().GetProperties();

                    foreach (PropertyInfo propertyInfo in properties)
                    {

                        Type propertyType = propertyInfo.PropertyType;
                        IDisplayableClass displayClass = WebPartServiceLocator.Current.DisplayableClasses.FirstOrDefault(c => c.IsAppliable(propertyType));
                        Control control = FindControlRecursive(section,"control_" + propertyInfo.Name.ToString());
                        if (control != null)
                        {
                            propertyInfo.SetValue(webpart, displayClass.GetControlValue(control), null);
                        }
                    }

                    wpm.SaveChanges(webpart);
                    wpm.Web.AllowUnsafeUpdates = false;

                    foreach (Microsoft.SharePoint.WebPartPages.WebPart wp in wpm.WebParts)
                    {
                        wpm.SaveChanges(wp);
                    }
                }
            }
            return true;
        }

        private static Control FindControlRecursive(Control container, string name)
        {
            if ((container.ID != null) && (container.ID.Equals(name)))
                return container;

            foreach (Control ctrl in container.Controls)
            {
                Control foundCtrl = FindControlRecursive(ctrl, name);
                if (foundCtrl != null)
                    return foundCtrl;
            }
            return null;
        }
    }
}
