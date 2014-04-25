namespace Hemrika.SharePresence.Google.WebmasterTools
{
    using Client;
    using System;

    public class KeywordsEntry : WebmasterToolsBaseEntry
    {
        public KeywordsEntry()
        {
            //Tracing.TraceMsg("Created KeywordsEntry");
            base.AddExtension(new WebmasterTools.Keyword());
        }

        public WebmasterTools.Keyword Keyword
        {
            get
            {
                return (base.getWebmasterToolsExtension("keyword") as WebmasterTools.Keyword);
            }
            set
            {
                base.ReplaceExtension("keyword", "http://schemas.google.com/webmasters/tools/2007", value);
            }
        }
    }
}

