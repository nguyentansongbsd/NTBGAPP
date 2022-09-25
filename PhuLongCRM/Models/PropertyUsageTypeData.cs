using System;
using System.Collections.Generic;
using System.Linq;
using PhuLongCRM.Resources;

namespace PhuLongCRM.Models
{
    public class PropertyUsageTypeData
    {
        public static OptionSet GetPropertyUsageTypeById(string Id)
        {
            return PropertyUsageTypes().SingleOrDefault(x => x.Val == Id);
        }
        public static List<OptionSet> PropertyUsageTypes()
        {
            return new List<OptionSet>()
            {
                new OptionSet("100000000",Language.can_ho_chung_cu),
                new OptionSet("100000001",Language.nha_pho),
                new OptionSet("100000002",Language.dat_nen),
                new OptionSet("100000003",Language.nghi_duong),
                new OptionSet("100000004",Language.shophouse_va_officetel),
                new OptionSet("100000005",Language.xuong_kho),
            };
        }
    }
}
