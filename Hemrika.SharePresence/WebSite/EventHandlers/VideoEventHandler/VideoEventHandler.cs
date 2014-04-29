// <copyright file="VideoEventHandler.cs" company="SharePresence">
// Copyright SharePresence. All rights reserved.
// </copyright>
// <author>MARKETING\Administrator</author>
// <date>2013-04-14 14:26:44Z</date>
namespace Hemrika.SharePresence.WebSite
{
    using System;
    using System.Collections.Generic;
    using System.Security.Permissions;
    using System.Text;
    using Microsoft.SharePoint;
    using Microsoft.SharePoint.Security;
    using Hemrika.SharePresence.WebSite.Video;
    using System.IO;
using System.Diagnostics;
    using Hemrika.SharePresence.WebSite.Fields;
    using System.Threading;
    using Microsoft.SharePoint.Administration;

    /// <summary>
    /// TODO: Add comment for VideoEventHandler
    /// </summary>
    public class VideoEventHandler : SPItemEventReceiver
    {
        /// <summary>
        /// TODO: Add comment for event ItemAdded in VideoEventHandler 
        /// </summary>
        /// <param name="properties">Contains list event properties</param>
        [SharePointPermission(SecurityAction.LinkDemand, ObjectModel = true)]
        public override void ItemAdded(SPItemEventProperties properties)
        {
            base.ItemAdded(properties);

            if (properties.List.BaseTemplate.ToString() == "20009")
            {
                SPListItem item = properties.ListItem;

                if (item != null)
                {
                    string display = properties.ListItem.DisplayName;
                    string foldername = display;
                    if(display.IndexOf('-') != -1)
                    {
                        foldername = foldername.Split(new char[1] { '-' })[0].ToString();
                    }
                    
                    SPFile file = properties.ListItem.File;
                    if (file != null)
                    {
                        bool copied = false;
                        bool eventfire = false;

                        string filename = file.Name;
                        string extension = Path.GetExtension(filename);//.Replace(display, string.Empty).ToLower();

                        if (DoesFolderExist(properties.List, foldername))//, filename))
                        {
                            SPListItemCollection folders = properties.List.Folders;

                            foreach (SPListItem folder in folders)
                            {
                                SPFolder sfolder = folder.Folder;
                                if (sfolder.Name == foldername)
                                {
                                    copied = false;
                                    eventfire = false;
                                    if (sfolder.ItemCount > 0)
                                    {
                                        foreach (SPFile sfile in sfolder.Files)
                                        {
                                            if (sfile.Name == filename && file.ParentFolder.Name != sfolder.Name)
                                            {
                                                file.CopyTo(sfolder.Url + "/" + file.Name, true);
                                                file.Delete();
                                                file.Update();
                                                copied = true;
                                                break;
                                            }

                                            if (sfile.Name == filename && file.ParentFolder.Name == sfolder.Name)
                                            {
                                                eventfire = true;
                                            }
                                        }
                                    }

                                    if (!copied && !eventfire)
                                    {
                                        file.MoveTo(sfolder.Url + "/" + file.Name);
                                        file.Update();
                                        item = file.Item;
                                        break;
                                    }
                                }
                            }
                        }

                        item[SPBuiltInFieldId.Title] = display;

                        if (extension == ".avi")
                        {
                            item[SPBuiltInFieldId.ContentTypeId] = ContentTypes.ContentTypeId.AVI;
                        }

                        if (extension == ".vtt")
                        {
                            item[SPBuiltInFieldId.ContentTypeId] = ContentTypes.ContentTypeId.Caption;
                        }

                        if (extension == ".flv")
                        {
                            item[SPBuiltInFieldId.ContentTypeId] = ContentTypes.ContentTypeId.FLV;
                        }

                        if (extension == ".mov")
                        {
                            item[SPBuiltInFieldId.ContentTypeId] = ContentTypes.ContentTypeId.MOV;
                        }

                        if (extension == ".mp4")
                        {
                            item[SPBuiltInFieldId.ContentTypeId] = ContentTypes.ContentTypeId.MP4;
                        }

                        if (extension == ".mpg")
                        {
                            item[SPBuiltInFieldId.ContentTypeId] = ContentTypes.ContentTypeId.MPG;
                        }

                        if (extension == ".ogg" || extension == ".ogv")
                        {
                            item[SPBuiltInFieldId.ContentTypeId] = ContentTypes.ContentTypeId.OGG;
                        }

                        if (extension == ".webm")
                        {
                            item[SPBuiltInFieldId.ContentTypeId] = ContentTypes.ContentTypeId.WebM;
                        }

                        if (extension == ".wmv")
                        {
                            item[SPBuiltInFieldId.ContentTypeId] = ContentTypes.ContentTypeId.WMV;
                        }

                        if (extension == ".jpg" || extension == ".png")
                        {
                            item[SPBuiltInFieldId.ContentTypeId] = ContentTypes.ContentTypeId.Video;
                        }

                        try
                        {
                            item.SystemUpdate(false);
                        }
                        catch (Exception ex)
                        {
                            ex.ToString();
                        }

                        if (extension != ".jpg" && extension != ".png"  && extension != ".vtt")
                        {
                            UpdateFileInformation(item.File);
                        }
                    }
                }
            }
        }

