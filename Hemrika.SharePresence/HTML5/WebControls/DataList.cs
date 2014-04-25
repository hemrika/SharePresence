using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Collections;
using System.ComponentModel;
using System.Runtime;
using System.Globalization;

namespace Hemrika.SharePresence.Html5.WebControls
{
    /// <summary>
    /// Represents a set of DataListItems that represent predefined options for other controls.
    /// </summary>
    [ParseChildren(true, "Items")]
    public class DataList : Html5DataBoundControl
    {
        #region Fields
        private DataListItemCollection items;
        #endregion
        #region Methods

        /// <summary>
        /// Adds HTML attributes and styles that need to be rendered to the specified
        /// System.Web.UI.HtmlTextWriter instance.
        /// </summary>
        /// <param name="writer">An System.Web.UI.HtmlTextWriter that represents the output stream to render HTML content on the client</param>
        protected override void AddAttributesToRender(HtmlTextWriter writer)
        {
            if (this.Page != null)
            {
                this.Page.VerifyRenderingInServerForm(this);
            }
            if (this.Enabled && (!base.IsEnabled))// & this.SupportsDisabledAttribute))
            {
                writer.AddAttribute(HtmlTextWriterAttribute.Disabled, "disabled");
            }
            base.AddAttributesToRender(writer);
        }
        /// <summary>
        /// Restores view-state information from a previous request that was saved with the System.Web.UI.WebControls.WebControl.SaveViewState() method.
        /// </summary>
        /// <param name="savedState">An object that represents the control state to restore.</param>
        protected override void LoadViewState(object savedState)
        {
            if (savedState != null)
            {
                Triplet triplet = (Triplet)savedState;
                base.LoadViewState(triplet.First);
                this.Items.LoadViewState(triplet.Second);
            }
            else
            {
                base.LoadViewState(null);
            }
        }
        /// <summary>
        /// Raises the System.Web.UI.Control.DataBinding event.
        /// </summary>
        /// <param name="e">An System.EventArgs object that contains the event data.</param>
        protected override void OnDataBinding(EventArgs e)
        {
            base.OnDataBinding(e);
            IEnumerable data = this.DataSource as IEnumerable;
            this.PerformDataBinding(data);
        }
        /// <summary>
        ///  Binds data from the data source to the control.
        /// </summary>
        /// <param name="dataSource">The System.Collections.IEnumerable list of data returned from a System.Web.UI.WebControls.DataBoundControl.PerformSelect() method call.</param>
        protected override void PerformDataBinding(IEnumerable dataSource)
        {
            base.PerformDataBinding(dataSource);
            if (dataSource != null)
            {
                string dataValueField = this.DataValueField;
                if (!this.AppendDataBoundItems)
                {
                    this.Items.Clear();
                }
                ICollection dataCollection = dataSource as ICollection;
                if (dataCollection != null)
                {
                    this.Items.Capacity = dataCollection.Count + this.Items.Count;
                }
                foreach (object dataItem in dataSource)
                {
                    var item = new DataListItem();
                    if (!string.IsNullOrEmpty(dataValueField))
                    {
                        item.Value = DataBinder.GetPropertyValue(dataItem, dataValueField, null);
                    }
                    else
                    {
                        item.Value = dataItem.ToString();
                    }
                    this.Items.Add(item);
                }
            }
        }
        /// <summary>
        /// Retrieves data from the associated data source.
        /// </summary>
        protected override void PerformSelect()
        {
            this.OnDataBinding(EventArgs.Empty);
            base.RequiresDataBinding = false;
            base.MarkAsDataBound();
            this.OnDataBound(EventArgs.Empty);
        }
        /// <summary>
        /// Renders the contents of the control to the specified writer. This method is used primarily by control developers.
        /// </summary>
        /// <param name="writer">A System.Web.UI.HtmlTextWriter that represents the output stream to render HTML content on the client.</param>
        protected override void RenderContents(HtmlTextWriter writer)
        {
            var items = this.Items;
            int count = items.Count;
            if (count > 0)
            {
                for (int i = 0; i < count; i++)
                {
                    var item = items[i];
                    if (item.Enabled)
                    {
                        writer.WriteBeginTag("option");
                        writer.WriteAttribute("value", item.Value, true);
                        if (item.HasAttributes)
                        {
                            item.Attributes.Render(writer);
                        }
                        writer.Write('>');
                        writer.WriteEndTag("option");
                        writer.WriteLine();
                    }
                }
            }
        }
        /// <summary>
        /// Saves any state that was modified after the System.Web.UI.WebControls.Style.TrackViewState() method was invoked.
        /// </summary>
        /// <returns>An object that contains the current view state of the control; otherwise, if there is no view state associated with the control, null.</returns>
        protected override object SaveViewState()
        {
            object x = base.SaveViewState();
            object y = this.Items.SaveViewState();
            if (((y == null)) && (x == null))
            {
                return null;
            }
            return new Triplet(x, y, null);
        }
        /// <summary>
        ///  Causes the control to track changes to its view state so they can be stored in the object's System.Web.UI.Control.ViewState property.
        /// </summary>
        protected override void TrackViewState()
        {
            base.TrackViewState();
            this.Items.TrackViewState();
        }
        #endregion
        #region Properties
        /// <summary>
        /// Gets or sets a value indicating data bound items shoud be appended to previous items
        /// </summary>
        [DefaultValue(false), Description("Data bound items shoud be appended to previous items"), Themeable(false), Category("Behavior")]
        public virtual bool AppendDataBoundItems
        {
            get
            {
                return GetViewState("AppendDataBoundItems", false);
            }
            set
            {
                SetViewState("AppendDataBoundItems", value);
                if (base.Initialized)
                {
                    base.RequiresDataBinding = true;
                }
            }
        }
        /// <summary>
        /// Name of value property of data object
        /// </summary>
        [DefaultValue(""), Themeable(false), Category("Data"), Description("ListControl_DataValueField")]
        public virtual string DataValueField
        {
            get
            {
                return GetViewState("DataValueField", string.Empty);
            }
            set
            {
                SetViewState("DataValueField", value);
                if (base.Initialized)
                {
                    base.RequiresDataBinding = true;
                }
            }
        }
        /// <summary>
        /// Gets the current DataListItems
        /// </summary>
        [MergableProperty(false), PersistenceMode(PersistenceMode.InnerDefaultProperty), Category("Default"), DefaultValue((string)null), Description("ListControl_Items")]
        public virtual DataListItemCollection Items
        {
            get
            {
                if (this.items == null)
                {
                    this.items = new DataListItemCollection();
                    if (base.IsTrackingViewState)
                    {
                        this.items.TrackViewState();
                    }
                }
                return this.items;
            }
        }
        /// <summary>
        /// "datalist" tag in html
        /// </summary>
        protected override string TagName
        {
            get
            {
                return "datalist";
            }
        }
        /// <summary>
        /// Set to unknown to force control to read TagName
        /// </summary>
        protected override HtmlTextWriterTag TagKey
        {
            get
            {
                return HtmlTextWriterTag.Unknown;
            }
        }
        #endregion
    }
}
