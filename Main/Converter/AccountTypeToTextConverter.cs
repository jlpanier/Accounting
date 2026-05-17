using Common;
using System.Globalization;
using static Business.BankAccount;

namespace Main.Converter
{
    public class AccountTypeToTextConverter : IValueConverter
    {
        public object Convert(object? value, Type targetType, object? parameter, CultureInfo culture)
        {
            return value is AccountType accountType ? accountType.GetStringValue() : string.Empty;

        }

        public object ConvertBack(object? value, Type targetType, object? parameter, CultureInfo culture)
        {
            var result = AccountType.Cheque;
            if (value is string str)
            {
                foreach (AccountType accountType in Enum.GetValues(typeof(AccountType)))
                {
                    if (accountType.GetStringValue() == str)
                    {
                        result = accountType;
                    }
                }
            }
            return result;
        }
    }
}
