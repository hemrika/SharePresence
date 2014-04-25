namespace Hemrika.SharePresence.Google.WebmasterTools
{
    using Extensions;
    using System;

    public class Language : SimpleNameValueAttribute
    {
        public Language() : base("verification-method", "wt", "http://schemas.google.com/webmasters/tools/2007")
        {
            base.Attributes.Add("language", null);
        }

        public Language(string value) : base("language", "wt", "http://schemas.google.com/webmasters/tools/2007")
        {
            base.Attributes.Add("language", value);
        }

        public string LanguageAttribute
        {
            get
            {
                return (base.Attributes["language"] as string);
            }
            set
            {
                base.Attributes["language"] = value;
            }
        }
    }
}

