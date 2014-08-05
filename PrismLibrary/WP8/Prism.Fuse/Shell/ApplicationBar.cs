using System;
using System.Linq;
using System.Windows;
using System.Windows.Input;
using System.Windows.Markup;
using System.Windows.Media;
using Microsoft.Phone.Shell;

namespace Microsoft.Practices.Prism.Shell
{
    /// <summary>
    /// Represents an Application Bar in Windows Phone applications.
    /// </summary>
    [ContentProperty("Buttons")]
    public sealed class ApplicationBar : DependencyObject
    {
        // Fields
        private const int maxMenuItems = 50;
        private const int maxIconButtons = 4;
        private readonly IApplicationBar applicationBar;

        #region Properties

        internal Microsoft.Phone.Shell.IApplicationBar InternalApplicationBar
        {
            get { return this.applicationBar; }
        }

        /// <summary>
        /// Gets the distance that the Application Bar extends into a page when the <see cref="P:Microsoft.Practices.Prism.Shell.ApplicationBar.Mode" /> 
        /// property is set to <see cref="F:Microsoft.Phone.Shell.ApplicationBarMode.Default" />.
        /// </summary>
        public double DefaultSize
        {
            get { return applicationBar.DefaultSize; }
        }

        /// <summary>
        /// Gets the distance that the Application Bar extends into a page when the <see cref="P:Microsoft.Phone.Shell.IApplicationBar.Mode" /> 
        /// property is set to <see cref="F:Microsoft.Phone.Shell.ApplicationBarMode.Minimized" />.
        /// </summary>
        public double MiniSize
        {
            get { return applicationBar.MiniSize; }
        }

        #endregion

        #region Constructors

        static ApplicationBar()
        {
            ApplicationBar.ButtonsProperty = DependencyProperty.Register("Buttons", typeof(ApplicationBarIconButtonList), typeof(ApplicationBar), null);
            ApplicationBar.MenuItemsProperty = DependencyProperty.Register("MenuItems", typeof(ApplicationBarMenuItemList), typeof(ApplicationBar), null);
            ApplicationBar.BackgroundColorProperty = DependencyProperty.Register("BackgroundColor", typeof(Color), typeof(ApplicationBar), new PropertyMetadata(ApplicationBar.BackgroundColorChanged));
            ApplicationBar.ForegroundColorProperty = DependencyProperty.Register("ForegroundColor", typeof(Color), typeof(ApplicationBar), new PropertyMetadata(ApplicationBar.ForegroundColorChanged));
            ApplicationBar.OpacityProperty = DependencyProperty.Register("Opacity", typeof(double), typeof(ApplicationBar), new PropertyMetadata(1.0, ApplicationBar.OpacityChanged));
            ApplicationBar.IsMenuEnabledProperty = DependencyProperty.Register("IsMenuEnabled", typeof(bool), typeof(ApplicationBar), new PropertyMetadata(true, ApplicationBar.IsMenuEnabledChanged));
            ApplicationBar.IsVisibleProperty = DependencyProperty.RegisterAttached("IsVisible", typeof(bool), typeof(ApplicationBar), new PropertyMetadata(true, ApplicationBar.IsVisibleChanged));
            ApplicationBar.ModeProperty = DependencyProperty.RegisterAttached("Mode", typeof(ApplicationBarMode), typeof(ApplicationBar), new PropertyMetadata(ApplicationBarMode.Default, ApplicationBar.ModeChanged));
            ApplicationBar.StateChangedCommandProperty = DependencyProperty.Register("StateChangedCommand", typeof(ICommand), typeof(ApplicationBar), null);
        }

        /// <summary>
        /// Creates a new instance of the <see cref="T:Microsoft.Practices.Prism.Shell.ApplicationBar" /> class.
        /// </summary>
        public ApplicationBar()
        {
            applicationBar = new Microsoft.Phone.Shell.ApplicationBar();

            Buttons = new ApplicationBarIconButtonList(this, maxIconButtons);
            MenuItems = new ApplicationBarMenuItemList(this, maxMenuItems);

            // Prevents flickering when hiding/showing the application bar.
            Opacity = 0.99;
        }

        #endregion

        #region Dependency Properties

        public static readonly DependencyProperty ButtonsProperty;
        public static readonly DependencyProperty MenuItemsProperty;
        public static readonly DependencyProperty BackgroundColorProperty;
        public static readonly DependencyProperty ForegroundColorProperty;
        public static readonly DependencyProperty IsMenuEnabledProperty;
        public static readonly DependencyProperty IsVisibleProperty;
        public static readonly DependencyProperty ModeProperty;
        public static readonly DependencyProperty StateChangedCommandProperty;
        public static readonly DependencyProperty OpacityProperty;

        /// <summary>
        /// Gets the list of the buttons that appear on the Application Bar.
        /// </summary>
        public ApplicationBarIconButtonList Buttons
        {
            get { return (ApplicationBarIconButtonList)GetValue(ButtonsProperty); }
            set { SetValue(ButtonsProperty, value); }
        }

        /// <summary>
        /// Gets the list of the menu items that appear on the Application Bar.
        /// </summary>
        public ApplicationBarMenuItemList MenuItems
        {
            get { return (ApplicationBarMenuItemList)GetValue(MenuItemsProperty); }
            set { SetValue(MenuItemsProperty, value); }
        }

        /// <summary>
        /// Gets or sets the background color of the Application Bar
        /// </summary>
        public Color BackgroundColor
        {
            get { return (Color)GetValue(BackgroundColorProperty); }
            set { SetValue(BackgroundColorProperty, value); }
        }

