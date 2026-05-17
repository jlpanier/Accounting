using System.Globalization;


namespace Main.Converter
{
    public class BoolToColorConverter : IValueConverter
    {
        public object Convert(object? value, Type targetType, object? parameter, CultureInfo culture)
        {
            var parts = parameter.ToString().Split(',');
            return (bool)value
                ? Application.Current.Resources[parts[0]]
                : Application.Current.Resources[parts[1]];
        }

        public object ConvertBack(object? value, Type targetType, object? parameter, CultureInfo culture)
            => throw new NotImplementedException();
    }

}
