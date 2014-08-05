using System.Windows;

namespace Microsoft.Practices.Prism.Interactivity
{
    public class ApplicationBarBehaviorItem : DependencyObject
    {
        /// <summary>
        /// Identifier for the <see cref="Index" /> dependency property.
        /// </summary>
        public static readonly DependencyProperty IndexProperty =
            DependencyProperty.Register(
                "Index",
                typeof(int),
                typeof(ApplicationBarBehaviorItem),
                new PropertyMetadata(-1));

        public int Index
        {
            get { return (int)GetValue(IndexProperty); }
            set { SetValue(IndexProperty, value); }
        }

        /// <summary>
        /// Identifier for the <see cref="ResourceName" /> dependency property.
        /// </summary>
        public static readonly DependencyProperty ResourceNameProperty =
            DependencyProperty.Register(
                "ResourceName",
                typeof(string),
                typeof(ApplicationBarBehaviorItem),
                new PropertyMetadata(null));
        
        public string ResourceName
        {
            get { return (string)GetValue(ResourceNameProperty); }
            set { SetValue(ResourceNameProperty, value); }
        }
    }
}