// -----------------------------------------------------------------------
// <copyright file="Account.cs" company="">
// TODO: Update copyright text.
// </copyright>
// -----------------------------------------------------------------------

namespace Hemrika.SharePresence.Google.Analytics
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using Hemrika.SharePresence.Google.Client;

    /// <summary>
    /// TODO: Update summary.
    /// </summary>
    public class Account : Item
    {
        [CLSCompliant(false)]
        public string Id { get { return this.id; } set { this.id = value; } }
        [CLSCompliant(false)]
        public string Kind { get { return this.kind; } set { this.kind = value; } }
        [CLSCompliant(false)]
        public string SelfLink { get { return this.selfLink; } set { this.selfLink = value; } }
        [CLSCompliant(false)]
        public string Name { get { return this.name; } set { this.name = value; } }
        [CLSCompliant(false)]
        public string Created { get { return this.created; } set { this.created = value; } }
        [CLSCompliant(false)]
        public string Updated { get { return this.updated; } set { this.updated = value; } }
        [CLSCompliant(false)]
        public ChildLink ChildLink { get { return this.childLink; } set { this.childLink = value; } }
    }
}
