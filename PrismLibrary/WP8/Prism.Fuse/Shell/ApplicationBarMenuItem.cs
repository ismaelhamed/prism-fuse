using System;
using System.ComponentModel;
using System.Windows;
using System.Windows.Input;
using Microsoft.Phone.Shell;

namespace Microsoft.Practices.Prism.Shell
{
    public class ApplicationBarMenuItem : DependencyObject, IApplicationBarMenuItem
    {
        // Fields
        protected IApplicationBar nativeBar;
        protected IApplicationBarMenuItem nativeMenuItem;

        #region Properties

        /// <summary>
        /// Gets the internal item.
        /// </summary>
        internal IApplicationBarMenuItem NativeItem
        {
            get { return nativeMenuItem; }
        }

        /// <summary>
        /// Gets or sets the parent <see cref="Shell.ApplicationBar"/>.
        /// </summary>
        internal ApplicationBar Parent { get; set; }

        #endregion

        #region Constructors

        static ApplicationBarMenuItem()
        {
            ApplicationBarMenuItem.IsEnabledProperty = DependencyProperty.Register("IsEnabled", typeof(bool), typeof(ApplicationBarMenuItem), new PropertyMetadata(true, ApplicationBarMenuItem.IsEnabledChanged));
            ApplicationBarMenuItem.IsVisibleProperty = DependencyProperty.Register("IsVisible", typeof(bool), typeof(ApplicationBarMenuItem), new PropertyMetadata(true, ApplicationBarMenuItem.IsVisibleChanged));
            ApplicationBarMenuItem.TextProperty = DependencyProperty.Register("Text", typeof(string), typeof(ApplicationBarMenuItem), new PropertyMetadata(ApplicationBarMenuItem.TextChanged));
            ApplicationBarMenuItem.CommandProperty = DependencyProperty.Register("Command", typeof(ICommand), typeof(ApplicationBarMenuItem), new PropertyMetadata(default(ICommand), ApplicationBarMenuItem.CommandChanged));
            ApplicationBarMenuItem.CommandParameterProperty = DependencyProperty.Register("CommandParameter", typeof(object), typeof(ApplicationBarMenuItem), new PropertyMetadata(null, ApplicationBarMenuItem.CommandParameterChanged));
        }

        public ApplicationBarMenuItem()
            : this(new Microsoft.Phone.Shell.ApplicationBarMenuItem())
        { }

        protected ApplicationBarMenuItem(IApplicationBarMenuItem item)
        {
            nativeMenuItem = item;
            nativeMenuItem.Text = "Text";
        }

        #endregion

        #region Dependency Properties

        public static readonly DependencyProperty TextProperty;
        public static readonly DependencyProperty IsEnabledProperty;
        public static readonly DependencyProperty IsVisibleProperty;
        public static readonly DependencyProperty CommandProperty;
        public static readonly DependencyProperty CommandParameterProperty;

        /// <summary>
        /// Gets or sets the Command property. This dependency property indicates the command to execute when the button gets clicked.
        /// </summary>
        public ICommand Command
        {
            get { return (ICommand)GetValue(ApplicationBarMenuItem.CommandProperty); }
            set { SetValue(ApplicationBarMenuItem.CommandProperty, value); }
        }

        /// <summary>
        /// Occurs when the ApplicationBarMenuItem command changes.
        /// </summary>
        private static void CommandChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var target = d as ApplicationBarMenuItem;
            if (target == null)
                return;

            var oldCommand = e.OldValue as ICommand;
            if (oldCommand != null)
            {
                oldCommand.CanExecuteChanged -= target.CanExecuteChanged;
            }

            var newCommand = e.NewValue as ICommand;
            if (newCommand == null)
                return;

            newCommand.CanExecuteChanged += target.CanExecuteChanged;

            // Update ApplicationBarMenuItem.IsEnabled property based on command state
            target.IsEnabled = newCommand.CanExecute(target.CommandParameter);
        }

        /// <summary>
        /// Gets or sets the CommandParameter property. This dependency property indicates the parameter to be passed to the Command when the 
        /// button gets pressed.
        /// </summary>
        public object CommandParameter
        {
            get { return GetValue(ApplicationBarMenuItem.CommandParameterProperty); }
            set { SetValue(ApplicationBarMenuItem.CommandParameterProperty, value); }
        }

