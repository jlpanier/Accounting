using System.Globalization;

namespace Main.Converter
{
    public class DecimalConverter : IValueConverter
    {
        public object Convert(object? value, Type targetType, object? parameter, CultureInfo culture)
            => value?.ToString();

        public object ConvertBack(object? value, Type targetType, object? parameter, CultureInfo culture)
        {
            if (value == null)
            {
                return 0m;
            }
            else
            {
                var numerostr = value.ToString();
                if (string.IsNullOrWhiteSpace(numerostr))
                {
                    return 0m;
                }
                else
                {
                    if (decimal.TryParse(numerostr.Replace(",", "."), NumberStyles.AllowDecimalPoint, CultureInfo.InvariantCulture, out var result))
                        return result;
                }

            }

            return 0m;
        }
    }

}
