// -----------------------------------------------------------------------
// <copyright file="ErrorModule.cs" company="">
// TODO: Update copyright text.
// </copyright>
// -----------------------------------------------------------------------
//Hemrika.SharePresence.WebSite.Modules.ErrorModule.ErrorModule, Hemrika.SharePresence.WebSite, Version=1.0.0.0, Culture=neutral, PublicKeyToken=3421bd1d946bda6c

namespace Hemrika.SharePresence.WebSite.Modules.LicenseModule
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using Hemrika.SharePresence.Common.WebSiteController;
using System.Web;
    using System.Text.RegularExpressions;
    using System.Globalization;
    using Microsoft.SharePoint;
    using System.IO;
    using System.Net;
    using System.Web.Hosting;
    using System.ComponentModel;
    using Hemrika.SharePresence.SPLicense.LicenseFile;

    /// <summary>
    /// TODO: Update summary.
    /// </summary>
    public class LicenseModule : IWebSiteControllerModule, IDisposable
    {
        /// <summary>
        /// The GUID to identify this module
        /// </summary>
        /// 
        //private const string ID = "17A3219B-049F-4056-9566-37590122BE8E";
        private Guid ID;
        private HttpApplication application;
        private bool LicenseProviderInitialized;
        static object locker = new object();
        /// <summary>
        /// Initializes a new instance of the <see cref="ErrorModule"/> class.
        /// </summary>
        public LicenseModule()
        {
        }

        /// <summary>
        /// Gets the type of the rule.
        /// </summary>
        /// <value>The type of the rule.</value>
        public string RuleType
        {
            get { return this.GetType().FullName; }
        }

        /// <summary>
        /// Gets the short type of the rule for display purposes.
        /// </summary>
        /// <value>The short type of the rule for display purposes.</value>
        public string ShortRuleType
        {
            get { return this.GetType().Name; }
        }

        
        /// <summary>
        /// Gets the id associated with this module.
        /// </summary>
        /// <value>The id associated with this module.</value>
        public Guid Id
        {
            get { return ID; }
            set { ID = value; }
        }

        /// <summary>
        /// Gets the virutal path to the control used to update properties for this rule type.
        /// </summary>
        /// <value>The control.</value>
        public string Control
        {
            get { return "~/_controltemplates/Hemrika/Controls/LicenseModuleControl.ascx"; }
        }

        /// <summary>
        /// Attaches to appropriate request pipeline events
        /// </summary>
        /// <param name="pageControllerModule">The PageControllerModule whose event the module can attach to</param>
        public void Init(WebSiteControllerModule WebSiteControllerModule)
        {
            //WebSiteControllerModule.OnBeginRequest += new EventHandler(WebSiteControllerModule_OnBeginRequest);
        }

        void WebSiteControllerModule_OnBeginRequest(object sender, EventArgs e)
        {
            application = sender as HttpApplication;
            string absolutePath = application.Request.Url.AbsolutePath.ToLower();
            if (absolutePath.Contains(".dll") ||
                absolutePath.Contains(".asmx") ||
                absolutePath.Contains(".svc") ||
                absolutePath.Contains("favicon.ico"))
            {
                return;
            }

            if (!LicenseProviderInitialized)
            {
                lock (locker)
                {
                    if (!LicenseProviderInitialized)
                    {

                        //SPLicenseFile license = SPLicenseFile.LoadFromString("",this.GetType());
                        //license.Validate();
                        LicenseProviderInitialized = true;
                    }
                }
            }
        }


        /// <summary>
        /// Gets the user-friendly name of a property.
        /// </summary>
        /// <param name="propertyName">Name of the property.</param>
        /// <returns>
        /// The friendly name of the property to be used for display purposes
        /// </returns>
        public string GetFriendlyName(string propertyName)
        {
            switch (propertyName)
            {
                case "LicenseDomain":
                    return "LicenseDomain";
                case "LicenseFile":
                    return "LicenseFile";

                default:
                    return propertyName;
            }
        }

        public bool CanBeRemoved
        {
            get { return false; }
        }


        public bool AlwaysRun
        {
            get { return true; }
        }

        public void Dispose() { }
    }
}
