﻿using System;

namespace Microsoft.Practices.Prism.Navigation
{
    public interface INavigationService
    {
        /// <summary>
        /// Navigates to the most recent entry in the back navigation history, or throws an exception if no entry exists in back navigation.
        /// </summary>
        bool CanGoBack { get; }

        /// <summary>
        /// Navigates to the most recent entry in the forward navigation history, or throws an exception if no entry exists in forward navigation.
        /// </summary>
        bool CanGoForward { get; }

        /// <summary>
        /// Gets or sets the data type of the current content, or the content that should be navigated to.
        /// </summary>
        Type SourcePageType { get; set; }

        /// <summary>
        /// Gets the data type of the content that is currently displayed.
        /// </summary>
        Type CurrentSourcePageType { get; }

        /// <summary>
        /// Navigates to the most recent entry in the back navigation history, or throws an exception if no entry exists in back navigation.
        /// </summary>
        void GoBack();

        /// <summary>
        /// Navigates to the most recent entry in the forward navigation history, or throws an exception if no entry exists in forward navigation.
        /// </summary>
        void GoForward();

        /// <summary>
        /// Causes the Frame to load content represented by the specified Page-derived data type, also passing a parameter to be interpreted by the target of the navigation.
        /// </summary>
        /// <param name="sourcePageType">The data <see cref="System.Type" /> of the content to load.</param>
        /// <param name="parameter">The navigation parameter to pass to the target page. May be null.</param>
        bool Navigate(Type sourcePageType, object parameter = null);
    }
}
