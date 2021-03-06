﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml;
using System.Web.UI;
using System.Runtime.InteropServices;

using Microsoft.SharePoint;
//using Microsoft.Web.CommandUI;
using Microsoft.SharePoint.WebControls;

using  Hemrika.SharePresence.Common.Ribbon.Definitions;
using  Hemrika.SharePresence.Common.Ribbon.Definitions.Controls;
using  Hemrika.SharePresence.Common.Ribbon.Commands;
using Microsoft.SharePoint.Administration;
using System.Reflection;

namespace  Hemrika.SharePresence.Common.Ribbon
{
    /// <summary>
    /// Manage local ribbon customizations (i.e. this affects only current page, and customizations are not stored anywhere, unlike custom actions approach)
    /// </summary>
    public class RibbonController
    {
        #region Singleton

        private static RibbonController instance = null;

        /// <summary>
        /// Singleton instance of RibbonController class
        /// </summary>
        public static RibbonController Current
        {
            get
            {
                if (instance == null)
                    instance = new RibbonController();

                return instance;
            }
        }

        private RibbonController()
        {
        }

        #endregion

        internal void AddRibbonContextualTabToPage(ContextualGroupDefinition definition, Page page)
        {
            page.PreRenderComplete -= new EventHandler(page_PreRenderComplete);
            page.PreRenderComplete += new EventHandler(page_PreRenderComplete);

            AddRibbonExtension(XmlGenerator.Current.GetContextualGroupXML(definition), page, "Ribbon.ContextualTabs", false);
            AddGroupTemplatesRibbonExtensions(definition.Tabs.SelectMany(t => t.GroupTemplates), page);

            RibbonCommandRepository.Current.AddCommands(definition);
        }

        /// <summary>
        /// Add ribbon tab to the specified page
        /// </summary>
        /// <param name="definition">Definition of the ribbon tab</param>
        /// <param name="page">Page, to which the definition will be added</param>
        /// <param name="makeInitial">if true, the ribbon tab will be active when page is loaded, otherwise the default tab (Browse) will be active</param>
        /// <remarks>
        /// <para>
        /// This method is intended to provide ability to add local ribbon customizations. The customizations have to be specified each time 
        /// when the page gets loaded. To add permanent customizations, use <see cref="RibbonCustomAction"/>.
        /// </para>
        /// <para>
        /// This method cannot create contextual tabs, only static.
        /// </para>
        /// </remarks>
        /// <example>
        /// <para>
        /// Example of usage:
        /// </para>
        /// <code lang="csharp">
        /// public class MyPage : LayoutsPageBase
        /// {
        ///     public void Page_Load(object sender, EventArgs e)
        ///     {
        ///         RibbonController.Current.AddRibbonTabToPage(MyRibbonManager.MyTabDefinition, this, true);
        ///     }
        /// }
        /// </code>
        /// <para>
        /// This will add initially active ribbon tab to the application page MyPage.
        /// Tab definition is supposed to be stored in some custom user class MyRibbonManager.
        /// </para>
        /// <para>
        /// Also its possible to use this method for adding tabs to ribbon from webparts. But you must provide different ribbon ids
        /// for different webparts and even for different instances of the same webpart.
        /// </para>
        /// </example>
        public void AddRibbonTabToPage(TabDefinition definition, Page page, bool makeInitial)
        {
            if (SPRibbon.GetCurrent(page) == null)
                throw new Exception("SPRibbon.GetCurrent returned null for the specified page!");
            
            AddRibbonExtension(XmlGenerator.Current.GetTabXML(definition), page, "Ribbon.Tabs", makeInitial);
            AddGroupTemplatesRibbonExtensions(definition.GroupTemplates, page);

            RibbonCommandRepository.Current.AddCommands(definition);
            page.PreRenderComplete -= new EventHandler(page_PreRenderComplete);
            page.PreRenderComplete += new EventHandler(page_PreRenderComplete);

            //TODO
            /*
            SPRibbon ribbon = SPRibbon.GetCurrent(page);
            ribbon.MakeTabAvailable("Ribbon." + definition.Id);
            if (makeInitial)
                ribbon.InitialTabId = "Ribbon." + definition.Id;
            */
        }


