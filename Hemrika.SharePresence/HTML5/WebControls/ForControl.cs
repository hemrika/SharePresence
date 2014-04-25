using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web.UI;
using System.Web;
using System.Globalization;
using System.ComponentModel;
using AttributeCollection = System.Web.UI.AttributeCollection;

namespace Hemrika.SharePresence.Html5.WebControls
{
    /// <summary>
    /// Represents an class for containing ID of input controls
    /// </summary>
    public class ForControl : IParserAccessor
    {
        #region Fields
        private string refId;
        #endregion
        #region Methods
        /// <summary>
        /// Creates a new instance of <see cref="Hemrika.SharePresence.Html5.WebControls.ForControl" />
        /// </summary>
        public ForControl() : this(null) { }
        /// <summary>
        /// Creates a new instance of <see cref="Hemrika.SharePresence.Html5.WebControls.ForControl" />
        /// </summary>
        /// <param name="refId">The server ID of the associated control</param>
        public ForControl(string refId)
        {
            this.refId = refId;
        }
        /// <summary>
        /// Determines whether the specified <see cref="System.Object" /> is equal to the current <see cref="Hemrika.SharePresence.Html5.WebControls.ForControl" />
        /// </summary>
        /// <param name="o">The <see cref="System.Object" /> to compare with the current ForControl</param>
        /// <returns>true if the specified <see cref="System.Object" /> is equal to the current <see cref="Hemrika.SharePresence.Html5.WebControls.ForControl" />; otherwise, false.</returns>    
        public override bool Equals(object o)
        {
            ForControl item = o as ForControl;
            if (item == null)
            {
                return false;
            }
            return this.RefID.Equals(item.RefID);
        }

        /// <summary>
        /// Serves as a hash function for a particular type.
        /// </summary>
        /// <returns>A hash code for the current <see cref="Hemrika.SharePresence.Html5.WebControls.ForControl" />.</returns>
        public override int GetHashCode()
        {
            return RefID.GetHashCode();
        }
        /// <summary>
        /// returns a new <see cref="Hemrika.SharePresence.Html5.WebControls.ForControl" /> instance with the specified value
        /// </summary>
        /// <param name="value">the value member of the new instance</param>
        /// <returns>A new instance of type <see cref="Hemrika.SharePresence.Html5.WebControls.ForControl" /></returns>
        public static ForControl FromString(string value)
        {
            return new ForControl(value);
        }

        private void ResetValue()
        {
            this.RefID = null;
        }
        /// <summary>
        /// Return string representation of the current instance
        /// </summary>
        /// <returns>The string representation of the current instance</returns>
        public override string ToString()
        {
            return this.RefID;
        }

        void IParserAccessor.AddParsedSubObject(object obj)
        {
            if (obj is LiteralControl)
            {
                this.RefID = ((LiteralControl)obj).Text;
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
        #endregion
        #region Properties

        /// <summary>
        /// Gets or sets the server ID of associated control
        /// </summary>
        [Localizable(true), DefaultValue("")]
        public string RefID
        {
            get
            {
                return this.refId ?? "";
            }
            set
            {
                this.refId = value;
            }
        }
        #endregion

        
    }
}
