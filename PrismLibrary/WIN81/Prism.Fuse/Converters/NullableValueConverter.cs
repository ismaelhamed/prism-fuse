using System;
using Windows.UI.Xaml.Data;

namespace Microsoft.Practices.Prism.Converters
{
    /// <summary>
    /// Provides automatic conversion between a nullable type and its underlying primitive type.
    /// </summary>
    public class NullableValueConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, string language)
        {
            return value;
        }

        public object ConvertBack(object value, Type targetType, object parameter, string language)
        {
            return string.IsNullOrEmpty(value.ToString()) ? null : value;
        }
    }
}
