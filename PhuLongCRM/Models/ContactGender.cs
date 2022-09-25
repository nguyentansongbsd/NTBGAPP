using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using PhuLongCRM.Resources;

namespace PhuLongCRM.Models
{
    public class ContactGender
    {
        public static List<OptionSet> GenderData()
        {
            return new List<OptionSet>()
            {
                new OptionSet("1",Language.nam),
                new OptionSet("2",Language.nu),
                new OptionSet("100000000",Language.khac)
            };
        }
        public static OptionSet GetGenderById(string Id)
        {
            return GenderData().SingleOrDefault(x => x.Val == Id);
        }
    }
}
