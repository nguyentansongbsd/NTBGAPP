using System;
using System.Globalization;
using Xamarin.Forms;

namespace PhuLongCRM.Converters
{
    public class PhoneConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value == null) return null;
            string phone = value.ToString();
            if (phone.StartsWith("84"))
            {
                return "+84-" + phone.Replace("84", "");
            }
            else
            {
                return phone;
            }
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
