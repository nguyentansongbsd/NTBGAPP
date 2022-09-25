using System;
using System.Collections.Generic;
using System.Globalization;
using System.Text;
using PhuLongCRM.Resources;
using Xamarin.Forms;

namespace PhuLongCRM.Converters
{
    public class MandatorySecondaryStatusCodeConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value != null)
            {
                switch ((int)value)
                {
                    case 1: 
                        return Language.nhap_mandatory_sts; // Active
                    case 100000000: 
                        return Language.dang_ap_dung_mandatory_sts; // Applying
                    case 100000001: 
                        return Language.huy_mandatory_sts; //Cancel
                    case 2: 
                        return Language.vo_hieu_luc_mandatory_sts; // Inactive 
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
