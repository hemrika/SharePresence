// -----------------------------------------------------------------------
// <copyright file="SPServiceExtensions.cs" company="">
// TODO: Update copyright text.
// </copyright>
// -----------------------------------------------------------------------

namespace Hemrika.SharePresence.Common.Extensions
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using Microsoft.SharePoint.Administration;

    /// <summary> 
    /// SPService extension methods 
    /// </summary> 
    public static class SPServiceExtensions
    {

        /// <summary>; 
        /// Gets the jobdefintions by name. 
        /// </summary>; 
        /// <param name="service">The service</param>; 
        /// <param name="name">The name.</param> 
        /// <returns>SPJobDefinition</returns>; 
        /// <exception cref="System.ArgumentNullException">Exception is thrown when the service or name equal null</exception> 
        public static SPJobDefinition GetJobDefinitionByName(this SPService service, string name)
        {
            if (service == null)
            {
                throw new ArgumentNullException("service", "Argument 'service' cannot be 'null'");
            }

            if (String.IsNullOrEmpty(name))
            {
                throw new ArgumentNullException("name", "Argument 'name' cannot be 'null' or 'String.Empty'");
            }

            var query = from SPJobDefinition job in service.JobDefinitions
                        where job.Name == name
                        select job;

            return query.FirstOrDefault();
        }


    }
}