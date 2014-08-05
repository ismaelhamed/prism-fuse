// THIS CODE AND INFORMATION IS PROVIDED "AS IS" WITHOUT WARRANTY OF
// ANY KIND, EITHER EXPRESSED OR IMPLIED, INCLUDING BUT NOT LIMITED TO
// THE IMPLIED WARRANTIES OF MERCHANTABILITY AND/OR FITNESS FOR A
// PARTICULAR PURPOSE.
//
// Copyright (c) Microsoft Corporation. All rights reserved

using System;
using System.Collections.Generic;
using System.IO;
using System.IO.IsolatedStorage;
using System.Runtime.Serialization;
using System.Security.Cryptography;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;

namespace Microsoft.Practices.Prism
{
    /// <summary>
    /// SessionStateService captures global session state to simplify process lifetime management for an application. Note that session 
    /// state will be automatically cleared under a variety of conditions and should only be used to store information that would be 
    /// convenient to carry across sessions, but that should be discarded when an application crashes or is upgraded.
    /// </summary>
    public class SuspensionManager : ISuspensionManager
    {
        // Fields
        private readonly List<Type> knownTypes = new List<Type>();
        private const string persistentStateFilename = "_sessionState.xml";
        private Dictionary<string, object> persistentState = new Dictionary<string, object>();
        private static readonly List<WeakReference<Frame>> registeredFrames = new List<WeakReference<Frame>>();

        private static readonly DependencyProperty FrameSessionStateProperty =
            DependencyProperty.RegisterAttached(
                "_FrameSessionState",
                typeof(Dictionary<string, object>),
                typeof(SuspensionManager), null);

        private static readonly DependencyProperty FrameSessionStateKeyProperty =
            DependencyProperty.RegisterAttached(
                "_FrameSessionStateKey",
                typeof(string),
                typeof(SuspensionManager), null);

        /// <summary>
        /// Provides access to global session state for the current session. This state is serialized by <see cref="SaveAsync"/> and restored by
        /// <see cref="RestoreAsync"/>, so values must be serializable by <see cref="DataContractSerializer"/> and should be as compact as possible. 
        /// Strings and other self-contained data types are strongly recommended.
        /// </summary>
        public Dictionary<string, object> SessionState
        {
            get { return persistentState; }
        }

        /// <summary>
        /// Adds a type to the list of custom types provided to the <see cref="DataContractSerializer"/> when reading and 
        /// writing session state. Initially empty, additional types may be added to customize the serialization process.
        /// </summary>
        public void RegisterKnownType(Type type)
        {
            knownTypes.Add(type);
        }

        /// <summary>
        /// Save the current <see cref="SessionState"/>. 
        /// </summary>
        /// <returns>An asynchronous task that reflects when session state has been saved.</returns>
        public async Task SaveAsync()
        {
            try
            {
                var ms = new MemoryStream();

                // Serialize the session state synchronously to avoid asynchronous access to shared state
                var serializer = new DataContractSerializer(typeof(Dictionary<string, object>), knownTypes);
                serializer.WriteObject(ms, persistentState);

                // Get an output stream for the SessionState file and write the state asynchronously
                using (var file = IsolatedStorageFile.GetUserStoreForApplication())
                using (var fileStream = new IsolatedStorageFileStream(persistentStateFilename, FileMode.Create, FileAccess.Write, file))
                {
                    ms.Seek(0, SeekOrigin.Begin);

                    // Encrypt the session data and write it to disk.
                    var data = ProtectedData.Protect(ms.ToArray(), null);
                    await fileStream.WriteAsync(data, 0, data.Length);
                }
            }
            catch (Exception e)
            {
                throw new SuspensionManagerException(e);
            }
        }

        /// <summary>
        /// Restores previously saved <see cref="SessionState" />.
        /// </summary>
        /// <returns>An asynchronous task that reflects when session state has been read. The content of <see cref="SessionState"/> should not be relied upon until this task completes.</returns>
        public async Task RestoreAsync()
        {
            persistentState = new Dictionary<string, object>();

            try
            {
                // Get the input stream for the SessionState file
                using (var file = IsolatedStorageFile.GetUserStoreForApplication())
                using (var fileStream = new IsolatedStorageFileStream(persistentStateFilename, FileMode.Open, FileAccess.Read, file))
                {
                    var bytes = new byte[fileStream.Length];
                    await fileStream.ReadAsync(bytes, 0, bytes.Length);

                    var ms = new MemoryStream(ProtectedData.Unprotect(bytes, null));
                    ms.Seek(0, SeekOrigin.Begin);

                    var serializer = new DataContractSerializer(typeof(Dictionary<string, object>), knownTypes);
                    persistentState = (Dictionary<string, object>)serializer.ReadObject(ms);
                }
            }
            catch (Exception e)
            {
                throw new SuspensionManagerException(e);
            }
        }