        /// <summary>
        /// Handles changes to the CommandParameter property.
        /// </summary>
        private static void CommandParameterChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var target = d as ApplicationBarMenuItem;
            if (target == null)
                return;

            var command = target.Command;
            // FIXME: if (target.IsEnabled && command != null)
            if (command != null)
            {
                target.IsEnabled = command.CanExecute(e.NewValue);
            }
        }

        /// <summary>
        /// Gets or sets the text string that is displayed as a label for the button. The label is displayed when the user taps the ellipses 
        /// symbol on the Application Bar. 
        /// </summary>
        public string Text
        {
            get { return (string)GetValue(ApplicationBarMenuItem.TextProperty); }
            set { SetValue(ApplicationBarMenuItem.TextProperty, value); }
        }

        /// <summary>
        /// Handles changes to the Text property.
        /// </summary>
        private static void TextChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var target = d as ApplicationBarMenuItem;
            if (target != null)
            {
                target.NativeItem.Text = e.NewValue.ToString();
            }
        }

        /// <summary>
        /// Gets or sets a value indicating whether the associated button is enabled.
        /// </summary>
        public bool IsEnabled
        {
            get { return (bool)GetValue(ApplicationBarMenuItem.IsEnabledProperty); }
            set { SetValue(ApplicationBarMenuItem.IsEnabledProperty, value); }
        }

        /// <summary>
        /// Handles changes to the IsEnabled property.
        /// </summary>
        private static void IsEnabledChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var target = d as ApplicationBarMenuItem;
            if (target != null)
            {
                target.NativeItem.IsEnabled = (bool)e.NewValue;
            }
        }

        /// <summary>
        /// Gets or sets a value indicating whether the item is visible.
        /// </summary>
        public bool IsVisible
        {
            get { return (bool)GetValue(ApplicationBarMenuItem.IsVisibleProperty); }
            set { SetValue(ApplicationBarMenuItem.IsVisibleProperty, value); }
        }

        /// <summary>
        /// Handles changes to the IsVisible property.
        /// </summary>
        private static void IsVisibleChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var button = d as ApplicationBarMenuItem;
            if (button == null)
                return;

            var applicationBar = button.Parent;
            if (applicationBar != null)
            {
                applicationBar.Refresh(button.GetType());
            }
        }

        #endregion

        #region Methods

        /// <summary>
        /// Creates an associated <see cref="ApplicationBarMenuItem"/> and attaches it to the specified application bar.
        /// </summary>
        /// <param name="phoneApplicationBar">The application bar to attach to.</param>
        internal virtual void Attach(IApplicationBar phoneApplicationBar)
        {
            if (DesignerProperties.IsInDesignTool)
                return;

            if (phoneApplicationBar == null)
                return;

            nativeBar = phoneApplicationBar;
            nativeMenuItem.Text = Text;
            nativeMenuItem.IsEnabled = IsEnabled;
            nativeMenuItem.Click += ApplicationBarMenuItemClick;

            nativeBar.MenuItems.Add(nativeMenuItem);
        }

        /// <summary>
        /// Detaches the associated <see cref="ApplicationBarMenuItem"/> from the <see cref="Shell.ApplicationBar"/> and from this instance.
        /// </summary>
        internal virtual void Detach()
        {
            if (nativeBar == null)
                return;

            nativeMenuItem.Click -= ApplicationBarMenuItemClick;
            nativeBar.MenuItems.Remove(nativeMenuItem);

            nativeBar = null;
            nativeMenuItem = null;
        }

        #endregion

        #region Event Handlers

        /// <summary>
        /// Occurs when the user taps a button in the Application Bar.
        /// </summary>
        public event EventHandler Click
        {
            add { nativeMenuItem.Click += value; }
            remove { nativeMenuItem.Click -= value; }
        }

        protected void ApplicationBarMenuItemClick(object sender, EventArgs e)
        {
            var command = Command;
            if (command == null)
                return;

            var canExecute = IsEnabled = command.CanExecute(CommandParameter);
            if (canExecute)
            {
                command.Execute(CommandParameter ?? e);
            }
        }

        private void CanExecuteChanged(object sender, EventArgs e)
        {
            var command = sender as ICommand;
            if (command != null)
            {
                IsEnabled = command.CanExecute(CommandParameter);
            }
        }

        #endregion

    }
}
