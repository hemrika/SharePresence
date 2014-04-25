using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web.UI;
using System.Runtime;
using System.Web;
using System.ComponentModel;
using AttributeCollection = System.Web.UI.AttributeCollection;
using System.Globalization;
namespace Hemrika.SharePresence.Html5.WebControls
{
    /// <summary>
    /// Represents an option among the list of suggestions in a <see cref="Hemrika.SharePresence.Html5.WebControls.DataList" />
    /// </summary>
    [ParseChildren(true, "Value")]
    public class DataListItem : IStateManager, IParserAccessor, IAttributeAccessor
    {
        #region Fields
        private AttributeCollection _attributes;
        private bool enabled;
        private bool enabledisdirty;
        private bool marked;
        private string value;
        private bool valueisdirty;
        #endregion
        #region Methods
        /// <summary>
        /// Creates a new instance of DataListItem
        /// </summary>
        public DataListItem()
            : this(null, true)
        {
        }
        /// <summary>
        /// Creates a new instance of DataListItem
        /// </summary>
        /// <param name="value">The initial value member</param>
        public DataListItem(string value)
            : this(value, true)
        {
        }
        /// <summary>
        /// Creates a new instance of DataListItem
        /// </summary>
        /// <param name="value">the initial value member</param>
        /// <param name="enabled">the initial enabled member</param>
        public DataListItem(string value, bool enabled)
        {
            this.value = value;
            this.enabled = enabled;
        }
        /// <summary>
        /// Determines whether the specified <see cref="System.Object" /> is equal to the current <see cref="Hemrika.SharePresence.Html5.WebControls.DataListItem" />
        /// </summary>
        /// <param name="o">The <see cref="System.Object" /> to compare with the current DataListItem</param>
        /// <returns>true if the specified <see cref="System.Object" /> is equal to the current <see cref="Hemrika.SharePresence.Html5.WebControls.DataListItem" />; otherwise, false.</returns>    
        public override bool Equals(object o)
        {
            DataListItem item = o as DataListItem;
            if (item == null)
            {
                return false;
            }
            return this.Value.Equals(item.Value);
        }
        /// <summary>
        /// Serves as a hash function for a particular type.
        /// </summary>
        /// <returns>A hash code for the current <see cref="Hemrika.SharePresence.Html5.WebControls.DataListItem" />.</returns>
        public override int GetHashCode()
        {
            return Value.GetHashCode();
        }
        /// <summary>
        /// returns a new <see cref="Hemrika.SharePresence.Html5.WebControls.DataListItem" /> instance with the specified value
        /// </summary>
        /// <param name="value">the value member of the new instance</param>
        /// <returns>A new instance of type <see cref="Hemrika.SharePresence.Html5.WebControls.DataListItem" /></returns>
        public static DataListItem FromString(string value)
        {
            return new DataListItem(value);
        }
        internal void LoadViewState(object state)
        {
            if (state == null)
            {
                return;
            }
            if (state is Pair)
            {
                Pair triplet = (Pair)state;
                if (triplet.First != null)
                {
                    this.Value = (string)triplet.First;
                }
                if (triplet.Second != null)
                {
                    try { this.Enabled = (bool)triplet.Second; }
                    catch { }
                }
            }
            else
            {
                this.Value = (string)state;
            }

        }

        internal void RenderAttributes(HtmlTextWriter writer)
        {
            if (this._attributes != null)
            {
                this._attributes.AddAttributes(writer);
            }
        }

        private void ResetValue()
        {
            this.Value = null;
        }

        internal object SaveViewState()
        {
            string val = null;
            if (this.valueisdirty)
            {
                val = this.Value;
            }
            if (this.enabledisdirty)
            {
                return new Pair(val, this.Enabled);
            }
            else
            {
                return val;
            }
        }

        private bool ShouldSerializeValue()
        {
            return ((this.value != null) && (this.value.Length != 0));
        }

        string IAttributeAccessor.GetAttribute(string name)
        {
            return this.Attributes[name];
        }

        void IAttributeAccessor.SetAttribute(string name, string value)
        {
            this.Attributes[name] = value;
        }

        void IParserAccessor.AddParsedSubObject(object obj)
        {
            if (obj is LiteralControl)
            {
                this.Value = ((LiteralControl)obj).Text;
            }
            else
            {
                if (obj is DataBoundLiteralControl)
                {
                    throw new HttpException("Control Cannot Databind");
                }
                throw new HttpException("Cannot Have Children Of Type" + obj.GetType().Name.ToString(CultureInfo.InvariantCulture));
            }
        }

        void IStateManager.LoadViewState(object state)
        {
            this.LoadViewState(state);
        }

        object IStateManager.SaveViewState()
        {
            return this.SaveViewState();
        }

        void IStateManager.TrackViewState()
        {
            this.TrackViewState();
        }
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public override string ToString()
        {
            return this.Value;
        }

        internal void TrackViewState()
        {
            this.marked = true;
        }
        #endregion
        #region Properties
        /// <summary>
        /// Gets the collection of arbitrary attributes (for rendering only) that do 
        /// not correspond to properties on the control.
        /// </summary>
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden), Browsable(false)]
        public AttributeCollection Attributes
        {
            get
            {
                if (this._attributes == null)
                {
                    this._attributes = new AttributeCollection(new StateBag(true));
                }
                return this._attributes;
            }
        }

        internal bool Dirty
        {
            get
            {
                return this.valueisdirty || this.enabledisdirty;
            }
            set
            {
                this.valueisdirty = value;
                this.enabledisdirty = value;
            }
        }

        /// <summary>
        /// If set to false will not be rendered
        /// </summary>
        [DefaultValue(true), Description("If set to false will not be rendered")]
        public bool Enabled
        {
            get
            {
                return this.enabled;
            }
            set
            {
                this.enabled = value;
                if (((IStateManager)this).IsTrackingViewState)
                {
                    this.enabledisdirty = true;
                }
            }
        }

        internal bool HasAttributes
        {
            get
            {
                return ((this._attributes != null) && (this._attributes.Count > 0));
            }
        }

        bool IStateManager.IsTrackingViewState
        {
            get
            {
                return this.marked;
            }
        }

        /// <summary>
        /// The value that the option suggests
        /// </summary>
        [Localizable(true), DefaultValue("")]
        public string Value
        {
            get
            {
                return this.value ?? "";
            }
            set
            {
                this.value = value;
                if (((IStateManager)this).IsTrackingViewState)
                {
                    this.valueisdirty = true;
                }
            }
        }
        #endregion
    }
}
