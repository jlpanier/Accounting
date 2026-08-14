using System.Globalization;


namespace Main.Converter
{
    public class BoolToColorConverter : IValueConverter
    {
        public object? Convert(object? value, Type targetType, object? parameter, CultureInfo culture)
        {
            if (parameter == null) return false;
            var data = parameter.ToString();
            if (data != null)
            {
                var parts = data.Split(',');
                if (value is bool val)
                {
                    var part = val ? parts[0] : parts[1];
                    if (Application.Current!=null)
                    {
                        return Application.Current.Resources[part];
                    }
                }
            }
            return null;
        }

        public object ConvertBack(object? value, Type targetType, object? parameter, CultureInfo culture)
            => throw new NotImplementedException();
    }

}
