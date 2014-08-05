using Windows.UI.Xaml.Controls;

namespace Microsoft.Practices.Prism.Navigation
{
    public static class NavigationServiceExtensions
    {
        /// <summary>
        /// Causes the Frame to load content represented by the specified Page-derived data type, also passing a parameter to be interpreted by the target of the navigation.
        /// </summary>
        /// <param name="target">Instance of the NavigationService.</param>
        /// <param name="parameter">The navigation parameter to pass to the target page. May be null.</param>
        public static bool Navigate<TView>(this INavigationService target, object parameter = null)
            where TView : Page
        {
            return target.Navigate(typeof(TView), parameter);
        }
    }
}
