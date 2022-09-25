using System;
using System.Collections.Generic;
using System.Text;

namespace PhuLongCRM.Helper
{
    public class StringHelper
    {
        public static string DecimalToCurrencyText(decimal? input)
        {
            if (input.HasValue)
            {
                if (input.Value == 0)
                    return null;
                else
                    //return String.Format("{0:0,0.00 đ}", input.Value);  co 2 số 0 sau dau chấm.
                    return String.Format("{0:#,0 đ}", input.Value);
            }
            return "";
        }

        public static string DateFormat(DateTime? date)
        {
            if (date.HasValue && date.Value > DateTime.MinValue)
            {
                return date.Value.ToString("dd/MM/yyyy");
            }
            return "";
        }

        public static string DecimalToPercentFormat(decimal? input)
        {
            if (input.HasValue)
            {
                if (input.Value == 0) return "0 %";
                return String.Format("{0:0,0.00}", input.Value) + "%";
            }
            return "";
        }
    }

}
