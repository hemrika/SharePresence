// -----------------------------------------------------------------------
// <copyright file="ListItemTemplateProcessor.cs" company="">
// TODO: Update copyright text.
// </copyright>
// -----------------------------------------------------------------------

namespace Hemrika.SharePresence.Common.TemplateEngine
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Xml.Xsl;
    using System.Web.UI;
    using System.IO;
    using System.Xml;
    using System.Security.Permissions;

    /// <summary>
    /// TODO: Update summary.
    /// </summary>
    [PermissionSet(SecurityAction.Demand, Name="FullTrust")]
    public class ListItemTemplateProcessor : ITemplateProcessor
    {
        private XslCompiledTransform xsl;
        private string ProcessorName = "Design";
        private XsltException ErrorMessage;

        /// <summary>
        /// 
        /// </summary>
        /// <param name="baseDefinition"></param>
        /// <returns></returns>
        public bool LoadDefinition(TemplateDefinition baseDefinition)
        {
            try
            {
                if (baseDefinition.ProcessorName == ProcessorName)
                {
                    xsl = Utilities.CachedXslt(baseDefinition.TemplateVirtualPath,true);
                }
                return (baseDefinition.ProcessorName == ProcessorName) ? true : false;
            }
            catch (XsltException ex)
            {
                ErrorMessage = ex;
                return false;
            }
        }

        public bool LoadDefinition(TemplateDefinition baseDefinition,string xslsource)
        {
            try
            {
                if (baseDefinition.ProcessorName == ProcessorName)
                {
                    xsl = Utilities.CachedXslt(xslsource);
                }
                return (baseDefinition.ProcessorName == ProcessorName) ? true : false;
            }
            catch (XsltException ex)
            {
                ErrorMessage = ex;
                return false;
            }
        }

        public void Render(object source, HtmlTextWriter htmlWriter, TemplateDefinition liveDefinition)
        {
            try
            {

                var args = new XsltArgumentList();
                args.AddParam("ControlID", "", liveDefinition.clientId);
                args.AddParam("Options", "", ConvertToJson(liveDefinition.ClientOptions));
                liveDefinition.TemplateArguments.ForEach(a => args.AddParam(a.Name, "", a.Value));

                using (MemoryStream stream = new MemoryStream())
                {
                    StreamWriter writer = new StreamWriter(stream);
                    writer.Write(source);
                    writer.Flush();
                    stream.Position = 0L;
                    XmlReader reader = XmlReader.Create(stream);
                    xsl.Transform(reader, args, htmlWriter);
                }
            }
            catch (XsltException ex)
            {
                ErrorMessage = ex;
                throw (ex);
            }
        }

        protected static string ConvertToJson(List<ClientOption> options)
        {
            var result = new StringBuilder();
            result.Append("{");

            if (options != null)
            {
                foreach (var option in options)
                {
                    if (option is ClientNumber)
                    {
                        result.AppendFormat("{0}:{1},", option.Name, Utilities.ConvertToJs(Convert.ToDecimal(option.Value)));
                    }
                    else if (option is ClientString)
                    {
                        result.AppendFormat("{0}:{1},", option.Name, Utilities.ConvertToJs(option.Value));
                    }
                    else if (option is ClientBoolean)
                    {
                        result.AppendFormat(
                            "{0}:{1},", option.Name, Utilities.ConvertToJs(Convert.ToBoolean(option.Value.ToLowerInvariant())));
                    }
                    else
                    {
                        result.AppendFormat("{0}:{1},", option.Name, option.Value);
                    }
                }
                if (options.Count > 0)
                {
                    result.Remove(result.Length - 1, 1);
                }
            }

            result.Append("}");
            return result.ToString();
        }

        XsltException ITemplateProcessor.ErrorMessage()
        {
            return ErrorMessage;
        }
    }
}