using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.SharePoint.WebControls;

namespace  Hemrika.SharePresence.Common.Ribbon.Commands
{
    /// <summary>
    /// This helps to convert  Hemrika.SharePresence.Common.RibbonCommand class instances to SPRibbonCommand instances.
    /// Note: We are using  Hemrika.SharePresence.Common.RibbonCommand to allow SharePoint sandboxed solutions.
    /// </summary>
    internal class RibbonCommandConverter
    {
        public static IEnumerable<IRibbonCommand> Convert(IEnumerable<RibbonCommand> commands)
        {
            return commands.Select<RibbonCommand, IRibbonCommand>(c => new SPRibbonCommand(
                c.Id,
                c.HandlerStatement,
                c.EnabledStatement));
        }

    }
}
