// -----------------------------------------------------------------------
// <copyright file="FormWebPart.cs" company="">
// TODO: Update copyright text.
// </copyright>
// -----------------------------------------------------------------------

namespace Hemrika.SharePresence.WebSite.WebParts
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.ComponentModel;
    using System.Runtime.InteropServices;
    using System.Web.UI.WebControls.WebParts;
    using Hemrika.SharePresence.Common.TemplateEngine;
    using Microsoft.SharePoint;
    using Microsoft.SharePoint.Security;
    using System.Security.Permissions;
    using System.Web;
    using System.Xml;
    using Microsoft.SharePoint.Utilities;
    using System.Web.UI;
    using System.Globalization;
    using System.IO;
    using System.Xml.Xsl;
    using Microsoft.SharePoint.WebPartPages;
    using Microsoft.SharePoint.WebControls;
    using System.Web.UI.WebControls;
    using Hemrika.SharePresence.WebSite.MetaData;

    [ToolboxItemAttribute(false)]
    [Guid("6d648c5a-d9d6-41c0-882c-c1b0095dfa8e")]
    public class FormWebPart : WebSitePart, IWebEditable, IWebPartMetaData
    {
        public static TemplateDefinition TemplateDef { get; set; }
        protected UpdatePanel formpanel;
        protected UpdateProgress formprogess;
        protected FormContentTemplate contenttemplate;

        [WebBrowsable(false)]
        [WebPartStorage(Storage.Shared)]
        [Personalizable(PersonalizationScope.Shared)]
        public string FormType { get; set; }

        [Personalizable(PersonalizationScope.Shared)]
        [WebBrowsable(false)]
        [WebPartStorage(Storage.Shared)]
        public string FormStyle { get; set; }

        [Personalizable(PersonalizationScope.Shared), WebBrowsable(false)]
        [WebPartStorage(Storage.Shared)]
        public string XslSourceData { get; set; }

        [Personalizable(PersonalizationScope.Shared), WebBrowsable(false)]
        [WebPartStorage(Storage.Shared)]
        public string XslSourceLastModified { get; set; }

        [Personalizable(PersonalizationScope.Shared), WebBrowsable(false)]
        [WebPartStorage(Storage.Shared)]
        public string DataSourcesData { get; set; }

        [DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
        [PersistenceMode(PersistenceMode.InnerProperty)]
        [WebPartStorage(Storage.Shared)]
        public List<ClientOption> ClientOptions { get; set; }

        [DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
        [PersistenceMode(PersistenceMode.InnerProperty)]
        [WebPartStorage(Storage.Shared)]
        public List<TemplateArgument> TemplateArguments { get; set; }

        [WebPartStorage(Storage.Shared), WebBrowsable(false), Personalizable(PersonalizationScope.Shared)]
        public string FormTitle { get; set; }

        [WebPartStorage(Storage.Shared), WebBrowsable(false), Personalizable(PersonalizationScope.Shared)]
        public string FormDesciption { get; set; }

        private string idPrefix;
        private bool cached = true;

        public FormWebPart()
            : base()
        {
            this.ChromeState = PartChromeState.Normal;
            this.ChromeType = PartChromeType.None;

        }

        protected override void OnInit(EventArgs e)
        {
            LoadXsl();
            ProcessDataSources();

            formpanel = new UpdatePanel { ID = ID + "_UpdateFormPanel", RenderMode = UpdatePanelRenderMode.Block };//, EnableViewState = true };

            contenttemplate = new FormContentTemplate();
            contenttemplate.WebPart = this;            
            formpanel.ContentTemplate = contenttemplate;
            //Add on Page
            this.Controls.Add(formpanel);

            //Add Field Controls
            SetListFormFields(contenttemplate.canvas);

            formprogess = new UpdateProgress { AssociatedUpdatePanelID = formpanel.ID };
            formprogess.ProgressTemplate = new FormProgessTemplate();
            this.Controls.Add(formprogess);

            //if (TemplateDef == null)
            //{
                try
                {

                    TemplateDef = TemplateDefinition.FromName(contenttemplate.canvas.ClientID, FormType, FormStyle, true, XslSourceData, cached);
                    TemplateDef.AddTemplateArguments(TemplateArguments, true);
                    TemplateDef.AddClientOptions(ClientOptions, true);
                }
                catch (Exception)
                {
                }
            //}

            base.OnInit(e);
        }

        protected override void EnsureChildControls()
        {
            try
            {
                //base.EnsureChildControls();
                //this.Controls.Add(this.menuControls);
            }
            catch (Exception ex)
            {
                ex.ToString();
            }
        }

        private void ProcessDataSources()
        {
            try
            {

                //put modified date to each data source:
                if (!string.IsNullOrEmpty(DataSourcesData))
                {
                    List<DesignDataSource> sources = Utilities.DeserializeObject<List<DesignDataSource>>(DataSourcesData);
                    bool sourcesChanged = false;
                    bool saveSources = false;
                    foreach (DesignDataSource t in sources)
                    {
                        var listId = new Guid(t.ListId);
                        try
                        {
                            string currentModifiedDate;
                            if (string.IsNullOrEmpty(t.WebId))
                            {
                                currentModifiedDate = GetListModifiedDate(SPContext.Current.Web, listId).ToString(CultureInfo.InvariantCulture);
                            }
                            else
                            {
                                using (var site = new SPSite(SPContext.Current.Web.Url))
                                {
                                    using (SPWeb web = site.OpenWeb(new Guid(t.WebId)))//.OpenWeb())
                                    {
                                        currentModifiedDate = GetListModifiedDate(web, listId).ToString(CultureInfo.InvariantCulture);
                                    }
                                }
                            }

                            if (string.IsNullOrEmpty(t.Modified) || currentModifiedDate != t.Modified)
                            {
                                t.Modified = currentModifiedDate;
                                sourcesChanged = true;
                            }
                            if (!t.CacheOnClient.HasValue)
                            {
                                t.CacheOnClient = true; //default value for older data source (previous versions)
                                sourcesChanged = true;
                                saveSources = true;
                            }
                        }
                        catch (Exception)
                        {
                            t.CacheOnClient = false; // clent caching not allowed if we cant to retreive last modified date of the source
                            sourcesChanged = true;
                            saveSources = true;
                        }
                    }

                    if (sourcesChanged)
                    {
                        DataSourcesData = Utilities.SerializeObject(sources); //save modified data sources if it changes
                        if (saveSources)
                            SetPersonalizationDirty();
                        cached = false;
                    }
                }
            }
            catch (Exception)
            {
            }

        }

        private void LoadXsl()
        {
            if (!string.IsNullOrEmpty(FormType) && !string.IsNullOrEmpty(FormStyle) && TemplateDef != null)
            {
                try
                {
                    TemplateDef = TemplateDefinition.FromName(ClientID, FormType, FormStyle, false);

                    if (TemplateDef.TemplateVirtualPath != null)
                    {
                        SPFile xslFile = SPContext.Current.Web.Site.RootWeb.GetFile(TemplateDef.TemplateVirtualPath);
                        if (xslFile != null && xslFile.Exists)
                        {
                            //check cache:
                            string lastModifiedStamp = xslFile.TimeLastModified.ToString(CultureInfo.InvariantCulture);
                            if (XslSourceLastModified != lastModifiedStamp)
                            {
                                XslSourceLastModified = lastModifiedStamp;
                                XslSourceData = GetFileData(xslFile).Replace(Environment.NewLine, "").Replace("\n", "");
                                cached = false;
                                SetPersonalizationDirty();
                            }
                        }
                    }
                }
                catch (Exception)
                {
                    XslSourceData = string.Empty;
                }
            }
        }

        private DateTime GetListModifiedDate(SPWeb web, Guid listId)
        {
            return web.Lists[listId].LastItemModifiedDate;
        }

        private string GetFileData(SPFile spFile)
        {
            if (spFile == null)
                return string.Empty;
            try
            {
                var encoding = new UTF8Encoding(false);
                byte[] bytes = spFile.OpenBinary();
                return encoding.GetString(bytes);
            }
            catch
            {
                return string.Empty;
            }
        }

        [SharePointPermission(SecurityAction.Demand, ObjectModel = true)]
        [SharePointPermission(SecurityAction.InheritanceDemand, ObjectModel = true)]
        [SharePointPermission(SecurityAction.LinkDemand, ObjectModel = true)]
        public override void PreRenderWebPart(EventArgs e)
        {
            if (TemplateDef != null)
            {
                TemplateDef.PreRender(Page);
            }

            /*
            if (inEditMode)
            {
                base.PreRenderWebPart(e);
            }
            */
        }

        [SharePointPermission(SecurityAction.Demand, ObjectModel = true)]
        [SharePointPermission(SecurityAction.InheritanceDemand, ObjectModel = true)]
        [SharePointPermission(SecurityAction.LinkDemand, ObjectModel = true)]
        public override void RenderWebPart(HtmlTextWriter writer)
        {
            string output = string.Empty;
            StringBuilder builder = new StringBuilder();
            using (var baseWriter = new HtmlTextWriter(new StringWriter(builder, CultureInfo.InvariantCulture)))
            {
                bool configure = false;
                baseWriter.Write("<!-- Begin SharePresence FormWebPart - {0} {1} -->", FormType, FormStyle);

                if (TemplateDef != null)
                {
                    List<TemplateArgument> args = new List<TemplateArgument>();
                    //var args = new XsltArgumentList();
                    if (!string.IsNullOrEmpty(FormTitle))
                    {
                        args.Add(new TemplateArgument("FormName", FormTitle));
                    }

                    if (!string.IsNullOrEmpty(FormDesciption))
                    {
                        args.Add(new TemplateArgument("FormDescription", FormDesciption));
                        //args.AddParam("FormDescription", "", FormDesciption);
                    }
                    TemplateDef.AddTemplateArguments(args, false);

                    //TemplateDef.TemplateArguments.ForEach(a => args.AddParam(a.Name, "", a.Value));

                    TemplateDef.AddClientOptions(new List<ClientOption> { new ClientString(FormType + "_FormOptions_", FormStyle) }, false);

                    string data = string.Empty;
                    try
                    {
                        GetListItems(out data);
                        if (!string.IsNullOrEmpty(data))
                        {
                            StringBuilder templatebuilder = new StringBuilder();
                            using (var templateWriter = new HtmlTextWriter(new StringWriter(templatebuilder, CultureInfo.InvariantCulture)))
                            {

                                TemplateDef.Render(data, templateWriter);

                            }

                            //Add Template for Canvas Render
                            contenttemplate.innerHtml = templatebuilder.ToString();

                            //formpanel.Triggers.Add(contenttemplate.canvas as UpdatePanelTrigger);
                            formpanel.RenderControl(baseWriter);

                        }
                        else
                        {
                            HttpApplication application = HttpContext.Current.ApplicationInstance;
                            if (application.User.Identity.IsAuthenticated)
                            {
                                baseWriter.Write("No DataSource and or Style configured. ");

                                if (SPContext.Current.FormContext.FormMode == Microsoft.SharePoint.WebControls.SPControlMode.Edit)
                                {
                                    baseWriter.Write(String.Format(" <a onclick=\"ShowToolPane2Wrapper('Edit',this,'{0}');return false;\" style=\"cursor: hand;\" >Configure</a>", StorageKey.ToString()));
                                    configure = true;
                                }
                            }
                        }
                    }
                    catch (Exception ex)
                    {
                        baseWriter.Write(ex.Message);

                        if (SPContext.Current.FormContext.FormMode == Microsoft.SharePoint.WebControls.SPControlMode.Edit)
                        {
                            baseWriter.Write(String.Format(" <a onclick=\"ShowToolPane2Wrapper('Edit',this,'{0}');return false;\" style=\"cursor: hand;\" >Configure</a>", StorageKey.ToString()));
                            configure = true;
                        }
                    }
                }
                else
                {
                    HttpApplication application = HttpContext.Current.ApplicationInstance;
                    if (application.User.Identity.IsAuthenticated)
                    {
                        baseWriter.Write("No DataSource and or Style configured. ");

                        if (SPContext.Current.FormContext.FormMode == Microsoft.SharePoint.WebControls.SPControlMode.Edit)
                        {
                            baseWriter.Write(String.Format(" <a onclick=\"ShowToolPane2Wrapper('Edit',this,'{0}');return false;\">Configure</a>", StorageKey.ToString()));
                            configure = true;
                        }
                    }
                }

                baseWriter.Write("<!-- End SharePresence FormWebPart - {0} {1} -->", FormType, FormStyle);

                if (inEditMode)
                {
                    if (!configure)
                    {
                        if (SPContext.Current.FormContext.FormMode == Microsoft.SharePoint.WebControls.SPControlMode.Edit)
                        {
                            baseWriter.Write(String.Format(" <a onclick=\"ShowToolPane2Wrapper('Edit',this,'{0}');return false;\">Configure</a>", StorageKey.ToString()));
                        }
                    }

                    menuControls.RenderControl(baseWriter);
                  //base.RenderWebPart(baseWriter);
                }

                output = builder.ToString();
            }

            writer.Write(output);
        }

        private void SetListFormFields(Control canvas)
        {
            List<DesignDataSource> sources = new List<DesignDataSource>();
            if (!string.IsNullOrEmpty(DataSourcesData))
            {
                sources = Utilities.DeserializeObject<List<DesignDataSource>>(DataSourcesData);
            }

            if (inEditMode)
            {

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
                        if (list.Fields.ContainsField(viewfield.ToString()))
                        {
                            try
                            {
                                string fieldname = viewfield.ToString().Replace("_x0020_", " ");
                                SPField field = list.Fields.GetField(fieldname);
                                FormField fld = new FormField();

                                if (inEditMode && Page.IsPostBack)
                                //if (DesignMode)// && Page.IsPostBack)
                                {
                                    fld.ControlMode = SPControlMode.Display;
                                }
                                else
                                {
                                    fld.ControlMode = SPControlMode.New;
                                }

                                //fld.ControlMode = SPControlMode.New;

                                fld.ListId = list.ID;
                                fld.FieldName = field.InternalName;
                                fld.ID = field.InternalName;
                                fld.DisableInputFieldLabel = true;

                                fld.InDesign = inEditMode;
                                fld.IsValid = inEditMode;

                                try
                                {
                                    if (!string.IsNullOrEmpty(field.DefaultValue))
                                    {
                                        if (field.FieldValueType == typeof(Boolean))
                                        {
                                            bool isbool = false;
                                            fld.Value = (Boolean.TryParse(field.DefaultValue, out isbool));
                                            fld.InputFieldLabel = string.Empty;
                                        }
                                        else
                                        {
                                            fld.Value = field.DefaultValue;
                                        }
                                    }
                                }
                                catch (Exception)
                                {
                                }

                                canvas.Controls.Add(fld);

                                if (field.Required)
                                {
                                    InputFormRequiredFieldValidator required = new InputFormRequiredFieldValidator();
                                    required.ID = "required_" + fld.ClientID;
                                    required.ControlToValidate = fld.ClientID;
                                    required.ValidationGroup = fld.ClientID;
                                    required.ErrorMessage = field.ValidationMessage;
                                    //required.EnableClientScript = true;
                                    required.BreakAfter = true;
                                    required.BreakBefore = false;
                                    required.Display = System.Web.UI.WebControls.ValidatorDisplay.Dynamic;
                                    canvas.Controls.Add(required);
                                }

                                if (field.Type == SPFieldType.Number)
                                {
                                    SPFieldNumber Number = (SPFieldNumber)field;
                                    if (Number.MinimumValue != Double.MinValue && Number.MaximumValue != Double.MaxValue)
                                    {
                                        InputFormRangeValidator range = new InputFormRangeValidator();
                                        range.ID = "range_" + fld.ClientID;
                                        range.ControlToValidate = fld.ClientID;
                                        range.ValidationGroup = fld.ClientID;
                                        range.ErrorMessage = field.ValidationMessage;
                                        //range.EnableClientScript = true;
                                        range.BreakAfter = true;
                                        range.BreakBefore = false;
                                        range.Display = System.Web.UI.WebControls.ValidatorDisplay.Dynamic;
                                        range.MaximumValue = Number.MaximumValue.ToString();
                                        range.MinimumValue = Number.MinimumValue.ToString();
                                        range.Type = System.Web.UI.WebControls.ValidationDataType.Double;
                                        canvas.Controls.Add(range);
                                    }
                                }

                                if (field.Type == SPFieldType.Currency)
                                {
                                    SPFieldCurrency Currency = (SPFieldCurrency)field;
                                    if (Currency.MaximumValue != Double.MaxValue && Currency.MinimumValue != Double.MinValue)
                                    {
                                        InputFormRangeValidator range = new InputFormRangeValidator();
                                        range.ID = "range_" + fld.ClientID;
                                        range.ControlToValidate = fld.ClientID;
                                        range.ValidationGroup = fld.ClientID;
                                        range.ErrorMessage = field.ValidationMessage;
                                        //range.EnableClientScript = true;
                                        range.BreakAfter = true;
                                        range.BreakBefore = false;
                                        range.Display = System.Web.UI.WebControls.ValidatorDisplay.Dynamic;
                                        range.MaximumValue = Currency.MaximumValue.ToString();
                                        range.MinimumValue = Currency.MinimumValue.ToString();
                                        range.Type = System.Web.UI.WebControls.ValidationDataType.Currency;
                                        canvas.Controls.Add(range);
                                    }
                                }

                                if (!string.IsNullOrEmpty(field.ValidationFormula))
                                {
                                    InputFormRegularExpressionValidator regex = new InputFormRegularExpressionValidator();
                                    regex.ControlToValidate = fld.ClientID;
                                    regex.ValidationGroup = fld.ClientID;
                                    regex.ErrorMessage = fld.ErrorMessage;
                                    regex.ErrorMessage = field.ValidationMessage;
                                    //regex.EnableClientScript = true;
                                    regex.BreakAfter = true;
                                    regex.BreakBefore = false;
                                    regex.Display = System.Web.UI.WebControls.ValidatorDisplay.Dynamic;
                                    regex.ValidationExpression = field.ValidationFormula;
                                    canvas.Controls.Add(regex);
                                }

                                /*
                                if (field.Type == SPFieldType.MultiChoice)
                                {
                                    InputFormCheckBoxListValidator checkboxes = new InputFormCheckBoxListValidator();
                                    //SPFieldMultiChoice choice = (SPFieldMultiChoice)field;
                                    checkboxes.ControlToValidate = fld.ClientID;
                                    checkboxes.ValidationGroup = fld.ClientID;
                                    checkboxes.ErrorMessage = fld.ErrorMessage;
                                    checkboxes.EnableClientScript = true;
                                    //checkboxes.BreakAfter = true;
                                    checkboxes.Display = System.Web.UI.WebControls.ValidatorDisplay.Dynamic;
                                    canvas.Controls.Add(checkboxes);

                                }
                                */
                            }
                            catch (Exception ex)
                            {
                                ex.Message.ToString();
                            }
                        }
                    }
                }
            });
        }

        private void GetListItems(out string xml)
        {
            xml = string.Empty;
            SPSite site = SPContext.Current.Site;
            XmlDocument doc = new XmlDocument();
            List<DesignDataSource> sources = new List<DesignDataSource>();
            if (!string.IsNullOrEmpty(DataSourcesData))
            {
                sources = Utilities.DeserializeObject<List<DesignDataSource>>(DataSourcesData);
            }

            SPSecurity.RunWithElevatedPrivileges(delegate()
            {

                //bool first = true;

                foreach (DesignDataSource source in sources)
                {
                    SPWeb web = site.OpenWeb(new Guid(source.WebId));
                    SPList list = web.Lists[new Guid(source.ListId)];
                    SPView view = list.GetView(new Guid(source.ViewId));
                    //view.RowLimit = 0;
                    //SPQuery query = new SPQuery(view);
                    //query.ViewFieldsOnly = true;

                    //string sxml = list.GetItems(query).Xml;
                    SPViewFieldCollection viewfields = view.ViewFields;
                    List<string> used = new List<string>();

                    foreach (var viewfield in viewfields)
                    {
                        used.Add(viewfield.ToString());
                    }

                    string dataschema = list.SchemaXml;

                    //Cleanup document
                    doc.LoadXml(dataschema);
                    XmlNode listnode = doc.SelectSingleNode("//List", NameSpaceManager(doc.NameTable));
                    XmlNode targetfields = listnode.SelectSingleNode("//Fields", NameSpaceManager(doc.NameTable));
                    targetfields.RemoveAll();
                    XmlNode RegionalSettings = doc.SelectSingleNode("//RegionalSettings", NameSpaceManager(doc.NameTable));
                    listnode.RemoveChild(RegionalSettings);
                    XmlNode ServerSettings = doc.SelectSingleNode("//ServerSettings", NameSpaceManager(doc.NameTable));
                    listnode.RemoveChild(ServerSettings);

                    //Get All Node again
                    XmlDocument schemadoc = new XmlDocument();
                    schemadoc.LoadXml(dataschema);

                    listnode = schemadoc.SelectSingleNode("//List", NameSpaceManager(schemadoc.NameTable));
                    XmlNode fields = listnode.SelectSingleNode("//Fields", NameSpaceManager(schemadoc.NameTable));
                    XmlNodeList fieldnodes = fields.SelectNodes("//Field", NameSpaceManager(schemadoc.NameTable));

                    foreach (XmlNode field in fieldnodes)
                    {
                        XmlNode namenode = field.Attributes["Name"];
                        XmlNode displaynamenode = field.Attributes["DisplayName"];
                        XmlNode staticnamenode = field.Attributes["StaticName"];
                        XmlNode hiddennode = field.Attributes["Hidden"];

                        string name = string.Empty;
                        if (namenode != null)
                        {
                            name = namenode.Value;
                        }

                        string staticname = string.Empty;
                        if (staticnamenode != null)
                        {
                            staticname = staticnamenode.Value;
                        }

                        string displayname = string.Empty;
                        if (displaynamenode != null)
                        {
                            displayname = displaynamenode.Value;
                        }

                        bool hidden = false;
                        if (hiddennode != null)
                        {
                            hidden = Boolean.Parse(hiddennode.Value);
                        }

                        if (used.Contains(name) || used.Contains(displayname) || used.Contains(staticname))
                        {
                            try
                            {
                                if (field.ParentNode == fields)
                                {
                                    XmlNode importNode = doc.ImportNode(field, true);
                                    targetfields.AppendChild(importNode);

                                    if (importNode.ParentNode == targetfields)
                                    {
                                        if (hiddennode == null)
                                        {
                                            XmlAttribute hiddenAttribute = importNode.OwnerDocument.CreateAttribute("Hidden");
                                            hiddenAttribute.Value = Boolean.FalseString;
                                            importNode.Attributes.Append(hiddenAttribute);
                                        }

                                        XmlNode requirednode = importNode.Attributes["Required"];

                                        if (requirednode == null && importNode.ParentNode == targetfields)
                                        {
                                            XmlAttribute requiredAttribute = importNode.OwnerDocument.CreateAttribute("Required");
                                            requiredAttribute.Value = Boolean.FalseString;
                                            importNode.Attributes.Append(requiredAttribute);
                                        }

                                        XmlNode groupnode = importNode.Attributes["Group"];

                                        if (groupnode == null && importNode.ParentNode == targetfields)
                                        {
                                            XmlAttribute groupAttribute = importNode.OwnerDocument.CreateAttribute("Group");
                                            groupAttribute.Value = string.Empty;
                                            importNode.Attributes.Append(groupAttribute);
                                        }
                                    }
                                }
                            }
                            catch (Exception ex)
                            {
                                ex.ToString();
                            }
                        }       
                    }
                }
            });
            xml = doc.InnerXml;
        }

        internal static XmlNamespaceManager NameSpaceManager(XmlNameTable name)
        {
            XmlNamespaceManager mgr = new XmlNamespaceManager(name);
            mgr.AddNamespace("ddwrt", "http://schemas.microsoft.com/WebParts/v2/DataView/runtime");
            mgr.AddNamespace("xsl", "http://www.w3.org/1999/XSL/Transform");
            mgr.AddNamespace("asp", "http://schemas.microsoft.com/ASPNET/20");
            mgr.AddNamespace("msxsl", "urn:schemas-microsoft-com:xslt");
            mgr.AddNamespace("SharePoint", "Microsoft.SharePoint.WebControls");
            mgr.AddNamespace("d", "http://schemas.microsoft.com/sharepoint/dsp");
            mgr.AddNamespace("exclude-result-prefixes", "xsl msxsl ddwrt");
            mgr.AddNamespace("designer", "http://schemas.microsoft.com/WebParts/v2/DataView/designer");
            mgr.AddNamespace("ddwrt2", "urn:frontpage:internal");
            mgr.AddNamespace("s", "uuid:BDC6E3F0-6DA3-11d1-A2A3-00AA00C14882");
            mgr.AddNamespace("dt", "uuid:C2F41010-65B3-11d1-A29F-00AA00C14882");
            mgr.AddNamespace("rs", "urn:schemas-microsoft-com:rowset");
            mgr.AddNamespace("z", "#RowsetSchema");
            return mgr;
        }

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
                    return SPUtility.GetNewIdPrefix(this.Context) + "_formwebpart";
                }

                return base.ID;
            }
            set
            {
                base.ID = value;
            }
        }

        EditorPartCollection IWebEditable.CreateEditorParts()
        {

            List<EditorPart> editors = new List<EditorPart>();

            EditorPartCollection baseParts = base.CreateEditorParts();

            foreach (EditorPart basePart in baseParts)
            {
                editors.Add(basePart);
            }

            FormXslEditorPart form = new FormXslEditorPart
            {
                ID = ID + "_StyleEditor",
                Title = "Style"
            };
            editors.Add(form);

            FormDataEditorPart data = new FormDataEditorPart
            {
                ID = ID + "_FormDataEditor",
                Title = "Data"
            };
            editors.Add(data);

            return new EditorPartCollection(editors);

        }

        object IWebEditable.WebBrowsableObject
        {

            get { return this; }

        }

        public override Common.Ribbon.Definitions.ContextualGroupDefinition GetContextualGroupDefinition()
        {
            return null;
        }

        public string MetaData()
        {
            return "";
        }
    }
}