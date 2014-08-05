using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Markup;
using Microsoft.Phone.Controls;
using Microsoft.Phone.Shell;
using Microsoft.Xaml.Interactivity;

namespace Microsoft.Practices.Prism.Interactivity
{
    [ContentProperty("Items")]
    public class ApplicationBarBehavior : Behavior<FrameworkElement>
    {
        // Fields
        private IApplicationBar storedApplicationBar;

        // ReSharper disable once StaticFieldInGenericType
        public static readonly DependencyProperty ItemsProperty =
            DependencyProperty.RegisterAttached(
                "Items",
                typeof(List<ApplicationBarBehaviorItem>),
                typeof(ApplicationBarBehavior),
                new PropertyMetadata(new List<ApplicationBarBehaviorItem>()));

        public static List<ApplicationBarBehaviorItem> GetItems(DependencyObject obj)
        {
            return (List<ApplicationBarBehaviorItem>)obj.GetValue(ItemsProperty);
        }

        public static void SetItems(DependencyObject obj, List<ApplicationBarBehaviorItem> value)
        {
            obj.SetValue(ItemsProperty, value);
        }

        // ReSharper disable once StaticFieldInGenericType
        public readonly static DependencyProperty BindingProperty =
            DependencyProperty.Register(
                "Binding",
                typeof(object),
                typeof(ApplicationBarBehavior),
                new PropertyMetadata(ApplicationBarBehavior.OnBindingChanged));

        /// <summary>
        /// Gets or sets the binding that produces the property value of the data object. This is a dependency property.
        /// </summary>
        public object Binding
        {
            get { return GetValue(ApplicationBarBehavior.BindingProperty); }
            set { SetValue(ApplicationBarBehavior.BindingProperty, value); }
        }

        private static void OnBindingChanged(DependencyObject obj, DependencyPropertyChangedEventArgs args)
        {
            if (args.OldValue != null)
            {
                ((ApplicationBarBehavior)obj).UpdateApplicationBar(args.NewValue);
            }
        }

        /// <summary>
        /// Called after the behavior is attached to an AssociatedObject.
        /// </summary>
        /// <remarks>Override this to hook up functionality to the AssociatedObject.</remarks>
        protected override void OnAttached()
        {
            base.OnAttached();
            AssociatedObject.Loaded += (o, e) => SetApplicationBar();
        }

        /// <summary>
        /// Called when the behavior is being detached from its AssociatedObject, but before it has actually occurred.
        /// </summary>
        /// <remarks>Override this to unhook functionality from the AssociatedObject.</remarks>
        protected override void OnDetaching()
        {
            base.OnDetaching();
            AssociatedObject.Unloaded += (o, e) => Interaction.GetBehaviors(AssociatedObject).Remove(this);
        }

        protected virtual void SetApplicationBar()
        {
            var page = VisualTreeHelpers.FindVisualAncestor<PhoneApplicationPage>(AssociatedObject);
            if (page != null)
            {
                // Cache default ApplicationBar so that we can restore it later.
                storedApplicationBar = page.ApplicationBar;
            }
        }

        protected void UpdateApplicationBar(object value)
        {
            if (DesignerProperties.IsInDesignTool)
                return;

            // Get the current Microsoft.Phone.Controls.PhoneApplicationPage
            var page = VisualTreeHelpers.FindVisualAncestor<PhoneApplicationPage>(AssociatedObject);
            if (page == null)
            {
                return;
            }

            ApplicationBarBehaviorItem item = null;

            var items = ApplicationBarBehavior.GetItems(this);

            if (value is bool)
                item = items.FirstOrDefault(i => i.Index.Equals(Convert.ToByte(value)));
            else if (value is byte)
                item = items.FirstOrDefault(i => i.Index.Equals(Convert.ToByte(value)));
            else if (value is short)
                item = items.FirstOrDefault(i => i.Index.Equals(Convert.ToInt16(value)));
            else if (value is int)
                item = items.FirstOrDefault(i => i.Index.Equals(Convert.ToInt32(value)));
            else if (value is long)
                item = items.FirstOrDefault(i => i.Index.Equals(Convert.ToInt32(value)));
            else if (value is Enum)
                item = items.FirstOrDefault(i => i.Index.Equals(Convert.ToInt32(value)));

            IApplicationBar applicationBar = null;

            // Try to get the ApplicationBar {x:Name} based on a value.
            if (item != null)
            {
                if (item.ResourceName == null)
                {
                    // Restore default ApplicationBar for the current page
                    page.ApplicationBar = storedApplicationBar;
                    return;
                }

                // Look in both, Local and Global resources
                var res = Application.Current.Resources[item.ResourceName] ?? page.Resources[item.ResourceName];

                var bar = res as Microsoft.Practices.Prism.Shell.ApplicationBar;
                if (bar != null)
                {
                    applicationBar = bar.InternalApplicationBar;
                }
                else
                {
                    applicationBar = (IApplicationBar)res;
                }
            }

            page.ApplicationBar = applicationBar;
        }
    }
}


