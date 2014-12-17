using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web.Hosting;
using System.IO;
using System.Collections;
using Microsoft.SharePoint;
using System.Web;
using System.Net;
using Hemrika.SharePresence.WebSite.FieldTypes;
using Hemrika.SharePresence.WebSite.Fields;
using System.Collections.Specialized;
using Hemrika.SharePresence.Common.WebSiteController;
using Microsoft.SharePoint.Utilities;
using Microsoft.SharePoint.Administration;

namespace Hemrika.SharePresence.WebSite.Modules.WebPageModule
{
    class WebSiteFile : VirtualFile
    {
        public WebSiteFile(string virtualPath, WebSitePathProvider provider)
            : base(virtualPath)
        {
            this.provider = provider;
        }

        #region "Class Variables"
        private WebSitePathProvider provider;
        #endregion

        #region "Methods"
        /// <summary>
        /// Open the page layout.
        /// </summary>
        /// <returns></returns>
        public override Stream Open()
        {
            bool authenticated = false;// HttpContext.Current.User.Identity.IsAuthenticated;
            SPContext spcontext = null;
            HttpContext context = null;

            if (HttpContext.Current != null)
            {
                context = HttpContext.Current;
                authenticated = HttpContext.Current.User.Identity.IsAuthenticated;

                if (SPContext.Current == null)
                {
                    spcontext = SPContext.GetContext(HttpContext.Current);
                }
                else
                {
                    spcontext = SPContext.Current;
                }

            }

            if (context == null && spcontext == null)
            {
                throw (new HttpException((int)HttpStatusCode.NotFound, this.VirtualPath));
            }

            Stream stream = new MemoryStream();


            //SPSite site = new SPSite(spcontext.Web.Url);
            using (SPSite site = new SPSite(spcontext.Web.Url))
            {
                bool found = false;
                //SPWeb web = site.OpenWeb(this.VirtualPath);
                using (SPWeb web = site.OpenWeb(this.VirtualPath))
                {
                    found = web.Exists;
                    if (found)
                    {
                        GetFileContents(authenticated, stream, context, site, web);
                    }
                }


                if (!found)
                {
                    if (this.VirtualPath.StartsWith("/_"))
                    {
                        using (SPWeb web = site.RootWeb)
                        {
                            found = web.Exists;
                            if (found)
                            {
                                GetFileContents(authenticated, stream, context, site, web);
                            }
                        }
                    }
                    else
                    {
                        using (SPWeb web = site.OpenWeb())
                        {
                            found = web.Exists;
                            if (found)
                            {
                                GetFileContents(authenticated, stream, context, site, web);
                            }
                        }
                    }

                    if (!found)
                    {
                        using (SPWeb web = site.RootWeb)
                        {
                            found = web.Exists;
                            if (found)
                            {
                                GetFileContents(authenticated, stream, context, site, web);
                            }
                        }
                    }
                }


                //});

                if (stream != null)
                {
                    return stream;
                }
                else
                {
                    throw (new HttpException((int)HttpStatusCode.NoContent, ""));
                }
            }
        #endregion
        }

