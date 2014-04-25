using System;
using System.Globalization;
using System.Data;
using Microsoft.SharePoint;
//using Microsoft.Office.Server.Diagnostics;
using Microsoft.SharePoint.Administration;
using Microsoft.SharePoint.Utilities;
using System.Net;
using Hemrika.SharePresence.Common.WebSiteController;

namespace Hemrika.SharePresence.Administration
{
    public class LicenseChecker : SPJobDefinition
    {
        public LicenseChecker() : base() { }
        public LicenseChecker(string jobName, SPWebApplication webApplication)
            : base(jobName, webApplication, null, SPJobLockType.Job)
        { this.Title = "Check License"; }

        public override void Execute(Guid targetInstanceId)
        {
            try
            {
                this.UpdateProgress(100);
            }
            catch (Exception x)
            {
                x.ToString();
            }
            finally
            {
            }
        }
    }
}
