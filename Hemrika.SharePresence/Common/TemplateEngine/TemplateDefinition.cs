using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Text.RegularExpressions;
using System.Web;
using System.Web.Caching;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Xml;
using Hemrika.SharePresence.Common;
using Microsoft.SharePoint;
using System.Xml.Xsl;

namespace Hemrika.SharePresence.Common.TemplateEngine
{
	public class TemplateDefinition
	{
        internal string clientId;
		internal string TemplatePath;
		public string TemplateVirtualPath;
		internal string TemplateHeadPath;
		//internal readonly List<string> ScriptKeys = new List<string>();
        internal readonly List<ScriptKey> Scripts = new List<ScriptKey>();
		//internal readonly Dictionary<string, string> Scripts = new Dictionary<string, string>();
		internal readonly List<string> StyleSheets = new List<string>();
		internal readonly List<ClientOption> DefaultClientOptions = new List<ClientOption>();
		internal readonly List<TemplateArgument> DefaultTemplateArguments = new List<TemplateArgument>();
		internal ITemplateProcessor Processor;
        internal string ProcessorName = "Menu";
        
		public List<ClientOption> ClientOptions = new List<ClientOption>();
		public List<TemplateArgument> TemplateArguments = new List<TemplateArgument>();

		private readonly Regex regexLinks =
			new Regex(
				"( (href|src)=['\"]?)(?!http:|ftp:|mailto:|file:|javascript:|/)([^'\">]+['\">])",
				RegexOptions.Singleline | RegexOptions.IgnoreCase | RegexOptions.Compiled);

        private string ControlType = "Navigation";
        private string ControlStyle = "Default";

        /// <summary>
        /// Caching Method
        /// </summary>
        /// <param name="ClientId"></param>
        /// <param name="ControlType"></param>
        /// <param name="ControlStyle"></param>
        /// <returns></returns>
        public static TemplateDefinition FromName(string ClientId, string ControlType, string ControlStyle)
        {
            return FromName(ClientId, ControlType, ControlStyle, true, true);
        }

        /// <summary>
        /// Optional Caching Method
        /// </summary>
        /// <param name="ClientId"></param>
        /// <param name="ControlType"></param>
        /// <param name="ControlStyle"></param>
        /// <returns></returns>
        public static TemplateDefinition FromName(string ClientId, string ControlType, string ControlStyle, bool useCache)
        {
            return FromName(ClientId, ControlType, ControlStyle, true,useCache);
        }

        /// <summary>
        /// Override Template processing on load
        /// </summary>
        /// <param name="ClientId"></param>
        /// <param name="ControlType"></param>
        /// <param name="ControlStyle"></param>
        /// <param name="load"></param>
        /// <returns></returns>
        public static TemplateDefinition FromName(string ClientId, string ControlType, string ControlStyle, bool load, bool useCache)
		{
            SPList style = SPContext.Current.Site.RootWeb.Lists["Style Library"];

            string styleLib = "/"+style.RootFolder.Url;
            var manifestUrl = styleLib + "/" + ControlType + "/" + ControlStyle;
            return FromName(ClientId, ControlType, ControlStyle, true, string.Empty,useCache);
		}

        /// <summary>
        /// 
        /// </summary>
        /// <param name="ClientId"></param>
        /// <param name="ControlType"></param>
        /// <param name="ControlStyle"></param>
        /// <param name="load"></param>
        /// <param name="xslsource"></param>
        /// <returns></returns>
        public static TemplateDefinition FromName(string ClientId, string ControlType, string ControlStyle, bool load, string xslsource, bool useCache)
        {
            string styleLib = "/" + SPContext.Current.Site.RootWeb.Lists["Style Library"].RootFolder.Url;
            var manifestUrl = styleLib + "/" + ControlType + "/" + ControlStyle;

            TemplateDefinition definition = FromManifest(ClientId, manifestUrl, load, xslsource, useCache);
            if (definition != null)
            {
                definition.ControlStyle = ControlStyle;
                definition.ControlType = ControlType;
            }
            return definition;
        }

