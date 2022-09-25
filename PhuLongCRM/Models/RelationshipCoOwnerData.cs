using System;
using System.Collections.Generic;
using System.Linq;
using PhuLongCRM.Resources;

namespace PhuLongCRM.Models
{
    public class RelationshipCoOwnerData
    {
        public static List<OptionSet> RelationshipData()
        {
            return new List<OptionSet>()
            {
                new OptionSet("100000000",Language.vo_chong),
                new OptionSet("100000001",Language.con),
                new OptionSet("100000002",Language.cha_me),
                new OptionSet("100000003",Language.ban),
                new OptionSet("100000004",Language.khac),
            };
        }
        public static OptionSet GetRelationshipById(string id)
        {
            return RelationshipData().SingleOrDefault(x => x.Val == id);
        }
    }
}
