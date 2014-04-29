// <copyright file="ContentRibbon.cs" company="Hemrika">
// Copyright Hemrika. All rights reserved.
// </copyright>
// <author>MARKETING\Administrator</author>
// <date>2012-03-22 15:41:28Z</date>
namespace Hemrika.SharePresence.WebSite
{
    using System;
    using System.Security.Permissions;
    using System.Text;
    using System.Web.UI.WebControls;
    using Microsoft.SharePoint;
    using Microsoft.SharePoint.Security;
    using Hemrika.SharePresence.Common.UI;
    using Hemrika.SharePresence.Common.Ribbon.Definitions;
    using Hemrika.SharePresence.Common.Ribbon.Libraries;
    using Hemrika.SharePresence.Common.Ribbon.Definitions.Controls;
    using Microsoft.SharePoint.WebPartPages;
    using System.Collections.Generic;
    using System.Xml;
    using System.IO;
    using System.Linq;
    using Microsoft.SharePoint.Utilities;
    using Hemrika.SharePresence.Common.Ribbon;
    using Hemrika.SharePresence.WebSite.PageParts;
    using Microsoft.SharePoint.WebControls;
    using System.Web.UI;
    using System.Web.UI.WebControls.WebParts;
    using System.Web;
    using System.Web.UI.HtmlControls;

    /// <summary>
    /// TODO: Add comment for ContentRibbon
    /// </summary> 
    public class ContentRibbon : SPRibbonCommandHandler, IPostBackEventHandler, ICallbackEventHandler
    {
        public ContentRibbon(SPPageStateControl psc)
            : base(psc)
        {
        }

        private ICollection<WebPartGalleryItem> sources;
        private List<String> categories;
        private ICollection<GroupDefinition> groups;

        protected override void OnLoad(EventArgs e)
        {
            base.OnLoad(e);

            if (SPContext.Current != null)
            {
                GetSources();
            }

            var tabDefinition = GetTabDefinition();
            try
            {
                if (SPRibbon.GetCurrent(this.Page) == null)
                    return;
                if (tabDefinition != null && !this.DesignMode)
                    RibbonController.Current.AddRibbonTabToPage(tabDefinition, this.Page, false);
            }
            catch (Exception ex)
            {
                ex.ToString();
            }
        }

        private ICollection<GroupDefinition> Groups
        {
            get
            {
                if (this.groups == null)
                {
                    this.groups = new List<GroupDefinition>();
                }
                return this.groups;

            }
            set
            {
                this.groups = value;
            }
        }
        private ICollection<WebPartGalleryItem> Sources
        {
            get
            {
                if (this.sources == null)
                {
                    this.sources = new List<WebPartGalleryItem>();
                }
                return this.sources;
            }
            set
            {
                this.sources = value;
            }
        }

        private ICollection<String> Categories
        {
            get
            {
                if (this.categories == null)
                {
                    this.categories = new List<String>();
                }
                return this.categories;
            }
        }

        private SPRibbon Ribbon
        {
            get
            {
                if (this.Page != null)
                {
                    return SPRibbon.GetCurrent(this.Page);
                }
                return null;
            }
        }


        private WebPartAdder WebPartAdder
        {
            get
            {
                if (this.Page != null)
                {
                    return (this.Page.Items[typeof(WebPartAdder)] as WebPartAdder);
                }
                return null;
            }
        }

        private SPWebPartManager WebPartManager
        {
            get
            {
                if (this.Page != null)
                {
                    return (SPWebPartManager)System.Web.UI.WebControls.WebParts.WebPartManager.GetCurrentWebPartManager(this.Page);
                }
                return null;
            }
        }

