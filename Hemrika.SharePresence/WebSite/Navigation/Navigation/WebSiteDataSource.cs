// -----------------------------------------------------------------------
// <copyright file="WebSiteDataSource.cs" company="">
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
    using System.ComponentModel;
    using System.Web.UI.WebControls.WebParts;
    using System.Web.UI.WebControls;

    public class WebSiteDataSource : HierarchicalDataSourceControl, IDataSource, IListSource
    {

        // Fields
        private WebSiteDataSourceView _dataSourceView;
        private WebSiteProvider _provider;
        private ICollection _viewNames;
        private const string DefaultViewName = "DefaultView";

        public WebSiteDataSource() : base()
        {
            this.DataSourceChanged += new EventHandler(WebSiteDataSource_DataSourceChanged);
        }

        void WebSiteDataSource_DataSourceChanged(object sender, EventArgs e)
        {
            //throw new NotImplementedException();
        }

        public virtual IList GetList()
        {
            return ListSourceHelper.GetList(this);
        }

        private WebSiteNodeCollection GetNodes()
        {
            WebSiteNode currentNode = null;
            int startingNodeOffset = this.StartingNodeOffset;
            if (!string.IsNullOrEmpty(this.StartingNodeUrl) && this.StartFromCurrentNode)
            {
                //throw new InvalidOperationException(SR.GetString("SiteMapDataSource_StartingNodeUrlAndStartFromcurrentNode_Defined"));
            }
            if (this.StartFromCurrentNode)
            {
                currentNode = this.Provider.CurrentNode;
            }
            else if (!string.IsNullOrEmpty(this.StartingNodeUrl))
            {
                currentNode = this.Provider.FindWebSiteNode(this.MakeUrlAbsolute(this.StartingNodeUrl));
                if (currentNode == null)
                {
                    //throw new ArgumentException(SR.GetString("SiteMapPath_CannotFindUrl", new object[] { this.StartingNodeUrl }));
                }
            }
            else
            {
                currentNode = this.Provider.RootNode;
            }
            if (currentNode == null)
            {
                return null;
            }
            if (startingNodeOffset <= 0)
            {
                if (startingNodeOffset != 0)
                {
                    this.Provider.HintNeighborhoodNodes(currentNode, Math.Abs(startingNodeOffset), 0);
                    WebSiteNode parentNode = currentNode.ParentNode;
                    while ((startingNodeOffset < 0) && (parentNode != null))
                    {
                        currentNode = currentNode.ParentNode;
                        parentNode = currentNode.ParentNode;
                        startingNodeOffset++;
                    }
                }
                return this.GetNodes(currentNode);
            }
            WebSiteNode currentNodeAndHintAncestorNodes = this.Provider.GetCurrentNodeAndHintAncestorNodes(-1);
            if (((currentNodeAndHintAncestorNodes == null) || !currentNodeAndHintAncestorNodes.IsDescendantOf(currentNode)) || currentNodeAndHintAncestorNodes.Equals(currentNode))
            {
                return null;
            }
            WebSiteNode node4 = currentNodeAndHintAncestorNodes;
            for (int i = 0; i < startingNodeOffset; i++)
            {
                node4 = node4.ParentNode;
                if ((node4 == null) || node4.Equals(currentNode))
                {
                    return this.GetNodes(currentNodeAndHintAncestorNodes);
                }
            }
            WebSiteNode node5 = currentNodeAndHintAncestorNodes;
            while ((node4 != null) && !node4.Equals(currentNode))
            {
                node5 = node5.ParentNode;
                node4 = node4.ParentNode;
            }
            return this.GetNodes(node5);
        }

        private WebSiteNodeCollection GetNodes(WebSiteNode node)
        {
            if (this.ShowStartingNode)
            {
                return new WebSiteNodeCollection(node);
            }
            return node.ChildNodes;
        }

        internal WebSiteNodeCollection GetPathNodeCollection(string viewPath)
        {
            WebSiteNodeCollection childNodes = null;
            if (string.IsNullOrEmpty(viewPath))
            {
                childNodes = this.GetNodes();
            }
            else
            {
                WebSiteNode node = this.Provider.FindSiteMapNodeFromKey(viewPath);
                if (node != null)
                {
                    childNodes = node.ChildNodes;
                }
            }
            if (childNodes == null)
            {
                childNodes = WebSiteNodeCollection.Empty;
            }
            return childNodes;
        }

        private string MakeUrlAbsolute(string url)
        {
            if (url.Length == 0) //|| !UrlPath.IsRelativeUrl(url))
            {
                return url;
            }
            string appRelativeTemplateSourceDirectory = base.AppRelativeTemplateSourceDirectory;
            if (appRelativeTemplateSourceDirectory.Length == 0)
            {
                return url;
            }
            return url;//UrlPath.Combine(appRelativeTemplateSourceDirectory, url);
        }

        IList IListSource.GetList()
        {
            if (base.DesignMode)
            {
                return null;
            }
            return this.GetList();
        }

        public virtual bool ContainsListCollection
        {
            get
            {
                return ListSourceHelper.ContainsListCollection(this);
            }
        }

        public WebSiteProvider Provider
        {
            get
            {
                if (this._provider == null)
                {
                    if (string.IsNullOrEmpty(this.SiteMapProvider))
                    {
                        this._provider = (WebSiteProvider)SiteMap.Provider;
                        if (this._provider == null)
                        {
                            //throw new HttpException(SR.GetString("SiteMapDataSource_DefaultProviderNotFound"));
                        }
                    }
                    else
                    {
                        try
                        {
                            this._provider = (WebSiteProvider)SiteMap.Providers[this.SiteMapProvider];
                        }
                        catch (Exception) { }

                        if (this._provider == null)
                        {
                            //throw new HttpException(SR.GetString("SiteMapDataSource_ProviderNotFound", new object[] { this.SiteMapProvider }));
                        }
                    }
                }
                return this._provider;
            }
            set
            {
                if (this._provider != value)
                {
                    this._provider = value;
                    this.OnDataSourceChanged(EventArgs.Empty);
                }
            }
        }

        public virtual bool ShowStartingNode
        {
            get
            {
                object obj2 = this.ViewState["ShowStartingNode"];
                if (obj2 != null)
                {
                    return (bool)obj2;
                }
                return true;
            }
            set
            {
                if (value != this.ShowStartingNode)
                {
                    this.ViewState["ShowStartingNode"] = value;
                    this.OnDataSourceChanged(EventArgs.Empty);
                }
            }
        }

        public virtual string SiteMapProvider
        {
            get
            {
                string str = this.ViewState["SiteMapProvider"] as string;
                if (str != null)
                {
                    return str;
                }
                return string.Empty;
            }
            set
            {
                if (value != this.SiteMapProvider)
                {
                    this._provider = null;
                    this.ViewState["SiteMapProvider"] = value;
                    this.OnDataSourceChanged(EventArgs.Empty);
                }
            }
        }

        public virtual bool StartFromCurrentNode
        {
            get
            {
                object obj2 = this.ViewState["StartFromCurrentNode "];
                return ((obj2 != null) && ((bool)obj2));
            }
            set
            {
                if (value != this.StartFromCurrentNode)
                {
                    this.ViewState["StartFromCurrentNode "] = value;
                    this.OnDataSourceChanged(EventArgs.Empty);
                }
            }
        }

        public virtual int StartingNodeOffset
        {
            get
            {
                object obj2 = this.ViewState["StartingNodeOffset"];
                if (obj2 == null)
                {
                    return 0;
                }
                return (int)obj2;
            }
            set
            {
                if (value != this.StartingNodeOffset)
                {
                    this.ViewState["StartingNodeOffset"] = value;
                    this.OnDataSourceChanged(EventArgs.Empty);
                }
            }
        }

        public virtual string StartingNodeUrl
        {
            get
            {
                string str = this.ViewState["StartingNodeUrl"] as string;
                if (str != null)
                {
                    return str;
                }
                return string.Empty;
            }
            set
            {
                if (value != this.StartingNodeUrl)
                {
                    this.ViewState["StartingNodeUrl"] = value;
                    this.OnDataSourceChanged(EventArgs.Empty);
                }
            }
        }

        [Category("Data"), DefaultValue("")]
        public string TypeName
        {
            get { return View.TypeName; }
            set { View.TypeName = value; }
        }

        [Category("Data"), DefaultValue("")]
        public string SelectMethod
        {
            get { return View.SelectMethod; }
            set { View.SelectMethod = value; }
        }

        [PersistenceMode(PersistenceMode.InnerProperty), Category("Data"), DefaultValue((string)null), MergableProperty(false)]
        public ParameterCollection SelectParameters
        {
            get
            {
                return View.SelectParameters;
            }
            set
            {
                View.SelectParameters = value;
            }
        }

        protected static readonly string[] _views = { "DefaultView" };

        protected WebSiteDataSourceView _view;

        protected WebSiteDataSourceView View
        {
            get
            {
                if (_view == null)
                {
                    _view = new WebSiteDataSourceView(this, _views[0]);
                    if (base.IsTrackingViewState)
                    {
                        ((IStateManager)_view).TrackViewState();
                    }
                }
                return _view;
            }
        }

        bool IListSource.ContainsListCollection
        {
            get
            {
                if (base.DesignMode)
                {
                    return false;
                }
                return this.ContainsListCollection;
            }
        }

        private HierarchicalDataSourceView GetTreeView(string viewPath)
        {
            WebSiteNode node = null;
            if (string.IsNullOrEmpty(viewPath))
            {
                WebSiteNodeCollection nodes = this.GetNodes();
                if (nodes != null)
                {
                    return nodes.GetHierarchicalDataSourceView();
                }
            }
            else
            {
                node = this.Provider.FindSiteMapNodeFromKey(viewPath);
                if (node != null)
                {
                    return node.ChildNodes.GetHierarchicalDataSourceView();
                }
            }
            return WebSiteNodeCollection.Empty.GetHierarchicalDataSourceView();
        }

        public virtual WebSiteDataSourceView GetView(string viewName)
        {
            if (this.Provider == null)
            {
                //throw new HttpException(SR.GetString("SiteMapDataSource_ProviderNotFound", new object[] { this.SiteMapProvider }));
            }
            if (this._dataSourceView == null)
            {
                GetDataSourceView(this, string.Empty);
            }
            return this._dataSourceView;
        }

        public virtual ICollection GetViewNames()
        {
            if (this._viewNames == null)
            {
                this._viewNames = new string[] { "DefaultView" };
            }
            return this._viewNames;
        }

        protected override HierarchicalDataSourceView GetHierarchicalView(string viewPath)
        {
            throw new NotImplementedException();
        }

        public event EventHandler DataSourceChanged;

        event EventHandler IDataSource.DataSourceChanged
        {
            add
            {
                this.DataSourceChanged += value;
            }
            remove
            {
                this.DataSourceChanged -= value;
            }
        }

        private void GetDataSourceView(WebSiteDataSource webSiteDataSource, string name)
        {
            this._dataSourceView = new WebSiteDataSourceView(webSiteDataSource, name);
            //throw new NotImplementedException();
        }

        DataSourceView IDataSource.GetView(string viewName)
        {
            return this.GetView(viewName)as DataSourceView;
        }
    }
}
