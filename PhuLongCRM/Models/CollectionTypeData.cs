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
                new OptionSet("100000000","Acceptance Notices"),
                new OptionSet("100000003","Units- Handover"),
                new OptionSet("100000001","Pink-book Handover"),
                new OptionSet("100000004","End of contract"),
                new OptionSet("100000002","Termination"),
            };
        }
        public static OptionSet GetCollectionTypeById(string id)
        {
            return GetColectionData().SingleOrDefault(x => x.Val == id);
        }
    }
}
