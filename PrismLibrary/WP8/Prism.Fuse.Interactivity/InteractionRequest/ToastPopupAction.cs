//===============================================================================
// Microsoft patterns & practices
// A Case Study for Building Advanced Windows Phone Applications
//===============================================================================
// Copyright © Microsoft Corporation.  All rights reserved.
// This code released under the terms of the 
// Microsoft patterns & practices license (http://wp7guide.codeplex.com/license)
//===============================================================================

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
using System.Threading;
using System.Windows;
using System.Windows.Controls.Primitives;
using Microsoft.Xaml.Interactivity;

namespace Microsoft.Practices.Prism.Interactivity.InteractionRequest
{
    /// <summary>
    /// Displays a toast-like popup in response to a trigger event.
    /// </summary>
    public class ToastPopupAction : DependencyObject, IAction
    {
        private Timer closePopupTimer;

        /// <summary>
        /// The element name of the <see cref="Popup"/> to show upon the interaction request.
        /// </summary>
        public static readonly DependencyProperty PopupElementNameProperty =
            DependencyProperty.Register(
                "PopupElementName",
                typeof(string),
                typeof(ToastPopupAction),
                new PropertyMetadata(null));

        /// <summary>
        /// Gets or sets the name of the <see cref="Popup"/> element to show when
        /// an <see cref="IInteractionRequest"/> is received.
        /// </summary>
        public string PopupElementName
        {
            get { return (string)GetValue(PopupElementNameProperty); }
            set { SetValue(PopupElementNameProperty, value); }
        }

        /// <summary>
        /// Executes the action.
        /// </summary>
        /// <param name="sender">The object that is passed to the action by the behavior. Generally this is AssociatedObject or the target object.</param>
        /// <param name="parameter">The value of this parameter is determined by the caller.</param>
        /// <returns>true if updating the property value succeeds; otherwise, false.</returns>
        public object Execute(object sender, object parameter)
        {
            var requestedEventArgs = parameter as InteractionRequestedEventArgs;
            if (requestedEventArgs == null)
                return false;

            var associatedObject = sender as FrameworkElement;
            if (associatedObject == null) 
                return false;

            var popUp = (Popup)associatedObject.FindName(PopupElementName);
            popUp.DataContext = requestedEventArgs.Context;
            popUp.IsOpen = true;

            DisposeTimer();

            closePopupTimer = new Timer(s => Deployment.Current.Dispatcher.BeginInvoke(() => popUp.IsOpen = false), null, 6000, 5000);

            popUp.Closed += OnPopupClosed;

            return true;
        }

        private void OnPopupClosed(object sender, EventArgs e)
        {
            DisposeTimer();
            ((Popup)sender).Closed -= OnPopupClosed;
        }

        private void DisposeTimer()
        {
            lock (this)
            {
                if (closePopupTimer == null)
                    return;

                closePopupTimer.Dispose();
                closePopupTimer = null;
            }
        }
    }
}
