using System;
using System.Collections.Generic;
using System.Linq;
using PhuLongCRM.Resources;

namespace PhuLongCRM.Models
{
    public class TypeIdCardData
    {
        public static List<OptionSet> TypeIdCards()
        {
            return new List<OptionSet>()
            {
                new OptionSet("100000000",Language.cmnd),
                new OptionSet("100000001",Language.cccd),
                new OptionSet("100000002",Language.ho_chieu),
            };
        }

        public static OptionSet GetTypeIdCardById(string Id)
        {
            return TypeIdCards().SingleOrDefault(x => x.Val == Id);
        }
    }
}
