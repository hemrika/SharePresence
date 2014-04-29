// -----------------------------------------------------------------------
// <copyright file="BaseFieldControlAdapter.cs" company="">
// TODO: Update copyright text.
// </copyright>
// -----------------------------------------------------------------------

namespace Hemrika.SharePresence.WebSite.Adapters
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Web.UI;
    using System.IO;
    using System.Globalization;
    using System.Text.RegularExpressions;

    /// <summary>
    /// TODO: Update summary.
    /// </summary>
    public class BaseFieldControlAdapter : System.Web.UI.Adapters.ControlAdapter
    {
        protected override void OnLoad(EventArgs e)
        {
            try
            {
                base.OnLoad(e);
            }
            catch (Exception ex)
            {
                ex.ToString();
            }
        }

        protected override void Render(System.Web.UI.HtmlTextWriter writer)
        {
            var builder = new StringBuilder();
            using (var baseWriter = new HtmlTextWriter(new StringWriter(builder, CultureInfo.InvariantCulture)))
            {
                base.Render(baseWriter);
            }

            string output = builder.ToString();
            
            //@"(?<=<div class=""ms-formfieldlabelcontainer""[^>]*>).*?(?=</div>)",RegexOptions.Singleline);
            /*
            MatchCollection divs = Regex.Matches(output, @"(?<=<div [^>]*>).*?(?=</div>)", RegexOptions.Singleline); //"<div.*?>.*?</div>",(RegexOptions.IgnoreCase | RegexOptions.Singleline));

            string div = string.Empty;
            foreach (Match match in divs)
            {
                Match name = Regex.Match(match.Value, @"(?<=\bclass="")[^""]*");
                if (name.Success)
                {
                    if (name.Value == "ms-formfieldlabelcontainer")
                    {
                        div += match.Value;
                        break;
                    }
                }
            }

            if (!string.IsNullOrEmpty(div))
            {
                output = output.Replace(div, string.Empty);
            }
            */
            /*
            MatchCollection spans = Regex.Matches(output, @"(?<=<span [^>]*>).*?(?=</span>)", RegexOptions.Singleline);

            foreach (Match span in spans)
            {
                if (!string.IsNullOrEmpty(span.Value))
                {
                    output = span.Value;
                    break;
                }
            }
            */
            writer.Write(output);
            //base.Render(writer);
        }
    }
}
