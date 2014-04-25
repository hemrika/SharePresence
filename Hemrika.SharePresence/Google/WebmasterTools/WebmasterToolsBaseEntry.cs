namespace Hemrika.SharePresence.Google.WebmasterTools
{
    using Client;
    using Extensions;
    using System;

    public class WebmasterToolsBaseEntry : AbstractEntry
    {
        public SimpleAttribute getWebmasterToolsAttribute(string extension)
        {
            return (base.FindExtension(extension, "http://schemas.google.com/webmasters/tools/2007") as SimpleAttribute);
        }

        public string getWebmasterToolsAttributeValue(string extension)
        {
            SimpleAttribute attribute = this.getWebmasterToolsAttribute(extension);
            if (attribute != null)
            {
                return attribute.Value;
            }
            return null;
        }

        public SimpleElement getWebmasterToolsExtension(string extension)
        {
            return (base.FindExtension(extension, "http://schemas.google.com/webmasters/tools/2007") as SimpleElement);
        }

        public string getWebmasterToolsValue(string extension)
        {
            SimpleElement element = this.getWebmasterToolsExtension(extension);
            if (element != null)
            {
                return element.Value;
            }
            return null;
        }

        public SimpleElement setWebmasterToolsAttribute(string extension, string newValue)
        {
            if (extension == null)
            {
                throw new ArgumentNullException("extension");
            }
            SimpleAttribute attribute = this.getWebmasterToolsAttribute(extension);
            if (attribute == null)
            {
                attribute = base.CreateExtension(extension, "http://schemas.google.com/webmasters/tools/2007") as SimpleAttribute;
                base.ExtensionElements.Add(attribute);
            }
            attribute.Value = newValue;
            return attribute;
        }

        public SimpleElement setWebmasterToolsExtension(string extension, string newValue)
        {
            if (extension == null)
            {
                throw new ArgumentNullException("extension");
            }
            SimpleElement element = this.getWebmasterToolsExtension(extension);
            if ((element == null) && (newValue != null))
            {
                element = base.CreateExtension(extension, "http://schemas.google.com/webmasters/tools/2007") as SimpleElement;
                if (element != null)
                {
                    base.ExtensionElements.Add(element);
                }
            }
            if (element == null)
            {
                throw new ArgumentException("invalid extension element specified");
            }
            if ((newValue == null) && (element != null))
            {
                base.DeleteExtensions(extension, "http://schemas.google.com/webmasters/tools/2007");
            }
            if (element != null)
            {
                element.Value = newValue;
            }
            return element;
        }
    }
}

