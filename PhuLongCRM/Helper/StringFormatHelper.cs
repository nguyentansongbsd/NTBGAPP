using PhuLongCRM.Settings;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Text;
using System.Text.RegularExpressions;

namespace PhuLongCRM.Helper
{
    public class StringFormatHelper
    {
        public static string FormatCurrency(decimal? input)
        {
            if (input.HasValue)
            {
                if (input.Value == 0)
                    return "0";
                else if (UserLogged.Language == "en")
                    return string.Format("{0:#,##0.##}", input.Value); // luôn có 2 số thập phân 0.00 thay ## nếu k cần
                else
                    return String.Format(new CultureInfo("vi-VN"), "{0:#,##0.##}", input.Value);
            }
            return null;
        }
        public static string FormatPercent(decimal? input)
        {
            if (input.HasValue)
            {
                if (input.Value == 0)
                    return "0";
                else if (UserLogged.Language == "en")
                    return string.Format("{0:#,##0.00}", input.Value); // luôn có 2 số thập phân 0.00 thay ## nếu k cần
                else
                    return String.Format(new CultureInfo("vi-VN"), "{0:#,##0.00}", input.Value);
            }
            return null;
        }
        public static bool CheckValueID(string input, int? charNumber)
        {
            if (!string.IsNullOrWhiteSpace(input))
            {
                Regex regex = new Regex(@"[^0-9|a-z|A-Z]");
                Match match = regex.Match(input);
                if (!match.Success)
                {
                    if (charNumber.HasValue)
                        if (input.Trim().Length != charNumber)
                            return false;
                        else
                            return true;
                    else
                        if (input.Contains(" "))
                        return false;
                    else
                        return true;
                }
                else
                    return false;
            }
            return false;
        }
        public static string ReplaceNameContact(string name)
        {
            if(!string.IsNullOrWhiteSpace(name))
            {
                return name.Replace("#", "-")
                    .Replace("%", "-")
                    .Replace("&", "-")
                    .Replace("*", "-")
                    .Replace("~", "-")
                    .Replace("+", "-")
                    .Replace("{", "-")
                    .Replace("}", "-")
                    .Replace("|", "-")
                    .Replace(":", "-")
                    .Replace("<", "-")
                    .Replace(".", "-")
                    .Replace(">", "-")
                    .Replace("/", "-")
                    .Replace("?", "-")
                    .Replace(@"\","-")
                    .Replace('"', '-');
            }
            return null;
        }
    }
}
