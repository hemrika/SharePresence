// -----------------------------------------------------------------------
// <copyright file="ILicenseRepository.cs" company="Microsoft">
// TODO: Update copyright text.
// </copyright>
// -----------------------------------------------------------------------

namespace Hemrika.SharePresence.Common.License.LicenseProxy
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.IO;
    using Hemrika.SharePresence.SPLicense.LicenseFile;

    /// <summary>
    /// TODO: Update summary.
    /// </summary>
    public interface ILicenseRepository
    {
        SPLicenseFile GetLicense(Type type, object instance, bool allowExceptions);


        SPLicenseFile SetLicense(Type type, SPLicenseFile license);
    }
}
