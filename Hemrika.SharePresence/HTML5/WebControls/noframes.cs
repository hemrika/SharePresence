using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web.UI.HtmlControls;

namespace Hemrika.SharePresence.Html5.WebControls
{
    public class NoFrames : ContainerControl
    {
        /// <summary>
        /// Not supported in HTML5
        /// </summary>
        public NoFrames(): base(ContainerType.NoFrames) { }
        
    }
}
