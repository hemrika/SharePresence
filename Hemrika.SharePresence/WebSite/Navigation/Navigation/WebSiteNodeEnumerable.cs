using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web.UI;
using System.Collections;

namespace Hemrika.SharePresence.WebSite.Navigation
{
    public class WebSiteNodeEnumerable : ArrayList, IHierarchicalEnumerable
    {
        public IHierarchyData GetHierarchyData(object enumeratedItem)
        {
            return (IHierarchyData)enumeratedItem;
        }
    }
}
