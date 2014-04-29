using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web.UI;
using System.Text.RegularExpressions;
using System.IO;
using System.Web;
using System.Net;
using Microsoft.SharePoint;
using Hemrika.SharePresence.Common.TemplateEngine;
using System.Web.UI.HtmlControls;
using Microsoft.SharePoint.WebPartPages;
using Microsoft.SharePoint.WebControls;
using Microsoft.SharePoint.Security;
using System.Security.Permissions;
using Microsoft.SharePoint.Utilities;
using System.Web.UI.WebControls;
using Microsoft.SharePoint.Administration;

namespace Hemrika.SharePresence.WebSite.WebParts
{
    public class FormCanvas : UserControl, IPostBackDataHandler, IPostBackEventHandler
    {
        private Control Container;

        public string CanvasHtml { get; set; }
        public IDictionary<string, string> Replacements { get; set; }
        public FormWebPart WebPart { get; set; }
        private bool Changed = false;
        private bool Result = false;
        private Button sbutton;
        //private Button cbutton;
        public FormCanvas(string InnerHTML) : this(InnerHTML, null) { }

        public FormCanvas(string InnerHTML, Control container)
        {
            //this.EnableViewState = true;
            CanvasHtml = InnerHTML;
            Container = container;
            if (Container == null)
            {
                Container = this;
                sbutton = new Button();
                Container.Controls.Add(sbutton);
                //cbutton = new Button();
                //Container.Controls.Add(cbutton);
            }
        }

        private string idPrefix;

        /// <summary>
        /// Gets the ClientID of this control. 
        /// </summary>
        public override string ClientID
        {
            [SharePointPermission(SecurityAction.Demand, ObjectModel = true)]
            get
            {
                if (this.idPrefix == null)
                {
                    this.idPrefix = SPUtility.GetNewIdPrefix(this.Context);
                }

                return SPUtility.GetShortId(this.idPrefix, this);
            }
        }

        public override string ID
        {
            get
            {
                if (base.ID == null)
                {
                    return SPUtility.GetNewIdPrefix(this.Context) + "_formcanvas";
                }

                return base.ID;
            }
            set
            {
                base.ID = value;
            }
        }
        
        protected override void CreateChildControls()
        {
            if (!WebPart.inEditMode)
            {
                ScriptManager.GetCurrent(Page).RegisterAsyncPostBackControl(this);
                Page.RegisterRequiresPostBack(this);
                Page.RegisterRequiresRaiseEvent(this);
            }

            base.CreateChildControls();
        }

        protected override void OnPreRender(EventArgs e)
        {
            foreach (Control control in Controls)
            {
                Type type = control.GetType();
                if (type.IsSubclassOf(typeof(BaseValidator)))
                {
                    BaseValidator validator = control as BaseValidator;
                    Control validated = FindControl(validator.ControlToValidate);
                    if (validated != null)
                    {
                        validator.ControlToValidate = validated.ClientID;
                        //validator.ValidationGroup = summery.ValidationGroup;

                        if (WebPart.inEditMode)
                        {
                            validator.Enabled = false;
                            validator.EnableClientScript = false;
                        }
                    }
                }
            }
            base.OnPreRender(e);
        }

