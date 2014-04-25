﻿//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by a tool.
//     Runtime Version:4.0.30319.18449
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace Hemrika.SharePresence.Common {
    using System;
    
    
    /// <summary>
    ///   A strongly-typed resource class, for looking up localized strings, etc.
    /// </summary>
    // This class was auto-generated by the StronglyTypedResourceBuilder
    // class via a tool like ResGen or Visual Studio.
    // To add or remove a member, edit your .ResX file then rerun ResGen
    // with the /str option, or rebuild your VS project.
    [global::System.CodeDom.Compiler.GeneratedCodeAttribute("System.Resources.Tools.StronglyTypedResourceBuilder", "4.0.0.0")]
    [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
    [global::System.Runtime.CompilerServices.CompilerGeneratedAttribute()]
    internal class Resources {
        
        private static global::System.Resources.ResourceManager resourceMan;
        
        private static global::System.Globalization.CultureInfo resourceCulture;
        
        [global::System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1811:AvoidUncalledPrivateCode")]
        internal Resources() {
        }
        
        /// <summary>
        ///   Returns the cached ResourceManager instance used by this class.
        /// </summary>
        [global::System.ComponentModel.EditorBrowsableAttribute(global::System.ComponentModel.EditorBrowsableState.Advanced)]
        internal static global::System.Resources.ResourceManager ResourceManager {
            get {
                if (object.ReferenceEquals(resourceMan, null)) {
                    global::System.Resources.ResourceManager temp = new global::System.Resources.ResourceManager("Hemrika.SharePresence.Common.Resources", typeof(Resources).Assembly);
                    resourceMan = temp;
                }
                return resourceMan;
            }
        }
        
        /// <summary>
        ///   Overrides the current thread's CurrentUICulture property for all
        ///   resource lookups using this strongly typed resource class.
        /// </summary>
        [global::System.ComponentModel.EditorBrowsableAttribute(global::System.ComponentModel.EditorBrowsableState.Advanced)]
        internal static global::System.Globalization.CultureInfo Culture {
            get {
                return resourceCulture;
            }
            set {
                resourceCulture = value;
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Activation error occured while trying to get all instances of type {0}.
        /// </summary>
        internal static string ActivateAllExceptionMessage {
            get {
                return ResourceManager.GetString("ActivateAllExceptionMessage", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Activation error occured while trying to get instance of type {0}, key &quot;{1}&quot;.
        /// </summary>
        internal static string ActivationExceptionMessage {
            get {
                return ResourceManager.GetString("ActivationExceptionMessage", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to The default area can not be added to the configured areas collection..
        /// </summary>
        internal static string AddedDefaultDiagnosticsAreaToCollection {
            get {
                return ResourceManager.GetString("AddedDefaultDiagnosticsAreaToCollection", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to An area must already exist when adding a category, area {0} not found..
        /// </summary>
        internal static string AreaMustExist {
            get {
                return ResourceManager.GetString("AreaMustExist", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to The value for &apos;{0}&apos; must be greater than zero.  Value provided: &apos;{1}&apos;.
        /// </summary>
        internal static string ArgumentMustBeGreaterThanZero {
            get {
                return ResourceManager.GetString("ArgumentMustBeGreaterThanZero", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to The argument &apos;{0}&apos; must not be empty or null..
        /// </summary>
        internal static string ArgumentMustNotBeEmpty {
            get {
                return ResourceManager.GetString("ArgumentMustNotBeEmpty", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to The site URL set for configuration  &apos;{0}&apos; is not valid..
        /// </summary>
        internal static string BadSiteUrl {
            get {
                return ResourceManager.GetString("BadSiteUrl", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Farm configuration cannot be accessed from the sandbox unless the farm proxy is installed..
        /// </summary>
        internal static string CantAccessFarmFromSandbox {
            get {
                return ResourceManager.GetString("CantAccessFarmFromSandbox", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Web application configuration cannot be accessed from the sandbox unless the farm proxy is installed..
        /// </summary>
        internal static string CantAccessWebApplicationFromSandbox {
            get {
                return ResourceManager.GetString("CantAccessWebApplicationFromSandbox", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to There is a category with the name {0}  already added to the properties bag..
        /// </summary>
        internal static string CategoryExistsExceptionMessage {
            get {
                return ResourceManager.GetString("CategoryExistsExceptionMessage", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to The Category &apos;{0}&apos; was not found in the diagnostic categories collections..
        /// </summary>
        internal static string CategoryNotFoundExceptionMessage {
            get {
                return ResourceManager.GetString("CategoryNotFoundExceptionMessage", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Error when deserializing type: &apos;{0}&apos; in the sandbox deserialization.
        /// </summary>
        internal static string ConfigDeserializeError {
            get {
                return ResourceManager.GetString("ConfigDeserializeError", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Configsetting with key &apos;{0}&apos; could not be set &apos;{1}&apos; with type &apos;{2}&apos;. The technical exception was: {3}: {4}.
        /// </summary>
        internal static string ConfigSettingNotSet {
            get {
                return ResourceManager.GetString("ConfigSettingNotSet", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Configuration values stored in the sandbox must be string, int, or implement the IXmlSerializable interface, and have a default constructor.
        /// </summary>
        internal static string ConfigSettingNotSupportedInSandbox {
            get {
                return ResourceManager.GetString("ConfigSettingNotSupportedInSandbox", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to No farm or site context available when setting type mapping..
        /// </summary>
        internal static string ContextMissingSetTypeMappingList {
            get {
                return ResourceManager.GetString("ContextMissingSetTypeMappingList", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to The SPContext was not found. The property bag needs access to the SPContext.Current because it wants to access the current Web..
        /// </summary>
        internal static string ContextNotFound {
            get {
                return ResourceManager.GetString("ContextNotFound", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Creating the content type for configuration data failed..
        /// </summary>
        internal static string CreateConfigContentTypeFailed {
            get {
                return ResourceManager.GetString("CreateConfigContentTypeFailed", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to The name of a category being added to an area was null.
        /// </summary>
        internal static string DiagnosticsAreaAddedCateogoryWithNullName {
            get {
                return ResourceManager.GetString("DiagnosticsAreaAddedCateogoryWithNullName", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to DiagnosticsAreaCollection: Add called with throw on duplicate areas, area already existed.
        /// </summary>
        internal static string DiagnosticsAreaCollectionAddNullException {
            get {
                return ResourceManager.GetString("DiagnosticsAreaCollectionAddNullException", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to The area being added already exists \n\t Value: &apos;{0}&apos;.
        /// </summary>
        internal static string DiagnosticsAreaCollectionAreaExists {
            get {
                return ResourceManager.GetString("DiagnosticsAreaCollectionAreaExists", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Area {0} provided does not exist in the collection to delete.
        /// </summary>
        internal static string DiagnosticsAreaCollectionDeleteAreaDoesntExist {
            get {
                return ResourceManager.GetString("DiagnosticsAreaCollectionDeleteAreaDoesntExist", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Failed to save configuration updates for logger areas and categories to the farm property bag..
        /// </summary>
        internal static string DiagnosticsAreaCollectionSaveConfigFailure {
            get {
                return ResourceManager.GetString("DiagnosticsAreaCollectionSaveConfigFailure", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Attempted to save areas configuration without setting configuration manager.
        /// </summary>
        internal static string DiagnosticsAreaCollectionSaveWithNoConfig {
            get {
                return ResourceManager.GetString("DiagnosticsAreaCollectionSaveWithNoConfig", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to The deserialization of the configuration settings for logging failed..
        /// </summary>
        internal static string DiagnosticsAreaReadXmlConfigDeserializationFailed {
            get {
                return ResourceManager.GetString("DiagnosticsAreaReadXmlConfigDeserializationFailed", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to The DiagnosticsCategory exists, only one entry for a category per area. Value: &apos;{0}&apos;.
        /// </summary>
        internal static string DiagnosticsCategoryExists {
            get {
                return ResourceManager.GetString("DiagnosticsCategoryExists", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to The Site ID guid is empty.  You must provide a valid Site ID..
        /// </summary>
        internal static string EmptySiteGuid {
            get {
                return ResourceManager.GetString("EmptySiteGuid", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to An error occurred while writing tot the Trace Log, trace message:{0}.
        /// </summary>
        internal static string ErrorWritingTrace {
            get {
                return ResourceManager.GetString("ErrorWritingTrace", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Failed deserializing key:&apos;{0}&apos;, serialized data: &apos;{1}&apos;.
        /// </summary>
        internal static string FailedDeserialization {
            get {
                return ResourceManager.GetString("FailedDeserialization", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Failed when attempting to detect if a proxy was installed..
        /// </summary>
        internal static string FailureReadingProxyInstalled {
            get {
                return ResourceManager.GetString("FailureReadingProxyInstalled", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to {0} does not have an implicit conversion defined for {1}.
        /// </summary>
        internal static string ImplicitConversionNotDefined {
            get {
                return ResourceManager.GetString("ImplicitConversionNotDefined", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to The category name should be formatted as a path (area/category), invalid format: {0}.
        /// </summary>
        internal static string InvalidCategoryFormat {
            get {
                return ResourceManager.GetString("InvalidCategoryFormat", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to The config level {0} should not be accessed through the configuration proxy..
        /// </summary>
        internal static string InvalidConfigLevel {
            get {
                return ResourceManager.GetString("InvalidConfigLevel", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Configsetting with key &apos;{0}&apos; could not be retrieved. The configured value could not be converted from &apos;{1}&apos; to an instance of &apos;{2}&apos;. The technical exception was: {3}: {4}.
        /// </summary>
        internal static string InvalidConfigSetting {
            get {
                return ResourceManager.GetString("InvalidConfigSetting", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Key &apos;{0}&apos; must be between 1 and &apos;{1}&apos; in length.  Key Length: &apos;{2}&apos;..
        /// </summary>
        internal static string InvalidKeyLength {
            get {
                return ResourceManager.GetString("InvalidKeyLength", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to The key name provided does not conform to the permitted values, key name: &apos;{0}&apos;..
        /// </summary>
        internal static string InvalidKeyName {
            get {
                return ResourceManager.GetString("InvalidKeyName", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to The key &apos;{0}&apos; cannot be used. Key&apos;s may not be prefixed with the text &apos;{1}&apos;..
        /// </summary>
        internal static string InvalidKeyPrefix {
            get {
                return ResourceManager.GetString("InvalidKeyPrefix", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to The key &apos;{0}&apos; cannot be used. Key&apos;s may not be suffixed with the text &apos;{1}&apos; because this is used by the SPSitePropertyBag to differentiate between properties of the SPWeb and the SPSite..
        /// </summary>
        internal static string InvalidKeySuffix {
            get {
                return ResourceManager.GetString("InvalidKeySuffix", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to The proxy arguments provide where not the required type, provided &apos;{0}&apos;, expected &apos;{1}&apos;..
        /// </summary>
        internal static string InvalidProxyArgProvided {
            get {
                return ResourceManager.GetString("InvalidProxyArgProvided", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to The proxy operation requires a proxy argument type of &apos;{0}&apos;, provided type &apos;{1}&apos;..
        /// </summary>
        internal static string InvalidProxyArgumentType {
            get {
                return ResourceManager.GetString("InvalidProxyArgumentType", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to The SharePointServiceLocator needs to run in a SharePoint context and have access to the SPFarm..
        /// </summary>
        internal static string InvalidRunContext {
            get {
                return ResourceManager.GetString("InvalidRunContext", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to The sandbox event severity was invalid, Value: &apos;{0}&apos;.
        /// </summary>
        internal static string InvalidSandboxEventSeverity {
            get {
                return ResourceManager.GetString("InvalidSandboxEventSeverity", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to The sandbox trace severity was invalid, Value: &apos;{0}&apos;.
        /// </summary>
        internal static string InvalidSandboxTraceSeverity {
            get {
                return ResourceManager.GetString("InvalidSandboxTraceSeverity", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to SPListItem &apos;{0}&apos; does not have a field with Id &apos;{1}&apos; which was mapped to property: &apos;{2}&apos; for entity &apos;{3}&apos;..
        /// </summary>
        internal static string InvalidSPListItem {
            get {
                return ResourceManager.GetString("InvalidSPListItem", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to There was no value configured for key &apos;{0}&apos; in a propertyBag..
        /// </summary>
        internal static string KeyNotConfigured {
            get {
                return ResourceManager.GetString("KeyNotConfigured", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to One or more error occurred while writing messages into the log..
        /// </summary>
        internal static string LoggingExceptionMessage1 {
            get {
                return ResourceManager.GetString("LoggingExceptionMessage1", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to \r\nThe error while writing to the EventLog was:.
        /// </summary>
        internal static string LoggingExceptionMessage2 {
            get {
                return ResourceManager.GetString("LoggingExceptionMessage2", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to \r\n Orginal logged message was: .
        /// </summary>
        internal static string LoggingExceptionMessage3 {
            get {
                return ResourceManager.GetString("LoggingExceptionMessage3", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to {0} must be a non-abstract type with a parameterless constructor.
        /// </summary>
        internal static string NonAbstractType {
            get {
                return ResourceManager.GetString("NonAbstractType", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Sandbox Log, Missing Site Context, Message: &apos;{0}&apos;.
        /// </summary>
        internal static string NoSiteContextLogMessage {
            get {
                return ResourceManager.GetString("NoSiteContextLogMessage", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Sandbox Trace,Missing Site Context, Message:&apos;{0}&apos;.
        /// </summary>
        internal static string NoSiteContextTraceMessage {
            get {
                return ResourceManager.GetString("NoSiteContextTraceMessage", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to This code should run in a SharePoint context. SPContext.Current is null..
        /// </summary>
        internal static string NullSPContext {
            get {
                return ResourceManager.GetString("NullSPContext", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to The property bag for level &apos;{0}&apos; is not accessible in the current context..
        /// </summary>
        internal static string PropertyBagNotValidForContext {
            get {
                return ResourceManager.GetString("PropertyBagNotValidForContext", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Type &apos;{0}&apos; does not have a property &apos;{1}&apos; which was mapped to FieldID: &apos;{2}&apos; for SPListItem &apos;{3}&apos;..
        /// </summary>
        internal static string PropertyTypeNotMapped {
            get {
                return ResourceManager.GetString("PropertyTypeNotMapped", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to The attempt to write to the sandbox log failed..
        /// </summary>
        internal static string SandboxLoggingFailed {
            get {
                return ResourceManager.GetString("SandboxLoggingFailed", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Sandbox Log, SiteID:&apos;{0}&apos;  SiteName:&apos;{1}&apos;  Message:&apos;{2}&apos;.
        /// </summary>
        internal static string SandboxLogMessage {
            get {
                return ResourceManager.GetString("SandboxLogMessage", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to The attempt to trace from the sandbox failed..
        /// </summary>
        internal static string SandboxTraceFailed {
            get {
                return ResourceManager.GetString("SandboxTraceFailed", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Sandbox Trace, SiteID:&apos;{0}&apos;   SiteName:&apos;{1}&apos;  Message:&apos;{2}&apos;.
        /// </summary>
        internal static string SandboxTraceMessage {
            get {
                return ResourceManager.GetString("SandboxTraceMessage", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Failure to save configuration due to contention on &apos;{0}&apos; property bag..
        /// </summary>
        internal static string SaveConfigConcurrencyFailure {
            get {
                return ResourceManager.GetString("SaveConfigConcurrencyFailure", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to ServiceLocator.Current is not supported. Use SharePointServiceLocator.Current instead..
        /// </summary>
        internal static string ServiceLocatorNotSupported {
            get {
                return ResourceManager.GetString("ServiceLocatorNotSupported", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to SPFarm was not found..
        /// </summary>
        internal static string SPFarmNotFound {
            get {
                return ResourceManager.GetString("SPFarmNotFound", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to The index value is not found in the collection: Collection &apos;{0}&apos; Value &apos;{1}&apos; .
        /// </summary>
        internal static string StringIndexOutOfRange {
            get {
                return ResourceManager.GetString("StringIndexOutOfRange", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to No type mapping was registered for type &apos;{0}&apos; and key &apos;{1}&apos;..
        /// </summary>
        internal static string TypeMappingNotRegistered {
            get {
                return ResourceManager.GetString("TypeMappingNotRegistered", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to The type {1} cannot be assigned to variables of type {0}..
        /// </summary>
        internal static string TypesAreNotAssignable {
            get {
                return ResourceManager.GetString("TypesAreNotAssignable", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to The config level &apos;{0}&apos; was not supported. Use Farm, WebApplication, Site or Web..
        /// </summary>
        internal static string UndefinedConfigLevel {
            get {
                return ResourceManager.GetString("UndefinedConfigLevel", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to The sandbox threw an unexpected exception when accessing configuration at the {0} level..
        /// </summary>
        internal static string UnexpectedExceptionFromSandbox {
            get {
                return ResourceManager.GetString("UnexpectedExceptionFromSandbox", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to A category must exist in order to update, category did not exist:{0}&quot;.
        /// </summary>
        internal static string UpdateCategoryCategoryDoesntExist {
            get {
                return ResourceManager.GetString("UpdateCategoryCategoryDoesntExist", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Set value in hierarchical config.\n\tKey: &apos;{0}&apos;\n\tLevel: &apos;{1}&apos;\n\tValue: &apos;{2}&apos;.
        /// </summary>
        internal static string ValueSetInConfig {
            get {
                return ResourceManager.GetString("ValueSetInConfig", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Writes are not permitted to the web application level from the sandbox..
        /// </summary>
        internal static string WriteNotAllowedInSandboxToWebApplication {
            get {
                return ResourceManager.GetString("WriteNotAllowedInSandboxToWebApplication", resourceCulture);
            }
        }
    }
}
