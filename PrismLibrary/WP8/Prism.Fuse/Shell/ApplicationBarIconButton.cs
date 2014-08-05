using System;
using System.ComponentModel;
using System.Windows;
using Microsoft.Phone.Shell;

namespace Microsoft.Practices.Prism.Shell
{
    /// <summary>
    /// An Application Bar button with an icon.
    /// </summary>
    public class ApplicationBarIconButton : ApplicationBarMenuItem, IApplicationBarIconButton
    {

        #region Constructors

        static ApplicationBarIconButton()
        {
            ApplicationBarIconButton.IconUriProperty = DependencyProperty.Register("IconUri", typeof(Uri), typeof(ApplicationBarIconButton), new PropertyMetadata(ApplicationBarIconButton.IconUriChanged));
        }

        public ApplicationBarIconButton()
            : base(new Microsoft.Phone.Shell.ApplicationBarIconButton(new Uri("", UriKind.Relative)))
        { }

        #endregion

        #region Dependency Properties

        public static readonly DependencyProperty IconUriProperty;

        /// <summary>
        /// Gets or sets the URI of the icon to use for the button.
        /// </summary>
        public Uri IconUri
        {
            get { return (Uri)GetValue(IconUriProperty); }
            set { SetValue(IconUriProperty, value); }
        }

        /// <summary>
        /// Called after the URI of the icon to use for the button is changed.
        /// </summary>
        /// <param name="d">The <see cref="DependencyObject" />.</param>
        /// <param name="e">The <see cref="DependencyPropertyChangedEventArgs" /> instance containing the event data.</param>
        private static void IconUriChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var target = d as ApplicationBarIconButton;
            if (target != null)
            {
                ((IApplicationBarIconButton)target.NativeItem).IconUri = (Uri)e.NewValue;
            }
        }

        #endregion

        #region Overriden Methods

        /// <summary>
        /// Creates an associated <see cref="ApplicationBarIconButton"/> and attaches it to the specified application bar.
        /// </summary>
        /// <param name="phoneApplicationBar">The application bar to attach to.</param>
        internal override void Attach(IApplicationBar phoneApplicationBar)
        {
            if (DesignerProperties.IsInDesignTool)
                return;

            if (phoneApplicationBar == null)
                return;

            nativeBar = phoneApplicationBar;
            nativeMenuItem.Text = Text;
            nativeMenuItem.IsEnabled = IsEnabled;
            nativeMenuItem.Click += ApplicationBarMenuItemClick;
            ((IApplicationBarIconButton)nativeMenuItem).IconUri = IconUri;

            nativeBar.Buttons.Add(nativeMenuItem);
        }

        /// <summary>
        /// Detaches the associated <see cref="ApplicationBarIconButton"/> from the <see cref="ApplicationBar"/> and from this instance.
        /// </summary>
        internal override void Detach()
        {
            if (nativeBar == null)
                return;

            nativeMenuItem.Click -= ApplicationBarMenuItemClick;
            nativeBar.Buttons.Remove(nativeMenuItem);

            nativeBar = null;
            nativeMenuItem = null;
        }

        #endregion

    }
}
