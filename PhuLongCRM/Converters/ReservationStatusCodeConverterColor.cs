using System;
using System.Collections.Generic;
using System.Globalization;
using System.Text;
using PhuLongCRM.Models;
using Xamarin.Forms;

namespace PhuLongCRM.Converters
{
    public class ReservationStatusCodeConverterColor : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value != null)
            {
                return QuoteStatusCodeData.GetQuoteStatusCodeById(value.ToString()).Background;
            }
            else
            {
                return "#bfbfbf";
            }
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
