// THIS CODE AND INFORMATION IS PROVIDED "AS IS" WITHOUT WARRANTY OF
// ANY KIND, EITHER EXPRESSED OR IMPLIED, INCLUDING BUT NOT LIMITED TO
// THE IMPLIED WARRANTIES OF MERCHANTABILITY AND/OR FITNESS FOR A
// PARTICULAR PURPOSE.
//
// Copyright (c) Microsoft Corporation. All rights reserved

using System.Linq;
using System.Xml.Linq;

namespace Microsoft.Practices.Prism
{
    /// <summary>
    /// This helper class loads the AppManifest in memory, letting you obtain properties that are not exposed through the Windows Store API.
    /// Currently, this class has methods for discovering if the Search contract is enabled and to get the Applications Id.
    /// Nevertheless, you can extend it to get other values that you may need.
    /// </summary>
    public static class AppManifestHelper
    {
        private static readonly XDocument manifest = XDocument.Load("AppxManifest.xml", LoadOptions.None);
        private static readonly XNamespace xNamespace = XNamespace.Get("http://schemas.microsoft.com/appx/2010/manifest");

        /// <summary>
        /// Checks if the Search declaration was activated in the Package.appxmanifest.
        /// </summary>
        /// <returns>True if Search is declared</returns>
        public static bool IsSearchDeclared()
        {
            // Get the Extension nodes located under Package/Applications/Extensions
            var extensions = manifest.Descendants(xNamespace + "Extension");
            return extensions.Any(extension => extension.Attribute("Category") != null && extension.Attribute("Category").Value == "windows.search");
        }

        /// <summary>
        /// Retrieves the Application Id from the AppManifest.
        /// </summary>
        /// <returns>The Application Id</returns>
        public static string GetApplicationId()
        {
            // Get the Application node located under Package/Applications
            var applications = manifest.Descendants(xNamespace + "Application");

            foreach (var application in applications.Where(application => application.Attribute("Id") != null))
            {
                return application.Attribute("Id").Value;
            }

            return string.Empty;
        }
    }
}
