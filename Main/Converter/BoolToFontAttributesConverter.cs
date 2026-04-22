using System.Globalization;

namespace Main.Converter
{
    /// <summary>
    /// Conversion d'un booléen en attribut de police (gras ou normal)
    /// </summary>
    public class BoolToFontAttributesConverter : IValueConverter
    {
        public object Convert(object? value, Type targetType, object? parameter, CultureInfo culture)
        {
            return (value is bool b && b) ? FontAttributes.Bold : FontAttributes.None;
        }

        public object ConvertBack(object? value, Type targetType, object? parameter, CultureInfo culture)
        {
            return value is FontAttributes fa && fa == FontAttributes.Bold;
        }
    }
}
