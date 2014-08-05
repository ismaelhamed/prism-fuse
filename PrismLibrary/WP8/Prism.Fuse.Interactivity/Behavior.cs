using System;
using System.Globalization;
using System.Windows;
using Microsoft.Xaml.Interactivity;

namespace Microsoft.Practices.Prism.Interactivity
{
    /// <summary>
    /// Encapsulates state information and zero or more ICommands into an attachable object.
    /// </summary>
    /// <remarks>This is an infrastructure class. Behavior authors should derive from Behavior&lt;T&gt; instead of from this class.</remarks>
    public abstract class Behavior : DependencyObject, IBehavior
    {
        /// <summary>
        /// The type to which this behavior can be attached.
        /// </summary>
        protected Type AssociatedType
        {
            get;
            private set;
        }

        /// <summary>
        /// Gets the object to which this behavior is attached.
        /// </summary>
        public DependencyObject AssociatedObject
        {
            get; 
            private set;
        }

        internal Behavior(Type associatedType)
        {
            AssociatedType = associatedType;
        }

        /// <summary>
        /// Attaches to the specified object.
        /// </summary>
        /// <param name="dependencyObject">The object to attach to.</param>
        /// <exception cref="T:System.InvalidOperationException">The Behavior is already hosted on a different element.</exception>
        /// <exception cref="T:System.InvalidOperationException">dependencyObject does not satisfy the Behavior type constraint.</exception>
        public void Attach(DependencyObject dependencyObject)
        {
            if (AssociatedObject == dependencyObject)
                return;

            if (AssociatedObject != null)
            {
                throw new InvalidOperationException(Properties.Resources.CannotHostBehaviorMultipleTimesExceptionMessage);
            }

            if (dependencyObject != null && !AssociatedType.IsInstanceOfType(dependencyObject))
            {
                var currentCulture = CultureInfo.CurrentCulture;
                var typeConstraintViolatedExceptionMessage = Properties.Resources.TypeConstraintViolatedExceptionMessage;
                var name = new object[] { GetType().Name, dependencyObject.GetType().Name, AssociatedType.Name };
                throw new InvalidOperationException(string.Format(currentCulture, typeConstraintViolatedExceptionMessage, name));
            }

            AssociatedObject = dependencyObject;
            OnAttached();
        }

        /// <summary>
        /// Detaches this instance from its associated object.
        /// </summary>
        public void Detach()
        {
            OnDetaching();
            AssociatedObject = null;
        }

        /// <summary>
        /// Called after the behavior is attached to an AssociatedObject.
        /// </summary>
        /// <remarks>Override this to hook up functionality to the AssociatedObject.</remarks>
        protected virtual void OnAttached()
        { }

        /// <summary>
        /// Called when the behavior is being detached from its AssociatedObject, but before it has actually occurred.
        /// </summary>
        /// <remarks>Override this to unhook functionality from the AssociatedObject.</remarks>
        protected virtual void OnDetaching()
        { }
    }
}
