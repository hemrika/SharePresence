// -----------------------------------------------------------------------
// <copyright file="PageLayout.cs" company="Microsoft">
// TODO: Update copyright text.
// </copyright>
// -----------------------------------------------------------------------

namespace Hemrika.SharePresence.WebSite.Master
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;

    /// <summary>
    /// TODO: Update summary.
    /// </summary>
    public class MasterPage
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
            get { return description; }
            set { description = value; }
        }

        private string url;

        public string Url
        {
            get { return url; }
            set { url = value; }
        }

        #region "Methods"
        public static MasterPage CreateMasterPage()
        {
            return new MasterPage();
        }

        #endregion

    }
}
