using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Windows.ApplicationModel;
using Windows.ApplicationModel.Activation;
#if WINDOWS_APP
using Windows.UI.ApplicationSettings;
#endif
using Windows.UI.Core;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
#if WINDOWS_PHONE_APP
using Windows.UI.Xaml.Media.Animation;
using Windows.UI.Xaml.Navigation;
#endif

namespace Microsoft.Practices.Prism
{
    using Microsoft.Practices.Prism.Navigation;
    using Microsoft.Practices.Prism.ViewModel;

    public abstract class UniversalPrismApplication : PrismApplicationBase<Frame>
    {
        // Fields
#if WINDOWS_PHONE_APP
        private TransitionCollection transitions;
#endif
        private PrismNavigationService prismNavigationService;

        /// <summary>
        /// The root frame used for navigation.
        /// </summary>
        public Frame RootFrame
        {
            get;
            protected set;
        }

        /// <summary>
        /// Factory for creating the ExtendedSplashScreen instance.
        /// </summary>
        /// <value>The Func that creates the ExtendedSplashScreen. It requires a SplashScreen parameter, and must return a Page instance.</value>
        protected Func<SplashScreen, Page> ExtendedSplashScreenFactory
        {
            get;
            set;
        }

        /// <summary>
        /// Initializes the singleton application object. This is the first line of authored code
        /// executed, and as such is the logical equivalent of main() or WinMain().
        /// </summary>
        protected UniversalPrismApplication()
        {
            Resuming += OnInternalResuming;
            Suspending += OnInternalSuspending;
            UnhandledException += (s, e) => OnUnhandledException(e);
        }

        /// <summary>
        /// Invoked when the application is launched normally by the end user. Other entry points will be used when 
        /// the application is launched to open a specific file, to display search results, and so forth.
        /// </summary>
        /// <param name="args">Details about the launch request and process.</param>
        protected override sealed async void OnLaunched(LaunchActivatedEventArgs args)
        {
#if WINDOWS_APP
            if (ExtendedSplashScreenFactory != null)
            {
                var extendedSplashScreen = ExtendedSplashScreenFactory.Invoke(args.SplashScreen);
                RootFrame.Content = extendedSplashScreen;
            }
#endif

            // Delays the creation of the RootFrame until InitializeComponent() has been called.
            Run();

            // Register RootFrame with the SuspensionManager
            SuspensionManager.RegisterFrame(RootFrame, "AppFrame");

            // Create an instance of the PrismNavigationService
            prismNavigationService = new PrismNavigationService(RootFrame, SuspensionManager);

#if WINDOWS_APP 
            // Request for populating the Settings charm with the charm items registered by the user
            SettingsPane.GetForCurrentView().CommandsRequested += OnCommandsRequested;
#endif

            // Register knowntypes with the SuspensionManager
            OnRegisterKnownTypesForSerialization();

            // Try to restore the state when resuming from suspension
            await RestoreStatus(args.PreviousExecutionState);

            // Place the frame in the current Window
            if (Window.Current.Content != RootFrame)
                Window.Current.Content = RootFrame;

            // If the app is launched via the app's primary tile, the args.TileId property will have the same value as 
            // the AppUserModelId, which is set in the Package.appxmanifest. 
            // See http://go.microsoft.com/fwlink/?LinkID=288842
            var tileId = AppManifestHelper.GetApplicationId();

            if (RootFrame != null && (RootFrame.Content == null || args.TileId != tileId))
            {
#if WINDOWS_PHONE_APP
                // Removes the turnstile navigation for startup.
                if (RootFrame.ContentTransitions != null)
                {
                    transitions = new TransitionCollection();
                    foreach (var transition in RootFrame.ContentTransitions)
                    {
                        transitions.Add(transition);
                    }
                }

                RootFrame.ContentTransitions = null;
                RootFrame.Navigated += OnRootFrameFirstNavigated;
#endif

                await OnLaunching(args);
            }

            // Ensure the current window is active
            Window.Current.Activate();
            Window.Current.VisibilityChanged += OnVisibilityChanged;
        }

        /// <summary>
        /// Override this method with logic that will be performed after the application is initialized. For example, 
        /// navigating to the application's home page.
        /// </summary>
        /// <param name="e">The <see cref="LaunchActivatedEventArgs"/> instance containing the event data.</param>
        protected abstract Task OnLaunching(LaunchActivatedEventArgs e);

        /// <summary>
        /// Initializes the frame.
        /// </summary>
        protected override sealed void InitializeFrame()
        {
            // Do not repeat app initialization when the Window already has content.
            RootFrame = Window.Current.Content as Frame ?? CreateRootFrame();
        }

        /// <summary>
        /// Creates the frame or main window of the application.
        /// </summary>
        protected override Frame CreateRootFrame()
        {
            return new Frame();
        }

        /// <summary>
        /// Occurs when the application transitions from Suspended state to Running state.
        /// </summary>
        protected virtual void OnResuming()
        { }

        /// <summary>
        /// Occurs when the application transitions to Suspended state from some other state.
        /// </summary>
        protected virtual void OnSuspending()
        { }

