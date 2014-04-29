using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.SharePoint.WebControls;
using Hemrika.SharePresence.Html5.WebControls;
using Hemrika.SharePresence.WebSite.FieldTypes;
using System.Collections;
using Microsoft.SharePoint;
using System.Web.UI.WebControls;
using System.Web.UI;
using System.IO;
using Hemrika.SharePresence.WebSite.Fields;
using Hemrika.SharePresence.WebSite.Video;
using System.Drawing;
using System.Web;

namespace Hemrika.SharePresence.WebSite.FieldTypes
{
    [ToolboxData("<{0}:HTML5VideoControl runat=server></{0}:HTML5VideoControl>")]
    public class HTML5VideoControl : BaseFieldControl
    {
        protected Hemrika.SharePresence.Html5.WebControls.Video video;
        //protected Hemrika.SharePresence.Html5.WebControls.Image image;
        protected HyperLink upload;
        protected HTML5VideoPicker videoPicker;
        protected HTML5VideoField videoField;
        private VideoSettings settings = null;

        protected override void OnLoad(EventArgs e)
        {
            base.OnLoad(e);
        }

        protected override string DefaultTemplateName
        {
            get
            {
                if (base.ControlMode == SPControlMode.Display)
                {
                    return this.DisplayTemplateName;
                }
                return "HTML5Video";
            }
        }

        public override string DisplayTemplateName
        {
            get
            {
                return "HTML5VideoDisplay";
            }
            set
            {
                base.DisplayTemplateName = value;
            }
        }

        protected override void OnPreRender(EventArgs e)
        {
            base.OnPreRender(e);
        }

        protected override void CreateChildControls()
        {
            this.DisableInputFieldLabel = true;
            base.ControlMode = SPContext.Current.FormContext.FormMode;
            base.CreateChildControls();


            videoField = (HTML5VideoField)ItemFieldValue;

            if (videoField == null)
            {
                videoField = new HTML5VideoField();
            }

            settings = new VideoSettings();
            settings = settings.Load();

            SPFolder folder = null;
            //SPWeb web = null;

            //image = (Hemrika.SharePresence.Html5.WebControls.Image)this.TemplateContainer.FindControl("html_image");
            videoPicker = (HTML5VideoPicker)this.TemplateContainer.FindControl("VideoPicker");
            video = (Hemrika.SharePresence.Html5.WebControls.Video)this.TemplateContainer.FindControl("html_video");

            if (!ChildControlsCreated)
            {

                if (base.ControlMode == SPControlMode.Edit || base.ControlMode == SPControlMode.New)
                {

                    if (!Page.IsCallback)
                    {
                        HTML5VideoPropertyBag properties = new HTML5VideoPropertyBag();

                        if (folder != null)
                        {
                            properties.ItemId = folder.UniqueId;
                            properties.WebId = folder.ParentWeb.ID;
                            properties.ListId = folder.ParentListId;
                        }
                        else
                        {
                            properties.WebId = SPContext.Current.Web.ID;
                            properties.ListId = SPContext.Current.ListId;
                            properties.ItemId = SPContext.Current.ListItem.UniqueId;
                        }

                        if (videoPicker != null)
                        {
                            upload = (HyperLink)videoPicker.FindControl("upload");

                            if (upload != null)
                            {
                                upload.NavigateUrl = String.Format(upload.NavigateUrl, video.ClientID, videoPicker.ClientID, base.Web.ID);
                            }

                            videoPicker.OnValueChangedClientScript = "UpdateVideoAfterDialog('" + video.ClientID + "','" + videoPicker.ClientID + "');";
                            videoPicker.AfterCallbackClientScript = "UpdateVideoAfterDialog('" + video.ClientID + "','" + videoPicker.ClientID + "');";

                            if (folder != null)
                            {
                                HTML5VideoPickerEntity entity = new HTML5VideoPickerEntity(folder.Item);

                                videoPicker.CustomProperty = properties.ToString();
                                videoPicker.Entities.Add(entity);
                                videoPicker.UpdateEntities(videoPicker.Entities);
                            }
                        }
                    }
                }

                if (video != null)
                {
                    videoField = (HTML5VideoField)ItemFieldValue;

                    if (videoField != null)
                    {
                        if (!Page.ClientScript.IsClientScriptIncludeRegistered("Video"))
                        {
                            Page.ClientScript.RegisterClientScriptInclude(typeof(HTML5VideoControl), "Video", "/_layouts/Hemrika/Content/js/video.js");
                        }

                        CssRegistration.Register("/_layouts/Hemrika/Content/css/video-js.css");

                        if (!Page.ClientScript.IsClientScriptBlockRegistered("VideoFlash"))
                        {
                            Page.ClientScript.RegisterClientScriptBlock(typeof(HTML5VideoControl), "VideoFlash", "_V_.options.flash.swf = \"" + ItemContext.Site.Url + "/_layouts/Hemrika/Content/js/video-js.swf\";", true);
                        }

                        if (videoField.WebId != Guid.Empty && videoField.ItemId != Guid.Empty)
                        {
                            using (SPWeb web = SPContext.Current.Site.OpenWeb(videoField.WebId))
                            {
                                if (web.Exists)
                                {
                                    folder = web.GetFolder(videoField.ItemId);
                                    if (folder.Exists)
                                    {
                                        BuildVideoControl(folder, web);
                                    }
                                    else
                                    {
                                        video.Visible = false;
                                    }
                                }
                                else
                                {
                                    video.Visible = false;
                                }
                            }
                        }
                        else
                        {
                            bool useRoot = false;
                            using (SPWeb web = SPContext.Current.Site.OpenWeb(videoField.Src))
                            {
                                if (web.Exists)
                                {
                                    folder = web.GetFolder(videoField.Src);

                                    if (folder.Exists)
                                    {
                                        BuildVideoControl(folder, web);
                                    }
                                    else
                                    {
                                        video.Visible = false;
                                    }
                                }
                                else
                                {
                                    useRoot = true;
                                }
                            }

                            if (useRoot)
                            {
                                SPWeb web = SPContext.Current.Site.RootWeb;
                                
                                    if (web.Exists)
                                    {
                                        folder = web.GetFolder(videoField.Src);

                                        if (folder.Exists)
                                        {
                                            BuildVideoControl(folder, web);
                                        }
                                        else
                                        {
                                            video.Visible = false;
                                        }
                                    }
                            }
                        }
                    }
                }
            }
            //ChildControlsCreated = true;
        }