        private void GetSources()
        {
            if ((SPContext.Current != null) && (this.WebPartManager != null))
            {
                PageParts.RibbonWebPartGallerySource wp = new RibbonWebPartGallerySource(this.Page);
                foreach (WebPartGalleryItem item in wp.GetItems())
                {
                    if (!Categories.Contains(item.Category))
                    {
                        Categories.Add(item.Category);
                    }

                    if (!Sources.Contains(item))
                    {
                        Sources.Add(item);
                    }
                }

                PageParts.PagePartGallerySource pp = new PageParts.PagePartGallerySource(this.Page);
                foreach (WebPartGalleryItem item in pp.GetItems())
                {
                    if (!Categories.Contains(item.Category))
                    {
                        Categories.Add(item.Category);
                    }

                    if (!Sources.Contains(item))
                    {
                        Sources.Add(item);
                    }
                }
            }
        }

        public TabDefinition GetTabDefinition()
        {
            var groups = new List<GroupDefinition>();

            var maintenance = new List<ControlDefinition>();

            ToolTipDefinition tooltip = new ToolTipDefinition();
            tooltip.Description = String.Format("View all WebParts on page {0}.", SPContext.Current.ListItem.Title);
            tooltip.Title = "WebParts on Page.";

            var button = new ButtonDefinition()
            {
                Id = "WebpartMaintenance",
                Title = "WebParts on Page",
                ToolTip = tooltip,
                Image = ImageLibrary.GetStandardImage(3, 4),
                CommandJavaScript = "ShowMaintenancePage(commandId, properties, sequence)"
            };
            maintenance.Add(button);

            button = new ButtonDefinition()
            {
                Id = "WebPartInsert",
                Title = "WebPart Adder",
                Image = ImageLibrary.GetStandardImage(13, 9),
                CommandEnableJavaScript = "document.forms[MSOWebPartPageFormName].MSOLayout_InDesignMode.value == 1",
                CommandJavaScript = "CoreInvoke('ShowWebPartAdder', 'insertWebPart');return false"
            };
            //CommandJavaScript = "window.LoadWPAdderOnDemand(); ShowWebPartAdder('insertWebPart')"
            
            maintenance.Add(button);

            var maintenancegroup = new GroupDefinition()
            {
                Id = "Maintenance",
                Title = "Maintenance",
                Sequence = "10",
                Template = GroupTemplateLibrary.SimpleTemplate,
                Controls = maintenance.ToArray()
            };

            groups.Add(maintenancegroup);

            if (WebPartManager.Zones.Count > 1)
            {
                var zones = new List<ControlDefinition>();

                var label = new LabelDefinition()
                {
                    Id = "ZoneLabel",
                    Title = "Select your Zone",
                    Image = ImageLibrary.GetStandardImage(2, 4),
                    TemplateAlias = "c1",
                    ForId = "ZoneDropDown"
                };

                zones.Add(label);

                var choices = new List<ControlDefinition>();
                string choice = string.Empty;
                foreach (WebPartZoneBase zone in WebPartManager.Zones)
                {
                    if (zone != null && zone.ID != "wpz")
                    {
                        tooltip = new ToolTipDefinition();
                        tooltip.Description = String.Format("Add WebPart to {0} zone.", zone.DisplayTitle);
                        tooltip.Title = zone.DisplayTitle;
                        if (string.IsNullOrEmpty(choice))
                        {
                            choice = zone.ID;
                        }

                        button = new ButtonDefinition()
                        {
                            Id = zone.ID,
                            Title = zone.DisplayTitle,
                            ToolTip = tooltip,
                            Image = ImageLibrary.GetStandardImage(13, 2),
                            TemplateAlias = "c1",
                            CommandJavaScript = String.Format("SetWebpartZone('{0}')", zone.ID)
                        };
                        choices.Add(button);
                    }
                }

                var dropdown = new DropDownDefinition()
                {
                    Id = "ZoneDropDown",
                    ControlsSize = ControlSize.Size32x32,
                    Title = "Zones",
                    InitialValueJavaScript = "'"+choice+"'",
                    Controls = choices.ToArray()
                };

                zones.Add(dropdown);


                var Selectedlabel = new LabelDefinition()
                {
                    Id = "SelectedZoneLabel",
                    Title = "Selected Zone",
                    TemplateAlias = "c1"
                };

                zones.Add(Selectedlabel);

                var zonegroup = new GroupDefinition()
                {
                    Id = "WebpartZone",
                    Title = "WebpartZone",
                    Sequence = "20",
                    Template = GroupTemplateLibrary.ThreeRowTemplate,
                    Controls = zones.ToArray()
                };

                groups.Add(zonegroup);
            }

            int index = 0;
            int sequence = 30;


            foreach (string category in Categories)
            {
                List<WebPartGalleryItem> buttons = Sources.Where(n => n.Category == category).Select(n => n).ToList();

                if (buttons.Count > 0)
                {
                    var controls = new List<ControlDefinition>();

                    foreach (WebPartGalleryItem item in buttons)
                    {
                        ImageDefinition image = new ImageDefinition();
                        image.Url16 = item.IconUrl;
                        image.Url32 = item.IconUrl;

                        button = new ButtonDefinition()
                        {
                            Id = item.Title.Replace(" ", String.Empty).Trim(),
                            Title = item.Title,
                            Image = image,
                            CommandEnableJavaScript = "webpartzone != null",
                            CommandJavaScript = String.Format("AddWebPartToPage('{0}','{1}')",this.ClientID, item.Id),
                            TemplateAlias = "c1"
                        };

                        controls.Add(button);
                    }

                    var group = new GroupDefinition()
                    {
                        Id = ((category != null) ? category.Replace(" ", string.Empty).Trim() : "None"),
                        Title = (category != null) ? category : "None",
                        Sequence = sequence.ToString(),
                        Template = GroupTemplateLibrary.SimpleTemplate,
                        Controls = new ControlDefinition[]
                        {
                            new MRUSplitButtonDefinition()
                            {
                                Id = "MRU"+ ((category != null) ? category.Replace(" ", string.Empty).Trim() : "None"),
                                InitialItem = controls[0].Id,                                
                                ControlsSize = ControlSize.Size32x32,
                                Controls = controls.ToArray()
                            }
                        }
                    };

                    groups.Add(group);

                    sequence += 10;
                    index += 1;
                }
            }
            if (groups.Count > 0)
            {
                return new TabDefinition()
                {
                    Id = "Hemrika.SharePresence.Content",
                    Title = "Content",
                    Sequence = "110",
                    GroupTemplates = GroupTemplateLibrary.AllTemplates,
                    Groups = groups.ToArray()
                };
            }
            else
            {
                return null;
            }
        }

