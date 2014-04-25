using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web.UI.WebControls.WebParts;
using System.Reflection;
using System.ComponentModel;
using Hemrika.SharePresence.Common.ServiceLocation;
using Microsoft.SharePoint.WebControls;
using Hemrika.SharePresence.Common.Ribbon;
using Hemrika.SharePresence.Common.Ribbon.Definitions;
using System.Web.UI;
using System.IO;

namespace Hemrika.SharePresence.Common.UI
{
    public abstract class EnhancedWebPart : Microsoft.SharePoint.WebPartPages.WebPart, IWebPartPageComponentProvider
    {
        
        /// <summary>
        /// If you need to use some extra DisplayableClasses for displaying your own classes,
        /// you can use this method to add them from specified assembly list
        /// For example, use:
        ///   return new array Assembly[] { new Assembly(typeof(EnhancedWebPart.SharePoint.DisplayableSPUser)) };
        /// if you are using Sharepoint
        /// </summary>
        protected virtual IEnumerable<Assembly> GetCompositionAssemblies()
        {
            return new List<Assembly>();
        }

        /// <summary>
        /// Override this method to add your own custom editor parts, if you need this.
        /// By default, EnhancedWebPart will automatically create one editor part for each
        /// property category ([Category("category-name-here")] attribute), and one for
        /// all the properties, which were not marked with Category attrubute
        /// </summary>
        protected virtual IEnumerable<EditorPart> GetAdditionalEditorParts()
        {
            return new List<EditorPart>();
        }

        /// <summary>
        /// This method is sealed and cannot be overriden in EnhancedWebPart
        /// If you need to create your custom editor parts, use <see cref="GetAdditionalEditorParts">GetAdditionalEditorParts</cref>
        /// </summary>
        public override EditorPartCollection CreateEditorParts()
        {
            WebPartServiceLocator.Current.Initialize(
                GetCompositionAssemblies()
                .Concat(new Assembly[]
                    {
                        Assembly.GetExecutingAssembly(),
                        Assembly.GetCallingAssembly()
                    })
                );
            
            List<EditorPart> editorParts = new List<EditorPart>(1);

            List<PropertyInfo> enhancedProperties = new List<PropertyInfo>();

            foreach (PropertyInfo pi in this.GetType().GetProperties())
            {
                if (pi.GetCustomAttributes(typeof(EnhancedWebPartPropertyAttribute), false).Count() != 0)
                {
                    enhancedProperties.Add(pi);
                }
            }

            if (enhancedProperties.Count > 0)
            {
                Dictionary<string, List<string>> editorPartsData = new Dictionary<string, List<string>>();
                foreach (PropertyInfo pi in enhancedProperties)
                {
                    CategoryAttribute categoryAttribute = (CategoryAttribute)pi.GetCustomAttributes(typeof(CategoryAttribute), false).FirstOrDefault();
                    if (categoryAttribute != null)
                    {
                        if (!editorPartsData.ContainsKey(categoryAttribute.Category))
                            editorPartsData.Add(categoryAttribute.Category, new List<string>());

                        editorPartsData[categoryAttribute.Category].Add(pi.Name);
                    }
                    else
                    {
                        if (!editorPartsData.ContainsKey("Preferences"))
                            editorPartsData.Add(categoryAttribute.Category, new List<string>());

                        editorPartsData["Preferences"].Add(pi.Name);
                    }
                }

                int i = 0;
                foreach (string key in editorPartsData.Keys)
                {
                    EditorPart part = new EnhancedEditorPart(editorPartsData[key].ToArray());
                    part.ID = "EnhancedEditorPart" + i.ToString();
                    part.Title = key;
                    i++;
                    editorParts.Add(part);
                }

            }

            editorParts.AddRange(GetAdditionalEditorParts());

            EditorPartCollection baseParts = base.CreateEditorParts();

            return new EditorPartCollection(baseParts, editorParts);
        }
        
        /// <summary>
        /// Return contextual group definition for the webpart here
        /// </summary>
        /// <remarks>
        /// If you return null, contextual tab will not be shown
        /// </remarks>
        public abstract ContextualGroupDefinition GetContextualGroupDefinition();

        /// <summary>
        /// Get contextual group definition with Unique Id
        /// </summary>
        private ContextualGroupDefinition GetUniqueContextualGroupDefinition()
        {
            var definition = GetContextualGroupDefinition();
            if (definition != null)
            {
                definition.Id = this.UniqueID.Replace("_", "").Replace("$", "") + definition.Id;
            }

            return definition;
        }

