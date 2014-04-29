#region Using

using System;
using System.IO.Compression;
using System.Net;
using System.Text.RegularExpressions;
using System.Web;
using Hemrika.SharePresence.WebSite.HTTPModules.WebPageModule;

#endregion

namespace Hemrika.SharePresence.WebSite.Modules.WebPageModule.Filters
{
    /// <summary>
    /// Contains various methods usable by web projects
    /// </summary>
    internal static class WebPageProcessor
    {
        #region Private variables

        /// <summary>
        /// The name of the GZIP encoding.
        /// </summary>
        private const string GZIP = "gzip";

        /// <summary>
        /// The name of the Deflate encoding.
        /// </summary>
        private const string DEFLATE = "deflate";

        /// <summary>
        /// A regular expression to localize all whitespace preceeding HTML tag endings.
        /// </summary>
        private static readonly Regex RegexBetweenTags = new Regex(@">(?! )\s+", RegexOptions.Compiled);

        /// <summary>
        /// A regular expression to localize all whitespace preceeding a line break.
        /// </summary>
        private static readonly Regex RegexLineBreaks = new Regex(@"([\n\s])+?(?<= {2,})<", RegexOptions.Compiled);

        #endregion

        /// <summary>
        /// Writes the Last-Modified header and sets the conditional get headers.
        /// </summary>
        /// <param name="lastModified">The date of the last modification.</param>
        /// <param name="context">The HTTP context.</param>
        public static void SetConditionalGetHeaders(DateTime lastModified, HttpContext context)
        {
            HttpResponse response = context.Response;
            HttpRequest request = context.Request;
            lastModified = new DateTime(lastModified.Year, lastModified.Month, lastModified.Day, lastModified.Hour, lastModified.Minute, lastModified.Second);

            string incomingDate = request.Headers["If-Modified-Since"];

            response.Cache.SetLastModified(lastModified);

            DateTime testDate = DateTime.MinValue;

            if (DateTime.TryParse(incomingDate, out testDate) && testDate == lastModified)
            {
                response.ClearContent();
                response.StatusCode = (int)System.Net.HttpStatusCode.NotModified;
                response.SuppressContent = true;
            }
        }

        /// <summary>
        /// Sets the Etag for the conditional get headers.
        /// </summary>
        /// <param name="etag">The etag to send to the response.</param>
        /// <param name="context">The HTTP context.</param>
        public static void SetConditionalGetHeaders(string etag, HttpContext context)
        {
            string ifNoneMatch = context.Request.Headers["If-None-Match"];
            etag = "\"" + etag + "\"";

            if (ifNoneMatch != null && ifNoneMatch.Contains(","))
            {
                ifNoneMatch = ifNoneMatch.Substring(0, ifNoneMatch.IndexOf(",", StringComparison.Ordinal));
            }

            context.Response.AppendHeader("Etag", etag);
            context.Response.Cache.VaryByHeaders["If-None-Match"] = true;

            if (etag == ifNoneMatch)
            {
                context.Response.ClearContent();
                context.Response.StatusCode = (int)HttpStatusCode.NotModified;
                context.Response.SuppressContent = true;
            }
        }

        private static Regex reg = new Regex(@"(?<=[^])\t{2,}|(?<=[>])\s{2,}(?=[<])|(?<=[>])\s{2,11}(?=[<])|(?=[\n])\s{2,}");

        /// <summary>
        /// Removes whitespace from the specified string of HTML.
        /// </summary>
        /// <param name="html">The HTML string to remove white space from.</param>
        /// <returns>The specified HTML string stripped from all whitespace.</returns>
        public static string RemoveWhitespaceFromHtml(string html)
        {
            html = reg.Replace(html, string.Empty);
            html = RegexBetweenTags.Replace(html, ">");
            html = RegexLineBreaks.Replace(html, "<");
            
            return html; //.Trim();
        }

        /// <summary>
        /// Replace style and script url's for branding
        /// </summary>
        /// <param name="html">The HTML string to replace branding url from configuration.</param>
        /// <returns>The specified HTML string replaced with FQDN for branding.</returns>
        public static string ReplaceBrandingForFQDN(string html, string fqdn)
        {
            html = reg.Replace(html, string.Empty);
            html = RegexBetweenTags.Replace(html, ">");
            html = RegexLineBreaks.Replace(html, "<");

            return html; //.Trim();
        }

