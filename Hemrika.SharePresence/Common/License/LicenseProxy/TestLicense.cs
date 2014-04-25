// -----------------------------------------------------------------------
// <copyright file="TestLicense.cs" company="">
// TODO: Update copyright text.
// </copyright>
// -----------------------------------------------------------------------

namespace Hemrika.SharePresence.Common.License.LicenseProxy
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using Hemrika.SharePresence.SPLicense.LicenseFile;
    using System.Reflection;
    using Hemrika.SharePresence.SPLicense.LicenseFile.Constraints;
    using Microsoft.SharePoint.Administration;

    /// <summary>
    /// TODO: Update summary.
    /// </summary>
    public class TestLicense
    {
        public SPLicenseFile Create(Type type, object instance, bool allowExceptions)
        {
            SPLicenseFile manualCreate = new SPLicenseFile(type,String.Empty);
            manualCreate.SetupEncryptionKey(String.Empty);
            manualCreate.Product = new Product(Assembly.GetAssembly(type), true, "", "LicenseManager", "License Manager", "1.0", "Hemrika", "License Management",false);
            manualCreate.User = new User("Rutger Hemrika", "rutger@hemrika.nl", "Hemrika");
            manualCreate.Issuer = new Issuer("Hemrika", "rutger@hemrika.nl", "http://www.hemrika.nl/");

            FarmConstraint fc = new FarmConstraint(manualCreate);
            fc.CurrentFarm = SPFarm.Local.Id.ToString();

            manualCreate.Constraints.Add(fc);
            
            return manualCreate;
        }
    }
}