        private void UpdateFileInformation(SPFile file)
        {
            string extension = Path.GetExtension(file.Name);

            if (extension != ".jpg" && extension != ".png" && extension != ".vtt")
            {
                string guiID = file.UniqueId.ToString();
                string tempfile = file.Name.Replace(file.Item.DisplayName, guiID);
                string tempvideo = string.Empty;
                string tempPoster = string.Empty;

                SPContext context = SPContext.Current;

                if (context == null)
                {
                    context = SPContext.GetContext(file.Web);
                }

                FFMpegProcessWrapper wrapper = new FFMpegProcessWrapper(context);
                FileSystemWrapper filesystem = new FileSystemWrapper();

                if (!filesystem.DirectoryExists(wrapper.settings.InputVideoPath))
                {
                    filesystem.CreateDirectory(wrapper.settings.InputVideoPath);
                }

                bool Isnew = false;

                if (filesystem.DirectoryExists(wrapper.settings.InputVideoPath))
                {
                    tempvideo = Path.Combine(wrapper.settings.InputVideoPath, tempfile);

                    if (!filesystem.FileExists(tempvideo))
                    {
                        filesystem.CreateFile(tempvideo);
                        Isnew = true;
                    }

                    if (Isnew)
                    {
                        Stream filestream = file.OpenBinaryStream();
                        filesystem.DemandFileIOPermission(System.Security.Permissions.FileIOPermissionAccess.AllAccess, tempvideo);
                        filesystem.WriteStreamToFile(filestream, tempvideo);
                        filestream.Flush();
                        filestream.Close();
                    }
                }

                SPSecurity.RunWithElevatedPrivileges(
                delegate()
                {
                    if (Isnew)
                    {
                        VideoFile video = new VideoFile(tempvideo);
                        video.UniqueId = file.UniqueId;
                        video.infoGathered = false;

                        try
                        {
                            wrapper.GetVideoInfo(video);

                            if (video.infoGathered)
                            {
                                try
                                {
                                    file.Item[BuildFieldId.Audio_Format] = video.AudioFormat;
                                    file.Item[BuildFieldId.Content_Bitrate] = video.BitRate;
                                    file.Item[BuildFieldId.Content_Duration] = video.Duration.TotalMinutes;
                                    file.Item[BuildFieldId.Content_Height] = video.Height;
                                    file.Item[BuildFieldId.Content_Width] = video.Width;
                                    file.Item[BuildFieldId.Video_Format] = video.VideoFormat;
                                    file.Item.SystemUpdate(false);
                                }
                                catch (Exception ex)
                                {
                                    ex.ToString();
                                }
                            }

                            wrapper.GetVideoPoster(video);

                            try
                            {
                                tempPoster = tempvideo.Replace(Path.GetExtension(tempvideo), ".jpg");

                                while (!filesystem.FileExists(tempPoster))
                                {
                                    Thread.Sleep(1000);
                                }

                                if (filesystem.FileExists(tempPoster))
                                {
                                    Stream posterStream = null;

                                    try
                                    {
                                        posterStream = filesystem.FileOpenRead(tempPoster);

                                        if (posterStream != null)
                                        {
                                            string PosterFile = file.Item.DisplayName + ".jpg";
                                            if (PosterFile.IndexOf('-') > -1)
                                            {
                                                PosterFile = PosterFile.Split(new char[1] { '-' })[0].ToString();
                                            }

                                            SPFile poster = null;

                                            try
                                            {
                                                poster = file.ParentFolder.Files[PosterFile];
                                            }
                                            catch (Exception ex)
                                            {
                                                ex.ToString();
                                                poster = null;
                                            }

                                            if (poster == null)
                                            {
                                                poster = file.ParentFolder.Files.Add(PosterFile, posterStream, true);
                                                poster.Update();

                                                poster.Item[SPBuiltInFieldId.ContentTypeId] = ContentTypes.ContentTypeId.Video;

                                                if (video.infoGathered)
                                                {
                                                    poster.Item[BuildFieldId.Content_Buffer] = wrapper.settings.Buffer;
                                                    poster.Item[BuildFieldId.Content_AutoPlay] = wrapper.settings.AutoPlay;
                                                    poster.Item[BuildFieldId.Content_Loop] = wrapper.settings.Loop;
                                                    poster.Item[BuildFieldId.Audio_Format] = video.AudioFormat;
                                                    poster.Item[BuildFieldId.Content_Bitrate] = video.BitRate;
                                                    poster.Item[BuildFieldId.Content_Duration] = video.Duration.TotalMinutes;
                                                    poster.Item[BuildFieldId.Content_Height] = video.Height;
                                                    poster.Item[BuildFieldId.Content_Width] = video.Width;
                                                    poster.Item[BuildFieldId.Video_Format] = video.VideoFormat;
                                                }
                                                poster.Item.SystemUpdate(false);
                                            }

                                        }
                                    }
                                    catch (Exception ex)
                                    {
                                        ex.ToString();
                                    }
                                    finally
                                    {
                                        if (posterStream != null)
                                        {
                                            posterStream.Flush();
                                            posterStream.Close();
                                        }
                                    }
                                }
                            }
                            catch (Exception ex)
                            {
                                ex.ToString();
                            }
                        }
                        catch (Exception ex)
                        {
                            ex.ToString();
                        }
                        finally
                        {
                            try
                            {
                                if (!string.IsNullOrEmpty(tempvideo))
                                {
                                    filesystem.DemandFileIOPermission(System.Security.Permissions.FileIOPermissionAccess.AllAccess, tempvideo);
                                    filesystem.DeleteFile(tempvideo);
                                }
                                if (!string.IsNullOrEmpty(tempPoster))
                                {
                                    filesystem.DemandFileIOPermission(System.Security.Permissions.FileIOPermissionAccess.AllAccess, tempPoster);
                                    filesystem.DeleteFile(tempPoster);
                                }
                            }
                            catch (Exception ex)
                            {
                                SPDiagnosticsService.Local.WriteTrace(0, new SPDiagnosticsCategory(ex.Source, TraceSeverity.High, EventSeverity.Error), TraceSeverity.High, ex.Message, ex.Data);
                            }
                        }
                    }
                });
            }
        }

