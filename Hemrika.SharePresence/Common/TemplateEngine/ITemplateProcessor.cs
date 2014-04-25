using System.Web.UI;
using System.Xml.Xsl;

namespace Hemrika.SharePresence.Common.TemplateEngine
{
	internal interface ITemplateProcessor
	{
        XsltException ErrorMessage();
		bool LoadDefinition(TemplateDefinition baseDefinition);
        bool LoadDefinition(TemplateDefinition baseDefinition, string xslsource);
		void Render(object source, HtmlTextWriter htmlWriter, TemplateDefinition liveDefinition);
	}
}