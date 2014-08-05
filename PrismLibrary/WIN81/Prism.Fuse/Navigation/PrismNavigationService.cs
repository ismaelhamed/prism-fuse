using System;
using System.Collections.Generic;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Navigation;

namespace Microsoft.Practices.Prism.Navigation
{
    using Microsoft.Practices.Prism.Regions;

    /// <summary>
    /// The <see cref="PrismNavigationService" /> class provides access to various aspects of the application's lifetime: this 
    /// includes management of the application's idle behavior and management of the application's state when it becomes active 
    /// or inactive.
    /// </summary>
    internal class PrismNavigationService
    {
        // Fields
        private readonly Frame rootFrame;
        private readonly ISuspensionManager suspensionManager;

        /// <summary>
        /// Gets the <see cref="PrismNavigationService" /> object associated with the current application.
        /// </summary>
        public static PrismNavigationService Current
        {
            get;
            private set;
        }

        /// <summary>
        /// Instantiates a new instance of the <see cref="PrismNavigationService" /> class. 
        /// </summary>
        /// <param name="rootFrame">The frame.</param>
        /// <param name="suspensionManager">The session state service.</param>
        public PrismNavigationService(Frame rootFrame, ISuspensionManager suspensionManager)
        {
            if (rootFrame == null)
                throw new ArgumentNullException("rootFrame");

            if (suspensionManager == null)
                throw new ArgumentNullException("suspensionManager");

            if (PrismNavigationService.Current != null)
                throw new InvalidOperationException("Creating multiple instances of PhoneApplicationService is not supported.");

            PrismNavigationService.Current = this;

            this.rootFrame = rootFrame;
            this.rootFrame.Navigating += OnNavigating;
            this.rootFrame.Navigated += OnNavigated;

            this.suspensionManager = suspensionManager;
        }

        /// <summary>
        /// Occurs when the application transitions from Suspended state to Running state.
        /// </summary>
        public void Activating()
        {
            OnNavigatedImplementation();
        }

        /// <summary>
        /// Occurs when the application transitions to Suspended state from some other state.
        /// </summary>
        public void Deactivating()
        {
            OnNavigatingImplementation();
        }

        /// <summary>
        /// Occurs when a new navigation is requested.
        /// </summary>
        private void OnNavigating(object sender, NavigatingCancelEventArgs e)
        {
            OnNavigatingImplementation(e);
        }

        /// <summary>
        /// Occurs when the content that is being navigated to has been found and is available from the Content property, although it may not have completed loading.
        /// </summary>
        private void OnNavigated(object sender, NavigationEventArgs e)
        {
            // ReSharper disable once PossibleNullReferenceException
            suspensionManager.SessionState["LastNavigationPageKey"] = rootFrame.Content.GetType().FullName;
            suspensionManager.SessionState["LastNavigationParameterKey"] = e.Parameter;

            OnNavigatedImplementation(e);
        }

        /// <summary>
        /// Occurs when the content that is being navigated to has been found and is available.Moved the actual implementation 
        /// of the OnNavigated event because it needs to be call from multimple locations.
        /// </summary>
        /// <param name="e">Event data. Null if we're resuming from a Terminated state.</param>
        private void OnNavigatedImplementation(NavigationEventArgs e = null)
        {
            var frameState = suspensionManager.GetSessionStateForFrame(rootFrame);
            var key = "ViewModel-" + rootFrame.BackStackDepth;

            var navigationMode = (e != null ? e.NavigationMode : NavigationMode.Refresh);
            if (navigationMode == NavigationMode.New)
            {
                var nextViewModelKey = key;
                var nextViewModelIndex = rootFrame.BackStackDepth;

                // Clear existing state for forward navigation when adding a new page/view model to the navigation stack
                while (frameState.Remove(nextViewModelKey))
                {
                    nextViewModelIndex++;
                    nextViewModelKey = "ViewModel-" + nextViewModelIndex;
                }
            }

            var view = rootFrame.Content as FrameworkElement;
            if (view == null)
                return;

            // If application is being resumed from a terminated state, provide a SessionState bag and recover the original parameter being passed
            if (e == null)
            {
                var applicationLifecycleAware = view.DataContext as IApplicationLifecycleAware;
                if (applicationLifecycleAware != null)
                {
                    var sessionState = frameState.ContainsKey(key)
                        ? (Dictionary<string, object>)frameState[key]
                        : null;

                    applicationLifecycleAware.OnActivated(new SessionStateEventArgs(sessionState));
                }
            }

            var navigationAwareDataContext = view.DataContext as INavigationAware;
            if (navigationAwareDataContext != null)
            {
                // If application is being resumed from a terminated state, recover the original parameter being passed
                var parameter = (e == null ? suspensionManager.SessionState["LastNavigationParameterKey"] : e.Parameter);
                navigationAwareDataContext.OnNavigatedTo(new NavigationContext(navigationMode, parameter));
            }
        }

        /// <summary>
        /// Occurs when a new navigation is requested. Moved the actual implementation out of the OnNavigating event because 
        /// it needs to be called from multiple locations.
        /// </summary>
        /// <param name="e">Event data. Null if we're navigating away from the ViewModel due to a suspend event.</param>
        private void OnNavigatingImplementation(NavigatingCancelEventArgs e = null)
        {
            var view = rootFrame.Content as FrameworkElement;
            if (view == null)
                return;

            // If the ViewModel for the current active view implements IConfirmNavigationRequest, request confirmation
            // providing a callback to resume the navigation request.
            var confirmNavigationRequest = view.DataContext as IConfirmNavigationRequest;
            if (confirmNavigationRequest != null && e != null)
            {
                var cancelNavigation = false;
                var navigationContext = new NavigationContext(e.NavigationMode);

                confirmNavigationRequest.ConfirmNavigationRequest(
                    navigationContext,
                    canNavigate => { cancelNavigation = !canNavigate; });

                if (cancelNavigation)
                {
                    e.Cancel = true;
                }
            }

            var navigationAware = view.DataContext as INavigationAware;
            if (navigationAware != null)
            {
                var navigationMode = (e != null ? e.NavigationMode : NavigationMode.Refresh);
                navigationAware.OnNavigatedFrom(new NavigationContext(navigationMode));
            }

            var applicationLifecycleAware = view.DataContext as IApplicationLifecycleAware;
            if (applicationLifecycleAware != null)
            {
                // If application is being suspended, provide a SessionState bag
                if (e == null)
                {
                    var key = "ViewModel-" + rootFrame.BackStackDepth;
                    var frameState = suspensionManager.GetSessionStateForFrame(rootFrame);

                    var sessionState = frameState.ContainsKey(key)
                        ? (Dictionary<string, object>)frameState[key]
                        : new Dictionary<string, object>();

                    // Set session state before closing
                    frameState[key] = sessionState;

                    applicationLifecycleAware.OnDeactivated(new SessionStateEventArgs(sessionState));
                }
            }
        }
    }
}

