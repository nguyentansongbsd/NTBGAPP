using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using PhuLongCRM.Resources;

namespace PhuLongCRM.Models
{
    public class CaseStatusCodeData
    {
        public static List<OptionSet> CaseStatusData()
        {
            return new List<OptionSet>()
            {
                new OptionSet("1", Language.dang_xu_ly),
                new OptionSet("2", Language.dang_cho),
                new OptionSet("3", Language.dang_cho_thong_tin_chi_tiet),
                new OptionSet("4", Language.nghien_cuu),
                new OptionSet("5", Language.van_de_da_duoc_giai_quyet),
                new OptionSet("1000", Language.cung_cap_thong_tin),
                new OptionSet("6", Language.da_huy),
                new OptionSet("2000", Language.hop_nhat),
                new OptionSet("0", "")
            };
        }

        public static OptionSet GetCaseStatusCodeById(string id)
        {
            return CaseStatusData().SingleOrDefault(x => x.Val == id);
        }

        public static List<OptionSet> GetCaseStatusCodeByIds(List<string> ids)
        {
            List<OptionSet> data = new List<OptionSet>();
            foreach (var item in ids)
            {
                data.Add(CaseStatusData().SingleOrDefault(x => x.Val == item));
            }
            return data;
        }
    }
}
