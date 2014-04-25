using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Reflection;
//using System.ComponentModel.Composition.Hosting;
//using System.ComponentModel.Composition;
using Microsoft.SharePoint.Security;
using System.Security.Permissions;

namespace Hemrika.SharePresence.Common.ServiceLocation
{
    /// <summary>
    /// 
    /// </summary>
    public class WebPartServiceLocator
    {
        #region Singleton

        /// <summary>
        /// 
        /// </summary>
        public static WebPartServiceLocator instance = null;

        /// <summary>
        /// 
        /// </summary>
        public static WebPartServiceLocator Current
        {
            get
            {
                if (instance == null)
                {
                    instance = new WebPartServiceLocator();
                }

                return instance;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        private WebPartServiceLocator()
        {
        }

        #endregion

        /// <summary>
        /// 
        /// </summary>
        //[ImportMany(typeof(IDisplayableClass))]
        public List<IDisplayableClass> DisplayableClasses;

        /// <summary>
        /// 
        /// </summary>
        /// <param name="assemblies"></param>
        [SharePointPermission(SecurityAction.InheritanceDemand, ObjectModel = true)]
        [SharePointPermission(SecurityAction.LinkDemand, ObjectModel = true)]
        public void Initialize(IEnumerable<Assembly> assemblies)
        {
            //TODO
            /*
            var catalog = new AggregateCatalog();
            foreach (var assembly in assemblies)
            {
                catalog.Catalogs.Add(new AssemblyCatalog(assembly));
            }

            var container = new CompositionContainer(catalog);
            container.ComposeParts(this);
            */
        }
    }
}
