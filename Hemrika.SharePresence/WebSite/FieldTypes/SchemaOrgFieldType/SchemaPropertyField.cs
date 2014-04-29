using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.SharePoint;
using Hemrika.SharePresence.WebSite.FieldTypes;

namespace Hemrika.SharePresence.WebSite.FieldTypes
{
    class SchemaPropertyField : SPFieldMultiColumnValue
    {
        private const int NumberOfFields = 2;

        public SchemaPropertyField() : base(2)
        {
            base[0] = "";
            base[1] = "";
        }

        public SchemaPropertyField(string value) : base(value)
        {
        }

        internal string Property
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

        internal string Type
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
