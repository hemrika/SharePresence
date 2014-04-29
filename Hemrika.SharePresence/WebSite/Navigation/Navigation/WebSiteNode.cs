// -----------------------------------------------------------------------
// <copyright file="NavigationNode.cs" company="">
// TODO: Update copyright text.
// </copyright>
// -----------------------------------------------------------------------

namespace Hemrika.SharePresence.WebSite.Navigation
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Web;
    using System.Web.UI;
    using System.Xml.Serialization;
    using System.Xml;
    using System.Collections;
    using Microsoft.SharePoint;
    using System.Runtime.Serialization;

    [DataContract(IsReference=true, Name="WebSiteNode", Namespace="Hemrika.SharePresence.WebSite.Navigation")]
    [Serializable]
    [XmlRoot("root", Namespace = "Hemrika.SharePresence.WebSite.Navigation")]
    public class WebSiteNode : ICloneable, IHierarchyData, INavigateUIData, IXmlSerializable
    {
        private XmlDocument _XMLDoc;

        public XmlDocument XmlDoc
        {
            get { return _XMLDoc; }
            set { _XMLDoc = value; }
        }
        private System.Xml.XmlNode _XMLNode;

        public System.Xml.XmlNode XmlNode
        {
            get { return _XMLNode; }
            set { _XMLNode = value; }
        }

        [DataMember]
        public int Id { get; set; }
        [DataMember]
        public string Key { get; set; }
        [DataMember]
        public string Keywords { get; set; }
        //[DataMember]
        public string Description { get; set; }
        //[DataMember]
        public string Title { get; set; }
        //[DataMember]
        public string Url { get; set; }
        //[DataMember]
        public bool Enabled { get; set; }
        //[DataMember]
        public bool Selected { get; set; }
        //[DataMember]
        public bool Breadcrumb { get; set; }
        //[DataMember]
        public bool Separator { get; set; }
        //[DataMember]
        public string Icon { get; set; }
        //[DataMember]
        public string LargeImage { get; set; }
        //[DataMember]
        public string CommandName { get; set; }
        //[DataMember]
        public string CommandArgument { get; set; }
        //[DataMember]
        public System.Collections.SortedList ExtendedAttributes { get; set; }
        //[DataMember]
        public bool First { get { return (Parent == null) || (Parent.Children[0] == this); } }
        //[DataMember]
        public bool Last { get { return (Parent == null) || (Parent.Children[Parent.Children.Count - 1] == this); } }
        //[DataMember]
        public int Depth
        {
            get
            {
                var result = -1;
                var current = this;
                while (current.Parent != null)
                {
                    result++;
                    current = current.Parent;
                }
                return result;
            }
        }
        //[DataMember]
        public string Name
        {
            get;
            set;
        }
        //[DataMember]
        public string NavigateUrl
        {
            get;
            set;
        }
        //[DataMember]
        public string Value
        {
            get;
            set;
        }
        //[DataMember]
        public object Item
        {
            get;
            set;
        }
        //[DataMember]
        public string Path
        {
            get;
            set;
        }
        //[DataMember]
        public string Type
        {
            get;
            set;
        }

        private List<WebSiteNode> _Children;
        private WebSiteNode node;
        //[DataMember]
        public List<WebSiteNode> Children { get { return _Children ?? (_Children = new List<WebSiteNode>()); } set { _Children = value; } }

        public WebSiteNode Parent { get; set; }

        public WebSiteNode() : this(new XmlDocument().CreateNode(XmlNodeType.Element, "n", ""))
        {
        }

        public WebSiteNode(List<WebSiteNode> nodes)
        {
            Children = nodes;
            Children.ForEach(c => c.Parent = this);
        }

        public WebSiteNode(WebSiteNode node)
        {
            this.node = node;
        }

        public WebSiteNode(SPListItem item)
        {

        }

        public WebSiteNode(string NodeText, string navigateUrl)
        {
            if ((NodeText == null) || (navigateUrl == null))
            {
                throw new ArgumentNullException();
            }
            this.Title = NodeText;
            this.NavigateUrl = navigateUrl;
        }

        public WebSiteNode(System.Xml.XmlNode objXmlNode)
        {
            this.XmlNode = objXmlNode;
            this.XmlDoc = objXmlNode.OwnerDocument;
        }

        public WebSiteNode FindById(int tabId)
        {
            if (tabId == Id)
            {
                return this;
            }

            foreach (var child in Children)
            {
                var result = child.FindById(tabId);
                if (result != null)
                {
                    return result;
                }
            }

            return null;
        }

        public WebSiteNode FindByName(string tabName)
        {
            if (tabName.Equals(Title, StringComparison.InvariantCultureIgnoreCase))
            {
                return this;
            }

            foreach (var child in Children)
            {
                var result = child.FindByName(tabName);
                if (result != null)
                {
                    return result;
                }
            }

            return null;
        }

        public WebSiteNode FindByNameOrId(string tabNameOrId)
        {
            if (tabNameOrId.Equals(Title, StringComparison.InvariantCultureIgnoreCase))
            {
                return this;
            }
            if (tabNameOrId == Id.ToString())
            {
                return this;
            }

            foreach (var child in Children)
            {
                var result = child.FindByNameOrId(tabNameOrId);
                if (result != null)
                {
                    return result;
                }
            }

            return null;
        }

        public bool HasChildren()
        {
            return (Children.Count > 0);
        }

        public IHierarchyData GetParent()
        {
            return this.Parent;
        }


        public System.Xml.Schema.XmlSchema GetSchema()
        {
            throw new NotImplementedException();
        }


        bool IHierarchyData.HasChildren
        {
            get { return (this.Children.Count > 0); }
        }


        public object Clone()
        {
            WebSiteNode cnode = (WebSiteNode)this.MemberwiseClone();
            if (this.ExtendedAttributes != null)
            {
                cnode.ExtendedAttributes = (SortedList)this.ExtendedAttributes.Clone();
            }
            if (this.XmlDoc != null)
            {
                cnode.XmlDoc = (XmlDocument)this.XmlDoc.Clone();
            }
            return cnode;

            //return this.Clone(true);
        }



        public WebSiteNode Clone(bool blnDeep)
        {
            WebSiteNode cnode = (WebSiteNode)this.MemberwiseClone();
            if (this.ExtendedAttributes != null)
            {
                cnode.ExtendedAttributes = (SortedList)this.ExtendedAttributes.Clone();
            }
            if (this.XmlDoc != null)
            {
                cnode.XmlDoc = (XmlDocument)this.XmlDoc.Clone();
            }
            return cnode;

            //return new WebSiteNode(this);
        }

        public IHierarchicalEnumerable GetChildren()
        {
            if (HasChildren())
            {
                return this.Children as IHierarchicalEnumerable;
            }
            else
            {
                return null;
            }
        }

        public void ReadXml(XmlReader reader)
        {
            try
            {
                var empty = reader.IsEmptyElement;

                if (reader.Name == "row")
                {
                    XmlReader valueReader = reader.ReadSubtree();
                    while (valueReader.Read())
                    {
                        XmlNodeType nType = valueReader.NodeType;
                        string nName = valueReader.Name;
                        string nValue = valueReader.Value;
                        Type type = valueReader.ValueType;

                        switch (valueReader.NodeType)
                        {
                            case XmlNodeType.Element:
                                {
                                    switch (valueReader.Name.ToLowerInvariant())
                                    {
                                        case "id":
                                            valueReader.Read();
                                            Id = int.Parse(reader.Value);
                                            //Id = reader.ReadElementContentAsInt();
                                            break;
                                        case "uniqueid":
                                            valueReader.Read();
                                            Key = reader.Value;
                                            //Key = reader.ReadElementContentAsString().Trim();
                                            break;
                                        case "description":
                                            valueReader.Read();
                                            Description = reader.Value;
                                            break;
                                        case "title":
                                            valueReader.Read();
                                            Title = reader.Value;
                                            //Title = reader.ReadElementContentAsString().Trim();//valueReader.Value;
                                            break;
                                        case "url":
                                            valueReader.Read();
                                            string[] _url = reader.Value.Split(new char[1]{','});
                                            Url = _url[0];
                                            Description = _url[1];
                                            //Url = reader.ReadElementContentAsString().Trim();
                                            break;
                                        /*
                                        case "parent":
                                            Parent = reader.ReadElementContentAsString().Trim();
                                            break;
                                        */
                                        case "enabled":
                                            //valueReader.ReadElementContentAsBoolean();
                                            valueReader.Read();
                                            bool boolenabled = false;
                                            if (bool.TryParse(reader.Value, out boolenabled))
                                            {
                                                Enabled = boolenabled;// bool.Parse(reader.Value);
                                            }
                                            if (reader.Value == "1")
                                            {
                                                boolenabled = true;
                                            }
                                            Enabled = boolenabled;
                                            break;
                                        case "selected":
                                            valueReader.Read();
                                            Selected = bool.Parse(reader.Value);
                                            break;
                                        case "breadcrumb":
                                            valueReader.Read();
                                            Breadcrumb = bool.Parse(reader.Value);
                                            break;
                                        case "separator":
                                            valueReader.Read();
                                            Separator = bool.Parse(reader.Value);
                                            break;
                                        case "icon":
                                            valueReader.Read();
                                            Icon = reader.Value;
                                            break;
                                        case "largeimage":
                                            valueReader.Read();
                                            LargeImage = reader.Value;
                                            break;
                                        case "commandname":
                                            valueReader.Read();
                                            CommandName = reader.Value;
                                            break;
                                        case "commandargument":
                                            valueReader.Read();
                                            CommandArgument = reader.Value;
                                            break;
                                        case "row":
                                            break;
                                        /*
                                        default:
                                            string _name = valueReader.Name;
                                            valueReader.Read();
                                            string _value = valueReader.Value;
                                            if (ExtendedAttributes == null)
                                            {
                                                ExtendedAttributes = new SortedList();
                                            }

                                            ExtendedAttributes.Add(_name,_value);
                                            break;
                                        */
                                    }
                                    break;
                                }
                        }
                    }
                }

                /*
                if (empty)
                {
                    return;
                }
                */

                while (reader.Read())
                {
                    switch (reader.NodeType)
                    {
                        case XmlNodeType.Element:
                            switch (reader.Name.ToLowerInvariant())
                            {
                                case "rows":
                                    ReadXml(reader);
                                    break;
                                case "row":
                                    WebSiteNode child = new WebSiteNode { Parent = this };
                                        child.ReadXml(reader);
                                        Children.Add(child);
                                    break;
                             /*
                                case "keywords":
                                    Keywords = reader.ReadElementContentAsString().Trim();
                                    break;
                                case "description":
                                    Description = reader.ReadElementContentAsString().Trim();
                                    break;
                             */
                                default:
                                    throw new XmlException(String.Format("Unexpected element '{0}'", reader.Name));
                            }                            break;
                        case XmlNodeType.EndElement:
                            reader.ReadEndElement();
                            break;
                    }
                }
            }
            catch (Exception ex)
            {
                ex.ToString();
                throw;
            }
        }

        public void WriteXml(XmlWriter writer)
        {
            if (Parent != null)
            {
                writer.WriteStartElement("node");

                AddXmlAttribute(writer, "id", Id);
                AddXmlAttribute(writer, "title", Title);
                AddXmlAttribute(writer, "url", Url);
                AddXmlAttribute(writer, "enabled", Enabled);
                AddXmlAttribute(writer, "selected", Selected);
                AddXmlAttribute(writer, "breadcrumb", Breadcrumb);
                AddXmlAttribute(writer, "separator", Separator);
                AddXmlAttribute(writer, "icon", Icon);
                AddXmlAttribute(writer, "largeimage", LargeImage);
                AddXmlAttribute(writer, "commandname", CommandName);
                AddXmlAttribute(writer, "commandargument", CommandArgument);
                AddXmlAttribute(writer, "first", First);
                AddXmlAttribute(writer, "last", Last);
                AddXmlAttribute(writer, "only", First && Last);
                AddXmlAttribute(writer, "depth", Depth);
                AddXmlAttribute(writer, "keywords", Keywords);
                AddXmlAttribute(writer, "description", Description);

                /*
                if (ExtendedAttributes != null)
                {
                    foreach (DictionaryEntry extend in ExtendedAttributes)
                    {
                        AddXmlAttribute(writer, extend.Key.ToString(), extend.Value.ToString());
                    }
                }
                */
            }

            Children.ForEach(c => c.WriteXml(writer));

            if (Parent != null)
            {
                writer.WriteEndElement();
            }
        }

        private static void AddXmlAttribute(XmlWriter writer, string name, string value)
        {
            if (!String.IsNullOrEmpty(value))
            {
                writer.WriteAttributeString(name, value);
            }
        }

        private static void AddXmlAttribute(XmlWriter writer, string name, int value)
        {
            writer.WriteAttributeString(name, value.ToString());
        }

        private static void AddXmlAttribute(XmlWriter writer, string name, bool value)
        {
            writer.WriteAttributeString(name, value ? "1" : "0");
        }

        private static void AddXmlElement(XmlWriter writer, string name, string value)
        {
            if (!String.IsNullOrEmpty(value))
            {
                writer.WriteElementString(name, value);
            }
        }

        /*
        internal XmlDocument XMLDoc
        {
            get
            {
                return this._XMLDoc;
            }
            set
            {
                this._XMLDoc = value;
            }
        }

        internal System.Xml.XmlNode XmlNode
        {
            get
            {
                return this._XMLNode;
            }
            set
            {
                this._XMLNode = value;
            }
        }

        public XmlNodeList XmlNodes
        {
            get
            {
                return this.XmlNode.ChildNodes;
            }
        }
        */
        //[DataMember]
        public WebSiteNodeCollection ChildNodes { get; set; }
        //[DataMember]
        public WebSiteNode ParentNode { get; set; }

        internal bool IsDescendantOf(WebSiteNode currentNode)
        {
            throw new NotImplementedException();
        }
    }
}