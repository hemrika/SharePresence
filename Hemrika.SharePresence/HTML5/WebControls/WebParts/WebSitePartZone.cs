// -----------------------------------------------------------------------
// <copyright file="WebPartZone.cs" company="">
// TODO: Update copyright text.
// </copyright>
// -----------------------------------------------------------------------

namespace Hemrika.SharePresence.Html5.WebControls.WebParts
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Web.UI;
    using System.Web.UI.WebControls.WebParts;
    using Microsoft.SharePoint.WebPartPages;
    using Microsoft.SharePoint.Utilities;
    using System.ComponentModel;


    public class WebSitePartZone : System.Web.UI.WebControls.WebParts.WebPartZone
    {
        public WebSitePartZone() : base()
        {}

        protected override void Render(HtmlTextWriter writer)
        {
            RenderContents(writer);
        }


        protected override void RenderContents(HtmlTextWriter writer)
        {
            bool inEditMode = false;
            SPWebPartManager swpm = (SPWebPartManager)SPWebPartManager.GetCurrentWebPartManager(this.Page);
            inEditMode = !swpm.GetDisplayMode().AllowPageDesign;

            if (inEditMode)
            {
                writer.AddAttribute(HtmlTextWriterAttribute.Id, this.ID);
                writer.AddAttribute(HtmlTextWriterAttribute.Class, "webpartzone");
                writer.RenderBeginTag(HtmlTextWriterTag.Div);
                base.RenderBody(writer);
                writer.RenderEndTag();
            }
            else
            {
                

                writer.AddAttribute(HtmlTextWriterAttribute.Class, "ms-SPButton ms-WPAddButton");
                writer.AddAttribute(HtmlTextWriterAttribute.Onclick, "CoreInvoke('ShowWebPartAdder', '" + SPHttpUtility.EcmaScriptStringLiteralEncode(this.ID) + "');return false;");
                writer.RenderBeginTag(HtmlTextWriterTag.Div);
                //writer.AddAttribute(HtmlTextWriterAttribute.Href, "javascript:");
                writer.AddAttribute(HtmlTextWriterAttribute.Onclick, "CoreInvoke('ShowWebPartAdder', '" + SPHttpUtility.EcmaScriptStringLiteralEncode(this.ID) + "');return false;");
                writer.RenderBeginTag(HtmlTextWriterTag.P);
                writer.RenderBeginTag(HtmlTextWriterTag.Span);
                //writer.Write(SPHttpUtility.HtmlEncode(WebPartPageResource.GetString("WebPartQuickAdd_AddANewWebPart")));
                //base.RenderBody(writer);
                writer.RenderEndTag();
                writer.RenderEndTag();
                writer.RenderEndTag();

                base.RenderContents(writer);
            }
        }

        protected override WebPartChrome CreateWebPartChrome()
        {
            // Return a new derived class to build the chrome
            return new WebSitePartChrome(this, base.WebPartManager);
        }

        /*
        protected override WebPartCollection GetInitialWebParts()
        {
            throw new NotImplementedException();
        }
        */
        /*
        private ITemplate _zoneTemplate;

        [Browsable(false), DefaultValue((string)null), PersistenceMode(PersistenceMode.InnerProperty), TemplateInstance(TemplateInstance.Single)]
        public virtual ITemplate ZoneTemplate
        {
            get
            {
                return base._zoneTemplate;
            }
            set
            {
                if (!base.DesignMode)
                {
                    throw new InvalidOperationException("WebPart_SetZoneTemplateTooLate");
                }
                this._zoneTemplate = value;
            }
        }
        */
    }
}
