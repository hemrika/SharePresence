// -----------------------------------------------------------------------
// <copyright file="HtmlAttributeWriter.cs" company="">
// TODO: Update copyright text.
// </copyright>
// -----------------------------------------------------------------------

namespace Hemrika.SharePresence.WebSite.Adapters
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Web.UI;
    using System.IO;

    /// <summary>
    /// TODO: Update summary.
    /// </summary>
    public class HtmlAttributeWriter : HtmlTextWriter
    {
        private IDictionary<string, string> attributes;

        public HtmlAttributeWriter(TextWriter writer, IDictionary<string, string> attributes)
            : base(writer)
        {
            base.InnerWriter = writer;
            this.attributes = attributes;
        }

        public HtmlAttributeWriter(HtmlTextWriter writer, IDictionary<string, string> attributes)
            : base(writer)
        {
            base.InnerWriter = writer.InnerWriter;
            this.attributes = attributes;
        }

        public override void WriteAttribute(string key, string value, bool fEncode)
        {
            if (this.attributes.ContainsKey(key))
            {
                value = this.attributes[key];
                this.attributes.Remove(key);
            }
            base.WriteAttribute(key, value, fEncode);
        }
    }
}

