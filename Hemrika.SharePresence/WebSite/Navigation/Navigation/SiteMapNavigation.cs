//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by a tool.
//     Runtime Version:2.0.50727.5456
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

using System;

namespace Hemrika.SharePresence.WebSite.Navigation
{

    public partial class SitemapDataContext : Microsoft.SharePoint.Linq.DataContext
    {

        #region Extensibility Method Definitions
        partial void OnCreated();
        #endregion

        public SitemapDataContext(string requestUrl) :
            base(requestUrl)
        {
            this.OnCreated();
        }

        /// <summary>
        /// Navigation Entries
        /// </summary>
        [Microsoft.SharePoint.Linq.ListAttribute(Name = "SiteMap")]
        public Microsoft.SharePoint.Linq.EntityList<SiteMapNavigationEntry> SiteMap
        {
            get
            {
                return this.GetList<SiteMapNavigationEntry>("SiteMap");
            }
        }
    }

    /// <summary>
    /// Create a new list item.
    /// </summary>
    [Microsoft.SharePoint.Linq.ContentTypeAttribute(Name = "Item", Id = "0x01")]
    [Microsoft.SharePoint.Linq.DerivedEntityClassAttribute(Type = typeof(NavigationEntry))]
    public partial class Item : Microsoft.SharePoint.Linq.ITrackEntityState, Microsoft.SharePoint.Linq.ITrackOriginalValues, System.ComponentModel.INotifyPropertyChanged, System.ComponentModel.INotifyPropertyChanging
    {

        private System.Nullable<int> _id;

        private System.Nullable<int> _version;

        private string _path;

        private Microsoft.SharePoint.Linq.EntityState _entityState;

        private System.Collections.Generic.IDictionary<string, object> _originalValues;

        private string _title;

        #region Extensibility Method Definitions
        partial void OnLoaded();
        partial void OnValidate();
        partial void OnCreated();
        #endregion

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1033:InterfaceMethodsShouldBeCallableByChildTypes")]
        Microsoft.SharePoint.Linq.EntityState Microsoft.SharePoint.Linq.ITrackEntityState.EntityState
        {
            get
            {
                return this._entityState;
            }
            set
            {
                if ((value != this._entityState))
                {
                    this._entityState = value;
                }
            }
        }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1033:InterfaceMethodsShouldBeCallableByChildTypes")]
        System.Collections.Generic.IDictionary<string, object> Microsoft.SharePoint.Linq.ITrackOriginalValues.OriginalValues
        {
            get
            {
                if ((null == this._originalValues))
                {
                    this._originalValues = new System.Collections.Generic.Dictionary<string, object>();
                }
                return this._originalValues;
            }
        }

        public Item()
        {
            this.OnCreated();
        }

        [Microsoft.SharePoint.Linq.ColumnAttribute(Name = "ID", Storage = "_id", ReadOnly = true, FieldType = "Counter")]
        public System.Nullable<int> Id
        {
            get
            {
                return this._id;
            }
            set
            {
                if ((value != this._id))
                {
                    this.OnPropertyChanging("Id", this._id);
                    this._id = value;
                    this.OnPropertyChanged("Id");
                }
            }
        }

        [Microsoft.SharePoint.Linq.ColumnAttribute(Name = "owshiddenversion", Storage = "_version", ReadOnly = true, FieldType = "Integer")]
        public System.Nullable<int> Version
        {
            get
            {
                return this._version;
            }
            set
            {
                if ((value != this._version))
                {
                    this.OnPropertyChanging("Version", this._version);
                    this._version = value;
                    this.OnPropertyChanged("Version");
                }
            }
        }

        [Microsoft.SharePoint.Linq.ColumnAttribute(Name = "FileDirRef", Storage = "_path", ReadOnly = true, FieldType = "Lookup", IsLookupValue = true)]
        public string Path
        {
            get
            {
                return this._path;
            }
            set
            {
                if ((value != this._path))
                {
                    this.OnPropertyChanging("Path", this._path);
                    this._path = value;
                    this.OnPropertyChanged("Path");
                }
            }
        }

        [Microsoft.SharePoint.Linq.ColumnAttribute(Name = "Title", Storage = "_title", Required = true, FieldType = "Text")]
        public string Title
        {
            get
            {
                return this._title;
            }
            set
            {
                if ((value != this._title))
                {
                    this.OnPropertyChanging("Title", this._title);
                    this._title = value;
                    this.OnPropertyChanged("Title");
                }
            }
        }

        public event System.ComponentModel.PropertyChangedEventHandler PropertyChanged;

        public event System.ComponentModel.PropertyChangingEventHandler PropertyChanging;

