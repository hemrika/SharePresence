// -----------------------------------------------------------------------
// <copyright file="HTML5VideoPropertyBag.cs" company="">
// TODO: Update copyright text.
// </copyright>
// -----------------------------------------------------------------------

namespace Hemrika.SharePresence.WebSite.FieldTypes
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;

    /// <summary>
    /// TODO: Update summary.
    /// </summary>
    public class HTML5VideoPropertyBag
    {
        public Guid ListId { get; set; }
        public Guid WebId { get; set; }
        public Guid ItemId { get; set; }
        public string QueryBox { get; set; }
        
        public HTML5VideoPropertyBag()
        {
            ListId = Guid.Empty;
            WebId = Guid.Empty;
            ItemId = Guid.Empty;
            QueryBox = string.Empty;
        }

        public HTML5VideoPropertyBag(string value)
        {
            if (!string.IsNullOrEmpty(value))
            {
                string[] tokens = value.Split(';');
                this.ListId = new Guid(tokens[0]);
                this.WebId = new Guid(tokens[1]);
                this.ItemId = new Guid(tokens[2]);
                this.QueryBox = tokens[3];
            }
        }
        
        public override string ToString()
        {
            //return ListId.ToString() + ";" + FieldId.ToString() + ";" + _searchFields + ";" + MaxSearchResults +  ";" + EntityEditorRows + ";" + WebId.ToString();
            return ListId.ToString() + ";" + WebId.ToString() + ";" + ItemId.ToString() + ";" + QueryBox.ToString();
        }
    }
}