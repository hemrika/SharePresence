﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.SharePoint;
using Microsoft.SharePoint.Administration;

namespace Hemrika.SharePresence.WebSite.SiteMap
{
    public class AppData : SPPersistedObject
    {
        [Persisted]
        public Dictionary<string, string> Entries;

        public AppData() { }
        public AppData(string Name, SPPersistedObject Parent, Guid Id) : base(Name, Parent, Id) { }

        public const string DATA_TAG = "c690efe7-46ef-4ab1-afa9-6b37f83fee3a";
        public const string SELECTED_INDEX = "SelectedIndex";

        private static AppData local;
        public static AppData Local
        {
            get
            {
                {
                    SPAdministrationWebApplication caWebApp = SPAdministrationWebApplication.Local;
                    local = caWebApp.GetChild<AppData>(DATA_TAG);

                    if (local == null)
                    {
                        Guid id = Guid.NewGuid();
                        SPPersistedObject parentWebApp = (SPPersistedObject)caWebApp;
                        local = new AppData(DATA_TAG, parentWebApp, id);
                        local.Update();
                    }

                    if (local.Entries == null)
                    {
                        local.Entries = new Dictionary<string, string>();
                        local.Entries.Add(SELECTED_INDEX, "0");
                        local.Update();
                    }
                }
                return local;
            }
        }
    }
}
