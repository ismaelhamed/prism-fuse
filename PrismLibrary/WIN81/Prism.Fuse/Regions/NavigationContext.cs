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
using Windows.UI.Xaml.Navigation;

namespace Microsoft.Practices.Prism.Regions
{
    /// <summary>
    /// Encapsulates information about a navigation request.
    /// </summary>
    public class NavigationContext
    {
        /// <summary>
        /// Gets any Parameter object passed to the target page for the navigation.
        /// </summary>
        /// <value>An object that potentially passes parameters to the navigation target. May be null.</value>
        public object Parameter
        {
            get;
            private set;
        }

        /// <summary>
        /// Specifies the type of navigation that is occurring.
        /// </summary>
        public NavigationMode NavigationMode
        {
            get;
            private set;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="NavigationContext"/> class for a region name and a <see cref="Uri"/>.
        /// </summary>
        /// <param name="navigationMode">Gets a value that indicates the direction of movement during navigation</param>
        /// <param name="parameter">Gets any Parameter object passed to the target page for the navigation.</param>
        public NavigationContext(NavigationMode navigationMode, object parameter = null)
        {
            Parameter = parameter;
            NavigationMode = navigationMode;
        }
    }
}
