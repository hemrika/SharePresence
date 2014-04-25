using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web.UI.WebControls;
using System.Web.UI;
using System.ComponentModel;

namespace Hemrika.SharePresence.Html5.WebControls
{
    /// <summary>
    /// Represents a control for editing a list of e-mail addresses given in the 
    /// </summary>
    public class EmailInput : InputControl
    {
        /// <summary>
        /// Creates a new instance of <see cref="Hemrika.SharePresence.Html5.WebControls.EmailInput" />
        /// </summary>
        public EmailInput() : base(InputType.Email) { }

        /// <summary>
        /// Gets or sets a value indicating if user can input a comma seperated list of email addresses
        /// </summary>
        [Themeable(false), DefaultValue(false), Category("Behavior"), Description("Enables user to input a comma seperated list of email addresses")]
        public bool Multiple
        { get { return GetViewState("Multiple", false); } set { SetViewState("Multiple", value); } }


        /// <summary>
        /// Adds HTML attributes and styles that need to be rendered to the specified
        /// System.Web.UI.HtmlTextWriter instance.
        /// </summary>
        /// <param name="writer">An System.Web.UI.HtmlTextWriter that represents the output stream to render HTML content on the client</param>
        protected override void AddAttributesToRender(System.Web.UI.HtmlTextWriter writer)
        {
            base.AddAttributesToRender(writer);
            Helper.AddBooleanAttribute(writer, "multiple", Multiple);
        }
        /// <summary>
        /// Gets or sets the selected email address
        /// </summary>
        [Themeable(false), DefaultValue(false), Category("Behavior"), Description("Selected Email Address")]
        public string Value
        { get { return Text; } set { Text = value; } }

        /// <summary>
        /// Gets or sets the selected email addresses
        /// </summary>
        [Themeable(false), DefaultValue(false), Category("Behavior"), Description("Selected Email Addresses")]
        public IEnumerable<string> Values
        {
            get
            {
                return Text.Split(',').Select(n => n.Trim());
            }
            set
            {
                Text = string.Join(",", value.Select(n => n.Trim()).ToArray());
            }
        }
    }
}
