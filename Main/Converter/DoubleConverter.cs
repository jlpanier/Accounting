using System.Globalization;

namespace Main.Converter
{
    public class DoubleConverter : IValueConverter
    {
        public object Convert(object? value, Type targetType, object? parameter, CultureInfo culture)
            => value?.ToString();

        public object ConvertBack(object? value, Type targetType, object? parameter, CultureInfo culture)
        {
            if (double.TryParse(value?.ToString(), NumberStyles.AllowDecimalPoint, CultureInfo.InvariantCulture, out var result))
                return result;

            return 0m;
        }
    }

}
