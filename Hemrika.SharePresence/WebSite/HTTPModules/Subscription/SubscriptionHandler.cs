// -----------------------------------------------------------------------
// <copyright file="SubscriptionHandler.cs" company="">
// TODO: Update copyright text.
// </copyright>
// -----------------------------------------------------------------------

namespace Hemrika.SharePresence.WebSite.Modules.Subscription
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Web;
    using System.Web.SessionState;

    public class SubscriptionHandler : IHttpHandler, IReadOnlySessionState
    {
        internal readonly IHttpHandler OriginalHandler;

        public SubscriptionHandler(IHttpHandler originalHandler)
        {
            OriginalHandler = originalHandler;
        }

        public void ProcessRequest(HttpContext context)
        {
            // do not worry, ProcessRequest() will not be called, but let's be safe
            throw new InvalidOperationException("MyHttpHandler cannot process requests.");
        }

        public bool IsReusable // IsReusable must be set to false since class has a member!
        {
            get { return false; }
        }
    }
}
