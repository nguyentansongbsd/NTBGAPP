using System;
using System.Globalization;
using Xamarin.Forms;

namespace PhuLongCRM.Converters
{
    public class TinhTrangConverter :IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if ((int)value == 1)
            {
                return "Mở";
            }
            else if ((int)value == 3)
            {
                return "Đủ điều kiện";
            }
            else if ((int)value == 5)
            {
                return "Không điều kiện";
            }
            else
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
