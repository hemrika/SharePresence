// <copyright file="CreateWebPageProvisioning.cs" company="Hemrika">
// Copyright Hemrika. All rights reserved.
// </copyright>
// <author>MARKETING\Administrator</author>
// <date>2012-01-17 13:34:08Z</date>
namespace Hemrika.SharePresence.WebSite
{
    using System;
    using System.IO;
    using System.Security.Permissions;
    using System.Text;
    using System.Web;
    using System.Web.UI;
    using System.Web.UI.HtmlControls;
    using System.Web.UI.WebControls;
    using System.Xml;
    using System.Xml.Serialization;
    using Microsoft.SharePoint;
    using Microsoft.SharePoint.Security;
    using Microsoft.SharePoint.Utilities;
    using Microsoft.SharePoint.WebControls;
    using System.Threading;
    using Microsoft.SharePoint.Administration;
    using Hemrika.SharePresence.Common.WebSiteController;

    /// <summary>
    /// TODO: Add comment for CreateWebPageProvisioning
    /// </summary>
    [SharePointPermission(SecurityAction.InheritanceDemand, ObjectModel = true)]
    public class CreateWebPageProvisioning : LayoutsPageBase
    {
        /// <summary>
        /// Initializes a new instance of the CreateWebPageProvisioning class
        /// </summary>
        public CreateWebPageProvisioning()
        {
            this.RightsCheckMode = RightsCheckModes.OnPreInit;
        }

        /// <summary>
        /// Defines which rights are required
        /// </summary>
        protected override SPBasePermissions RightsRequired
        {
            get
            {
                return base.RightsRequired | SPBasePermissions.BrowseUserInfo | SPBasePermissions.ManageLists;
            }
        }

        /// <summary>
        /// Sets the inital values of controls
        /// </summary>
        /// <param name="e">Event arguments</param>
        protected override void OnLoad(EventArgs e)
        {
            try
            {
                using (SPLongOperation longOp = new SPLongOperation(this.Page))
                {
                    longOp.LeadingHTML = "Provisioning your WebSite";
                    //longOp.TrailingHTML = "This may take few seconds.";

                    WaitForJobs(longOp);

                    EndOperation(longOp);
                }
            }
            catch (ThreadAbortException) { /* Thrown when redirected */}
            catch (Exception ex)
            {
                SPUtility.TransferToErrorPage(ex.ToString());
            }
        }

        protected void WaitForJobs(SPLongOperation longOp)
        {
            SPJobDefinitionCollection jobs = SPContext.Current.Site.WebApplication.JobDefinitions;
            int _seconds = 10;

            foreach (SPJobDefinition job in jobs)
            {
                if (job.Name == WebSiteControllerRuleWorkItem.WorkItemJobDisplayName)
                {
                    TimeSpan now = new TimeSpan(DateTime.Now.Ticks);
                    TimeSpan last = new TimeSpan(job.LastRunTime.Ticks);
                    TimeSpan past = now.Subtract(last);
                    int next = 60 - past.Seconds;

                    _seconds += next;
                    break;
                }
            }
            
            longOp.TrailingHTML = "This may take few seconds. " + _seconds.ToString() +" seconds actually.";
            longOp.Begin();

            Thread.Sleep(_seconds*1000);

            return;
        }

        protected void EndOperation(SPLongOperation operation)
        {
            try
            {
                HttpContext context = HttpContext.Current;
                if (context.Request.QueryString["IsDlg"] != null)
                {
                    context.Response.Write("<script type='text/javascript'>window.frameElement.commitPopup();</script>");
                    context.Response.Flush();
                    context.Response.End();
                }
                else
                {
                    string url = SPContext.Current.Web.Url;
                    operation.End(url, SPRedirectFlags.CheckUrl, context, string.Empty);
                }
            }
            catch (ThreadAbortException) { /* Thrown when redirected */}
            catch (Exception ex)
            {
                SPUtility.TransferToErrorPage(ex.ToString());
            }
        }
    


        /*
        Use this code to perform own security checks
        protected virtual void CheckCustomRights()
        {

          bool userCheckedForCustomLogic = false;
          //write here a custom logic to check if user has enough rights to access application page
          //if yes, set userCheckedForCustomLogic to true;

          if (!userCheckedForCustomLogic)
          {
            SPUtility.HandleAccessDenied(new UnauthorizedAccessException());
          }
        } 

        protected override void OnLoad(EventArgs e)
        {
          this.CheckCustomRights();   
        }
        */
    }
}

