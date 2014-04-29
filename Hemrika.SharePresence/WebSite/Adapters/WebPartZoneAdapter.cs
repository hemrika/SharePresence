// -----------------------------------------------------------------------
// <copyright file="WebPartZoneAdapter.cs" company="">
// TODO: Update copyright text.
// </copyright>
// -----------------------------------------------------------------------

namespace Hemrika.SharePresence.WebSite.Adapters
{
	using System;
	using System.Collections.Generic;
	using System.Linq;
	using System.Text;
	using System.Web.UI.WebControls.WebParts;
	using System.Web.UI;
	using System.Web.UI.Adapters;
	//using Microsoft.SharePoint.WebPartPages;
	//using Hemrika.SharePresence.HTML5.WebControls.WebParts;
	using System.ComponentModel;
	using System.Web.UI.WebControls;
	using System.IO;
	using System.Globalization;
	using Microsoft.SharePoint.WebPartPages;
	using Microsoft.SharePoint.Utilities;
    using Hemrika.SharePresence.WebSite.WebParts;

	/// <summary>
	/// TODO: Update summary.
	/// </summary>
	public class WebPartZoneAdapter: ControlAdapter
	{
		private Microsoft.SharePoint.WebPartPages.WebPartZone zone;
		#region | Methods [Public] |

		/// <summary>
		/// Generates the target-specific markup for the control to which the control adapter is attached.
		/// </summary>
		/// <param name="writer">The <see cref="HtmlTextWriter"/> to use to render the target-specific output.</param>
		protected override void Render(HtmlTextWriter writer)
		{
			zone = Control as Microsoft.SharePoint.WebPartPages.WebPartZone;

			// if not edit mode, change each table to a div; otherwise, add summary to each table
			if (!IsEditMode(Page))
			{
                if (zone != null && (zone.ID.ToLower() != "main" && zone.ID.ToLower() != "wpz"))
                {
                    RenderWithDiv(writer, zone);
                }
                else
                {
                    base.Render(writer);
                }
			}
			else
			{
                
				// get html from original control
				StringBuilder builder = new StringBuilder();
				using (var baseWriter = new HtmlTextWriter(new StringWriter(builder, CultureInfo.InvariantCulture)))
				{
                    //FindWebSiteParts(baseWriter, zone);
					base.Render(baseWriter);

                    //RenderAddWebPartZoneCell(baseWriter);
				}

                
                string output = builder.ToString();
                if (zone != null && (zone.ID.ToLower() != "main" && zone.ID.ToLower() != "wpz"))
                {
                    output = AddZoneTitleSummary(output);
                    output = RemoveHrefWebPart(output);
                    output = RemoveWebpartMenu(output);
                    output = RemoveWebpartSelection(output);
                }
                //output = CreateAddWebPartScript(output);
				// render base html with summary attribute added to the zone title table
				//output = AddZoneTitleSummary(output);

				
				writer.Write(output);
			}
		}

        private string RemoveHrefWebPart(string output)
        {
            string oldclick = @"<a href=""javascript:"" onclick=""CoreInvoke('ShowWebPartAdder',";
            int startAdderIndex = 0;
            int endAdderIndex = 0;

            while (startAdderIndex != -1)
            {
                startAdderIndex = output.IndexOf(oldclick, startAdderIndex, StringComparison.OrdinalIgnoreCase);

                if (startAdderIndex > 0)
                {
                    endAdderIndex = output.IndexOf("</a>", startAdderIndex, StringComparison.OrdinalIgnoreCase);

                    string adder = output.Substring(startAdderIndex, (endAdderIndex - startAdderIndex) + 3);
                    string padder = adder.Replace("<a", "<p");
                    padder = padder.Replace("</a>", "</p>");
                    padder = padder.Replace(@"href=""javascript:""", string.Empty);
                    output = output.Replace(adder, padder);
                }
            }

            return output;
        }

        private void FindWebSiteParts(HtmlTextWriter writer, Microsoft.SharePoint.WebPartPages.WebPartZone zone)
        {
            for (int i = 0; i < zone.WebParts.Count; i++)
            {
                System.Web.UI.WebControls.WebParts.WebPart webpart = zone.WebParts[i];

                if (webpart.GetType() == typeof(WebSitePart))
                {
                    writer.AddAttribute(HtmlTextWriterAttribute.Class, "WebSitePart");
                    writer.RenderBeginTag(HtmlTextWriterTag.Div);
                    zone.WebParts[i].RenderControl(writer);
                    writer.RenderEndTag();
                }
            }
        }

