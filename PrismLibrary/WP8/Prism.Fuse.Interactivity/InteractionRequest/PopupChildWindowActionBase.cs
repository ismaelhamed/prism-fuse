//===================================================================================
// Microsoft patterns & practices
// Composite Application Guidance for Windows Presentation Foundation and Silverlight
//===================================================================================
// Copyright (c) Microsoft Corporation.  All rights reserved.
// THIS CODE AND INFORMATION IS PROVIDED "AS IS" WITHOUT WARRANTY
// OF ANY KIND, EITHER EXPRESSED OR IMPLIED, INCLUDING BUT NOT
// LIMITED TO THE IMPLIED WARRANTIES OF MERCHANTABILITY AND
// FITNESS FOR A PARTICULAR PURPOSE.
//===================================================================================
// The example companies, organizations, products, domain names,
// e-mail addresses, logos, people, places, and events depicted
// herein are fictitious.  No association with any real company,
// organization, product, domain name, email address, logo, person,
// places, or events is intended or should be inferred.
//===================================================================================

using System;
using System.Windows;
using Microsoft.Xaml.Interactivity;

namespace Microsoft.Practices.Prism.Interactivity.InteractionRequest
{
    /// <summary>
    /// Base class for trigger actions that handle an interaction request by popping up a child window.
    /// </summary>
    public abstract class PopupChildWindowActionBase : DependencyObject, IAction
    {
        /// <summary>
        /// Executes the action.
        /// </summary>
        /// <param name="sender">The object that is passed to the action by the behavior. Generally this is AssociatedObject or the target object.</param>
        /// <param name="parameter">The value of this parameter is determined by the caller.</param>
        /// <returns>true if updating the property value succeeds; otherwise, false.</returns>
        public object Execute(object sender, object parameter)
        {
            var args = parameter as InteractionRequestedEventArgs;
            if (args == null)
            {
                return false;
            }

            var childWindow = GetChildWindow(args.Context);
            var callback = args.Callback;

            EventHandler handler = null;
            handler = (o, e) =>
            {
                childWindow.Closed -= handler;
                callback();
            };

            childWindow.Closed += handler;
            childWindow.Show();

            return true;
        }

        /// <summary>
        /// Returns the child window to display as part of the trigger action.
        /// </summary>
        /// <param name="notification">The notification to display in the child window.</param>
        /// <returns></returns>
        protected abstract PopupChildWindow GetChildWindow(Notification notification);
    }
}