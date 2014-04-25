using System;
using System.Collections.Generic;
using System.IO;
using System.Web;
using System.Web.Caching;
using System.Xml;
using System.Xml.Serialization;
using System.Xml.Xsl;
using Microsoft.SharePoint;
using Microsoft.SharePoint.WebPartPages;
using System.Security;
using Microsoft.SharePoint.Utilities;
using System.Xml.XPath;
using System.Net;
using Microsoft.SharePoint.Security;
using System.Security.Permissions;
using System.Web.Script.Serialization;

namespace Hemrika.SharePresence.Common.TemplateEngine
{
    [SharePointPermission(SecurityAction.Demand, ObjectModel = true, Unrestricted=true)]
    [SharePointPermission(SecurityAction.InheritanceDemand, ObjectModel = true)]
    [SharePointPermission(SecurityAction.LinkDemand, ObjectModel = true, Unrestricted=true)]
    [AspNetHostingPermission(SecurityAction.LinkDemand, Level = AspNetHostingPermissionLevel.Unrestricted)]
    [PermissionSet(SecurityAction.Demand, Name = "FullTrust")]
	public static class Utilities
	{
		private static readonly Dictionary<Type, XmlSerializer> serialisers = new Dictionary<Type, XmlSerializer>();
        private static string mainXslFolderPath;
        private static string styleXslFolderPath;

        internal static IEnumerable<ITemplateProcessor> SupportedTemplateProcessors()
        {
            return new ITemplateProcessor[] { new MenuTemplateProcessor(), new ListItemTemplateProcessor() };
        }

        internal static XslCompiledTransform CachedXslt(string xslsource)
        {
            XslCompiledTransform result = new XslCompiledTransform(true);
            XsltSettings settings = new XsltSettings(true, true);
            SPXmlUrlResolver resolver = new SPXmlUrlResolver();
            resolver.Credentials = CredentialCache.DefaultNetworkCredentials;

            XmlReaderSettings readerSettings = new XmlReaderSettings();
            readerSettings.ValidationFlags = System.Xml.Schema.XmlSchemaValidationFlags.None;
            readerSettings.XmlResolver = resolver;
            readerSettings.DtdProcessing = DtdProcessing.Ignore;
            //readerSettings.ProhibitDtd = false;
            readerSettings.CheckCharacters = false;

            MemoryStream stream = new MemoryStream();
            StreamWriter writer = new StreamWriter(stream);
            writer.Write(xslsource);
            writer.Flush();
            stream.Position = 0L;


            using (XmlReader reader = XmlReader.Create(stream, readerSettings))//, SPContext.Current.Site.RootWeb.Url))
            {
                try
                {
                    result.Load(reader, settings, resolver);
                }
                catch (XsltException ex)
                {
                    throw ex;
                }
            }

            return result;
        }

        internal static XslCompiledTransform CachedXslt(string filename, bool UseXSL)
        {
            XslCompiledTransform result = new XslCompiledTransform(true);
            XsltSettings settings = new XsltSettings(true, true);
            SPXmlUrlResolver resolver = new SPXmlUrlResolver();
            resolver.Credentials = CredentialCache.DefaultNetworkCredentials;

            XmlReaderSettings readerSettings = new XmlReaderSettings();
            readerSettings.ValidationFlags = System.Xml.Schema.XmlSchemaValidationFlags.None;
            readerSettings.XmlResolver = resolver;
            readerSettings.DtdProcessing = DtdProcessing.Ignore;
            //readerSettings.ProhibitDtd = false;
            readerSettings.CheckCharacters = false;

            if (filename != null)
            {
                SPFile file = SPContext.Current.Site.RootWeb.GetFile(filename);

                using (XmlReader reader = XmlReader.Create(file.OpenBinaryStream(SPOpenBinaryOptions.None), readerSettings))//, SPContext.Current.Site.RootWeb.Url))
                {
                    result.Load(reader, settings, resolver);
                }
            }

            return result;
        }

        internal static string MainXslFolderPath
        {
            get
            {
                if (string.IsNullOrEmpty(mainXslFolderPath))
                {
                    //mainXslFolderPath = string.Concat(new object[] { SPUtility.GetGenericSetupPath(@"Template\layouts"), Path.DirectorySeparatorChar, "xsl", Path.DirectorySeparatorChar });
                    mainXslFolderPath = string.Concat(new object[] { SPUtility.GetVersionedGenericSetupPath(@"Template\layouts",15), Path.DirectorySeparatorChar, "xsl", Path.DirectorySeparatorChar });
                }
                return mainXslFolderPath;
            }
        }

