using System;
using System.Globalization;
using Xamarin.Forms;

namespace PhuLongCRM.Converters
{
    public class GroupConverter :IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if ((int)value== 100000000)
            {
                return "S&M";
            }
            else if ((int)value == 100000001)
            {
                return "CCR";
            }
            else if ((int)value == 100000002)
            {
                return "FIN";
            }
            {
                return null;
            }
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
