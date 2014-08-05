using System;
using System.Collections.Generic;

namespace Microsoft.Practices.Prism.Regions
{
    /// <summary>
    /// Provides a way for objects involved in navigation to be notified when the application is being activated or deactivated.
    /// </summary>
    public interface IApplicationLifecycleAware
    {
#if !NETFX_CORE
        /// <summary>
        /// Called when the application has been activated.
        /// </summary>
        /// <param name="isApplicationInstancePreserved">Indicates whether the application instance is reactivating from dormant state.</param>
        /// <param name="e">EventArgs containing the SessionState dictionary.</param>
        void OnActivated(bool isApplicationInstancePreserved, SessionStateEventArgs e);
#else
        /// <summary>
        /// Called when the application has been activated.
        /// </summary>
        /// <param name="e"></param>
        void OnActivated(SessionStateEventArgs e);
#endif
        /// <summary>
        /// Called when the application is being deactivated.
        /// </summary>
        /// <param name="e">EventArgs containing the SessionState dictionary.</param>
        void OnDeactivated(SessionStateEventArgs e);
    }

    /// <summary>
    /// Class used to hold the event data required when a page attempts to load state.
    /// </summary>
    public class SessionStateEventArgs : EventArgs
    {
        /// <summary>
        /// A dictionary of state preserved by this page during an earlier session. This will be null the first time a page is visited.
        /// </summary>
        public Dictionary<string, object> SessionState
        {
            get;
            private set;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="SessionStateEventArgs"/> class.
        /// </summary>
        /// <param name="sessionState">A dictionary of state preserved by this page during an earlier session. This will be null the first time a page is visited.</param>
        public SessionStateEventArgs(Dictionary<string, object> sessionState)
        {
            SessionState = sessionState;
        }
    }
}