        protected override void Render(HtmlTextWriter writer)
        {
            if (Replacements != null)
            {
                // make the additional replacements
                foreach (var replacement in Replacements)
                    CanvasHtml = CanvasHtml.Replace(replacement.Key, replacement.Value);
            }

            UpdatePanel panel = (UpdatePanel)Container.Parent;
            FormContentTemplate template = (FormContentTemplate)panel.ContentTemplate;

            CanvasHtml = template.innerHtml;
            
            if (!string.IsNullOrEmpty(CanvasHtml))
            {
                PlaceButtons();

                if (Changed)
                {
                    ScriptManager.RegisterStartupScript(this, this.GetType(), "UpdateForm", "UpdateForm();", true);

                    var matches = Regex.Matches(CanvasHtml, "<div[^>]*>");
                    foreach (var match in matches)
                    {
                        var name = Regex.Match(match.ToString(), @"(?<=\bid="")[^""]*");

                        if ((name.ToString().ToLower() == "successful") && Result)
                        {
                            var style = Regex.Match(match.ToString(), @"(?<=\bstyle="")[^""]*");
                            string proccesed = match.ToString().Replace(style.ToString(), "display:block;");
                            CanvasHtml = CanvasHtml.Replace(match.ToString(), proccesed);
                            continue;
                        }

                        if ((name.ToString().ToLower() == "unsuccessful") && !Result)
                        {
                            var style = Regex.Match(match.ToString(), @"(?<=\bstyle="")[^""]*");
                            string proccesed = match.ToString().Replace(style.ToString(), "display:block;");
                            CanvasHtml = CanvasHtml.Replace(match.ToString(), proccesed);
                            MoveFormElements();
                            continue;
                        }

                        if (Result)
                        {
                            var style = Regex.Match(match.ToString(), @"(?<=\bstyle="")[^""]*");
                            string proccesed = match.ToString().Replace(style.ToString(), "display:none;");
                            CanvasHtml = CanvasHtml.Replace(match.ToString(), proccesed);
                            continue;
                        }
                        /*
                        else
                        {
                            var style = Regex.Match(match.ToString(), @"(?<=\bstyle="")[^""]*");
                            string proccesed = match.ToString().Replace(style.ToString(), "display:block;");
                            CanvasHtml = CanvasHtml.Replace(match.ToString(), proccesed);
                            continue;

                        }
                        */
                    }
                }
                else
                {
                    MoveFormElements();
                }
            }

            writer.Write(CanvasHtml);

            var postbackReference = Page.ClientScript.GetPostBackEventReference(this,"ProcessForm", true);
            var postbackScript = String.Format("function ProcessForm() {{ {0}; }}", postbackReference);

            writer.WriteLine(@"<script type=""text/javascript"">");
            writer.WriteLine(postbackScript);
            writer.WriteLine("</script>");
            //base.Render(writer);
        }

        private void PlaceButtons()
        {
            // substitute our controls into the template. regex is fast.
            var matches = Regex.Matches(CanvasHtml, "<formbutton[^>]*>*</formbutton>");//("<[^>]*>", "g");

            foreach (var match in matches)
            {
                string formfield = match.ToString();
                var id = Regex.Match(match.ToString(), @"(?<=\bid="")[^""]*");
                var text = Regex.Match(match.ToString(), @"(?<=\btext="")[^""]*");
                var value = Regex.Match(match.ToString(), @"(?<=\bvalue="")[^""]*");
                var groupvalidation = Regex.Match(match.ToString(), @"(?<=\bGroupValidation="")[^""]*");
                var onclick = Regex.Match(match.ToString(), @"(?<=\bonclick="")[^""]*");

                if (value.ToString() == "Save")
                {
                    sbutton = new Button { ID = id.ToString(), ValidationGroup = groupvalidation.ToString(), OnClientClick = onclick.ToString(), Text = text.ToString() };
                    //Container.Controls.Add(sbutton);
                    CanvasHtml = CanvasHtml.Replace(match.ToString(), RenderControl(sbutton));
                    if (String.IsNullOrEmpty(groupvalidation.ToString()))
                    {
                        foreach (Control control in Controls)
                        {
                            Type type = control.GetType();
                            if (type.IsSubclassOf(typeof(BaseValidator)))
                            {
                                BaseValidator validator = control as BaseValidator;
                                validator.ValidationGroup = groupvalidation.ToString();
                            }
                        }
                    }

                }
                /*
                if (value.ToString() == "Cancel")
                {
                    cbutton = new Button { ID = id.ToString(), OnClientClick = onclick.ToString(), Text = text.ToString() };
                    //Container.Controls.Add(cbutton);
                    CanvasHtml = CanvasHtml.Replace(match.ToString(), RenderControl(cbutton));
                }
                */
                
            }
        }

        private void MoveFormElements()
        {
            // substitute our controls into the template. regex is fast.
            var matches = Regex.Matches(CanvasHtml, "<formfield[^>]*>*</formfield>");

            foreach (var match in matches)
            {
                string formfield = match.ToString();
                var name = Regex.Match(match.ToString(), @"(?<=\bname="")[^""]*");
                var internalname = Regex.Match(match.ToString(), @"(?<=\binternalname="")[^""]*");
                var staticname = Regex.Match(match.ToString(), @"(?<=\bstaticname="")[^""]*");

                var control = FindControl(this, name.ToString());
                if (control == null)
                {
                    control = FindControl(this, internalname.ToString());
                    if (control == null)
                    {
                        control = FindControl(this, staticname.ToString());
                    }

                }

                if (control != null)
                {
                    List<Control> validators = FindValidators(this, control);
                    if (validators.Count > 0)
                    {
                        CanvasHtml = CanvasHtml.Replace(match.ToString(), RenderControl(control, validators));
                    }
                    else
                    {
                        CanvasHtml = CanvasHtml.Replace(match.ToString(), RenderControl(control));
                    }
                }
            }
        }

