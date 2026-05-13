using System.Globalization;

namespace Main.Converter
{
    public class DecimalConverter : IValueConverter
    {
        public object Convert(object? value, Type targetType, object? parameter, CultureInfo culture)
            => value?.ToString();

        public object ConvertBack(object? value, Type targetType, object? parameter, CultureInfo culture)
        {
            if (decimal.TryParse(value?.ToString(), NumberStyles.Any, CultureInfo.InvariantCulture, out var result))
                return result;

            return 0m;
        }
    }

}
