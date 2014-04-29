using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Security.Permissions;
using Microsoft.SharePoint.WebControls;
using Microsoft.SharePoint;
using System.Collections.ObjectModel;

namespace Hemrika.SharePresence.WebSite.Page
{
    [PermissionSet(SecurityAction.LinkDemand, Name = "FullTrust")]
    public sealed class WebSiteRibbonTrimmer : RibbonTrimmer
    {
        private System.Web.UI.Page page;

        public WebSiteRibbonTrimmer(System.Web.UI.Page page, SPRibbon ribbon)
            : base(ribbon)
        {
            this.page = page;
        }

        /*
        protected override bool TrimPublishingGroup()
        {
            
            base.Ribbon.TrimById("Ribbon.PublishTab.Workflow.ManageWorkflow", "WSSPageStateVisibilityContext");
            bool flag = true;
            flag &= base.TrimPublishingGroup();
            AuthoringStates currentState = ConsoleNode.CurrentState(this.page);
            if (ConsoleNode.IsValidStateForNode(AuthoringStates.IsSchedulingEnabledFalse, currentState))
            {
                base.Ribbon.TrimById("Ribbon.PublishTab.Publishing.Schedule", "WSSPageStateVisibilityContext");
            }
            else
            {
                flag = false;
            }
            SPContext current = SPContext.Current;
            if (current != null)
            {
                Collection<Guid> enabledQuickDeployJobsForSite = ContentDeploymentJob.GetEnabledQuickDeployJobsForSite(current.Site);
                if ((enabledQuickDeployJobsForSite == null) || (enabledQuickDeployJobsForSite.Count == 0))
                {
                    base.Ribbon.TrimById("Ribbon.PublishTab.Publishing.QuickDeploy", "WSSPageStateVisibilityContext");
                    return flag;
                }
                return false;
            }
            
            return false;
            
        }
        */

        protected override void TrimPublishTab()
        {
            /*
            bool flag = true;
            flag &= this.TrimPublishingGroup();
            flag &= this.TrimWorkflowGroup();
            if (flag & this.TrimVariationsGroup())
            {
                base.Ribbon.TrimById("Ribbon.PublishTab", "PublishTabTrimmingVisibilityContext");
            }
            */
        }

        /*
        protected override bool TrimWorkflowGroup()
        {
            CachedListItem contextualListItemCached = ConsoleUtilities.ContextualListItemCached;
            if (((contextualListItemCached != null) && (contextualListItemCached.ParentList != null)) && ((contextualListItemCached.ParentList.WorkflowAssociationsCount > 0) && ConsoleUtilities.CheckPermissions(SPBasePermissions.EditListItems)))
            {
                return false;
            }
            base.Ribbon.TrimById("Ribbon.PublishTab.Workflow", "WSSPageStateVisibilityContext");
            return true;
        }
        */
    }
}
