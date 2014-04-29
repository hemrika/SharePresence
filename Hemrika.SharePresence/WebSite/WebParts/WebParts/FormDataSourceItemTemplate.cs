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
    using Hemrika.SharePresence.Common.TemplateEngine;

    /// <summary>
    /// TODO: Update summary.
    /// </summary>
    public class FormDataSourceItemTemplate : ITemplate
    {
        public void InstantiateIn(Control container)
        {
            Label title = new Label();
            title.Font.Bold = true;
            title.DataBinding += new EventHandler(title_DataBinding);
            container.Controls.Add(title);

            LiteralControl blank = new LiteralControl("&nbsp;");
            container.Controls.Add(blank);

            LinkButton edit = new LinkButton();
            edit.ID="editItem";
            edit.Text = " Edit ";
            edit.DataBinding += new EventHandler(edit_DataBinding);
            container.Controls.Add(edit);

            LiteralControl blink = new LiteralControl("&nbsp;");
            container.Controls.Add(blink);

            LinkButton delete = new LinkButton();
            delete.ID = "deleteItem";
            delete.Text = " Delete ";
            delete.DataBinding += new EventHandler(delete_DataBinding);
            container.Controls.Add(delete);

            LiteralControl br = new LiteralControl(@"</br>");
            container.Controls.Add(br);

        }

        void delete_DataBinding(object sender, EventArgs e)
        {
            LinkButton delete = (LinkButton)sender;
            RepeaterItem container = (RepeaterItem)delete.NamingContainer;
            DesignDataSource source = container.DataItem as DesignDataSource;
            delete.OnClientClick = "DeleteFormDataDialog(" + Utilities.SerializeObject(source) + ");return false;";
        }

        void edit_DataBinding(object sender, EventArgs e)
        {
            LinkButton edit = (LinkButton)sender;
            RepeaterItem container = (RepeaterItem)edit.NamingContainer;
            DesignDataSource source = container.DataItem as DesignDataSource;
            edit.OnClientClick = "OpenFormDataDialog(" + Utilities.SerializeObject(source) + ");return false;";
            //OpenWebSitePartDialog('{0}', '{1}', '{2}');return false;
        }

        void title_DataBinding(object sender, EventArgs e)
        {
            Label title = (Label)sender;
            RepeaterItem container = (RepeaterItem)title.NamingContainer;
            title.Text = DataBinder.GetPropertyValue(
                container.DataItem, "Title").ToString();
        }
    }
}
