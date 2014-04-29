// -----------------------------------------------------------------------
// <copyright file="MenuBase.cs" company="">
// TODO: Update copyright text.
// </copyright>
// -----------------------------------------------------------------------

namespace Hemrika.SharePresence.WebSite.Navigation
{
    using System;
    using System.Collections.Generic;
    using Hemrika.SharePresence.Common.TemplateEngine;
    using System.Web;
    using System.Web.UI;
    using System.Xml;
    using System.Xml.Serialization;
    using System.Web.Caching;
    using System.IO;
    using System.Web.Compilation;

    public class MenuBase
    {
        internal static MenuBase Instantiate(string ClientId,string ControlType, string ControlStyle)
        {
            try
            {
                
                TemplateDefinition templateDef = TemplateDefinition.FromName(ClientId,ControlType,ControlStyle);
                return new MenuBase { TemplateDef = templateDef };
            }
            catch (Exception exc)
            {
                throw new ApplicationException(String.Format("Couldn't load menu style '{0}': {1}", ControlStyle, exc));
            }
        }

        private Settings menuSettings;
        internal WebSiteNode RootNode { get; set; }
        internal Boolean SkipLocalisation { get; set; }
        protected TemplateDefinition TemplateDef { get; set; }

        private HttpContext currentContext;
        private HttpContext CurrentContext { get { return currentContext ?? (currentContext = HttpContext.Current); } }

        private readonly Dictionary<string, string> nodeSelectorAliases = new Dictionary<string, string>
																		  {
																			{"rootonly", "*,0,0"},
																			{"rootchildren", "+0"},
																			{"currentchildren", "."}
																		  };

        internal void ApplySettings(Settings settings)
        {
            menuSettings = settings;
        }

        internal virtual void PreRender(Page page)
        {
            TemplateDef.AddTemplateArguments(menuSettings.TemplateArguments, true);
            TemplateDef.AddClientOptions(menuSettings.ClientOptions, true);

            if (!String.IsNullOrEmpty(menuSettings.NodeXmlPath))
            {
                LoadNodeXml();
            }
            if (!String.IsNullOrEmpty(menuSettings.NodeSelector))
            {
                ApplyNodeSelector();
            }
            if (!String.IsNullOrEmpty(menuSettings.IncludeNodes))
            {
                FilterNodes(menuSettings.IncludeNodes, false);
            }
            if (!String.IsNullOrEmpty(menuSettings.ExcludeNodes))
            {
                FilterNodes(menuSettings.ExcludeNodes, true);
            }
            if (String.IsNullOrEmpty(menuSettings.NodeXmlPath) && !SkipLocalisation)
            {
                //new Localiser(HostPortalSettings.PortalId).LocaliseNode(RootNode);
            }
            if (!String.IsNullOrEmpty(menuSettings.NodeManipulator))
            {
                ApplyNodeManipulator();
            }

            var imagePathOption =
                menuSettings.ClientOptions.Find(o => o.Name.Equals("PathImage", StringComparison.InvariantCultureIgnoreCase));
            //RootNode.ApplyContext("");
            //	imagePathOption == null ? DNNContext.Current.PortalSettings.HomeDirectory : imagePathOption.Value);

            TemplateDef.PreRender(page);
        }

        internal void Render(HtmlTextWriter htmlWriter)
        {
            htmlWriter.Write("<!-- Begin SharePresence Navigation - {0} template -->", menuSettings.ControlStyle);

            TemplateDef.AddClientOptions(new List<ClientOption> { new ClientString( menuSettings.ControlStyle+"MenuOptions", menuSettings.ControlStyle) }, false);

            //MenuXml menuXml = new MenuXml { root = RootNode };
            //menuXml.ToString();
            TemplateDef.Render(new MenuXml { root = RootNode }, htmlWriter);

            htmlWriter.Write("<!-- End SharePresence Navigation - {0} template -->", menuSettings.ControlStyle);
        }

        private void LoadNodeXml()
        {
            menuSettings.NodeXmlPath = "";
            /*
                MapPath(
                    new PathResolver(TemplateDef.Folder).Resolve(
                        menuSettings.NodeXmlPath,
                        PathResolver.RelativeTo.Manifest,
                        PathResolver.RelativeTo.Skin,
                        PathResolver.RelativeTo.Module,
                        PathResolver.RelativeTo.Portal,
                        PathResolver.RelativeTo.Dnn));
            */
            var cache = CurrentContext.Cache;
            RootNode = cache[menuSettings.NodeXmlPath] as WebSiteNode;
            if (RootNode != null)
            {
                return;
            }

            var doc = new XmlDocument();
            doc.Load(menuSettings.NodeXmlPath);

            // ReSharper disable PossibleNullReferenceException
            using (var reader = doc.CreateNavigator().SelectSingleNode(@"//node[@title]/..").ReadSubtree())
                // ReSharper restore PossibleNullReferenceException
                RootNode = (WebSiteNode)(new XmlSerializer(typeof(WebSiteNode), "").Deserialize(reader));
            cache.Insert(menuSettings.NodeXmlPath, RootNode, new CacheDependency(menuSettings.NodeXmlPath));
        }

