//===================================================================================
// Microsoft patterns & practices
// Composite Application Guidance for Windows Presentation Foundation and Silverlight
//===================================================================================
// Copyright (c) Microsoft Corporation.  All rights reserved.
// THIS CODE AND INFORMATION IS PROVIDED "AS IS" WITHOUT WARRANTY
// OF ANY KIND, EITHER EXPRESSED OR IMPLIED, INCLUDING BUT NOT
// LIMITED TO THE IMPLIED WARRANTIES OF MERCHANTABILITY AND
// FITNESS FOR A PARTICULAR PURPOSE.
//===================================================================================
// The example companies, organizations, products, domain names,
// e-mail addresses, logos, people, places, and events depicted
// herein are fictitious.  No association with any real company,
// organization, product, domain name, email address, logo, person,
// places, or events is intended or should be inferred.
//===================================================================================
using System.Windows;

namespace Microsoft.Practices.Prism.Interactivity.InteractionRequest
{
    /// <summary>
    /// Concrete class that pops up a specified child window or a default child window configured with a data template.
    /// </summary>
    public class PopupChildWindowAction : PopupChildWindowActionBase
    {
        /// <summary>
        /// The child window to display as part of the popup.
        /// </summary>
        public static readonly DependencyProperty ChildWindowProperty =
            DependencyProperty.Register(
                "ChildWindow",
                typeof(PopupChildWindow),
                typeof(PopupChildWindowAction),
                new PropertyMetadata(null));

        /// <summary>
        /// Gets or sets the child window to pop up.
        /// </summary>
        /// <remarks>
        /// If not specified, a default child window is used instead.
        /// </remarks>
        public PopupChildWindow ChildWindow
        {
            get { return (PopupChildWindow)GetValue(ChildWindowProperty); }
            set { SetValue(ChildWindowProperty, value); }
        }

        /// <summary>
        /// The <see cref="DataTemplate"/> to apply to the popup content.
        /// </summary>
        public static readonly DependencyProperty ContentTemplateProperty =
            DependencyProperty.Register(
                "ContentTemplate",
                typeof(DataTemplate),
                typeof(PopupChildWindowAction),
                new PropertyMetadata(null));

        /// <summary>
        /// Gets or sets the content template for a default child window.
        /// </summary>
        public DataTemplate ContentTemplate
        {
            get { return (DataTemplate)GetValue(ContentTemplateProperty); }
            set { SetValue(ContentTemplateProperty, value); }
        }

        /// <summary>
        /// Identifies the IsFullScreen dependency property.
        /// </summary>
        public static readonly DependencyProperty IsFullScreenProperty =
            DependencyProperty.Register(
                "IsFullScreen",
                typeof(bool),
                typeof(PopupChildWindowAction),
                new PropertyMetadata(null));

        /// <summary>
        /// Gets or sets whether the message box occupies the whole screen.
        /// </summary>
        public bool IsFullScreen
        {
            get { return (bool)GetValue(IsFullScreenProperty); }
            set { SetValue(IsFullScreenProperty, value); }
        }

        /// <summary>
        /// Returns the child window to display as part of the trigger action.
        /// </summary>
        /// <param name="notification">The notification to display in the child window.</param>
        /// <returns></returns>
        protected override PopupChildWindow GetChildWindow(Notification notification)
        {
            var childWindow = ChildWindow ?? CreateDefaultWindow(notification);
            childWindow.VerticalAlignment = IsFullScreen ? VerticalAlignment.Stretch : VerticalAlignment.Top;
            childWindow.DataContext = notification;

            return childWindow;
        }

        private PopupChildWindow CreateDefaultWindow(Notification notification)
        {
            return notification is Confirmation
                ? (PopupChildWindow)new ConfirmationChildWindow { ConfirmationTemplate = ContentTemplate }
                : new NotificationChildWindow { NotificationTemplate = ContentTemplate };
        }
    }
}