        /// <summary>
        /// Replace image and video url's for media
        /// </summary>
        /// <param name="html">The HTML string to replace media url from configuration.</param>
        /// <returns>The specified HTML string replaced with FQDN for media.</returns>
        public static string ReplaceMediaForFQDN(string html, string fqdn)
        {
            html = reg.Replace(html, string.Empty);
            html = RegexBetweenTags.Replace(html, ">");
            html = RegexLineBreaks.Replace(html, "<");

            return html; //.Trim();
        }

        public static string MoveViewState(string html)
        {
            int startPoint = html.IndexOf("<input type=\"hidden\" name=\"__VIEWSTATE\"");
            if (startPoint >= 0)
            {
                int endPoint = html.IndexOf("/>", startPoint) + 2;
                string viewstateInput = html.Substring(startPoint, endPoint - startPoint);
                html = html.Remove(startPoint, endPoint - startPoint);
                int formEndStart = html.IndexOf("</form>") - 1;
                if (formEndStart >= 0)
                {
                    html = html.Insert(formEndStart, viewstateInput);
                }
            }
            return html;
        }

        /*
        /// <summary>
        /// Strips the whitespace from any .js file.
        /// </summary>
        /// <param name="body">The body text of which to remove white space from.</param>
        /// <returns>The specified body with no white space.</returns>
        public static string RemoveWhiteSpaceFromJavaScript(string body)
        {
            JavaScriptMinifier jsmin = new JavaScriptMinifier();
            return jsmin.Minify(body);
        }
        */

        /// <summary>
        /// Strips the whitespace from any .css file.
        /// </summary>
        /// <param name="body">The body/contents of the CSS file.</param>
        /// <returns>The body of the CSS file stripped from whitespace and comments.</returns>
        public static string RemoveWhiteSpaceFromStylesheets(string body)
        {
            body = Regex.Replace(body, @"[a-zA-Z]+#", "#");
            body = Regex.Replace(body, @"[\n\r]+\s*", string.Empty);
            body = Regex.Replace(body, @"\s+", " ");
            body = Regex.Replace(body, @"\s?([:,;{}])\s?", "$1");
            body = body.Replace(";}", "}");
            body = Regex.Replace(body, @"([\s:]0)(px|pt|%|em)", "$1");

            // Remove comments from CSS
            body = Regex.Replace(body, @"/\*[\d\D]*?\*/", string.Empty);

            return body;
        }

        #region HTTP compression

        /// <summary>
        /// Compresses the HttpContext's output stream using either Deflate or GZip.
        /// </summary>
        /// <param name="context">The current HTTP context to compress.</param>
        public static void Compress(HttpContext context)
        {
            if (context != null)
            {
                if (IsEncodingAccepted(DEFLATE))
                {
                    context.Response.Filter = new DeflateStream(context.Response.Filter, CompressionMode.Compress);
                    SetEncoding(DEFLATE);
                }
                else if (IsEncodingAccepted(GZIP))
                {
                    context.Response.Filter = new GZipStream(context.Response.Filter, CompressionMode.Compress);
                    SetEncoding(GZIP);
                }
            }
        }

        /// <summary>
        /// Checks the request headers to see if the specified
        /// encoding is accepted by the client.
        /// </summary>
        /// <param name="encoding">The name of the encoding to check for.</param>
        /// <returns>True if the client supports the specified encoding; otherwise false.</returns>
        private static bool IsEncodingAccepted(string encoding)
        {
            return HttpContext.Current.Request.Headers["Accept-encoding"] != null && HttpContext.Current.Request.Headers["Accept-encoding"].Contains(encoding);
        }

        /// <summary>
        /// Adds the specified encoding to the response headers.
        /// </summary>
        /// <param name="encoding">The encoding to sent to the Accept-encoding HTTP header of the response.</param>
        private static void SetEncoding(string encoding)
        {
            HttpContext.Current.Response.AppendHeader("Content-encoding", encoding);
            HttpContext.Current.Response.Cache.VaryByHeaders["Accept-encoding"] = true;
        }

        #endregion

        /// <summary>
        /// Strips the whitespace from any .js file.
        /// </summary>
        /// <param name="body">The body text of which to remove white space from.</param>
        /// <returns>The specified body with no white space.</returns>
        public static string RemoveWhiteSpaceFromJavaScript(string body)
        {
            JavaScriptMinifier jsmin = new JavaScriptMinifier();
            return jsmin.Minify(body);
        }
    }
}