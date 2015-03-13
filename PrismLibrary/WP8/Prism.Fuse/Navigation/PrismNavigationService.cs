using System;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Navigation;
using Microsoft.Phone.Controls;

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
        private readonly PhoneApplicationFrame rootFrame;
        private readonly ISuspensionManager suspensionManager;

        /// <summary>
        /// If true, the application has resumed from dormancy or a tombstoned state. 
        /// </summary>
        public bool IsResuming { get; set; }

        /// <summary>
        /// Gets or sets whether the application instance was preserved.
        /// </summary>
        public bool IsApplicationInstancePreserved { get; set; }

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
        public PrismNavigationService(PhoneApplicationFrame rootFrame, ISuspensionManager suspensionManager)
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
        /// Occurs when the content that is being navigated to has been found and is available.
        /// </summary>
        private void OnNavigated(object sender, NavigationEventArgs e)
        {
            if (e.Uri.IsAbsoluteUri || e.Content == null)
                return;

            var view = e.Content as FrameworkElement;
            if (view == null)
                return;

            if (IsResuming)
            {
                var applicationLifecycleAware = view.DataContext as IApplicationLifecycleAware;
                if (applicationLifecycleAware != null)
                {
                    var frameState = suspensionManager.GetSessionStateForFrame(rootFrame);
                    var sessionState = frameState.ContainsKey("CurrentViewModel")
                        ? (Dictionary<string, object>)frameState["CurrentViewModel"]
                        : null;

                    applicationLifecycleAware.OnActivated(IsApplicationInstancePreserved, new SessionStateEventArgs(sessionState));
                }
            }

            var navigationAwareDataContext = view.DataContext as INavigationAware;
            if (navigationAwareDataContext != null)
            {
                EventHandler onLayoutUpdate = null;
                onLayoutUpdate = (o, args) =>
                {
                    // To achieve better permorfance in WP7, the OnNavigatedTo is hooked up to the first LayoutUpdate 
                    // event call: this ensures the first frame is drawn before we do any heavy lifting.
                    navigationAwareDataContext.OnNavigatedTo(new NavigationContext(e.NavigationMode, e.Uri));
                    view.LayoutUpdated -= onLayoutUpdate;
                };

                view.LayoutUpdated += onLayoutUpdate;
            }

            // Remove this view from the BackStack
            if (e.NavigationMode == NavigationMode.New && e.Uri.ToString().Contains("removeEntryFromBackStack=true"))
            {
                rootFrame.RemoveBackEntry();
            }
        }

        /// <summary>
        /// Occurs when a new navigation is requested.
        /// </summary>
        private void OnNavigating(object sender, NavigatingCancelEventArgs e)
        {
            var view = rootFrame.Content as FrameworkElement;
            if (view == null)
                return;

            // If the ViewModel for the current active view implements IConfirmNavigationRequest, request confirmation
            // providing a callback to resume the navigation request
            var confirmNavigationRequest = view.DataContext as IConfirmNavigationRequest;
            if (confirmNavigationRequest != null)
            {
                // MSDN: In Windows Phone applications, you can use this property to determine whether the navigation system will 
                // ignore the Cancel property. This happens whenever the navigation destination is an external location, including 
                // other applications, launchers, and choosers. This also includes backward navigation that exits the application. 
                if (e.IsCancelable)
                {
                    var cancelNavigation = false;
                    var navigationContext = new NavigationContext(e.NavigationMode, e.Uri);

                    confirmNavigationRequest.ConfirmNavigationRequest(
                        navigationContext,
                        canNavigate => { cancelNavigation = !canNavigate; });

                    if (cancelNavigation)
                    {
                        e.Cancel = true;
                        return;
                    }
                }
            }

            var navigationAware = view.DataContext as INavigationAware;
            if (navigationAware != null)
            {
                // Raise the OnNavigatedFrom event just before navigating away.
                navigationAware.OnNavigatedFrom(new NavigationContext(e.NavigationMode, e.Uri));
            }

            // The Deactivated event is raised when the user navigates forward, away from your app, by pressing the Start button 
            // or by launching another application. The Deactivated event is also raised if your application launches a Chooser.
            if (e.Uri == rootFrame.CurrentSource || e.NavigationMode == NavigationMode.Back || e.IsNavigationInitiator)
                return;

            var applicationLifecycleAware = view.DataContext as IApplicationLifecycleAware;
            if (applicationLifecycleAware != null)
            {
                var frameState = suspensionManager.GetSessionStateForFrame(rootFrame);
                var sessionState = frameState.ContainsKey("CurrentViewModel")
                    ? (Dictionary<string, object>)frameState["CurrentViewModel"]
                    : new Dictionary<string, object>();

                // Set session state before closing
                frameState["CurrentViewModel"] = sessionState;

                applicationLifecycleAware.OnDeactivated(new SessionStateEventArgs(sessionState));
            }
        }
    }
}
