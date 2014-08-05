namespace System
{
    /// <summary>
    /// Represents a typed weak reference, which references an object while still allowing that object to be reclaimed by garbage collection.
    /// </summary>
    /// <typeparam name="T">The type of the object referenced.</typeparam>
    public class WeakReference<T>
        where T : class
    {
        /// <summary>
        /// The actual reference
        /// </summary>
        private readonly WeakReference reference;

        /// <summary>
        /// The item being tracked, or null if it is no longer alive
        /// </summary>
        private T Target
        {
            get { return (T)reference.Target; }
            set { reference.Target = value; }
        }

        /// <summary>
        /// Initializes a new instance of the WeakReference{T} class that references the specified object.
        /// </summary>
        /// <param name="target">The object to reference, or null.</param>
        public WeakReference(T target)
        {
            reference = new WeakReference(target);
        }

        /// <summary>
        /// Initializes a new instance of the WeakReference{T} class that references the specified object and uses the specified resurrection tracking.
        /// </summary>
        /// <param name="target">The object to reference, or null.</param>
        /// <param name="trackResurrection">true to track the object after finalization; false to track the object only until finalization.</param>
        public WeakReference(T target, bool trackResurrection)
        {
            reference = new WeakReference(target, trackResurrection);
        }

        /// <summary>
        /// Sets the target object that is referenced by this WeakReference<T> object.
        /// </summary>
        /// <param name="target">The new target object.</param>
        public void SetTarget(T target)
        {
            Target = target;
        }

        /// <summary>
        /// Tries to retrieve the target object that is referenced by the current WeakReference<T> object.
        /// </summary>
        /// <param name="target">When this method returns, contains the target object, if available. This parameter is passed uninitialized.</param>
        /// <returns>true if the target was retrieved, false otherwise.</returns>
        public bool TryGetTarget(out T target)
        {
            var t = Target;
            target = t;
            return t != null;
        }
    }
}
