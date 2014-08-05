// (c) Copyright Microsoft Corporation.
// This source is subject to the Microsoft Public License (Ms-PL).
// Please see http://go.microsoft.com/fwlink/?LinkID=131993 for details.
// All other rights reserved.

using System.Collections.Generic;
using System.Windows.Media;
using Microsoft.Phone.Controls;

namespace System.Windows.Controls
{
    /// <summary>
    /// A static class providing methods for working with the visual tree.  
    /// </summary>
    public static class VisualTreeHelpers
    {
        /// <summary>
        /// The visual ancestor of the framework element of the specified type that is found while traversing the visual tree upwards.
        /// </summary>
        /// <typeparam name="T">The element type of the dependency object.</typeparam>
        /// <param name="element">The dependency object element.</param>
        /// <returns>The first parent of the framework element of the specified type.</returns>
        internal static T FindVisualAncestor<T>(DependencyObject element)
            where T : FrameworkElement
        {
            if (element == null)
                return null;

            var parent = VisualTreeHelper.GetParent(element);

            while (parent != null)
            {
                var result = parent as T;
                if (result != null)
                {
                    return result;
                }

                parent = VisualTreeHelper.GetParent(parent);
            }

            return null;
        }

        /// <summary>
        /// Finds a visual descendant from a given type that is in the hierarchy of a given parent element.
        /// </summary>
        /// <typeparam name="T">The type of the element to look for.</typeparam>
        /// <param name="parent">The parent.</param>
        internal static T FindVisualDescendant<T>(DependencyObject parent)
            where T : DependencyObject
        {
            if (parent == null)
                return null;

            var enumerator = VisualTreeHelpers.GetVisualChildren(parent).GetEnumerator();
            if (enumerator.MoveNext())
            {
                return enumerator.Current as T;
            }

            return null;
        }

        /// <summary>
        /// Retrieves all the visual children of a framework element.
        /// </summary>
        /// <param name="parent">The parent framework element.</param>
        /// <returns>The visual children of the framework element.</returns>
        internal static IEnumerable<DependencyObject> GetVisualChildren(DependencyObject parent)
        {
            var childCount = VisualTreeHelper.GetChildrenCount(parent);

            for (var counter = 0; counter < childCount; counter++)
            {
                yield return VisualTreeHelper.GetChild(parent, counter);
            }
        }

        internal static PhoneApplicationPage GetCurrentPhoneApplicationPage()
        {
            var rootVisual = Application.Current.RootVisual as PhoneApplicationFrame;
            if (rootVisual != null && rootVisual.Content is PhoneApplicationPage)
            {
                return (PhoneApplicationPage)rootVisual.Content;
            }

            return null;
        }
    }
}
