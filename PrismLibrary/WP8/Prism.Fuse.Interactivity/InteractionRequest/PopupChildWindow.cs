using System;
using System.ComponentModel;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Markup;
using System.Windows.Media;
using System.Windows.Media.Animation;
using Microsoft.Phone.Controls;

namespace Microsoft.Practices.Prism.Interactivity.InteractionRequest
{
    /// <summary>
    /// Provides a window that can be displayed over a parent window and blocks interaction with the parent window.
    /// </summary>
    public class PopupChildWindow : ContentControl
    {
        /// <summary>
        /// The current application page.
        /// </summary>
        private PhoneApplicationPage page;

        /// <summary>
        /// Identifies whether the application bar is visible or not before opening the message box.
        /// </summary>
        private bool shouldRestoreApplicationBar;

        /// <summary>
        /// Get whether the ChildWindowPopup is open.
        /// </summary>
        protected bool IsOpen
        {
            get { return ChildWindowPopup != null && ChildWindowPopup.IsOpen; }
        }

        /// <summary>
        /// The popup used to display the message box.
        /// </summary>
        protected Popup ChildWindowPopup
        {
            get;
            private set;
        }

        /// <summary>
        /// Occurs when the <see cref="T:Microsoft.Practices.Prism.Interactivity.InteractionRequest.PopupChildWindow" /> is closed.
        /// </summary>
        public event EventHandler Closed;

        /// <summary>
        /// Initializes a new instance of the <see cref="T:Microsoft.Practices.Prism.Interactivity.InteractionRequest.PopupChildWindow" /> class.
        /// </summary>
        public PopupChildWindow()
        {
            DefaultStyleKey = typeof(PopupChildWindow);
        }

        /// <summary>
        /// Called when the back key is pressed. This event handler cancels the backward navigation and dismisses the message box.
        /// </summary>
        /// <param name="sender">The event sender.</param>
        /// <param name="args">The event information.</param>
        private void PageBackKeyPress(object sender, CancelEventArgs args)
        {
            args.Cancel = true;
            Close();
        }

        private void PopupChildWindow_LayoutUpdated(object sender, EventArgs args)
        {
            var storyboard = XamlReader.Load(SwivelInStoryboard) as Storyboard;
            if (storyboard != null)
            {
                Projection = new PlaneProjection();
                Storyboard.SetTarget(storyboard, this);

                storyboard.Completed += (s, e) => storyboard.Stop();
                storyboard.Begin();
            }

            LayoutUpdated -= PopupChildWindow_LayoutUpdated;
        }

        /// <summary>
        /// Raises the <see cref="E:Microsoft.Practices.Prism.Interactivity.InteractionRequest.PopupChildWindow.Closed" /> event.
        /// </summary>
        /// <param name="e">The event data.</param>
        protected virtual void OnClosed(EventArgs e)
        {
            var handler = Closed;
            if (handler != null)
            {
                handler(this, e);
            }
        }

        /// <summary>
        /// Opens a <see cref="T:Microsoft.Practices.Prism.Interactivity.InteractionRequest.PopupChildWindow" /> and returns without waiting 
        /// for the <see cref="T:Microsoft.Practices.Prism.Interactivity.InteractionRequest.PopupChildWindow" /> to close.
        /// </summary>
        /// <exception cref="T:System.InvalidOperationException">The child window is already in the visual tree.</exception>
        public void Show()
        {
            if (IsOpen) return;

            LayoutUpdated += PopupChildWindow_LayoutUpdated;

            page = VisualTreeHelpers.GetCurrentPhoneApplicationPage();

            if (page != null)
            {
                if (page.ApplicationBar != null)
                {
                    shouldRestoreApplicationBar = page.ApplicationBar.IsVisible;
                    page.ApplicationBar.IsVisible = false;
                }

                page.BackKeyPress += PageBackKeyPress;
            }

            if (ChildWindowPopup == null)
            {
                ChildWindowPopup = new Popup();
                try
                {
                    var container = new Grid
                    {
                        Width = Application.Current.Host.Content.ActualWidth,
                        Height = Application.Current.Host.Content.ActualHeight - 32.0
                    };
                    container.Children.Add(this);

                    ChildWindowPopup.Child = container;
                }
                catch (ArgumentException)
                {
                    // If the FloatableWindow is already in the visualtree, we cannot set it to be the child of the popup
                    // we are throwing a friendlier exception for this case
                    throw new InvalidOperationException(Properties.Resources.ChildWindowInvalidOperation);
                }
            }

            if (ChildWindowPopup != null)
            {
                ChildWindowPopup.IsOpen = true;
                ChildWindowPopup.HorizontalOffset = 0.0;
                ChildWindowPopup.VerticalOffset = 32.0;
            }
        }