		internal static TemplateDefinition FromManifest(string ClientId,string manifestUrl, bool load, string xslsource, bool useCache)
		{
			var httpContext = HttpContext.Current;
			var cache = httpContext.Cache;
			var manifestPath = httpContext.Server.MapPath(manifestUrl);

            TemplateDefinition baseDef = null;

            if (useCache)
            {
                baseDef = cache[ClientId] as TemplateDefinition;
            }

			if (baseDef == null)
			{
				baseDef = new TemplateDefinition {};
                baseDef.clientId = ClientId;
                SPFile file = SPContext.Current.Site.RootWeb.GetFile(manifestUrl + "/definition.xml");

                if (file != null && file.Exists)
                {
                    var xml = new XmlDocument();
                    xml.Load(file.OpenBinaryStream(SPOpenBinaryOptions.None));

                    foreach (XmlNode node in xml.DocumentElement.ChildNodes)
                    {
                        if (node.NodeType == XmlNodeType.Element)
                        {
                            var elt = (XmlElement)node;
                            switch (elt.LocalName)
                            {
                                case "template":
                                    var processorname = elt.GetAttribute("Processor");
                                    baseDef.ProcessorName = String.IsNullOrEmpty(processorname) ? "Menu" : processorname;
                                    baseDef.TemplateVirtualPath = manifestUrl + "/" + elt.InnerText;
                                    baseDef.TemplatePath = httpContext.Server.MapPath(baseDef.TemplateVirtualPath);
                                    break;
                                case "templateHead":
                                    baseDef.TemplateHeadPath = manifestUrl + "/" + elt.InnerText;
                                    break;
                                case "scripts":
                                    foreach (XmlElement scriptElt in elt.GetElementsByTagName("script"))
                                    {
                                        var jsType = scriptElt.GetAttribute("jsType");
                                        var jsObject = scriptElt.GetAttribute("jsObject");
                                        var scriptText = scriptElt.InnerText.Trim();
                                        //manifestUrl + "/" + scriptElt.InnerText;
                                        //var key = String.IsNullOrEmpty(jsObject) ? scriptPath : jsObject;
                                        
                                        if (String.IsNullOrEmpty(jsType))
                                        {
                                            var script = CreateScript(jsObject, manifestUrl + "/" + scriptElt.InnerText);
                                            ScriptKey screy = new ScriptKey();
                                            screy.key = jsObject;
                                            screy.type = jsType;
                                            screy.value = script;
                                            baseDef.Scripts.Add(screy);
                                        }
                                        else
                                        {
                                            ScriptKey screy = new ScriptKey();
                                            screy.key = jsObject;
                                            screy.type = jsType;
                                            screy.value = scriptText;
                                            baseDef.Scripts.Add(screy);
                                        }
                                    }
                                    break;
                                case "stylesheets":
                                    foreach (XmlElement cssElt in elt.GetElementsByTagName("stylesheet"))
                                    {
                                        var cssPath = manifestUrl + "/" + cssElt.InnerText;
                                        baseDef.StyleSheets.Add(cssPath);
                                    }
                                    break;
                                case "defaultClientOptions":
                                    foreach (XmlElement optionElt in elt.GetElementsByTagName("clientOption"))
                                    {
                                        var optionName = optionElt.GetAttribute("name");
                                        var optionType = optionElt.GetAttribute("type");
                                        var optionValue = optionElt.GetAttribute("value");
                                        if (String.IsNullOrEmpty(optionType))
                                        {
                                            optionType = "passthrough";
                                        }
                                        switch (optionType)
                                        {
                                            case "number":
                                                baseDef.DefaultClientOptions.Add(new ClientNumber(optionName, optionValue));
                                                break;
                                            case "boolean":
                                                baseDef.DefaultClientOptions.Add(new ClientBoolean(optionName, optionValue));
                                                break;
                                            case "string":
                                                baseDef.DefaultClientOptions.Add(new ClientString(optionName, optionValue));
                                                break;
                                            default:
                                                baseDef.DefaultClientOptions.Add(new ClientOption(optionName, optionValue));
                                                break;
                                        }
                                    }
                                    break;
                                case "defaultTemplateArguments":
                                    foreach (XmlElement argElt in elt.GetElementsByTagName("templateArgument"))
                                    {
                                        var argName = argElt.GetAttribute("name");
                                        var argValue = argElt.GetAttribute("value");
                                        baseDef.DefaultTemplateArguments.Add(new TemplateArgument(argName, argValue));
                                    }
                                    break;
                            }
                        }
                    }
                }

                if (load)
                {
                    try
                    {

                        foreach (var processor in Utilities.SupportedTemplateProcessors())
                        {
                            if (string.IsNullOrEmpty(xslsource))
                            {
                                if (processor.LoadDefinition(baseDef))
                                {
                                    baseDef.Processor = processor;
                                    break;
                                }
                                else
                                {
                                    if (processor.ErrorMessage() != null)
                                    {
                                        throw processor.ErrorMessage();
                                    }
                                }
                            }
                            else
                            {
                                if (processor.LoadDefinition(baseDef, xslsource))
                                {
                                    baseDef.Processor = processor;
                                    break;
                                }
                                else
                                {
                                    if (processor.ErrorMessage() != null)
                                    {
                                        throw processor.ErrorMessage();
                                    }
                                }
                            }
                        }
                    }
                    catch (XsltException ex)
                    {
                        throw ex;
                        //throw new Exception(ex.Message + "Error : " + ex.Data.ToString());
                    }

                    if (baseDef.Processor == null)
                    {
                        throw new ApplicationException(String.Format("Can't find processor for manifest {0}", manifestUrl));
                    }
                }
                if (useCache)
                {
                    cache.Remove(ClientId);
                    cache.Insert(ClientId, baseDef);
                }
			}

			var result = baseDef.Clone();
			result.Reset();
			return result;
		}

