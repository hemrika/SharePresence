using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web.UI.WebControls.WebParts;
using System.Web;
using System.Web.UI;

namespace Hemrika.SharePresence.Html5.WebControls.WebParts
{
    public class WebSitePartChrome: WebPartChrome
	{
        public WebSitePartChrome(WebSitePartZone zone, WebPartManager manager) : base(zone, manager) { }

		// In case you are rewriting URLs, you need to find the base URL
		private string BaseUrl
		{
			get
			{
				string url = HttpContext.Current.Request.ApplicationPath;
				if (url.EndsWith("/"))
					return url;
				else
					return url + "/";
			}
		}

		public override void RenderWebPart(HtmlTextWriter writer, WebPart webPart)
		{
			// Begin wrapper
			writer.AddAttribute(HtmlTextWriterAttribute.Id, webPart.ID);
			writer.AddAttribute(HtmlTextWriterAttribute.Class, "webpart");
			writer.RenderBeginTag(HtmlTextWriterTag.Div);

			// Begin outer border or chrome
			writer.AddAttribute(HtmlTextWriterAttribute.Class, "webpart_chrome");
			writer.RenderBeginTag(HtmlTextWriterTag.Div);

			// Begin title
			writer.AddAttribute(HtmlTextWriterAttribute.Class, "webpart_title");
			writer.RenderBeginTag(HtmlTextWriterTag.Div);

			//If there's an image url, print that
			if (!String.IsNullOrEmpty(webPart.TitleIconImageUrl))
			{
				writer.AddAttribute(HtmlTextWriterAttribute.Src, BaseUrl + webPart.TitleIconImageUrl);
				writer.RenderBeginTag(HtmlTextWriterTag.Img);
				writer.RenderEndTag();
			}

			//Print the title
			if (!String.IsNullOrEmpty(webPart.Title))
			{
				writer.Write(webPart.Title);
			}
			else
			{
				// If your app is localized, use a definition instead
				writer.Write("Untitled");
			}

			// Close title
			writer.RenderEndTag();

			// Begin body
			writer.AddAttribute(HtmlTextWriterAttribute.Class, "webpart_body");
			writer.RenderBeginTag(HtmlTextWriterTag.Div);

			//Write the subtitle if there is one
			if (!String.IsNullOrEmpty(webPart.Subtitle))
			{
				writer.AddAttribute(HtmlTextWriterAttribute.Class, "webpart_subtitle");
				writer.RenderBeginTag(HtmlTextWriterTag.Div);
				writer.Write(webPart.Subtitle);
				writer.RenderEndTag();
			}

			// Begin data (I'm using this because some users may want to
			// have fancy double borders or other custom styles)
			writer.AddAttribute(HtmlTextWriterAttribute.Class, "webpart_data");
			writer.RenderBeginTag(HtmlTextWriterTag.Div);

			// Print the contents
			RenderPartContents(writer, webPart);

			// Close data
			writer.RenderEndTag();

			// Close body
			writer.RenderEndTag();

			// Close chrome
			writer.RenderEndTag();

			// Close wrapper
			writer.RenderEndTag();
		}

        protected override void RenderPartContents(HtmlTextWriter writer, WebPart webPart)
        {
            
            webPart.RenderControl(writer);

           //base.RenderPartContents(writer, webPart);
        }
	}
}