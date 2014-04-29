// -----------------------------------------------------------------------
// <copyright file="WebSiteForm.cs" company="">
// TODO: Update copyright text.
// </copyright>
// -----------------------------------------------------------------------

namespace Hemrika.SharePresence.WebSite.Adapters
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Web.UI.HtmlControls;
    using System.Web;
    using System.Web.UI;
    using System.ComponentModel;
    using System.Web.UI.Adapters;

    [DefaultProperty("Text")]
    [ToolboxData("<{0}:WebSiteForm runat=server></{0}:WebSiteForm>")]
    public class WebSiteForm : ControlAdapter
    {
        protected override void Render(HtmlTextWriter writer)
        {

            base.Render(new RewriteFormHtmlTextWriter(writer));
        }
        
        /*
        /// <summary>
        /// Renders the attributes using the RewriteFormHtmlTextWriter.
        /// </summary>
        protected override void RenderAttributes(HtmlTextWriter writer)
        {
            RewriteFormHtmlTextWriter custom = new RewriteFormHtmlTextWriter(writer);
            //base.RenderAttributes(custom);
        }
        */

        /// <summary>
        /// Writes the HtmlForm's markup to support URL rewrites.
        /// </summary>
        private class RewriteFormHtmlTextWriter : HtmlTextWriter
        {
            /// <summary>
            /// Initializes a new instance of the <see cref="RewriteFormHtmlTextWriter"/> class.
            /// </summary>
            /// <param name="writer">The writer.</param>
            public RewriteFormHtmlTextWriter(HtmlTextWriter writer)
                : base(writer)
            {
                base.InnerWriter = writer.InnerWriter;
            }

            /// <summary>
            /// Writes all attributes the normal way, but writes the action attribute differently.
            /// </summary>
            /// <param name="name">The markup attribute to write to the output stream.</param>
            /// <param name="value">The value assigned to the attribute.</param>
            /// <param name="fEncode">true to encode the attribute and its assigned value; otherwise, false.</param>
            public override void WriteAttribute(string name, string value, bool fEncode)
            {
                if (name == "action")
                {
                    HttpContext context = HttpContext.Current;

                    if (context.Items["ActionAlreadyWritten"] == null)
                    {
                        value = context.Request.RawUrl;
                        context.Items["ActionAlreadyWritten"] = true;
                    }
                }

                base.WriteAttribute(name, value, fEncode);
            }
        }
    }
}
        /*
        protected override void RenderAttributes(HtmlTextWriter writer)
        {
            writer.WriteAttribute("name", this.Name);
            base.Attributes.Remove("name");

            writer.WriteAttribute("method", this.Method);
            base.Attributes.Remove("method");

            this.Attributes.Render(writer);

            writer.WriteAttribute("action", HttpUtility.HtmlEncode(Context.Request.RawUrl));
            base.Attributes.Remove("action");

            writer.WriteAttribute("onsubmit", "if (typeof(WebForm_OnSubmit) == 'function') return WebForm_OnSubmit();");
            base.Attributes.Remove("onsubmit");

            if (base.ID != null)
                writer.WriteAttribute("id", base.ClientID);
        }

        /// <summary>
        /// Renders the attributes using the RewriteFormHtmlTextWriter.
        /// </summary>
        protected override void RenderAttributes(HtmlTextWriter writer)
        {
            RewriteFormHtmlTextWriter custom = new RewriteFormHtmlTextWriter(writer);
            base.RenderAttributes(custom);
        }

        /// <summary>
        /// Writes the HtmlForm's markup to support URL rewrites.
        /// </summary>
        private class RewriteFormHtmlTextWriter : HtmlTextWriter
        {
            /// <summary>
            /// Initializes a new instance of the <see cref="RewriteFormHtmlTextWriter"/> class.
            /// </summary>
            /// <param name="writer">The writer.</param>
            public RewriteFormHtmlTextWriter(HtmlTextWriter writer)
                : base(writer)
            {
                base.InnerWriter = writer.InnerWriter;
            }

            /// <summary>
            /// Writes all attributes the normal way, but writes the action attribute differently.
            /// </summary>
            /// <param name="name">The markup attribute to write to the output stream.</param>
            /// <param name="value">The value assigned to the attribute.</param>
            /// <param name="fEncode">true to encode the attribute and its assigned value; otherwise, false.</param>
            public override void WriteAttribute(string name, string value, bool fEncode)
            {
                if (name == "action")
                {
                    HttpContext context = HttpContext.Current;

                    if (context.Items["ActionAlreadyWritten"] == null)
                    {
                        value = context.Request.RawUrl;
                        context.Items["ActionAlreadyWritten"] = true;
                    }
                }

                base.WriteAttribute(name, value, fEncode);
            }
        }
    }
}
*/