using System;
using System.Collections.Generic;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Data;

namespace Microsoft.Practices.Prism.Converters
{
    public abstract class BooleanConverter<T> : IValueConverter
    {
        public T TrueCondition { get; set; }
        public T FalseCondition { get; set; }

        protected BooleanConverter(T trueValue, T falseValue)
        {
            TrueCondition = trueValue;
            FalseCondition = falseValue;
        }

        public object Convert(object value, Type targetType, object parameter, string language)
        {
            return value is bool && ((bool)value) ? TrueCondition : FalseCondition;
        }

        public object ConvertBack(object value, Type targetType, object parameter, string language)
        {
            return value is T && EqualityComparer<T>.Default.Equals((T)value, TrueCondition);
        }
    }

    public sealed class BooleanToVisibilityConverter : BooleanConverter<Visibility>
    {
        public BooleanToVisibilityConverter() :
            base(Visibility.Visible, Visibility.Collapsed) { }
    }
}
