using System.Windows;

namespace Microsoft.Practices.Prism.Interactivity.InteractionRequest
{
    public partial class NotificationChildWindow
    {
        ///<summary>
        /// The <see cref="DataTemplate"/> to apply when displaying <see cref="Notification"/> data.
        ///</summary>
        public static readonly DependencyProperty NotificationTemplateProperty =
            DependencyProperty.Register(
                "NotificationTemplate",
                typeof(DataTemplate),
                typeof(NotificationChildWindow),
                new PropertyMetadata(null));

        ///<summary>
        /// The <see cref="DataTemplate"/> to apply when displaying <see cref="Notification"/> data.
        ///</summary>
        public DataTemplate NotificationTemplate
        {
            get { return (DataTemplate)GetValue(NotificationTemplateProperty); }
            set { SetValue(NotificationTemplateProperty, value); }
        }
        
        ///<summary>
        /// The notification <see cref="PopupChildWindow"/> to use when displaying <see cref="Notification"/> messages.
        ///</summary>
        public NotificationChildWindow()
        {
            InitializeComponent();
        }
    }
}