        protected virtual void OnPropertyChanged(string propertyName)
        {
            if ((null != this.PropertyChanged))
            {
                this.PropertyChanged(this, new System.ComponentModel.PropertyChangedEventArgs(propertyName));
            }
        }

        protected virtual void OnPropertyChanging(string propertyName, object value)
        {
            if ((null == this._originalValues))
            {
                this._originalValues = new System.Collections.Generic.Dictionary<string, object>();
            }
            if ((false == this._originalValues.ContainsKey(propertyName)))
            {
                this._originalValues.Add(propertyName, value);
            }
            if ((null != this.PropertyChanging))
            {
                this.PropertyChanging(this, new System.ComponentModel.PropertyChangingEventArgs(propertyName));
            }
        }
    }

    /// <summary>
    /// Hemrika Navigation Entries
    /// </summary>
    [Microsoft.SharePoint.Linq.ContentTypeAttribute(Name = "NavigationEntry", Id = "0x0100AC0BDBCFCED44248B1A3368090B8E248")]
    [Microsoft.SharePoint.Linq.DerivedEntityClassAttribute(Type = typeof(SiteMapNavigationEntry))]
    public partial class NavigationEntry : Item
    {

        private string _uRL;

        private string _comments;

        private bool _enabled;

        private string _selected;

        private string _breadcrumb;

        private string _separator;

        private string _icon;

        private string _largeImage;

        #region Extensibility Method Definitions
        partial void OnLoaded();
        partial void OnValidate();
        partial void OnCreated();
        #endregion

        public NavigationEntry()
        {
            this.OnCreated();
        }

        [Microsoft.SharePoint.Linq.ColumnAttribute(Name = "URL", Storage = "_uRL", FieldType = "Url")]
        public string URL
        {
            get
            {
                return this._uRL;
            }
            set
            {
                if ((value != this._uRL))
                {
                    this.OnPropertyChanging("URL", this._uRL);
                    this._uRL = value;
                    this.OnPropertyChanged("URL");
                }
            }
        }

        /// <summary>
        /// A summary of this resource
        /// </summary>
        [Microsoft.SharePoint.Linq.ColumnAttribute(Name = "_Comments", Storage = "_comments", FieldType = "Note")]
        public string Comments
        {
            get
            {
                return this._comments;
            }
            set
            {
                if ((value != this._comments))
                {
                    this.OnPropertyChanging("Comments", this._comments);
                    this._comments = value;
                    this.OnPropertyChanged("Comments");
                }
            }
        }

        [Microsoft.SharePoint.Linq.ColumnAttribute(Name = "Enabled", Storage = "_enabled", FieldType = "Boolean")]
        public bool Enabled
        {
            get
            {
                return this._enabled;
            }
            set
            {
                if ((value != this._enabled))
                {
                    this.OnPropertyChanging("Enabled", this._enabled);
                    this._enabled = value;
                    this.OnPropertyChanged("Enabled");
                }
            }
        }

        [Microsoft.SharePoint.Linq.ColumnAttribute(Name = "Selected", Storage = "_selected", FieldType = "Text")]
        public string Selected
        {
            get
            {
                return this._selected;
            }
            set
            {
                if ((value != this._selected))
                {
                    this.OnPropertyChanging("Selected", this._selected);
                    this._selected = value;
                    this.OnPropertyChanged("Selected");
                }
            }
        }

        [Microsoft.SharePoint.Linq.ColumnAttribute(Name = "Breadcrumb", Storage = "_breadcrumb", FieldType = "Text")]
        public string Breadcrumb
        {
            get
            {
                return this._breadcrumb;
            }
            set
            {
                if ((value != this._breadcrumb))
                {
                    this.OnPropertyChanging("Breadcrumb", this._breadcrumb);
                    this._breadcrumb = value;
                    this.OnPropertyChanged("Breadcrumb");
                }
            }
        }

        [Microsoft.SharePoint.Linq.ColumnAttribute(Name = "Separator", Storage = "_separator", FieldType = "Text")]
        public string Separator
        {
            get
            {
                return this._separator;
            }
            set
            {
                if ((value != this._separator))
                {
                    this.OnPropertyChanging("Separator", this._separator);
                    this._separator = value;
                    this.OnPropertyChanged("Separator");
                }
            }
        }

        [Microsoft.SharePoint.Linq.ColumnAttribute(Name = "Icon", Storage = "_icon", FieldType = "Text")]
        public string Icon
        {
            get
            {
                return this._icon;
            }
            set
            {
                if ((value != this._icon))
                {
                    this.OnPropertyChanging("Icon", this._icon);
                    this._icon = value;
                    this.OnPropertyChanged("Icon");
                }
            }
        }

