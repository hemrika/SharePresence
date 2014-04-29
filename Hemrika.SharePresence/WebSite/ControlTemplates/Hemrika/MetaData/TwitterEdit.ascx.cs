using System;
using System.Linq;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Collections.Generic;
using Microsoft.SharePoint;
using Hemrika.SharePresence.WebSite.MetaData;
using Hemrika.SharePresence.Common;
using Hemrika.SharePresence.Common.UI;
using Microsoft.SharePoint.Administration;
using System.Web.UI.HtmlControls;
using Hemrika.SharePresence.Html5.WebControls;
using Microsoft.SharePoint.Utilities;

namespace Hemrika.SharePresence.WebSite.ControlTemplates
{
    public partial class TwitterEdit : UserControl
    {
        private IEnumerable<SPField> metafields;
        private IEnumerable<SPContentType> publishingtypes;
        private MetaDataSettings settings;
        private SPListItem listitem;

        protected override void OnInit(EventArgs e)
        {
            base.OnInit(e);
            settings = new MetaDataSettings();
            settings = settings.Load(SPContext.Current.Site);
            btn_Save.Click += new EventHandler(btn_Save_Click);
        }

        void btn_Save_Click(object sender, EventArgs e)
        {
            bool UpdateNeeded = false;
            foreach (Control control in Fields.Controls)
            {
                string Id = string.Empty;
                string value = string.Empty;
                Guid fieldId = Guid.Empty;

                if (control.GetType().BaseType == typeof(Html5.WebControls.InputControl))
                {
                    Html5.WebControls.InputControl input = (Html5.WebControls.InputControl)control;
                    Id = input.ID;
                    value = input.Text;                    
                }
                if (control.GetType().BaseType == typeof(ListControl))
                {
                    ListControl input = (ListControl)control;
                    Id = input.ID;
                    if (control.GetType() == typeof(CheckBoxList))
                    {
                        foreach (ListItem item in input.Items)
                        {
                            if (item.Selected)
                            {
                                value += item.Value + ";#";
                            }
                        }
                        value = value.Remove(value.Length - 2);
                    }
                    else
                    {
                        value = input.SelectedValue;
                    }
                }

                if (Hemrika.SharePresence.Common.Validation.GuidTryParse(Id, out fieldId))
                {
                    if (listitem.Fields.Contains(fieldId))
                    {
                        SPField field = listitem.Fields[fieldId];
                        if (field != null)
                        {
                            string oldvalue = field.GetFieldValueAsText(listitem[field.InternalName]);

                            if (oldvalue != value && oldvalue != field.DefaultValue)
                            {
                                listitem[fieldId] = value;
                                UpdateNeeded = true;
                            }

                            if (string.IsNullOrEmpty(value) && !string.IsNullOrEmpty(field.DefaultValue) || value == field.DefaultValue)
                            {
                                listitem[fieldId] = string.Empty;//field.DefaultValue;
                                UpdateNeeded = true;
                            }
                        }
                    }
                }
            }

            if (UpdateNeeded)
            {
                listitem.Update();
            }

            var eventArgsJavaScript = String.Format("{{Message:'{0}',controlIDs:window.frameElement.dialogArgs}}", "The Properties have been updated.");

            ((EnhancedLayoutsPage)Page).EndOperation(ModalDialogResult.OK, eventArgsJavaScript);
            Context.Response.Flush();
            Context.Response.End();

        }

        protected void Page_Load(object sender, EventArgs e)
        {
            GetCurrentItem();

            UpdateMetaFields();

            GetCurrentItem();

            GetMetaFields();

            CreateEditorFields();
        }

        private void GetCurrentItem()
        {
            if (Request.QueryString.AllKeys.Contains("List") && Request.QueryString.AllKeys.Contains("ID"))
            {
                Guid listGuid = new Guid(Request.QueryString["List"]);
                int Id = int.Parse(Request.QueryString["ID"]);
                SPList currentlist = SPContext.Current.Web.Lists[listGuid];
                listitem = currentlist.GetItemById(Id);
            }
        }

