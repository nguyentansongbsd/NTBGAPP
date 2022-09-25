using PhuLongCRM.Resources;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Text;
using Xamarin.Forms;

namespace PhuLongCRM.Converters
{
    public class RegardingConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
                if (value != null)
                {
                    if ((string)value == "appointment")
                    { 
                        return Language.nguoi_lien_quan; 
                    }
                    else
                    {
                        return Language.khach_hang;
                    }
                }
                else
                {
                    return "";
                }
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
