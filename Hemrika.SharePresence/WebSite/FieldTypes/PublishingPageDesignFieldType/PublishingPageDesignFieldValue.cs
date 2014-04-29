// -----------------------------------------------------------------------
// <copyright file="PublishingLayoutFieldValue.cs" company="">
// TODO: Update copyright text.
// </copyright>
// -----------------------------------------------------------------------

namespace Hemrika.SharePresence.WebSite.FieldTypes
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using Microsoft.SharePoint;
    using Hemrika.SharePresence.WebSite.Layout;

    /// <summary>
    /// TODO: Update summary.
    /// </summary>
    public class PublishingPageDesignFieldValue: SPFieldMultiColumnValue
    {
        private const int NumberOfFields = 3;

        public PublishingPageDesignFieldValue() : base(3)
        {
        }

        public PublishingPageDesignFieldValue(PageLayout layout) : base(3)
        {
            base[0] = layout.UniqueId.ToString();
            base[1] = layout.Title;
            base[2] = layout.Url;
        }

        public PublishingPageDesignFieldValue(string value)
            : base(value)
        {
        }

        public Guid Id
        {
            get
            {
                return new Guid(base[0]);
            }
            set
            {
                base[0] = value.ToString();
            }
        }

        public bool IsEmpty
        {
            get
            {
                return string.IsNullOrEmpty(base[0]);
            }
        }

        internal string Title
        {
            get
            {
                return base[1];
            }
            set
            {
                base[1] = value;
            }
        }

        internal string Url
        {
            get
            {
                return base[2];
            }
            set
            {
                base[2] = value;
            }
        }
    }
}
