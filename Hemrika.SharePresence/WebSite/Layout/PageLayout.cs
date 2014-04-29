// -----------------------------------------------------------------------
// <copyright file="PageLayout.cs" company="Microsoft">
// TODO: Update copyright text.
// </copyright>
// -----------------------------------------------------------------------

namespace Hemrika.SharePresence.WebSite.Layout
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using Microsoft.SharePoint;

    /// <summary>
    /// TODO: Update summary.
    /// </summary>
    public class PageLayout
    {
        private Guid uniqueId;

        public Guid UniqueId
        {
            get { return uniqueId; }
            set { uniqueId = value; }
        }

        private int id;

        public int Id
        {
            get { return id; }
            set { id = value; }
        }

        private string name;

        public string Name
        {
            get { return name; }
            set { name = value; }
        }

        private string title;

        public string Title
        {
            get { return title; }
            set { title = value; }
        }

        private string description;

        public string Description
        {
            get
            {
                if (String.IsNullOrEmpty(description))
                {
                    return string.Empty;
                }
                else
                {
                    return description;
                }
            }
            set { description = value; }
        }

        private string url;

        public string Url
        {
            get { return url; }
            set { url = value; }
        }

        private string icon;
        public string Icon { get { return icon; } set { icon = value; } }

        private string preview;
        public string Preview { get { return preview; } set { preview = value; } }

        private string _class;
        public string Class { get { return _class; } set { _class = value; } }

        private SPContentTypeId contenTypeId;

        public SPContentTypeId ContenTypeId
        {
            get { return contenTypeId; }
            set { contenTypeId = value; }
        }

        private string contentTypeGroup;

        public string ContentTypeGroup
        {
            get { return contentTypeGroup; }
            set { contentTypeGroup = value; }
        }

        #region "Methods"
        public static PageLayout CreatePageLayout()
        {
            return new PageLayout();
        }

        #endregion


        
    }
}
