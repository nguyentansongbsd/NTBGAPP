using System;
using System.Collections.Generic;
using System.Globalization;
using System.Text;
using Xamarin.Forms;

namespace PhuLongCRM.Converters
{
    public class PhanHoiStatusCodeConvert : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value != null)
            {
                switch ((int)value)
                {
                    case 1:
                        return "#FDC206";
                    case 2:
                        return "#06CF79";
                    case 3:
                        return "#04A388";
                    case 4:
                        return "#9A40AB";
                    case 5:
                        return "#03ACF5";
                    case 1000:
                        return "#808080";
                    case 6:
                        return "#FA7901";
                    case 2000:
                        return "#333333";              
                    default:
                        return "#f1f1f1";
                }
            }
            return "#f1f1f1";
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
