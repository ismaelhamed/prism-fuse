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
using System.Windows.Navigation;

namespace Microsoft.Practices.Prism.Regions
{
    /// <summary>
    /// Encapsulates information about a navigation request.
    /// </summary>
    public class NavigationContext
    {
        /// <summary>
        /// Gets the <see cref="NavigationParameters"/> extracted from the URI.
        /// </summary>
        /// <value>The URI query.</value>
        public NavigationParameters Parameters
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
        /// <param name="navigationMode">Gets a NavigationMode value that indicates the type of navigation that is occurring.</param>
        /// <param name="uri">Gets the uniform resource identifier (URI) for the content being navigated to.</param>
        public NavigationContext(NavigationMode navigationMode, Uri uri)
        {
            Parameters = UriParsingHelper.ParseQuery(uri);
            NavigationMode = navigationMode;
        }
    }
}
