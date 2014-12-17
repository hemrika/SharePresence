// -----------------------------------------------------------------------
// <copyright file="PageLayout.cs" company="Microsoft">
// TODO: Update copyright text.
// </copyright>
// -----------------------------------------------------------------------

namespace Hemrika.SharePresence.WebSite.Navigation
{
	using System;
	using System.Collections.Generic;
	using System.Linq;
	using System.Text;
	using System.Web;
	using System.Configuration.Provider;
	using Microsoft.SharePoint;
	using System.Data;
	using System.Xml;
	using System.Xml.Linq;
	using System.Xml.Xsl;
    using System.Net;
	/// <summary>
	/// TODO: Update summary.
	/// </summary>
	public class WebSiteProvider : SiteMapProvider//ProviderBase
	{
		private WebSiteNodeCollection nodes;
		private SPWeb rootWeb;
		private SPList siteMap;
		private WebSiteNode rootNode;

		public WebSiteProvider()
		{
            ResetProvider();
			InitiateProvider();
		}

		internal void ResetProvider()
		{
			nodes = null;
			rootWeb = null;
			siteMap = null;
			rootNode = null;
            InitiateProvider();
		}

        private void InitiateProvider()
        {
            if (nodes == null)
            {
                nodes = new WebSiteNodeCollection();
            }

            if (rootWeb != null)
            {
                string url = rootWeb.Url;
            }
            SPWeb currentWeb = SPContext.Current.Site.RootWeb;
            string curl = currentWeb.Url;

            if (rootWeb == null || rootWeb != currentWeb)
            {
                SPSite site = SPContext.Current.Site;
                site.CatchAccessDeniedException = false;
                rootWeb = site.RootWeb;
                rootNode = null;
                //if (siteMap == null)
                //{

                    siteMap = rootWeb.Lists.TryGetList("SiteMap");

                    if (rootNode == null && siteMap != null)
                    {
                        SPListItemCollection rootNodes = null;

                        try
                        {
                            SPQuery query = new SPQuery();
                            query.Query = "<Where><Eq><FieldRef Name=\"Parent\" /><Value Type=\"Lookup\">Root</Value></Eq></Where><OrderBy><FieldRef Name=\"Position\" Ascending=\"True\" /></OrderBy>";
                            //query.Query = "<Where><IsNull><FieldRef Name=\"Parent\"/></IsNull></Where>";
                            query.ViewFields = "<FieldRef Name=\"Title\" /><FieldRef Name=\"Description\" /><FieldRef Name=\"URL\" /><FieldRef Name=\"Enabled\" /><FieldRef Name=\"Selected\" /><FieldRef Name=\"Breadcrumb\" /><FieldRef Name=\"Separator\" /><FieldRef Name=\"Icon\" /><FieldRef Name=\"LargeImage\" /><FieldRef Name=\"Parent\" /><FieldRef Name=\"Position\" /><FieldRef Name=\"ID\" />";
                            rootNodes = siteMap.GetItems(query);
                        }
                        catch (Exception ex)
                        {
                            ex.ToString();
                        }

                        /*
                        try
                        {
                            string url = rootWeb.RootFolder.WelcomePage;
                        }
                        catch (Exception)
                        {
                            HttpApplication application = HttpContext.Current.ApplicationInstance;
                            application.Server.ClearError();
                            application.Response.StatusCode = (int)HttpStatusCode.Forbidden;
                            throw (new HttpException((int)HttpStatusCode.Forbidden, ""));
                        }
                        */

                        if (rootNodes != null)
                        {
                            rootNode = new WebSiteNode();
                            //nodes.Add(rootNode);
                            try
                            {

                                foreach (SPListItem item in rootNodes)
                                {
                                    string xmlData = ConvertZRowToRegularXml(item.Xml);

                                    using (System.IO.StringReader sr = new System.IO.StringReader(xmlData))
                                    {
                                        XmlReader reader = XmlReader.Create(sr);
                                        rootNode.ReadXml(reader);
                                    }

                                }

                                if (rootNode.HasChildren())
                                {
                                    //bool isSet = false;
                                    foreach (WebSiteNode cnode in rootNode.Children)
                                    {
                                        try
                                        {

                                            nodes.Add(cnode);
                                            //int i = nodes.Add(cnode);
                                            //if (!isSet)
                                            //{
                                            //    rootNode = (WebSiteNode)nodes[i];
                                            //    isSet = true;
                                            //}
                                            FindChildNodes(cnode);
                                        }
                                        catch (Exception ex)
                                        {
                                            ex.ToString();
                                            //throw;
                                        }

                                    }

                                }
                            }
                            catch (Exception ex)
                            {
                                ex.ToString();
                            }

                        }
                    //}
                }
            }
        }

