using System.Windows;

namespace Microsoft.Practices.Prism.Interactivity.InteractionRequest
{
    public partial class ConfirmationChildWindow
    {
        ///<summary>
        /// The content template to use when showing <see cref="Confirmation"/> data.
        ///</summary>
        public static readonly DependencyProperty ConfirmationTemplateProperty =
            DependencyProperty.Register(
                "ConfirmationTemplate",
                typeof(DataTemplate),
                typeof(ConfirmationChildWindow),
                new PropertyMetadata(null));

        ///<summary>
        /// The content template to use when showing <see cref="Confirmation"/> data.
        ///</summary>
        public DataTemplate ConfirmationTemplate
        {
            get { return (DataTemplate)GetValue(ConfirmationTemplateProperty); }
            set { SetValue(ConfirmationTemplateProperty, value); }
        }

        public ConfirmationChildWindow()
        {
            InitializeComponent();
        }
    }
}
