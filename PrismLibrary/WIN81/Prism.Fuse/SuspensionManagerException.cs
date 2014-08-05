using System;

namespace Microsoft.Practices.Prism
{
    /// <summary>
    /// Exception thrown by the internal SuspensionManager for suspension failures.
    /// </summary>
    public class SuspensionManagerException : Exception
    {
        /// <summary>
        /// Initializes a new instance of the SuspensionManagerException class.
        /// </summary>
        public SuspensionManagerException()
        { }

        /// <summary>
        /// Initializes a new instance of the SuspensionManagerException class.
        /// </summary>
        /// <param name="e">Inner Exception</param>
        public SuspensionManagerException(Exception e)
            : base("SuspensionManager failed", e)
        { }
    }
}