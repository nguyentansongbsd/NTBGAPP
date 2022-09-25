using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using PhuLongCRM.Resources;

namespace PhuLongCRM.Models
{
    public class BoolToStringData
    {       
        public static string GetStringByBool(bool _bool)
        {
            if(_bool)
            {
                return Language.co;
            }   
            else
            {
                return Language.khong;
            }    
        }
    }
}
