using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web.UI.WebControls;
using System.ComponentModel;
using System.Web.UI.HtmlControls;
using System.Web.UI;

namespace Hemrika.SharePresence.Html5.WebControls
{
    /// <summary>
    /// Represents a control for generating a public-private key pair and 
    /// for submitting the public key from that key pair.
    /// </summary>
    public class Keygen : Html5Control
    {
        /// <summary>
        /// Creates a new instance of <see cref="Hemrika.SharePresence.Html5.WebControls.Keygen" />
        /// </summary>
        public Keygen() : base("keygen") { }

        /// <summary>
        /// Adds HTML attributes and styles that need to be rendered to the specified
        /// System.Web.UI.HtmlTextWriter instance.
        /// </summary>
        /// <param name="writer">An System.Web.UI.HtmlTextWriter that represents the output stream to render HTML content on the client</param>
        protected override void AddAttributesToRender(HtmlTextWriter writer)
        {
            writer.AddAttribute("name", this.UniqueID);
            base.AddAttributesToRender(writer);
            Helper.AddStringAttributeIfNotEmpty(writer, "form", CreateFormAttributeValue());
            Helper.AddStringAttributeIfNotEmpty(writer, "challenge", Challenge);
            Helper.AddStringAttributeIfNotEmpty(writer, "keytype", KeyType);
            Helper.AddBooleanAttribute(writer, "autofocus", AutoFocus);
        }
        internal virtual string CreateFormAttributeValue()
        {
            if (string.IsNullOrEmpty(FormID))
                return null;
            return GetFormRenderID(FormID);
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
        /// Gets or sets the value of the id attribute on the form with which to associate the element.
        /// </summary>
        [Description("The value of the id attribute on the form with which to associate the element"), DefaultValue(""), Category("Default")]
        public string FormID
        { get { return GetViewState("FormID", string.Empty); } set { SetViewState("FormID", value); } }

        /// <summary>
        /// Gets or sets a challenge string that is submitted along with the public key
        /// </summary>
        [Description("A challenge string that is submitted along with the public key"), DefaultValue(""), Category("Behavior")]
        public string Challenge
        { get { return GetViewState("Challenge", string.Empty); } set { SetViewState("Challenge", value); } }


        /// <summary>
        /// Gets or sets the type of key generated
        /// </summary>
        [Description("The type of key generated"), DefaultValue(""), Category("Behavior")]
        public string KeyType
        { get { return GetViewState("KeyType", string.Empty); } set { SetViewState("KeyType", value); } }


        /// <summary>
        /// Gets or sets a value which indicate that a control is to be focused as soon as the page is loaded
        /// </summary>
        [Themeable(false), DefaultValue(false), Category("Behavior"), Description("Indicate that a control is to be focused as soon as the page is loaded")]
        public bool AutoFocus
        { get { return GetViewState("AutoFocus", false); } set { SetViewState("AutoFocus", value); } }

    }
}
