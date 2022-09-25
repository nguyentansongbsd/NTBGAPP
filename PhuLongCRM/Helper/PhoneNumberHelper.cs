using System;
namespace PhuLongCRM.Helper
{
    public static class PhoneNumberHelper
    {
        public static string formatPhoneNumber(string phoneNumber)
        {
            var isNumber = long.TryParse(phoneNumber.Replace(" ","").Replace("(","").Replace(")","").Replace("-","").Replace("+","").Replace(",",""), out long n);
            if (isNumber)
            {
                string phone = phoneNumber.Replace(" ", "").Replace("(", "").Replace(")", "").Replace("-", "").Replace("+", "").Replace(",", "");
                if(phone.ToCharArray()[0] == '0') { phone = "84" + phone.Remove(0, 1); }

                return phone;
            }
            else
            {
                return phoneNumber;
            }
        }
    }
}
