namespace Hemrika.SharePresence.Google.WebmasterTools
{
    using Client;
    using System;

    public class MessagesEntry : WebmasterToolsBaseEntry
    {
        public MessagesEntry()
        {
            //Tracing.TraceMsg("Created MessagesEntry");
            base.AddExtension(new WebmasterTools.Body());
            base.AddExtension(new WebmasterTools.Date());
            base.AddExtension(new Language());
            base.AddExtension(new WebmasterTools.Read());
            base.AddExtension(new WebmasterTools.Subject());
        }

        public string Body
        {
            get
            {
                return base.getWebmasterToolsValue("body");
            }
            set
            {
                base.setWebmasterToolsExtension("body", value);
            }
        }

        public string Date
        {
            get
            {
                return base.getWebmasterToolsValue("date");
            }
            set
            {
                base.setWebmasterToolsExtension("date", value);
            }
        }

        public Language LanguageElement
        {
            get
            {
                return (base.getWebmasterToolsExtension("language") as Language);
            }
            set
            {
                base.ReplaceExtension("language", "http://schemas.google.com/webmasters/tools/2007", value);
            }
        }

        public string Read
        {
            get
            {
                return base.getWebmasterToolsValue("read");
            }
            set
            {
                base.setWebmasterToolsExtension("read", value);
            }
        }

        public WebmasterTools.Subject Subject
        {
            get
            {
                return (base.getWebmasterToolsExtension("subject") as WebmasterTools.Subject);
            }
            set
            {
                base.ReplaceExtension("subject", "http://schemas.google.com/webmasters/tools/2007", value);
            }
        }
    }
}

