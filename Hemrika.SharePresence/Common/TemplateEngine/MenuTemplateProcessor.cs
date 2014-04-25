using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Web;
using System.Web.UI;
using System.Xml;
using System.Xml.Xsl;
using Hemrika.SharePresence.Common;
using System.Security.Permissions;

namespace Hemrika.SharePresence.Common.TemplateEngine
{
    [PermissionSet(SecurityAction.Demand, Name = "FullTrust")]
	public class MenuTemplateProcessor : ITemplateProcessor
	{
		private XslCompiledTransform xsl;
        private string ProcessorName = "Menu";
        private XsltException ErrorMessage;

        public bool LoadDefinition(TemplateDefinition baseDefinition, string xslsource)
        {
            return LoadDefinition(baseDefinition);
        }

		public bool LoadDefinition(TemplateDefinition baseDefinition)
		{
			try
			{
                if (baseDefinition.ProcessorName == ProcessorName)
                {
                    xsl = Utilities.CachedXslt(baseDefinition.TemplateVirtualPath, false);
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

                using (var xmlStream = new MemoryStream())
                {
                    Utilities.SerialiserFor(source.GetType()).Serialize(xmlStream, source);
                    xmlStream.Seek(0, SeekOrigin.Begin);

                    XmlReader reader = XmlReader.Create(xmlStream);

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