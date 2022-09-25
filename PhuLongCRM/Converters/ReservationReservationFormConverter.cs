using System;
using System.Collections.Generic;
using System.Globalization;
using System.Text;
using Xamarin.Forms;

namespace PhuLongCRM.Converters
{
    public class ReservationReservationFormConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value != null)
            {
                switch ((int)value)
                {
                    case 100000003:
                        return "Expired RF"; // đã hết hạn ký phiếu đặt cọc
                    case 100000000:
                        return "Not Print RF"; // chưa in phiếu đặt cọc
                    case 100000001:
                        return "Printed RF"; // đã in phiếu đặt cọc
                    case 100000002:
                        return "Signed RF"; // đã ký phiếu đặt cọc
                    default:
                        return "";
                }
            }
            return "";
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
