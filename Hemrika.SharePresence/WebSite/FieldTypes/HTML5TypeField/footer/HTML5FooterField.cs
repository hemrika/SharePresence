using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.SharePoint;

namespace Hemrika.SharePresence.WebSite.FieldTypes
{
    class HTML5FooterField: SPFieldMultiColumnValue
    {
        private const int NumberOfFields = 2;

        public HTML5FooterField() : base(2)
        {
            base[0] = "";
            base[1] = "";
        }

        public HTML5FooterField(string value)
            : base(value)
        {
        }

        internal string Heading
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

        internal string Text
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
    }
}
