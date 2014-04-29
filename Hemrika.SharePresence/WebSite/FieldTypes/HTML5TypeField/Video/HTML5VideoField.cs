using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.SharePoint;
using Hemrika.SharePresence.Html5.WebControls;

namespace Hemrika.SharePresence.WebSite.FieldTypes
{
    public class HTML5VideoField : SPFieldMultiColumnValue
    {
        private const int NumberOfFields = 6;
        private HTML5VideoPickerEntity videoEntity;

        public HTML5VideoField()
            : base(6)
        {
            /*
            //TODO Default video module and video settings
            Html5.WebControls.Video video = new Html5.WebControls.Video();

            this.AutoPlay = video.AutoPlay;
            this.DisplayControls = video.DisplayControls;
            this.IsMuted = video.IsMuted;
            this.Loop = video.Loop;
            this.MediaGroup = video.MediaGroup;
            this.Poster = video.Poster;
            this.PreLoad = video.PreLoad;
            this.Url = video.Url;
            */

            //TODO Default video module and video settings
            base[0] = "";
            base[1] = "";
            base[2] = Guid.Empty.ToString();
            base[3] = Guid.Empty.ToString();
            base[4] = "";
            base[5] = "";

        }

        /*
        public HTML5VideoField(Html5.WebControls.Video video)
            : base(8)
        {
            this.AutoPlay = video.AutoPlay;
            this.DisplayControls = video.DisplayControls;
            this.IsMuted = video.IsMuted;
            this.Loop = video.Loop;
            this.MediaGroup = video.MediaGroup;
            this.Poster = video.Poster;
            this.PreLoad = video.PreLoad;
            this.Url = video.Url;
        }
        */

        public HTML5VideoField(Guid WebId, Guid ItemId)
            : base(6)
        {
            base[0] = "";
            base[1] = "";
            base[2] = WebId.ToString();
            base[3] = ItemId.ToString();
            base[4] = "";
            base[5] = "";
        }

        public HTML5VideoField(string value) : base(value)
        {
        }

        public HTML5VideoField(HTML5VideoPickerEntity videoEntity)
        {
            // TODO: Complete member initialization
            ItemId = videoEntity.ItemId;
            ListId = videoEntity.ListId;
            WebId = videoEntity.WebId;
            Src = videoEntity.Src;
            this.videoEntity = videoEntity;
        }

        public bool IsEmpty
        {
            get
            {
                return string.IsNullOrEmpty(base[0]);
            }
        }

        internal string Src
        {
            get
            {
                return base[0];
            }
            set
            {
                base[0] = value;
            }
        }

        internal Guid WebId
        {
            get
            {
                try
                {
                    return new Guid(base[1]);
                }
                catch (Exception)
                {

                    return Guid.Empty;
                }

            }
            set
            {
                base[1] = value.ToString();
            }
        }

        internal Guid ListId
        {
            get
            {
                try
                {
                    return new Guid(base[2]);
                }
                catch (Exception)
                {

                    return Guid.Empty;
                }

            }
            set
            {
                base[2] = value.ToString();
            }
        }

        internal Guid ItemId
        {
            get
            {
                try
                {
                    return new Guid(base[3]);
                }
                catch (Exception)
                {

                    return Guid.Empty;
                }

            }
            set
            {
                base[3] = value.ToString();
            }
        }

        /*
        internal string Url
        {
            get
            {
                return base[0];
            }
            set
            {
                base[0] = value;
            }
        }

        internal bool AutoPlay
        {
            get
            {
                return Convert.ToBoolean(base[1]);
            }
            set
            {
                base[1] = Convert.ToString(value);
            }
        }

        internal bool DisplayControls
        {
            get
            {
                return Convert.ToBoolean(base[2]);
            }
            set
            {
                base[2] = Convert.ToString(value);
            }
        }

        internal bool IsMuted
        {
            get
            {
                return Convert.ToBoolean(base[3]);
            }
            set
            {
                base[3] = Convert.ToString(value);
            }
        }

        internal bool Loop
        {
            get
            {
                return Convert.ToBoolean(base[4]);
            }
            set
            {
                base[4] = Convert.ToString(value);
            }
        }

        internal string MediaGroup
        {
            get
            {
                return base[5];
            }
            set
            {
                base[5] = value;
            }
        }

        internal string Poster
        {
            get
            {
                return base[6];
            }
            set
            {
                base[6] = value;
            }
        }

        internal PreLoadMode PreLoad
        {
            get
            {
                PreLoadMode mode = PreLoadMode.NotSet;

                switch (base[7].ToString())
                {
                    case "Auto":
                        {
                            mode = PreLoadMode.Auto;
                            break;
                        }
                    case "MetaData":
                        {
                            mode = PreLoadMode.MetaData;
                            break;
                        }
                    case "None":
                        {
                            mode = PreLoadMode.None;
                            break;
                        }
                    case "NotSet":
                        {
                            mode = PreLoadMode.NotSet;
                            break;
                        }
                }
                return mode;
            }
            set
            {
                base[7] = value.ToString();
            }
        }
        */
    }
}
