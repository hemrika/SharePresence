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
    public class DesignDataEditorPart : EditorPart, ICallbackEventHandler
    {
        private DesignWebPart designWebPart;
        //private readonly HiddenField _hiddenSourceValue;
        private static string SourceValue;
        protected List<DesignDataSource> sources;
        protected RadioButtonList rbl_Source;
        protected UpdatePanel panel;
        protected Repeater repeater;

        public DesignDataEditorPart()
        {
            //_hiddenSourceValue = new HiddenField { ID = "designdatasource" };
        }

        protected override void CreateChildControls()
        {
            string webUrl = SPContext.Current.Site.RootWeb.Url;
            //Controls.Add(_hiddenSourceValue);

            Controls.Add(new LiteralControl(string.Format("<script type='text/javascript' src='{0}/_layouts/Hemrika/Content/js/json2.js'></script>", webUrl)));
            Controls.Add(new LiteralControl(string.Format("<script type='text/javascript' src='{0}/_layouts/Hemrika/DesignWebPart/Hemrika.SharePresence.DesignWebPart.js'></script>", webUrl)));

            panel = new UpdatePanel { ID = ID + "UpdatePanel", UpdateMode = UpdatePanelUpdateMode.Always, ChildrenAsTriggers = true };
            panel.ContentTemplateContainer.Controls.Add(
                new LiteralControl(@"<table cellspacing=""0"" border=""0""
                style=""border-width:0px;width:100%;
                border-collapse:collapse;""><tr><td>
                <div class=""UserSectionHead"">"));

            repeater = new Repeater();
            repeater.FooterTemplate = new DesignDataSourceFooterTemplate();
            repeater.ItemTemplate = new DesignDataSourceItemTemplate();

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
            String cbReference = cm.GetCallbackEventReference(this, "arg", "RefreshDesignData", "context", true);
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
                designWebPart = WebPartToEdit as DesignWebPart;
                if (designWebPart == null) return false;
                designWebPart.DataSourcesData = SourceValue;
            }
            catch (Exception)
            {
                return true;
            }
            return true;
        }

        /// <summary>
        /// sync changes from web part
        /// </summary>
        public override void SyncChanges()
        {
            EnsureChildControls();

            designWebPart = WebPartToEdit as DesignWebPart;
            if (designWebPart == null) return;

            SourceValue = designWebPart.DataSourcesData;
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

            //Controls.Add(new LiteralControl("<script>$(document).ready(function() { " + string.Format("datasources = {0};", Utilities.SerializeObject(sources)) + " });</script>"));

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


            designWebPart = WebPartToEdit as DesignWebPart;
            if (designWebPart != null)
            {
                if (!string.IsNullOrEmpty(designWebPart.DataSourcesData))
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
                    if (sources.Remove(found))
                    {
                        result = "DataSource Deleted";
                        //designWebPart.DataSourcesData = Utilities.SerializeObject(sources);
                    }
                }

                if (action.ToLower() == "update")
                {
                    int index = sources.FindIndex(n => n.Id == source.Id);
                    if (index >= 0)
                    {
                        sources[index] = source;
                        result = "DataSource Updated";
                        //designWebPart.DataSourcesData = Utilities.SerializeObject(sources);
                    }
                    else
                    {
                        sources.Add(source);
                        result = "DataSource Added";
                        //designWebPart.DataSourcesData = Utilities.SerializeObject(sources);
                    }
                }
                SourceValue = Utilities.SerializeObject(sources);
                designWebPart.DataSourcesData = SourceValue;
                repeater.DataSource = sources;
                repeater.DataBind();
                this.DataBind(true);
            }

            //webpart.DataSourcesData = Utilities.SerializeObject(repeater.DataSource);
        }
    }
}
