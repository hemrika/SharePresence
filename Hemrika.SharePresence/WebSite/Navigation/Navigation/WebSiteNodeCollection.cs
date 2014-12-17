// -----------------------------------------------------------------------
// <copyright file="NavigationNodeCollection.cs" company="">
// TODO: Update copyright text.
// </copyright>
// -----------------------------------------------------------------------

namespace Hemrika.SharePresence.WebSite.Navigation
{
    using System;
    using System.Collections.Generic;
    using System.Web;
    using System.Collections;
    using System.Web.UI;

    /// <summary>
    /// TODO: Update summary.
    /// </summary>
    public class WebSiteNodeCollection : IHierarchicalEnumerable, IList, ICollection, IEnumerable
    {
        private List<WebSiteNode> _innerList = null;

        private List<WebSiteNode> InnerList
        {
            get
            {
                if (_innerList == null)
                {
                    _innerList = new List<WebSiteNode>();
                }
                return _innerList;
            }
            set { _innerList = value; }
        }

        public static WebSiteNodeCollection Empty;
        private WebSiteNode node;

        public WebSiteNodeCollection(WebSiteNode node)
        {
            // TODO: Complete member initialization
            this.node = node;
        }

        public WebSiteNodeCollection() : this(new WebSiteNode())
        {
            // TODO: Complete member initialization
        }

        public IHierarchyData GetHierarchyData(object enumeratedItem)
        {
            throw new NotImplementedException();
        }

        public IEnumerator GetEnumerator()
        {
            return this.InnerList.GetEnumerator();
        }

        public int Add(WebSiteNode node)
        {
            this.InnerList.Add(node);
            return this.InnerList.FindIndex(item => item == node);
        }

        public void Clear()
        {
            this.InnerList.Clear();
        }

        public bool Contains(WebSiteNode value)
        {
            return this.InnerList.Contains(value);
        }

        public bool Contains(object value)
        {
            return this.InnerList.Contains((WebSiteNode)value);
        }

        public int IndexOf(WebSiteNode value)
        {
            return this.InnerList.IndexOf(value);
        }

        public int IndexOf(object value)
        {
            return this.InnerList.IndexOf((WebSiteNode)value);
        }

        public void Insert(int index, WebSiteNode value)
        {
            this.InnerList.Insert(index, value);
        }

        public void Insert(int index, object value)
        {
            this.InnerList.Insert(index, (WebSiteNode)value);
        }

        public bool IsFixedSize
        {
            get { return false; }
        }

        public bool IsReadOnly
        {
            get { return false; }
        }

        public void Remove(WebSiteNode value)
        {
            this.InnerList.Remove(value);
        }

        public void RemoveAt(int index)
        {
            this.InnerList.RemoveAt(index);
        }

        public object this[int index]
        {
            get
            {
                return this.InnerList[index];
            }
            set
            {
                this.InnerList[index] = (WebSiteNode)value;
            }
        }

        public object this[string key]
        {
            get
            {
                return this.InnerList.Find(item => item.Key == key);
            }
        }

        public WebSiteNode this[WebSiteNode Value]
        {
            get
            {
                return this.InnerList.Find(item => item == Value);
            }
            set
            {
                this[Value.Id] = Value;
            }
        }

        public WebSiteNode this[Uri Value]
        {
            get
            {
                return this.InnerList.Find(item => item.Url == Value.AbsoluteUri);
            }
            /*
            set
            {
                this[Value.Id] = Value;
            }
            */
        }

        public void CopyTo(WebSiteNode[] array, int index)
        {
            this.InnerList.CopyTo(array, index);
        }

        public int Count
        {
            get { return this.InnerList.Count; }
        }

        public bool IsSynchronized
        {
            get { return false; }
        }

        public int Add(object value)
        {
            this.InnerList.Add((WebSiteNode)value);
            return IndexOf((WebSiteNode)value);
        }

        public void Remove(object value)
        {
            this.InnerList.Remove((WebSiteNode)value);
        }

        public void CopyTo(Array array, int index)
        {
            throw new NotImplementedException();
        }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1065:DoNotRaiseExceptionsInUnexpectedLocations")]
        public object SyncRoot
        {
            get { throw new NotImplementedException(); }
        }

        internal HierarchicalDataSourceView GetHierarchicalDataSourceView()
        {
            throw new NotImplementedException();
        }

        internal static WebSiteDataSourceView ReadOnly(WebSiteNodeCollection webSiteNodeCollection)
        {
            throw new NotImplementedException();
        }
    }
}
