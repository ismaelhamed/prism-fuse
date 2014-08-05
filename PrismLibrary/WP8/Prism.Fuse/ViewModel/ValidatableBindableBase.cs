using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq.Expressions;

namespace Microsoft.Practices.Prism.ViewModel
{
    /// <summary>
    /// Contains the logic to run the validation rules of the instance of a model class and return the results of this 
    /// validation as a list of properties' errors.
    /// </summary>
    public class ValidatableBindableBase : BindableBase, INotifyDataErrorInfo
    {
        // Fields
        private readonly ErrorsContainer<string> errorsContainer;

        public ValidatableBindableBase()
        {
            errorsContainer = new ErrorsContainer<string>(OnErrorsChanged);
        }

        protected virtual void SetErrors<TProp>(Expression<Func<TProp>> propExpression, IEnumerable<string> errors)
        {
            errorsContainer.SetErrors(propExpression, errors);
        }

        protected virtual void ClearErrors<TProp>(Expression<Func<TProp>> propExpression)
        {
            errorsContainer.ClearErrors(propExpression);
        }

        #region Implementation of INotifyDataErrorInfo

        /// <summary>
        /// Occurs when the Errors collection changed because new errors were added or old errors were fixed.
        /// </summary>
        public event EventHandler<DataErrorsChangedEventArgs> ErrorsChanged;

        protected virtual void OnErrorsChanged(string propertyName)
        {
            var handler = ErrorsChanged;
            if (handler != null)
            {
                handler(this, new DataErrorsChangedEventArgs(propertyName));
            }
        }

        /// <summary>
        /// Gets a value that indicates whether the entity has validation errors. 
        /// </summary>
        /// <returns>
        /// true if the entity currently has validation errors; otherwise, false.
        /// </returns>
        public bool HasErrors
        {
            get { return errorsContainer.HasErrors; }
        }

        /// <summary>
        /// Gets the validation errors for a specified property or for the entire entity.
        /// </summary>
        /// <returns>
        /// The validation errors for the property or entity.
        /// </returns>
        /// <param name="propertyName">The name of the property to retrieve validation errors for; or null or <see cref="F:System.String.Empty"/>, to retrieve entity-level errors.</param>
        public IEnumerable GetErrors(string propertyName)
        {
            return errorsContainer.GetErrors(propertyName);
        }

        #endregion

    }
}