        private void FindChildNodes(WebSiteNode node)
        {
            InitiateProvider();

            try
            {

                SPQuery query = new SPQuery();
                query.Query = "<Where><Eq><FieldRef Name=\"Parent\" /><Value Type=\"Lookup\">" + node.Title + "</Value></Eq></Where>";
                query.ViewFields = "<FieldRef Name=\"Title\" /><FieldRef Name=\"Description\" /><FieldRef Name=\"URL\" /><FieldRef Name=\"Enabled\" /><FieldRef Name=\"Selected\" /><FieldRef Name=\"Breadcrumb\" /><FieldRef Name=\"Separator\" /><FieldRef Name=\"Icon\" /><FieldRef Name=\"LargeImage\" /><FieldRef Name=\"Parent\" /><FieldRef Name=\"ID\" />";
                SPListItemCollection childeren = siteMap.GetItems(query);
                //DataTable childerenDT = ConvertToTable(childeren);

                if (childeren.Count >= 1)
                {
                    foreach (SPListItem item in childeren)
                    {
                        string xmlData = ConvertZRowToRegularXml(item.Xml);

                        using (System.IO.StringReader sr = new System.IO.StringReader(xmlData))
                        {
                            XmlReader reader = XmlReader.Create(sr);
                            node.ReadXml(reader);
                            //nodes[node].Children.Add(wsnode);
                        }
                    }

                    if (node.HasChildren())
                    {
                        foreach (WebSiteNode cnode in node.Children)
                        {
                            FindChildNodes(cnode);
                        }

                    }

                }
            }
            catch (Exception ex)
            {
                ex.ToString();
                //throw;
            }
        }

		#region transform XML
		
		private static readonly string xsltFromZRowToXml =
				"<xsl:stylesheet version=\"1.0\" " +
				 "xmlns:xsl=\"http://www.w3.org/1999/XSL/Transform\" " +
				 "xmlns:s=\"uuid:BDC6E3F0-6DA3-11d1-A2A3-00AA00C14882\" " +
                 "xmlns:msxsl=\"urn:schemas-microsoft-com:xslt\" "+
                 //"xmlns:docs=\"http://example.com/2010/docs\" "+
                 //"xmlns:fo=\"http://www.w3.org/1999/XSL/Format\" "+
                 //"xmlns:url=\"http://a.unique.identifier/url-methods\" exclude-result-prefixes=\"msxsl\" "+
				 "xmlns:z=\"#RowsetSchema\">" +
			 "<s:Schema id=\"RowsetSchema\"/>" +
			 "<xsl:output method=\"html\" omit-xml-declaration=\"yes\" indent=\"yes\" />" +
			 "<xsl:template match=\"/\">" +
			  "<xsl:text disable-output-escaping=\"yes\">&lt;rows&gt;</xsl:text>" +
			  "<xsl:apply-templates select=\"//z:row\"/>" +
			  "<xsl:text disable-output-escaping=\"yes\">&lt;/rows&gt;</xsl:text>" +
			 "</xsl:template>" +
			 "<xsl:template match=\"z:row\">" +
			  "<xsl:text disable-output-escaping=\"yes\">&lt;row&gt;</xsl:text>" +
			  "<xsl:apply-templates select=\"@*\"/>" +
			  "<xsl:text disable-output-escaping=\"yes\">&lt;/row&gt;</xsl:text>" +
			 "</xsl:template>" +
			 "<xsl:template match=\"@*\">" +
			  "<xsl:text disable-output-escaping=\"yes\">&lt;</xsl:text>" +
                "<xsl:choose>"+
                    "<xsl:when test=\"starts-with(name(),'ows_')\">" +
                        "<xsl:value-of select=\"substring-after(name(), 'ows_')\"/>" +
                    "</xsl:when>"+
                    "<xsl:otherwise>"+
                        "<xsl:value-of select=\"name()\"/>" +
                    "</xsl:otherwise>"+
                "</xsl:choose>"+
			  "<xsl:text disable-output-escaping=\"yes\">&gt;</xsl:text>" +
              //"<xsl:for-each select=\"text()\">"+
              //"<xsl:value-of select=\"url:encode(string(.))\" />" +
              //"</xsl:for-each>"+
              "<xsl:value-of select=\".\" disable-output-escaping=\"yes\" />" +

