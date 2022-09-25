using System;
using System.Collections.Generic;
using System.Globalization;
using System.Text;
using Xamarin.Forms;

namespace PhuLongCRM.Converters
{
    public class ReservationStatusCodeConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value != null)
            {
                switch ((int)value)
                {
                    case 6:
                        return "Canceled";
                    case 3:
                        return "Deposited";
                    case 100000006:
                        return "Collected";
                    case 100000009:
                        return "Expired";
                    case 100000005:
                        return "Expired of signing RF";
                    case 100000008:
                        return "Expired Quotation";
                    case 2:
                        return "In Progress";
                    case 1:
                        return "In Progress";
                    case 5:
                        return "Lost";
                    case 100000002:
                        return "Pending Cancel Deposit";
                    case 100000007:
                        return "Quotation";
                    case 100000003:
                        return "Reject";
                    case 100000000:
                        return "Reservation";
                    case 7:
                        return "Revised";
                    case 100000004:
                        return "Signed RF";
                    case 100000001:
                        return "Terminated";
                    case 4:
                        return "Won";
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
