using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.SharePoint;
using Microsoft.SharePoint.Administration;

namespace Hemrika.SharePresence.Administration
{
    public class AppData : SPPersistedObject
    {
        [Persisted]
        public Dictionary<string, string> Entries;

        public AppData() { }
        public AppData(string Name, SPPersistedObject Parent, Guid Id) : base(Name, Parent, Id) { }

        public const string DATA_TAG = "19d6c8ff-5410-40fe-96a8-28a28adf6526";
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
