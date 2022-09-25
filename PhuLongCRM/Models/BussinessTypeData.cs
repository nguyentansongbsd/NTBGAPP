using System;
using System.Collections.Generic;
using System.Linq;
using PhuLongCRM.Resources;

namespace PhuLongCRM.Models
{
    public class BussinessTypeData
    {
        public static List<OptionSet> BussinessTypes()
        {
            return new List<OptionSet>()
            {
                new OptionSet("100000000",Language.khach_hang),
                new OptionSet("100000001",Language.doi_tac),
                new OptionSet("100000002",Language.dai_ly),
                new OptionSet("100000003",Language.chu_dau_tu),
            };
        }
        public static OptionSet GetBussinessTypeById(string Id)
        {
            return BussinessTypes().SingleOrDefault(x => x.Val == Id);
        }
    }
}
