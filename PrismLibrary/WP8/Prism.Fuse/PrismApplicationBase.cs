namespace Microsoft.Practices.Prism
{
    using Microsoft.Practices.Prism.Logging;

#if NETFX_CORE
    public abstract class PrismApplicationBase<TFrame> : Windows.UI.Xaml.Application
#else
    public abstract class PrismApplicationBase<TFrame> : System.Windows.Application
#endif
        where TFrame : class, new()
    {
        /// <summary>
        /// Gets the <see cref="ILoggerFacade"/> for the application.
        /// </summary>
        protected ILoggerFacade Logger { get; set; }

        /// <summary>
        /// Create the <see cref="ILoggerFacade" /> used by the bootstrapper.
        /// </summary>
        /// <remarks>The base implementation returns a new TextLogger.</remarks>
        protected virtual ILoggerFacade CreateLogger()
        {
            return new EmptyLogger();
        }

        /// <summary>
        /// Gets the default <see cref="ISuspensionManager"/> for the application.
        /// </summary>
        protected ISuspensionManager SuspensionManager { get; set; }

        /// <summary>
        /// Creates the <see cref="SuspensionManager"/> that will be used as the default.
        /// </summary>
        /// <returns>A new instance of <see cref="SuspensionManager"/>.</returns>   
        protected virtual SuspensionManager CreateSuspensionManager()
        {
            return new SuspensionManager();
        }

        /// <summary>
        /// Initializes the frame.
        /// </summary>
        protected abstract void InitializeFrame();

        /// <summary>
        /// Creates the frame or main window of the application.
        /// </summary>
        protected abstract TFrame CreateRootFrame();

        /// <summary>
        /// Runs the bootstrapper process.
        /// </summary>
        protected abstract void Run();

        /// <summary>
        /// Configures the LocatorProvider for the <see cref="Microsoft.Practices.ServiceLocation.ServiceLocator" />.
        /// </summary>
        protected abstract void ConfigureServiceLocator();
    }
}