        private static string CreateScript(string jsObject, string scriptPath)
        {
            return CreateScript(jsObject, scriptPath, string.Empty);
        }

        private static string CreateScript(string jsObject, string scriptPath, string jsLocation)
		{
			string result;

			jsObject = jsObject ?? "";

			if (string.IsNullOrEmpty(jsObject))
			{
				result = String.IsNullOrEmpty(scriptPath)
				         	? ""
				         	: String.Format(@"<script type=""text/javascript"" src=""{0}""></script>", scriptPath);
			}
			else
			{
                /*
				if (jsObject == "DDRjQuery")
				{
					result = String.IsNullOrEmpty(scriptPath)
					         	? @"<script type=""text/javascript"">DDRjQuery=window.DDRjQuery||jQuery;</script>"
					         	: String.Format(
					         		@"<script type=""text/javascript"">if (!window.DDRjQuery) {{if (window.jQuery && (jQuery.fn.jquery>=""1.3"")) DDRjQuery=jQuery; else document.write(unescape('%3Cscript src=""{0}"" type=""text/javascript""%3E%3C/script%3E'));}}</script><script type=""text/javascript"">if (!window.DDRjQuery) DDRjQuery=jQuery.noConflict(true);</script>",
					         		scriptPath);
				}
                */

				//else
				//{
					result = String.IsNullOrEmpty(scriptPath)
					         	? ""
					         	: String.Format(
					         		@"<script type=""text/javascript"">if (!({0})) document.write(unescape('%3Cscript src=""{1}"" type=""text/javascript""%3E%3C/script%3E'));</script>",
					         		GetObjectCheckScript(jsObject),
					         		scriptPath);
				//}
			}

			return result;
		}

		private static string GetObjectCheckScript(string jsObject)
		{
			var objectParts = jsObject.Split('.');
			var objectToCheck = new StringBuilder("window");
			var objectsToCheck = new List<String>();
			foreach (var part in objectParts)
			{
				objectToCheck.AppendFormat(".{0}", part);
				objectsToCheck.Add(objectToCheck.ToString());
			}
			return String.Join(" && ", objectsToCheck.ToArray());
		}

		public TemplateDefinition Clone()
		{
			return (TemplateDefinition)MemberwiseClone();
		}

