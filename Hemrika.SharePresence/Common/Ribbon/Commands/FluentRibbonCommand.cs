using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace  Hemrika.SharePresence.Common.Ribbon.Commands
{
    internal class  RibbonCommand
    {
        public string EnabledStatement { get; private set; }
        public string HandlerStatement { get; private set; }
        public string Id { get; private set; }

        public RibbonCommand(string id, string handlerStatement, string enabledStatement)
        {
            Id = id;
            HandlerStatement = handlerStatement;
            EnabledStatement = enabledStatement;
        }

    }
}