        private void CreateEditorFields()
        {
            if (Fields != null && metafields != null)
            {
                foreach (SPField field in metafields)
                {
                    if (listitem.Fields.ContainsField(field.InternalName))
                    {
                        string value = listitem.GetFormattedValue(field.Title);

                        Html5.WebControls.Label label = new Html5.WebControls.Label();
                        label.Title = field.Title;
                        label.Class = "required";
                        label.For = "previous";
                        label.InnerText = field.Title;
                        Fields.Controls.Add(label);

                        if (!string.IsNullOrEmpty(field.Description))
                        {
                            Html5.WebControls.Label description = new Html5.WebControls.Label();
                            description.Title = field.Title;
                            description.Class = "required";
                            description.For = "previous";
                            description.InnerText = field.Description;
                            Fields.Controls.Add(description);

                        }

                        if (string.IsNullOrEmpty(value))
                        {
                            value = field.DefaultValue;
                        }

                        WebControl FieldControl = null;

                        //SPFieldMultiLineText
                        if (field.Type == SPFieldType.Text)
                        {
                            var text = (SPFieldText)field;
                            Html5.WebControls.TextInput input = new Html5.WebControls.TextInput();
                            input.ID = field.Id.ToString(); ;
                            input.Class = "required";
                            input.For = "previous";
                            input.TextMode = TextBoxMode.SingleLine;
                            input.Value = value;
                            FieldControl = input;
                        }

                        //SPFieldText
                        if (field.Type == SPFieldType.Note)
                        {
                            var note = (SPFieldMultiLineText)field;
                            Html5.WebControls.TextInput input = new Html5.WebControls.TextInput();
                            input.ID = field.Id.ToString();
                            input.Class = "required";
                            input.For = "previous";
                            input.Rows = note.NumberOfLines;
                            input.TextMode = TextBoxMode.MultiLine;
                            input.Value = value;
                            FieldControl = input;
                        }

                        if (field.Type == SPFieldType.Choice)
                        {
                            var choice = (SPFieldChoice)field;
                            if (choice.FieldRenderingControl.GetType() == typeof(Microsoft.SharePoint.WebControls.CheckBoxChoiceField))
                            {
                                CheckBoxList input = new CheckBoxList();

                                foreach (String achoice in choice.Choices)
                                {
                                    ListItem item = new ListItem(achoice, achoice, true);
                                    if (value.Contains(achoice))
                                    {
                                        item.Selected = true;
                                    }

                                    input.Items.Add(item);
                                }

                                input.ID = field.Id.ToString();
                                //input.SelectedValue = value;
                                FieldControl = input;
                            }

                            if (choice.FieldRenderingControl.GetType() == typeof(Microsoft.SharePoint.WebControls.RadioButtonChoiceField))
                            {
                                RadioButtonList input = new RadioButtonList();
                                foreach (String achoice in choice.Choices)
                                {
                                    ListItem item = new ListItem(achoice, achoice, true);
                                    input.Items.Add(item);
                                }

                                input.ID = field.Id.ToString();
                                input.SelectedValue = value;
                                FieldControl = input;
                            }

                            if (choice.FieldRenderingControl.GetType() == typeof(Microsoft.SharePoint.WebControls.DropDownChoiceField))
                            {
                                DropDownList input = new DropDownList();
                                foreach (String achoice in choice.Choices)
                                {
                                    ListItem item = new ListItem(achoice, achoice, true);
                                    input.Items.Add(item);
                                }

                                input.ID = field.Id.ToString();
                                input.SelectedValue = value;
                                FieldControl = input;
                            }
                        }

                        if (field.Type == SPFieldType.MultiChoice)
                        {
                            var choice = (SPFieldMultiChoice)field;
                            if (choice.FieldRenderingControl.GetType() == typeof(Microsoft.SharePoint.WebControls.CheckBoxChoiceField))
                            {
                                CheckBoxList input = new CheckBoxList();

                                foreach (String achoice in choice.Choices)
                                {
                                    ListItem item = new ListItem(achoice, achoice, true);
                                    if (value.Contains(achoice))
                                    {
                                        item.Selected = true;
                                    }

                                    input.Items.Add(item);
                                }

                                input.ID = field.Id.ToString();
                                input.SelectedValue = value;
                                FieldControl = input;
                            }

                            if (choice.FieldRenderingControl.GetType() == typeof(Microsoft.SharePoint.WebControls.RadioButtonChoiceField))
                            {
                                RadioButtonList input = new RadioButtonList();
                                foreach (String achoice in choice.Choices)
                                {
                                    ListItem item = new ListItem(achoice, achoice, true);
                                    input.Items.Add(item);
                                }

                                input.ID = field.Id.ToString();
                                input.SelectedValue = value;
                                FieldControl = input;
                            }

                            if (choice.FieldRenderingControl.GetType() == typeof(Microsoft.SharePoint.WebControls.DropDownChoiceField))
                            {
                                DropDownList input = new DropDownList();
                                foreach (String achoice in choice.Choices)
                                {
                                    ListItem item = new ListItem(achoice, achoice, true);
                                    input.Items.Add(item);
                                }

                                input.ID = field.Id.ToString();
                                input.SelectedValue = value;
                                FieldControl = input;
                            }
                        }

                        if (field.Type == SPFieldType.Number)
                        {
                            var number = (SPFieldNumber)field;
                            Html5.WebControls.NumberInput input = new Html5.WebControls.NumberInput();
                            input.ID = field.Id.ToString();
                            FieldControl = input;
                        }

                        if (FieldControl != null)
                        {
                            Fields.Controls.Add(FieldControl);
                        }
                    }
                }
            }
        }

