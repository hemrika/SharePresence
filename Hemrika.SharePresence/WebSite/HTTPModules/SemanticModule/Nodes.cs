// -----------------------------------------------------------------------
// <copyright file="Nodes.cs" company="">
// TODO: Update copyright text.
// </copyright>
// -----------------------------------------------------------------------

namespace Hemrika.SharePresence.WebSite.Modules.SemanticModule
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;

    [Serializable]
    public class SemanticUrl : IComparable<SemanticUrl>
    {
        private Guid id;
        public Guid Id
        {
            get { return id; }
            set { id = value; }
        }

        private bool disabled;
        public bool Disabled
        {
            get { return disabled; }
            set { disabled = value; }
        }

        private string semantic;
        public string Semantic
        {
            get { return semantic; }
            set { semantic = value; }
        }

        private string originalUrl;
        public string OriginalUrl
        {
            get { return originalUrl; }
            set { originalUrl = value; }
        }

        public static Comparison<SemanticUrl> UrlComparison = delegate(SemanticUrl p1, SemanticUrl p2)
        {
            return p1.Semantic.CompareTo(p2.Semantic);
        };

        public int CompareTo(SemanticUrl other)
        {
            return Semantic.CompareTo(other.Semantic);
        }

    }

}
