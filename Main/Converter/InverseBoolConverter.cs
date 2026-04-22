using System.Globalization;

namespace Main.Converter
{
    /// <summary>
    /// Hestion de booléen inversé pour les bindings
    /// </summary>
    public class InverseBoolConverter : IValueConverter
    {
        public object Convert(object? value, Type targetType, object? parameter, CultureInfo culture)
            => !(bool)(value ?? false);

        public object ConvertBack(object? value, Type targetType, object? parameter, CultureInfo culture)
            => !(bool)(value ?? false);
    }

}
