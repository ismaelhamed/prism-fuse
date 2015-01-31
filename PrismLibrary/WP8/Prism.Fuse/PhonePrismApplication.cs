using System;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Navigation;
using Microsoft.Phone.Controls;
using Microsoft.Phone.Shell;
using Microsoft.Practices.Prism.ViewModel;

namespace Microsoft.Practices.Prism
{
    using Microsoft.Practices.Prism.Navigation;

    public abstract class PhonePrismApplication : PrismApplicationBase<PhoneApplicationFrame>
    {
        // Fields
        private bool phoneApplicationInitialized;
        private AppManifest appManifest;
        private PrismNavigationService prismNavigationService;

        /// <summary>
        /// Occurs when an application is started.
        /// </summary>
        [Obsolete("The Startup event should not be used by Windows Phone applications. Instead, use the overridable event handlers (OnLaunching, OnClosing, OnActivated, OnDeactivated) exposed by the Application class.")]
#pragma warning disable 67
        public new event StartupEventHandler Startup;
#pragma warning restore 67

        /// <summary>
        /// The root frame used for navigation.
        /// </summary>
        public PhoneApplicationFrame RootFrame
        {
            get;
            protected set;
        }

        /// <summary>
        /// Procides a type-safe representation of the current app manifest file.
        /// </summary>
        public AppManifest Manifest
        {
            get
            {
                return appManifest ?? (appManifest = AppManifestHelper.Read());
            }
        }

        protected PhonePrismApplication()
        {
            UnhandledException += (s, e) => OnUnhandledException(e);
        }

        /// <summary>
        /// Initializes the frame.
        /// </summary>
        protected override sealed void InitializeFrame()
        {
            if (phoneApplicationInitialized)
                return;

            // Create the frame but don't set it as RootVisual yet; this allows the splash screen to remain active until the 
            // application is ready to render.
            RootFrame = CreateRootFrame();
            RootFrame.Navigated += CompleteInitializePhoneApplication;

#if WINDOWS_PHONE_8
            if (Manifest.ActivationPolicy == "Resume")
            {
                // Handle reset requests for clearing the backstack
                RootFrame.Navigated += CheckForResetNavigation;
            }
#endif
            // Register RootFrame with the SuspensionManager
            SuspensionManager.RegisterFrame(RootFrame, "AppFrame");

            OnRegisterKnownTypesForSerialization();

            // Create an instance of the PrismNavigationService 
            prismNavigationService = new PrismNavigationService(RootFrame, SuspensionManager);

            // Create an instance of the PhoneApplicationService and register it as an ApplicationService
            var phoneService = new PhoneApplicationService();
            phoneService.Launching += (s, e) => OnLaunching(e);
            phoneService.Closing += (s, e) => OnClosing();
            phoneService.Activated += OnActivated;
            phoneService.Deactivated += OnDeactivated;

            ApplicationLifetimeObjects.Add(phoneService);

            // Ensure we don't initialize again
            phoneApplicationInitialized = true;
        }

        /// <summary>
        /// Configures the LocatorProvider for the <see cref="Microsoft.Practices.ServiceLocation.ServiceLocator" />.
        /// </summary>
        protected override void ConfigureServiceLocator()
        { }

        /// <summary>
        /// Creates the frame or main window of the application.
        /// </summary>
        protected override PhoneApplicationFrame CreateRootFrame()
        {
            return new PhoneApplicationFrame();
        }

        /// <summary>
        /// Occurs when the application is being launched. This code will not execute when the application is reactivated.
        /// </summary>
        /// <param name="e">Provides data for Launching events.</param>
        protected virtual void OnLaunching(LaunchingEventArgs e)
        { }

        /// <summary>
        /// Occurs when the application is exiting. This code will not execute when the application is deactivated.
        /// </summary>
        protected virtual void OnClosing()
        { }

        /// <summary>
        /// Occurs when the application is being made active (brought to foreground) after previously being put into a dormant state or tombstoned. 
        /// </summary>
        protected virtual void OnResuming()
        { }

        /// <summary>
        /// Occurs when the application is being deactivated (sent to background).
        /// </summary>
        protected virtual void OnSuspending()
        { }

