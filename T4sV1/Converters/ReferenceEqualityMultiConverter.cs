using System.Globalization;
using Microsoft.Maui.Controls;

namespace T4sV1.Converters
{
    public sealed class ReferenceEqualityMultiConverter : IMultiValueConverter
    {
        public object Convert(object[] values, Type targetType, object parameter, CultureInfo culture)
            => values is { Length: >= 2 } && ReferenceEquals(values[0], values[1]);

        public object[] ConvertBack(object value, Type[] targetTypes, object parameter, CultureInfo culture)
            => throw new NotSupportedException();
    }
}
