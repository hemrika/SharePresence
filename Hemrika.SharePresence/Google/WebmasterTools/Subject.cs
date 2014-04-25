namespace Hemrika.SharePresence.Google.WebmasterTools
{
    using Extensions;
    using System;

    public class Subject : SimpleNameValueAttribute
    {
        public Subject() : base("subject", "wt", "http://schemas.google.com/webmasters/tools/2007")
        {
            base.Attributes.Add("subject", null);
        }

        public Subject(string value) : base("subject", "wt", "http://schemas.google.com/webmasters/tools/2007")
        {
            base.Attributes.Add("subject", value);
        }

        public string SubjectAttribute
        {
            get
            {
                return (base.Attributes["subject"] as string);
            }
            set
            {
                base.Attributes["subject"] = value;
            }
        }
    }
}

