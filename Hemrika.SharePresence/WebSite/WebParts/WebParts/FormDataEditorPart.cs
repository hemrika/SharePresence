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
    using Hemrika.SharePresence.Common.TemplateEngine;
    using Microsoft.SharePoint.WebControls;

    /// <summary>
    /// TODO: Update summary.
    /// </summary>
    public class FormDataEditorPart : EditorPart, ICallbackEventHandler
    {
        private FormWebPart formWebPart;
        private static string SourceValue;
        protected List<DesignDataSource> sources;
        protected RadioButtonList rbl_Source;
        protected UpdatePanel panel;
        protected Repeater repeater;

        public FormDataEditorPart()
        {
        }

        protected override void CreateChildControls()
        {
            string webUrl = SPContext.Current.Site.RootWeb.Url;

            Controls.Add(new LiteralControl(string.Format("<script type='text/javascript' src='{0}/_layouts/Hemrika/Content/js/json2.js'></script>", webUrl)));
            Controls.Add(new LiteralControl(string.Format("<script type='text/javascript' src='{0}/_layouts/Hemrika/FormWebPart/Hemrika.SharePresence.FormWebPart.js'></script>", webUrl)));

            panel = new UpdatePanel { ID = ID + "UpdatePanel", UpdateMode = UpdatePanelUpdateMode.Always, ChildrenAsTriggers = true, RenderMode = UpdatePanelRenderMode.Block };
            panel.ContentTemplateContainer.Controls.Add(
                new LiteralControl(@"<table cellspacing=""0"" border=""0""
                style=""border-width:0px;width:100%;
                border-collapse:collapse;""><tr><td>
                <div class=""UserSectionHead"">"));

            repeater = new Repeater();
            repeater.FooterTemplate = new FormDataSourceFooterTemplate();
            repeater.ItemTemplate = new FormDataSourceItemTemplate();

            panel.ContentTemplateContainer.Controls.Add(
            new LiteralControl(@"DataSources:<br/>"));
            panel.ContentTemplateContainer.Controls.Add(repeater);
            panel.ContentTemplateContainer.Controls.Add(
                new LiteralControl(@"</div></td></tr>
                <tr><td><div class=""UserSectionHead"">"));

            panel.ContentTemplateContainer.Controls.Add(
            new LiteralControl(@"</div></td></tr></table>"));

            Controls.Add(panel);

            ClientScriptManager cm = Page.ClientScript;
            String cbReference = cm.GetCallbackEventReference(this, "arg", "RefreshFormData", "context", true);
            String updatebackScript = "function UpdateDataSources(arg, context) {" + cbReference + "; }";
            cm.RegisterClientScriptBlock(this.GetType(), "UpdateDataSources", updatebackScript, true);
            String deletebackScript = "function DeleteDataSource(arg, context) {" + cbReference + "; }";
            cm.RegisterClientScriptBlock(this.GetType(), "DeleteDataSource", deletebackScript, true);

            Controls.Add(new LiteralControl("<script>$(document).ready(function() {" + string.Format("{0} = \"{1}\";", "updatepanel", panel.ID) + " });</script>"));
            //Controls.Add(new LiteralControl("<script>$(document).ready(function() {" + string.Format("{0} = \"{1}\";", "DesignEditorPart", this.ID) + " });</script>"));            

            base.CreateChildControls();

            SyncSources();
        }

        /// <summary>
        /// apply changes to web part
        /// </summary>
        /// <returns></returns>
        public override bool ApplyChanges()
        {
            try
            {
                UpdateListSettings();
                formWebPart = WebPartToEdit as FormWebPart;
                if (formWebPart == null) return false;
                formWebPart.DataSourcesData = SourceValue;
            }
            catch (Exception)
            {
                return true;
            }
            return true;
        }

        #region List Settings

        private void UpdateListSettings()
        {
            SPSecurity.RunWithElevatedPrivileges(delegate()
            {
                SPSite site = SPContext.Current.Site;

                foreach (DesignDataSource source in sources)
                {
                    SPWeb web = site.OpenWeb(new Guid(source.WebId));
                    SPList list = web.Lists[new Guid(source.ListId)];

                    if (!list.HasUniqueRoleAssignments)
                    {
                        list.BreakRoleInheritance(true);
                        source.ListInheritanceChange = true;
                    }
                    
                    SPBasePermissions anon = list.AnonymousPermMask64;
                    list.AnonymousPermMask64 = anon | SPBasePermissions.AddListItems;
                    list.Update();
                    source.ListRoleChange = true;
                }
                SourceValue = Utilities.SerializeObject(sources);
            });
        }

        private void ResetListSettings(ref DesignDataSource source)
        {
            DesignDataSource _source = source;
            if (source.ListInheritanceChange || source.ListRoleChange)
            {
                SPSecurity.RunWithElevatedPrivileges(delegate()
                {
                    SPSite site = SPContext.Current.Site;

                    SPWeb web = site.OpenWeb(new Guid(_source.WebId));
                    SPList list = web.Lists[new Guid(_source.ListId)];
                    if (_source.ListInheritanceChange)
                    {
                        list.ResetRoleInheritance();
                        list.Update();
                        _source.ListInheritanceChange = false;
                    }
                    if (_source.ListRoleChange)
                    {
                        SPBasePermissions anon = list.AnonymousPermMask64;
                        list.AnonymousPermMask64 -= SPBasePermissions.AddListItems;
                        list.Update();
                        _source.ListRoleChange = false;
                    }
                });
            }
            source = _source;
        }

        #endregion

        /// <summary>
        /// sync changes from web part
        /// </summary>
        public override void SyncChanges()
        {
            EnsureChildControls();

            formWebPart = WebPartToEdit as FormWebPart;
            if (formWebPart == null) return;

            SourceValue = formWebPart.DataSourcesData;
            SyncSources();
        }

        private void SyncSources()
        {
            try
            {
                if (!string.IsNullOrEmpty(SourceValue))
                {
                    sources = Utilities.DeserializeObject<List<DesignDataSource>>(SourceValue);
                }
            }
            catch (Exception)
            {
            }

            if (sources == null)
            {
                sources = new List<DesignDataSource>();
                SourceValue = string.Empty;
            }

            repeater.DataSource = sources;
            repeater.DataBind();
        }

        private String action = string.Empty;
        private DesignDataSource source = null;
        private string result = string.Empty;
        public string GetCallbackResult()
        {
            return result;
        }

        public void RaiseCallbackEvent(string eventArgument)
        {
            string[] eventArg = eventArgument.Split(new string[1] { "#;" }, StringSplitOptions.None);
            
            action = eventArg[0];
            source = Utilities.DeserializeObject<DesignDataSource>(eventArg[1]);


            formWebPart = WebPartToEdit as FormWebPart;

            if (formWebPart != null)
            {
                if (!string.IsNullOrEmpty(formWebPart.DataSourcesData))
                {
                    sources = Utilities.DeserializeObject<List<DesignDataSource>>(SourceValue);
                }
                
                if(sources == null)
                {
                    sources = new List<DesignDataSource>();
                }

                if (action.ToLower() == "delete")
                {
                    DesignDataSource found = sources.Single(n => n.Id == source.Id);
                    if (found.ListRoleChange)
                    {
                        ResetListSettings(ref found);
                    }
                    if (sources.Remove(found))
                    {
                        result = "DataSource Deleted";
                    }
                }

                if (action.ToLower() == "update")
                {
                    int index = sources.FindIndex(n => n.Id == source.Id);
                    if (index >= 0)
                    {
                        sources[index] = source;
                        result = "DataSource Updated";
                    }
                    else
                    {
                        sources.Add(source);
                        result = "DataSource Added";
                    }
                }

                SourceValue = Utilities.SerializeObject(sources);
                formWebPart.DataSourcesData = SourceValue;
                repeater.DataSource = sources;
                repeater.DataBind();
                this.DataBind(true);
            }
        }
    }
}
