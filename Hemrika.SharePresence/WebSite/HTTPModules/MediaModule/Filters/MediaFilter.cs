using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Hemrika.SharePresence.WebSite.Modules.WebPageModule.Filters;
using System.Web;

namespace Hemrika.SharePresence.WebSite.Modules.MediaModule.Filters
{
    class MediaFilter: WebPageBaseFilter
	{
        //private HttpResponse httpResponse;

		/// <summary>
		/// Initializes a new instance of the <see cref="WebPageWhiteSpaceFilter"/> class.
		/// </summary>
		/// <param name="context">The HTTP context.</param>
        public MediaFilter(HttpContext context)
		{
			this.Sink = context.Response.Filter;
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

		/// <summary>
		/// Process and manipulate the buffer.
		/// </summary>
		/// <param name="buffer">An array of bytes. This method copies <paramref name="count"/> bytes from <paramref name="buffer"/> to the current stream.</param>
		/// <param name="offset">The zero-based byte offset in <paramref name="buffer"/> at which to begin copying bytes to the current stream.</param>
		/// <param name="count">The number of bytes to be written to the current stream.</param>
		protected override void Process(byte[] buffer, int offset, int count)
		{
			string html = Encoding.Default.GetString(buffer, offset, count);
			html = WebPageProcessor.RemoveWhitespaceFromHtml(html);

			byte[] outdata = System.Text.Encoding.Default.GetBytes(html);
			this.Sink.Write(outdata, 0, outdata.GetLength(0));
		}
	}
}