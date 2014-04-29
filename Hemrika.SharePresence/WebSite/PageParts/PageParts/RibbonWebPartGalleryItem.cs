// -----------------------------------------------------------------------
// <copyright file="WebPartGalleryItem.cs" company="">
// TODO: Update copyright text.
// </copyright>
// -----------------------------------------------------------------------

namespace Hemrika.SharePresence.WebSite.PageParts
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using Microsoft.SharePoint.WebPartPages;
    using Microsoft.SharePoint;
    using Microsoft.SharePoint.Utilities;
    using System.IO;
    using Microsoft.SharePoint.Administration;

    public class RibbonWebPartGalleryItem : WebPartGalleryItemBase
    {
        // Fields
        private readonly string category;
        private readonly string description;
        private readonly string dwpUrl;
        private readonly string iconUrl;
        private readonly string id;
        private readonly bool recommended;
        private readonly string title;

        // Methods
        public RibbonWebPartGalleryItem(WebPartGallerySource source,System.Web.UI.Page page, SPListItem item, bool isRecommended)
            : base(source, page)
        {
            Uri uri;
            this.category = item["Group"] as string;
            this.description = item["WebPartDescription"] as string;
            this.id = item.UniqueId.ToString();
            this.title = item["Title"] as string;
            this.iconUrl = GetIconUrl(item);
            this.dwpUrl = item["EncodedAbsUrl"] as string;
            this.recommended = isRecommended;
            /*
            if (string.IsNullOrEmpty(this.dwpUrl))
            {
                string message = WebPartAdder.Text(false, "WebPartAdder_InvalidDwpUrl", new object[] { this.title, "", "" });
                ULS.SendTraceTag(0x746b7169, ULSCat.msoulscat_WSS_WebParts, ULSTraceLevel.High, "{0}", new object[] { message });
                throw new ArgumentException(message, "item");
            }
            */
            this.dwpUrl = SPHttpUtility.UrlKeyValueDecode(this.dwpUrl);
            if ((string.IsNullOrEmpty(this.title) && Uri.TryCreate(this.dwpUrl, UriKind.RelativeOrAbsolute, out uri)) && !string.IsNullOrEmpty(uri.AbsolutePath))
            {
                this.title = Path.GetFileName(uri.AbsolutePath);
            }
        }

        private static string GetIconUrl(SPListItem item)
        {
            string str = item["WebPartPartImageLarge"] as string;
            string str2 = null;
            if (!string.IsNullOrEmpty(str))
            {
                try
                {
                    string assembly = item["WebPartAssembly"] as string;
                    string typeName = item["WebPartTypeName"] as string;
                    str2 = str;//Utility.ReplaceClassResourcePathTokens(str, assembly, typeName);
                }
                catch (Exception ex)
                {
                    SPDiagnosticsService.Local.WriteTrace(0, new SPDiagnosticsCategory(ex.Source, TraceSeverity.High, EventSeverity.Error), TraceSeverity.High, ex.Message, ex.Data);
                }
            }
            return str2;
        }

        public override System.Web.UI.WebControls.WebParts.WebPart Instantiate()
        {
            string fileAsString;
            try
            {
                fileAsString = SPContext.Current.Site.RootWeb.GetFileAsString(this.dwpUrl);
            }
            catch (Exception ex)
            {
                SPDiagnosticsService.Local.WriteTrace(0, new SPDiagnosticsCategory(ex.Source, TraceSeverity.High, EventSeverity.Error), TraceSeverity.High, ex.Message, ex.Data);
                throw;
            }
            System.Web.UI.WebControls.WebParts.WebPart part = base.InstantiateFromXml(fileAsString);
            if (!string.IsNullOrEmpty(this.WebPartContent))
            {
                /*
                if (part is ContentEditorWebPart)
                {
                    ContentEditorWebPart part2 = part as ContentEditorWebPart;
                    XmlDocument document = new XmlDocument();
                    document.LoadXml("<Content></Content>");
                    document.DocumentElement.InnerText = this.WebPartContent;
                    part2.Content = document.DocumentElement;
                    part.ChromeType = PartChromeType.None;
                    return part;
                }
                */
                if (!(part is SilverlightWebPart))
                {
                    return part;
                }
                /*
                SilverlightWebPart slwp = part as SilverlightWebPart;
                if (this.WebPartContent.StartsWith("<"))
                {
                    SPExternalApplication appInfo = null;
                    try
                    {
                        appInfo = SPExternalApplication.ParseFromXml(this.WebPartContent);
                    }
                    catch (Exception)
                    {
                        throw new SPExternalApplicationRegistrationException(SPResource.GetString("InvalidApplicationXml", new object[0]));
                    }
                    slwp.ApplicationXml = SPExternalApplication.RemoveWebPartProperties(this.WebPartContent);
                    SetSilverlightWebPartPropsFromApplicationInfo(slwp, appInfo);
                    return part;
                }
                SPExternalApplication defaultValues = SPExternalApplication.GetDefaultValues();
                slwp.Url = this.WebPartContent;
                SetSilverlightWebPartPropsFromApplicationInfo(slwp, defaultValues);
                */
            }
            return part;
        }

        /*
        private static void SetSilverlightWebPartPropsFromApplicationInfo(SilverlightWebPart slwp, SPExternalApplication appInfo)
        {
            if (!string.IsNullOrEmpty(appInfo.Title))
            {
                slwp.Title = appInfo.Title;
            }
            if (!string.IsNullOrEmpty(appInfo.Description))
            {
                slwp.Description = appInfo.Description;
            }
            Unit height = appInfo.Height;
            if (height == Unit.Empty)
            {
                height = new Unit(SPResource.GetString("SilverlightWebPartDefaultHeight", new object[0]), CultureInfo.InvariantCulture);
            }
            slwp.Height = height;
            Unit width = appInfo.Width;
            if (width == Unit.Empty)
            {
                width = new Unit(SPResource.GetString("SilverlightWebPartDefaultWidth", new object[0]), CultureInfo.InvariantCulture);
            }
            slwp.Width = width;
            slwp.HelpUrl = appInfo.HelpUrl;
            slwp.HelpMode = appInfo.HelpMode;
            slwp.Direction = appInfo.Direction;
            slwp.WindowlessMode = appInfo.WindowlessMode;
            slwp.MinRuntimeVersion = appInfo.MinRuntimeVersion;
        }
        */

        // Properties
        public override string Category
        {
            get
            {
                return this.category;
            }
        }

        public override string Description
        {
            get
            {
                return this.description;
            }
        }

        public override string IconUrl
        {
            get
            {
                return this.iconUrl;
            }
        }

        public override string Id
        {
            get
            {
                return this.id;
            }
        }

        public override bool IsRecommended
        {
            get
            {
                return this.recommended;
            }
        }

        public override bool IsSafeAgainstScript
        {
            get
            {
                return true;
            }
        }

        public override string OnClientAdd
        {
            get
            {
                if (this.dwpUrl.Substring(this.dwpUrl.LastIndexOf("/") + 1).ToLowerInvariant() == "silverlight.webpart")
                {
                    return "AddSilverlightWebPart";
                }
                return null;
            }
        }

        public override string RibbonCommand
        {
            get
            {
                string str = this.dwpUrl.Substring(this.dwpUrl.LastIndexOf("/") + 1).ToLowerInvariant();
                string str2 = null;
                string str3 = str;
                if (str3 == null)
                {
                    return str2;
                }
                if (!(str3 == "mscontenteditor.dwp"))
                {
                    if (str3 != "msimage.dwp")
                    {
                        if (str3 != "silverlight.webpart")
                        {
                            return str2;
                        }
                        return "insertSilverlightWebPart";
                    }
                }
                else
                {
                    return "insertTextWebPart";
                }
                return "insertImageWebPart";
            }
        }

        public override string Title
        {
            get
            {
                return this.title;
            }
        }

        public override string V3PickerKey
        {
            get
            {
                return (SPHttpUtility.UrlKeyValueEncode(this.dwpUrl) + ";" + SPHttpUtility.UrlKeyValueEncode(this.Title));
            }
        }
    }
}
