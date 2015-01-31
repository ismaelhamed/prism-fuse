using System.Windows;

namespace Microsoft.Practices.Prism
{
    public sealed class NameScopeBinding : DependencyObject
    {
        /// <summary>
        /// Backing storage for Element property
        /// </summary>
        public static readonly DependencyProperty SourceProperty =
            DependencyProperty.Register(
                "Source",
                typeof(FrameworkElement), 
                typeof(NameScopeBinding), 
                new PropertyMetadata(null));

        /// <summary>
        /// Source to bind to and make available as a resource
        /// </summary>
        public object Source
        {
            get { return GetValue(SourceProperty); }
            set { SetValue(SourceProperty, value); }
        }
    }
}
