using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.SharePoint.WebControls;
using Hemrika.SharePresence.WebSite.FieldTypes;
using System.Collections;
using System.Xml;
using System.IO;
using System.Globalization;
using Microsoft.SharePoint.Security;
using System.Collections.Specialized;
using System.Security.Permissions;
using System.Web.UI;
using Microsoft.SharePoint.Utilities;
using System.Xml.Serialization;
using System.Web.UI.HtmlControls;
using Microsoft.SharePoint;
using System.Web.UI.WebControls;
using System.Web;
using System.Xml.Linq;

namespace Hemrika.SharePresence.WebSite.FieldTypes
{
    public class HTML5ImagePicker : EntityEditorWithPicker
    {
        protected HyperLink upload;

        public HTML5ImagePicker() : base()
        {
            PickerDialogType = typeof(HTML5ImagePickerDialog);
            ValidatorEnabled = false;
            AllowTypeIn = true;
            MultiSelect = false;
            DialogHeight = 600;
            DialogWidth = 800;
            BrowseButtonImageName = "/_layouts/Hemrika/Content/images/HTML5ImageSearch_16.png";
            CheckButtonImageName = "/_layouts/Hemrika/Content/images/HTML5ImageCheck_16.png";
            DialogImage = "/_layouts/Hemrika/Content/images/HTML5Image_16.png";
        }

        private Control BrowseParent = null;

        private void FindBrowseParent(Control picker)
        {
            foreach (Control control in picker.Controls)
            {
                if (control.ID != null && control.ID.ToLowerInvariant() == "browse".ToLowerInvariant())
                {
                    BrowseParent = control.Parent;
                    break;
                }
                if (control.HasControls())
                {
                    FindBrowseParent(control);
                }
            }
        }

        private void CreateUpload(Control parent)
        {
            if (this.Enabled)
            {
                ScriptLink.RegisterScriptAfterUI(this.Page, "/Hemrika/Content/js/HTML5ImageUpload.js", false);
                FindBrowseParent(parent);

                upload = new HyperLink();
                if (BrowseParent != null)
                {
                    BrowseParent.Controls.Add(new LiteralControl(SPHttpUtility.NoEncode("&#160;")));
                    upload.ID = "upload";
                    upload.Attributes.Add("onclick", "UploadHTML5Image('{0}', '{1}', '{2}');return false;");
                    upload.NavigateUrl = @"javascript:UploadHTML5Image('{0}', '{1}', '{2}')";
                    upload.ImageUrl = "/_layouts/Hemrika/Content/images/HTML5ImageAdd_16.png";
                    upload.Text = "Upload";
                    BrowseParent.Controls.Add(upload);
                    EnsureChildControls();
                }
                
            }
        }

        protected override void CreateChildControls()
        {
            base.CreateChildControls();

            CreateUpload(this);
        }

        protected override void OnPreRender(EventArgs e)
        {
            base.OnPreRender(e);

            string[] url = upload.NavigateUrl.Split(new char[1] { ':' });
            if (url.Length > 1)
            {
                upload.Attributes["onclick"] = url[1] + ";return false;";
                upload.NavigateUrl = string.Empty;
            }
        }

