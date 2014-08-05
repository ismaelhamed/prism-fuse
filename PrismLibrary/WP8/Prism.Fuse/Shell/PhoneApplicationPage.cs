using System;
using System.Windows;

namespace Microsoft.Practices.Prism.Shell
{
    public static class PhoneApplicationPage
    {
        /// <summary>
        /// The dependency property for <see cref="P:Microsoft.Phone.Controls.PhoneApplicationPage.ApplicationBar" />.
        /// </summary>
        public static readonly DependencyProperty ApplicationBarProperty =
            DependencyProperty.RegisterAttached(
                "ApplicationBar",
                typeof(ApplicationBar),
                typeof(PhoneApplicationPage), 
                new PropertyMetadata(PhoneApplicationPage.ApplicationBarChanged));

        public static ApplicationBar GetApplicationBar(Microsoft.Phone.Controls.PhoneApplicationPage obj)
        {
            return (ApplicationBar)obj.GetValue(PhoneApplicationPage.ApplicationBarProperty);
        }

        public static void SetApplicationBar(Microsoft.Phone.Controls.PhoneApplicationPage obj, ApplicationBar value)
        {
            obj.SetValue(PhoneApplicationPage.ApplicationBarProperty, value);
        }

        /// <summary>
        /// Called after the ApplicationBar is changed.
        /// </summary>
        /// <param name="d">The <see cref="DependencyObject" />.</param>
        /// <param name="e">The <see cref="DependencyPropertyChangedEventArgs" /> instance containing the event data.</param>
        private static void ApplicationBarChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var phoneApplicationPage = d as Microsoft.Phone.Controls.PhoneApplicationPage;
            if (phoneApplicationPage == null)
            {
                throw new ArgumentException("ApplicationBar property needs to be set on a PhoneApplicationPage element.");
            }

            var applicationBar = e.NewValue as ApplicationBar;
            if (applicationBar != null)
            {
                phoneApplicationPage.ApplicationBar = applicationBar.InternalApplicationBar;
            }
        }
    }
}
