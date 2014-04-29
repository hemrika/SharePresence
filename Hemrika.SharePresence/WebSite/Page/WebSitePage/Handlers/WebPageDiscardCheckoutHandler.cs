using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.SharePoint.WebControls;

namespace Hemrika.SharePresence.WebSite.Page
{
    class WebPageDiscardCheckoutHandler : DiscardCheckoutCommandHandler
    {
        public WebPageDiscardCheckoutHandler(SPPageStateControl psc)
            : base(psc)
        {
        }
    }
}
