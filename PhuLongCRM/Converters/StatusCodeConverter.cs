using PhuLongCRM.Resources;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Text;
using Xamarin.Forms;

namespace PhuLongCRM.Converters
{
    public class StatusCodeConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if ((string)value == "1")
            {
                return Language.new_sts;//"Mới"; // New
            }
            else if ((string)value == "2")
            {
                return Language.da_lien_he_sts;//"Đã Liên Hệ"; // Contacted
            }
            else if ((string)value == "3")
            {
                return Language.da_xac_nhan_sts;//"Đã Xác Nhận";
            }
            else if ((string)value == "4")
            {
                return Language.mat_khach_hang;//"Mất Khách Hàng"; // Lost
            }
            else if ((string)value == "5")
            {
                return Language.khong_lien_lac_duoc;//"Không Liên Hệ Được"; // Cannot Contact
            }
            else if ((string)value == "6")
            {
                return Language.khong_quan_tam;//"Không Quan Tâm";  //No Longer Interested
            }
            else if ((string)value == "7")
            {
                return Language.da_huy;// "Đã hủy"; // Canceled
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
