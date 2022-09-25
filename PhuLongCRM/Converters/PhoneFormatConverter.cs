using System;
using System.Globalization;
using Xamarin.Forms;

namespace PhuLongCRM.Converters
{
	public class PhoneFormatConverter : IValueConverter
	{
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value == null) return null;
            string mobilephone = value.ToString();
            if (mobilephone != null && mobilephone.Contains("-"))
            {
                return mobilephone.Split('-')[1].StartsWith("84") ? mobilephone.Replace("84", "+84-") : mobilephone;
            }
            else if (mobilephone != null && mobilephone.Contains("+84"))
            {
                return mobilephone.Replace("+84", "+84-");
            }
            else if (mobilephone != null && mobilephone.StartsWith("84"))
            {
                return mobilephone.Replace("84", "+84-");
            }
            else
            {
                return mobilephone;
            }
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}

