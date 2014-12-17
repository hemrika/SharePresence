// -----------------------------------------------------------------------
// <copyright file="LicenseRepository.cs" company="Microsoft">
// TODO: Update copyright text.
// </copyright>
// -----------------------------------------------------------------------

namespace Hemrika.SharePresence.Common.License.LicenseProxy
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using Hemrika.SharePresence.Common.Configuration;
    using Hemrika.SharePresence.Common.ServiceLocation;
    using Hemrika.SharePresence.SPLicense.LicenseFile;
    using Microsoft.SharePoint;
    using System.IO;
    using System.ComponentModel;
    using Microsoft.SharePoint.Administration;
    using System.Xml;
    using System.Threading;

    /// <summary>
    /// TODO: Update summary.
    /// </summary>
    public class LicenseRepository : ILicenseRepository
    {
        private IServiceLocator serviceLocator;
        private IConfigManager configManager;
        private SPFarmPropertyBag Farmbag;
        //private string encrypt;
        public LicenseRepository()
        {
            serviceLocator = SharePointServiceLocator.GetCurrent();
            configManager = serviceLocator.GetInstance<IConfigManager>();
            Farmbag = (SPFarmPropertyBag)configManager.GetPropertyBag(ConfigLevel.CurrentSPFarm);
        }

        public SPLicense.LicenseFile.SPLicenseFile GetLicense(Type type, object instance, bool allowExceptions)
        {
            try
            {
                TestLicense test = new TestLicense();
                SPLicenseFile license = test.Create(type, instance, allowExceptions);
                string key = new Guid("7F3A08D4-7308-4142-A1DF-DF705136D0A8").ToString();

                SPAdministrationWebApplication centralWebApp = SPAdministrationWebApplication.Local;
                SPSite lowSite = centralWebApp.Sites[0];

                SPUserToken oSysToken = GetSysToken(lowSite.ID);
                using (SPSite centralSite = new SPSite(lowSite.ID, oSysToken))
                {
                    centralSite.AllowUnsafeUpdates = true;
                    

                    using (SPWeb centralWeb = centralSite.OpenWeb())
                    {
                        bool availableList = true;
                        SPFile licenseFile = null;
                        SPFolder licenseFolder = null;

                        centralWeb.AllowUnsafeUpdates = true;
                        try
                        {

                            SPList LicenseList = centralWeb.Lists["Hemrika License Files"];
                            SPDocumentLibrary LicenseLibrary = (SPDocumentLibrary)LicenseList;
                            licenseFolder = centralWeb.Folders["Hemrika License Files"];


                            /*
                            if (!Farmbag.Contains("Hemrika_Encryption_Key"))
                            {
                                Farmbag["Hemrika_Encryption_Key"] = new Guid("7F3A08D4-7308-4142-A1DF-DF705136D0A8").ToString();
                            }

                            encrypt = Farmbag["Hemrika_Encryption_Key"];
                            */

                            SPQuery oQuery = new SPQuery();
                            oQuery.Query = string.Format("<Where><Contains><FieldRef Name=\"FileLeafRef\" /><Value Type=\"File\">" + type.Assembly.FullName + "</Value></Contains></Where><OrderBy><FieldRef Name=\"FileLeafRef\" /></OrderBy>");
                            SPListItemCollection colListItems = LicenseLibrary.GetItems(oQuery);

                            foreach (SPListItem licenseItem in colListItems)
                            {
                                licenseFile = licenseItem.File;
                                break;
                            }
                        }
                        catch (Exception)
                        {
                            availableList = false;
                        }

                        MemoryStream lic = null; //= new MemoryStream();
                        XmlDocument xml = new XmlDocument();

                        if (licenseFile == null)
                        {
                            lic = new MemoryStream();
                            xml.LoadXml(license.ToXmlString());

                            SPLicenseFile.SaveFile(license, lic, false,String.Empty, false);

                            if (availableList && licenseFolder != null)
                            {
                                licenseFile = licenseFolder.Files.Add(type.Assembly.FullName + ".lic", lic, true);

                                licenseFile.Update();
                                licenseFile.Item.Update();
                            }
                        }

                        if (lic != null)
                        {
                            lic.Close();
                            //lic.Dispose();
                        }

                        if (licenseFile != null)
                        {
                            //byte[] bytes = licenseFile.OpenBinary();
                            //lic = new MemoryStream();
                            //lic.Read(bytes, 0, bytes.Length);

                            license = SPLicenseFile.LoadFile(licenseFile.OpenBinaryStream(), type, false, String.Empty);// true, key);
                        }
                        /*
                        if (lic != null)
                        {
                            lic.Close();
                            //lic.Dispose();
                        }
                        */
                        centralWeb.AllowUnsafeUpdates = false;
                    }
                    centralSite.AllowUnsafeUpdates = false;
                }

                return license;

            }
            catch (Exception ex)
            {
                throw new LicenseException(type, instance,ex.Message,ex);
            }
        }


        public SPLicense.LicenseFile.SPLicenseFile SetLicense(Type type, SPLicense.LicenseFile.SPLicenseFile license)
        {
            throw new NotImplementedException();
        }

        private static SPUserToken GetSysToken(Guid SPSiteID)
        {
            SPUserToken sysToken = new SPSite(SPSiteID).SystemAccount.UserToken;
            if (sysToken == null)
            {
                SPSecurity.RunWithElevatedPrivileges(
                delegate()
                {
                    using (SPSite site = new SPSite(SPSiteID))
                    {
                        sysToken = site.SystemAccount.UserToken;
                    }
                });
            }
            return sysToken;
        }
    }
}
