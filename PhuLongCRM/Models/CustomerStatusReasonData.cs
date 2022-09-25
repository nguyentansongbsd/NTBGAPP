using System;
using System.Collections.Generic;
using System.Linq;
using PhuLongCRM.Resources;

namespace PhuLongCRM.Models
{
    public class CustomerStatusReasonData
    {
        public static List<OptionSet> CustomerStatusReasons()
        {
            return new List<OptionSet>()
            {
                new OptionSet("1",Language.tiem_nang_sts),
                new OptionSet("100000000",Language.chinh_thuc),
            };
        }

        public static OptionSet GetCustomerStatusReasonById(string Id)
        {
            return CustomerStatusReasons().SingleOrDefault(x => x.Val == Id);
        }
    }
}