        /// <summary>
        /// IWebPartPageComponentProvider realization
        /// </summary>
        public WebPartContextualInfo WebPartContextualInfo
        {
            get
            {
                WebPartContextualInfo info = new WebPartContextualInfo();

                WebPartManager webPartManager = WebPartManager.GetCurrentWebPartManager(this.Page);
                if (webPartManager.DisplayMode.Name != WebPartManager.BrowseDisplayMode.Name)
                    return info;

                var contextualGroupDefinition = GetUniqueContextualGroupDefinition();
                if (contextualGroupDefinition == null)
                    return info;

                WebPartRibbonContextualGroup contextualGroup = new WebPartRibbonContextualGroup();

                // Contextual group
                contextualGroup.Id = "Ribbon." + contextualGroupDefinition.Id;
                contextualGroup.Command = contextualGroupDefinition.Id + ".EnableContextualGroup";
                contextualGroup.VisibilityContext = contextualGroupDefinition.Id + ".CustomVisibilityContext";
                info.ContextualGroups.Add(contextualGroup);

                // Tabs
                foreach (TabDefinition tab in contextualGroupDefinition.Tabs)
                {
                    WebPartRibbonTab ribbonTab = new WebPartRibbonTab();
                    ribbonTab.Id = "Ribbon." + tab.Id;
                    ribbonTab.VisibilityContext = tab.Id + ".CustomVisibilityContext";
                    info.Tabs.Add(ribbonTab);
                }

                info.PageComponentId = SPRibbon.GetWebPartPageComponentId(this);

                return info;
            }
        }
        

        #region Exception handling section
        private StringBuilder _errorOutput;
        private bool _abortProcessing;
        public virtual bool AbortProcessing
        {
            get { return _abortProcessing; }
            set { _abortProcessing = value; }
        }

        public virtual void HandleException(Exception e, HtmlTextWriter writer)
        {
            #if !DEBUG
                     writer.Write("An Error has Occured in this WebPart.  Please Check your settings and try again");
            #else
                     writer.Write(e.Message + "<br/>" + e.StackTrace);
            #endif

        }

        public void ExceptionHappened(Exception ex)
        {
            AbortProcessing = true;
            HandleException(ex, new HtmlTextWriter(new StringWriter(_errorOutput)));
        }

        #endregion

        #region Override framework methods for method piping
        /// <summary>
        /// 
        /// </summary>
        protected override sealed void CreateChildControls()
        {

            if (!AbortProcessing)
            {
                try
                {
                    CreateWebPartChildControls();
                }
                catch (Exception e)
                {
                    ExceptionHappened(e);
                }
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="e"></param>
        protected override void OnInit(EventArgs e)
        {
            AbortProcessing = false;

            _errorOutput = new StringBuilder();

            try
            {
                InitWebPart(e);
            }
            catch (Exception ex)
            {
                ExceptionHappened(ex);
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="writer"></param>
        protected override sealed void Render(HtmlTextWriter writer)
        {
            StringBuilder tempOutput = new StringBuilder();
            if (!AbortProcessing)
            {
                HtmlTextWriter tempWriter = new HtmlTextWriter(new StringWriter(tempOutput));

                try
                {
                    RenderWebPart(tempWriter);
                }
                catch (Exception ex)
                {
                    ExceptionHappened(ex);
                }
            }
            if (AbortProcessing)
            {
                writer.Write(_errorOutput.ToString());
            }
            else
            {
                writer.Write(tempOutput.ToString());
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="e"></param>
        protected override sealed void OnLoad(EventArgs e)
        {
            if (!AbortProcessing)
            {
                try
                {
                    LoadWebPart(e);
                }
                catch (Exception ex)
                {
                    ExceptionHappened(ex);
                }


            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="e"></param>
        protected override sealed void OnPreRender(EventArgs e)
        {
            if (!AbortProcessing)
            {
                try
                {
                    PreRenderWebPart(e);

                    WebPartManager webPartManager = WebPartManager.GetCurrentWebPartManager(this.Page);
                    if (webPartManager.DisplayMode.Name != WebPartManager.BrowseDisplayMode.Name)
                        return;

                    var contextualGroupDefinition = GetUniqueContextualGroupDefinition();
                    if (contextualGroupDefinition == null)
                        return;

                    RibbonController.Current.AddRibbonContextualTabToPage(contextualGroupDefinition, this.Page);
                }
                catch (Exception ex)
                {
                    ExceptionHappened(ex);
                }

            }
        }
        #endregion

        #region Temp methods for method piping (will be overridden and sealed in subclass)

        /// <summary>
        /// 
        /// </summary>
        /// <param name="writer"></param>
        public new virtual void RenderWebPart(HtmlTextWriter writer)
        {
            EnsureChildControls();
            base.Render(writer);
        }

        /// <summary>
        /// 
        /// </summary>
        public virtual void CreateWebPartChildControls()
        {
            base.CreateChildControls();
            EnsureChildControls();
            //AbortProcessing = true;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="e"></param>
        public virtual void InitWebPart(EventArgs e)
        {
            base.OnInit(e);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="e"></param>
        public virtual void LoadWebPart(EventArgs e)
        {
            base.OnLoad(e);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="e"></param>
        public virtual void PreRenderWebPart(EventArgs e)
        {
            base.OnPreRender(e);
        }
        #endregion
    }
}
