using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web.UI;
using Hemrika.SharePresence.Common.Ribbon.Definitions;
using Hemrika.SharePresence.Common.Ribbon;
using Microsoft.SharePoint.WebControls;
using Microsoft.SharePoint.Administration;

namespace Hemrika.SharePresence.Common.UI
{
    /// <summary>
    /// <para>
    /// This base class simplifies creation of Delegate Control with custom ribbon tab.
    /// </para>
    /// <para>
    /// You need to inherit from this class rather when from LayoutsPageBase, to use the functionality.
    /// Also, you should override the GetTabDefinition method and provide ribbon tab definition, using <see cref="TabDefinition"/> class.
    /// </para>
    /// </summary>
    public abstract class EnhancedControl : UserControl
    {
        /*
        public new EnhancedLayoutsPage Parent
        {
            get
            {
                return (EnhancedLayoutsPage)Parent;
            }
        }
        */

        /// <summary>
        /// Provide ribbon tab definition.
        /// </summary>
        /// <returns>
        /// If you return null here, tab will not be shown.
        /// Overwise, the ribbon tab is created and activated when the page is displayed.
        /// </returns>
        public abstract TabDefinition GetTabDefinition();

        /// <summary>
        /// Tab needs to be displayed. If false, tab will not be renderred.
        /// </summary>
        public virtual bool DisplayTab
        {
            get
            {
                
                return true;
            }

        }

        /// <summary>
        /// Adding ribbon tab to control's page here
        /// </summary>
        protected override void OnPreRender(EventArgs e)
        {
            base.OnPreRender(e);

            if (!DisplayTab)
                return;

            var tabDefinition = GetTabDefinition();
            try
            {
                if (SPRibbon.GetCurrent(this.Page) == null)
                    return;
                if (tabDefinition != null && !this.DesignMode)
                    RibbonController.Current.AddRibbonTabToPage(tabDefinition, this.Page, false);
            }
            catch (Exception ex)
            {
                SPDiagnosticsService diagSvc = SPDiagnosticsService.Local;
                diagSvc.WriteTrace(0, new SPDiagnosticsCategory("Ribbon", TraceSeverity.Monitorable, EventSeverity.Error),
                                        TraceSeverity.Monitorable,
                                        "Error occured: " + ex.Message + "\nStackTrace: " + ex.StackTrace);
            }
        }


    }
}
