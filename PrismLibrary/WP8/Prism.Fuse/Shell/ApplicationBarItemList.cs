using System.Collections.Specialized;
using System.Windows;
using System;

namespace Microsoft.Practices.Prism.Shell
{
    /// <summary>
    /// This class propagates the DataContext from the collection to its members.
    /// </summary>
    public abstract class ApplicationBarItemList<TItem> : DependencyObjectCollection<TItem>
        where TItem : ApplicationBarMenuItem
    {
        protected int MaxItems;
        protected ApplicationBar ApplicationBar;

        protected ApplicationBarItemList(ApplicationBar applicationBar, int maxItems)
        {
            if (applicationBar == null)
                throw new ArgumentNullException("applicationBar");

            MaxItems = maxItems;
            ApplicationBar = applicationBar;
            CollectionChanged += OnCollectionChanged;
        }

        protected virtual void OnCollectionChanged(object sender, NotifyCollectionChangedEventArgs e)
        { }
    }
}
