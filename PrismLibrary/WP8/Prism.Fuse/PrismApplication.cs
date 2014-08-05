using System;
using System.Globalization;
using Microsoft.Practices.ServiceLocation;

namespace Microsoft.Practices.Prism
{
    using Microsoft.Practices.Prism.IoC;
    using Microsoft.Practices.Prism.Logging;
    using Microsoft.Practices.Prism.Navigation;
    using Microsoft.Practices.Prism.PubSubEvents;

    public abstract class PrismApplication : PhonePrismApplication
    {
        // Fields
        private readonly bool useDefaultConfiguration;

        /// <summary>
        /// Gets or sets the default <see cref="TinyIoCContainer"/> for the application.
        /// </summary>
        public TinyIoCContainer Container
        {
            get;
            private set;
        }

        /// <summary>
        /// Creates an instance of the PrismApplication class.
        /// </summary>
        /// <param name="runWithDefaultConfiguration">If <see langword="true"/>, registers default services in the container. This is the default behavior.</param>
        protected PrismApplication(bool runWithDefaultConfiguration = true)
        {
            this.useDefaultConfiguration = runWithDefaultConfiguration;
            
            Run();
        }

        /// <summary>
        /// Run the bootstrapper process.
        /// </summary>
        protected override sealed void Run()
        {
            Logger = CreateLogger();
            if (Logger == null)
            {
                throw new InvalidOperationException(Properties.Resources.NullLoggerFacadeException);
            }

            SuspensionManager = CreateSuspensionManager();
            if (SuspensionManager == null)
            {
                throw new InvalidOperationException(Properties.Resources.NullSuspensionManagerException);
            }

            Logger.Log(Properties.Resources.InitializingShell, Category.Debug, Priority.Low);
            InitializeFrame();

            Logger.Log(Properties.Resources.CreatingContainer, Category.Debug, Priority.Low);
            Container = CreateContainer();
            if (Container == null)
            {
                throw new InvalidOperationException(Properties.Resources.NullContainerException);
            }

            Logger.Log(Properties.Resources.ConfiguringIocContainer, Category.Debug, Priority.Low);
            ConfigureContainer();

            Logger.Log(Properties.Resources.ConfiguringServiceLocatorSingleton, Category.Debug, Priority.Low);
            ConfigureServiceLocator();
            
            Logger.Log(Properties.Resources.BootstrapperSequenceCompleted, Category.Debug, Priority.Low);
        }

        /// <summary>
        /// Creates the <see cref="TinyIoCContainer"/> that will be used as the default container.
        /// </summary>
        /// <returns>A new instance of <see cref="TinyIoCContainer"/>.</returns>        
        protected virtual TinyIoCContainer CreateContainer()
        {
            return TinyIoCContainer.Current;
        }

        /// <summary>
        /// Configures the <see cref="Container"/> container. May be overwritten in a derived class to add specific
        /// type mappings required by the application.
        /// </summary>
        protected virtual void ConfigureContainer()
        {
            Container.Register(Logger);
            Container.Register<INavigationService>((c, p) => new NavigationService(RootFrame));

            if (useDefaultConfiguration)
            {
                RegisterTypeIfMissing(typeof(IServiceLocator), typeof(TinyIoCServiceLocator), true);
                RegisterTypeIfMissing(typeof(IEventAggregator), typeof(EventAggregator), true);
            }

            // Set a factory for the ViewModelLocator to use the default resolution mechanism to construct view models
            ViewModelLocator.SetDefaultViewModelFactory(t => Container.Resolve(t));
        }

        /// <summary>
        /// Configures the LocatorProvider for the <see cref="Microsoft.Practices.ServiceLocation.ServiceLocator" />.
        /// </summary>
        protected override void ConfigureServiceLocator()
        {
            var serviceLocator = Container.Resolve<IServiceLocator>();
            ServiceLocator.SetLocatorProvider(() => serviceLocator);
        }

        /// <summary>
        /// Registers the type if missing.
        /// </summary>
        /// <param name="fromType">From type.</param>
        /// <param name="toType">To type.</param>
        /// <param name="registerAsSingleton">if set to <c>true</c> [register as singleton].</param>
        private void RegisterTypeIfMissing(Type fromType, Type toType, bool registerAsSingleton)
        {
            if (fromType == null)
                throw new ArgumentNullException("fromType");

            if (toType == null)
                throw new ArgumentNullException("toType");

            if (Container.CanResolve(fromType))
            {
                Logger.Log(string.Format(CultureInfo.CurrentCulture, Properties.Resources.TypeMappingAlreadyRegistered, fromType.Name), Category.Debug, Priority.Low);
            }
            else
            {
                if (registerAsSingleton)
                    Container.Register(fromType, toType).AsSingleton();
                else
                    Container.Register(fromType, toType);
            }
        }
    }
}