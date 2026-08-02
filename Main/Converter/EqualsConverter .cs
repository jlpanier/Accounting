using System.Globalization;


namespace Main.Converter
{
    public class EqualsConverter : IValueConverter
    {
        public object Convert(object? value, Type targetType, object? parameter, CultureInfo culture)
            => value?.ToString() == parameter?.ToString();

        public object? ConvertBack(object? value, Type targetType, object? parameter, CultureInfo culture)
        {
            object? result = null;
            if (value is bool val)
            {
                result = parameter;
            }
            return result;
        }
    }
}