        /// <summary>
        /// Occurs when an exception can be handled by app code, as forwarded from a native-level Windows Runtime error. Apps can mark 
        /// the occurrence as handled in event data.
        /// </summary>
        /// <param name="args">Event data.</param>
        protected virtual void OnUnhandledException(UnhandledExceptionEventArgs args)
        { }

        /// <summary>
        /// Occurs when the value of the Visible property changes. Immediately notifies when your app switches to the background.
        /// </summary>
        /// <param name="sender">The sender.</param>
        /// <param name="e">Contains the arguments returned by the event fired when a CoreWindow instance's visibility changes.</param>
        protected virtual void OnVisibilityChanged(object sender, VisibilityChangedEventArgs e)
        { }

        /// <summary>
        /// Used for setting up the list of known types for the SessionStateService, using the RegisterKnownType method.
        /// </summary>
        protected virtual void OnRegisterKnownTypesForSerialization()
        {
            SuspensionManager.RegisterKnownType(typeof(BindableBase));
        }

#if WINDOWS_APP
        /// <summary>
        /// Gets the Settings charm action items.
        /// </summary>
        /// <returns>The list of Setting charm action items that will populate the Settings pane.</returns>
        protected virtual IList<SettingsCommand> GetSettingsCommands()
        {
            return null;
        }

        /// <summary>
        /// Called when the Settings charm is invoked, this handler populates the Settings charm with the charm items returned by the GetSettingsCommands function.
        /// </summary>
        /// <param name="sender">The sender.</param>
        /// <param name="args">The <see cref="SettingsPaneCommandsRequestedEventArgs"/> instance containing the event data.</param>
        private void OnCommandsRequested(SettingsPane sender, SettingsPaneCommandsRequestedEventArgs args)
        {
            if (args == null || args.Request == null || args.Request.ApplicationCommands == null)
            {
                return;
            }

            var settingsCommands = GetSettingsCommands();
            var applicationCommands = args.Request.ApplicationCommands;

            foreach (var settingsCommand in settingsCommands)
            {
                applicationCommands.Add(settingsCommand);
            }
        }
#endif

        /// <summary>
        /// Occurs when the application transitions from Suspended state to Running state.
        /// </summary>
        /// <param name="sender">The source of the suspend request.</param>
        /// <param name="e">Null</param>
        private void OnInternalResuming(object sender, object e)
        {
            // When your app is resumed, it continues from the state that it was in when Windows suspended it. To be specific: app 
            // data and state are kept in memory while the app is suspended, so when it’s resumed, everything is as it was when the app 
            // was suspended. You don’t need to restore any saved data explicitly when receiving the resuming event.

            // But you want your app to appear to be always alive. For this, it has to be connected and display the latest data. It is 
            // possible that your app stays in a suspended state for quite a long time before it is resumed. Data or network connections 
            // could become stale and may need to be refreshed when your app resumes. When a user brings your app back to the foreground,
            // your app receives a resuming event. You can refresh the app content and reconnect network resources when your app receives 
            // this event.

            // Gives the user a chance to participate in the Resuming event.
            OnResuming();
        }

        /// <summary>
        /// Occurs when the application transitions to Suspended state from some other state. Application state is saved 
        /// without knowing whether the application will be terminated or resumed with the contents of memory still intact.
        /// </summary>
        /// <param name="sender">The source of the suspend request.</param>
        /// <param name="e">Details about the suspend request.</param>
        private async void OnInternalSuspending(object sender, SuspendingEventArgs e)
        {
            var deferral = e.SuspendingOperation.GetDeferral();
            await SaveStatus();
            deferral.Complete();
        }

#if WINDOWS_PHONE_APP
        /// <summary>
        /// Restores the content transitions after the app has launched.
        /// </summary>
        /// <param name="sender">The object where the handler is attached.</param>
        /// <param name="e">Details about the navigation event.</param>
        private void OnRootFrameFirstNavigated(object sender, NavigationEventArgs e)
        {
            var rootFrame = sender as Frame;
            if (rootFrame == null)
            {
                return;
            }

            rootFrame.ContentTransitions = this.transitions ?? new TransitionCollection { new NavigationThemeTransition() };
            rootFrame.Navigated -= this.OnRootFrameFirstNavigated;
        }
#endif

        private async Task SaveStatus()
        {
            // ...and here we go again. Notify the PrismNavigationService that the App is Suspending.
            prismNavigationService.Deactivating();

            // Gives the user a chance to participate in the Suspending event.
            OnSuspending();

            // Save application state
            await SuspensionManager.SaveAsync();
        }

        private async Task RestoreStatus(ApplicationExecutionState previousExecutionState)
        {
            if (previousExecutionState == ApplicationExecutionState.Terminated)
            {
                try
                {
                    await SuspensionManager.RestoreAsync();

                    // I really hate having to resort to doing this but, unlike in Silverlight, the Frame class in WinRT won't 
                    // fire the Navigating and Navigated events when Resuming/Suspending, so we're forced to manually notify 
                    // the PrismNavigationService.
                    prismNavigationService.Activating();
                }
                catch (SuspensionManagerException)
                {
                    // Something went wrong restoring state. Assume there is no state and continue
                }
            }
        }
    }
}