        private Control FindControl(Control container, string controlID)
        {
            var control = container.FindControl(controlID);
            if (control != null) return control;
            else
            {
                foreach (Control child in container.Controls)
                {
                    FindControl(child, controlID);
                }
            }

            return control;
        }

        private List<Control> FindValidators(Control container, Control formControl)
        {
            List<Control> validators = new List<Control>();
            foreach (Control control in container.Controls)
            {
                Type type = control.GetType();
                if (type.IsSubclassOf(typeof(BaseValidator)))
                {
                    BaseValidator validator = control as BaseValidator;
                    if (validator.ControlToValidate == formControl.ClientID)
                    {
                        validators.Add(validator);
                    }
                }
            }
            return validators;
        }

        private bool ValidateField(Control container, BaseFieldControl formControl)
        {
            bool valid = true;
            formControl.IsValid = true;
            List<Control> validators = FindValidators(container, formControl);

            foreach (BaseValidator validator in validators)
            {
                validator.Validate();
                if (!validator.IsValid)
                {
                    formControl.IsValid = validator.IsValid;
                    valid = validator.IsValid;
                }
            }
            return valid;
        }

        public string RenderControl(Control ctrl)
        {
            StringBuilder sb = new StringBuilder();
            SPSecurity.RunWithElevatedPrivileges(delegate()
            {
                StringWriter tw = new StringWriter(sb);
                HtmlTextWriter hw = new HtmlTextWriter(tw);
                ctrl.RenderControl(hw);
            });

            return sb.ToString();
        }

        private string RenderControl(Control ctrl, List<Control> validators)
        {
            StringBuilder sb = new StringBuilder();
            SPSecurity.RunWithElevatedPrivileges(delegate()
            {
                StringWriter tw = new StringWriter(sb);
                HtmlTextWriter hw = new HtmlTextWriter(tw);
                ctrl.RenderControl(hw);

                foreach (Control validator in validators)
                {
                    validator.RenderControl(hw);
                }
            });
            return sb.ToString();
        }

        public bool LoadPostData(string postDataKey, System.Collections.Specialized.NameValueCollection postCollection)
        {
            try
            {
                Result = true;
                List<BaseFieldControl> fields = FieldList(this.WebPart);

                foreach (BaseFieldControl field in fields)
                {
                    bool valid = ValidateField(this, field);

                    if (!valid) { Result = valid; }
                }

                /*
                if (Result)
                {
                    CreateItemforForm();
                }
                */
            }
            catch (Exception ex)
            {
                SPDiagnosticsService.Local.WriteTrace(0, new SPDiagnosticsCategory(ex.Source, TraceSeverity.High, EventSeverity.Error), TraceSeverity.High, ex.Message, ex.Data);
                Result = false;
            }
            finally
            {
                if (!WebPart.inEditMode)
                {
                    Changed = true;
                }
            }

            return Result;
        }

        private void CreateItemforForm()
        {
            FormWebPart wp = (FormWebPart)this.WebPart;

            List<BaseFieldControl> fields = new List<BaseFieldControl>();

            List<DesignDataSource> sources = new List<DesignDataSource>();
            if (!string.IsNullOrEmpty(wp.DataSourcesData))
            {
                sources = Utilities.DeserializeObject<List<DesignDataSource>>(wp.DataSourcesData);
            }

            SPSecurity.RunWithElevatedPrivileges(delegate()
            {
                SPSite site = SPContext.Current.Site;

                site.AllowUnsafeUpdates = true;
                foreach (DesignDataSource source in sources)
                {
                    fields = new List<BaseFieldControl>();
                    SPWeb web = site.OpenWeb(new Guid(source.WebId));
                    web.AllowUnsafeUpdates = true;
                    SPList list = web.Lists[new Guid(source.ListId)];
                    SPView view = list.GetView(new Guid(source.ViewId));
                    SPViewFieldCollection viewfields = view.ViewFields;

                    foreach (var viewfield in viewfields)
                    {
                        BaseFieldControl formField = (BaseFieldControl)FindControl(viewfield.ToString());
                        if (formField == null) continue;

                        fields.Add(formField);
                    }

                    SPListItem item = list.Items.Add();

                    foreach (BaseFieldControl Fld in fields)
                    {
                        try
                        {
                            item[Fld.FieldName] = Fld.Value;
                        }
                        catch { };
                    }
                    UpdateUserInfo(item);

                    item.Update();

                    web.AllowUnsafeUpdates = false;
                }
                site.AllowUnsafeUpdates = false;
            });
        }

