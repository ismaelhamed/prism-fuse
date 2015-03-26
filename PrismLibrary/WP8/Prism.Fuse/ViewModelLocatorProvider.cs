using System;
using System.Collections.Generic;
using System.Globalization;
#if NETFX_CORE
using System.Reflection;
#endif

namespace Microsoft.Practices.Prism
{
    /// <summary>
    /// The ViewModelLocationProvider class locates the view model for the view that has the AutoWireViewModelChanged attached property set to true.
    /// The view model will be located and injected into the view's DataContext. To locate the view, two strategies are used: First the ViewModelLocationProvider
    /// will look to see if there is a view model factory registered for that view, if not it will try to infer the view model using a convention based approach.
    /// This class also provide methods for registering the view model factories, and also to override the default view model factory and the default 
    /// view type to view model type resolver.
    /// </summary>
    public static class ViewModelLocatorProvider
    {
        /// <summary>
        /// A dictionary that contains all the registered factories for the views.
        /// </summary>
        private static readonly Dictionary<string, Func<object>> factories = new Dictionary<string, Func<object>>();

        /// <summary>
        /// The default view model factory.
        /// </summary>
        private static Func<Type, object> defaultViewModelFactory = type => Activator.CreateInstance(type);

        /// <summary>
        /// Default View Type to VM Type resolver, assumes VM is in same assembly and namespace as View Type.
        /// </summary>
        private static Func<Type, Type> defaultViewTypeToViewModelTypeResolver =
            viewType =>
            {
                var fixedFullName = viewType.FullName.Replace(".Views.", ".ViewModels.");
#if NETFX_CORE
                var viewModelName = string.Format(CultureInfo.InvariantCulture, fixedFullName.EndsWith("View") ? "{0}Model, {1}" : "{0}ViewModel, {1}", fixedFullName, viewType.GetTypeInfo().Assembly.FullName);
#else
                var viewModelName = string.Format(CultureInfo.InvariantCulture, fixedFullName.EndsWith("View") ? "{0}Model, {1}" : "{0}ViewModel, {1}", fixedFullName, viewType.Assembly.FullName);
#endif

                return Type.GetType(viewModelName);
            };
        
        /// <summary>
        /// Sets the default view model factory.
        /// </summary>
        /// <param name="viewModelFactory">The view model factory.</param>
        public static void SetDefaultViewModelFactory(Func<Type, object> viewModelFactory)
        {
            defaultViewModelFactory = viewModelFactory;
        }

        /// <summary>
        /// Sets the default view type to view model type resolver.
        /// </summary>
        /// <param name="viewTypeToViewModelTypeResolver">The view type to view model type resolver.</param>
        public static void SetDefaultViewTypeToViewModelTypeResolver(Func<Type, Type> viewTypeToViewModelTypeResolver)
        {
            defaultViewTypeToViewModelTypeResolver = viewTypeToViewModelTypeResolver;
        }

        /// <summary>
        /// Automatically looks up the viewmodel that corresponds to the current view, using two strategies:
        /// It first looks to see if there is a mapping registered for that view, if not it will fallback to the convention based approach.
        /// </summary>
        /// <param name="view">The dependency object, typically a view.</param>
        /// <param name="setDataContextCallback"></param>
        public static void AutoWireViewModelChanged(object view, Action<object, object> setDataContextCallback)
        {
            var viewModel = GetViewModelForView(view);
            if (viewModel == null)
            {
                var viewModelType = defaultViewTypeToViewModelTypeResolver(view.GetType());
                if (viewModelType == null)
                    return;

                // Really need Container or Factories here to deal with injecting dependencies on construction
                viewModel = defaultViewModelFactory(viewModelType);
            }

            setDataContextCallback(view, viewModel);
        }

        /// <summary>
        /// Gets the view model for the specified view.
        /// </summary>
        /// <param name="view">The view which viewmodel want.</param>
        /// <returns>The viewmodel that correspond to the view passed as a paramenter.</returns>
        private static object GetViewModelForView(object view)
        {
            // Mapping of view models base on view type (or instance) goes here
            return factories.ContainsKey(view.GetType().ToString()) ? factories[view.GetType().ToString()]() : null;
        }

        /// <summary>
        /// Registers the view model factory for the specified view type name.
        /// </summary>
        /// <param name="viewTypeName">The name of the view type.</param>
        /// <param name="factory">The viewmodel factory.</param>
        public static void Register(string viewTypeName, Func<object> factory)
        {
            factories[viewTypeName] = factory;
        }
    }
}
