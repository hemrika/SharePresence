using System;
using System.Data;
using System.Configuration;
using System.Linq;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Xml.Linq;
using System.IO;
using System.Text;
using System.Text.RegularExpressions;
using System.Collections.Generic;
using System.Xml;

namespace Hemrika.SharePresence.WebSite.Modules.WebPageModule.Filters
{
    /// <summary>
    /// Summary description for ScriptDeferFilter
    /// </summary>
    public class WebPageViewStateFilter : WebPageBaseFilter
    {
        //private HttpResponse httpResponse;

		/// <summary>
		/// Initializes a new instance of the <see cref="WebPageWhiteSpaceFilter"/> class.
		/// </summary>
		/// <param name="context">The HTTP context.</param>
		public WebPageViewStateFilter(HttpContext context)
		{
			this.Sink = context.Response.Filter;
		}

        public WebPageViewStateFilter(HttpResponse httpResponse)
        {
            this.Sink = httpResponse.Filter;
            //this.httpResponse = httpResponse;
        }

		/// <summary>
		/// Gets or sets a value indicating whether to ignore requests that are too large to be garbage collected efficiently.
		/// </summary>
		/// <value>
		/// 	<c>true</c> if large response should be ignored; otherwise, <c>false</c>.
		/// </value>
		protected override bool IgnoreLargeResponse 
		{
			get { return true; } 
		}

        string viewstateInput;

        public override void Write(byte[] buffer, int offset, int count)
        {
            byte[] data = new byte[count];
            Buffer.BlockCopy(buffer, offset, data, 0, count);
            string html = System.Text.Encoding.Default.GetString(buffer);

            int startPoint = html.IndexOf("<input type=\"hidden\" name=\"__VIEWSTATE\"");
            if (startPoint >= 0)
            {
                int endPoint = html.IndexOf("/>", startPoint) + 2;
                if (endPoint <= 0)
                {
                    endPoint = html.Length;
                }

                viewstateInput = html.Substring(startPoint, endPoint - startPoint);
                html = html.Remove(startPoint, endPoint - startPoint);
            }

            int formEndStart = html.ToUpper().IndexOf("</FORM>");
            int eNdPoint = html.IndexOf("</form>");

            if (formEndStart >= 0)
            {
                html = html.Insert(formEndStart, viewstateInput);
            }

            byte[] outdata = System.Text.Encoding.Default.GetBytes(html);
            this.Sink.Write(outdata, 0, outdata.GetLength(0)); 
            /*
            byte[] data = new byte[count];
            Buffer.BlockCopy(buffer, offset, data, 0, count);
            string html = System.Text.Encoding.Default.GetString(buffer);

            int startPoint = html.IndexOf("<input type=\"hidden\" name=\"__VIEWSTATE\"");
            if (startPoint >= 0)
            {
                int endPoint = html.IndexOf("/>", startPoint) + 2;
                viewstateInput = html.Substring(startPoint, endPoint - startPoint);
                
            }

            int formEndStart = html.IndexOf("</form>") - 1;
            if (formEndStart >= 0)
            {
                html = html.Remove(startPoint, endPoint - startPoint);
                html = html.Insert(formEndStart, viewstateInput);
            }

            byte[] outdata = System.Text.Encoding.Default.GetBytes(html);
            this.Sink.Write(outdata, 0, outdata.GetLength(0));
            */
        }

		/// <summary>
		/// Process and manipulate the buffer.
		/// </summary>
		/// <param name="buffer">An array of bytes. This method copies <paramref name="count"/> bytes from <paramref name="buffer"/> to the current stream.</param>
		/// <param name="offset">The zero-based byte offset in <paramref name="buffer"/> at which to begin copying bytes to the current stream.</param>
		/// <param name="count">The number of bytes to be written to the current stream.</param>
		protected override void Process(byte[] buffer, int offset, int count)
		{
			string html = Encoding.Default.GetString(buffer, offset, count);
			//html = WebPageProcessor.MoveViewState(html);

			byte[] outdata = System.Text.Encoding.Default.GetBytes(html);
			this.Sink.Write(outdata, 0, outdata.GetLength(0));
		}
    }
}