        private string CreateAddWebPartScript(string output)
        {
            throw new NotImplementedException();
        }

		#endregion


		#region | Methods [Private] |

		/// <summary>
		/// Determines whether the page is in edit mode.
		/// </summary>
		/// <param name="page">The current page.</param>
		/// <returns></returns>
		private static bool IsEditMode(Page page)
		{
			if (page == null)
			{
				return false;
			}
            SPWebPartManager manager = WebPartManager.GetCurrentWebPartManager(page) as SPWebPartManager;
			return (manager != null && manager.GetDisplayMode().AllowPageDesign);
		}


		/// <summary>
		/// Renders the specified <paramref name="zone"/> with a div, instead of the standard table.
		/// </summary>
		/// <param name="writer">The <see cref="HtmlTextWriter"/> to use to render the specific output.</param>
		/// <param name="zone">The <see cref="WebPartZone"/> to render contents for.</param>
		private static void RenderWithDiv(HtmlTextWriter writer, WebPartZoneBase zone)
		{
			// render zone begin tag
			writer.AddAttribute(HtmlTextWriterAttribute.Id, zone.ID);
			if (!String.IsNullOrEmpty(zone.CssClass))
			{
				writer.AddAttribute(HtmlTextWriterAttribute.Class, zone.CssClass);
			}
			writer.AddAttribute(HtmlTextWriterAttribute.Class, "WebSitePartZone-" + zone.LayoutOrientation);
			writer.RenderBeginTag(HtmlTextWriterTag.Div);

			// render web parts within a div
            for (int i = 0; i < zone.WebParts.Count; i++)
            {
                writer.AddAttribute(HtmlTextWriterAttribute.Class, "WebSitePart");
                writer.AddAttribute("orientation", zone.LayoutOrientation.ToString(), false);
                writer.AddAttribute(HtmlTextWriterAttribute.Name, "MSOZoneCell", false);
                string related = "WebPart" + GetQualifier(zone.WebParts[i]);
                writer.AddAttribute("relatedwebpart", related, false);
                if (!zone.WebParts[i].AllowZoneChange)
                {
                    writer.AddAttribute("allowZoneChange", "0", false);
                }
                writer.RenderBeginTag(HtmlTextWriterTag.Div);
                try
                {
                    zone.WebParts[i].RenderControl(writer);
                }
                catch (Exception)
                {
                }

                writer.RenderEndTag();
            }
			// render zone end tag
			writer.RenderEndTag();
		}

        internal static string GetQualifier(System.Web.UI.WebControls.WebParts.WebPart webPart)
        {
            /*
            if (webPart is Microsoft.SharePoint.WebPartPages.WebPart)
            {
                Microsoft.SharePoint.WebPartPages.WebPart part = webPart as Microsoft.SharePoint.WebPartPages.WebPart;
                return part._wpq;
            }
            */
            return webPart.ClientID;
        }

