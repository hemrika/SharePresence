﻿// -----------------------------------------------------------------------
// <copyright file="Settings.cs" company="">
// TODO: Update copyright text.
// </copyright>
// -----------------------------------------------------------------------

namespace Hemrika.SharePresence.WebSite.Navigation
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using Hemrika.SharePresence.Common.TemplateEngine;
    using System.Xml.Serialization;
    using System.IO;

    public class Settings
    {
        public string ControlType { get; set; }
        public string ControlStyle { get; set; }
        public string NodeXmlPath { get; set; }
        public string NodeSelector { get; set; }
        public string IncludeNodes { get; set; }
        public string ExcludeNodes { get; set; }
        public string NodeManipulator { get; set; }
        public bool IncludeContext { get; set; }
        public bool IncludeHidden { get; set; }

        private List<ClientOption> clientOptions;
        public List<ClientOption> ClientOptions { get { return clientOptions ?? (clientOptions = new List<ClientOption>()); } set { clientOptions = value; } }

        private List<TemplateArgument> templateArguments;
        public List<TemplateArgument> TemplateArguments { get { return templateArguments ?? (templateArguments = new List<TemplateArgument>()); } set { templateArguments = value; } }

        public static Settings FromXml(string xml)
        {
            var ser = new XmlSerializer(typeof(Settings));
            using (var reader = new StringReader(xml))
                return (Settings)ser.Deserialize(reader);
        }

        public string ToXml()
        {
            var sb = new StringBuilder();
            var ser = new XmlSerializer(typeof(Settings));
            using (var writer = new StringWriter(sb))
                ser.Serialize(writer, this);
            return sb.ToString();
        }

        public override string ToString()
        {
            try
            {
                return ToXml();
            }
            catch (Exception exc)
            {
                return exc.ToString();
            }
        }

        public static List<ClientOption> ClientOptionsFromSettingString(string s)
        {
            var result = new List<ClientOption>();
            foreach (var clientOption in SplitIntoStrings(s))
            {
                var n = clientOption.IndexOf('=');
                result.Add(new ClientOption(clientOption.Substring(0, n), clientOption.Substring(n + 1)));
            }
            return result;
        }

        public static List<TemplateArgument> TemplateArgumentsFromSettingString(string s)
        {
            var result = new List<TemplateArgument>();
            foreach (var templateArgument in SplitIntoStrings(s))
            {
                var n = templateArgument.IndexOf('=');
                result.Add(new TemplateArgument(templateArgument.Substring(0, n), templateArgument.Substring(n + 1)));
            }
            return result;
        }

        public static string ToSettingString(List<ClientOption> clientOptions)
        {
            return String.Join("\r\n", clientOptions.ConvertAll(o => o.Name + "=" + o.Value).ToArray());
        }

        public static string ToSettingString(List<TemplateArgument> templateArguments)
        {
            return String.Join("\r\n", templateArguments.ConvertAll(o => o.Name + "=" + o.Value).ToArray());
        }

        private static IEnumerable<string> SplitIntoStrings(string fullString)
        {
            var strings = new List<string>(fullString.Split('\r', '\n'));
            strings.RemoveAll(String.IsNullOrEmpty);
            return strings;
        }
    }
}