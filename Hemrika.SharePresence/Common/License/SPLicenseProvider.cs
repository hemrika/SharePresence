// -----------------------------------------------------------------------
// <copyright file="SPLicenseProvider.cs" company="Microsoft">
// TODO: Update copyright text.
// </copyright>
// -----------------------------------------------------------------------

namespace Hemrika.SharePresence.Common.License
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.ComponentModel;
    using Hemrika.SharePresence.Common.License.LicenseProxy;
    using Hemrika.SharePresence.Common.Configuration;
    using Hemrika.SharePresence.Common.ServiceLocation;
    using Hemrika.SharePresence.SPLicense.LicenseFile;

    /// <summary>
    /// TODO: Update summary.
    /// </summary>
    public class SPLicenseProvider : LicenseProvider
    {
        private IServiceLocator serviceLocator;
        private ILicenseRepository serviceLicense;

        /// <summary>
        /// 
        /// </summary>
        public SPLicenseProvider()
        {
            serviceLocator = SharePointServiceLocator.GetCurrent();
            serviceLicense = serviceLocator.GetInstance<ILicenseRepository>();

        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="context"></param>
        /// <param name="type"></param>
        /// <param name="instance"></param>
        /// <param name="allowExceptions"></param>
        /// <returns></returns>
        public override License GetLicense(LicenseContext context, Type type, object instance, bool allowExceptions)
        {
            SPLicenseFile license = serviceLicense.GetLicense(type, instance, allowExceptions);
            //SPLicenseFile license = new SPLicenseFile(type,"This is a key");
            //SPLicenseFile license = serviceLicense.GetLicense(type, instance, allowExceptions);

            //license = serviceLicense.SetLicense(type, license);

            return license;
        }
    }
}
