namespace Hemrika.SharePresence.Google.WebmasterTools
{
    using Extensions;
    using System;

    public class VerificationMethod : SimpleNameValueAttribute
    {
        public VerificationMethod() : base("verification-method", "wt", "http://schemas.google.com/webmasters/tools/2007")
        {
            base.Attributes.Add("type", null);
            base.Attributes.Add("in-use", null);
        }

        public VerificationMethod(string type, string inUse) : base("verification-method", "wt", "http://schemas.google.com/webmasters/tools/2007")
        {
            base.Attributes.Add("type", type);
            base.Attributes.Add("in-use", inUse);
        }

        public string InUse
        {
            get
            {
                return (base.Attributes["in-use"] as string);
            }
            set
            {
                base.Attributes["in-use"] = value;
            }
        }

        public string Type
        {
            get
            {
                return (base.Attributes["type"] as string);
            }
            set
            {
                base.Attributes["type"] = value;
            }
        }
    }
}