        public override object Value
        {
            get
            {
                //this.EnsureChildControls();
                if (videoPicker != null)
                {
                    this.videoPicker.Entities.Count.ToString();

                    ArrayList resolvedEntities = this.videoPicker.ResolvedEntities;
                    if (resolvedEntities.Count == 0)
                        return this.ItemFieldValue;

                    if (resolvedEntities.Count == 1)
                    {
                        HTML5VideoPickerEntity videoEntity = (HTML5VideoPickerEntity)resolvedEntities[0];
                        return new HTML5VideoField(videoEntity);
                    }
                    else
                        throw new IndexOutOfRangeException();

                }
                else
                {
                    return this.ItemFieldValue;
                }
            }

            set
            {
                this.EnsureChildControls();
                this.SetFieldControlValue(value);
            }
        }

        private void SetFieldControlValue(object value)
        {
            ArrayList list = new ArrayList();
            HTML5VideoField videoField = null;

            if (videoPicker != null)
            {
                if (this.ControlMode == SPControlMode.New && videoPicker.Entities.Count == 0)
                {
                    videoField = new HTML5VideoField();
                    HTML5VideoPickerEntity defaultEntity = this.videoPicker.ValidateEntity(new HTML5VideoPickerEntity() { Key = videoField.Src });
                    if (defaultEntity != null)
                        list.Add(defaultEntity);
                    videoPicker.Entities.Clear();
                    videoPicker.UpdateEntities(list);
                }

                if (value != null)
                {
                    ArrayList entities = new ArrayList();
                    HTML5VideoField field = (HTML5VideoField)value;
                    HTML5VideoPickerEntity entity = new HTML5VideoPickerEntity(field);
                    entities.Add(entity);
                    videoPicker.UpdateEntities(entities);
                }
            }
        }

        public override void UpdateFieldValueInItem()
        {

            EnsureChildControls();

            if (videoPicker.Enabled)
            {
                videoPicker.Validate();
                if (videoPicker.Entities.Count > 0)
                {
                    PickerEntity entity = null;
                    if (videoPicker.ResolvedEntities.Count == 1)
                    {
                        entity = (PickerEntity)videoPicker.ResolvedEntities[0];
                    }
                    else
                    {
                        entity = (PickerEntity)videoPicker.Entities[0];
                    }

                    HTML5VideoPickerEntity videoEntity = new HTML5VideoPickerEntity(entity);

                    HTML5VideoField itemFieldValue = (HTML5VideoField)this.ItemFieldValue;

                    if (videoEntity.IsResolved)
                    {
                        if (itemFieldValue == null)
                        {
                            itemFieldValue = new HTML5VideoField();
                        }

                        itemFieldValue.Src = videoEntity.Src;
                        itemFieldValue.ItemId = videoEntity.ItemId;
                        itemFieldValue.WebId = videoEntity.WebId;
                        itemFieldValue.ListId = videoEntity.ListId;

                        this.ItemFieldValue = itemFieldValue;

                        UpdateVideoElement();
                    }
                }
            }
        }

