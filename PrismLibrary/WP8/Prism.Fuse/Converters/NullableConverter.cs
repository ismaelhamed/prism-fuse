using System;
using System.Globalization;
using System.Windows.Data;

namespace Microsoft.Practices.Prism.Converters
{
    /// <summary>
    /// Provides automatic conversion between a nullable type and its underlying primitive type.
    /// </summary>
    public class NullableValueConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return value;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return string.IsNullOrEmpty(value.ToString()) ? null : value;
        }
    }
}
