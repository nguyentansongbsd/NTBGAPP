using System;
using System.Globalization;
using Xamarin.Forms;

namespace PhuLongCRM.Converters
{
    public class CurrencyFormatConverter :IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            double currency = decimal.ToDouble((decimal)value);
            if (currency > 0 && currency.ToString().Length <= 6) // đ
            {
                return string.Format("{0:#,0} đ", currency);
            }
            else if(currency > 0 && (currency.ToString().Length <= 9 && currency.ToString().Length >6)) //Triệu
            {
                var _currency = currency.ToString().Substring(0,currency.ToString().Length - 6);
                return string.Format("{0} Triệu", _currency);
            }
            else if (currency > 0 && currency.ToString().Length > 9) //Tỷ
            {
                var _currency = currency.ToString().Substring(0, currency.ToString().Length - 9);
                return string.Format("{0:#,0} Tỷ", decimal.Parse(_currency));
            }
            else
            {
                return string.Format("{0:#,0} đ", currency);
            }
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