        private void UpdateVideoElement()
        {
            if (settings == null)
            {
                settings = new VideoSettings();
                settings = settings.Load();
            }

            SPFolder folder = null;
            //SPWeb web = null;

            videoField = (HTML5VideoField)ItemFieldValue;

            if (videoField != null && videoField.WebId != Guid.Empty && videoField.ItemId != Guid.Empty)
            {
                using (SPWeb web = SPContext.Current.Site.OpenWeb(videoField.WebId))
                {
                    if (web.Exists)
                    {
                        folder = web.GetFolder(videoField.ItemId);

                        if (folder.Exists)
                        {
                            BuildVideoControl(folder, web);
                        }
                        else
                        {
                            video.Visible = false;
                        }
                    }
                    else
                    {
                        video.Visible = false;
                    }
                }
            }
        }

        private void BuildVideoControl(SPFolder folder, SPWeb web)
        {
            SPFile poster = null;
            List<SPFile> videos = new List<SPFile>();
            List<SPFile> tracks = new List<SPFile>();

            foreach (SPFile subfile in folder.Files)
            {
                string extension = Path.GetExtension(subfile.Name);

                if (extension == ".jpg" || extension == ".png")
                {
                    poster = subfile;
                }
                else if (extension == ".vtt")
                {
                    tracks.Add(subfile);
                }
                else
                {
                    videos.Add(subfile);
                }
            }

            if (poster != null && poster.Exists)
            {
                SPListItem iPoster = poster.Item;

                if (iPoster.Fields.Contains(BuildFieldId.Content_AutoPlay))
                {
                    try
                    {
                        string autoplay = iPoster[BuildFieldId.Content_AutoPlay].ToString();
                        if (!string.IsNullOrEmpty(autoplay))
                        {
                            try
                            {
                                video.AutoPlay = Boolean.Parse(autoplay);
                            }
                            catch
                            {
                                video.AutoPlay = settings.AutoPlay;
                            }
                        }
                        else
                        {
                            video.AutoPlay = settings.AutoPlay;
                        }
                    }
                    catch { };
                }
                else
                {
                    video.AutoPlay = settings.AutoPlay;
                }

                if (iPoster.Fields.Contains(BuildFieldId.Content_Loop))
                {
                    try
                    {
                        string loop = iPoster[BuildFieldId.Content_Loop].ToString();
                        if (!string.IsNullOrEmpty(loop))
                        {
                            try
                            {
                                video.Loop = Boolean.Parse(loop);
                            }
                            catch
                            {
                                video.Loop = settings.Loop;
                            }

                        }
                        else
                        {
                            video.Loop = settings.Loop;
                        }
                    }
                    catch { };
                }
                else
                {
                    video.Loop = settings.Loop;
                }

                if (iPoster.Fields.Contains(BuildFieldId.Content_Buffer))
                {
                    try
                    {
                        string buffer = iPoster[BuildFieldId.Content_Buffer].ToString();
                        if (!string.IsNullOrEmpty(buffer))
                        {
                            try
                            {
                                video.PreLoad = (PreLoadMode)Enum.Parse(typeof(PreLoadMode), buffer);
                            }
                            catch
                            {
                                video.PreLoad = settings.Buffer;
                            }
                        }
                        else
                        {
                            video.PreLoad = settings.Buffer;
                        }
                    }
                    catch { };
                }
                else
                {
                    video.PreLoad = settings.Buffer;
                }

                if (iPoster.Fields.Contains(BuildFieldId.Content_Controls))
                {
                    try
                    {
                        string controls = iPoster[BuildFieldId.Content_Controls].ToString();
                        if (!string.IsNullOrEmpty(controls))
                        {
                            try
                            {
                                video.Attributes.Add("controls", controls.ToLower());

                            }
                            catch
                            {
                                video.Attributes.Add("controls", settings.Controls.ToString().ToLower());
                            }

                        }
                        else
                        {
                            video.Attributes.Add("controls", settings.Controls.ToString().ToLower());
                        }
                    }
                    catch { };
                }
                else
                {
                    video.Attributes.Add("controls", settings.Controls.ToString().ToLower());
                }

                video.Width = Unit.Parse(iPoster[BuildFieldId.Content_Width].ToString());
                video.Height = Unit.Parse(iPoster[BuildFieldId.Content_Height].ToString());
                video.Attributes.Add("width", iPoster[BuildFieldId.Content_Width].ToString());
                video.Attributes.Add("height", iPoster[BuildFieldId.Content_Height].ToString());

                video.Poster = web.Url + "/" + HttpUtility.UrlPathEncode(poster.Url);
                video.BackColor = Color.Transparent;
                video.CssClass = "video-js vjs-default-skin";
                video.Attributes.Add("data-setup", "{}");

            }

            if (videos != null && videos.Count > 0)
            {
                foreach (SPFile avideo in videos)
                {
                    SPListItem iVideo = avideo.Item;

                    string xml = iVideo.Xml;

                    Source source = new Source();
                    source.Url = web.Url + "/" + HttpUtility.UrlPathEncode(avideo.Url);
                    string extension = Path.GetExtension(source.Url).Trim(new char[1] { '.' });
                    source.Type = "video/" + extension;

                    if (iVideo.Fields.Contains(BuildFieldId.Media_Query))
                    {
                        try
                        {
                            string media = iVideo[BuildFieldId.Media_Query].ToString();
                            if (!string.IsNullOrEmpty(media))
                            {
                                source.Media = media;
                            }
                        }
                        catch { };
                    }

                    video.Controls.Add(source);
                }
            }

            if (tracks != null && tracks.Count > 0)
            {
                foreach (SPFile atrack in tracks)
                {
                    SPListItem iTrack = atrack.Item;
                    Track track = new Track();
                    track.Kind = TrackKind.Captions;
                    track.Url = web.Url + "/" + HttpUtility.UrlPathEncode(atrack.Url);

                    if (iTrack.Fields.Contains(BuildFieldId.Track_Type))
                    {
                        try
                        {
                            string ttype = iTrack[BuildFieldId.Track_Type].ToString();
                            if (!string.IsNullOrEmpty(ttype))
                            {
                                try
                                {
                                    track.Kind = (TrackKind)Enum.Parse(typeof(TrackKind), ttype);
                                }
                                catch
                                {
                                    track.Kind = TrackKind.Captions;
                                }

                            }
                        }
                        catch { };
                    }

                    if (iTrack.Fields.Contains(BuildFieldId.Track_Language))
                    {
                        try
                        {
                            string language = iTrack[BuildFieldId.Track_Language].ToString();
                            if (!string.IsNullOrEmpty(language))
                            {
                                track.Language = language;
                            }
                        }
                        catch { };
                    }

                    if (iTrack.Fields.Contains(BuildFieldId.Track_Label))
                    {
                        try
                        {
                            string label = iTrack[BuildFieldId.Track_Label].ToString();
                            if (!string.IsNullOrEmpty(label))
                            {
                                track.Label = label;
                            }
                        }
                        catch { };
                    }

                    video.Controls.Add(track);

                }
            }
        }