        /// <summary>
        /// Occurs when an exception that is raised by Silverlight is not handled.
        /// </summary>
        /// <param name="e">Provides data for the Application.UnhandledException event.</param>
        protected virtual void OnUnhandledException(ApplicationUnhandledExceptionEventArgs e)
        { }

        /// <summary>
        /// Used for setting up the list of known types for the SessionStateService, using the RegisterKnownType method.
        /// </summary>
        protected virtual void OnRegisterKnownTypesForSerialization()
        {
            SuspensionManager.RegisterKnownType(typeof(BindableBase));
        }

#if WINDOWS_PHONE_8
        /// <summary> 
        /// Checks for a reset navigation.
        /// </summary> 
        /// <param name="sender">The sender.</param> 
        /// <param name="e">A NavigationEventArgs that contains the event data.</param> 
        private void CheckForResetNavigation(object sender, NavigationEventArgs e)
        {
            // If the app has received a 'reset' navigation, then we need to check 
            // on the next navigation to see if the page stack should be reset 
            if (e.NavigationMode == NavigationMode.Reset)
            {
                RootFrame.Navigated += ClearBackStackAfterReset;
            }
        }

        /// <summary> 
        /// Clears back stack after a reset.
        /// </summary> 
        /// <param name="sender">The sender.</param> 
        /// <param name="e">A NavigationEventArgs that contains the event data.</param> 
        private void ClearBackStackAfterReset(object sender, NavigationEventArgs e)
        {
            // Unregister the event so it doesn't get called again
            RootFrame.Navigated -= ClearBackStackAfterReset;

            // Only clear the stack for 'new' (forward) and 'refresh' navigations
            if (e.NavigationMode != NavigationMode.New && e.NavigationMode != NavigationMode.Refresh)
                return;

            // For UI consistency, clear the entire page stack
            while (RootFrame.RemoveBackEntry() != null)
            {
                // do nothing
            }
        }
#endif

        /// <summary>
        /// Occurs when the content that is being navigated to has been found, and is available from the Content property, 
        /// although it may not have completed loading.
        /// </summary>
        /// <param name="sender">The sender.</param>
        /// <param name="e">A NavigationEventArgs that contains the event data.</param>
        private void CompleteInitializePhoneApplication(object sender, NavigationEventArgs e)
        {
            // Set the root visual to allow the application to render
            if (RootVisual != RootFrame)
                RootVisual = RootFrame;

            // Remove this handler since it is no longer needed
            RootFrame.Navigated -= CompleteInitializePhoneApplication;
        }

        /// <summary>
        /// Occurs when the application is being made active (brought to foreground) after previously being put into a dormant state or tombstoned. 
        /// </summary>
        /// <param name="sender">The sender.</param>
        /// <param name="e">The event args.</param>
        private async void OnActivated(object sender, ActivatedEventArgs e)
        {
            // Application is re-activating either from dormant state or tombstoning (suspended).
            prismNavigationService.IsResuming = true;
            prismNavigationService.IsApplicationInstancePreserved = e.IsApplicationInstancePreserved;

            OnResuming();

            await RestoreStatus();

            NavigatedEventHandler onNavigated = null;
            onNavigated = (o, args) =>
            {
                prismNavigationService.IsResuming = false;
                prismNavigationService.IsApplicationInstancePreserved = false;

                RootFrame.Navigated -= onNavigated;
            };

            RootFrame.Navigated += onNavigated;
        }

        /// <summary>
        /// Occurs when the application is being deactivated.
        /// </summary>
        /// <param name="sender">The sender.</param>
        /// <param name="e">Provides data for the Deactivated event.</param>
        private async void OnDeactivated(object sender, DeactivatedEventArgs e)
        {
            OnSuspending();

            // Save application state
            await SuspensionManager.SaveAsync();
        }

        private async Task RestoreStatus()
        {
            try
            {
                await SuspensionManager.RestoreAsync();
            }
            catch (SuspensionManagerException)
            {
                // Something went wrong restoring state.
                // Assume there is no state and continue
            }
        }
    }
}