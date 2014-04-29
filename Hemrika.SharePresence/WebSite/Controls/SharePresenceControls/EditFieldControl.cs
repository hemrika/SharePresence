// -----------------------------------------------------------------------
// <copyright file="EditFieldControl.cs" company="">
// TODO: Update copyright text.
// </copyright>
// -----------------------------------------------------------------------

namespace Hemrika.SharePresence.WebSite.Controls
{
    using Microsoft.SharePoint;
    using Microsoft.SharePoint.WebControls;
    using System;
    using System.Web.UI;
    using System.Web.UI.WebControls.WebParts;
    using System.Web.UI.WebControls;
    using System.ComponentModel;

    [ToolboxData("<{0}:EditField runat=server></{0}:EditField>")]
    public class EditField : UserControl
    {
        private BaseFieldControl baseControl;
        private string fieldName;
        private SPItem item = SPContext.Current.Item;

        protected void Page_Load(object sender, EventArgs e)
        {
            try
            {
                if (this.item.Fields.ContainsField(this.FieldName))
                {
                    this.baseControl = this.item.Fields[this.FieldName].FieldRenderingControl;
                    WebPartManager currentWebPartManager = WebPartManager.GetCurrentWebPartManager(this.Page);
                    if (currentWebPartManager.DisplayMode == WebPartManager.DesignDisplayMode)
                    {
                        this.baseControl.ControlMode = SPControlMode.Edit;
                    }
                    this.Controls.Add(this.baseControl);
                }
            }
            catch (Exception)
            {
            }
        }

        public BaseFieldControl BaseControl
        {
            get
            {
                return this.baseControl;
            }
            set
            {
                this.baseControl = value;
            }
        }

        [Bindable(true)]
        [Category("SharePresence")]
        [Description(@"SharePoint Field to be used")]
        [DefaultValue("Title")]
        public string FieldName
        {
            get
            {
                return this.fieldName;
            }
            set
            {
                this.fieldName = value;
            }
        }
    }
}