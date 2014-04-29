using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.SharePoint;

namespace Hemrika.SharePresence.WebSite.FieldTypes
{
    class HTML5BookmarkField: SPFieldMultiColumnValue
    {
        private const int NumberOfFields = 1;

        public HTML5BookmarkField() : base(1)
        {
            base[0] = Bookmarks.ToString();
        }

        public HTML5BookmarkField(string value)
            : base(value)
        {
        }

        internal int Bookmarks
        {
            get
            {
                int _bookmarks = 0;
                bool parsed = int.TryParse(base[0], out _bookmarks);
                if (parsed)
                {
                    return _bookmarks;
                }
                return 0;
            }
            set
            {
                base[0] = value.ToString();
            }
        }
    }
}