        [Microsoft.SharePoint.Linq.ColumnAttribute(Name = "LargeImage", Storage = "_largeImage", FieldType = "Text")]
        public string LargeImage
        {
            get
            {
                return this._largeImage;
            }
            set
            {
                if ((value != this._largeImage))
                {
                    this.OnPropertyChanging("LargeImage", this._largeImage);
                    this._largeImage = value;
                    this.OnPropertyChanged("LargeImage");
                }
            }
        }
    }

    /// <summary>
    /// Hemrika Navigation Entries
    /// </summary>
    [Microsoft.SharePoint.Linq.ContentTypeAttribute(Name = "NavigationEntry", Id = "0x0100AC0BDBCFCED44248B1A3368090B8E248", List = "SiteMap")]
    public partial class SiteMapNavigationEntry : NavigationEntry
    {

        private Microsoft.SharePoint.Linq.EntitySet<SiteMapNavigationEntry> _siteMapNavigationEntry0;

        private Microsoft.SharePoint.Linq.EntityRef<SiteMapNavigationEntry> _parent;

        #region Extensibility Method Definitions
        partial void OnLoaded();
        partial void OnValidate();
        partial void OnCreated();
        #endregion

        public SiteMapNavigationEntry()
        {
            this._siteMapNavigationEntry0 = new Microsoft.SharePoint.Linq.EntitySet<SiteMapNavigationEntry>();
            this._siteMapNavigationEntry0.OnSync += new System.EventHandler<Microsoft.SharePoint.Linq.AssociationChangedEventArgs<SiteMapNavigationEntry>>(this.OnSiteMapNavigationEntry0Sync);
            this._siteMapNavigationEntry0.OnChanged += new System.EventHandler(this.OnSiteMapNavigationEntry0Changed);
            this._siteMapNavigationEntry0.OnChanging += new System.EventHandler(this.OnSiteMapNavigationEntry0Changing);
            this._parent = new Microsoft.SharePoint.Linq.EntityRef<SiteMapNavigationEntry>();
            this._parent.OnSync += new System.EventHandler<Microsoft.SharePoint.Linq.AssociationChangedEventArgs<SiteMapNavigationEntry>>(this.OnParentSync);
            this._parent.OnChanged += new System.EventHandler(this.OnParentChanged);
            this._parent.OnChanging += new System.EventHandler(this.OnParentChanging);
            this.OnCreated();
        }

        [Microsoft.SharePoint.Linq.AssociationAttribute(Name = "Parent", Storage = "_siteMapNavigationEntry0", ReadOnly = true, MultivalueType = Microsoft.SharePoint.Linq.AssociationType.Backward, List = "SiteMap")]
        public Microsoft.SharePoint.Linq.EntitySet<SiteMapNavigationEntry> SiteMapNavigationEntry0
        {
            get
            {
                return this._siteMapNavigationEntry0;
            }
            set
            {
                this._siteMapNavigationEntry0.Assign(value);
            }
        }

        [Microsoft.SharePoint.Linq.AssociationAttribute(Name = "Parent", Storage = "_parent", MultivalueType = Microsoft.SharePoint.Linq.AssociationType.Single, List = "SiteMap")]
        public SiteMapNavigationEntry Parent
        {
            get
            {
                return this._parent.GetEntity();
            }
            set
            {
                this._parent.SetEntity(value);
            }
        }

        private void OnSiteMapNavigationEntry0Changing(object sender, System.EventArgs e)
        {
            this.OnPropertyChanging("SiteMapNavigationEntry0", this._siteMapNavigationEntry0.Clone());
        }

        private void OnSiteMapNavigationEntry0Changed(object sender, System.EventArgs e)
        {
            this.OnPropertyChanged("SiteMapNavigationEntry0");
        }

        private void OnSiteMapNavigationEntry0Sync(object sender, Microsoft.SharePoint.Linq.AssociationChangedEventArgs<SiteMapNavigationEntry> e)
        {
            if ((Microsoft.SharePoint.Linq.AssociationChangedState.Added == e.State))
            {
                e.Item.Parent = this;
            }
            else
            {
                e.Item.Parent = null;
            }
        }

        private void OnParentChanging(object sender, System.EventArgs e)
        {
            this.OnPropertyChanging("Parent", this._parent.Clone());
        }

        private void OnParentChanged(object sender, System.EventArgs e)
        {
            this.OnPropertyChanged("Parent");
        }

        private void OnParentSync(object sender, Microsoft.SharePoint.Linq.AssociationChangedEventArgs<SiteMapNavigationEntry> e)
        {
            if ((Microsoft.SharePoint.Linq.AssociationChangedState.Added == e.State))
            {
                e.Item.SiteMapNavigationEntry0.Add(this);
            }
            else
            {
                e.Item.SiteMapNavigationEntry0.Remove(this);
            }
        }
    }
}
