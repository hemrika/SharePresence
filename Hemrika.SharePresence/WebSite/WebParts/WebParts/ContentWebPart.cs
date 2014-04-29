// -----------------------------------------------------------------------
// <copyright file="DesignWebPart.cs" company="">
// TODO: Update copyright text.
// </copyright>
// -----------------------------------------------------------------------

namespace Hemrika.SharePresence.WebSite.WebParts
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using Hemrika.SharePresence.Common.TemplateEngine;
    using System.Web;
    using System.Web.UI;
    using System.ComponentModel;
    using Microsoft.SharePoint.Utilities;
    using System.Web.UI.WebControls.WebParts;
    using System.Runtime.InteropServices;
    using Hemrika.SharePresence.Common.UI;
    using Microsoft.SharePoint.Security;
    using System.Security.Permissions;
    using Microsoft.SharePoint;
    using System.Xml.Xsl;
    using System.Xml;
    using System.Data;
    using System.IO;
    using System.Globalization;
    using Hemrika.SharePresence.WebSite.MetaData;

    /// <summary>
    /// TODO: Update summary.
    /// </summary>
    [ToolboxItemAttribute(false)]
    [Guid("a0e67f3d-e5ba-4d2d-8c1d-ceb0c15b1725")]
    public class ContentWebPart : WebSitePart, IWebPartMetaData, IPostBackDataHandler, IPostBackEventHandler
    {

        public ContentWebPart()
            : base()
        {
            this.ChromeState = PartChromeState.Normal;
            this.ChromeType = PartChromeType.None;
        }

        protected override void OnInit(EventArgs e)
        {
            base.OnInit(e);
        }

        [SharePointPermission(SecurityAction.Demand, ObjectModel = true)]
        [SharePointPermission(SecurityAction.InheritanceDemand, ObjectModel = true)]
        [SharePointPermission(SecurityAction.LinkDemand, ObjectModel = true)]
        public override void PreRenderWebPart(EventArgs e)
        {
            base.PreRenderWebPart(e);
        }

        [SharePointPermission(SecurityAction.Demand, ObjectModel = true)]
        [SharePointPermission(SecurityAction.InheritanceDemand, ObjectModel = true)]
        [SharePointPermission(SecurityAction.LinkDemand, ObjectModel = true)]
        public override void RenderWebPart(HtmlTextWriter writer)
        {
            base.RenderWebPart(writer);
        }

        public override string ID
        {
            get
            {
                if (base.ID == null)
                {
                    return SPUtility.GetNewIdPrefix(this.Context) + "contentwebpart";
                }

                return base.ID;
            }
            set
            {
                base.ID = value;
            }
        }

        public override Common.Ribbon.Definitions.ContextualGroupDefinition GetContextualGroupDefinition()
        {
            return null;
        }

        public string MetaData()
        {
            return "";
        }

        public bool LoadPostData(string postDataKey, System.Collections.Specialized.NameValueCollection postCollection)
        {
            //throw new NotImplementedException();
            return true;
        }

        public void RaisePostDataChangedEvent()
        {
            //throw new NotImplementedException();
        }

        public void RaisePostBackEvent(string eventArgument)
        {
            //throw new NotImplementedException();
        }
    }
}
