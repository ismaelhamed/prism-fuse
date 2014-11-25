using System;
using System.Collections.Generic;
using System.Windows.Controls;
using System.Windows.Navigation;
using Microsoft.Phone.Controls;

namespace Microsoft.Practices.Prism.Navigation
{
    /// <summary>
    /// Provides methods, properties, and events to support navigation within an application.
    /// </summary>
    public sealed class NavigationService : INavigationService
    {
        // Fields
        private readonly PhoneApplicationFrame rootFrame;

        /// <summary>
        /// Gets a value that indicates whether there is at least one entry in the back navigation history.
        /// </summary>
        public bool CanGoBack
        {
            get { return rootFrame.CanGoBack; }
        }

        /// <summary>
        /// Navigates to the most recent entry in the forward navigation history, or throws an exception if no entry exists in forward navigation.
        /// </summary>
        public bool CanGoForward
        {
            get { return rootFrame.CanGoForward; }
        }

        /// <summary>
        /// Gets the uniform resource identifier (URI) of the content that is currently displayed.
        /// </summary>
        public Uri CurrentSource
        {
            get { return rootFrame.CurrentSource; }
        }

        /// <summary>
        /// Gets or sets the uniform resource identifier (URI) of the current content or the content that is being navigated to.
        /// </summary>
        public Uri Source
        {
            get { return rootFrame.Source; }
            set { rootFrame.Source = value; }
        }

        /// <summary>
        /// Gets the value of the Frame's ContentControl dependency property. This most probably will be the current PhoneApplicationPage.
        /// </summary>
        public object CurrentContent
        {
            get { return rootFrame.Content; }
        }

        /// <summary>
        /// Gets the backstack, as an enumerator of JournalEntry objects.
        /// </summary>
        public IEnumerable<JournalEntry> BackStack
        {
            get { return ((Page)rootFrame.Content).NavigationService.BackStack; }
        }

        public NavigationService(PhoneApplicationFrame rootFrame)
        {
            if (rootFrame == null)
                throw new ArgumentNullException("rootFrame");

            this.rootFrame = rootFrame;
        }

        /// <summary>
        /// Navigates to the most recent entry in the back navigation history.
        /// </summary>
        public void GoBack()
        {
            if (CanGoBack)
                rootFrame.GoBack();
        }

        /// <summary>
        /// Navigates to the most recent entry in the forward navigation history, or throws an exception if no entry exists in forward navigation.
        /// </summary>
        public void GoForward()
        {
            rootFrame.GoForward();
        }

        /// <summary>
        /// Causes the Frame to load content represented by the specified Page-derived data type, also passing a list of parameters 
        /// to be interpreted by the target of the navigation.
        /// </summary>
        /// <param name="sourcePageType">The data <see cref="System.Type" /> of the content to load.</param>
        /// <param name="parameters">The navigation parameters to pass to the target page. May be null.</param>
        /// <param name="removeEntryFromBackStack">Specifies whether this view should be removed from the BackStack. Default is false.</param>
        public bool Navigate(Type sourcePageType, NavigationParameters parameters = null, bool removeEntryFromBackStack = false)
        {
            // Gets the pack uri from the View type. 
            var uri = ViewLocator.DefaultViewTypeToPackUriResolver(sourcePageType);

            if (removeEntryFromBackStack)
            {
                if (parameters == null)
                {
                    parameters = new NavigationParameters();
                }

                parameters.Add("removeEntryFromBackStack", "true");
            }
            
            if (parameters != null)
            {
                uri += parameters.ToString();
            }          

            // Creates an Uri using both the view path and the queryString from the parameters.
            return rootFrame.Navigate(new Uri(uri, UriKind.Relative));
        }

        /// <summary>
        /// This method is used to remove the most recent entry from the back stack, or throws an InvalidOperationException if there are no 
        /// more entries to remove. If you want to remove more than one item, you call this method multiple times. This API is synchronous 
        /// and must be called from the UI thread.
        /// </summary>
        public JournalEntry RemoveBackEntry()
        {
            return ((Page)rootFrame.Content).NavigationService.RemoveBackEntry();
        }

        /// <summary>
        /// Stops asynchronous navigations that have not yet been processed.
        /// </summary>
        public void StopLoading()
        {
            rootFrame.StopLoading();
        }
    }
}
