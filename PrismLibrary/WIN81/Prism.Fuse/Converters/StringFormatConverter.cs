using System;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Data;

namespace Microsoft.Practices.Prism.Converters
{
    /// <summary>
    /// USAGE:
    ///  
    /// <TextBlock Text="{Binding Amount, 
    ///     Converter={StaticResource StringFormatConverter}, 
    ///     ConverterParameter='{}{0:C}'}" />
    /// 
    /// <TextBlock Text="{Binding Name, 
    ///     Converter={StaticResource StringFormatConverter}, 
    ///     ConverterParameter='Welcome, {0}!'}" />  
    /// 
    /// </summary>
    public sealed class StringFormatConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, string language)
        {
            if (value == null)
                return null;

            return parameter == null ? value : string.Format((string)parameter, value);
        }

        public object ConvertBack(object value, Type targetType, object parameter, string language)
        {
            return DependencyProperty.UnsetValue;
        }
    }
}