        /// <summary>
        /// Closes a <see cref="T:Microsoft.Practices.Prism.Interactivity.InteractionRequest.PopupChildWindow" />.
        /// </summary>
        public void Close()
        {
            if (!IsOpen) return;

            var storyboard = XamlReader.Load(SwivelOutStoryboard) as Storyboard;
            if (storyboard != null)
            {
                Storyboard.SetTarget(storyboard, this);

                storyboard.Completed += (s, e) =>
                {
                    storyboard.Stop();

                    ChildWindowPopup.IsOpen = false;

                    OnClosed(EventArgs.Empty);

                    if (page != null)
                    {
                        if (page.ApplicationBar != null)
                            page.ApplicationBar.IsVisible = shouldRestoreApplicationBar;

                        page.BackKeyPress -= PageBackKeyPress;
                        page = null;
                    }
                };
                storyboard.Begin();
            }
        }

        private const string SwivelInStoryboard =
          @"<Storyboard xmlns=""http://schemas.microsoft.com/winfx/2006/xaml/presentation"">
                <DoubleAnimationUsingKeyFrames Storyboard.TargetProperty=""(UIElement.Projection).(PlaneProjection.RotationX)"">
                    <EasingDoubleKeyFrame KeyTime=""0"" Value=""-45"" />                    
                    <EasingDoubleKeyFrame KeyTime=""0:0:0.2"" Value=""-30"" />
                    <EasingDoubleKeyFrame KeyTime=""0:0:0.35"" Value=""0"">
                        <EasingDoubleKeyFrame.EasingFunction>
                            <ExponentialEase EasingMode=""EaseOut"" Exponent=""6"" />
                        </EasingDoubleKeyFrame.EasingFunction>
                    </EasingDoubleKeyFrame>
                </DoubleAnimationUsingKeyFrames>
                <DoubleAnimationUsingKeyFrames Storyboard.TargetProperty=""(UIElement.Opacity)"">
                    <EasingDoubleKeyFrame KeyTime=""0"" Value=""0"" />
                    <EasingDoubleKeyFrame KeyTime=""0:0:0.20"" Value=""0"" />
                    <EasingDoubleKeyFrame KeyTime=""0:0:0.21"" Value=""1"" />
                </DoubleAnimationUsingKeyFrames>
            </Storyboard>";

        private const string SwivelOutStoryboard =
          @"<Storyboard xmlns=""http://schemas.microsoft.com/winfx/2006/xaml/presentation"">
                <DoubleAnimationUsingKeyFrames Storyboard.TargetProperty=""(UIElement.Projection).(PlaneProjection.RotationX)"">
                    <EasingDoubleKeyFrame KeyTime=""0"" Value=""0""/>
                    <EasingDoubleKeyFrame KeyTime=""0:0:0.25"" Value=""30"">
                        <EasingDoubleKeyFrame.EasingFunction>
                            <ExponentialEase EasingMode=""EaseIn"" Exponent=""6""/>
                        </EasingDoubleKeyFrame.EasingFunction>
                    </EasingDoubleKeyFrame>
                </DoubleAnimationUsingKeyFrames>
                <DoubleAnimationUsingKeyFrames Storyboard.TargetProperty=""(UIElement.Opacity)"">
                    <EasingDoubleKeyFrame KeyTime=""0"" Value=""1"" />
                    <EasingDoubleKeyFrame KeyTime=""0:0:0.24"" Value=""1"" />
                    <EasingDoubleKeyFrame KeyTime=""0:0:0.25"" Value=""0"" />
                </DoubleAnimationUsingKeyFrames>
            </Storyboard>";
    }
}
