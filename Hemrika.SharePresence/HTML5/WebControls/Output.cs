using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.ComponentModel;
using System.Web.UI.HtmlControls;

namespace Hemrika.SharePresence.Html5.WebControls
{
    /// <summary>
    /// Represents the result of a calculation.
    /// </summary>
    [ParseChildren(true, "For")]
    public class Output : Html5Control
    {
        private ForControlCollection forControls;
        /// <summary>
        /// Creates a new instance of <see cref="Hemrika.SharePresence.Html5.WebControls.Output" />
        /// </summary>
        public Output() : base("output") { }

        /// <summary>
        /// Adds HTML attributes and styles that need to be rendered to the specified
        /// System.Web.UI.HtmlTextWriter instance.
        /// </summary>
        /// <param name="writer">An System.Web.UI.HtmlTextWriter that represents the output stream to render HTML content on the client</param>
        protected override void AddAttributesToRender(HtmlTextWriter writer)
        {
            writer.AddAttribute("name", this.UniqueID);
            base.AddAttributesToRender(writer);
            Helper.AddStringAttributeIfNotEmpty(writer, "for", CreateForAttributeValue());
            Helper.AddStringAttributeIfNotEmpty(writer, "form", CreateFormAttributeValue());
        }
        internal virtual string CreateFormAttributeValue()
        {
            if (string.IsNullOrEmpty(FormID))
                return null;
            return GetFormRenderID(FormID);
        }
        internal virtual string CreateForAttributeValue()
        {
            var ids = For.Cast<ForControl>().Select(n => GetControlRenderID(n.RefID));
            return string.Join(" ", ids.ToArray());
        }
        internal string GetControlRenderID(string id)
        {
            if (string.IsNullOrEmpty(id))
                return null;
            Control control = this.FindControl(id);
            if (control == null)
            {
                return id;
            }
            return control.ClientID;
        }
        internal string GetFormRenderID(string id)
        {
            if (this.Page == null || string.IsNullOrEmpty(id))
                return id;
            var form = Page.FindControl(id);
            if (form == null)
                return id;
            if (!(form is HtmlForm))
            {
                throw new InvalidOperationException("Form attribute must be related to a valid form element in page");
            }
            return form.ClientID;
        }
        /// <summary>
        /// Gets or sets a list of one or more elements associated with the calculation whose result this output element represents.
        /// </summary>
        [MergableProperty(false), PersistenceMode(PersistenceMode.InnerDefaultProperty), Category("Default"), DefaultValue((string)null), Description("Identifies one or more elements associated with the calculation whose result this output element represents")]
        public virtual ForControlCollection For
        {
            get
            {
                if (this.forControls == null)
                {
                    this.forControls = new ForControlCollection();
                }
                return this.forControls;
            }
        }
        /// <summary>
        /// Gets or sets the value of the id attribute on the form with which to associate the element.
        /// </summary>
        [Description("The value of the id attribute on the form with which to associate the element"), DefaultValue(""), Category("Default")]
        public string FormID
        { get { return GetViewState("FormID", string.Empty); } set { SetViewState("FormID", value); } }
    }
}
