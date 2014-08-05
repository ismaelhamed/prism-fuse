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

using System.Windows;
using Microsoft.Xaml.Interactivity;

namespace Microsoft.Practices.Prism.Interactivity.InteractionRequest
{
    /// <summary>
    /// Displays a message box with the content of the <see cref="InteractionRequestedEventArgs"/> as the item.
    /// </summary>
    public class MessageBoxAction : DependencyObject, IAction
    {
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

            var notification = requestedEventArgs.Context;
            if (notification is Confirmation)
            {
                var result = MessageBox.Show((string)requestedEventArgs.Context.Content, requestedEventArgs.Context.Title, MessageBoxButton.OKCancel);
                if (result == MessageBoxResult.OK)
                {
                    ((Confirmation)requestedEventArgs.Context).Confirmed = true;
                }
            }
            else
            {
                MessageBox.Show((string)requestedEventArgs.Context.Content, requestedEventArgs.Context.Title, MessageBoxButton.OK);
            }
            
            if (requestedEventArgs.Callback != null)
                requestedEventArgs.Callback.Invoke();

            return true;
        }
    }
}