        internal static string StyleXslFolderPath 
        {
            get
            {
                if (string.IsNullOrEmpty(styleXslFolderPath))
                {
                    try
                    {
                        SPWeb root = SPContext.Current.Site.RootWeb;
                        SPList library = root.Lists.TryGetList("Style Library");
                        SPFolder xsl = library.RootFolder.SubFolders["XSL"];
                        if (xsl.Exists)
                        {
                            string fullpath = root.Url + xsl.Url;
                            styleXslFolderPath = SPUtility.ConcatUrls(root.Url, xsl.Url);
                            //styleXslFolderPath = SPHttpUtility.UrlPathEncode(styleXslFolderPath, false);
                            //mainXslFolderPath = string.Concat(new object[] { , Path.DirectorySeparatorChar, "xsl", Path.DirectorySeparatorChar });
                        }
                        else
                        {
                            styleXslFolderPath = MainXslFolderPath;
                        }
                    }
                    catch { }
                }
                return styleXslFolderPath;
            }
        }

        internal static void PreProcessNameSpaceXSL(XmlDocument xmlDocument)
        {
            XmlNamespaceManager mgr = new XmlNamespaceManager(xmlDocument.NameTable);
            mgr.AddNamespace("ddwrt", "http://schemas.microsoft.com/WebParts/v2/DataView/runtime");
            mgr.AddNamespace("xsl", "http://www.w3.org/1999/XSL/Transform");
            mgr.AddNamespace("asp", "http://schemas.microsoft.com/ASPNET/20");
            mgr.AddNamespace("msxsl", "urn:schemas-microsoft-com:xslt");
            mgr.AddNamespace("SharePoint", "Microsoft.SharePoint.WebControls");
            mgr.AddNamespace("d", "http://schemas.microsoft.com/sharepoint/dsp");
            mgr.AddNamespace("exclude-result-prefixes", "xsl msxsl ddwrt");
            mgr.AddNamespace("designer", "http://schemas.microsoft.com/WebParts/v2/DataView/designer");
            mgr.AddNamespace("ddwrt2", "urn:frontpage:internal");
            mgr.AddNamespace("rs", "urn:schemas-microsoft-com:rowset");
            mgr.AddNamespace("z", "#RowsetSchema");
            mgr.AddNamespace("user", "user");
            mgr.AddNamespace("exclude-result-prefixes", "msxsl user");

            XmlNode sheet = xmlDocument.SelectSingleNode("/xsl:stylesheet");

            /*
            foreach (XmlNode node in xmlDocument)
                if (node.NodeType == XmlNodeType.XmlDeclaration)
                {
                    xmlDocument.RemoveChild(node);
                }

            XmlNode refChild = null;
            XmlNode parentNode = null;
            XmlNodeList list = xmlDocument.SelectNodes("/xsl:stylesheet/xsl:import | /xsl:stylesheet/xsl:include", mgr);
            if ((list != null) && (list.Count > 0))
            {
                refChild = list[list.Count - 1];
                parentNode = refChild.ParentNode;
            }
            else
            {
                parentNode = xmlDocument.SelectSingleNode("/xsl:stylesheet", mgr);
            }

            foreach (XmlNode node in list)
            {
                string value = node.Attributes["href"].Value;
                string root = SPContext.Current.Site.Url;
                if (!Uri.IsWellFormedUriString(value, UriKind.Absolute))
                {

                    node.Attributes["href"].Value = SPUtility.ConcatUrls(root, value);
                }
            }
            */
        }

