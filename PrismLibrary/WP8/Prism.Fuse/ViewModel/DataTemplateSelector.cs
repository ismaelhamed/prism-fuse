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
using System.Windows.Controls;

namespace Microsoft.Practices.Prism.ViewModel
{
    /// <summary>
    /// This custom ContentControl changes its ContentTemplate based on the content it is presenting.
    /// </summary>
    /// <remarks>
    /// In order to determine the template it must use for the new content, this control retrieves it from its
    /// resources using the name for the type of the new content as the key.
    /// </remarks>
    public class DataTemplateSelector : ContentControl
    {
        /// <summary>
        /// Called when the value of the <see cref="P:System.Windows.Controls.ContentControl.Content"/> property changes. 
        /// </summary>
        /// <param name="oldContent">The old value of the <see cref="P:System.Windows.Controls.ContentControl.Content"/> property.</param>
        /// <param name="newContent">The new value of the <see cref="P:System.Windows.Controls.ContentControl.Content"/> property.</param>
        /// <remarks>Will attempt to discover the <see cref="DataTemplate"/> from the <see cref="ResourceDictionary"/> by matching the type name of <paramref name="newContent"/>.</remarks>
        protected override void OnContentChanged(object oldContent, object newContent)
        {
            var contentTemplate = GetDefaultContentTemplate();
            if (newContent != null)
            {
                var contentTypeName = newContent.GetType().Name;
                contentTemplate = Resources[contentTypeName] as DataTemplate;
            }

            ContentTemplate = contentTemplate;
        }

        /// <summary>
        /// Returns the default content template to use if not other content template can be located.
        /// </summary>
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1024:UsePropertiesWhereAppropriate")]
        protected virtual DataTemplate GetDefaultContentTemplate()
        {
            return null;
        }
    }
}