			  "<xsl:text disable-output-escaping=\"yes\">&lt;/</xsl:text>" +
                "<xsl:choose>" +
                    "<xsl:when test=\"starts-with(name(),'ows_')\">" +
                        "<xsl:value-of select=\"substring-after(name(), 'ows_')\"/>" +
                    "</xsl:when>" +
                    "<xsl:otherwise>" +
                        "<xsl:value-of select=\"name()\"/>" +
                    "</xsl:otherwise>" +
                "</xsl:choose>" +
              "<xsl:text disable-output-escaping=\"yes\">&gt;</xsl:text>" +
			 "</xsl:template>" +
             /*
             "<msxsl:script language=\"JScript\" implements-prefix=\"url\"> "+
                "function encode(string) "+
                    "{"+
                        "return encodeURIComponent(string); "+
                    "}"+
            "</msxsl:script>" +
            */
			"</xsl:stylesheet>";


		private DataTable ConvertToTable(SPListItemCollection itemCollection)
		{
			DataSet ds = new DataSet();

			string xmlData = ConvertZRowToRegularXml(itemCollection.Xml);
			if (string.IsNullOrEmpty(xmlData))
				return null;

			using (System.IO.StringReader sr = new System.IO.StringReader(xmlData))
			{
				ds.ReadXml(sr, XmlReadMode.Auto);

				if (ds.Tables.Count == 0)
					return null;

				return ds.Tables[0];
			}
		}

		private string ConvertZRowToRegularXml(string zRowData)
		{
            XsltSettings settings = new XsltSettings(true,true);
            XslCompiledTransform transform = new XslCompiledTransform();
            XmlDocument tidyXsl = new XmlDocument();

			try
			{
				//Transformer 
				tidyXsl.LoadXml(xsltFromZRowToXml);
                transform.Load(tidyXsl, settings, new XmlUrlResolver());

				//output (result) writers 
                using (System.IO.StringWriter sw = new System.IO.StringWriter())
                {
                    XmlTextWriter tw = new XmlTextWriter(sw);
                    //Source (input) readers 
                    System.IO.StringReader srZRow = new System.IO.StringReader(zRowData);
                    XmlTextReader xtrZRow = new XmlTextReader(srZRow);

                    //Transform 
                    transform.Transform(xtrZRow, null, tw);
                    return sw.ToString();
                }
			}
			catch
			{
				return null;
			}
		}
		#endregion

		public WebSiteNode FindWebSiteNode(string rawUrl)
		{
            return nodes[new Uri(rawUrl)];
			//nodes.Contains()
			
		}

		public override SiteMapNode FindSiteMapNode(string rawUrl)
		{
			return null;// new SiteMapNode(this, "not found");
			//throw new NotImplementedException();
		}
		
		public WebSiteNodeCollection GetChildNodes(WebSiteNode node)
		{
			WebSiteNodeCollection collection = new WebSiteNodeCollection();

			if (node != null)
			{
				List<WebSiteNode> Children = nodes[node].Children;

				foreach (WebSiteNode childNode in Children)
				{
					collection.Add(childNode);
				}
			}
			
			return collection;
		}

		public override SiteMapNodeCollection GetChildNodes(SiteMapNode node)
		{
			//SPListItem item = siteMap.Items[new Guid(node.Key)];
			WebSiteNode websitenode = (WebSiteNode)nodes[node.Key];

			if (websitenode != null && websitenode.HasChildren())
			{
				SiteMapNodeCollection childeren = new SiteMapNodeCollection();
				foreach (WebSiteNode childNode in websitenode.Children)
				{
					childeren.Add(new SiteMapNode(this, childNode.Key, childNode.Url, childNode.Title, childNode.Description));
				}
				return childeren;
			}
			else
			{
				return null;
			}

		}

		public WebSiteNode GetParentNode(WebSiteNode node)
		{
			if (nodes.Count == 0)
			{
				//PopulateSiteMap();
			}

			return nodes[node].Parent;
		}

		public override SiteMapNode GetParentNode(SiteMapNode node)
		{
			throw new NotImplementedException();
		}

		
		protected WebSiteNode GetWebSiteRootNodeCore()
		{
			WebSiteNode rootNode = new WebSiteNode();

			if (nodes.Count == 0)
			{
				//PopulateSiteMap();
			}

			foreach (WebSiteNode node in nodes)
			{
				if (node.Parent == null)
				{
					rootNode = node;
					break;
				}
			}
			return rootNode;
		}
		