        internal static void PreProcessCachedXSL(XmlDocument xmlDocument, string prefix)
        {
            try
            {
                XmlNamespaceManager mgr = new XmlNamespaceManager(xmlDocument.NameTable);
                mgr.AddNamespace("ddwrt", "http://schemas.microsoft.com/WebParts/v2/DataView/runtime");
                mgr.AddNamespace("xsl", "http://www.w3.org/1999/XSL/Transform");
                mgr.AddNamespace("asp", "http://schemas.microsoft.com/ASPNET/20");
                mgr.AddNamespace("msxsl", "urn:schemas-microsoft-com:xslt");
                mgr.AddNamespace("SharePoint", "Microsoft.SharePoint.WebControls");
                mgr.AddNamespace("d","http://schemas.microsoft.com/sharepoint/dsp");
                mgr.AddNamespace("exclude-result-prefixes","xsl msxsl ddwrt");
                mgr.AddNamespace("designer","http://schemas.microsoft.com/WebParts/v2/DataView/designer");
                mgr.AddNamespace("ddwrt2", "urn:frontpage:internal");
                mgr.AddNamespace("s", "uuid:BDC6E3F0-6DA3-11d1-A2A3-00AA00C14882");
                mgr.AddNamespace("dt", "uuid:C2F41010-65B3-11d1-A29F-00AA00C14882");
                mgr.AddNamespace("rs", "urn:schemas-microsoft-com:rowset");
                mgr.AddNamespace("z", "#RowsetSchema");

                DirectoryInfo info = new DirectoryInfo(MainXslFolderPath);
                string searchPattern = prefix + "*.xsl";
                string b = prefix + ".xsl";
                FileInfo[] files = info.GetFiles(searchPattern);
                XmlNode refChild = null;
                XmlNode parentNode = null;
                XmlNodeList list = xmlDocument.SelectNodes("/xsl:stylesheet/xsl:import | /xsl:stylesheet/xsl:include", mgr);
                if ((list != null) && (list.Count > 0))
                {
                    refChild = list[list.Count - 1];
                    parentNode = refChild.ParentNode;
                }
                else
                {
                    parentNode = xmlDocument.SelectSingleNode("/xsl:stylesheet", mgr);
                }
                foreach (FileInfo info2 in files)
                {
                    if (!string.Equals(info2.Name, b, StringComparison.OrdinalIgnoreCase))
                    {
                        XmlNode newChild = xmlDocument.CreateNode(XmlNodeType.Element, "xsl", "include", "http://www.w3.org/1999/XSL/Transform");
                        XmlAttribute node = xmlDocument.CreateAttribute("href");
                        node.Value = "/_layouts/xsl/" + info2.Name;
                        newChild.Attributes.SetNamedItem(node);
                        parentNode.InsertAfter(newChild, refChild);
                    }
                }
            }
            catch
            {
                //throw;
            }
        }

        internal static void PreProcessCustomXSL(XmlDocument xmlDocument)
        {
            try
            {
                SPWeb root = SPContext.Current.Site.RootWeb;
                SPFolder xsl = root.GetFolder(StyleXslFolderPath);
                if (xsl.Exists)
                {
                    XmlNamespaceManager mgr = new XmlNamespaceManager(xmlDocument.NameTable);
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

                    SPFileCollection files = xsl.Files;
                    XmlNode refChild = null;
                    XmlNode parentNode = null;

                    XmlNodeList list = xmlDocument.SelectNodes("/xsl:stylesheet/xsl:import | /xsl:stylesheet/xsl:include", mgr);
                    if ((list != null) && (list.Count > 0))
                    {
                        refChild = list[list.Count - 1];
                        parentNode = refChild.ParentNode;
                    }
                    else
                    {
                        parentNode = xmlDocument.SelectSingleNode("/xsl:stylesheet", mgr);
                    }
                    foreach (SPFile file in files)
                    {
                        XmlNode newChild = xmlDocument.CreateNode(XmlNodeType.Element, "xsl", "include", "http://www.w3.org/1999/XSL/Transform");
                        XmlAttribute node = xmlDocument.CreateAttribute("href");
                        node.Value = file.ServerRelativeUrl;
                        newChild.Attributes.SetNamedItem(node);
                        parentNode.InsertAfter(newChild, refChild);
                    }
                }
            }
            catch
            {
                //throw;
            }

        }

		internal static XmlSerializer SerialiserFor(Type t)
		{
			XmlSerializer result;
			if (!serialisers.TryGetValue(t, out result))
			{
				lock (serialisers)
				{
					result = (t.Name == "MenuXml")
					         	? new XmlSerializer(t, new XmlRootAttribute {ElementName = "Root"})
					         	: new XmlSerializer(t);
					if (!serialisers.ContainsKey(t))
					{
						serialisers.Add(t, result);
					}
				}
			}
			return result;
		}

		public static string ConvertToJs(object obj)
		{
			string result;

			if (obj == null)
			{
				return "null";
			}

			var objType = obj.GetType();
			if (objType == typeof(bool))
			{
				result = (bool)obj ? "true" : "false";
			}
			else if (objType == typeof(int) || objType == typeof(decimal) || objType == typeof(double))
			{
				result = obj.ToString();
			}
			else
			{
				result = String.Format("\"{0}\"", obj.ToString().Replace("\"", "\\\""));
			}

			return result;
		}

        public static string SerializeObject(object obj)
        {
            var s = new JavaScriptSerializer();

            return s.Serialize(obj);
        }

        public static T DeserializeObject<T>(string json)
        {
            var s = new JavaScriptSerializer();
            return s.Deserialize<T>(json);
        }
	}
}