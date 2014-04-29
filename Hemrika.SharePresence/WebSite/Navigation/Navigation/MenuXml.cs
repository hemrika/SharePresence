// -----------------------------------------------------------------------
// <copyright file="MenuXml.cs" company="">
// TODO: Update copyright text.
// </copyright>
// -----------------------------------------------------------------------

namespace Hemrika.SharePresence.WebSite.Navigation
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Xml.Serialization;

    [Serializable]
    [XmlRoot("xmlroot", Namespace = "")]
    public class MenuXml
    {
        public WebSiteNode root { get; set; }
    }
}