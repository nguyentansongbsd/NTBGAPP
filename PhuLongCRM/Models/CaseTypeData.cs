using System;
using System.Collections.Generic;
using System.Linq;
using PhuLongCRM.Resources;

namespace PhuLongCRM.Models
{
    public class CaseTypeData
    {
        public static List<OptionSet> CasesData()
        {
            return new List<OptionSet>()
            {
                new OptionSet("1",Language.cau_hoi),
                new OptionSet("2",Language.van_de),
                new OptionSet("3",Language.yeu_cau),
                new OptionSet("0",""),
            };
        }

        public static OptionSet GetCaseById(string id)
        {
            return CasesData().SingleOrDefault(x => x.Val == id);
        }
    }
}