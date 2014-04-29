// -----------------------------------------------------------------------
// <copyright file="MetaDataSettings.cs" company="">
// TODO: Update copyright text.
// </copyright>
// -----------------------------------------------------------------------

namespace Hemrika.SharePresence.WebSite.MetaData
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Web.Script.Serialization;
    using Microsoft.SharePoint;
    using System.Web;
    using System.Collections.Specialized;
    using System.Text.RegularExpressions;

    public class KeywordSettings
    {
        /*
        private StringCollection _Scraping;

        public StringCollection Scraping
        {
            get
            {
                return _Scraping;
            }
            set { _Scraping = value; }
        }
        */

        public void Remove(SPSite site)
        {
            SPWeb rootWeb = site.RootWeb;
            rootWeb.AllowUnsafeUpdates = true;
            if (rootWeb.AllProperties.ContainsKey("SharePresenceKeywordSettings"))
            {
                rootWeb.AllProperties.Remove("SharePresenceKeywordSettings");
            }
            rootWeb.Update();
            rootWeb.AllowUnsafeUpdates = false;
        }


        public KeywordSettings Save(SPSite site)
        {
            string value = new JavaScriptSerializer().Serialize(this);
            SPWeb rootWeb = site.RootWeb;
            rootWeb.AllowUnsafeUpdates = true;
            rootWeb.AllProperties["SharePresenceKeywordSettings"] = value;
            rootWeb.Update();
            rootWeb.AllowUnsafeUpdates = false;
            return this;
        }

        public KeywordSettings Save()
        {
            string value = new JavaScriptSerializer().Serialize(this);
            SPWeb rootWeb = SPContext.Current.Site.RootWeb;
            rootWeb.AllowUnsafeUpdates = true;
            if (rootWeb.AllProperties.ContainsKey("SharePresenceKeywordSettings"))
            {
                rootWeb.AllProperties["SharePresenceKeywordSettings"] = value;
            }
            else
            {
                rootWeb.AddProperty("SharePresenceKeywordSettings", value);
            }

            rootWeb.Update();
            rootWeb.AllowUnsafeUpdates = false;
            return this;
        }

        public KeywordSettings Load(SPSite site)
        {
            SPWeb rootWeb = site.RootWeb;

            if (rootWeb.AllProperties.ContainsKey("SharePresenceKeywordSettings"))
            {
                string value = rootWeb.AllProperties["SharePresenceKeywordSettings"] as string;
                if (!string.IsNullOrEmpty(value))
                {
                    try
                    {
                        KeywordSettings settings = new JavaScriptSerializer().Deserialize<KeywordSettings>(value);
                        if (settings != null)
                        {
                            /*
                            this.Scraping = settings.Scraping;
                            */
                        }
                    }
                    catch
                    {
                    }
                }
            }
            return this;
        }

        public KeywordSettings Load()
        {

            SPWeb rootWeb;
            bool dispose = false;
            if (SPContext.Current != null)
            {
                rootWeb = SPContext.Current.Site.RootWeb;
            }
            else
            {
                HttpContext context = HttpContext.Current;
                SPSite site = new SPSite(context.Request.Url.ToString());
                rootWeb = site.RootWeb;
                dispose = true;
            }

            if (rootWeb.AllProperties.ContainsKey("SharePresenceKeywordSettings"))
            {
                string value = rootWeb.AllProperties["SharePresenceKeywordSettings"] as string;
                if (!string.IsNullOrEmpty(value))
                {
                    try
                    {
                        KeywordSettings settings = new JavaScriptSerializer().Deserialize<KeywordSettings>(value);
                        if (settings != null)
                        {
                            if (dispose) { rootWeb.Dispose(); };

                            return settings;
                        }
                    }
                    catch
                    {
                        return new KeywordSettings();
                    }
                }
            }
            else
            {
                //this.Scraping = new StringCollection();
                //LoadDefaults();
                if (HttpContext.Current.User.Identity.IsAuthenticated)
                {
                    Save();// (site);
                }
            }

            return this;
        }

        /*
        private void LoadDefaults()
        {
            this.Scraping.Add(@"a|aboard|about|above|absent|according\sto|across|after|against|ago|ahead\sof|ain't|all|along|alongside");
            this.Scraping.Add(@"also|although|am|amid|amidst|among|amongst|an|and|anti|anybody|anyone|anything|apart|apart\sfrom|are|been");
            this.Scraping.Add(@"aren't|around|as|as\sfar\sas|as\ssoon\sas|as\swell\sas|aside|at|atop|away|be|because|because\sof|before");
            this.Scraping.Add(@"behind|below|beneath|beside|besides|between|betwixt|beyond|but|by|by\smeans\sof|by\sthe\stime|can|cannot");
            this.Scraping.Add(@"circa|close\sto|com|concerning|considering|could|couldn't|cum|'d|despite|did|didn't|do|does|doesn't|don't");
            this.Scraping.Add(@"down|due\sto|during|each_other|'em|even\sif|even\sthough|ever|every|every\stime|everybody|everyone");
            this.Scraping.Add(@"everything|except|far\sfrom|few|first\stime|following|for|from|get|got|had|hadn't|has|hasn't|have");
            this.Scraping.Add(@"haven't|he|hence|her|here|hers|herself|him|himself|his|how|i|if|in|in\saccordance\swith|in\saddition\sto|in\scase");
            this.Scraping.Add(@"in\sfront\sof|in\slieu\sof|in\splace\sof|in\sspite\sof|in\sthe\sevent\sthat|in\sto|inside|inside\sof");
            this.Scraping.Add(@"instead\sof|into|is|isn't|it|itself|just\sin\scase|like|'ll|lots|may|me|mid|might|mightn't|mine|more|most");
            this.Scraping.Add(@"must|mustn't|myself|near|near\sto|nearest|new|no|no\sone|nobody|none|not|nothing|notwithstanding|now\sthat|of");
            this.Scraping.Add(@"off|on|on\sbehalf\sof|on\sto|on\stop\sof|once|one|one\sanother|only\sif|onto|opposite|or|org|other|our|any");
            this.Scraping.Add(@"ours|ourselves|out|out\sof|outside|outside\sof|over|past|per|plenty|plus|prior\sto|qua|re|'re|really|set");
            this.Scraping.Add(@"regarding|round|'s|said|sans|save|say|says|shall|shan't|she|should|shouldn't|since|so|somebody|its|only");
            this.Scraping.Add(@"someone|something|than|that|the|thee|their|theirs|them|themselves|there|these|they|thine|this|thou");
            this.Scraping.Add(@"though|through|throughout|till|to|toward|towards|under|underneath|unless|unlike|until|unto|up|upon|using|even");
            this.Scraping.Add(@"us|'ve|versus|via|was|wasn't|we|were|weren't|what|when|whenever|where|whereas|whether\sor\snot|things");
            this.Scraping.Add(@"which|while|who|whoever|whom|why|will|with|with\sregard\sto|withal|within|without|won't|would|wouldn't|mere");
            this.Scraping.Add(@"ya|ye|yes|you|your|yours|yourself");
        }
        */
    }
}
