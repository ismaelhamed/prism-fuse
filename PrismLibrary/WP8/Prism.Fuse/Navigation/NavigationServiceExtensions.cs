using Microsoft.Phone.Controls;

namespace Microsoft.Practices.Prism.Navigation
{
    public static class NavigationServiceExtensions
    {
        /// <summary>
        /// Causes the Frame to load content represented by the specified Page-derived data type, also passing a parameter to be interpreted by the target of the navigation.
        /// </summary>
        /// <param name="target">Instance of the NavigationService.</param>
        /// <param name="parameters">The navigation parameters to pass to the target page. May be null.</param>
        /// <param name="removeCurrentViewFromBackStack">Specifies whether the current view should be removed from the BackStack. Default is false.</param>
        public static bool Navigate<TView>(this INavigationService target, NavigationParameters parameters = null, bool removeCurrentViewFromBackStack = false)
            where TView : PhoneApplicationPage
        {
            return target.Navigate(typeof(TView), parameters, removeCurrentViewFromBackStack);
        }
    }
}
