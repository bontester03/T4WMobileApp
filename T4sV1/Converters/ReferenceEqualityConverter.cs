// Converters/ReferenceEqualityConverter.cs
using System.Globalization;

namespace T4sV1.Converters
{
    public sealed class ReferenceEqualityConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
            => ReferenceEquals(value, parameter);

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
            => value is bool b && b ? parameter! : BindableProperty.UnsetValue;
    }
}