        /// <summary>
        /// Called after the background color of the ApplicationBar is changed.
        /// </summary>
        /// <param name="d">The <see cref="DependencyObject" />.</param>
        /// <param name="e">The <see cref="DependencyPropertyChangedEventArgs" /> instance containing the event data.</param>
        private static void BackgroundColorChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var target = d as ApplicationBar;
            if (target != null)
            {
                target.InternalApplicationBar.BackgroundColor = (Color)e.NewValue;
            }
        }

        /// <summary>
        /// Gets or sets the foreground color of the Application Bar.
        /// </summary>
        public Color ForegroundColor
        {
            get { return (Color)GetValue(ForegroundColorProperty); }
            set { SetValue(ForegroundColorProperty, value); }
        }

        /// <summary>
        /// Called after the foreground color of the ApplicationBar is changed.
        /// </summary>
        /// <param name="d">The <see cref="DependencyObject" />.</param>
        /// <param name="e">The <see cref="DependencyPropertyChangedEventArgs" /> instance containing the event data.</param>
        private static void ForegroundColorChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var target = d as ApplicationBar;
            if (target != null)
            {
                target.InternalApplicationBar.ForegroundColor = (Color)e.NewValue;
            }
        }

        /// <summary>
        /// Gets or sets the opacity of the Application Bar.
        /// </summary>
        public double Opacity
        {
            get { return (double)GetValue(OpacityProperty); }
            set { SetValue(OpacityProperty, value); }
        }

        /// <summary>
        /// Called after the opacity of the ApplicationBar is changed.
        /// </summary>
        /// <param name="d">The <see cref="DependencyObject" />.</param>
        /// <param name="e">The <see cref="DependencyPropertyChangedEventArgs" /> instance containing the event data.</param>
        private static void OpacityChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var target = d as ApplicationBar;
            if (target != null)
            {
                target.InternalApplicationBar.Opacity = (double)e.NewValue;
            }
        }

        /// <summary>
        /// Gets or sets a value indicating whether the user can open the menu.
        /// </summary>
        public bool IsMenuEnabled
        {
            get { return (bool)GetValue(IsMenuEnabledProperty); }
            set { SetValue(IsMenuEnabledProperty, value); }
        }

        /// <summary>
        /// Called after the menu enabled state of the Application Bar is changed.
        /// </summary>
        /// <param name="d">The <see cref="DependencyObject" />.</param>
        /// <param name="e">The <see cref="DependencyPropertyChangedEventArgs" /> instance containing the event data.</param>
        private static void IsMenuEnabledChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var target = d as ApplicationBar;
            if (target != null)
            {
                target.InternalApplicationBar.IsMenuEnabled = (bool)e.NewValue;
            }
        }

        /// <summary>
        /// Gets or sets a value indicating whether the Application Bar is visible.
        /// </summary>
        public bool IsVisible
        {
            get { return (bool)GetValue(IsVisibleProperty); }
            set { SetValue(IsVisibleProperty, value); }
        }

        /// <summary>
        /// Called after the visible state of the Application Bar is changed.
        /// </summary>
        /// <param name="d">The <see cref="DependencyObject" />.</param>
        /// <param name="e">The <see cref="DependencyPropertyChangedEventArgs" /> instance containing the event data.</param>
        private static void IsVisibleChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var target = d as ApplicationBar;
            if (target != null)
            {
                target.InternalApplicationBar.IsVisible = (bool)e.NewValue;
            }
        }

        /// <summary>
        /// Gets or sets the size of the Application Bar.
        /// </summary>
        public ApplicationBarMode Mode
        {
            get { return (ApplicationBarMode)GetValue(ModeProperty); }
            set { SetValue(ModeProperty, value); }
        }

        /// <summary>
        /// Called after the size of the ApplicationBar is changed.
        /// </summary>
        /// <param name="d">The <see cref="DependencyObject" />.</param>
        /// <param name="e">The <see cref="DependencyPropertyChangedEventArgs" /> instance containing the event data.</param>
        private static void ModeChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var target = d as ApplicationBar;
            if (target != null)
            {
                target.InternalApplicationBar.Mode = (ApplicationBarMode)e.NewValue;
            }
        }

        /// <summary>
        /// Gets or sets the command to invoke when the user opens or closes the menu. This is a DependencyProperty.
        /// </summary>
        public ICommand StateChangedCommand
        {
            get { return (ICommand)GetValue(StateChangedCommandProperty); }
            set { SetValue(StateChangedCommandProperty, value); }
        }

        #endregion

        #region Event Handlers

        /// <summary>
        /// Occurs when the user opens or closes the menu.
        /// </summary>
        public event EventHandler<ApplicationBarStateChangedEventArgs> StateChanged
        {
            add { applicationBar.StateChanged += value; }
            remove { applicationBar.StateChanged -= value; }
        }

        #endregion

        #region Methods

        /// <summary>
        /// Forces the control to invalidate its client area and immediately redraw itself and any child controls.
        /// </summary>
        /// <param name="type">Type to be used as the discriminator</param>
        internal void Refresh(Type type)
        {
            if (type == typeof(ApplicationBarIconButton))
            {
                foreach (var button in Buttons)
                    button.Detach();

                foreach (var button in Buttons.Where(c => c.IsVisible).Take(maxIconButtons))
                {
                    button.Parent = this;
                    button.Attach(applicationBar);
                }
            }
            else if (type == typeof(ApplicationBarMenuItem))
            {
                foreach (var menuItem in MenuItems)
                    menuItem.Detach();

                foreach (var menuItem in MenuItems.Where(c => c.IsVisible).Take(maxMenuItems))
                {
                    menuItem.Parent = this;
                    menuItem.Attach(applicationBar);
                }
            }
        }

        #endregion

    }
}
