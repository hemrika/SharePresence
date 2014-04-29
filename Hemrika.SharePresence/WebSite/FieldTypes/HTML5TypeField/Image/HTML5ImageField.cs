using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.SharePoint;
using Hemrika.SharePresence.Html5.WebControls;

namespace Hemrika.SharePresence.WebSite.FieldTypes
{
    public class HTML5ImageField : SPFieldMultiColumnValue
    {
        private const int NumberOfFields = 6;

        public HTML5ImageField() : base(6)
        {
            //TODO Default image module and image settings
            base[0] = "";
            base[1] = "";
            base[2] = Guid.Empty.ToString();
            base[3] = Guid.Empty.ToString();
            base[4] = "";
            base[5] = "";
        }

        public HTML5ImageField(string src, string alt) : base(6)
        {
            base[0] = src;
            base[1] = alt;
            base[2] = Guid.Empty.ToString();
            base[3] = Guid.Empty.ToString();
            base[4] = "";
            base[5] = "";
        }

        public HTML5ImageField(Image image,Guid WebId,Guid ItemId) : base(6)
        {
            base[0] = image.Src;
            base[1] = image.Alt;
            base[2] = WebId.ToString();
            base[3] = ItemId.ToString();
            base[4] = "";
            base[5] = "";
        }

        public HTML5ImageField(string value) : base(value)
        {
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

        internal string Alt
        {
            get
            {
                return base[1];
            }
            set
            {
                base[1] = value;
            }
        }

        internal Guid WebId
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

        internal string Style
        {
            get
            {
                try
                {
                    return base[4];
                }
                catch (Exception)
                {
                    return string.Empty;
                }

            }
            set
            {
                try
                {
                    base[4] = value;
                }
                catch (Exception)
                {
                }
            }
        }


        public string Title
        {
            get
            {
                try
                {
                    return base[5];
                }
                catch (Exception)
                {
                    return string.Empty;
                }

            }
            set
            {
                try
                {
                    base[5] = value;
                }
                catch (Exception)
                {
                }
            }
        }
    }
}
