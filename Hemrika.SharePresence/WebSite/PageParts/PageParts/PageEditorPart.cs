// -----------------------------------------------------------------------
// <copyright file="PageEditorPart.cs" company="">
// TODO: Update copyright text.
// </copyright>
// -----------------------------------------------------------------------

namespace Hemrika.SharePresence.WebSite.PageParts
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Web.UI.WebControls.WebParts;
    using System.Web.UI.WebControls;
    using System.Web.UI;
    using Microsoft.SharePoint;

    /// <summary>
    /// TODO: Update summary.
    /// </summary>
    public class PageEditorPart : EditorPart
    {
        protected DropDownList ddl_PageFields;

        protected override void CreateChildControls()
        {
            // autopostback
            ddl_PageFields = new DropDownList
            {
                CssClass = "UserInput",
                Width = 176,
                ID = ID + "PageFields",
                AutoPostBack = true
            };

            SPFieldCollection fields = SPContext.Current.ListItem.ContentType.Fields;

            ddl_PageFields.DataSource = fields;
            ddl_PageFields.DataTextField = "Title";
            ddl_PageFields.DataValueField = "Title";
            ddl_PageFields.DataBind();
            //ddl_PageFields.SelectedIndexChanged += new EventHandler(ddl_PageFields_SelectedIndexChanged);

            var panel = new UpdatePanel { ID = ID + "UpdatePanel" };
            panel.ContentTemplateContainer.Controls.Add(
                new LiteralControl(@"<table cellspacing=""0"" border=""0""
                style=""border-width:0px;width:100%;
                border-collapse:collapse;""><tr><td>
                <div class=""UserSectionHead"">"));

            panel.ContentTemplateContainer.Controls.Add(new LiteralControl(@"Page Fields:<br/>"));
            panel.ContentTemplateContainer.Controls.Add(ddl_PageFields);
            panel.ContentTemplateContainer.Controls.Add(new LiteralControl(@"</div></td></tr></table>"));

            Controls.Add(panel);
            base.CreateChildControls();

        }

        public override bool ApplyChanges()
        {
            var part = (PagePart)WebPartToEdit;
            part.FieldName = ddl_PageFields.SelectedValue;
            part.Title = part.FieldName;
            return true;
        }

        public override void SyncChanges()
        {
            EnsureChildControls();
            var part = (PagePart)WebPartToEdit;
            ddl_PageFields.SelectedValue = part.FieldName;
        }
    }
}
