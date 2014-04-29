// -----------------------------------------------------------------------
// <copyright file="EditorSettings.cs" company="">
// TODO: Update copyright text.
// </copyright>
// -----------------------------------------------------------------------

namespace Hemrika.SharePresence.WebSite.Editor
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using Microsoft.SharePoint;
    using System.Web.Script.Serialization;
    using System.Web;
    using Microsoft.SharePoint.Utilities;
    using System.IO;
    using System.Collections;

    [Serializable]
    public class EditorSettings
    {
        private System.Collections.SortedList _Common;
        private string _CommonDefault = "abbr;align;block;characterpicker;commands;contenthandler;dom-to-xhtml;format;highlighteditables;link;list;paste;summery;table;ui;undo";

        public System.Collections.SortedList CommonDefault
        {
            get
            {
                SortedList common = new System.Collections.SortedList();
                string[] commons = _CommonDefault.Split(new char[1] { ';' });
                foreach (string cstring in commons)
                {
                    if (!string.IsNullOrEmpty(cstring))
                    {
                        common.Add(cstring, true);
                    }
                }

                return common;
            }
        }

        public System.Collections.SortedList Common
        {
            get
            {
                if (_Common == null)
                {
                    _Common = new System.Collections.SortedList();
                }
                return _Common;
            }
            set { _Common = value; }
        }

        private System.Collections.SortedList _Extra;
        private string _ExtraDefault = "attributes;cite;draganddropfiles;flag-icons;formatlesspaste;headerids;htmlsource;linkbrowser;linkchecker;metaview;numerated-headers;ribbon;textcolor;toc;validation";

        public System.Collections.SortedList ExtraDefault
        {
            get
            {
                SortedList extra = new System.Collections.SortedList();
                string[] extras = _ExtraDefault.Split(new char[1] { ';' });
                foreach (string cstring in extras)
                {
                    if (!string.IsNullOrEmpty(cstring))
                    {
                        extra.Add(cstring, true);
                    }
                }

                return extra;
            }
        }

        public System.Collections.SortedList Extra
        {
            get
            {
                if (_Extra == null)
                {
                    _Extra = new System.Collections.SortedList();
                }
                return _Extra;
            }
            set { _Extra = value; }
        }

        public void Remove()
        {
            SPWeb rootWeb = SPContext.Current.Site.RootWeb;
            rootWeb.AllowUnsafeUpdates = true;
            if (rootWeb.AllProperties.ContainsKey("SharePresenceEditorSettings"))
            {
                rootWeb.AllProperties.Remove("SharePresenceEditorSettings");
            }
            rootWeb.Update();
            rootWeb.AllowUnsafeUpdates = false;
        }

        public EditorSettings Save()
        {
            string value = new JavaScriptSerializer().Serialize(this);
            SPWeb rootWeb = SPContext.Current.Site.RootWeb;
            rootWeb.AllowUnsafeUpdates = true;
            if (!rootWeb.AllProperties.ContainsKey("SharePresenceEditorSettings"))
            {
                rootWeb.AddProperty("SharePresenceEditorSettings", value);
            }
            else
            {
                rootWeb.AllProperties["SharePresenceEditorSettings"] = value;
            }

            rootWeb.Update();
            rootWeb.AllowUnsafeUpdates = false;
            return this;

        }

        public EditorSettings Load()
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

            if (rootWeb.AllProperties.ContainsKey("SharePresenceEditorSettings"))
            {
                string value = rootWeb.AllProperties["SharePresenceEditorSettings"] as string;
                if (!string.IsNullOrEmpty(value))
                {
                    try
                    {
                        EditorSettings settings = new JavaScriptSerializer().Deserialize<EditorSettings>(value);
                        if (settings != null)
                        {
                            if (dispose) { rootWeb.Dispose(); };

                            if (settings.Common.Count == 0)
                            {
                                SetDefaultCommon(ref settings);
                            }

                            if (settings.Extra.Count == 0)
                            {
                                SetDefaultExtra(ref settings);
                            }

                            return settings;
                        }
                        else
                        {
                            settings = new EditorSettings();
                            SetDefaultCommon(ref settings);
                            SetDefaultExtra(ref settings);
                            settings.Save();
                            return settings;
                        }
                    }
                    catch
                    {
                        EditorSettings settings = new EditorSettings();
                        SetDefaultCommon(ref settings);
                        SetDefaultExtra(ref settings);
                        //settings.Save();
                        return settings;
                    }
                }
                else
                {
                    EditorSettings settings = new EditorSettings();
                    SetDefaultCommon(ref settings);
                    SetDefaultExtra(ref settings);
                    settings.Save();
                    return settings;
                }
            }
            else
            {
                EditorSettings settings = new EditorSettings();
                SetDefaultCommon(ref settings);
                SetDefaultExtra(ref settings);
                settings.Save();
                return settings;
            }
            //return this;
        }

        private void SetDefaultExtra(ref EditorSettings settings)
        {
            //string commonPath = SPUtility.GetGenericSetupPath(@"Template\Layouts\Hemrika\Editor\plugins\common");
            string commonPath = SPUtility.GetVersionedGenericSetupPath(@"Template\Layouts\Hemrika\Editor\plugins\common",15);
            string[] commons = Directory.GetDirectories(commonPath);

            foreach (string common in commons)
            {
                DirectoryInfo info = new DirectoryInfo(common);
                if (settings.CommonDefault.ContainsKey(info.Name))
                {
                    settings.Common.Add(info.Name, true);
                }
            }
        }

        private void SetDefaultCommon(ref EditorSettings settings)
        {
            //string extraPath = SPUtility.GetGenericSetupPath(@"Template\Layouts\Hemrika\Editor\plugins\extra");
            string extraPath = SPUtility.GetVersionedGenericSetupPath(@"Template\Layouts\Hemrika\Editor\plugins\extra",15);
            string[] extras = Directory.GetDirectories(extraPath);

            foreach (string extra in extras)
            {
                DirectoryInfo info = new DirectoryInfo(extra);
                if (settings.ExtraDefault.ContainsKey(info.Name))
                {
                    settings.Extra.Add(info.Name, true);
                }
            }
        }
    }
}