        public string GetCallbackResult()
        {
            try
            {
                if (result != null)
                {
                    throw (result);
                }
            }
            catch (SPException exception)
            {
                base.SetGenericErrorMessage(exception);
            }
            base.RefreshPageState();
            base.parentStateControl.OnPageStateChanged();

            SPRibbon ribbon = SPRibbon.GetCurrent(this.Page);

            if (ribbon != null)
            {
                string currentTab = ribbon.ActiveTabId;
                ribbon.SetInitialTabId(currentTab, "WSSPageStateVisibilityContext");
            }

            //UriBuilder builder = new UriBuilder(HttpContext.Current.Request.Url.OriginalString);
            //SPUtility.Redirect(builder.Uri.PathAndQuery, SPRedirectFlags.Trusted, HttpContext.Current);

            return base.BuildReturnValue("Webpart has been added.Refresh your page.");        

        }

        private Exception result = null;

        public void RaiseCallbackEvent(string eventArgument)
        {
            try
            {
                string[] eventArg = eventArgument.Split(new string[1]{"#;"},StringSplitOptions.RemoveEmptyEntries);
                List<WebPartGalleryItem> webparts = Sources.Where(n => n.Id == eventArg[0]).Select(n => n).ToList();
                
                SPListItem item = SPContext.Current.ListItem;
                SPFile currentFile = item.File;

                if (item != null && item != null)
                {
                    if (currentFile != null && currentFile.Exists)
                    {
                        if (currentFile.CheckOutType == SPFile.SPCheckOutType.None && currentFile.RequiresCheckout)
                        {
                            currentFile.CheckOut();
                        }
                    }
                }

                if (webparts.Count > 0)
                {
                    foreach (WebPartGalleryItem galleryItem in webparts)
                    {
                        WebPartZoneBase zone = WebPartManager.Zones[eventArg[1]];
                        //base.waitScreenText = string.Format("Adding {0} in {1} zone.", item.Title, zone.DisplayTitle);
                        galleryItem.Source.AddItemToPage(zone, 0, galleryItem, galleryItem.Id);
                        
                    }
                    //SPContext.Current.ListItem.Update();
                }
            }
            catch (Exception ex)
            {
                result = ex;
            }
        }
        [SharePointPermission(SecurityAction.Demand, ObjectModel = true)]
        public void RaisePostBackEvent(string eventArgument)
        {
            SPContext.Current.Web.AllowUnsafeUpdates = true;
            try
            {
                string[] eventArg = eventArgument.Split(new string[1] { "#;" }, StringSplitOptions.RemoveEmptyEntries);
                List<WebPartGalleryItem> webparts = Sources.Where(n => n.Id == eventArg[0]).Select(n => n).ToList();

                SPListItem item = SPContext.Current.ListItem;
                SPFile currentFile = item.File;

                if (item != null && item != null)
                {
                    if (currentFile != null && currentFile.Exists)
                    {
                        if (currentFile.CheckOutType == SPFile.SPCheckOutType.None && currentFile.RequiresCheckout)
                        {
                            currentFile.CheckOut();
                        }
                    }
                }

                string wpid = StorageKeyToID(eventArg[0]);
                if (this.WebPartManager.WebParts[wpid] != null)
                {
                    wpid = null;
                }

                if (webparts.Count > 0)
                {
                    WebPartZoneBase zone = WebPartManager.Zones[eventArg[1]];
                    
                    foreach (WebPartGalleryItem galleryItem in webparts)
                    {
                        galleryItem.Source.AddItemToPage(zone, 0, galleryItem,wpid);//, galleryItem.Id);
                    }

                    if (wpid != null)
                    {
                        if (this.WebPartManager.WebParts[wpid] != null)
                        {
                            System.Web.UI.WebControls.WebParts.WebPart wp = this.WebPartManager.WebParts[wpid];
                            
                            //this.WebPartManager.MoveWebPart(wp, zone, 0);

                            //this.WebPartManager.AddWebPart(wp)
                            this.WebPartManager.SaveChanges(this.WebPartManager.GetStorageKey(wp));
                        }

                    }

                }

            }
            catch (Exception ex)
            {
                result = ex;
            }
            SPContext.Current.Web.AllowUnsafeUpdates = false;

            this.RefreshPageState();

            HtmlMeta pragma = new HtmlMeta();
            pragma.HttpEquiv = "Pragma";
            pragma.Content = "no-cache";
            Page.Header.Controls.Add(pragma);
            HtmlMeta expires = new HtmlMeta();
            expires.HttpEquiv = "Expires";
            expires.Content = "-1";
            Page.Header.Controls.Add(expires);
            HtmlMeta store = new HtmlMeta();
            store.HttpEquiv = "Cache-Control";
            store.Content = "no-store, no-cache, must-revalidate";
            Page.Header.Controls.Add(store);

            HtmlMeta post = new HtmlMeta();
            post.HttpEquiv = "Cache-Control";
            post.Content = "post-check=0, pre-check=0";
            Page.Header.Controls.Add(post);

        }

        internal static string StorageKeyToID(string storageKey)
        {
            if (!string.IsNullOrEmpty(storageKey))
            {
                return ("g_" + storageKey.ToString().Replace('-', '_'));
            }
            return string.Empty;
        }

        internal static Guid IDToStorageKey(string storageKey)
        {
            if (!string.IsNullOrEmpty(storageKey))
            {
                string guidstring = storageKey.ToString().Replace('_', '-');
                guidstring = guidstring.Remove(0,2);
                return new Guid(guidstring);
            }
            return Guid.Empty;
        }

    }
}