        /// <summary>
        /// Registers a <see cref="Frame"/> instance to allow its navigation history to be saved to and restored from <see cref="SessionState"/>. 
        /// Frames should be registered once immediately after creation if they will participate in session state management. Upon registration, 
        /// if state has already been restored for the specified key, the navigation history will immediately be restored. Subsequent invocations 
        /// of <see cref="RestoreAsync"/> will also restore navigation history.
        /// </summary>
        /// <param name="frame">An instance whose navigation history should be managed by <see cref="SuspensionManagerException"/></param>
        /// <param name="sessionStateKey">A unique key into <see cref="SessionState"/> used to store navigation-related information.</param>
        public void RegisterFrame(Frame frame, string sessionStateKey)
        {
            if (frame == null)
            {
                throw new ArgumentNullException("frame");
            }

            if (frame.GetValue(FrameSessionStateKeyProperty) != null)
            {
                throw new InvalidOperationException("Frames can only be registered to one session state key");
            }

            if (frame.GetValue(FrameSessionStateProperty) != null)
            {
                throw new InvalidOperationException("Frames must be either be registered before accessing frame session state, or not registered at all");
            }

            // Use a dependency property to associate the session key with a frame, and keep a list of frames whose
            // navigation state should be managed
            frame.SetValue(FrameSessionStateKeyProperty, sessionStateKey);
            registeredFrames.Add(new WeakReference<Frame>(frame));
        }

        /// <summary>
        /// Disassociates a <see cref="Frame"/> previously registered by <see cref="RegisterFrame"/> from <see cref="SessionState"/>. 
        /// Any navigation state previously captured will be removed.
        /// </summary>
        /// <param name="frame">An instance whose navigation history should no longer be managed.</param>
        public void UnregisterFrame(Frame frame)
        {
            // Remove session state and remove the frame from the list of frames whose navigation state will be saved 
            // (along with any weak references that are no longer reachable)
            SessionState.Remove((string)frame.GetValue(FrameSessionStateKeyProperty));

            registeredFrames.RemoveAll(weakFrameReference =>
            {
                Frame testFrame;
                return !weakFrameReference.TryGetTarget(out testFrame) || testFrame == frame;
            });
        }

        /// <summary>
        /// Provides storage for session state associated with the specified <see cref="Frame"/>. Frames that have been previously 
        /// registered with <see cref="RegisterFrame"/> have their session state saved and restored automatically as a part of the 
        /// global <see cref="SessionState"/>. Frames that are not registered have transient state that can still be useful when 
        /// restoring pages that have been discarded from the navigation cache.
        /// </summary>
        /// <param name="frame">The instance for which session state is desired.</param>
        /// <returns>A collection of state, subject to the same serialization mechanism as <see cref="SessionState"/>.</returns>
        public Dictionary<string, object> GetSessionStateForFrame(Frame frame)
        {
            if (frame == null)
                throw new ArgumentNullException("frame");

            var sessionState = (Dictionary<string, object>)frame.GetValue(FrameSessionStateProperty);
            if (sessionState == null)
            {
                var sessionStateKey = (string)frame.GetValue(FrameSessionStateKeyProperty);
                if (sessionStateKey != null)
                {
                    // Registered frames reflect the corresponding session state
                    if (!persistentState.ContainsKey(sessionStateKey))
                    {
                        persistentState[sessionStateKey] = new Dictionary<string, object>();
                    }

                    sessionState = (Dictionary<string, object>)persistentState[sessionStateKey];
                }
                else
                {
                    // Frames that aren't registered have transient state
                    sessionState = new Dictionary<string, object>();
                }

                frame.SetValue(FrameSessionStateProperty, sessionState);
            }

            return sessionState;
        }
    }
}
