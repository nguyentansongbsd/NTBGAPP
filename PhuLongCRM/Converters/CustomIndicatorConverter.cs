using System;
using System.Globalization;
using Xamarin.Forms;

namespace PhuLongCRM.Converters
{
    public class CustomIndicatorConverter :IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if ((bool)value == true)
            {
                return "\uf078";
            }
            else
            {
                return "\uf053";
            }
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
