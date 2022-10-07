using PhuLongCRM.Resources;
using System;
using System.Collections.Generic;
using System.Linq;

namespace PhuLongCRM.Models
{
    public class CollectionTypeData
    {
        public static List<OptionSet> GetColectionData()
        {
            return new List<OptionSet>()
            {
                new OptionSet("100000000",Language.meet_acceptance_notices_type),
                new OptionSet("100000003",Language.meet_units_handover_type),
                new OptionSet("100000001",Language.meet_pink_book_handover_type),
                new OptionSet("100000004",Language.meet_end_of_contract_type),
                new OptionSet("100000002",Language.meet_termination_type),
            };
        }
        public static OptionSet GetCollectionTypeById(string id)
        {
            return GetColectionData().SingleOrDefault(x => x.Val == id);
        }
    }
}
