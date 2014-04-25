using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using  Hemrika.SharePresence.Common.Ribbon.Commands;

namespace  Hemrika.SharePresence.Common.Ribbon
{
    internal class ScriptHelper
    {

        internal static string GetCommandsScript(IEnumerable<RibbonCommand> commands)
        {
            if (commands == null || commands.Count() == 0)
                return String.Empty;

            return String.Format(@" 
                <script language=""javascript"" defer=""true""> 
                 //<![CDATA[
                    function DynamicgetFocusedCommands()
                    {{
                        return [{0}];
                    }}
                    function DynamicgetGlobalCommands()
                    {{
                        return [{1}];
                    }}
                    function DynamiccommandEnabled(commandId)
                    {{
                        {2}
                        return false;
                    }}
                    function DynamichandleCommand(commandId, properties, sequence)
                    {{
                        {3}
                        return false;
                    }}

                 //]]> 
                </script>",
                String.Join(",", commands.Where(c => !String.IsNullOrEmpty(c.Id) && c.Id.EndsWith("QueryCommand")).Select(c => "'" + c.Id + "'").ToArray()),
                String.Join(",", commands.Where(c => !String.IsNullOrEmpty(c.Id) && !c.Id.EndsWith("QueryCommand")).Select(c => "'" + c.Id + "'").ToArray()),
                String.Join("\n", commands.Select(c => String.Format("if (commandId == '{0}') {{ return {1}; }}", c.Id, c.EnabledStatement ?? String.Empty)).ToArray()),
                String.Join("\n", commands.Select(c => String.Format("if (commandId == '{0}') {{ {1}; return true; }}", c.Id, c.HandlerStatement ?? String.Empty)).ToArray())
                 );
        }

        internal static string GetPageComponentScript(string nameSpace)
        {
            // This is not the best way to load the script, but it has an important advantage: you don't need to deploy  Hemrika.SharePresence.Common.Ribbon.
            // All is now in the assembly, and all you need is to install the assembly in GAC.
            return @" 
                 <script language=""javascript"" defer=""true"">
                 //<![CDATA[

function ULS_SP() {
    if (ULS_SP.caller) {
        ULS_SP.caller.ULSTeamName = ""Windows SharePoint Services 4"";
        ULS_SP.caller.ULSFileName = ""{0}.js"";
    }
}

Type.registerNamespace('{0}');

{0}.PageComponent = function () {
    ULS_SP();
    {0}.PageComponent.initializeBase(this);
}
{0}.PageComponent.initialize = function () {
    ULS_SP();
    ExecuteOrDelayUntilScriptLoaded(
        Function.createDelegate(
            null,
            {0}.PageComponent.initializePageComponent),
        'SP.Ribbon.js');
}
{0}.PageComponent.initialize = function (controlId) {
    ULS_SP();
    {0}.PageComponent.ControlClientId = controlId;
    ExecuteOrDelayUntilScriptLoaded(
        Function.createDelegate(
            null,
            {0}.PageComponent.initializePageComponent),
        'SP.Ribbon.js');
}
{0}.PageComponent.refreshRibbonStatus = function () {
    SP.Ribbon.PageManager.get_instance().get_commandDispatcher().executeCommand(Commands.CommandIds.ApplicationStateChanged, null);
}

{0}.PageComponent.initializePageComponent = function () {
    ULS_SP();
    var ribbonPageManager = SP.Ribbon.PageManager.get_instance();
    if (null !== ribbonPageManager) {
        ribbonPageManager.addPageComponent({0}.PageComponent.instance);
        ribbonPageManager
            .get_focusManager()
            .requestFocusForComponent({0}.PageComponent.instance);
    }
}
{0}.PageComponent.refreshRibbonStatus = function () {
SP.Ribbon.PageManager
        .get_instance()
        .get_commandDispatcher()
        .executeCommand(Commands.CommandIds.ApplicationStateChanged, null);
}
{0}.PageComponent.ControlClientId = null;
 
{0}.PageComponent.prototype = {
    init: function () {
        ULS_SP();
    },
    getFocusedCommands: function () {
        ULS_SP();
        return DynamicgetFocusedCommands();
    },
    getGlobalCommands: function () {
        ULS_SP();
        return DynamicgetGlobalCommands();
    },
    isFocusable: function () {
        ULS_SP();
        return true;
    },
    receiveFocus: function() {
        ULS_SP();
        return true;
    },
    yieldFocus: function() {
        ULS_SP();
        return true;
    },
    canHandleCommand: function (commandId) {
        ULS_SP();
        return DynamiccommandEnabled(commandId);
    },
    handleCommand: function (commandId, properties, sequence) {
        ULS_SP();
        return DynamichandleCommand(commandId, properties, sequence);
    }
}
 
// Register classes

function initContextualPageComponent() {{
    {0}.PageComponent.registerClass('{0}.PageComponent', CUI.Page.PageComponent);
    {0}.PageComponent.instance = new {0}.PageComponent();
    {0}.PageComponent.initialize();
}}

NotifyScriptLoadedAndExecuteWaitingJobs(""{0}.js"");

ExecuteOrDelayUntilScriptLoaded(initContextualPageComponent,'sp.ribbon.js');

                 //]]> 
                 </script>".Replace("{0}", nameSpace);
        }

        internal static string GetInititalizeFuntion(string nameSpace)
        {
            return "";
        }
    }
}
