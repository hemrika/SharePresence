// -----------------------------------------------------------------------
// <copyright file="WebSiteDocItemModifier.cs" company="">
// TODO: Update copyright text.
// </copyright>
// -----------------------------------------------------------------------

namespace Hemrika.SharePresence.Common.WebSiteController
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using Microsoft.SharePoint.Administration;
    using System.IO;
    using System.Xml.Linq;
    using Microsoft.SharePoint.Utilities;

    /// <summary>
    /// Modifies the DocIcon.xml file with the requested mapping.
    /// </summary>
    public class WebSiteDocItemModifier: SPJobDefinition
    {
        /// <summary>
        /// 
        /// </summary>
        public WebSiteDocItemModifier() : base() { }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="service"></param>
        /// <param name="featureID"></param>
        /// <param name="delete"></param>
        public WebSiteDocItemModifier(SPService service, Guid featureID, bool delete) : base("DocIcon Modifier for "+ featureID.ToString(), service, null, SPJobLockType.None)
        {
            this._featureID = featureID;
            SPFeatureDefinition def = Farm.FeatureDefinitions[featureID];
            this.Key = def.Properties[keyKey].Value;
            this.Value = def.Properties[valueKey].Value;
            this.EditText = def.Properties[textKey].Value;
            this.OpenControl = def.Properties[controlKey].Value;
            this.docIconType = def.Properties[typeKey].Value;
            this._delete = delete;
            
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="service"></param>
        /// <param name="docType"></param>
        /// <param name="key"></param>
        /// <param name="value"></param>
        /// <param name="editText"></param>
        /// <param name="openControl"></param>
        /// <param name="delete"></param>
        public WebSiteDocItemModifier(SPService service, DocIconType docType, string key, string value, string editText, string openControl, bool delete)
            : base("DocIcon Modifier for "+key, service, null, SPJobLockType.None)
        {
            this._delete = delete;
            this.Key = key;
            this.Value = value;
            this.EditText = editText;
            this.OpenControl = openControl;
            this.docIconType = docType.ToString();

        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="targetInstanceId"></param>
        public override void Execute(Guid targetInstanceId)
        {
            base.Execute(targetInstanceId);
            this.ChangeDocIconFile();
        }

        /// <summary>
        /// 
        /// </summary>
        private void ChangeDocIconFile()
        {
            string spThemesContent = string.Empty;
            if (File.Exists(SPDocIconFile))
            {
                XDocument doc = XDocument.Load(SPDocIconFile);

                var element = from b in doc.Element("DocIcons").Element(docIconType).Elements("Mapping")
                              where b.Attribute("Key").Value.ToLower() == this.Key.ToLower()
                              select b; bool containsElement = (element != null && element.Count() > 0);
                if (_delete)
                {
                    if (containsElement)
                    {
                        element.Remove();
                        doc.Save(SPDocIconFile);
                    }
                }
                else
                {
                    if (!containsElement)
                    {
                        XElement iconElement = new XElement("Mapping");
                        iconElement.SetAttributeValue("Key", Key);
                        iconElement.SetAttributeValue("Value", Value);
                        iconElement.SetAttributeValue("EditText", EditText);
                        iconElement.SetAttributeValue("OpenControl", OpenControl);
                        doc.Element("DocIcons").Element(docIconType).Add(iconElement);
                        doc.Save(SPDocIconFile);
                    }
                }
            }
            else
            {
                throw new FileNotFoundException("The DocIcon file does not exist on the server");
            }
        }

        /// <summary>          
        /// Filepath for the spthemes.xml file          
        /// </summary>          
        private readonly string filePath = "TEMPLATE\\XML\\DOCICON.XML";

        /// <summary>          
        /// The property key for the delete property          
        /// </summary>          
        private readonly string deleteKey = "Icon_Installation_Deletekey";

        /// <summary>          
        /// The property key for the feature property          
        /// </summary>          
        private readonly string featureKey = "Icon_Installation_Featurekey";
        private readonly string typeKey = "Icon_Installation_typekey";
        private readonly string keyKey = "Icon_Installation_Keykey";
        private readonly string valueKey = "Icon_Installation_Valuekey";
        //private readonly string urlKey = "Icon_Installation_Urlkey";
        private readonly string textKey = "Icon_Installation_Textkey";
        private readonly string controlKey = "Icon_Installation_Controlkey";

        /// <summary>
        /// 
        /// </summary>
        private bool _delete
        {
            get
            {
                if (this.Properties.ContainsKey(deleteKey))
                {
                    return Convert.ToBoolean(this.Properties[deleteKey]);
                }
                else
                {
                    return false;
                }
            }
            set
            {
                if (this.Properties.ContainsKey(deleteKey))
                {
                    this.Properties[deleteKey] = value.ToString();
                }
                else
                {
                    this.Properties.Add(deleteKey, value.ToString());
                }
            }
        }

        /// <summary>
        /// 
        /// </summary>
        private Guid _featureID
        {
            get
            {
                if (this.Properties.ContainsKey(featureKey))
                {
                    return new Guid(this.Properties[featureKey].ToString());
                }
                else
                {
                    return Guid.Empty;
                }
            }
            set
            {
                if (this.Properties.ContainsKey(featureKey))
                {
                    this.Properties[featureKey] = value.ToString();
                }
                else
                {
                    this.Properties.Add(featureKey, value.ToString());
                }
            }
        }

        /*
        private string IconExtension
        {
            get
            {
                if(this.Properties.ContainsKey(extensionKey))
                {
                    
                    return this.Properties[extensionKey].ToString();
                }
                else
                {
                    return string.Empty;
                }
            }
            set
            {
                if (this.Properties.ContainsKey(extensionKey))
                {
                    this.Properties[extensionKey] = value.ToString();
                }
                else
                {
                    this.Properties.Add(extensionKey, value.ToString());
                }
            }
        }
        */
        /*
        private string IconUrl
        {
            get
            {
                if (this.Properties.ContainsKey(urlKey))
                {

                    return this.Properties[urlKey].ToString();
                }
                else
                {
                    return string.Empty;
                }
            }
            set
            {
                if (this.Properties.ContainsKey(urlKey))
                {
                    this.Properties[urlKey] = value.ToString();
                }
                else
                {
                    this.Properties.Add(urlKey, value.ToString());
                }
            }
        }
        */

        /// <summary>
        /// 
        /// </summary>
        public string SPDocIconFile
        {
            get
            {
                //return SPUtility.GetGenericSetupPath(filePath);
                return SPUtility.GetVersionedGenericSetupPath(filePath,15);
            }
        }

        /// <summary>
        /// 
        /// </summary>
        public string OpenControl
        {
            get
            {
                if (this.Properties.ContainsKey(controlKey))
                {

                    return this.Properties[controlKey].ToString();
                }
                else
                {
                    return string.Empty;
                }
            }
            set
            {
                if (this.Properties.ContainsKey(controlKey))
                {
                    this.Properties[controlKey] = value.ToString();
                }
                else
                {
                    this.Properties.Add(controlKey, value.ToString());
                }
            }
        }

        /// <summary>
        /// 
        /// </summary>
        public string EditText
        {
            get
            {
                if (this.Properties.ContainsKey(textKey))
                {

                    return this.Properties[textKey].ToString();
                }
                else
                {
                    return string.Empty;
                }
            }
            set
            {
                if (this.Properties.ContainsKey(textKey))
                {
                    this.Properties[textKey] = value.ToString();
                }
                else
                {
                    this.Properties.Add(textKey, value.ToString());
                }
            }
        }

        /// <summary>
        /// 
        /// </summary>
        public string docIconType
        {
            get
            {
                if (this.Properties.ContainsKey(typeKey))
                {

                    return this.Properties[typeKey].ToString();
                }
                else
                {
                    return DocIconType.ByExtension.ToString();
                }
            }
            set
            {
                if (this.Properties.ContainsKey(typeKey))
                {
                    this.Properties[typeKey] = value.ToString();
                }
                else
                {
                    this.Properties.Add(typeKey, value.ToString());
                }
            }
        }

        /// <summary>
        /// 
        /// </summary>
        public string Key
        {
            get
            {
                if (this.Properties.ContainsKey(keyKey))
                {

                    return this.Properties[keyKey].ToString();
                }
                else
                {
                    return string.Empty;
                }
            }
            set
            {
                if (this.Properties.ContainsKey(keyKey))
                {
                    this.Properties[keyKey] = value.ToString();
                }
                else
                {
                    this.Properties.Add(keyKey, value.ToString());
                }
            }
        }

        /// <summary>
        /// 
        /// </summary>
        public string Value
        {
            get
            {
                if (this.Properties.ContainsKey(valueKey))
                {

                    return this.Properties[valueKey].ToString();
                }
                else
                {
                    return string.Empty;
                }
            }
            set
            {
                if (this.Properties.ContainsKey(valueKey))
                {
                    this.Properties[valueKey] = value.ToString();
                }
                else
                {
                    this.Properties.Add(valueKey, value.ToString());
                }
            }
        }
    }
}

