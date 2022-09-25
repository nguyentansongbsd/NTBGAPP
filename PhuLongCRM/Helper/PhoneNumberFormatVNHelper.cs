using System;
using System.Collections.Generic;
using System.Text;
using System.Text.RegularExpressions;

namespace PhuLongCRM.Helper
{
    public static class PhoneNumberFormatVNHelper
    {
        //kiểm tra số điện thoại di động theo định dạng sdt VN
        public static bool CheckValidate(string phone)
        {
            if (string.IsNullOrWhiteSpace(phone))
                return false;
            string strRegex = "^0+[^1246]+\\d{8}";
            Regex re = new Regex(strRegex);
            if (re.IsMatch(phone))
                return true;
            else
                return false;
        }
    }
}
