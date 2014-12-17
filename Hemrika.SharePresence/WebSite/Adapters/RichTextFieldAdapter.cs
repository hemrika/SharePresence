// -----------------------------------------------------------------------
// <copyright file="RichHtmlFieldControlAdapter.cs" company="">
// TODO: Update copyright text.
// </copyright>
// -----------------------------------------------------------------------

namespace Hemrika.SharePresence.WebSite.Adapters
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Web.UI.Adapters;
    using System.Text.RegularExpressions;
    using System.Web.UI;
    using Microsoft.SharePoint.Utilities;
    using Microsoft.SharePoint;
    using System.IO;
    using Microsoft.SharePoint.WebControls;

    /// <summary>
    /// TODO: Update summary.
    /// </summary>
    public class RichTextFieldAdapter : ControlAdapter
    {
        private static readonly Regex cWebPartWrapperIdRegex = new Regex(@"<div\sclass=""ms-rtestate-read\s([\w]{8}-(?:[\w]{4}-){3}[\w]{12})""[^>]*>", RegexOptions.IgnoreCase | RegexOptions.Compiled);
        private static readonly Regex cWebPartBodyRegex = new Regex(@".*<div[^>]+class=""ms-WPBody[^""]*""[^>]+>(.*)</div></td>\s*</tr>\s*</table></td></tr></table></div><div[^>]+id=""vid_[^>]+>.*", RegexOptions.IgnoreCase | RegexOptions.Compiled | RegexOptions.Singleline);

        protected override void Render(HtmlTextWriter writer)
        {
            using (new SPMonitoredScope("RichHtmlFieldAdapter"))
            {
                if (SPContext.Current != null && SPContext.Current.FormContext.FormMode == SPControlMode.Display)
                {
                    StringBuilder sb = new StringBuilder();
                    using (new SPMonitoredScope("Render original content"))
                    {
                        using (StringWriter sw = new StringWriter(sb))
                        {
                            HtmlTextWriter htw = new HtmlTextWriter(sw);
                            base.Render(htw);
                        }
                    }
                    string content = sb.ToString();
                    MatchCollection webPartIds = null;

                    using (new SPMonitoredScope("Retrieve Web Part IDs"))
                    {
                        webPartIds = cWebPartWrapperIdRegex.Matches(content);
                    }
                    foreach (Match m in webPartIds)
                    {
                        using (new SPMonitoredScope("Remove Web Part container"))
                        {
                            string wpId = m.Groups[1].Value;
                            content = Regex.Replace(content, String.Format(@"<div[^>]+><div[^>]+id=""div_{0}"">.*?<div[^>]+id=""vid_{0}""[^>]+></div></div>", wpId), (m1) =>
                            {
                                string wpHtml = m1.Value; using (new SPMonitoredScope("Get Web Part body"))
                                {
                                    Match m2 = cWebPartBodyRegex.Match(wpHtml);
                                    if (m2.Success) { wpHtml = m2.Groups[1].Value; }
                                }
                                return wpHtml;
                            }, RegexOptions.IgnoreCase | RegexOptions.Singleline);
                        }
                    }
                    writer.Write(content);
                }
                else
                {
                    base.Render(writer);
                }
            }
        }
    }
}
