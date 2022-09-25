using System;
using System.Collections.Generic;
using System.Linq;
using PhuLongCRM.Resources;

namespace PhuLongCRM.Models
{
    public class PaymentSchemeTypeData
    {
        public static List<OptionSet> PaymentSchemeTypes()
        {
            return new List<OptionSet>()
            {
                new OptionSet("100000000",Language.giu_nguyen),
                new OptionSet("100000001",Language.gop_dau),
                new OptionSet("100000002",Language.gop_cuoi),
            };
        }
        public static OptionSet GetPaymentSchemeTypeById(string Id)
        {
            return PaymentSchemeTypes().SingleOrDefault(x => x.Val == Id);
        }
    }
}
