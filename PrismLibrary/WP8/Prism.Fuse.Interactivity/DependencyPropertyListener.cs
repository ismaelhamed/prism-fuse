﻿//===============================================================================
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
using System.Windows;
using System.Windows.Data;

namespace Microsoft.Practices.Prism.Interactivity
{
    /// <summary>
    /// Registers a new dependency property for tracking data and provides notification on data changes.
    /// </summary>
    public class DependencyPropertyListener
    {
        private static int index;
        private FrameworkElement target;
        private readonly DependencyProperty property;

        /// <summary>
        /// This event is raised when the attached property value changes.
        /// </summary>
        public event EventHandler<BindingChangedEventArgs> Changed;

        /// <summary>
        /// Instantiates a new <see cref="DependencyPropertyListener"/>.
        /// </summary>
        /// <remarks>
        /// This registers creates an attached property with the name starting DependencyPropertyListener.  This
        /// attached property set on a <see cref="FrameworkElement"/> when <see cref="Attach"/> is called.</remarks>
        public DependencyPropertyListener()
        {
            property = DependencyProperty.RegisterAttached("DependencyPropertyListener" + index++, typeof(object), typeof(DependencyPropertyListener), new PropertyMetadata(null, HandleValueChanged));
        }

        /// <summary>
        /// Attaches a <see cref="DependencyProperty"/> to a framework element with
        /// the provided <see cref="Binding"/>.
        /// </summary>
        /// <param name="element">The <see cref="FrameworkElement"/> to attach the monitoring dependency property to.</param>
        /// <param name="binding">The binding to use with the attached property.</param>
        public void Attach(FrameworkElement element, Binding binding)
        {
            if (element == null)
                throw new ArgumentNullException("element");

            if (target != null)
                throw new InvalidOperationException("Cannot attach an already attached listener");

            target = element;
            target.SetBinding(property, binding);
        }

        ///<summary>
        /// Detaches binding listener from target.
        ///</summary>
        public void Detach()
        {
            target.ClearValue(property);
            target = null;
        }

        private void HandleValueChanged(DependencyObject sender, DependencyPropertyChangedEventArgs e)
        {
            var handler = Changed;
            if (handler != null)
            {
                handler(this, new BindingChangedEventArgs(e));
            }
        }
    }
}