        private bool DoesFolderExist(SPList list, string foldername)//, string filename)
        {
            bool exists = false;
            try
            {
                //SPListItemCollection folders = list.Folders;
                SPFolder existingFolder = null;

                foreach (SPFolder folder in list.RootFolder.SubFolders)
                {
                    //SPFolder spfolder = folder.Folder;
                    if (folder.Name == foldername)
                    {
                        existingFolder = folder;
                        exists = true;
                        break;
                    }
                }

                if(!exists)
                {
                    SPListItem newFolder = list.Folders.Add("/" + list.RootFolder.Url, SPFileSystemObjectType.Folder, foldername);
                    newFolder.Update();
                    list.Update();
                    exists = true;
                }
            }
            catch (Exception ex)
            {
                SPDiagnosticsService.Local.WriteTrace(0, new SPDiagnosticsCategory(ex.Source, TraceSeverity.High, EventSeverity.Error), TraceSeverity.High, ex.Message, ex.Data);
                exists = false;
            }

            return exists;
        }

        /// <summary>
        /// TODO: Add comment for event ItemUpdated in VideoEventHandler 
        /// </summary>
        /// <param name="properties">Contains list event properties</param>   
        [SharePointPermission(SecurityAction.LinkDemand, ObjectModel = true)]
        public override void ItemUpdated(SPItemEventProperties properties)
        {
            EventFiringEnabled = false;
            base.ItemUpdated(properties);
            if (properties.List.BaseTemplate.ToString() == "20009")
            {
                SPListItem item = properties.ListItem;

                if (item != null)
                {
                    string display = properties.ListItem.DisplayName;
                    SPFile file = properties.ListItem.File;

                    if (file != null)
                    {
                        string extension = Path.GetExtension(file.Name);
                        if (extension != ".jpg" && extension != ".vtt")
                        {
                            UpdateFileInformation(item.File);
                        }
                    }
                }
            }

            EventFiringEnabled = true;            
        }

        /// <summary>
        /// An item was deleted
        /// </summary>
        public override void ItemDeleted(SPItemEventProperties properties)
        {
            base.ItemDeleted(properties);
        }

        /// <summary>
        /// An item is being added
        /// </summary>
        public override void ItemAdding(SPItemEventProperties properties)
        {
            base.ItemAdding(properties);
        }
    }
}