        /*
        private void BuildImageControl(SPFolder folder, SPWeb web)
        {
            EnsureChildControls();
            if (image == null)
            {
                image = (Hemrika.SharePresence.Html5.WebControls.Image)this.TemplateContainer.FindControl("html_image");
            }
            
            SPFile poster = null;
            List<SPFile> videos = new List<SPFile>();
            List<SPFile> tracks = new List<SPFile>();

            foreach (SPFile subfile in folder.Files)
            {
                string extension = Path.GetExtension(subfile.Name);

                if (extension == ".jpg")
                {
                    poster = subfile;
                }
                else if (extension == ".vtt")
                {
                    tracks.Add(subfile);
                }
                else
                {
                    videos.Add(subfile);
                }
            }

            if (poster != null && poster.Exists)
            {
                SPListItem iPoster = poster.Item;

                image.Width = Unit.Parse(iPoster[BuildFieldId.Content_Width].ToString());
                image.Height = Unit.Parse(iPoster[BuildFieldId.Content_Height].ToString());
                //image.Attributes.Add("width", iPoster[BuildFieldId.Content_Width].ToString());
                //image.Attributes.Add("height", iPoster[BuildFieldId.Content_Height].ToString());

                image.Src = web.Url + "/" + poster.Url;
                image.BackColor = Color.Transparent;
            }
        }
        */
    }
}