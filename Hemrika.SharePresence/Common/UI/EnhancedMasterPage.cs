// -----------------------------------------------------------------------
// <copyright file="EnhancedMasterPage.cs" company="Microsoft">
// TODO: Update copyright text.
// </copyright>
// -----------------------------------------------------------------------

namespace Hemrika.SharePresence.Common.UI
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using Hemrika.SharePresence.Common.Ribbon.Definitions;
    using Hemrika.SharePresence.Common.Ribbon;
    using Microsoft.SharePoint.WebControls;
    using Microsoft.SharePoint.Administration;

    /// <summary>
    /// TODO: Update summary.
    /// </summary>
    public abstract class EnhancedMasterPage : System.Web.UI.MasterPage
    {
        /// <summary>
        /// Provide ribbon tab definition.
        /// </summary>
        /// <returns>
        /// If you return null here, tab will not be shown.
        /// Overwise, the ribbon tab is created and activated when the page is displayed.
        /// </returns>
        public abstract TabDefinition GetTabDefinition();

        public virtual bool DisplayTab
        {
            get
            {
                return true;
            }

        }

        /// <summary>
        /// Adding ribbon tab to page here
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
