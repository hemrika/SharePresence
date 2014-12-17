// -----------------------------------------------------------------------
// <copyright file="DesignWebPart.cs" company="">
// TODO: Update copyright text.
// </copyright>
// -----------------------------------------------------------------------

namespace Hemrika.SharePresence.WebSite.WebParts
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using Hemrika.SharePresence.Common.TemplateEngine;
    using System.Web;
    using System.Web.UI;
    using System.ComponentModel;
    using Microsoft.SharePoint.Utilities;
    using System.Web.UI.WebControls.WebParts;
    using System.Runtime.InteropServices;
    using Hemrika.SharePresence.Common.UI;
    using Microsoft.SharePoint.Security;
    using System.Security.Permissions;
    using Microsoft.SharePoint;
    using System.Xml.Xsl;
    using System.Xml;
    using System.Data;
    using System.IO;
    using System.Globalization;
    using Hemrika.SharePresence.WebSite.MetaData;
    using Microsoft.SharePoint.Administration;

    /// <summary>
    /// TODO: Update summary.
    /// </summary>
    [ToolboxItemAttribute(false)]
    [Guid("b21d48e2-cf0a-4174-a6c1-90be3fac9d89")]
    public class DesignWebPart : WebSitePart, IWebEditable, IWebPartMetaData
    {
        protected TemplateDefinition TemplateDef { get; set; }

        [Personalizable(PersonalizationScope.Shared), WebBrowsable(false)]
        public string DesignType { get; set; }

        [Personalizable(PersonalizationScope.Shared), WebBrowsable(false)]
        public string DesignStyle { get; set; }

        [Personalizable(PersonalizationScope.Shared), WebBrowsable(false)]
        public string XslSourceData { get; set; }

        [Personalizable(PersonalizationScope.Shared), WebBrowsable(false)]
        public string XslSourceLastModified { get; set; }

        [Personalizable(PersonalizationScope.Shared), WebBrowsable(false)]
        public string DataSourcesData { get; set; }

        private int _limit;

        [Personalizable(PersonalizationScope.Shared),
        Category("Appearance"),
        DefaultValue(5),
        WebBrowsable(true),
        WebDisplayName("Maximum number of items to return."),
        WebDescription("Limit: Maximum number of items to return.")]
        public int Limit { get { return _limit; } set { _limit = value; } }

        [DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
        [PersistenceMode(PersistenceMode.InnerProperty)]
        public List<ClientOption> ClientOptions { get; set; }

        [DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
        [PersistenceMode(PersistenceMode.InnerProperty)]
        public List<TemplateArgument> TemplateArguments { get; set; }

        private string idPrefix = "dsgn";
        private bool cached = false;
        private bool prerendered = false;
        private XsltException error = null;

        public DesignWebPart() : base()
        {
            this.ChromeState = PartChromeState.Normal;
            this.ChromeType = PartChromeType.None;

            try
            {
                cached = !HttpContext.Current.User.Identity.IsAuthenticated;
            }
            catch { };
        }

        protected override void OnInit(EventArgs e)
        {
            base.OnInit(e);
            
            LoadXsl();
            ProcessDataSources();

            if (TemplateDef == null)
            {
                try
                {
                    TemplateDef = TemplateDefinition.FromName(ClientID, DesignType, DesignStyle, true, XslSourceData, cached);
                    TemplateDef.AddTemplateArguments(TemplateArguments, true);
                    TemplateDef.AddClientOptions(ClientOptions, true);

                }
                catch (XsltException ex)
                {
                    error = ex;
                    SPDiagnosticsService.Local.WriteTrace(0, new SPDiagnosticsCategory(ex.Source, TraceSeverity.High, EventSeverity.Error), TraceSeverity.High, ex.Message, ex.Data);
                }
            }

            if (!prerendered)
            {
                if (TemplateDef != null)
                {
                    TemplateDef.PreRender(Page);
                    prerendered = true;
                }
            }

        }

        /// <summary>
        /// 
        /// </summary>
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
                                /*
                                //determine web url
                                string webUrl = t.WebUrl;
                                if (!t.WebUrl.StartsWith("/"))
                                    webUrl = "/" + webUrl;
                                */
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
                        catch (Exception ex)
                        {
                            t.CacheOnClient = false; // client caching not allowed if we cant to retreive last modified date of the source
                            sourcesChanged = true;
                            saveSources = true;
                            SPDiagnosticsService.Local.WriteTrace(0, new SPDiagnosticsCategory(ex.Source, TraceSeverity.High, EventSeverity.Error), TraceSeverity.High, ex.Message, ex.Data);
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
            catch (Exception ex)
            {
                SPDiagnosticsService.Local.WriteTrace(0, new SPDiagnosticsCategory(ex.Source, TraceSeverity.High, EventSeverity.Error), TraceSeverity.High, ex.Message, ex.Data);
                //ex.ToString();
            }          
        }

        /// <summary>
        /// 
        /// </summary>
        private void LoadXsl()
        {
            if (!string.IsNullOrEmpty(DesignType) && !string.IsNullOrEmpty(DesignStyle))
            {
                if (TemplateDef != null || HttpContext.Current.User.Identity.IsAuthenticated)
                {
                    try
                    {
                        TemplateDef = TemplateDefinition.FromName(ClientID, DesignType, DesignStyle, false, cached);

                        SPFile xslFile = null;
                        if (TemplateDef.TemplateVirtualPath != null)
                        {
                            xslFile = SPContext.Current.Web.Site.RootWeb.GetFile(TemplateDef.TemplateVirtualPath);
                        }

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
                    catch (Exception ex)
                    {
                        XslSourceData = string.Empty;
                        SPDiagnosticsService.Local.WriteTrace(0, new SPDiagnosticsCategory(ex.Source, TraceSeverity.High, EventSeverity.Error), TraceSeverity.High, ex.Message, ex.Data);
                    }
                    finally
                    {
                        TemplateDef = null;
                    }
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
            catch (Exception ex)
            {
                SPDiagnosticsService.Local.WriteTrace(0, new SPDiagnosticsCategory(ex.Source, TraceSeverity.High, EventSeverity.Error), TraceSeverity.High, ex.Message, ex.Data);
                return string.Empty;
            }
        }

        [SharePointPermission(SecurityAction.Demand, ObjectModel = true)]
        [SharePointPermission(SecurityAction.InheritanceDemand, ObjectModel = true)]
        [SharePointPermission(SecurityAction.LinkDemand, ObjectModel = true)]
        public override void PreRenderWebPart(EventArgs e)
        {
            if (TemplateDef != null && !prerendered)
            {
                TemplateDef.PreRender(Page);
                prerendered = true;
            }

            if (inEditMode)
            {
                base.PreRenderWebPart(e);
            }
        }


        [SharePointPermission(SecurityAction.Demand, ObjectModel = true)]
        [SharePointPermission(SecurityAction.InheritanceDemand, ObjectModel = true)]
        [SharePointPermission(SecurityAction.LinkDemand, ObjectModel = true)]
        public override void RenderWebPart(HtmlTextWriter writer)
        {
            writer.Write("<!-- Begin SharePresence DesignWebPart - {0} {1} -->", DesignType, DesignStyle);

            if (TemplateDef != null)
            {
                TemplateDef.AddClientOptions(new List<ClientOption> { new ClientString(DesignType + "_DesignOptions_", DesignStyle) }, false);

                string data = string.Empty;
                try
                {
                    GetListItems(out data);
                    if (!string.IsNullOrEmpty(data))
                    {
                        TemplateDef.Render(data, writer);
                    }
                    else
                    {
                        HttpApplication application = HttpContext.Current.ApplicationInstance;
                        if (application.User.Identity.IsAuthenticated)
                        {
                            writer.Write("No DataSource and or Style configured. ");

                            if (SPContext.Current.FormContext.FormMode == Microsoft.SharePoint.WebControls.SPControlMode.Edit)
                            {
                                writer.Write(String.Format(" <a onclick=\"ShowToolPane2Wrapper('Edit',this,'{0}');return false;\"> Configure </a>", StorageKey.ToString()));
                            }
                        }
                    }
                }
                catch (Exception ex)
                {
                    writer.Write(ex.Message);
                    SPDiagnosticsService.Local.WriteTrace(0, new SPDiagnosticsCategory(ex.Source, TraceSeverity.High, EventSeverity.Error), TraceSeverity.High, ex.Message, ex.Data);
                }
            }
            else
            {
                HttpApplication application = HttpContext.Current.ApplicationInstance;
                if (application.User.Identity.IsAuthenticated)
                {
                    if (error != null)
                    {
                        writer.Write(error.Message +":"+ error.InnerException.Message);
                    }
                    else
                    {
                        writer.Write("No DataSource and or Style configured. ");

                        if (SPContext.Current.FormContext.FormMode == Microsoft.SharePoint.WebControls.SPControlMode.Edit)
                        {
                            writer.Write(String.Format(" <a onclick=\"ShowToolPane2Wrapper('Edit',this,'{0}');return false;\"> Configure </a>", StorageKey.ToString()));
                        }
                    }
                }
            }

            writer.Write("<!-- End SharePresence DesignWebPart - {0} {1} -->", DesignType, DesignStyle);

            if (inEditMode)
            {
                base.RenderWebPart(writer);
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="xml"></param>
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

                bool first = true;

                foreach (DesignDataSource source in sources)
                {
                    SPWeb web = site.OpenWeb(new Guid(source.WebId));
                    SPList list = web.Lists[new Guid(source.ListId)];
                    SPView view = list.GetView(new Guid(source.ViewId));
                    view.RowLimit = (Limit > 0) ? (uint)Limit : (uint)5;
                    SPQuery query = new SPQuery(view);
                    query.ViewFieldsOnly = true;
                    string sxml = list.GetItems(query).Xml;

                    if (first)
                    {
                        doc.LoadXml(sxml);

                        XmlNodeList rows = doc.SelectNodes("//z:row", NameSpaceManager(doc.NameTable));
                        foreach (XmlNode row in rows)
                        {
                            string[] fileref = row.Attributes["ows_FileRef"].Value.Split(new char[1] { '#' });
                            if (fileref != null)
                            {
                                string absoluteUrl = web.Url + "/" + fileref[1];
                                row.Attributes["ows_FileRef"].Value = absoluteUrl;
                            }
                        }
                        first = false;
                    }
                    else
                    {
                        XmlDocument _doc = new XmlDocument();
                        _doc.LoadXml(sxml);

                        XmlNode element = doc.SelectSingleNode("//s:ElementType", NameSpaceManager(doc.NameTable));
                        XmlNodeList schema = _doc.SelectNodes("//s:AttributeType", NameSpaceManager(_doc.NameTable));

                        XmlNode data = doc.SelectSingleNode("//rs:data", NameSpaceManager(doc.NameTable));
                        XmlNodeList rows = _doc.SelectNodes("//z:row", NameSpaceManager(_doc.NameTable));

                        foreach (XmlNode node in schema)
                        {
                            try
                            {
                                XmlNode importXml = doc.ImportNode(node, true);
                                element.AppendChild(importXml);
                            }
                            catch (Exception ex)
                            {
                                SPDiagnosticsService.Local.WriteTrace(0, new SPDiagnosticsCategory(ex.Source, TraceSeverity.High, EventSeverity.Error), TraceSeverity.High, ex.Message, ex.Data);
                            }
                        }

                        int count = int.Parse(data.Attributes["ItemCount"].Value);
                        foreach (XmlNode row in rows)
                        {
                            try
                            {
                                string[] fileref = row.Attributes["ows_FileRef"].Value.Split(new char[1] { '#' });
                                if (fileref != null)
                                {
                                    string absoluteUrl = web.Url + "/" + fileref[1];
                                    row.Attributes["ows_FileRef"].Value = absoluteUrl;
                                }

                                XmlNode importXml = doc.ImportNode(row, true);
                                data.AppendChild(importXml);
                                count++;
                            }
                            catch (Exception ex)
                            {
                                SPDiagnosticsService.Local.WriteTrace(0, new SPDiagnosticsCategory(ex.Source, TraceSeverity.High, EventSeverity.Error), TraceSeverity.High, ex.Message, ex.Data);
                            }
                        }
                        data.Attributes["ItemCount"].Value = count.ToString();

                        doc.ImportNode(element, true);
                        doc.ImportNode(data, true);
                    }
                }
            });
            xml = doc.InnerXml;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
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
            mgr.AddNamespace("s","uuid:BDC6E3F0-6DA3-11d1-A2A3-00AA00C14882");
            mgr.AddNamespace("dt","uuid:C2F41010-65B3-11d1-A29F-00AA00C14882");
            mgr.AddNamespace("rs","urn:schemas-microsoft-com:rowset");
            mgr.AddNamespace("z", "#RowsetSchema");
            return mgr;
        }

        /// <summary>
        /// Gets the ClientID of this control. 
        /// </summary>
        public override string ClientID
        {
            [Microsoft.SharePoint.Security.SharePointPermission(System.Security.Permissions.SecurityAction.Demand, ObjectModel = true)]
            get
            {
                if (this.idPrefix == null)
                {
                    this.idPrefix = SPUtility.GetNewIdPrefix(this.Context);
                }

                return SPUtility.GetShortId(this.idPrefix, this);
            }
        }

        /// <summary>
        /// 
        /// </summary>
        public override string ID
        {
            get
            {
                if (base.ID == null)
                {
                    return SPUtility.GetNewIdPrefix(this.Context)+"designwebpart";
                }

                return base.ID;
            }
            set
            {
                base.ID = value;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        EditorPartCollection IWebEditable.CreateEditorParts()
        {

            List<EditorPart> editors = new List<EditorPart>();

            EditorPartCollection baseParts = base.CreateEditorParts();

            foreach (EditorPart basePart in baseParts)
            {
                editors.Add(basePart);
            }

            DesignXslEditorPart design = new DesignXslEditorPart
            {
                ID = ID + "_StyleEditor",
                Title = "Style"
            };
            editors.Add(design);

            DesignDataEditorPart data = new DesignDataEditorPart
            {
                ID = ID + "_DesignDataEditor",
                Title = "Data"
            };
            editors.Add(data);

            return new EditorPartCollection(editors);

        }

        /// <summary>
        /// 
        /// </summary>
        object IWebEditable.WebBrowsableObject
        {

            get { return this; }

        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public override Common.Ribbon.Definitions.ContextualGroupDefinition GetContextualGroupDefinition()
        {
            return null;
        }

        #region transform XML

        /// <summary>
        /// 
        /// </summary>
        private static readonly string xsltFromZRowToXml =
                "<xsl:stylesheet version=\"1.0\" " +
                 "xmlns:xsl=\"http://www.w3.org/1999/XSL/Transform\" " +
                 "xmlns:s=\"uuid:BDC6E3F0-6DA3-11d1-A2A3-00AA00C14882\" " +
                 "xmlns:msxsl=\"urn:schemas-microsoft-com:xslt\" " +
            //"xmlns:docs=\"http://example.com/2010/docs\" "+
            //"xmlns:fo=\"http://www.w3.org/1999/XSL/Format\" "+
            //"xmlns:url=\"http://a.unique.identifier/url-methods\" exclude-result-prefixes=\"msxsl\" "+
                 "xmlns:z=\"#RowsetSchema\">" +
             "<s:Schema id=\"RowsetSchema\"/>" +
             "<xsl:output method=\"html\" omit-xml-declaration=\"yes\" indent=\"yes\" />" +
             "<xsl:template match=\"/\">" +
              "<xsl:text disable-output-escaping=\"yes\">&lt;rows&gt;</xsl:text>" +
              "<xsl:apply-templates select=\"//z:row\"/>" +
              "<xsl:text disable-output-escaping=\"yes\">&lt;/rows&gt;</xsl:text>" +
             "</xsl:template>" +
             "<xsl:template match=\"z:row\">" +
              "<xsl:text disable-output-escaping=\"yes\">&lt;row&gt;</xsl:text>" +
              "<xsl:apply-templates select=\"@*\"/>" +
              "<xsl:text disable-output-escaping=\"yes\">&lt;/row&gt;</xsl:text>" +
             "</xsl:template>" +
             "<xsl:template match=\"@*\">" +
              "<xsl:text disable-output-escaping=\"yes\">&lt;</xsl:text>" +
                "<xsl:choose>" +
                    "<xsl:when test=\"starts-with(name(),'ows_')\">" +
                        "<xsl:value-of select=\"substring-after(name(), 'ows_')\"/>" +
                    "</xsl:when>" +
                    "<xsl:otherwise>" +
                        "<xsl:value-of select=\"name()\"/>" +
                    "</xsl:otherwise>" +
                "</xsl:choose>" +
              "<xsl:text disable-output-escaping=\"yes\">&gt;</xsl:text>" +
            //"<xsl:for-each select=\"text()\">"+
            //"<xsl:value-of select=\"url:encode(string(.))\" />" +
            //"</xsl:for-each>"+
              "<xsl:value-of select=\".\" disable-output-escaping=\"yes\" />" +

              "<xsl:text disable-output-escaping=\"yes\">&lt;/</xsl:text>" +
                "<xsl:choose>" +
                    "<xsl:when test=\"starts-with(name(),'ows_')\">" +
                        "<xsl:value-of select=\"substring-after(name(), 'ows_')\"/>" +
                    "</xsl:when>" +
                    "<xsl:otherwise>" +
                        "<xsl:value-of select=\"name()\"/>" +
                    "</xsl:otherwise>" +
                "</xsl:choose>" +
              "<xsl:text disable-output-escaping=\"yes\">&gt;</xsl:text>" +
             "</xsl:template>" +
            /*
            "<msxsl:script language=\"JScript\" implements-prefix=\"url\"> "+
               "function encode(string) "+
                   "{"+
                       "return encodeURIComponent(string); "+
                   "}"+
           "</msxsl:script>" +
           */
            "</xsl:stylesheet>";

        /// <summary>
        /// 
        /// </summary>
        /// <param name="itemCollection"></param>
        /// <returns></returns>
        private DataTable ConvertToTable(SPListItemCollection itemCollection)
        {
            DataSet ds = new DataSet();

            string xmlData = ConvertZRowToRegularXml(itemCollection.Xml);
            if (string.IsNullOrEmpty(xmlData))
                return null;

            using (System.IO.StringReader sr = new System.IO.StringReader(xmlData))
            {
                ds.ReadXml(sr, XmlReadMode.Auto);

                if (ds.Tables.Count == 0)
                    return null;

                return ds.Tables[0];
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="zRowData"></param>
        /// <returns></returns>
        private string ConvertZRowToRegularXml(string zRowData)
        {
            XsltSettings settings = new XsltSettings(true, true);
            XslCompiledTransform transform = new XslCompiledTransform();
            XmlDocument tidyXsl = new XmlDocument();

            try
            {
                //Transformer 
                tidyXsl.LoadXml(xsltFromZRowToXml);
                transform.Load(tidyXsl, settings, new XmlUrlResolver());

                //output (result) writers 
                using (System.IO.StringWriter sw = new System.IO.StringWriter())
                {
                    XmlTextWriter tw = new XmlTextWriter(sw);
                    System.IO.StringReader srZRow = new System.IO.StringReader(zRowData);
                    XmlTextReader xtrZRow = new XmlTextReader(srZRow);
                    //Transform 
                    transform.Transform(xtrZRow, null, tw);
                    return sw.ToString();
                }
            }
            catch (Exception ex)
            {
                SPDiagnosticsService.Local.WriteTrace(0, new SPDiagnosticsCategory(ex.Source, TraceSeverity.High, EventSeverity.Error), TraceSeverity.High, ex.Message, ex.Data);
                return null;
            }
        }
        #endregion

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public string MetaData()
        {
            return "";
        }
    }
}
