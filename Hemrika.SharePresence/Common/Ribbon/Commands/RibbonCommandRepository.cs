﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using  Hemrika.SharePresence.Common.Ribbon.Definitions;
using  Hemrika.SharePresence.Common.Ribbon.Definitions.Controls;

namespace  Hemrika.SharePresence.Common.Ribbon.Commands
{
    /// <summary>
    /// Helper class for Ribbon commands managing and collecting
    /// </summary>
    internal class RibbonCommandRepository
    {
        private readonly object syncObject = new object(); 

        #region Singleton

        private static RibbonCommandRepository instance = null;

        public static RibbonCommandRepository Current
        {
            get
            {
                if (instance == null)
                    instance = new RibbonCommandRepository();

                return instance;
            }
        }

        private RibbonCommandRepository()
        {
        }

        #endregion

        List<RibbonCommand> commands = new List<RibbonCommand>();

        public int GetCommandsCount()
        {
            lock (syncObject)
            {
                return commands.Count;
            }
        }

        public IEnumerable<RibbonCommand> GetCommands()
        {
            lock (syncObject)
            {
                return commands.AsReadOnly();
            }
        }

        public void ClearCommands()
        {
            lock (syncObject)
            {
                if (commands.Count == 0)
                    return;
                commands.Clear();
            }
        }

        public void AddCommands(RibbonDefinition definition)
        {
            lock (syncObject)
            {
                IEnumerable<ControlDefinition> controls;

                if (definition is ContextualGroupDefinition)
                    controls = (definition as ContextualGroupDefinition).Tabs.SelectMany(t => t.Groups).SelectMany(g => g.Controls).ToArray();
                else if (definition is TabDefinition)
                    controls = (definition as TabDefinition).Groups.SelectMany(g => g.Controls).ToArray();
                else if (definition is GroupDefinition)
                    controls = (definition as GroupDefinition).Controls.ToArray();
                else if (definition is ControlDefinition)
                    controls = new ControlDefinition[] { definition as ControlDefinition };
                else
                    throw new ArgumentException();

                // MRUSplitButtonDefinition: Command="{Id}MenuCommand"
                commands.AddRange(
                    controls
                    .WithDescendants(c => c is IContainer ? (c as IContainer).Controls : null)
                    .OfType<MRUSplitButtonDefinition>()
                    .Select<MRUSplitButtonDefinition, RibbonCommand>(c =>
                        new RibbonCommand(
                            c.FullId + "MenuCommand",
                            "handleCommand(properties['CommandValueId']); return true",
                            "true"
                            )
                        ).ToArray()
                    );

                // Buttons of all types, including Button, SplitButton, ToggleButton
                commands.AddRange(
                    controls
                    .WithDescendants(c => c is IContainer ? (c as IContainer).Controls : null)
                    .OfType<ButtonBaseDefinition>()
                    .Select<ButtonBaseDefinition, RibbonCommand>(b => new RibbonCommand(b.FullId + "Command", b.CommandJavaScript, b.CommandEnableJavaScript)).ToArray());

                // Initializable controls
                var initializationScript = "var initialValue = function() { {IVScript} }; var v = initialValue(); if (v != null) { properties['On'] = true; properties['Value'] = v; }";
                var buttonInitializationScript = "var initialValue = function() { {IVScript} }; var v = initialValue(); if (v != null) { properties['On'] = v; properties['Value'] = v; }";
                commands.AddRange(
                    controls
                    .WithDescendants(c => c is IContainer ? (c as IContainer).Controls : null)
                    .Where(c => c is IInitializable)
                    .SelectMany(c =>
                        c is ButtonBaseDefinition ?
                        new RibbonCommand[]
                    {
                        new  RibbonCommand(c.FullId + "QueryCommand", buttonInitializationScript.Replace("{IVScript}", (c as IInitializable).InitialValueJavaScript), "true")
                    }
                        :
                        new RibbonCommand[]
                    {
                        new  RibbonCommand(c.FullId + "Command", String.Empty, "true"),
                        new  RibbonCommand(c.FullId + "QueryCommand", initializationScript.Replace("{IVScript}", (c as IInitializable).InitialValueJavaScript), "true")
                    }).ToArray());
            }
        }



    }
}
