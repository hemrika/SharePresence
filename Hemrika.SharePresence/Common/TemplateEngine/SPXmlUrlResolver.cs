// -----------------------------------------------------------------------
// <copyright file="SPXmlUrlResolver.cs" company="">
// TODO: Update copyright text.
// </copyright>
// -----------------------------------------------------------------------

namespace Hemrika.SharePresence.Common.TemplateEngine
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Xml;
    using Microsoft.SharePoint;
    using Microsoft.SharePoint.Utilities;
    using System.Security.Policy;
    using System.Security;
    using Microsoft.SharePoint.Security;
    using System.Security.Permissions;
    using System.IO;
    using System.Xml.XPath;

    [SharePointPermission(SecurityAction.InheritanceDemand, ObjectModel = true), SharePointPermission(SecurityAction.LinkDemand, ObjectModel = true)]
    public class SPXmlUrlResolver : XmlUrlResolver //XmlSecureResolver
    {
        /*
        public SPXmlUrlResolver() : base(new XmlUrlResolver(), SPContext.Current.Site.Url) { }
        public SPXmlUrlResolver(string securityUrl) : base(new XmlUrlResolver(), securityUrl) { }
        public SPXmlUrlResolver(XmlResolver resolver, Evidence evidence) : base(resolver, evidence) { }
        public SPXmlUrlResolver(XmlResolver resolver, PermissionSet permissionSet) : base(resolver, permissionSet) { }
        public SPXmlUrlResolver(XmlResolver resolver, string securityUrl) : base(resolver, securityUrl) { }
*/
        public override Uri ResolveUri(Uri baseUri, string relativeUri)
        {
            if ((relativeUri != null) && relativeUri.StartsWith("/_layouts/"))
            {

                string uriString = SPUtility.GetVersionedGenericSetupPath("template\\layouts",15) + @"\";// .GetGenericSetupPath(@"template\layouts") + @"\";
                Uri uri = new Uri(uriString);
                uriString = uriString + relativeUri.Substring("/_layouts/".Length);
                Uri uri2 = new Uri(uriString);
                if (!uri.IsBaseOf(uri2))
                {
                    throw new InvalidOperationException();
                }
                return uri2;
            }
            if ((relativeUri != null) && relativeUri.StartsWith("~/"))
            {
                string genericSetupPath = SPUtility.GetVersionedGenericSetupPath("template\\",15); //SPUtility.GetGenericSetupPath(@"template\");
                Uri uri3 = new Uri(genericSetupPath);
                genericSetupPath = genericSetupPath + relativeUri.Substring(2);
                Uri uri4 = new Uri(genericSetupPath);
                if (!uri3.IsBaseOf(uri4))
                {
                    throw new InvalidOperationException();
                }
                return uri4;
            }

            if ((relativeUri != null) && relativeUri.StartsWith("/Style Library/"))
            {
                SPWeb root = SPContext.Current.Site.RootWeb;
                SPList library = root.Lists.TryGetList("Style Library");
                string[] folders = relativeUri.Split(new char[1] { '/' });
                SPFolder folder = library.RootFolder;

                foreach (string subfolder in folders)
                {
                    if ((!String.IsNullOrEmpty(subfolder)) && (folder.Name != subfolder && folder.SubFolders.Count > 0))
                    {
                        SPFolder afolder = folder.SubFolders[subfolder];
                        if (afolder.Exists)
                        {
                            folder = afolder;
                        }
                    }
                }

                //string subfolder = relativeUri.Replace("/Style Library", string.Empty);
                //SPFolder folder = library.RootFolder.SubFolders[subfolder];
                if (folder.Exists)
                {
                    SPFile file = folder.Files[relativeUri];
                    if (file.Exists)
                    {
                        string full = SPUtility.ConcatUrls(root.Url, file.ServerRelativeUrl);
                        //full = SPHttpUtility.UrlPathEncode(full, false);
                        Uri fullUri = new Uri(full);
                        return fullUri;
                    }
                }
                /*
                else
                {
                    string full = SPUtility.ConcatUrls(root.Url, library.RootFolder.Url);
                    //Uri fullUri = new Uri(full);
                    return fullUri;                   
                }
                */
            }
            return base.ResolveUri(baseUri, relativeUri);

        }

        [SharePointPermission(SecurityAction.Demand, ObjectModel = true)]
        public override object GetEntity(Uri absoluteUri, string role, Type ofObjectToReturn)
        {
            if ((absoluteUri != null) && ((absoluteUri.Scheme == Uri.UriSchemeHttp) || (absoluteUri.Scheme == Uri.UriSchemeHttps)))
            {
                try
                {
                    string output = null;
                    SPFile file = SPContext.Current.Site.RootWeb.GetFile(absoluteUri.AbsoluteUri);
                    if (!file.Exists)
                    {
                        return base.GetEntity(absoluteUri, role, ofObjectToReturn);
                    }
                    else
                    {
                        output = SPContext.Current.Site.RootWeb.GetFileAsString(absoluteUri.AbsoluteUri);
                        /*
                        if ((this._hashTable != null) && (this.Part.Web != null))
                        {
                            this._hashTable[absoluteUri.AbsoluteUri] = new Pair(time, num);
                        }
                        */
                        //Stream stream = f.OpenBinaryStream(SPOpenBinaryOptions.None);
                        MemoryStream stream = new MemoryStream();
                        StreamWriter writer = new StreamWriter(stream);
                        writer.Write(output);
                        writer.Flush();
                        stream.Position = 0L;
                        return stream;
                    }
                }
                catch (Exception)
                {
                    return base.GetEntity(absoluteUri, role, ofObjectToReturn);
                }
            }

            if (((absoluteUri != null) && (absoluteUri.LocalPath != null)) && (string.Compare(absoluteUri.LocalPath, 0, Utilities.MainXslFolderPath, 0, Utilities.MainXslFolderPath.Length, StringComparison.OrdinalIgnoreCase) == 0))
            {
                XPathNavigator navigator = this.ConvertGhostedXsl(absoluteUri.LocalPath);
                if (navigator != null)
                {
                    return navigator;
                }
            }
            
            return base.GetEntity(absoluteUri, role, ofObjectToReturn);
        }

        private XPathNavigator ConvertGhostedXsl(string filePath)
        {
            bool isFldType = filePath.EndsWith(Path.AltDirectorySeparatorChar + "fldtypes.xsl", StringComparison.OrdinalIgnoreCase) || filePath.EndsWith(Path.DirectorySeparatorChar + "fldtypes.xsl", StringComparison.OrdinalIgnoreCase);
            if (!isFldType)
            {
                return null;
            }

            XmlDocument dom = new XmlDocument();
            using (XmlTextReader reader = new XmlTextReader(filePath))
            {
                reader.DtdProcessing = DtdProcessing.Prohibit;
                //reader.ProhibitDtd = true;
                dom.Load(reader);
            }
            return this.ConvertGhostedXsl(dom, isFldType);
        }

        internal XPathNavigator ConvertGhostedXsl(XmlDocument dom, bool isFldType)
        {
            try
            {
                XmlDocument xmlDocument = dom;
                XmlNamespaceManager mgr = new XmlNamespaceManager(xmlDocument.NameTable);
                mgr.AddNamespace("ddwrt", "http://schemas.microsoft.com/WebParts/v2/DataView/runtime");
                mgr.AddNamespace("xsl", "http://www.w3.org/1999/XSL/Transform");
                if (xmlDocument.SelectSingleNode("/xsl:stylesheet[@ddwrt:oob='true']", mgr) != null)
                {
                    /*
                    if (lvwp.IsGhosted)
                    {
                        this.ConvertHtmlTags();
                    }
                    */
                    if (isFldType)
                    {
                        Utilities.PreProcessCachedXSL(xmlDocument,"fldtypes");
                        Utilities.PreProcessCachedXSL(xmlDocument, "vwstyles");
                        /*
                        if (lvwp.IsGhosted)
                        {
                            this.ConvertLinkTitleTemplate("xsl:apply-templates");
                            this.CacheDuplicateDispEx();
                            this.ConvertTemplateWithTag();
                        }
                        */
                    }
                    return xmlDocument.CreateNavigator();
                }
                return null;
            }
            catch
            {
                return null;
            }
        }
    }
}
