// -----------------------------------------------------------------------
// <copyright file="MobileBrowserCapabilities.cs" company="">
// TODO: Update copyright text.
// </copyright>
// -----------------------------------------------------------------------

namespace Hemrika.SharePresence.WebSite.Modules.MobileModule
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Web;
    using Microsoft.SharePoint.Administration;
    using System.Security.Permissions;
    using System.Collections;
    using System.Configuration;
    using System.Web.Mobile;
    using System.Globalization;
    using System.ComponentModel;
    using Microsoft.SharePoint.WebPartPages;
    using Hemrika.SharePresence.WebSite.Adapters;
    using Microsoft.SharePoint.WebControls;
    using Hemrika.SharePresence.WebSite.MetaData;
    using System.Web.UI.HtmlControls;
    using System.Diagnostics;

    /// <summary>
    /// TODO: Update summary.
    /// </summary>
    public class MobileBrowserCapabilities : HttpBrowserCapabilities
    {
        //private IDictionary _items;
        private static object _staticLock = new object();
        private static bool _IsMobileDevice = false;
        //private System.Collections.SortedList _sharepointAdapters;

        public override bool IsMobileDevice
        {
            get
            {
                return _IsMobileDevice; 
            }
        }

        public MobileBrowserCapabilities(): base()
        {
            _IsMobileDevice = false;
        }

        public new IDictionary Capabilities
        {
            get
            {
                if (base.Capabilities == null)
                {
                    lock (_staticLock)
                    {
                        if (base.Capabilities == null)
                        {
                            base.Capabilities = new Hashtable(StringComparer.OrdinalIgnoreCase);
                        }       
                    }
                }

                return base.Capabilities;
            }
            set
            {
                base.Capabilities = value;
            }
        }

        public override string this[string key]
        {
            get
            {
                if (Capabilities != null)
                {
                    if (Capabilities.Contains(key))
                    {
                        return Capabilities[key].ToString();
                    }
                }
                return string.Empty;
            }
        }

        private void SharePresenceAdapters(IDictionary Adapters)
        {
            //Debug.WriteLine("Begin Adapters:" + DateTime.Now);

            //fullName = typeof(HtmlForm).FullName;
            string fullName = "System.Web.UI.HtmlControls.HtmlForm";
            string assemblyQualifiedName = string.Empty;
            
            if (!Adapters.Contains(fullName))
            {
                assemblyQualifiedName = "Hemrika.SharePresence.WebSite.Adapters.WebSiteForm, Hemrika.SharePresence.WebSite, Version=1.0.0.0, Culture=neutral, PublicKeyToken=3421bd1d946bda6c";
                Adapters[fullName] = assemblyQualifiedName;// typeof(WebSiteForm).AssemblyQualifiedName;
            }

            //fullName = typeof(WebPartZone).FullName;
            fullName = "Microsoft.SharePoint.WebPartPages.WebPartZone";

            if (!Adapters.Contains(fullName))
            {
                assemblyQualifiedName = "Hemrika.SharePresence.WebSite.Adapters.WebPartZoneAdapter, Hemrika.SharePresence.WebSite, Version=1.0.0.0, Culture=neutral, PublicKeyToken=3421bd1d946bda6c";
                Adapters[fullName] = assemblyQualifiedName;// typeof(WebPartZoneAdapter).AssemblyQualifiedName;
            }

            //fullName = typeof(WebPart).FullName;
            fullName = "Microsoft.SharePoint.WebPartPages.WebPart";
            if (!Adapters.Contains(fullName))
            {
                assemblyQualifiedName = "Hemrika.SharePresence.WebSite.Adapters.WebPartAdapter, Hemrika.SharePresence.WebSite, Version=1.0.0.0, Culture=neutral, PublicKeyToken=3421bd1d946bda6c";
                Adapters[fullName] = assemblyQualifiedName;//typeof(WebPartAdapter).AssemblyQualifiedName;
            }

            //fullName = typeof(InputFormTextBox).FullName;
            fullName = "Microsoft.SharePoint.WebControls.InputFormTextBox";
            if (!Adapters.Contains(fullName))
            {
                assemblyQualifiedName = "Hemrika.SharePresence.WebSite.Adapters.InputFormTextBoxAdapter, Hemrika.SharePresence.WebSite, Version=1.0.0.0, Culture=neutral, PublicKeyToken=3421bd1d946bda6c";
                Adapters[fullName] = assemblyQualifiedName;// typeof(InputFormTextBoxAdapter).AssemblyQualifiedName;
            }

            //fullName = typeof(RichTextField).FullName;
            fullName = "Microsoft.SharePoint.WebControls.RichTextField";
            if (!Adapters.Contains(fullName))
            {
                assemblyQualifiedName = "Hemrika.SharePresence.WebSite.Adapters.RichTextFieldAdapter, Hemrika.SharePresence.WebSite, Version=1.0.0.0, Culture=neutral, PublicKeyToken=3421bd1d946bda6c";
                Adapters[fullName] = assemblyQualifiedName;//typeof(RichTextFieldAdapter).AssemblyQualifiedName;
            }

            //fullName = typeof(SPRibbonCommandHandler).FullName;
            fullName = "Microsoft.SharePoint.WebControls.SPRibbonCommandHandler";
            if (!Adapters.Contains(fullName))
            {
                assemblyQualifiedName = "Hemrika.SharePresence.WebSite.Adapters.SPRibbonCommandHandlerAdapter, Hemrika.SharePresence.WebSite, Version=1.0.0.0, Culture=neutral, PublicKeyToken=3421bd1d946bda6c";
                Adapters[fullName] = assemblyQualifiedName;//typeof(SPRibbonCommandHandlerAdapter).AssemblyQualifiedName;
            }

            //fullName = typeof(RobotsMetaTag).FullName;
            fullName = "Microsoft.SharePoint.WebControls.RobotsMetaTag";
            if (!Adapters.Contains(fullName))
            {
                assemblyQualifiedName = "Hemrika.SharePresence.WebSite.MetaData.MetaDataControl, Hemrika.SharePresence.WebSite, Version=1.0.0.0, Culture=neutral, PublicKeyToken=3421bd1d946bda6c";
                Adapters[fullName] = assemblyQualifiedName;//typeof(MetaDataControl).AssemblyQualifiedName;
            }

            //fullName = typeof(HtmlGenericControl).FullName;
            fullName = "System.Web.UI.HtmlControls.HtmlGenericControl";
            if (!Adapters.Contains(fullName))
            {
                assemblyQualifiedName = "Hemrika.SharePresence.WebSite.Adapters.MicroDataAdapter, Hemrika.SharePresence.WebSite, Version=1.0.0.0, Culture=neutral, PublicKeyToken=3421bd1d946bda6c";
                Adapters[fullName] = assemblyQualifiedName;//typeof(MicroDataAdapter).AssemblyQualifiedName;
            }

            //fullName = typeof(BaseFieldControl).FullName;
            fullName = "Microsoft.SharePoint.WebControls.BaseFieldControl";
            if (!Adapters.Contains(fullName))
            {
                assemblyQualifiedName = "Hemrika.SharePresence.WebSite.Adapters.BaseFieldControlAdapter, Hemrika.SharePresence.WebSite, Version=1.0.0.0, Culture=neutral, PublicKeyToken=3421bd1d946bda6c";
                Adapters[fullName] = assemblyQualifiedName;//typeof(BaseFieldControlAdapter).AssemblyQualifiedName;
            }

            //Debug.WriteLine("End Adapters:" + DateTime.Now);
        }

        public MobileBrowserCapabilities(HttpBrowserCapabilities browserCaps)
            : this()
        {
            _IsMobileDevice = false;

            if (browserCaps != null)
            {
                if (browserCaps.Adapters != null && browserCaps.Adapters.Count > 0)
                {
                    foreach (object key in browserCaps.Adapters.Keys)
                    {
                        try
                        {
                            if (Adapters.Contains(key))
                            {
                                Adapters[key] = browserCaps.Adapters[key];
                            }
                            else
                            {
                                Adapters.Add(key, browserCaps.Adapters[key]);
                            }
                        }
                        catch (Exception ex)
                        {
                            SPDiagnosticsService.Local.WriteTrace(0, new SPDiagnosticsCategory(ex.Source, TraceSeverity.High, EventSeverity.Error), TraceSeverity.High, ex.Message, ex.Data);
                        }
                    }
                }

                if (browserCaps.Adapters != null)
                {
                    SharePresenceAdapters(Adapters);
                }

                if (browserCaps.Capabilities != null && browserCaps.Capabilities.Count > 0)
                {
                    foreach (object key in browserCaps.Capabilities.Keys)
                    {
                        try
                        {
                            if (Capabilities.Contains(key))
                            {
                                Capabilities[key] = browserCaps.Capabilities[key];
                            }
                            else
                            {
                                Capabilities.Add(key, browserCaps.Capabilities[key]);
                            }
                        }
                        catch (Exception ex)
                        {
                            SPDiagnosticsService.Local.WriteTrace(0, new SPDiagnosticsCategory(ex.Source, TraceSeverity.High, EventSeverity.Error), TraceSeverity.High, ex.Message, ex.Data);
                        }
                    }
                }

                try
                {
                    //Capabilities = browserCaps.Capabilities;
                    HtmlTextWriter = browserCaps.HtmlTextWriter;
                }
                catch (Exception ex)
                {
                    SPDiagnosticsService.Local.WriteTrace(0, new SPDiagnosticsCategory(ex.Source, TraceSeverity.High, EventSeverity.Error), TraceSeverity.High, ex.Message, ex.Data);
                    //ex.ToString();
                }
            }
        }
    }

    /*
    [AspNetHostingPermission(SecurityAction.InheritanceDemand, Level = AspNetHostingPermissionLevel.Minimal), AspNetHostingPermission(SecurityAction.LinkDemand, Level = AspNetHostingPermissionLevel.Minimal)]
    public class MobileCapabilities : HttpBrowserCapabilities
    {
        // Fields
        private Hashtable _evaluatorResults = Hashtable.Synchronized(new Hashtable());
        private const string _kDeviceFiltersConfig = "system.web/deviceFilters";
        private static readonly object _staticLock = new object();
        public static readonly string PreferredRenderingTypeChtml10 = "chtml10";
        public static readonly string PreferredRenderingTypeHtml32 = "html32";
        public static readonly string PreferredRenderingTypeWml11 = "wml11";
        public static readonly string PreferredRenderingTypeWml12 = "wml12";

        // Methods
        [ConfigurationPermission(SecurityAction.Assert, Unrestricted = true), AspNetHostingPermission(SecurityAction.InheritanceDemand, Level = AspNetHostingPermissionLevel.Minimal), AspNetHostingPermission(SecurityAction.LinkDemand, Level = AspNetHostingPermissionLevel.Minimal)]
        private DeviceFilterDictionary GetCurrentFilters()
        {
            object obj2 = ConfigurationManager.GetSection("system.web/deviceFilters");
            
            //DeviceFiltersSection section = obj2 as DeviceFiltersSection;
            //if (section != null)
            //{
            //    return section.GetDeviceFilters();
            //}
            
            return (DeviceFilterDictionary)obj2;
        }

        public bool HasCapability(string delegateName, string optionalParameter)
        {
            if (string.IsNullOrEmpty(delegateName))
            {
                //throw new ArgumentException(SR.GetString("MobCap_DelegateNameNoValue"), "delegateName");
            }
            DeviceFilterDictionary currentFilters = this.GetCurrentFilters();
            string key = ((currentFilters == null) ? "null" : currentFilters.GetHashCode().ToString(CultureInfo.InvariantCulture)) + delegateName;
            if ((optionalParameter != null) && !this.IsComparisonEvaluator(delegateName))
            {
                key = key + optionalParameter;
            }
            if (this._evaluatorResults.Contains(key))
            {
                return (bool)this._evaluatorResults[key];
            }
            lock (_staticLock)
            {
                bool flag;
                if (this._evaluatorResults.Contains(key))
                {
                    return (bool)this._evaluatorResults[key];
                }
                bool flag2 = this.HasDelegatedEvaluator(delegateName, optionalParameter, out flag);
                if (!flag2)
                {
                    flag2 = this.HasComparisonEvaluator(delegateName, out flag);
                    if (!flag2)
                    {
                        flag2 = this.HasProperty(delegateName, optionalParameter, out flag);
                        if (!flag2)
                        {
                            flag2 = this.HasItem(delegateName, optionalParameter, out flag);
                        }
                    }
                }
                if (!flag2)
                {
                    //throw new ArgumentOutOfRangeException("delegateName", SR.GetString("MobCap_CantFindCapability", new object[] { delegateName }));
                }
                this._evaluatorResults.Add(key, flag);
                return flag;
            }
        }

        private bool HasComparisonEvaluator(string evaluatorName, out bool result)
        {
            string str;
            string str2;
            result = false;
            DeviceFilterDictionary currentFilters = this.GetCurrentFilters();
            if (currentFilters == null)
            {
                return false;
            }
            if (!currentFilters.FindComparisonEvaluator(evaluatorName, out str, out str2))
            {
                return false;
            }
            result = this.HasCapability(str, str2);
            return true;
        }

        private bool HasDelegatedEvaluator(string evaluatorName, string parameter, out bool result)
        {
            EvaluateCapabilitiesDelegate delegate2;
            result = false;
            DeviceFilterDictionary currentFilters = this.GetCurrentFilters();
            if (currentFilters == null)
            {
                return false;
            }
            if (!currentFilters.FindDelegateEvaluator(evaluatorName, out delegate2))
            {
                return false;
            }
            result = delegate2(this, parameter);
            return true;
        }

        private bool HasItem(string evaluatorName, string parameter, out bool result)
        {
            result = false;
            string str = this[evaluatorName];
            if (str == null)
            {
                return false;
            }
            result = str == parameter;
            return true;
        }

        private bool HasProperty(string evaluatorName, string parameter, out bool result)
        {
            result = false;
            PropertyDescriptor descriptor = TypeDescriptor.GetProperties(this)[evaluatorName];
            if (descriptor == null)
            {
                return false;
            }
            string a = descriptor.GetValue(this).ToString();
            StringComparison comparisonType = ((descriptor.PropertyType == typeof(bool)) && (parameter != null)) ? StringComparison.InvariantCultureIgnoreCase : StringComparison.CurrentCulture;
            result = string.Equals(a, parameter, comparisonType);
            return true;
        }

        private bool IsComparisonEvaluator(string evaluatorName)
        {
            DeviceFilterDictionary currentFilters = this.GetCurrentFilters();
            if (currentFilters == null)
            {
                return false;
            }
            return (currentFilters.IsComparisonEvaluator(evaluatorName) && !currentFilters.IsDelegateEvaluator(evaluatorName));
        }

        // Nested Types
        internal delegate bool EvaluateCapabilitiesDelegate(MobileCapabilities capabilities, string evalParameter);
    }

    internal class DeviceFilterDictionary
    {
        // Fields
        private Hashtable _comparisonEvaluators;
        private Hashtable _delegateEvaluators;

        // Methods
        internal DeviceFilterDictionary()
        {
            this._comparisonEvaluators = new Hashtable();
            this._delegateEvaluators = new Hashtable();
        }

        internal DeviceFilterDictionary(DeviceFilterDictionary original)
        {
            this._comparisonEvaluators = (Hashtable)original._comparisonEvaluators.Clone();
            this._delegateEvaluators = (Hashtable)original._delegateEvaluators.Clone();
        }

        internal void AddCapabilityDelegate(string delegateName, MobileCapabilities.EvaluateCapabilitiesDelegate evaluator)
        {
            this._delegateEvaluators[delegateName] = evaluator;
        }

        internal void AddComparisonDelegate(string delegateName, string comparisonName, string argument)
        {
            this._comparisonEvaluators[delegateName] = new ComparisonEvaluator(comparisonName, argument);
            this.CheckForComparisonDelegateLoops(delegateName);
        }

        private void CheckForComparisonDelegateLoops(string delegateName)
        {
            string key = delegateName;
            Hashtable hashtable = new Hashtable();
            while (true)
            {
                ComparisonEvaluator evaluator = (ComparisonEvaluator)this._comparisonEvaluators[key];
                if (evaluator == null)
                {
                    return;
                }
                if (hashtable.Contains(key))
                {
                    //throw new Exception(SR.GetString("DevFiltDict_FoundLoop", new object[] { evaluator.capabilityName, delegateName }));
                }
                hashtable[key] = null;
                key = evaluator.capabilityName;
            }
        }

        internal bool FindComparisonEvaluator(string evaluatorName, out string capabilityName, out string capabilityArgument)
        {
            capabilityName = null;
            capabilityArgument = null;
            ComparisonEvaluator evaluator = (ComparisonEvaluator)this._comparisonEvaluators[evaluatorName];
            if (evaluator == null)
            {
                return false;
            }
            capabilityName = evaluator.capabilityName;
            capabilityArgument = evaluator.capabilityArgument;
            return true;
        }

        internal bool FindDelegateEvaluator(string evaluatorName, out MobileCapabilities.EvaluateCapabilitiesDelegate evaluatorDelegate)
        {
            evaluatorDelegate = null;
            MobileCapabilities.EvaluateCapabilitiesDelegate delegate2 = (MobileCapabilities.EvaluateCapabilitiesDelegate)this._delegateEvaluators[evaluatorName];
            if (delegate2 == null)
            {
                return false;
            }
            evaluatorDelegate = delegate2;
            return true;
        }

        internal bool IsComparisonEvaluator(string evaluatorName)
        {
            return this._comparisonEvaluators.Contains(evaluatorName);
        }

        internal bool IsDelegateEvaluator(string evaluatorName)
        {
            return this._delegateEvaluators.Contains(evaluatorName);
        }

        // Nested Types
        internal class ComparisonEvaluator
        {
            // Fields
            internal readonly string capabilityArgument;
            internal readonly string capabilityName;

            // Methods
            internal ComparisonEvaluator(string name, string argument)
            {
                this.capabilityName = name;
                this.capabilityArgument = argument;
            }
        }
    }
    */
}