        private void UpdateUserInfo(SPListItem item)
        {
            SPFieldUserValueCollection fieldValues = new SPFieldUserValueCollection();
            //SPUser user = SPContext.Current.Web.EnsureUser("LogonName");
            SPUser user = SPContext.Current.Web.CurrentUser;
            if (user != null)
            {
                fieldValues.Add(new SPFieldUserValue(item.Web, user.ID, user.Name));
                item[SPBuiltInFieldId.Author] = fieldValues;
                item[SPBuiltInFieldId.Editor] = fieldValues;
            }
        }

        public void RaisePostDataChangedEvent()
        {
            /*
            int count = this.Controls.Count;

            try
            {
                Result = true;
                List<BaseFieldControl> fields = FieldList(this.WebPart);

                foreach (BaseFieldControl field in fields)
                {
                    bool valid = ValidateField(this, field);

                    if (!valid) { Result = valid; }
                }

                if (Result)
                {
                    CreateItemforForm();
                }
            }
            catch (Exception ex)
            {
                SPDiagnosticsService.Local.WriteTrace(0, new SPDiagnosticsCategory(ex.Source, TraceSeverity.High, EventSeverity.Error), TraceSeverity.High, ex.Message, ex.Data);
                Result = false;
            }
            finally
            {
                if (!WebPart.inEditMode)
                {
                    Changed = true;
                }
            }

            //return Result;
            */
            /*
            int count = this.Controls.Count;
            List<FormField> fields = FieldList(this.WebPart);

            foreach (FormField field in fields)
            {
                if (field.Value != null)
                {
                    field.Value.ToString();
                }
            }
            Changed = true;
            //CanvasHtml = "Update Complete";
            */
        }

        public void RaisePostBackEvent(string eventArgument)
        {
            try
            {

                Result = true;
                List<BaseFieldControl> fields = FieldList(this.WebPart);

                foreach (BaseFieldControl field in fields)
                {
                    bool valid = ValidateField(this, field);

                    if (!valid) { Result = valid; }
                }

                if (Result && !this.WebPart.inEditMode)
                {
                    CreateItemforForm();
                }
            }
            catch (Exception ex)
            {
                SPDiagnosticsService.Local.WriteTrace(0, new SPDiagnosticsCategory(ex.Source, TraceSeverity.High, EventSeverity.Error), TraceSeverity.High, ex.Message, ex.Data);
                Result = false;
            }
            finally
            {
                if (!WebPart.inEditMode)
                {
                    Changed = true;
                }
            }

            /*
            List<FormField> fields = FieldList(this.WebPart);

            foreach (FormField field in fields)
            {
                if (field.Value != null)
                {
                    field.Value.ToString();
                }
            }
            Changed = true;
            //CanvasHtml = "Update Complete";
            */
        }

        internal List<BaseFieldControl> FieldList(WebPart webpart)
        {
            FormWebPart wp = (FormWebPart)webpart;

            List<BaseFieldControl> fields = new List<BaseFieldControl>();

            List<DesignDataSource> sources = new List<DesignDataSource>();
            if (!string.IsNullOrEmpty(wp.DataSourcesData))
            {
                sources = Utilities.DeserializeObject<List<DesignDataSource>>(wp.DataSourcesData);
            }

            SPSecurity.RunWithElevatedPrivileges(delegate()
            {
                SPSite site = SPContext.Current.Site;

                foreach (DesignDataSource source in sources)
                {
                    SPWeb web = site.OpenWeb(new Guid(source.WebId));
                    SPList list = web.Lists[new Guid(source.ListId)];
                    SPView view = list.GetView(new Guid(source.ViewId));
                    SPViewFieldCollection viewfields = view.ViewFields;

                    foreach (var viewfield in viewfields)
                    {
                        BaseFieldControl formField = (BaseFieldControl)FindControl(viewfield.ToString());
                        //formField.ControlMode = Microsoft.SharePoint.WebControls.SPControlMode.Display;
                        if (formField == null) continue;

                        fields.Add(formField);
                    }
                }
            });

            return fields;
        }

    }
}