        private void FilterNodes(string nodeString, bool exclude)
        {
            var nodeTextStrings = SplitAndTrim(nodeString);
            var filteredNodes = new List<WebSiteNode>();

            foreach (var nodeText in nodeTextStrings)
            {
                if (nodeText.StartsWith("["))
                {
                    var roleName = nodeText.Substring(1, nodeText.Length - 2);
                    //var tc = new TabController();
                    filteredNodes.AddRange(
                        RootNode.Children.FindAll(
                            n =>
                            {
                                //var tab = tc.GetTab(n.TabId, Null.NullInteger, false);
                                /*
								foreach (TabPermissionInfo perm in tab.TabPermissions)
								{
									if (perm.AllowAccess && (perm.PermissionKey == "VIEW") &&
										((perm.RoleID == -1) || (perm.RoleName.ToLowerInvariant() == roleName)))
									{
										return true;
									}
								}
                                */
                                return false;
                            }));
                }
                else
                {
                    var nodeText2 = nodeText;
                    filteredNodes.AddRange(
                        RootNode.Children.FindAll(
                            n =>
                            {
                                var nodeName = n.Description.ToLowerInvariant();
                                var nodeId = n.Id.ToString();
                                return (nodeText2 == nodeName || nodeText2 == nodeId);
                            }));
                }
            }

            RootNode.Children.RemoveAll(n => filteredNodes.Contains(n) == exclude);
        }

        private void ApplyNodeSelector()
        {
            string selector;
            if (!nodeSelectorAliases.TryGetValue(menuSettings.NodeSelector.ToLowerInvariant(), out selector))
            {
                selector = menuSettings.NodeSelector;
            }

            var selectorSplit = SplitAndTrim(selector);

            //var currentTabId = HostPortalSettings.ActiveTab.TabID;

            var newRoot = RootNode;

            var rootSelector = selectorSplit[0];
            if (rootSelector != "*")
            {
                if (rootSelector.StartsWith("-") || rootSelector.StartsWith("+") || rootSelector == "0" || rootSelector == ".")
                {
                    //newRoot = RootNode.FindById(currentTabId);
                    if (newRoot == null)
                    {
                        RootNode = new WebSiteNode();
                        return;
                    }

                    if (rootSelector.StartsWith("-"))
                    {
                        for (var n = Convert.ToInt32(rootSelector); n < 0; n++)
                        {
                            if (newRoot.Parent != null)
                            {
                                newRoot = newRoot.Parent;
                            }
                        }
                    }

                    if (rootSelector.StartsWith("+"))
                    {
                        var currentDepth = newRoot.Depth;
                        var selectorDepth = Convert.ToInt32(rootSelector);
                        if (selectorDepth > currentDepth)
                        {
                            newRoot = new WebSiteNode();
                        }
                        else
                        {
                            for (; selectorDepth < currentDepth; selectorDepth++)
                            {
                                // ReSharper disable PossibleNullReferenceException
                                newRoot = newRoot.Parent;
                                // ReSharper restore PossibleNullReferenceException
                            }
                        }
                    }
                }
                else
                {
                    newRoot = RootNode.FindByNameOrId(rootSelector);
                    if (newRoot == null)
                    {
                        RootNode = new WebSiteNode();
                        return;
                    }
                }
            }

            // ReSharper disable PossibleNullReferenceException
            RootNode = new WebSiteNode(newRoot.Children);
            // ReSharper restore PossibleNullReferenceException

            if (selectorSplit.Count > 1)
            {
                for (var n = Convert.ToInt32(selectorSplit[1]); n > 0; n--)
                {
                    var newChildren = new List<WebSiteNode>();
                    foreach (var child in RootNode.Children)
                    {
                        newChildren.AddRange(child.Children);
                    }
                    RootNode = new WebSiteNode(newChildren);
                }
            }

            if (selectorSplit.Count > 2)
            {
                var newChildren = RootNode.Children;
                for (var n = Convert.ToInt32(selectorSplit[2]); n > 0; n--)
                {
                    var nextChildren = new List<WebSiteNode>();
                    foreach (var child in newChildren)
                    {
                        nextChildren.AddRange(child.Children);
                    }
                    newChildren = nextChildren;
                }
                foreach (var node in newChildren)
                {
                    node.Children = null;
                }
            }
        }

        private void ApplyNodeManipulator()
        {
            RootNode =
                new WebSiteNode(
                    ((INodeManipulator)Activator.CreateInstance(BuildManager.GetType(menuSettings.NodeManipulator, true, true))).
                        ManipulateNodes(RootNode.Children));
        }

        protected string MapPath(string path)
        {
            return String.IsNullOrEmpty(path) ? "" : Path.GetFullPath(CurrentContext.Server.MapPath(path));
        }

        private static List<string> SplitAndTrim(string str)
        {
            return new List<String>(str.Split(',')).ConvertAll(s => s.Trim().ToLowerInvariant());
        }

    }
}