		protected override SiteMapNode GetRootNodeCore()
		{
			GetWebSiteRootNodeCore();
			try
			{

				return new SiteMapNode(this, rootNode.Key, rootNode.Url, rootNode.Title, rootNode.Description);
			}
			catch (Exception)
			{

				return null;
			}

		}


		protected void AddNode(WebSiteNode node)
		{
			this.AddNode(node,null);
		}

		protected void AddNode(WebSiteNode node, WebSiteNode parentNode)
		{
			if (nodes.Count == 0)
			{
				//PopulateSiteMap();
			}

			if (parentNode == null)
			{
				nodes.Add(node);
			}
			else
			{
				nodes[parentNode].Children.Add(node);
			}
		}

		public new WebSiteNode CurrentNode {get; set;}
        /*
		{
			get
			{
				return this.CurrentNode;
			}
		}
        */

		public new string Description
		{
			get
			{
				return "WebSiteProvider for SharePresence";
			}
		}

		public new WebSiteNode FindSiteMapNode(HttpContext context)
		{
			return this.FindSiteMapNode(context);
		}

		public new WebSiteNode FindSiteMapNodeFromKey(string key)
		{
			return this.FindSiteMapNodeFromKey(key);
		}

		public new WebSiteNode GetCurrentNodeAndHintAncestorNodes(int upLevel)
		{
			return this.GetCurrentNodeAndHintAncestorNodes(upLevel);
		}

		public new WebSiteNode GetCurrentNodeAndHintNeighborhoodNodes(int upLevel, int downLevel)
		{
			return this.GetCurrentNodeAndHintNeighborhoodNodes(upLevel, downLevel);
		}

		public new WebSiteNode GetParentNodeRelativeToCurrentNodeAndHintDownFromParent(int walkupLevels, int relativeDepthFromWalkup)
		{
			return this.GetParentNodeRelativeToCurrentNodeAndHintDownFromParent(walkupLevels, relativeDepthFromWalkup);
		}

		public WebSiteNode GetParentNodeRelativeToNodeAndHintDownFromParent(WebSiteNode node, int walkupLevels, int relativeDepthFromWalkup)
		{
			return this.GetParentNodeRelativeToNodeAndHintDownFromParent(node, walkupLevels, relativeDepthFromWalkup);
		}

		public void HintAncestorNodes(WebSiteNode node, int upLevel)
		{
			this.HintAncestorNodes(node, upLevel);
		}

		public void HintNeighborhoodNodes(WebSiteNode node, int upLevel, int downLevel)
		{
			this.HintNeighborhoodNodes(node, upLevel, downLevel);
		}

		public new void Initialize(string name, System.Collections.Specialized.NameValueCollection attributes)
		{
			InitiateProvider();
			this.Initialize(name, attributes);
		}

		public bool IsAccessibleToUser(HttpContext context, WebSiteNode node)
		{
			return this.IsAccessibleToUser(context, node);
		}

		public new string Name
		{
			get
			{
				return "WebSiteProvider";
			}
		}

		public new WebSiteProvider ParentProvider
		{
			get
			{
				return this.ParentProvider;
			}
			set
			{
				this.ParentProvider = value;
			}
		}

		protected void RemoveNode(WebSiteNode node)
		{
			this.RemoveNode(node);
		}

		public new WebSiteNode RootNode
		{
			get
			{
				InitiateProvider();
				return rootNode;
			}
		}

		public new WebSiteProvider RootProvider
		{
			get
			{
				return this.RootProvider;
			}
		}

		/*
		public new WebSiteNodeCollection GetChildNodes(WebSiteNode node)
		{
			throw new NotImplementedException();
			
			SiteMapNodeCollection fallback_collection = new SiteMapNodeCollection();
			
			foreach (WebSiteNode webnode in nodes)
			{
				SiteMapNode fallback_node = new SiteMapNode(this.RootProvider, node.Key, node.Url, node.Title, node.Description);
				fallback_collection.Add(fallback_node);
			}
			return fallback_collection;
			
		}

		public new WebSiteNode GetParentNode(WebSiteNode node)
		{
			throw new NotImplementedException();
			
			SiteMapNode fallback_node = new SiteMapNode(this.RootProvider, RootNode.Key, RootNode.Url, RootNode.Title, RootNode.Description);
			return fallback_node;
			
		}
		*/
	}
}
