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

namespace Microsoft.Practices.Prism
{
    /// <summary>
    /// Helper class for parsing <see cref="Uri"/> instances.
    /// </summary>
    public sealed class UriParsingHelper
    {
        /// <summary>
        /// Gets the query part of <paramref name="uri"/>.
        /// </summary>
        /// <param name="uri">The Uri.</param>
        public static string GetQuery(Uri uri)
        {
            return EnsureAbsolute(uri).Query;
        }

        /// <summary>
        /// Gets the AbsolutePath part of <paramref name="uri"/>.
        /// </summary>
        /// <param name="uri">The Uri.</param>
        public static string GetAbsolutePath(Uri uri)
        {
            return EnsureAbsolute(uri).AbsolutePath;
        }

        /// <summary>
        /// Parses the query of <paramref name="uri"/> into a <see cref="T:Microsoft.Practices.Prism.NameValueCollection" />.
        /// </summary>
        /// <param name="uri">The URI.</param>
        /// <returns>A <see cref="T:Microsoft.Practices.Prism.NameValueCollection" /> of query parameters and values.</returns>
        public static NavigationParameters ParseQuery(Uri uri)
        {
            var query = GetQuery(uri);
            return UriParsingHelper.ParseQueryString(query);
        }

        /// <summary>
        /// Parses a query string into a <see cref="T:Microsoft.Practices.Prism.NameValueCollection" />. 
        /// </summary>
        /// <param name="query">The query string to parse.</param>
        /// <returns>A <see cref="T:Microsoft.Practices.Prism.NameValueCollection" /> of query parameters and values.</returns>
        public static NavigationParameters ParseQueryString(string query)
        {
            return new NavigationParameters(query);
        }

        private static Uri EnsureAbsolute(Uri uri)
        {
            if (uri.IsAbsoluteUri)
                return uri;

            // It doesn't matter which host we use: we just need an absolute uri so that we can correctly parse its querystring.
            return !uri.OriginalString.StartsWith("/", StringComparison.Ordinal) 
                ? new Uri("http://localhost/" + uri, UriKind.Absolute) 
                : new Uri("http://localhost" + uri, UriKind.Absolute);
        }
    }
}
