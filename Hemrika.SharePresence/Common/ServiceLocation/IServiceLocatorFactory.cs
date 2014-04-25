using System.Collections.Generic;

using Hemrika.SharePresence.Common;

namespace Hemrika.SharePresence.Common.ServiceLocation
{
    /// <summary>
    /// Interface for classes that will create and configure service locators, in such a way that
    /// they can be used by the <see cref="SharePointServiceLocator"/>. If you register
    /// an IServiceLocatorFactory in the <see cref="ServiceLocatorConfig"/>, it will use that 
    /// factory to create the service locator instance. 
    /// </summary>
    public interface IServiceLocatorFactory
    {
        /// <summary>
        /// Create the <see cref="IServiceLocator"/>
        /// </summary>
        /// <returns>The created service locator</returns>
        IServiceLocator Create();

        /// <summary>
        /// Loads the type mappings into the service locator. 
        /// </summary>
        /// <param name="serviceLocator">The service locator to load type mappings into.</param>
        /// <param name="typeMappings">The type mappings to load</param>
        void LoadTypeMappings(IServiceLocator serviceLocator, IEnumerable<TypeMapping> typeMappings);
    }
}