        private static string RemoveWebpartMenu(string html)
        {
            const string startmenu = @"<td align=""right"" class=""ms-WPHeaderTdMenu""";
            const string startnav = @"<nav class=""webpartmenu""";
            //const string replacemenu = @"<td align=""right""></td>";

            int startmenuIndex = 0;
            int endmenuIndex = 0;
            int nextmenuIndex = 0;
            int startnavindex = 0;
            int endnavindex = 0;
            int endheaderindex = 0;
            string menuHtml = string.Empty;
            string navHtml = string.Empty;

            while (startmenuIndex != -1)
            {
                menuHtml = string.Empty;
                navHtml = string.Empty;
                startmenuIndex = 0;
                endmenuIndex = 0;
                nextmenuIndex = 0;
                startnavindex = 0;
                endnavindex = 0;
                endheaderindex = 0;

                startmenuIndex = html.IndexOf(startmenu, StringComparison.OrdinalIgnoreCase);
                if (startmenuIndex > 0)
                {
                    endmenuIndex = html.IndexOf("</td>", startmenuIndex, StringComparison.OrdinalIgnoreCase);
                    endheaderindex = html.IndexOf("</tr>", endmenuIndex, StringComparison.OrdinalIgnoreCase);
                }

                nextmenuIndex = html.IndexOf(startmenu, endmenuIndex, StringComparison.OrdinalIgnoreCase);

                startnavindex = html.IndexOf(startnav, StringComparison.OrdinalIgnoreCase);
                if (startnavindex > 0)
                {
                    endnavindex = html.IndexOf("</nav>", startnavindex, StringComparison.OrdinalIgnoreCase);
                }

                if (nextmenuIndex == -1)
                {
                    nextmenuIndex = endnavindex;
                }

                if (startmenuIndex > 0 && endmenuIndex > 0)
                {
                    menuHtml = html.Substring(startmenuIndex, (endmenuIndex - startmenuIndex) + 5);
                }

                if ((startnavindex > 0 && endnavindex > 0) && (endnavindex <= nextmenuIndex ))
                {
                    navHtml = html.Substring(startnavindex, (endnavindex - startnavindex) + 6);
                }

                if (!string.IsNullOrEmpty(menuHtml) && string.IsNullOrEmpty(navHtml))
                {
                    html = html.Insert(startmenuIndex + 25, "done ");
                    //html = html.Replace(menuHtml, replacemenu);
                }
                else
                {
                    if (!string.IsNullOrEmpty(navHtml))
                    {
                        html = html.Replace(navHtml, string.Empty);
                        navHtml = navHtml.Insert(5, @"description=""WebSitePart Menu"" ");
                    }

                    if (!string.IsNullOrEmpty(menuHtml) && !string.IsNullOrEmpty(navHtml))
                    {
                        html = html.Replace(menuHtml, @"<td align=""right"">"+navHtml+"</td>");
                    }
                }

            }

            return html;
        }

        private string RemoveWebpartSelection(string html)
        {
            const string startselection = @"<td class=""ms-WPHeaderTdSelection""";

            int startselectionIndex = 0;
            int endselectionIndex = 0;

            while (startselectionIndex != -1)
            {

                startselectionIndex = html.IndexOf(startselection, StringComparison.OrdinalIgnoreCase);
                if (startselectionIndex > 0)
                {
                    endselectionIndex = html.IndexOf("</td>", startselectionIndex, StringComparison.OrdinalIgnoreCase);
                }

                if (startselectionIndex >= 0 && endselectionIndex > startselectionIndex)
                {
                    html = html.Remove(startselectionIndex, (endselectionIndex - startselectionIndex));
                }
            }

            return html;
        }

		/// <summary>
		/// Adds a summary attribute to the HTML table tag
		/// </summary>
		/// <param name="html">Original HTML.</param>
		/// <returns>Original HTML with a table summary attribute for the web part zone title.</returns>
		private static string AddZoneTitleSummary(string html)
		{
			/**
			 * A standard web part zone typically renders the following HTML:
			 *     <table cellpadding="1" cellspacing="0" border="0" style="width:100%;"><tr><td align="center" class="ms-SPZoneLabel"><nobr>Header</nobr></td>
			 * This method adds a summary attribute to the <table> element.
			 */

			// init vars
			const string tableTag = "<table";
			const string tableSummaryFormat = @" summary=""This webpart zone represents the {0} area.""";
			const string titlePrefix = @"class=""ms-SPZoneLabel""><nobr>";  // html directly before the name of the zone

			// get index of zone label
			int zoneTitlePrefixIndex = html.IndexOf(titlePrefix, StringComparison.OrdinalIgnoreCase);

			// if zone label exists, ...
			if (zoneTitlePrefixIndex > 0)
			{
				// init vars
				string zoneTitle = "";

				// find the starting position of the actual zone title
				int zoneTitleIndex = zoneTitlePrefixIndex + titlePrefix.Length;

				// loop thru characters to read the title
				while (html[zoneTitleIndex] != '<')
				{
					zoneTitle += html[zoneTitleIndex];
					zoneTitleIndex++;
				}

				// find the position of the zone title's table tag
				int zoneTitleTableIndex = html.Substring(0, zoneTitleIndex).LastIndexOf(tableTag, StringComparison.OrdinalIgnoreCase);
				if (zoneTitleTableIndex >= 0)
				{
					// append the length of the searched-for string to find the first position after it (i.e. "<table| ...")
					zoneTitleTableIndex += tableTag.Length;

					// insert a table summary
					html = html.Insert(zoneTitleTableIndex, String.Format(CultureInfo.CurrentCulture, tableSummaryFormat, zoneTitle));
				}
			}

			// return
			return html;
		}

		#endregion
	}
}