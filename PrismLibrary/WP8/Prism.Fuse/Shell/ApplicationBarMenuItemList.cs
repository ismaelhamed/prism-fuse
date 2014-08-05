using System.Collections.Specialized;
using System.Linq;

namespace Microsoft.Practices.Prism.Shell
{
    public class ApplicationBarMenuItemList : ApplicationBarItemList<ApplicationBarMenuItem>
    {
        public ApplicationBarMenuItemList(ApplicationBar applicationBar, int maxItems)
            : base(applicationBar, maxItems)
        { }

        protected override void OnCollectionChanged(object sender, NotifyCollectionChangedEventArgs e)
        {
            switch (e.Action)
            {
                case NotifyCollectionChangedAction.Add:
                    foreach (var item in e.NewItems.OfType<ApplicationBarMenuItem>().Where(c => c.IsVisible).Take(MaxItems))
                    {
                        item.Parent = ApplicationBar;
                        item.Attach(ApplicationBar.InternalApplicationBar);
                    }
                    break;
                case NotifyCollectionChangedAction.Reset:
                    foreach (var menuItem in ApplicationBar.MenuItems)
                        menuItem.Detach();
                    break;
            }
        }
    }
}