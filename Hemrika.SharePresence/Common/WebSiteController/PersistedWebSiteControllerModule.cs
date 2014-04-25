// <copyright company="Hemrika">
// Copyright Hemrika. All rights reserved.
// </copyright>
// <author>SEO\Administrator</author>
// <date>2011-09-09 20:48:32Z</date>
namespace Hemrika.SharePresence.Common.WebSiteController
{
    using System;
    using Microsoft.SharePoint.Administration;

    /// <summary>
    /// A class to hold the class name of the page controller modules
    /// </summary>
    internal sealed class PersistedWebSiteControllerModule : SPPersistedObject
    {
        /// <summary>
        /// Fully-qualified class name of the module
        /// </summary>
        [Persisted]
        private readonly string fullyQualifiedClassName;               

        /// <summary>
        /// Initializes a new instance of the <see cref="PersistedWebSiteControllerModule"/> class.
        /// </summary>
        public PersistedWebSiteControllerModule()
            : base()
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="PersistedWebSiteControllerModule"/> class.
        /// </summary>
        /// <param name="id">The id of the module</param>
        /// <param name="shortClassName">Short name of the class.</param>
        /// <param name="fullyQualifiedClassName">Full name of the class.</param>
        /// <param name="parent">The parent persisted object</param>
        public PersistedWebSiteControllerModule(Guid id, string shortClassName, string fullyQualifiedClassName, SPPersistedObject parent) :
            base(String.Empty, parent)
        {
            this.Id = id;
            this.Name = shortClassName;
            this.fullyQualifiedClassName = fullyQualifiedClassName;
        }

        /// <summary>
        /// Gets the fully qualified class name .
        /// </summary>
        /// <value>The fully qualified class name.</value>
        public string FullyQualifiedClassName
        {
            get { return this.fullyQualifiedClassName; }
        }

        protected override bool HasAdditionalUpdateAccess()
        {
            return true;
        }

        public Guid PersistedId
        {
            get
            {
                return this.Id;
            }            
        }
    }
}
