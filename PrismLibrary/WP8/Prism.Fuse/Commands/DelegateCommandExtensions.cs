using System;
using System.ComponentModel;
using System.Linq.Expressions;
using System.Reflection;

namespace Microsoft.Practices.Prism.Commands
{
    using Microsoft.Practices.Prism.Properties;

    public static class DelegateCommandExtensions
    {
        /// <summary>
        /// http://dotnetgeek.tumblr.com/post/33755827925/dependson-extension-method
        /// </summary>
        public static void DependsOn<T>(this DelegateCommandBase command, Expression<Func<T>> propertyExpression)
        {
            if (command == null)
                throw new ArgumentNullException("command");

            if (propertyExpression == null)
                throw new ArgumentNullException("propertyExpression");

            var memberExpression = propertyExpression.Body as MemberExpression;
            if (memberExpression == null)
                throw new ArgumentException(Resources.MemberExpression, "propertyExpression");

            var propertyInfo = memberExpression.Member as PropertyInfo;
            if (propertyInfo == null)
                throw new ArgumentException(Resources.PropertyExpression, "propertyExpression");

            var constantExpression = memberExpression.Expression as ConstantExpression;
            if (constantExpression == null)
                throw new ArgumentException(Resources.ConstantExpression, "propertyExpression");

            var propertyChanged = constantExpression.Value as INotifyPropertyChanged;
            if (propertyChanged == null)
            {
                throw new ArgumentException(string.Format("{0} must implement INotifyPropertyChanged", constantExpression.Type.Name));
            }

            propertyChanged.PropertyChanged += (sender, args) =>
            {
                if (args.PropertyName.Equals(propertyInfo.Name, StringComparison.OrdinalIgnoreCase))
                {
                    command.RaiseCanExecuteChanged();
                }
            };
        }
    }
}