        private void UpdateMetaFields()
        {
            try
            {

                GetMetaFields();

                SPContext.Current.Site.CatchAccessDeniedException = false;
                //SPWebCollection webs = SPContext.Current.Site.AllWebs;
                SPWeb web = SPContext.Current.Web;
                //foreach (SPWeb web in webs)
                //{
                    try
                    {

                        if (web.DoesUserHavePermissions((SPBasePermissions.ManageLists | SPBasePermissions.ManageWeb)))
                        {
                            web.AllowUnsafeUpdates = true;
                            web.Update();

                            GetPublishingTypes(web);

                            foreach (SPContentType content in publishingtypes)
                            {
                                foreach (SPField oField in metafields)
                                {
                                    if (!content.Fields.Contains(oField.Id))
                                    {
                                        string fieldname = SharePointWebControls.GetFieldName(oField);
                                        try
                                        {
                                            SPFieldLink fieldRef = new SPFieldLink(oField);
                                            fieldRef.DisplayName = fieldname;
                                            fieldRef.Required = false;
                                            fieldRef.ShowInDisplayForm = false;
                                            fieldRef.Hidden = true;

                                            content.FieldLinks.Add(fieldRef);
                                            content.Update();
                                        }
                                        catch { };
                                    }
                                }
                            }

                            SPListCollection lists = web.Lists;
                            //foreach (SPList list in web.Lists)
                            for (int i = 0; i < lists.Count; i++)
                            {
                                GetPublishingTypes(lists[i]);
                                foreach (SPContentType content in publishingtypes)
                                {
                                    foreach (SPField oField in metafields)
                                    {

                                        if (!content.Fields.Contains(oField.Id))
                                        {
                                            string fieldname = SharePointWebControls.GetFieldName(oField);
                                            try
                                            {
                                                SPFieldLink fieldRef = new SPFieldLink(oField);
                                                fieldRef.DisplayName = fieldname;
                                                fieldRef.Required = false;
                                                fieldRef.ShowInDisplayForm = false;
                                                fieldRef.Hidden = true;

                                                content.FieldLinks.Add(fieldRef);
                                                content.Update();
                                            }
                                            catch { };
                                        }
                                    }
                                }
                                lists[i].Update();
                            }

                            //web.AllowUnsafeUpdates = false;
                            //SPUtility.ValidateFormDigest();
                            //web.Update();
                        }
                    }
                    catch (Exception ex)
                    {
                        SPDiagnosticsService.Local.WriteTrace(0, new SPDiagnosticsCategory(ex.Source, TraceSeverity.High, EventSeverity.Error), TraceSeverity.High, ex.Message, ex.Data);
                    }
                    finally
                    {
                        SPContext.Current.Site.CatchAccessDeniedException = true;
                        web.AllowUnsafeUpdates = false;
                    }

                //}
            }
            catch (Exception ex)
            {
                SPDiagnosticsService.Local.WriteTrace(0, new SPDiagnosticsCategory(ex.Source, TraceSeverity.High, EventSeverity.Error), TraceSeverity.High, ex.Message, ex.Data);
            }
            finally
            {
                SPContext.Current.Site.CatchAccessDeniedException = true;
            }
        }

        private Authors FindAuthor(string selected)
        {
            Authors found = Authors.NoOverride;

            foreach (Authors author in Enum.GetValues(typeof(Authors)))
            {
                if (author.ToString() == selected)
                {
                    found = author;
                    break;
                }
            }
            return found;
        }

        private void GetMetaFields()
        {
            Func<SPField, bool> metagroup = null;
            if (metagroup == null)
            {
                metagroup = delegate(SPField f)
                {
                    return (f.Group == settings.GroupName);
                };
            
            }

            char[] opengraph = new char[2] { ':', '.' };
            Func<SPField, bool> extended = null;
            if (extended == null)
            {
                extended = delegate(SPField f)
                {
                    return (f.Title.Contains(opengraph.ToString()) == false);
                };
            }

            SPFieldCollection fields = SPContext.Current.Site.RootWeb.Fields;
            IEnumerable<SPField> basicFields = fields.Cast<SPField>().Where(f => f.Title.Contains("twitter:"));
            //.Where<SPField>(field => field.TypeDisplayName.Contains(""))
            metafields = basicFields.Cast<SPField>().Where<SPField>(metagroup).OrderBy(field => field.Title);
            //metafields = metafields.Except<SPField>(extended);
        }

        private void GetPublishingTypes(SPWeb web)
        {
            Func<SPContentType, bool> contentgroup = null;

            if (contentgroup == null)
            {
                contentgroup = delegate(SPContentType f)
                {
                    return f.Id.Parent == Hemrika.SharePresence.WebSite.ContentTypes.ContentTypeId.PageTemplate;
                };
            }

            SPContentTypeCollection contentypes = web.ContentTypes;
            publishingtypes = contentypes.Cast<SPContentType>().Where<SPContentType>(contentgroup);
        }

        private void GetPublishingTypes(SPList list)
        {
            Func<SPContentType, bool> contentgroup = null;

            if (contentgroup == null)
            {
                contentgroup = delegate(SPContentType f)
                {
                    return f.Id.Parent == Hemrika.SharePresence.WebSite.ContentTypes.ContentTypeId.PageTemplate;
                };
            }

            SPContentTypeCollection contentypes = list.ContentTypes;
            publishingtypes = contentypes.Cast<SPContentType>().Where<SPContentType>(contentgroup);
        }
    }
}
