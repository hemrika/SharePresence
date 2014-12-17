using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Permissions;
using System.Text;
using Microsoft.SharePoint;
using System.Web.Script.Serialization;
using System.Web;
using Microsoft.SharePoint.Utilities;
using Hemrika.SharePresence.Html5.WebControls;

namespace Hemrika.SharePresence.WebSite.Video
{
    [Serializable]
    public class VideoSettings
    {
        [NonSerialized]
        private SPContext _context;

        public SPContext Context
        {
            get { return _context; }
            set { _context = value; }
        }


        private string _FFMpegPath;

        public string FFMpegPath
        {
            get
            {
                //_FFMpegPath = SPUtility.GetGenericSetupPath(@"bin");
                _FFMpegPath = SPUtility.GetVersionedGenericSetupPath(@"bin",15);
                return _FFMpegPath;
            }
        }

        private string _InputVideoPath;

        public string InputVideoPath
        {
            get
            {
                string appId = Context.Site.WebApplication.Id.ToString();

                    HttpRequest request =  new HttpRequest(string.Empty, Context.Site.Url, string.Empty);
                    HttpResponse response = new HttpResponse(new System.IO.StreamWriter(new System.IO.MemoryStream()));
                    HttpContext dummyContext = new HttpContext(request, response);

                    _InputVideoPath = dummyContext.Server.MapPath("~/" + appId);

                //_InputVideoPath = SPUtility.GetGenericSetupPath(@"Data\Conversion\" + appId);
                return _InputVideoPath;
            }
        }

        //private string _OutputImagePath;

        public string OutputImagePath
        {
            get
            {
                string appId = Context.Site.WebApplication.Id.ToString();
                HttpRequest request = new HttpRequest(string.Empty, Context.Site.Url, string.Empty);
                HttpResponse response = new HttpResponse(new System.IO.StreamWriter(new System.IO.MemoryStream()));
                HttpContext dummyContext = new HttpContext(request, response);

                _InputVideoPath = dummyContext.Server.MapPath("~/" + appId);
                //_OutputImagePath = SPUtility.GetGenericSetupPath(@"Data\Conversion\" + appId);
                return _InputVideoPath;
            }

        }

        private int _ProcessTimeout = 20;

        public int ProcessTimeout
        {
            get { 
                return _ProcessTimeout; }
            set { _ProcessTimeout = value; }
        }

        private int _StartTimeOffset = 1;

        public int StartTimeOffset
        {
            get { return _StartTimeOffset; }
            set { _StartTimeOffset = value; }
        }

        private PreLoadMode _Buffer = PreLoadMode.Auto;

        public PreLoadMode Buffer
        {
            get { return _Buffer; }
            set { _Buffer = value; }
        }

        private bool _AutoPlay = false;

        public bool AutoPlay
        {
            get { return _AutoPlay; }
            set { _AutoPlay = value; }
        }

        private bool _Loop = false;

        public bool Loop
        {
            get { return _Loop; }
            set { _Loop = value; }
        }

        private bool _Controls = true;

        public bool Controls
        {
            get { return _Controls; }
            set { _Controls = value; }
        }

        public void Remove()
        {
            SPWeb rootWeb = SPContext.Current.Site.RootWeb;
            rootWeb.AllowUnsafeUpdates = true;
            if (rootWeb.AllProperties.ContainsKey("SharePresenceVideoSettings"))
            {
                rootWeb.AllProperties.Remove("SharePresenceVideoSettings");
            }
            rootWeb.Update();
            rootWeb.AllowUnsafeUpdates = false;
        }

        public VideoSettings Save()
        {
            string value = new JavaScriptSerializer().Serialize(this);
            SPWeb rootWeb = SPContext.Current.Site.RootWeb;
            rootWeb.AllowUnsafeUpdates = true;
            if (!rootWeb.AllProperties.ContainsKey("SharePresenceVideoSettings"))
            {
                rootWeb.AddProperty("SharePresenceVideoSettings", value);
            }
            else
            {
                rootWeb.AllProperties["SharePresenceVideoSettings"] = value;
            }

            rootWeb.Update();
            rootWeb.AllowUnsafeUpdates = false;
            return this;

        }

        public VideoSettings Load()
        {
            if (SPContext.Current != null)
            {
                _context = SPContext.Current;
                Load(SPContext.Current.Site);
            }
            else
            {
                HttpContext context = HttpContext.Current;
                if (context != null)
                {
                    if (_context != null)
                    {
                        _context = SPContext.GetContext(context);
                    }

                    SPSite site = Context.Site;// (context.Request.Url.ToString());
                    Load(site);
                }
            }

            return this;
        }

        public VideoSettings Load(SPSite site)
        {
            try
            {
                SPWeb rootWeb = site.RootWeb;
                bool dispose = false;

                if (rootWeb.AllProperties.ContainsKey("SharePresenceVideoSettings"))
                {
                    string value = rootWeb.AllProperties["SharePresenceVideoSettings"] as string;
                    if (!string.IsNullOrEmpty(value))
                    {
                        try
                        {
                            VideoSettings settings = new JavaScriptSerializer().Deserialize<VideoSettings>(value);
                            if (settings != null)
                            {
                                if (dispose) { rootWeb.Dispose(); };

                                return settings;
                            }
                        }
                        catch
                        {
                            return new VideoSettings();
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                ex.ToString();
            }

            return this;
        }
    }
}
