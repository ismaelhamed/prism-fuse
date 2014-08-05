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

using System;
using System.Reflection;
using System.Runtime.InteropServices.WindowsRuntime;
using Microsoft.Xaml.Interactivity;
using Windows.ApplicationModel;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Markup;
using Windows.UI.Xaml.Media;

namespace Microsoft.Practices.Prism.Interactivity.InteractionRequest
{
    /// <summary>
    /// Custom event trigger for using with <see cref="IInteractionRequest"/> objects.
    /// </summary>
    [ContentProperty(Name = "Actions")]
    public class InteractionRequestBehavior : DependencyObject, IBehavior
    {
        private object resolvedSource;
        private bool isLoadedEventRegistered;
        private bool isWindowsRuntimeEvent;
        private Delegate eventHandler;

        public readonly static DependencyProperty ActionsProperty;
        public readonly static DependencyProperty SourceObjectProperty;

        /// <summary>
        /// Gets the collection of actions associated with the behavior. This is a dependency property.
        /// </summary>
        public ActionCollection Actions
        {
            get
            {
                var value = (ActionCollection)GetValue(InteractionRequestBehavior.ActionsProperty);
                if (value == null)
                {
                    value = new ActionCollection();
                    SetValue(InteractionRequestBehavior.ActionsProperty, value);
                }
                return value;
            }
        }

        /// <summary>
        /// Gets the DependencyObject to which the IBehavior will be attached.
        /// </summary>
        public DependencyObject AssociatedObject
        {
            get;
            private set;
        }

        /// <summary>
        /// Gets the SourceObject dependency property.
        /// </summary>
        public object SourceObject
        {
            get { return GetValue(InteractionRequestBehavior.SourceObjectProperty); }
            set { SetValue(InteractionRequestBehavior.SourceObjectProperty, value); }
        }

        static InteractionRequestBehavior()
        {
            InteractionRequestBehavior.ActionsProperty = DependencyProperty.Register("Actions", typeof(ActionCollection), typeof(InteractionRequestBehavior), new PropertyMetadata(null));
            InteractionRequestBehavior.SourceObjectProperty = DependencyProperty.Register("SourceObject", typeof(object), typeof(InteractionRequestBehavior), new PropertyMetadata(null, InteractionRequestBehavior.OnSourceObjectChanged));
        }

        /// <summary>
        /// Specifies the name of the Event this EventTriggerBase is listening for.
        /// </summary>
        protected virtual string GetEventName()
        {
            return "Raised";
        }

        /// <summary>
        /// Attaches to the specified object.
        /// </summary>
        /// <param name="associatedObject">The DependencyObject to which the IBehavior will be attached.</param>
        public void Attach(DependencyObject associatedObject)
        {
            if (AssociatedObject == associatedObject || DesignMode.DesignModeEnabled)
                return;

            if (AssociatedObject != null)
            {
                //CultureInfo currentCulture = CultureInfo.CurrentCulture;
                //String cannotAttachBehaviorMultipleTimesExceptionMessage = ResourceHelper.CannotAttachBehaviorMultipleTimesExceptionMessage;
                //Object[] objArray = new Object[] { associatedObject, this.associatedObject };
                //throw new InvalidOperationException(String.Format(currentCulture, cannotAttachBehaviorMultipleTimesExceptionMessage, objArray));
                throw new InvalidOperationException("CannotAttachBehaviorMultipleTimesExceptionMessage");
            }

            AssociatedObject = associatedObject;
            SetResolvedSource(ComputeResolvedSource());
        }

        /// <summary>
        /// Detaches this instance from its associated object.
        /// </summary>
        public void Detach()
        {
            SetResolvedSource(null);
            AssociatedObject = null;
        }

        internal static bool IsElementLoaded(FrameworkElement element)
        {
            if (element == null)
                return false;

            if ((element.Parent ?? VisualTreeHelper.GetParent(element)) != null)
                return true;

            var content = Window.Current.Content;
            if (content == null)
            {
                return false;
            }
            return element == content;
        }

