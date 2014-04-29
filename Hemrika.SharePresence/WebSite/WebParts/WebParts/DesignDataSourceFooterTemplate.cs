// -----------------------------------------------------------------------
// <copyright file="DesignDataSourceTemplate.cs" company="">
// TODO: Update copyright text.
// </copyright>
// -----------------------------------------------------------------------

namespace Hemrika.SharePresence.WebSite.WebParts
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Web.UI;
    using System.Web.UI.WebControls;

    /// <summary>
    /// TODO: Update summary.
    /// </summary>
    public class DesignDataSourceFooterTemplate : ITemplate
    {
        public void InstantiateIn(Control container)
        {
            LiteralControl dotted = new LiteralControl(@"</br><div style=""width:100%"" class=""UserDottedLine""></div>");
            container.Controls.Add(dotted);

            LinkButton newItem = new LinkButton();
            newItem.ID = "NewItem";
            newItem.Text = "New";
            newItem.DataBinding += new EventHandler(newItem_DataBinding);
            container.Controls.Add(newItem);

        }

        void newItem_DataBinding(object sender, EventArgs e)
        {
            LinkButton edit = (LinkButton)sender;
            RepeaterItem container = (RepeaterItem)edit.NamingContainer;
            DesignDataSource source = container.DataItem as DesignDataSource;
            edit.OnClientClick = "OpenDesignDataDialog(" + null + ");return false;";
        }
    }
}
