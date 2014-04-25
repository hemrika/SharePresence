// -----------------------------------------------------------------------
// <copyright file="Image.cs" company="">
// TODO: Update copyright text.
// </copyright>
// -----------------------------------------------------------------------

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web.UI.HtmlControls;
using System.ComponentModel;
using System.Web.UI;

namespace Hemrika.SharePresence.Html5.WebControls
{
    public class Image : ContainerControl
    {
        public Image() : base(ContainerType.Img) { }

        /// <summary>
        /// Adds HTML attributes and styles that need to be rendered to the specified
        /// System.Web.UI.HtmlTextWriter instance.
        /// </summary>
        /// <param name="writer">An System.Web.UI.HtmlTextWriter that represents the output stream to render HTML content on the client</param>
        protected override void AddAttributesToRender(HtmlTextWriter writer)
        {
            base.AddAttributesToRender(writer);
            Helper.AddUrlAttributeIfNotEmpty(writer, "src", Src, this);
            Helper.AddUrlAttributeIfNotEmpty(writer, "alt", Alt, this);
            Helper.AddUrlAttributeIfNotEmpty(writer, "ismap", Ismap, this);
            Helper.AddUrlAttributeIfNotEmpty(writer, "usemap", Usemap, this);
            Helper.AddStringAttributeIfNotEmpty(writer, "style", Style);
            Helper.AddStringAttributeIfNotEmpty(writer, "class", Class);
            Helper.AddStringAttributeIfNotEmpty(writer, "title", Title);

        }

        /// <summary>
        /// Gets or sets the src for the image
        /// </summary>
        [DefaultValue(""), Description("The Source for the image"), Themeable(false), Category("Behavior"), UrlProperty]
        public string Src { get { return GetViewState("Src", string.Empty); } set { SetViewState("Src", value); } }

        /// <summary>
        /// Gets or sets the alt for the image
        /// </summary>
        [DefaultValue(""), Description("The Alternate Description for the image"), Themeable(false), Category("Behavior")]
        public string Alt { get { return GetViewState("Alt", string.Empty); } set { SetViewState("Alt", value); } }

        /// <summary>
        /// Gets or sets the ismap for the image
        /// </summary>
        [DefaultValue(""), Description("The Alternate Description for the image"), Themeable(false), Category("Behavior")]
        public string Ismap { get { return GetViewState("Ismap", string.Empty); } set { SetViewState("Ismap", value); } }

        /// <summary>
        /// Gets or sets the usemap for the image
        /// </summary>
        [DefaultValue(""), Description("The Alternate Description for the image"), Themeable(false), Category("Behavior")]
        public string Usemap { get { return GetViewState("Usemap", string.Empty); } set { SetViewState("Usemap", value); } }

        /// <summary>
        /// Gets or sets the Style
        /// </summary>
        [DefaultValue(""), Description("The Style for the image"), Themeable(false), Category("Behavior")]
        public new string Style { get { return GetViewState("Style", string.Empty); } set { SetViewState("Style", value); } }

        /// <summary>
        /// Gets or sets the Class
        /// </summary>
        [DefaultValue(""), Description("The Class for the image"), Themeable(false), Category("Behavior")]
        public string Class { get { return GetViewState("Class", string.Empty); } set { SetViewState("Class", value); } }

        /// <summary>
        /// Gets or sets the Title
        /// </summary>
        [DefaultValue(""), Description("The Title for the image"), Themeable(false), Category("Behavior")]
        public string Title { get { return GetViewState("Title", string.Empty); } set { SetViewState("Title", value); } }

    }
}
