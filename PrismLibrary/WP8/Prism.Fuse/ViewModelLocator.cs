// THIS CODE AND INFORMATION IS PROVIDED "AS IS" WITHOUT WARRANTY OF
// ANY KIND, EITHER EXPRESSED OR IMPLIED, INCLUDING BUT NOT LIMITED TO
// THE IMPLIED WARRANTIES OF MERCHANTABILITY AND/OR FITNESS FOR A
// PARTICULAR PURPOSE.
//
// Copyright (c) Microsoft Corporation. All rights reserved

#if NETFX_CORE
using Windows.UI.Xaml;
#else
using System.Windows;
#endif

namespace Microsoft.Practices.Prism
{
    /// <summary>
    /// The ViewModelLocator class locates the viemodel for the view that has the AutoWireViewModelChanged attached property set to true.
    /// The ViewModel will be located and injected into the view's datacontext. To locate the view, two strategies are used: First the viewmodel locator
    /// will look if there is any viewmodel factory registered for that view, if not it will try to infer the viewmodel using a convention based approach.
    /// This class also provide methods for registering the viewmodel factories, and also to override the default viewmodel factory and the default view 
    /// type to VM type resolver.
    /// </summary>
    public static class ViewModelLocator
    {
        /// <summary>
        /// The auto wire view model attached property
        /// </summary>
        public static DependencyProperty AutoWireViewModelProperty =
            DependencyProperty.RegisterAttached(
                "AutoWireViewModel",
                typeof(bool),
                typeof(ViewModelLocator),
                new PropertyMetadata(false, OnAutoWireViewModelChanged));

        /// <summary>
        /// Gets the value of the AutoWireViewModel attached property.
        /// </summary>
        /// <param name="obj">The dependency object which has this attached property.</param>
        /// <returns><c>True</c> if ViewModel Autowiring is enabled; otherwise, false.</returns>
        public static bool GetAutoWireViewModel(DependencyObject obj)
        {
            if (obj != null)
            {
                return (bool)obj.GetValue(AutoWireViewModelProperty);
            }

            return false;
        }

        /// <summary>
        /// Sets the value of the AutoWireViewModel attached property.
        /// </summary>
        /// <param name="obj">The dependency object which has this attached property.</param>
        /// <param name="value">if set to <c>true</c> the view model wiring will be performed.</param>
        public static void SetAutoWireViewModel(DependencyObject obj, bool value)
        {
            if (obj != null)
            {
                obj.SetValue(AutoWireViewModelProperty, value);
            }
        }

        /// <summary>
        /// Automatically looks up the viewmodel that correspond to the current view, using two strategies:
        /// the first one looks if there is a any mapping registered for that view, if not it will fallback to the convention based approach.
        /// </summary>
        /// <param name="d">The dependency object, typically a view.</param>
        /// <param name="e">The <see cref="DependencyPropertyChangedEventArgs"/> instance containing the event data.</param>
        private static void OnAutoWireViewModelChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            ViewModelLocatorProvider.AutoWireViewModelChanged(d, (view, viewModel) =>
            {
                var element = view as FrameworkElement;
                if (element != null)
                {
                    element.DataContext = viewModel;
                }
            });
        }
    }
}