		public void Reset()
		{
			ClientOptions = new List<ClientOption>(DefaultClientOptions);
			TemplateArguments = new List<TemplateArgument>(DefaultTemplateArguments);
		}

		public void AddClientOptions(List<ClientOption> options, bool replace)
		{
			if (options != null)
			{
				foreach (var option in options)
				{
					var option1 = option;
					if (replace)
					{
						ClientOptions.RemoveAll(o => o.Name == option1.Name);
					}
					if (!ClientOptions.Exists(o => o.Name == option1.Name))
					{
						ClientOptions.Add(option);
					}
				}
			}
		}

		public void AddTemplateArguments(List<TemplateArgument> args, bool replace)
		{
			if (args != null)
			{
				foreach (var arg in args)
				{
					var arg1 = arg;
					if (replace)
					{
						TemplateArguments.RemoveAll(a => a.Name == arg1.Name);
					}
					if (!TemplateArguments.Exists(a => a.Name == arg1.Name))
					{
						TemplateArguments.Add(arg);
					}
				}
			}
		}

		public void PreRender(Page page)
		{
            var headControls = page.Header.Controls;
			var contextItems = HttpContext.Current.Items;

			foreach (var stylesheet in StyleSheets)
			{
				if (!contextItems.Contains(stylesheet))
				{
					var cssControl = new HtmlGenericControl("link");
					cssControl.Attributes.Add("rel", "stylesheet");
					cssControl.Attributes.Add("type", "text/css");
					cssControl.Attributes.Add("href", stylesheet);

					headControls.Add(cssControl);

					contextItems.Add(stylesheet, true);
				}
			}

			//foreach (var scriptKey in ScriptKeys)
            foreach (ScriptKey script in Scripts)
			{

				var clientScript = page.ClientScript;
                if (!string.IsNullOrEmpty(script.type))
                {
                    if (script.type.ToLower() == "startup")
                    {
                        if (!clientScript.IsStartupScriptRegistered(typeof(Page), script.key))
                        {
                            clientScript.RegisterStartupScript(typeof(Page), script.key, script.value,true);
                        }
                    }
                    if (script.type.ToLower() == "block")
                    {
                        if (!clientScript.IsClientScriptBlockRegistered(typeof(Page), script.key))
                        {
                            clientScript.RegisterClientScriptBlock(typeof(Page), script.key, script.value, true);
                        }
                    }

                    if (script.type.ToLower() == "include")
                    {
                        if (!clientScript.IsClientScriptIncludeRegistered(typeof(Page), script.key))
                        {
                            clientScript.RegisterClientScriptInclude(typeof(Page), script.key, script.value);
                        }
                    }

                    if (script.type.ToLower() == "submit")
                    {
                        if (!clientScript.IsOnSubmitStatementRegistered(typeof(Page), script.key))
                        {
                            clientScript.RegisterOnSubmitStatement(typeof(Page), script.key, script.value);
                        }
                    }
                }
                else
                {
                    if (!clientScript.IsClientScriptBlockRegistered(typeof(Page), script.key))
                    {
                        clientScript.RegisterClientScriptBlock(typeof(Page), script.key, script.value, false);
                    }
                }
			}

            if (TemplateHeadPath != null)
            {
                SPFile file = SPContext.Current.Site.RootWeb.GetFile(TemplateHeadPath);
                byte[] bytes = file.OpenBinary();
                System.Text.StringBuilder builder = new StringBuilder();
                foreach (byte b in bytes)
                {
                    builder.Append(b);
                }

                page.Header.Controls.Add(new LiteralControl(builder.ToString()));
            }
            
			//var headContent = String.IsNullOrEmpty(TemplateHeadPath) ? "" : Utilities.CachedFileContent(TemplateHeadPath);
			//var expandedHead = regexLinks.Replace(headContent, "$1" + DNNContext.Current.ActiveTab.SkinPath + "$3");
			//page.Header.Controls.Add(new LiteralControl(expandedHead));
		}

		public void Render(object source, HtmlTextWriter htmlWriter)
		{
            Processor.Render(source, htmlWriter, this);
		}
    }
}