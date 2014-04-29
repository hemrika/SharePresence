using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.SharePoint;

namespace Hemrika.SharePresence.WebSite.FieldTypes
{
    class HTML5RatingField: SPFieldMultiColumnValue
    {
        private const int NumberOfFields = 5;

        public HTML5RatingField() : base(5)
        {
            base[0] = Minimum.ToString();
            base[1] = Maximum.ToString();
            base[2] = Rating.ToString();
            base[3] = Votes.ToString();
            base[4] = Step.ToString();
        }

        public HTML5RatingField(string value)
            : base(value)
        {
        }

        internal int Minimum
        {
            get
            {
                int _minimum = 0;
                bool parsed = int.TryParse(base[0], out _minimum);
                if (parsed)
                {
                    return _minimum;
                }
                return 0;
            }
            set
            {
                base[0] = value.ToString();
            }
        }

        internal int Maximum
        {
            get
            {
                int _maximum = 5;
                bool parsed = int.TryParse(base[1], out _maximum);
                if (parsed)
                {
                    return _maximum;
                }
                return 5;
            }
            set
            {
                base[1] = value.ToString();
            }
        }

        internal float Rating
        {
            get
            {
                float _rating = 0.0f;
                bool parsed = float.TryParse(base[2], out _rating);
                if (parsed)
                {
                    return _rating;
                }
                return (float)0.0f;
            }
            set
            {
                base[2] = value.ToString();
            }
        }

        internal int Votes
        {
            get
            {
                int _votes = 0;
                bool parsed = int.TryParse(base[3],out _votes);
                if(parsed)
                {
                    return _votes;
                }
                return 0;
            }
            set
            {
                base[3] = value.ToString();
            }
        }

        internal float Step
        {
            get
            {
                float _step = 0.5f;
                bool parsed = float.TryParse(base[4], out _step);
                if (parsed)
                {
                    return _step;
                }
                return (float)0.5f;
            }
            set
            {
                base[4] = value.ToString();
            }
        }

    }
}
