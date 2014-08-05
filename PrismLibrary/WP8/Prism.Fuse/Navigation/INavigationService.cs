using System;
using System.Collections.Generic;
using System.Windows.Navigation;

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
        /// Gets the uniform resource identifier (URI) of the content that is currently displayed.
        /// </summary>
        Uri CurrentSource { get; }

        /// <summary>
        /// Gets or sets the uniform resource identifier (URI) of the current content or the content that is being navigated to.
        /// </summary>
        Uri Source { get; set; }

        /// <summary>
        /// Gets the value of the Frame's ContentControl dependency property. This most probably will be the current PhoneApplicationPage.
        /// </summary>
        object CurrentContent { get; }

        /// <summary>
        /// Gets the backstack, as an enumerator of JournalEntry objects.
        /// </summary>
        IEnumerable<JournalEntry> BackStack { get; }

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
        /// <param name="parameters">The navigation parameter to pass to the target page. May be null.</param>
        /// <param name="removeEntryFromBackStack"></param>
        bool Navigate(Type sourcePageType, NavigationParameters parameters = null, bool removeEntryFromBackStack = false);

        /// <summary>
        /// Removes the last back entry from the navigation history.
        /// </summary>
        JournalEntry RemoveBackEntry();

        /// <summary>
        /// Stops asynchronous navigations that have not yet been processed.
        /// </summary>
        void StopLoading();
    }
}
