using PhuLongCRM.Resources;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace PhuLongCRM.Models
{
    public class AcceptanceType
    {
        public static List<OptionSet> AcceptanceTypeData()
        {
            return new List<OptionSet>()
            {
                new OptionSet("100000000",Language.acceptance_internal_type),
                new OptionSet("100000001",Language.acceptance_external_type),
            };
        }

        public static OptionSet GetAcceptanceTypeById(string Id)
        {
            return AcceptanceTypeData().SingleOrDefault(x => x.Val == Id);
        }
    }
}