        /// <summary>
        /// Resolve search text into a collection of picker entities
        /// </summary>
        /// <param name="unresolvedText"></param>
        /// <returns></returns>
        protected override PickerEntity[] ResolveErrorBySearch(string unresolvedText)
        {
            unresolvedText = unresolvedText.TrimStart(new char[] { '/' });
            List<HTML5ImagePickerEntity> resolvedItems = null;
            SPWeb web = null;
            SPListItem item = null;
            try
            {
                web = SPContext.Current.Site.OpenWeb(unresolvedText);
            }
            catch (Exception)
            {
                web = SPContext.Current.Site.RootWeb;
            }

            if (!web.Exists)
            {
                web = SPContext.Current.Web;
            }

            try
            {
                
                item = web.GetListItem(unresolvedText);
                if (item != null)
                {
                    resolvedItems = new List<HTML5ImagePickerEntity>();
                    resolvedItems.Add(new HTML5ImagePickerEntity(item));
                }
            }
            catch (Exception)
            {
                return null;
            }

            return resolvedItems != null ? resolvedItems.ToArray() : null;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        public override PickerEntity ValidateEntity(PickerEntity entity)
        {
            if (entity != null)
            {
                HTML5ImagePickerEntity imageEntity = new HTML5ImagePickerEntity(entity);
                entity = ValidateEntity(imageEntity) as PickerEntity;

                if (!entity.IsResolved)
                {
                    PickerEntity[] resolved = ResolveErrorBySearch(entity.DisplayText);
                    if (resolved != null && resolved.Count() == 1)
                    {
                        return resolved[0] as PickerEntity;
                    }
                    else
                    {
                        entity.MultipleMatches.Add(resolved);
                    }
                    return entity as PickerEntity;
                }
                else
                {
                    return entity;
                }
            }
            else
            {
                return entity;
            }
        }

        public override void Validate()
        {
            base.Validate();

            foreach (PickerEntity entity in Entities)
            {
                if (entity.IsResolved)
                {
                    continue;
                }
                
                //HTML5ImagePickerEntity validated = ValidateEntity(entity);

                //entity.MultipleMatches.Clear();
                //entity.MultipleMatches.AddRange(validated.MultipleMatches);
            }


            if (base.IsValid)
            {
                base.ErrorMessage = string.Empty;
            }
        }

        [SharePointPermission(SecurityAction.Demand, ObjectModel = true)]
        public override bool LoadPostData(string postDataKey, NameValueCollection values)
        {
            this.EnsureChildControls();
            string spans = StrEatUpNbsp(this.HiddenSpanData.Value);
            spans = HttpUtility.HtmlDecode(spans);

            if (!string.IsNullOrEmpty(spans))
            {
                int start = spans.IndexOf("<ArrayOfDictionaryEntry");
                int end = spans.LastIndexOf("</ArrayOfDictionaryEntry>");
                if (start > 0 && end > 0)
                {
                    string ArrayOfDictionaryEntry = spans.Substring(start, (end - start) + 25);
                    XmlDocument xsd = new XmlDocument();
                    xsd.LoadXml(ArrayOfDictionaryEntry);
                    XmlNode xsdNode = xsd.DocumentElement;
                    XmlElement element = null;
                    XmlDocument xml = new XmlDocument();
                    CreateXML(xsdNode, element, ref xml);
                    string cleaned = xsd.OuterXml;
                    spans = spans.Replace(ArrayOfDictionaryEntry, cleaned);

                }

                if (start > 0)
                {
                    int msxml = spans.IndexOf(">", start);
                    if (msxml > 0)
                    {
                        string namespaces = spans.Substring(start, (msxml - start));
                        spans = spans.Replace(namespaces, "<ArrayOfDictionaryEntry");
                    }
                }
                //spans = HttpUtility.HtmlEncode(spans);
                this.HiddenSpanData.Value = spans ;
            }

            try
            {
                base.LoadPostData(postDataKey, values);
            }
            catch (Exception)
            {
                return false;
            }

            return true;
        }

        /// <summary>
        /// Validate picker entity
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        public HTML5ImagePickerEntity ValidateEntity(HTML5ImagePickerEntity entity)
        {


            //entity.IsResolved = true;
            return entity;
            //entity.EntityData = imageEntity.EntityData;

            //return base.ValidateEntity(imageEntity);
        }
        
        public new HTML5ImagePickerDialog PickerDialog
        {
            get
            {
                return (HTML5ImagePickerDialog)base.PickerDialog;
            }
            set
            {
                base.PickerDialog = value;
            }
        }
        
        public Guid ListId
        {
            get
            {
                return new HTML5ImagePropertyBag(this.CustomProperty).ListId;
            }
        }

        public Guid WebId
        {
            get
            {
                return new HTML5ImagePropertyBag(this.CustomProperty).WebId;
            }
        }

        public Guid ItemId
        {
            get
            {
                return new HTML5ImagePropertyBag(this.CustomProperty).ItemId;
            }
        }

        public string QueryBox
        {
            get
            {
                return new HTML5ImagePropertyBag(this.CustomProperty).QueryBox;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="entities"></param>
        /// <param name="append"></param>
        /// <returns></returns>
        #pragma warning disable 612, 618
        internal string GenerateCallbackData(ArrayList entities, bool append)
        {
            return GenerateCallbackData(entities, this.ErrorMessage, this.DoEncodeErrorMessage, this.EntitySeparator, this.MaximumHeight, append);
        }
        #pragma warning restore 612, 618

        /// <summary>
        /// 
        /// </summary>
        /// <param name="entities"></param>
        /// <param name="errorMessage"></param>
        /// <param name="doEncodeErrorMessage"></param>
        /// <param name="entitySeparator"></param>
        /// <param name="maximumHeight"></param>
        /// <param name="append"></param>
        /// <returns></returns>
        internal static string GenerateCallbackData(ArrayList entities, string errorMessage, bool doEncodeErrorMessage, char entitySeparator, int maximumHeight, bool append)
        {
            StringBuilder sb = new StringBuilder(0x100);
            XmlTextWriter writer = new XmlTextWriter(new StringWriter(sb, CultureInfo.InvariantCulture));
            writer.WriteStartElement("Entities");
            writer.WriteAttributeString("Append", append.ToString());
            writer.WriteAttributeString("Error", errorMessage);
            writer.WriteAttributeString("DoEncodeErrorMessage", doEncodeErrorMessage.ToString());
            if (entitySeparator != '\0')
            {
                writer.WriteAttributeString("Separator", entitySeparator.ToString());
            }
            writer.WriteAttributeString("MaxHeight", maximumHeight.ToString(CultureInfo.InvariantCulture));
            foreach (PickerEntity entity in entities)
            {
                writer.WriteRaw(CleanEntity(entity.ToXmlData()));
            }
            writer.WriteEndElement();
            writer.Close();
            return sb.ToString();
        }

        private static string CleanEntity(string XmlData)
        {
            int start = XmlData.IndexOf("<ArrayOfDictionaryEntry");
            int end = XmlData.LastIndexOf("</ArrayOfDictionaryEntry>");
            if (start > 0 && end > 0)
            {
                string ArrayOfDictionaryEntry = XmlData.Substring(start, (end - start) + 25);
                XmlDocument xsd = new XmlDocument();
                xsd.LoadXml(ArrayOfDictionaryEntry);
                XmlNode xsdNode = xsd.DocumentElement;
                XmlElement element = null;
                XmlDocument xml = new XmlDocument();
                CreateXML(xsdNode, element, ref xml);
                string cleaned = xsd.OuterXml;
                XmlData = XmlData.Replace(ArrayOfDictionaryEntry, cleaned);

            }

            if (start > 0)
            {
                int msxml = XmlData.IndexOf(">", start);
                if (msxml > 0)
                {
                    string namespaces = XmlData.Substring(start, (msxml - start));
                    XmlData = XmlData.Replace(namespaces, "<ArrayOfDictionaryEntry");
                }
            }
            return XmlData;
        }

        private static void CreateXML(XmlNode xsdNode, XmlElement element, ref XmlDocument xml)
        {
            xsdNode.Attributes.RemoveAll();
            if (xsdNode.HasChildNodes)
            {
                var childs = xsdNode.ChildNodes;
                foreach (XmlNode node in childs)
                {
                    if (node.Attributes != null)
                    {
                        node.Attributes.RemoveAll();
                    }

                    if (node.HasChildNodes)
                    {
                        foreach (XmlNode subnode in node.ChildNodes)
                        {
                            CreateXML(subnode, element, ref xml);
                        }
                    }
                }
            }
        }
    }
}
