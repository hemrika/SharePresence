using Hemrika.SharePresence.WebSite.WebServices.PublishingService;

namespace Hemrika.SharePresence.WebSite
{
    using System;
    using System.Web.Services;
    using Microsoft.SharePoint;
    using Microsoft.SharePoint.Security;
    using System.Security.Permissions;
    using System.ComponentModel;
    using System.Collections;
    using System.Globalization;
    using Microsoft.SharePoint.WebControls;
    using System.Collections.Generic;
    using Hemrika.SharePresence.WebSite.ContentTypes;
    using System.IO;
    using System.Xml;
    using System.Web.Services.Protocols;
    using Microsoft.SharePoint.Utilities;
    using System.Text;
    using Hemrika.SharePresence.WebSite.Fields;
    using System.Web.UI;
    using Microsoft.SharePoint.Administration;

    [WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1), WebService(Namespace = "http://schemas.microsoft.com/sharepoint/publishing/soap/", Name = "CMS Content Area Toolbox Info service", Description = "This web service is designed for FrontPage client to use"), SharePointPermission(SecurityAction.LinkDemand, ObjectModel = true)]
    public sealed class contentAreaToolboxService : WebService
    {
        //private IContainer components;
        private static int contentTypepanelTypeIdentifier = 2;
        private static int pageControlspanelTypeIdentifier = 0;
        private static int publishingControlspanelTypeIdentifier = 1;

        public contentAreaToolboxService()
        {
            this.InitializeComponent();
        }

        [SharePointPermission(SecurityAction.Demand, ObjectModel = true)]
        protected override void Dispose(bool disposing)
        {
            try
            {
                /*
                if (disposing && (this.components != null))
                {
                    this.components.Dispose();
                }
                */
            }
            finally
            {
                base.Dispose(disposing);
            }
        }

        [WebMethod]
        public string[] FetchControlLists(string[] controlListIds, string cultureName)
        {
            //CommonUtilities.ConfirmNotNull(cultureName, "cultureName");
            //ULS.SendTraceTag(0x3670356d, ULSCat.msoulscat_CMS_Publishing, ULSTraceLevel.Medium, "SharepointPublishingToolboxService.FetchControlLists(, {0})", new object[] { cultureName });
            SPUtility.EnsureAuthentication(SPContext.Current.Site.RootWeb);
            CultureInfo culture = new CultureInfo(cultureName);
            //CommonUtilities.ConfirmNotNull(controlListIds, "controlListIds");
            SPContentTypeCollection contentTypes = this.GetContentTypes();
            TagNameCreator tagCreator = new TagNameCreator();
            string[] strArray = new string[controlListIds.Length];
            for (int i = 0; i < controlListIds.Length; i++)
            {
                string str = controlListIds[i];
                if (str == publishingControlspanelTypeIdentifier.ToString())
                {
                    strArray[i] = "";
                }
                else if (contentTypes != null)
                {
                    SPContentTypeId id = new SPContentTypeId(str);
                    SPContentType contentType = contentTypes[id];
                    if (contentType == null )
                    {
                        //string message = Resources.GetFormattedStringEx("ErrorContentTypeNotFound", culture, new object[] { str });
                        //ULS.SendTraceTag(0x3670356e, ULSCat.msoulscat_CMS_Publishing, ULSTraceLevel.Unexpected, "The content type {0} is not found", new object[] { str });
                        SoapException exception = new SoapException("ContentType is Empty", SoapException.ClientFaultCode);
                        throw exception;
                    }
                    strArray[i] = GetInfoFromCT(contentType, contentTypes, tagCreator, culture);
                }
            }
            //ULS.SendTraceTag(0x3670356f, ULSCat.msoulscat_CMS_Publishing, ULSTraceLevel.Medium, "SharepointPublishingToolboxService.GetCustomControlLists ends");
            return strArray;
        }

        private PanelInfo[] FetchPanelsInformation(string contentTypeId, string cultureName)
        {
            //CommonUtilities.ConfirmNotNullOrEmptyString(contentTypeId, "contentTypeId");
            //CommonUtilities.ConfirmNotNull(cultureName, "cultureName");
            //ULS.SendTraceTag(0x36703569, ULSCat.msoulscat_CMS_Publishing, ULSTraceLevel.Medium, "SharepointPublishingToolboxService.FetchPanelsInformation({0}, {1})", new object[] { contentTypeId, cultureName });
            SPUtility.EnsureAuthentication(SPContext.Current.Site.RootWeb);
            CultureInfo culture = new CultureInfo(cultureName);
            SPContentTypeCollection contentTypes = this.GetContentTypes();
            PanelInfo[] infoArray = new PanelInfo[] { this.GetPageControlsPanelInfo(contentTypes, culture), this.GetContentTypeControlsPanelInfo(contentTypes, contentTypeId, culture) };
            //ULS.SendTraceTag(0x3670356a, ULSCat.msoulscat_CMS_Publishing, ULSTraceLevel.Medium, "SharepointPublishingToolboxService.FetchPanelsInformation ends", new object[] { contentTypeId, cultureName });
            return infoArray;
        }

        [WebMethod]
        public PanelInfo[] FetchPanelsInformationByUrl(string pageLayoutUrl, string cultureName)
        {
            //CommonUtilities.ConfirmNotNullOrEmptyString(pageLayoutUrl, "pageLayoutUrl");
            //CommonUtilities.ConfirmNotNull(cultureName, "cultureName");
            //ULS.SendTraceTag(0x3670356b, ULSCat.msoulscat_CMS_Publishing, ULSTraceLevel.Medium, "SharepointPublishingToolboxService.FetchPanelsInformationByUrl({0}, {1})", new object[] { pageLayoutUrl, cultureName });
            PanelInfo[] infoArray = null;
            SPSite site = SPContext.Current.Site;
            SPUtility.EnsureAuthentication();
            Uri uri = new Uri(pageLayoutUrl);
            string url = ConvertToAbsoluteUrl(uri.PathAndQuery, site);
            SPWeb web = null;
            try
            {
                web = site.OpenWeb();
                SPFile fileFromUrl = GetFileFromUrl(url, web);
                if ((fileFromUrl != null) && fileFromUrl.Exists)
                {
                    ContentTypeIdFieldValue value2;
                    SPListItem item = fileFromUrl.Item;
                    SPContentTypeCollection contentTypes = this.GetContentTypes();
                    try
                    {
                        value2 = (ContentTypeIdFieldValue)item[BuildFieldId.PublishingAssociatedContentType];
                    }
                    catch (ArgumentException)
                    {
                        SPContentType contentType = item.ContentType;
                        while ((contentTypes[contentType.Id] == null) && (contentType.Parent != null))
                        {
                            contentType = contentType.Parent;
                        }
                        value2 = new ContentTypeIdFieldValue(contentType);
                    }
                    string contentTypeId = value2.Id.ToString();
                    SPContentType type2 = this.GetContentTypes()[value2.Id];
                    if ((type2 == null) || !type2.Id.IsChildOf(ContentTypeId.PageTemplate))
                    {
                        CultureInfo culture = new CultureInfo(cultureName);
                       // throw new SoapException(Resources.GetFormattedStringEx("ErrorInvalidAssociatedContentTypeId", culture, new object[] { contentTypeId, value2.StoredName, pageLayoutUrl }), SoapException.ServerFaultCode);
                    }
                    infoArray = this.FetchPanelsInformation(contentTypeId, cultureName);
                }
            }
            finally
            {
                if (web != null)
                {
                    web.Close();
                }
            }
            //ULS.SendTraceTag(0x3670356c, ULSCat.msoulscat_CMS_Publishing, ULSTraceLevel.Medium, "SharepointPublishingToolboxService.FetchPanelsInformationByUrl ends");
            return infoArray;
        }

        private SPFile GetFileFromUrl(string url, Microsoft.SharePoint.SPWeb web)
        {
            ConfirmNotNullOrEmpty(url, "url");
            ConfirmNotNull(web, "web");
            Microsoft.SharePoint.SPFile fileOrFolderObject = null;
            try
            {
                fileOrFolderObject = web.GetFileOrFolderObject(url) as Microsoft.SharePoint.SPFile;
            }
            catch (SPException ex)
            {
                SPDiagnosticsService.Local.WriteTrace(0, new SPDiagnosticsCategory(ex.Source, TraceSeverity.High, EventSeverity.Error), TraceSeverity.High, ex.Message, ex.Data);
            }
            catch (FileNotFoundException ex)
            {
                SPDiagnosticsService.Local.WriteTrace(0, new SPDiagnosticsCategory(ex.Source, TraceSeverity.High, EventSeverity.Error), TraceSeverity.High, ex.Message, ex.Data);
            }
            catch (ArgumentException ex)
            {
                SPDiagnosticsService.Local.WriteTrace(0, new SPDiagnosticsCategory(ex.Source, TraceSeverity.High, EventSeverity.Error), TraceSeverity.High, ex.Message, ex.Data);
            }
            catch (PathTooLongException ex)
            {
                SPDiagnosticsService.Local.WriteTrace(0, new SPDiagnosticsCategory(ex.Source, TraceSeverity.High, EventSeverity.Error), TraceSeverity.High, ex.Message, ex.Data);
            }
            return fileOrFolderObject;
        }

        private void ConfirmNotNullOrEmpty(string value, string parameterName)
        {
            if (string.IsNullOrEmpty(value))
            {
                throw new ArgumentNullException(parameterName);
            }
        }

        private void ConfirmNotNullOrEmptyString(string value, string parameterName)
        {
            if (string.IsNullOrEmpty(value))
            {
                throw new ArgumentException(parameterName+" is Null or Empty");
            }
        }

        private string ConvertToAbsoluteUrl(string siteCollectionRelativeUrl, SPSite site)
        {
            string str = null;
            Uri baseUri = new Uri(EnsureEndsWithPathSeparator(site.Url));
            Uri uri2 = new Uri(baseUri, siteCollectionRelativeUrl);
            if (uri2.IsAbsoluteUri)
            {
                str = Uri.UnescapeDataString(uri2.AbsoluteUri);
            }
            return str;
        }

        private string EnsureEndsWithPathSeparator(string url)
        {
            ConfirmNotNull(url, "url");
            if (url.EndsWith("/", StringComparison.OrdinalIgnoreCase))
            {
                return url;
            }
            return (url + "/");
        }

        private void ConfirmNotNull(object value, string parameterName)
        {
            if (value == null)
            {
                throw new ArgumentNullException(parameterName);
            }
        }

        private static ArrayList FilterOutPageFields(SPContentType contentType, SPContentTypeCollection contentTypes)
        {
            if ((contentType.Id != ContentTypeId.PageTemplate) && contentType.Id.IsChildOf(ContentTypeId.PageTemplate))
            {
                SPContentType type = contentTypes[ContentTypeId.PageTemplate];
                return GetFilteredFields(contentType.Fields, type.Fields);
            }
            return new ArrayList(contentType.Fields);
        }

        private PanelInfo GetContentTypeControlsPanelInfo(SPContentTypeCollection contentTypes, string contentTypeIdString, CultureInfo culture)
        {
            SPContentTypeId id = new SPContentTypeId(contentTypeIdString);
            SPContentType type = contentTypes[id];
            PanelInfo info = new PanelInfo();
            info = new PanelInfo();
            info.displayName = type.Name;// "";//Resources.GetFormattedStringEx("ToolboxPanelNameContentTypeControls", culture, new object[] { type.Name });
            info.controlListId = contentTypeIdString;
            info.panelTypeIdentifier = contentTypepanelTypeIdentifier;
            return info;
        }

        private SPContentTypeCollection GetContentTypes()
        {
            return SPContext.Current.Site.RootWeb.ContentTypes;
        }

        private static System.Type GetFieldRenderingControlType(SPField field)
        {
            if (field.FieldRenderingControl == null)
            {
                return typeof(FormField);
            }
            BaseFieldControl fieldRenderingControl = field.FieldRenderingControl;
            if (fieldRenderingControl.GetType().Equals(typeof(RichTextField)))
            {
                return typeof(NoteField);
            }
            return fieldRenderingControl.GetType();
        }

        private static ArrayList GetFilteredFields(SPFieldCollection fields, SPFieldCollection referenceFields)
        {
            ArrayList list = new ArrayList();
            foreach (SPField field in fields)
            {
                if(!referenceFields.ContainsField(field.InternalName))
                {
                    list.Add(field);
                }
            }
            return list;
        }

        private static string GetInfoFromCT(SPContentType contentType, SPContentTypeCollection contentTypes, TagNameCreator tagCreator, CultureInfo culture)
        {
            string str4;
            StringWriter w = new StringWriter(new StringBuilder(), CultureInfo.InvariantCulture);
            XmlTextWriter writer2 = new XmlTextWriter(w);
            try
            {
                writer2.WriteStartDocument();
                writer2.WriteStartElement("ControlsList");
                TagInfo info = null;
                TagInfo tag = null;
                foreach (SPField field in FilterOutPageFields(contentType, contentTypes))
                {
                    if (field.Hidden) // && (field.Group != "Publishing Content Columns") || field.Group != "HTML5 Columns")
                    {
                        continue;
                    }
                    System.Type fieldRenderingControlType = GetFieldRenderingControlType(field);
                    tag = tagCreator.GetTag(fieldRenderingControlType);
                    if (info != tag)
                    {
                        if (info != null)
                        {
                            writer2.WriteEndElement();
                        }
                        info = tag;
                        writer2.WriteStartElement("Assembly");
                        writer2.WriteAttributeString("Name", tag.assemblyName);
                        writer2.WriteAttributeString("Namespace", tag.namespaceName);
                        writer2.WriteAttributeString("TagPrefix", tag.name);
                    }
                    writer2.WriteStartElement("Control");
                    string title = field.Title;
                    string str2 = field.Id.ToString();
                    writer2.WriteAttributeString("Name", title);
                    writer2.WriteAttributeString("Title", field.Title);
                    string str3 = ""; //Resources.GetFormattedStringEx("ToolboxControlDescription", culture, new object[] { field.Title, field.TypeAsString });
                    writer2.WriteAttributeString("Description", str3);
                    writer2.WriteAttributeString("Template", string.Format(culture, "<{{0}}:{0} FieldName=\"{1}\" runat=\"server\"></{{0}}:{0}>", new object[] { fieldRenderingControlType.Name, str2 }));
                    writer2.WriteEndElement();
                }
                if (tag != null)
                {
                    writer2.WriteEndElement();
                }
                writer2.WriteEndElement();
                writer2.WriteEndDocument();
                str4 = w.GetStringBuilder().ToString();
            }
            finally
            {
                /*
                if (writer2 != null)
                {
                    writer2.Close();
                }
                */
                if (w != null)
                {
                    w.Close();
                }
            }
            return str4;
        }

        private PanelInfo GetPageControlsPanelInfo(SPContentTypeCollection contentTypes, CultureInfo culture)
        {
            SPContentType type = contentTypes[ContentTypeId.PageTemplate];
            PanelInfo info = new PanelInfo();
            info = new PanelInfo();
            info.displayName = type.Name;// ""; //Resources.GetStringEx("ToolboxPanelNamePageControls", culture);
            info.controlListId = type.Id.ToString();
            info.panelTypeIdentifier = pageControlspanelTypeIdentifier;
            return info;
        }

        private void InitializeComponent()
        {
        }

        private class TagInfo
        {
            public string assemblyName;
            public string name;
            public string namespaceName;

            public TagInfo(string name, string assemblyName, string namespaceName)
            {
                this.name = name;
                this.assemblyName = assemblyName;
                this.namespaceName = namespaceName;
            }
        }

        private class TagNameCreator
        {
            private static contentAreaToolboxService.TagInfo[] builtInTags = new contentAreaToolboxService.TagInfo[] { new contentAreaToolboxService.TagInfo("wss", typeof(FormField).Assembly.FullName, typeof(FormField).Namespace) };
            private const string CustomTagFormat = "WebSite";
            private List<contentAreaToolboxService.TagInfo> tagList;

            private contentAreaToolboxService.TagInfo CreateNewTag(System.Type type)
            {
                if (this.tagList == null)
                {
                    this.tagList = new List<contentAreaToolboxService.TagInfo>();
                }
                TagPrefixAttribute tagprefix = null;

                TagPrefixAttribute[] tagPrefixes = (TagPrefixAttribute[])type.Assembly.GetCustomAttributes(typeof(TagPrefixAttribute), true);

                foreach (TagPrefixAttribute tag in tagPrefixes)
                {
                    if (tag.NamespaceName == type.Namespace)
                    {
                        tagprefix = tag;
                        break;
                    }
                }

                contentAreaToolboxService.TagInfo item = null;
                if (tagprefix != null)
                {
                    item = new contentAreaToolboxService.TagInfo(tagprefix.TagPrefix, type.Assembly.FullName, type.Namespace);
                }
                else
                {
                    item = new contentAreaToolboxService.TagInfo(string.Format(CultureInfo.InvariantCulture, "CustomTag_{0}", new object[] { this.tagList.Count }), type.Assembly.FullName, type.Namespace);
                }

                if (item != null && !tagList.Contains(item))
                {
                    this.tagList.Add(item);
                }
                return item;
            }

            private static contentAreaToolboxService.TagInfo FindTag(System.Type type, IEnumerable enumerable)
            {
                if (enumerable != null)
                {
                    foreach (contentAreaToolboxService.TagInfo info in enumerable)
                    {
                        if (string.Equals(type.Assembly.FullName, info.assemblyName, StringComparison.Ordinal) && string.Equals(type.Namespace, info.namespaceName, StringComparison.Ordinal))
                        {
                            return info;
                        }
                    }
                }
                return null;
            }

            public contentAreaToolboxService.TagInfo GetTag(System.Type type)
            {
                contentAreaToolboxService.TagInfo info = FindTag(type, builtInTags);
                if (info == null)
                {
                    info = FindTag(type, this.tagList);
                }
                if (info == null)
                {
                    return this.CreateNewTag(type);
                }
                return info;
            }
        }
    }
}

