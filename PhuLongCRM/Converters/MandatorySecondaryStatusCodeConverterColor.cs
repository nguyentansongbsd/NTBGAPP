using System;
using System.Collections.Generic;
using System.Globalization;
using System.Text;
using Xamarin.Forms;

namespace PhuLongCRM.Converters
{
    public class MandatorySecondaryStatusCodeConverterColor : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value != null)
            {
                switch ((int)value)
                {
                    case 1:
                        return "#06CF79";
                    case 100000000:
                        return "#03ACF5";
                    case 100000001:
                        return "#FA7901";
                    case 2:
                        return "#FDC206";
                    default:
                        return "#f3f3f3";
                }
            }
            return "#f3f3f3";
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
