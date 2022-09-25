using System;
using System.Collections.Generic;
using System.Globalization;
using System.Text;
using Xamarin.Forms;

namespace PhuLongCRM.Converters
{
    public class QueueStatusConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value != null)
            {
                switch ((int)value)
                {
                    case 1:
                        return "Draft";
                    case 2:
                        return "On Hold";
                    case 3:
                        return "Won";
                    case 4:
                        return "Canceled";
                    case 5:
                        return "Out-Sold";
                    case 100000000:
                        return "Queuing";
                    case 100000002:
                        return "Waiting";
                    case 100000003:
                        return "Expired";
                    case 100000004:
                        return "Completed";
                    case 100000008:
                        return "Đề nghị hủy";
                    case 100000009:
                        return "Hủy giữ chỗ nhưng chưa hoàn tiền";
                    case 100000010:
                        return "Hủy giữ chỗ đã hoàn tiền";
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
