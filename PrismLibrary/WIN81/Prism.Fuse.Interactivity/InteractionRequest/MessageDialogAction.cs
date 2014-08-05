using System;
using Microsoft.Xaml.Interactivity;
using Windows.UI.Popups;
using Windows.UI.Xaml;

namespace Microsoft.Practices.Prism.Interactivity.InteractionRequest
{
    /// <summary>
    /// Displays a message dialog with the content of the <see cref="InteractionRequestedEventArgs"/> as the item.
    /// </summary>
    public class MessageDialogAction : DependencyObject, IAction
    {
        private bool isDisplaying;

        /// <summary>
        /// Executes the action.
        /// </summary>
        /// <param name="sender">The object that is passed to the action by the behavior. Generally this is AssociatedObject or the target object.</param>
        /// <param name="parameter">The value of this parameter is determined by the caller.</param>
        /// <returns>true if updating the property value succeeds; otherwise, false.</returns>
        public object Execute(object sender, object parameter)
        {
            if (isDisplaying)
                return false;

            var requestedEventArgs = parameter as InteractionRequestedEventArgs;
            if (requestedEventArgs == null)
                return false;

            var dialog = new MessageDialog((string)requestedEventArgs.Context.Content, requestedEventArgs.Context.Title);

            var notification = requestedEventArgs.Context;
            if (notification is Confirmation)
            {
                dialog.Commands.Add(new UICommand("OK", command => ((Confirmation)requestedEventArgs.Context).Confirmed = true));
                dialog.Commands.Add(new UICommand("Cancel"));

                // Set the command that will be invoked by default
                dialog.DefaultCommandIndex = 1;

                // Set the command to be invoked when escape is pressed
                dialog.CancelCommandIndex = 1;
            }

            dialog.ShowAsync().AsTask().ContinueWith(c =>
            {
                isDisplaying = false;

                if (requestedEventArgs.Callback != null)
                    requestedEventArgs.Callback.Invoke();
            });

            return true;
        }
    }
}
