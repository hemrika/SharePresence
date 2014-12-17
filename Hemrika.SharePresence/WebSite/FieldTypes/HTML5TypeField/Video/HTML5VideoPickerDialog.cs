using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.SharePoint.WebControls;
using Hemrika.SharePresence.WebSite.FieldTypes;
using Microsoft.SharePoint;
using System.Web.UI.WebControls;
using Hemrika.SharePresence.WebSite.FieldTypes.HTML5TypeField.Video;

namespace Hemrika.SharePresence.WebSite.FieldTypes
{
    public class HTML5VideoPickerDialog : PickerDialog
    {
        protected Literal videoHoverStyle;
        /// <summary>
        /// Default constructor
        /// </summary>
        public HTML5VideoPickerDialog() : base(new HTML5VideoQueryControl(), new HTML5VideoResultControl(), new HTML5VideoPicker())
        {
            this.MultiSelect = false;
            this.QueryControl.QueryText = EditorControl.QueryBox;
        }

        /// <summary>
        /// Load method
        /// </summary>
        /// <param name="e"></param>
        protected override void OnLoad(EventArgs e)
        {
            base.OnLoad(e);
            base.DialogTitle = "Video Picker";
            base.Description = "Please select a video.";
        }

        protected override void OnPreRender(EventArgs e)
        {

            base.OnPreRender(e);
        }

        protected override void CreateChildControls()
        {
            videoHoverStyle = new Literal();
            videoHoverStyle.Text = "<style type=\"text/css\">"
                                + ".thumbnail{position: relative;z-index: 0;}"
                                + ".thumbnail:hover{background-color: transparent;z-index: 50;}"
                                + ".thumbnail span{ position: absolute;background-color: lightyellow;padding: 5px;left: -1000px;border: 1px dashed gray;visibility: hidden !important;color: black;text-decoration: none;}"
                                + ".thumbnail span img{ border-width: 0;padding: 2px;}"
                                + ".thumbnail:hover span{ visibility: visible !important;top: 0;left: 60px;}"
                                + "</style>";
            this.Controls.Add(videoHoverStyle);
            base.CreateChildControls();
        }
        /// <summary>
        /// Editor control
        /// </summary>
        public new HTML5VideoPicker EditorControl
        {
            get { return (HTML5VideoPicker)base.EditorControl; }

        }

        /// <summary>
        /// Query control
        /// </summary>
        public new HTML5VideoQueryControl QueryControl
        {
            get { return (HTML5VideoQueryControl)base.QueryControl; }
        }

        /// <summary>
        /// Result control
        /// </summary>
        public new HTML5VideoResultControl ResultControl
        {
            get { return (HTML5VideoResultControl)base.ResultControl; }
        }

        public new HTML5VideoDataSet.HTML5VideosDataTable Results
        {
            get
            {
                return (HTML5VideoDataSet.HTML5VideosDataTable)base.Results;
            }
            set
            {
                base.Results = value;
            }
        }
    }
}