        #region Private functions

        private void page_PreRenderComplete(object sender, EventArgs e)
        {
            try
            {
                Page page = sender as Page;
                if (RibbonCommandRepository.Current.GetCommandsCount() > 0)
                    RegisterCommands(page);
            }
            catch (Exception ex)
            {
                SPDiagnosticsService diagSvc = SPDiagnosticsService.Local;
                diagSvc.WriteTrace(0,   new SPDiagnosticsCategory("Ribbon", TraceSeverity.Monitorable, EventSeverity.Error),
                                        TraceSeverity.Monitorable,
                                        "Error occured: " + ex.Message + "\nStackTrace: " + ex.StackTrace);
            }
        }

        private void AddGroupTemplatesRibbonExtensions(IEnumerable<GroupTemplateDefinition> templates, Page page)
        {
            SPRibbon ribbon = SPRibbon.GetCurrent(page);
            XmlDocument ribbonExtensions = new XmlDocument();

            foreach (GroupTemplateDefinition template in templates)
            {
                //TODO
                ribbonExtensions.LoadXml(template.XML);
                //ribbon.RegisterDataExtension(ribbonExtensions.FirstChild, "Ribbon.Templates._children");
            }
        }

        private void AddRibbonExtension(string xml, Page page, string parentId, bool makeInitial)
        {
            //TODO
            SPRibbon ribbon = SPRibbon.GetCurrent(page);
            ribbon.CommandUIVisible = true;
            //ribbon.Minimized = makeInitial ? false : ribbon.Minimized;
            XmlDocument ribbonExtensions = new XmlDocument();
            ribbonExtensions.LoadXml(xml);
            //ribbon.RegisterDataExtension(ribbonExtensions.FirstChild, parentId + "._children");
        }

        private void RegisterCommands(Page page)
        {
            // SPRibbonScriptManager is not avaliable for sandboxed solutions
            //SPRibbonScriptManager ribbonScriptManager = new SPRibbonScriptManager();
            //ribbonScriptManager.RegisterGetCommandsFunction(page, "getGlobalCommands", RibbonCommandConverter.Convert(RibbonCommandRepository.Current.GetCommands()));
            //ribbonScriptManager.RegisterCommandEnabledFunction(page, "commandEnabled", RibbonCommandConverter.Convert(RibbonCommandRepository.Current.GetCommands()));
            //ribbonScriptManager.RegisterHandleCommandFunction(page, "handleCommand", RibbonCommandConverter.Convert(RibbonCommandRepository.Current.GetCommands()));

            
            page.ClientScript.RegisterClientScriptBlock(
                page.GetType(),
                "Hemrika.SharePoint.Content.RibbonCommands",
                ScriptHelper.GetCommandsScript(RibbonCommandRepository.Current.GetCommands()));
            
            page.ClientScript.RegisterClientScriptBlock(
                page.GetType(),
                "InitPageComponentContent",
                ScriptHelper.GetPageComponentScript("Hemrika.SharePoint.Content.Ribbon"),false);
            
            /*
            page.Page.ClientScript.RegisterClientScriptBlock
                (page.GetType(), "RegisterInitializeFunction", ScriptHelper.GetInititalizeFuntion("Hemrika.SharePresence.Common.Ribbon"), false);
            */
            /*
            var manager = new SPRibbonScriptManager();
            var methodInfo = typeof(SPRibbonScriptManager).GetMethod("RegisterInitializeFunction", BindingFlags.Instance | BindingFlags.NonPublic);
            methodInfo.Invoke(manager, new object[] { page, "InitPageComponent", null, false, "Hemrika.SharePresence.Common.Ribbon.PageComponent.initialize()" });
            */

            RibbonCommandRepository.Current.ClearCommands();
        }

        #endregion

    }
}
