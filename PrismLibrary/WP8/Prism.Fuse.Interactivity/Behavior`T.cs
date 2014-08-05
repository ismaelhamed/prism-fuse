#if !NETFX_CORE
using System.Windows;
#else
using Windows.UI.Xaml;
#endif

namespace Microsoft.Practices.Prism.Interactivity
{
    /// <summary>
    /// Encapsulates state information and zero or more ICommands into an attachable object.
    /// </summary>
    /// <typeparam name="T">The type the <see cref="T:System.Windows.Interactivity.Behavior`1" /> can be attached to.</typeparam>
    /// <remarks>
    /// 	Behavior is the base class for providing attachable state and commands to an object.
    /// 	The types the Behavior can be attached to can be controlled by the generic parameter.
    /// 	Override OnAttached() and OnDetaching() methods to hook and unhook any necessary handlers
    /// 	from the AssociatedObject.
    /// </remarks>
    public abstract class Behavior<T> : Behavior 
        where T : DependencyObject
    {
        /// <summary>
        /// Gets the object to which this <see cref="T:Microsoft.Practices.Prism.Interactivity.Behavior`1" /> is attached.
        /// </summary>
        public new T AssociatedObject
        {
            get { return (T)base.AssociatedObject; }
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="T:System.Windows.Interactivity.Behavior`1" /> class.
        /// </summary>
        protected Behavior()
            : base(typeof(T))
        { }
    }
}