        private void OnEvent(object sender, object eventArgs)
        {
            Interaction.ExecuteActions(resolvedSource, Actions, eventArgs);
        }

        private static void OnSourceObjectChanged(DependencyObject dependencyObject, DependencyPropertyChangedEventArgs args)
        {
            var eventTriggerBehavior = (InteractionRequestBehavior)dependencyObject;
            eventTriggerBehavior.SetResolvedSource(eventTriggerBehavior.ComputeResolvedSource());
        }

        private object ComputeResolvedSource()
        {
            return ReadLocalValue(InteractionRequestBehavior.SourceObjectProperty) != DependencyProperty.UnsetValue ? SourceObject : AssociatedObject;
        }

        private void SetResolvedSource(object newSource)
        {
            if (AssociatedObject == null || resolvedSource == newSource)
                return;

            if (resolvedSource != null)
                UnregisterEvent(GetEventName());

            resolvedSource = newSource;

            if (resolvedSource != null)
            {
                RegisterEvent(GetEventName());
            }
        }

        private void RegisterEvent(string eventName)
        {
            if (string.IsNullOrEmpty(eventName))
            {
                return;
            }

            if (eventName == "Loaded")
            {
                if (!isLoadedEventRegistered)
                {
                    var frameworkElement = resolvedSource as FrameworkElement;
                    if (frameworkElement != null && !InteractionRequestBehavior.IsElementLoaded(frameworkElement))
                    {
                        isLoadedEventRegistered = true;
                        frameworkElement.Loaded += OnEvent;
                    }
                }
                return;
            }

            var type = resolvedSource.GetType();

            var runtimeEvent = type.GetRuntimeEvent(eventName);
            if (runtimeEvent == null)
            {
                //CultureInfo currentCulture = CultureInfo.CurrentCulture;
                //String cannotFindEventNameExceptionMessage = ResourceHelper.CannotFindEventNameExceptionMessage;
                //Object[] objArray = new Object[] { this.EventName, type.get_Name() };
                throw new ArgumentException("CannotFindEventNameExceptionMessage");
            }

            var declaredMethod = typeof(InteractionRequestBehavior).GetTypeInfo().GetDeclaredMethod("OnEvent");
            this.eventHandler = declaredMethod.CreateDelegate(runtimeEvent.EventHandlerType, this);
            this.isWindowsRuntimeEvent = InteractionRequestBehavior.IsWindowsRuntimeType(runtimeEvent.EventHandlerType);
            if (!isWindowsRuntimeEvent)
            {
                runtimeEvent.AddEventHandler(resolvedSource, eventHandler);
                return;
            }

            WindowsRuntimeMarshal.AddEventHandler(
                add => (EventRegistrationToken)runtimeEvent.AddMethod.Invoke(resolvedSource, new object[] { add }),
                token => runtimeEvent.RemoveMethod.Invoke(resolvedSource, new object[] { token }), 
                eventHandler);
        }

        private void UnregisterEvent(string eventName)
        {
            if (string.IsNullOrEmpty(eventName))
                return;

            if (eventName == "Loaded")
            {
                if (isLoadedEventRegistered)
                {
                    isLoadedEventRegistered = false;

                    var frameworkElement = (FrameworkElement)resolvedSource;
                    if (frameworkElement != null)
                    {
                        frameworkElement.Loaded -= OnEvent;
                    }
                }
                return;
            }

            if (eventHandler == null)
            {
                return;
            }

            var runtimeEvent = resolvedSource.GetType().GetRuntimeEvent(eventName);
            if (!isWindowsRuntimeEvent)
            {
                runtimeEvent.RemoveEventHandler(resolvedSource, eventHandler);
            }
            else
            {
                WindowsRuntimeMarshal.RemoveEventHandler(token => runtimeEvent.RemoveMethod.Invoke(resolvedSource, new object[] { token }), eventHandler);
            }
            eventHandler = null;
        }

        private static bool IsWindowsRuntimeType(Type type)
        {
            return type != null && type.AssemblyQualifiedName.EndsWith("ContentType=WindowsRuntime", StringComparison.Ordinal);
        }
    }
}
