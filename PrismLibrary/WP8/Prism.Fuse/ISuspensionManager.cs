using System;
using System.Collections.Generic;
using System.Runtime.Serialization;
using System.Threading.Tasks;
using System.Windows.Controls;

namespace Microsoft.Practices.Prism
{
    /// <summary>
    /// The ISuspensionManager interface will be implemented by a class that handles the application's state saving and retrieving. 
    /// The default implementation of ISuspensionManager is the SuspensionManager class, which captures global session state to 
    /// simplify process lifetime management for an application.
    /// </summary>
    public interface ISuspensionManager
    {
        /// <summary>
        /// Provides access to global session state for the current session. This state is serialized by <see cref="SaveAsync"/> and 
        /// restored by <see cref="RestoreAsync"/>, so values must be serializable by <see cref="DataContractSerializer"/> and should 
        /// be as compact as possible. Strings and other self-contained data types are strongly recommended.
        /// </summary>
        /// <value>The global session state.</value>
        Dictionary<string, object> SessionState { get; }

        /// <summary>
        /// Adds a type to the list of custom types provided to the <see cref="DataContractSerializer"/> when reading and writing session 
        /// state. The known type empty, additional types may be added to customize the serialization process.
        /// </summary>
        /// <param name="type">The type.</param>
        void RegisterKnownType(Type type);

        /// <summary>
        /// Save the current <see cref="SessionState"/>.
        /// </summary>
        /// <returns>An asynchronous task that reflects when session state has been saved.</returns>
        Task SaveAsync();

        /// <summary>
        /// Restores previously saved <see cref="SessionState" />.
        /// </summary>
        /// <returns>An asynchronous task that reflects when session state has been read. The content of <see cref="SessionState"/> should not be relied upon until this task completes.</returns>
        Task RestoreAsync();

        /// <summary>
        /// Registers a <see cref="Frame"/> instance to allow its navigation history to be saved to and restored from <see cref="SessionState"/>.  
        /// Frames should be registered once immediately after creation if they will participate in session state management. Upon registration 
        /// if state has already been restored for the specified key the navigation history will immediately be restored. Subsequent invocations of
        /// <see cref="RestoreAsync"/> will also restore navigation history.
        /// </summary>
        /// <param name="frame">An instance whose navigation history should be managed by <see cref="SuspensionManagerException"/></param>
        /// <param name="sessionStateKey">A unique key into <see cref="SessionState"/> used to store navigation-related information.</param>
        void RegisterFrame(Frame frame, string sessionStateKey);

        /// <summary>
        /// Disassociates a <see cref="Frame"/> previously registered by <see cref="RegisterFrame"/> from <see cref="SessionState"/>. 
        /// Any navigation state previously captured will be removed.
        /// </summary>
        /// <param name="frame">An instance whose navigation history should no longer be managed.</param>
        void UnregisterFrame(Frame frame);

        /// <summary>
        /// Provides storage for session state associated with the specified <see cref="Frame"/>. Frames that have been previously 
        /// registered with <see cref="RegisterFrame"/> have their session state saved and restored automatically as a part of the 
        /// global <see cref="SessionState"/>. Frames that are not registered have transient state that can still be useful when 
        /// restoring pages that have been discarded from the navigation cache.
        /// </summary>
        /// <param name="frame">The instance for which session state is desired.</param>
        /// <returns>A collection of state subject to the same serialization mechanism as <see cref="SessionState"/>.</returns>
        Dictionary<string, object> GetSessionStateForFrame(Frame frame);
    }
}