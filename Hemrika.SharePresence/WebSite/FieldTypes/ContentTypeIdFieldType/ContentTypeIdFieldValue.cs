// -----------------------------------------------------------------------
// <copyright file="ContentTypeIdFieldValue.cs" company="">
// TODO: Update copyright text.
// </copyright>
// -----------------------------------------------------------------------

namespace Hemrika.SharePresence.WebSite
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using Microsoft.SharePoint;

    /// <summary>
    /// TODO: Update summary.
    /// </summary>
    public class ContentTypeIdFieldValue : SPFieldMultiColumnValue
    {
        private const int NumberOfFields = 3;

        public ContentTypeIdFieldValue() : base(3)
        {
        }

        public ContentTypeIdFieldValue(SPContentType contentType) : base(3)
        {
            base[0] = contentType.Group;
            base[1] = contentType.Id.ToString();
            base[2] = contentType.Name;
        }

        public ContentTypeIdFieldValue(string value) : base(value)
        {
        }

        public SPContentTypeId Id
        {
            get
            {
                return new SPContentTypeId(base[1]);
            }
        }

        public bool IsEmpty
        {
            get
            {
                return string.IsNullOrEmpty(base[1]);
            }
        }

        internal string StoredName
        {
            get
            {
                return base[2];
            }
        }

        internal string StoredGroup
        {
            get
            {
                return base[0];
            }
        }
    }
}
