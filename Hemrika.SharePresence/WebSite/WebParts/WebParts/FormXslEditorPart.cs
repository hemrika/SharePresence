// -----------------------------------------------------------------------
// <copyright file="DesignEditorPart.cs" company="">
// TODO: Update copyright text.
// </copyright>
// -----------------------------------------------------------------------

namespace Hemrika.SharePresence.WebSite.WebParts
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Web.UI.WebControls.WebParts;
    using System.Web.UI.WebControls;
    using System.Web.UI;
    using System.Collections;
    using Microsoft.SharePoint;

    /// <summary>
    /// TODO: Update summary.
    /// </summary>
    public class FormXslEditorPart : EditorPart
    {
        protected DropDownList ddl_Controls;
        protected DropDownList ddl_Style;
        protected TextBox tbx_Title;
        protected TextBox tbx_Description;

        protected override void CreateChildControls()
        {

            tbx_Title = new TextBox
            {
                CssClass = "UserInput",
                Width = 176,
                ID = ID + "Title"
            };

            tbx_Description = new TextBox
            {
                CssClass = "UserInput",
                Width = 176,
                ID = ID + "Description"
            };

            // autopostback
            ddl_Controls = new DropDownList
            {
                CssClass = "UserInput",
                Width = 176,
                ID = ID + "Controls",
                AutoPostBack = true
            };

            // autopostback
            ddl_Style = new DropDownList
            {
                CssClass = "UserInput",
                Width = 176,
                ID = ID + "Styles",
                AutoPostBack = true
            };

            ddl_Controls.DataTextField = "Name";
            ddl_Controls.DataValueField = "Name";

            ddl_Controls.SelectedIndexChanged += new EventHandler(ddl_Controls_SelectedIndexChanged);

            ddl_Style.DataTextField = "Name";
            ddl_Style.DataValueField = "Name";

            UpdatePanel panel = new UpdatePanel { ID = ID + "UpdatePanel", UpdateMode = UpdatePanelUpdateMode.Always, ChildrenAsTriggers = true, RenderMode = UpdatePanelRenderMode.Block };
            panel.ContentTemplateContainer.Controls.Add(
                new LiteralControl(@"<table cellspacing=""0"" border=""0""
                style=""border-width:0px;width:100%;
                border-collapse:collapse;""><tr><td>
                <div class=""UserSectionHead"">"));

            panel.ContentTemplateContainer.Controls.Add(
            new LiteralControl(@"Form Title:<br/>"));
            panel.ContentTemplateContainer.Controls.Add(tbx_Title);
            panel.ContentTemplateContainer.Controls.Add(
                new LiteralControl(@"</div></td></tr>
                <tr><td><div class=""UserSectionHead"">"));

            panel.ContentTemplateContainer.Controls.Add(
            new LiteralControl(@"Form Description:<br/>"));
            panel.ContentTemplateContainer.Controls.Add(tbx_Description);
            panel.ContentTemplateContainer.Controls.Add(
                new LiteralControl(@"</div></td></tr>
                <tr><td><div class=""UserSectionHead"">"));

            panel.ContentTemplateContainer.Controls.Add(
                new LiteralControl(@"Control Type:<br/>"));
            panel.ContentTemplateContainer.Controls.Add(ddl_Controls);
            panel.ContentTemplateContainer.Controls.Add(
                new LiteralControl(@"</div></td></tr>
                <tr><td><div class=""UserSectionHead"">"));

            panel.ContentTemplateContainer.Controls.Add(
                new LiteralControl(@"Control Style:<br/>"));
            panel.ContentTemplateContainer.Controls.Add(ddl_Style);
            panel.ContentTemplateContainer.Controls.Add(
                new LiteralControl(@"</div></td></tr>"));
            panel.ContentTemplateContainer.Controls.Add(
                new LiteralControl(@"</div></td></tr><tr><td>
            <div style=""width:100%"" class=""UserDottedLine""></div>
            </td></tr></table>"));

            Controls.Add(panel);
            base.CreateChildControls();

        }

        void ddl_Controls_SelectedIndexChanged(object sender, EventArgs e)
        {
            string control = ddl_Controls.SelectedValue;
            SPList stylelib = SPContext.Current.Site.RootWeb.Lists.TryGetList("Style Library");

            if (!string.IsNullOrEmpty(control))
            {
                SPFolder controlFolder = stylelib.RootFolder.SubFolders[control];
                SPFolderCollection styleFolders = controlFolder.SubFolders;
                List<SPFolder> empty = new List<SPFolder>();
                foreach (SPFolder folder in controlFolder.SubFolders)
                {
                    bool found = false;
                    foreach (SPFile file in folder.Files)
                    {
                        if (string.Compare("definition.xml", file.Name, true) == 0)
                        {
                            found = true;
                            break;
                        }
                    }

                    if (!found)
                    {
                        empty.Add(folder);

                    }
                }
                ddl_Style.DataSource = styleFolders;
                ddl_Style.DataBind();

                foreach (SPFolder folder in empty)
                {
                    ddl_Style.Items.Remove(folder.Name);
                }

                if (ddl_Style.Items.Count == 0)
                {
                    ddl_Controls.Items.Remove(controlFolder.Name);
                }
            }
        }

        /// <summary>
        /// apply changes to web part
        /// </summary>
        /// <returns></returns>
        public override bool ApplyChanges()
        {
            var part = (FormWebPart)WebPartToEdit;
            part.FormType = ddl_Controls.SelectedValue;
            part.FormStyle = ddl_Style.SelectedValue;
            part.Title = part.FormType + " - " + part.FormStyle;
            part.FormTitle = tbx_Title.Text;
            part.FormDesciption = tbx_Description.Text;
            return true;
        }

        /// <summary>
        /// sync changes from web part
        /// </summary>
        public override void SyncChanges()
        {
            EnsureChildControls();
            var part = (FormWebPart)WebPartToEdit;
            tbx_Title.Text = part.FormTitle;
            tbx_Description.Text = part.FormDesciption;
            SyncControls(part.FormType);
            SyncStyles(part.FormStyle);
        }

        private void SyncControls(string ControlType)
        {
            try
            {
                SPList stylelib = SPContext.Current.Site.RootWeb.Lists.TryGetList("Style Library");
                SPFolderCollection controlFolders = stylelib.RootFolder.SubFolders;

                List<SPFolder> empty = new List<SPFolder>();
                bool exists = false;

                foreach (SPFolder folder in controlFolders)
                {
                    if (folder.Name == "Forms")
                    {
                        empty.Add(folder);
                    }

                    if (folder.SubFolders.Count == 0)
                    {
                        empty.Add(folder);
                    }

                    if (folder.Name == ControlType)
                    {
                        exists = true;
                    }
                }

                ddl_Controls.DataSource = controlFolders;
                if (exists)
                {
                    ddl_Controls.SelectedValue = ControlType;
                }
                else
                {
                    ddl_Controls.SelectedIndex = 0;
                }

                ddl_Controls.DataBind();

                foreach (SPFolder folder in controlFolders)
                {
                    bool found = false;
                    foreach (SPFolder subfolder in folder.SubFolders)
                    {

                        foreach (SPFile file in subfolder.Files)
                        {
                            if (string.Compare("definition.xml", file.Name, true) == 0)
                            {
                                string content = SPContext.Current.Site.RootWeb.GetFileAsString(file.Url);
                                if (content.Contains("Processor=\"Design\""))
                                {
                                    found = true;
                                }
                                break;
                            }
                        }
                    }
                    if (!found)
                    {
                        empty.Add(folder);

                    }
                }

                foreach (SPFolder folder in empty)
                {
                    ddl_Controls.Items.Remove(folder.Name);
                }
            }
            catch (Exception ex)
            {
                ex.ToString();
            }
        }

        private void SyncStyles(string style)
        {
            try
            {
                string control = ddl_Controls.SelectedValue;
                SPList stylelib = SPContext.Current.Site.RootWeb.Lists.TryGetList("Style Library");
                if (!string.IsNullOrEmpty(control))
                {
                    SPFolder controlFolder = stylelib.RootFolder.SubFolders[control];
                    SPFolderCollection styleFolders = controlFolder.SubFolders;
                    bool exists = false;
                    List<SPFolder> empty = new List<SPFolder>();
                    foreach (SPFolder folder in controlFolder.SubFolders)
                    {
                        bool found = false;
                        foreach (SPFile file in folder.Files)
                        {
                            if (string.Compare("definition.xml", file.Name, true) == 0)
                            {
                                string content = SPContext.Current.Site.RootWeb.GetFileAsString(file.Url);
                                if (content.Contains("Processor=\"Design\""))
                                {
                                    found = true;
                                }
                                break;
                            }
                        }

                        if (!found)
                        {
                            empty.Add(folder);

                        }

                        if (folder.Name == style)
                        {
                            exists = true;
                        }
                    }

                    ddl_Style.DataSource = styleFolders;
                    if (exists)
                    {
                        ddl_Style.SelectedValue = style;
                    }
                    else
                    {
                        ddl_Style.SelectedIndex = 0;
                    }
                    ddl_Style.DataBind();

                    foreach (SPFolder folder in empty)
                    {
                        ddl_Style.Items.Remove(folder.Name);
                    }

                    if (ddl_Style.Items.Count == 0)
                    {
                        ddl_Controls.Items.Remove(controlFolder.Name);
                    }
                }

            }
            catch (Exception ex)
            {
                ex.ToString();
            }
        }
    }
}
