// Copyright (c) Microsoft Corporation. All rights reserved. See License.txt in the project root for license information.

using System.Windows;
using System.Windows.Input;
using Microsoft.Xaml.Interactivity;

namespace Microsoft.Practices.Prism.Interactivity
{
    /// <summary>
    /// Trigger action that executes a command when invoked. 
    /// It also maintains the Enabled state of the target control based on the CanExecute method of the command.
    /// </summary>
    public class InvokeCommandAction : DependencyObject, IAction
    {
        /// <summary>
        /// Dependency property identifying the command to execute when invoked.
        /// </summary>
        public static readonly DependencyProperty CommandProperty = DependencyProperty.Register(
            "Command",
            typeof(ICommand),
            typeof(InvokeCommandAction),
            new PropertyMetadata(null));

        /// <summary>
        /// Gets or sets the command to execute when invoked.
        /// </summary>
        public ICommand Command
        {
            get { return GetValue(CommandProperty) as ICommand; }
            set { SetValue(CommandProperty, value); }
        }

        /// <summary>
        /// Dependency property identifying the command parameter to supply on command execution.
        /// </summary>
        public static readonly DependencyProperty CommandParameterProperty = DependencyProperty.Register(
            "CommandParameter",
            typeof(object),
            typeof(InvokeCommandAction),
            new PropertyMetadata(null));

        /// <summary>
        /// Gets or sets the command parameter to supply on command execution.
        /// </summary>
        public object CommandParameter
        {
            get { return GetValue(CommandParameterProperty); }
            set { SetValue(CommandParameterProperty, value); }
        }

        /// <summary>
        /// Dependency property identifying the CommandParameterPath to be parsed to identify the child property of the trigger parameter to be used as the command parameter.
        /// </summary>
        public static readonly DependencyProperty CommandParameterPathProperty = DependencyProperty.Register(
            "CommandParameterPath",
            typeof(string),
            typeof(InvokeCommandAction),
            new PropertyMetadata(null));

        /// <summary>
        /// Gets or sets the TriggerParameterPath value.
        /// </summary>
        public string CommandParameterPath
        {
            get { return GetValue(CommandParameterPathProperty) as string; }
            set { SetValue(CommandParameterPathProperty, value); }
        }

        /// <summary>
        /// Executes the action.
        /// </summary>
        /// <param name="sender">The object that is passed to the action by the behavior. Generally this is AssociatedObject or the target object.</param>
        /// <param name="parameter">The value of this parameter is determined by the caller.</param>
        /// <returns>true if updating the property value succeeds; otherwise, false.</returns>
        public object Execute(object sender, object parameter)
        {
            object commandParameter;

            if (Command == null)
                return false;

            if (CommandParameter == null)
            {
                if (!string.IsNullOrEmpty(CommandParameterPath))
                {
                    // Walk the ParameterPath for nested properties.
                    var propertyPathParts = CommandParameterPath.Split('.');
                    var propertyValue = parameter;

                    foreach (var propertyPathPart in propertyPathParts)
                    {
                        var propInfo = propertyValue.GetType().GetProperty(propertyPathPart);
                        propertyValue = propInfo.GetValue(propertyValue, null);

                    }
                    commandParameter = propertyValue;
                }
                else
                {
                    commandParameter = parameter;
                }
            }
            else
            {
                commandParameter = CommandParameter;
            }

            if (Command.CanExecute(commandParameter))
            {
                Command.Execute(commandParameter);
                return true;
            }

            return false;
        }
    }
}