using System;
using Windows.UI.Xaml.Controls;

namespace Microsoft.Practices.Prism.Navigation
{
    /// <summary>
    /// Provides methods, properties, and events to support navigation within an application.
    /// </summary>
    public sealed class NavigationService : INavigationService
    {
        // Fields
        private readonly Frame rootFrame;

        /// <summary>
        /// Navigates to the most recent entry in the back navigation history, or throws an exception if no entry exists in back navigation.
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
        /// Gets or sets the data type of the current content, or the content that should be navigated to.
        /// </summary>
        public Type SourcePageType
        {
            get { return rootFrame.SourcePageType; }
            set { rootFrame.SourcePageType = value; }
        }

        /// <summary>
        /// Gets the data type of the content that is currently displayed.
        /// </summary>
        public Type CurrentSourcePageType
        {
            get { return rootFrame.CurrentSourcePageType; }
        }

        public NavigationService(Frame rootFrame)
        {
            if (rootFrame == null)
                throw new ArgumentNullException("rootFrame");

            this.rootFrame = rootFrame;
        }

        /// <summary>
        /// Navigates to the most recent entry in the back navigation history, or throws an exception if no entry exists in back navigation.
        /// </summary>
        public void GoBack()
        {
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
        /// Causes the Frame to load content represented by the specified Page-derived data type, also passing a parameter to be interpreted by the target of the navigation.
        /// </summary>
        /// <param name="sourcePageType">The data <see cref="System.Type" /> of the content to load.</param>
        /// <param name="parameter">The navigation parameter to pass to the target page. May be null.</param>
        public bool Navigate(Type sourcePageType, object parameter = null)
        {
            return parameter == null ? rootFrame.Navigate(sourcePageType) : rootFrame.Navigate(sourcePageType, parameter);
        }
    }
}
