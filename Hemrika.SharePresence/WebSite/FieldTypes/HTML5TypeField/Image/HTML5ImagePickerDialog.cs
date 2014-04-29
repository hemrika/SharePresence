using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.SharePoint.WebControls;
using Hemrika.SharePresence.WebSite.FieldTypes;
using Microsoft.SharePoint;
using System.Web.UI.WebControls;

namespace Hemrika.SharePresence.WebSite.FieldTypes
{
    public class HTML5ImagePickerDialog : PickerDialog
    {
        protected Literal imageHoverStyle;
        /// <summary>
        /// Default constructor
        /// </summary>
        public HTML5ImagePickerDialog() : base(new HTML5ImageQueryControl(), new HTML5ImageResultControl(), new HTML5ImagePicker())
        {
            this.MultiSelect = false;
            this.QueryControl.QueryText = EditorControl.QueryBox;
            this.EditorControl.DialogImage = "/_layouts/Hemrika/Content/HTML5Image_16.png";
        }

        /// <summary>
        /// Load method
        /// </summary>
        /// <param name="e"></param>
        protected override void OnLoad(EventArgs e)
        {
            base.OnLoad(e);
            base.DialogTitle = "Image Picker";
            base.Description = "Please select a image.";
        }

        protected override void OnPreRender(EventArgs e)
        {

            base.OnPreRender(e);
        }

        protected override void CreateChildControls()
        {
            imageHoverStyle = new Literal();
            imageHoverStyle.Text = "<style type=\"text/css\">"
                                + ".thumbnail{position: relative;z-index: 0;}"
                                + ".thumbnail:hover{background-color: transparent;z-index: 50;}"
                                + ".thumbnail span{ position: absolute;background-color: lightyellow;padding: 5px;left: -1000px;border: 1px dashed gray;visibility: hidden !important;color: black;text-decoration: none;}"
                                + ".thumbnail span img{ border-width: 0;padding: 2px;}"
                                + ".thumbnail:hover span{ visibility: visible !important;top: 0;left: 60px;}"
                                + "</style>";
            this.Controls.Add(imageHoverStyle);
            base.CreateChildControls();
        }
        /// <summary>
        /// Editor control
        /// </summary>
        public new HTML5ImagePicker EditorControl
        {
            get { return (HTML5ImagePicker)base.EditorControl; }

        }

        /// <summary>
        /// Query control
        /// </summary>
        public new HTML5ImageQueryControl QueryControl
        {
            get { return (HTML5ImageQueryControl)base.QueryControl; }
        }

        /// <summary>
        /// Result control
        /// </summary>
        public new HTML5ImageResultControl ResultControl
        {
            get { return (HTML5ImageResultControl)base.ResultControl; }
        }

        public new HTML5ImageDataSet.HTML5ImagesDataTable Results
        {
            get
            {
                return (HTML5ImageDataSet.HTML5ImagesDataTable)base.Results;
            }
            set
            {
                base.Results = value;
            }
        }
    }
}
