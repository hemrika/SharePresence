// -----------------------------------------------------------------------
// <copyright file="DesignWebPartDataSource.cs" company="">
// TODO: Update copyright text.
// </copyright>
// -----------------------------------------------------------------------

namespace Hemrika.SharePresence.WebSite.WebParts
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using Hemrika.SharePresence.Common.TemplateEngine;

    /// <summary>
    /// 
    /// </summary>
    [Serializable]
    public class DesignDataSource
    {
        /*
        /// <summary>
        /// Content container id
        /// </summary>
        public string DynamicContentContainerId { get; set; }
        */

        /// <summary>
        /// Uid
        /// </summary>
        public string Id { get; set; }

        /// <summary>
        /// SharePoint list id
        /// </summary>
        public string ListId { get; set; }

        /// <summary>
        /// SharePoint Role Inheritance changed
        /// </summary>
        public bool ListInheritanceChange { get; set; }

        /// <summary>
        /// SharePoint Role changed
        /// </summary>
        public bool ListRoleChange { get; set; }

        /*
        /// <summary>
        /// Container Id
        /// </summary>
        public string PagerContainerId { get; set; }

        /// <summary>
        /// Pager link name
        /// </summary>
        public string PagerDisplayName { get; set; }

        /// <summary>
        /// Pager style
        /// 0 - No pager
        /// 1 - Replacement
        /// 2 - Adding
        /// 3 - Auto loading
        /// </summary>
        public int PagerStyle { get; set; }
        */

        /// <summary>
        /// CAML query
        /// </summary>
        public string Query { get; set; }

        /// <summary>
        /// Recursive Scope
        /// </summary>
        public bool RecursiveScope { get; set; }

        /// <summary>
        /// Enabled client caching for data source
        /// </summary>
        public bool? CacheOnClient { get; set; }

        /// <summary>
        /// Query row limit
        /// </summary>
        public int RowLimit { get; set; }

        /// <summary>
        /// Data source title
        /// </summary>
        public string Title { get; set; }

        /// <summary>
        /// View Fields Parameter
        /// </summary>
        public string ViewFields { get; set; }

        /// <summary>
        /// View Id
        /// </summary>
        public string ViewId { get; set; }

        /// <summary>
        /// Web url address
        /// </summary>
        public string WebId { get; set; }

        /// <summary>
        /// Last modified date
        /// </summary>
        public string Modified { get; set; }

        public string ConvertToJson()
        {
            var result = new StringBuilder();
            result.Append("{");

            string serialized = Utilities.SerializeObject(this);

            serialized = serialized.Trim(new char[2] { '{', '}' });
            string[] parameters = serialized.Split(',');

            int index = parameters.Length;
            int current = 1;
            foreach (string param in parameters)
            {

                string[] keyval = param.Split(':');
                if (index != current)
                {
                    result.AppendFormat("{0}:{1},", keyval[0], keyval[1]);
                }
                else
                {
                    result.AppendFormat("{0}:{1}", keyval[0], keyval[1]);
                }
                current += 1;
            }
            
            /*
            if (this != null)
            {
                result.AppendFormat("{0}:{1},", "Id", Id);
            }
            */
            result.Append("}");
            return result.ToString();
        }

    }
}