        private void GetFileContents(bool authenticated, Stream stream, HttpContext context, SPSite site, SPWeb web)
        {
            SPFile file = null;

            try
            {
                file = web.GetFile(this.VirtualPath);
            }
            catch { };

            if (file != null && file.Exists)
            {
                try
                {
                    byte[] binFile = null;
                    string content = string.Empty;

                    if (file.InDocumentLibrary)
                    {
                        SPListItem listItem = file.ListItemAllFields;

                        int versionId = 0;
                        bool versioning = file.DocumentLibrary.EnableVersioning;
                        SPFileVersionCollection versions = null;

                        if (versioning)
                        {
                            if (versioning && !authenticated && file.Level != SPFileLevel.Published && file.MajorVersion > 0)
                            {
                                versions = file.Versions;

                                try
                                {
                                    foreach (SPFileVersion version in versions)
                                    {
                                        if (version.Level == SPFileLevel.Published)
                                        {
                                            if (versionId <= version.ID)
                                            {
                                                versionId = version.ID;
                                            }
                                        }
                                    }
                                }
                                catch
                                {
                                }
                            }
                        }

                        string url = file.ServerRelativeUrl;

                        NameValueCollection queryString = context.Request.QueryString;
                        StringBuilder builder = new StringBuilder();
                        bool NoKeys = !queryString.HasKeys();

                        if (versionId > 0)
                        {
                            bool NoVersion = true;

                            foreach (string key in queryString.AllKeys)
                            {
                                if (key != null)
                                {
                                    //NoKeys = false;
                                    var value = queryString[key];
                                    if (key != "PageVersion")
                                    {
                                        builder.Append((NoKeys) ? "?" : "&" + key + "=" + value);
                                    }
                                    else
                                    {
                                        NoVersion = false;
                                    }

                                }
                            }

                            if (NoVersion)
                            {
                                builder.Append((NoKeys) ? "?" : "&");
                                builder.Append("PageVersion=" + versionId.ToString());
                                url = context.Request.Url.AbsolutePath + builder.ToString();
                                context.RewritePath(url);
                            }
                        }
                        /*
                        else if (file.MajorVersion > 0 && !authenticated)
                        {
                            int count = versions.Count;

                            for (int i = count; i > 0; i--)
                            {
                                try
                                {
                                    if (versions[i].Level == SPFileLevel.Published)
                                    {
                                        builder.Append((NoKeys) ? "?" : "&");
                                        builder.Append("PageVersion=" + versions[i].ID.ToString());
                                        url = context.Request.Url.AbsolutePath + builder.ToString();
                                        context.RewritePath(url);
                                        break;
                                    }
                                }
                                catch { };

                            }
                        }
                        */

                        if (versionId == 0 && !authenticated && versioning && file.Level != SPFileLevel.Published)// && !(file.MajorVersion > 0))
                        {
                            WebSiteControllerRule rule = WebSiteControllerConfig.GetRule("ErrorCode", "403");
                            string page = rule.Properties["ErrorPage"].ToString();
                            context.Response.Redirect("~/" + page + "?aspxerrorpath=" + url, false);
                        }

                        if (listItem != null)
                        {
                            PublishingPageDesignFieldValue value = null;
                            SPFile pageLayoutFile = null;
                            SPWeb rootWeb = null;
                            try
                            {
                                if (listItem.Fields.Contains(BuildFieldId.PublishingPageDesign))
                                {
                                    value = listItem[BuildFieldId.PublishingPageDesign] as PublishingPageDesignFieldValue;
                                }
                                if (value != null)
                                {
                                    rootWeb = site.RootWeb;

                                    if (rootWeb.ServerRelativeUrl != "/")
                                    {
                                        string rootsite = SPContext.Current.Site.RootWeb.Url.Replace(SPContext.Current.Site.ServerRelativeUrl, string.Empty);
                                        site = new SPSite(rootsite);
                                        rootWeb = site.OpenWeb();
                                    }

                                    pageLayoutFile = rootWeb.GetFile(value.Id);

                                    if (pageLayoutFile != null && !pageLayoutFile.Exists)
                                    {
                                        pageLayoutFile = rootWeb.GetFile(value.Url);
                                    }
                                }
                            }
                            catch { };


                            if (pageLayoutFile != null && pageLayoutFile.Exists)
                            {
                                if (pageLayoutFile.Item.HasPublishedVersion && (rootWeb != null && rootWeb.Exists))
                                {
                                    string slayout = rootWeb.GetFileAsString(pageLayoutFile.Url);
                                    binFile = Encoding.ASCII.GetBytes(slayout);

                                    //binFile = pageLayoutFile.OpenBinary();
                                }
                                else
                                {
                                    throw (new HttpException((int)HttpStatusCode.Forbidden, this.VirtualPath));
                                }
                            }
                            else
                            {
                                string sfile = web.GetFileAsString(file.Url);
                                binFile = Encoding.ASCII.GetBytes(sfile);

                                //binFile = file.OpenBinary();
                            }
                        }
                        else
                        {
                            throw (new HttpException((int)HttpStatusCode.Gone, this.VirtualPath));
                        }
                    }
                    else
                    {
                        binFile = file.OpenBinary();
                    }
                    //}
                    //else
                    //{
                    //    throw (new HttpException((int)HttpStatusCode.Forbidden, ""));
                    //}

                    if (binFile != null && binFile.Length > 0)
                    {
                        MemoryStream m = new MemoryStream(binFile);
                        StreamReader reader = new StreamReader(m);
                        content = reader.ReadToEnd();
                        reader.Close();
                        //m.Close();

                        StreamWriter writer = new StreamWriter(stream);

                        writer.Write(content);
                        writer.Flush();
                        stream.Seek(0, SeekOrigin.Begin);
                    }
                }
                catch (HttpException hex)
                {
                    throw;
                }
                catch (Exception ex)
                {
                    SPDiagnosticsService.Local.WriteTrace(0, new SPDiagnosticsCategory(ex.Source, TraceSeverity.High, EventSeverity.Error), TraceSeverity.High, ex.Message, ex.Data);
                    throw (new HttpException((int)HttpStatusCode.NotImplemented, ""));
                }
                finally
                {
                }
            }
            else
            {
                throw (new HttpException((int)HttpStatusCode.NotFound, this.VirtualPath));
            }
        }
    }
}
