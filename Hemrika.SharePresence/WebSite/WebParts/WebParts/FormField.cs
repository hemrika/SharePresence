// -----------------------------------------------------------------------
// <copyright file="FormField.cs" company="">
// TODO: Update copyright text.
// </copyright>
// -----------------------------------------------------------------------

namespace Hemrika.SharePresence.WebSite.WebParts
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Web.UI;
    using Microsoft.SharePoint.WebControls;

    /// <summary>
    /// TODO: Update summary.
    /// </summary>
    [Serializable]
    [ValidationPropertyAttribute("Value")]
    public class FormField : Microsoft.SharePoint.WebControls.FormField //, IValidator
    {
        /// <summary>
        /// Uid
        /// </summary>
        public string name { get; set; }

        /// <summary>
        /// SharePoint list id
        /// </summary>
        public string internalname { get; set; }

        public override string ClientID
        {
            get
            {
                return ID;
                //return base.ClientID;
            }
        }

        /*
        protected override bool HasPostBackEditData
        {
            get
            {
                return false;
                //return base.HasPostBackEditData;
            }
        }

        protected override void RenderValidationMessage(HtmlTextWriter output)
        {
            base.RenderValidationMessage(output);
        }
        */

        internal bool WpcmEnabled
        {
            get
            {
                return true;
            }
        }

        public override string GetDesignTimeHtml()
        {
            return base.GetDesignTimeHtml();
        }

        public override object Value
        {
            get
            {
                this.EnsureChildControls();
                return base.Value;
            }
            set
            {
                this.EnsureChildControls();
                try
                {
                    base.Value = value;
                }
                catch (Exception)
                {
                }
            }
        }

        public override void Validate()
        {
            base.Validate();
        }

        /*
        new bool IsValid 
        {
            get
            {
                return base.IsValid;
            }
            set
            {
                base.IsValid = value;
            }
        }

        public override void Validate()
        {
            if (!this.DesignMode)
            {
                base.Validate();
            }
        }
        */
    }
}
